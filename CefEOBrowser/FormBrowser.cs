using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;
using BrowserLib;
using System.Runtime.InteropServices;

namespace CefEOBrowser
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public partial class FormBrowser : Form, BrowserLib.IBrowser
    {
        public ChromiumWebBrowser chromeBrowser;
        private string cef_path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"CefEOBrowser");

        private void InitializeChromium()
        {
            CefLibraryHandle libraryLoader = new CefLibraryHandle(Path.Combine(cef_path, @"bin\libcef.dll"));
            CefSettings settings = new CefSettings();
            settings.CachePath = Path.Combine(cef_path, @"cache");
            settings.UserDataPath = Path.Combine(cef_path, @"userdata");
            settings.ResourcesDirPath = Path.Combine(cef_path, @"bin");
            settings.LocalesDirPath = Path.Combine(cef_path, @"bin\locales");
            settings.BrowserSubprocessPath = Path.Combine(cef_path, @"bin\CefSharp.BrowserSubprocess.exe");
            settings.LogSeverity = LogSeverity.Disable;
            Cef.Initialize(settings, performDependencyCheck: false, browserProcessHandler: null);

            chromeBrowser = new ChromiumWebBrowser("https://github.com/cefsharp/CefSharp");
            this.Controls.Add(chromeBrowser);
            chromeBrowser.Dock = DockStyle.Fill;
            libraryLoader.Dispose();
        }

        // FormBrowserHostの通信サーバ
        private string ServerUri;

        // FormBrowserの通信サーバ
        private PipeCommunicator<BrowserLib.IBrowserHost> BrowserHost;

        private BrowserLib.BrowserConfiguration Configuration;

        // 親プロセスが生きているか定期的に確認するためのタイマー
        private Timer HeartbeatTimer = new Timer();
        private IntPtr HostWindow;

        public FormBrowser(string serverUri)
        {
            ServerUri = serverUri;
            InitializeComponent();
            InitializeChromium();
        }

        private void FormBrowser_FormClosing(object sender, FormClosingEventArgs e)
        {
            Cef.Shutdown();
        }

        public void ConfigurationChanged(BrowserConfiguration conf)
        {
            Configuration = conf;
        }

        public void InitialAPIReceived()
        {
            throw new NotImplementedException();
        }

        public void SaveScreenShot()
        {
            throw new NotImplementedException();
        }

        public void RefreshBrowser()
        {
            throw new NotImplementedException();
        }

        public void ApplyZoom()
        {
            throw new NotImplementedException();
        }

        public void Navigate(string url)
        {
            throw new NotImplementedException();
        }

        public void SetProxy(string proxy)
        {
            throw new NotImplementedException();
        }

        public void ApplyStyleSheet()
        {
            throw new NotImplementedException();
        }

        public void DestroyDMMreloadDialog()
        {
            throw new NotImplementedException();
        }

        public void CloseBrowser()
        {
            throw new NotImplementedException();
        }

        public void SetIconResource(byte[] canvas)
        {
            throw new NotImplementedException();
        }

        [DllImport("user32.dll", EntryPoint = "SetWindowLongA", SetLastError = true)]
        private static extern uint SetWindowLong(IntPtr hwnd, int nIndex, uint dwNewLong);

        private void FormBrowser_Load(object sender, EventArgs e)
        {
            SetWindowLong(this.Handle, (-16), 0x40000000); // GWL_STYLE = (-16), WS_CHILD = 0x40000000

            // ホストプロセスに接続
            BrowserHost = new PipeCommunicator<BrowserLib.IBrowserHost>(
                this, typeof(BrowserLib.IBrowser), ServerUri + "Browser", "Browser");
            BrowserHost.Connect(ServerUri + "/BrowserHost");
            BrowserHost.Faulted += BrowserHostChannel_Faulted;


            ConfigurationChanged(BrowserHost.Proxy.Configuration);


            // ウィンドウの親子設定＆ホストプロセスから接続してもらう
            BrowserHost.Proxy.ConnectToBrowser(this.Handle);

            // 親ウィンドウが生きているか確認 
            HeartbeatTimer.Tick += (EventHandler)((sender2, e2) =>
            {
                BrowserHost.AsyncRemoteRun(() => { HostWindow = BrowserHost.Proxy.HWND; });
            });
            HeartbeatTimer.Interval = 2000; // 2秒ごと　
            HeartbeatTimer.Start();


            BrowserHost.AsyncRemoteRun(() => BrowserHost.Proxy.GetIconResource());
        }

        private void BrowserHostChannel_Faulted(Exception e)
        {
            throw new NotImplementedException();
        }
    }
}
