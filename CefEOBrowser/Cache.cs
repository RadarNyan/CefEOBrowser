using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CefSharp;

namespace CefEOBrowser
{
	public class CacheProtocolSchemeHandler : ResourceHandler
	{
		FormBrowser Browser;

		public CacheProtocolSchemeHandler(FormBrowser form)
		{
			Browser = form;
		}

		public override bool ProcessRequestAsync(IRequest request, ICallback callback)
		{
			Task.Run(() => {
				using (callback) {
					Stream stream = null;

					var remoteUri = new Uri(request.Url.Replace("kcs2cache://", "http://"));
					var remoteFilePath = remoteUri.AbsolutePath;
					var extension = Path.GetExtension(remoteFilePath);
					var localCachePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"CefEOBrowser\kcs2cache");
					var localFilePath = Path.Combine(localCachePath, remoteFilePath.Substring(1).Replace('/', Path.DirectorySeparatorChar));
					var localFileDirectory = Path.GetDirectoryName(localFilePath);
					if (remoteUri.Query != ""
						&& !remoteFilePath.StartsWith("/kcs2/resources/world/")
						&& !remoteFilePath.StartsWith("/kcs2/resources/area/")) { // Attach Version Info
						localFilePath = Path.Combine(localFileDirectory, $"{Path.GetFileNameWithoutExtension(localFilePath)}__{remoteUri.Query.Substring(1)}{extension}");
					}
					// Local Cacher
					var hackFilePath = Path.Combine(localFileDirectory, $"{Path.GetFileNameWithoutExtension(localFilePath)}.hack{extension}");
					if (File.Exists(hackFilePath)) {
						stream = new MemoryStream(File.ReadAllBytes(hackFilePath));
					} else if (File.Exists(localFilePath)) {
						stream = new MemoryStream(File.ReadAllBytes(localFilePath));
					} else {
						byte[] bytes = null;
						//
						bool download = true;
						int retries = 0;
						int sleep = 0;
						string retry_reason = "";
						//
						// string _Content_Type;
						string _Content_Length = "";
						string _Last_Modified = "";
						// Proxy
						string[] p;
						string[] proxies = Browser.BrowserProxy.Split(';');
						if (proxies.Length > 1) {
							// https=127.0.0.1:8080
							p = proxies[1].Substring(6).Split(':');
						} else {
							// http=127.0.0.1:8080
							p = proxies[0].Substring(5).Split(':');
						}
						using (var webClient = new WebClient() {
							Proxy = new WebProxy(p[0], int.Parse(p[1]))
							//Proxy = new WebProxy("127.0.0.1", 8888)
						}) {
							while (download) {
								try {
									// Headers (Must be set at every request or it gets cleared)
									webClient.Headers[HttpRequestHeader.UserAgent] = Browser.BrowserUA;
									// "Accept" header breaks things, better leave it alone
									//webClient.Headers["Accept"] = @"text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8";
									//webClient.Headers["Accept-Encoding"] = @"gzip, deflate";
									//webClient.Headers["Accept-Language"] = @"ja,en-US;q=0.9,en;q=0.8";
									//Browser.AddLog(2, $"Loading Resource {remoteFilePath} w/ UA: {webClient.Headers[HttpRequestHeader.UserAgent]}");

									// Download File
									bytes = webClient.DownloadData(remoteUri);

									// Check downloaded data is complete
									_Content_Length = webClient.ResponseHeaders["Content-Length"];
									if (string.IsNullOrEmpty(_Content_Length)) {
										// no way to check, asume it's finished?
										Browser.AddLog(2, $"Can't verify Resource {remoteFilePath} Length (Content-Length not available), assume download is complete.");
										download = false;
									} else if (bytes.Length == int.Parse(_Content_Length)) {
										_Last_Modified = webClient.ResponseHeaders["Last-Modified"];
										download = false;
									} else {
										retry_reason = "Download Imcomplete";
									}
								}
								catch (WebException we) {
									retry_reason = we.ToString();
									download = true;
									retries++;
								}
								catch (Exception e) {
									retry_reason = e.ToString();
									download = true;
									retries++;
								}
								if (retries > 5) {
									Browser.AddLog(2, $"Failed to download {remoteFilePath}, cancel request.");
									bytes = null;
									download = false;
									callback.Cancel();
								}
								if (download) {
									if (remoteFilePath.StartsWith("/kcs2/img/")) {
										sleep += 10000;
										Browser.AddLog(2, $"Loading Resource {remoteFilePath} Failed ({retry_reason}), retry in {(int)(sleep / 1000)} seconds.");
										System.Threading.Thread.Sleep(sleep);
									} else {
										sleep += 5000;
										Browser.AddLog(2, $"Loading Resource {remoteFilePath} Failed, ({retry_reason}) retry in {(int)(sleep / 1000)} seconds.");
										System.Threading.Thread.Sleep(sleep);
									}
								}
							}
						}
						if (bytes != null) {
							stream = new MemoryStream(bytes);
							// Save File
							Directory.CreateDirectory(localFileDirectory);
							File.WriteAllBytes(localFilePath, bytes);
							if (!string.IsNullOrEmpty(_Last_Modified)) {
								File.SetLastWriteTime(localFilePath, DateTime.Parse(_Last_Modified));
							}
						}
					}

					if (stream == null) {
						callback.Cancel();
					} else {
						stream.Position = 0;
						ResponseLength = stream.Length;
						MimeType = GetMimeType(extension);
						StatusCode = (int)HttpStatusCode.OK;
						Stream = stream;

						callback.Continue();
					}
				}
			});
			return true;
		}
	}

	public class CacheProtocolSchemeHandlerFactory : ISchemeHandlerFactory
	{
		FormBrowser Browser;

		public CacheProtocolSchemeHandlerFactory(FormBrowser form)
		{
			Browser = form;
		}

		public IResourceHandler Create(IBrowser browser, IFrame frame, string schemeName, IRequest request)
		{
			return new CacheProtocolSchemeHandler(Browser);
		}
	}
}
