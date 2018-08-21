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
using System.Drawing.Imaging;

namespace CefEOBrowser
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public partial class FormBrowser : Form, BrowserLib.IBrowser
    {
        public ChromiumWebBrowser Browser;
        private string cef_path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"CefEOBrowser");
        private bool Cef_started = false;

        private void InitializeChromium(string proxy, string url)
        {
            CefLibraryHandle libraryLoader = new CefLibraryHandle(Path.Combine(cef_path, @"bin\libcef.dll"));

            CefSettings settings = new CefSettings();
            settings.CachePath = Path.Combine(cef_path, @"cache");
            settings.UserDataPath = Path.Combine(cef_path, @"userdata");
            settings.ResourcesDirPath = Path.Combine(cef_path, @"bin");
            settings.LocalesDirPath = Path.Combine(cef_path, @"bin\locales");
            settings.BrowserSubprocessPath = Path.Combine(cef_path, @"bin\CefSharp.BrowserSubprocess.exe");
            settings.LogSeverity = LogSeverity.Disable;
            settings.CefCommandLineArgs.Add("proxy-server", proxy);
            // settings.DisableGpuAcceleration();
            Cef.Initialize(settings, performDependencyCheck: false, browserProcessHandler: null);

            Browser = new ChromiumWebBrowser(url)
            {
                FocusHandler = null,
                LifeSpanHandler = new BrowserLifeSpanHandler(),
                MenuHandler = new BrowserMenuHandler(),
                KeyboardHandler = new BrowserKeyboardHandler()
            };

            Browser.AddressChanged += Browser_AddressChanged;
            Browser.FrameLoadStart += Browser_FrameLoadEnd;

            this.SizeAdjuster.Controls.Add(Browser);
            Browser.Dock = DockStyle.Fill;
            Cef_started = true;

            libraryLoader.Dispose();
        }

        private void Browser_FrameLoadEnd(object sender, FrameLoadStartEventArgs e)
        {
            if (!GamePageLoaded)
                return;

            if (e.Frame.Name == "game_frame")
                ApplyStyleSheet();
        }

        private void Browser_AddressChanged(object sender, AddressChangedEventArgs e)
        {
            if (e.Address == KanColleUrl)
                GamePageLoaded = true;
        }

        private readonly Size KanColleSize = new Size(1200, 720);

        private readonly string StyleClassID = Guid.NewGuid().ToString().Substring(0, 8);

        private readonly string KanColleUrl = "http://www.dmm.com/netgame/social/-/gadgets/=/app_id=854854/";

        // FormBrowserHostの通信サーバ
        private string ServerUri;

        // FormBrowserの通信サーバ
        private PipeCommunicator<BrowserLib.IBrowserHost> BrowserHost;

        private BrowserLib.BrowserConfiguration Configuration;

        // 親プロセスが生きているか定期的に確認するためのタイマー
        private Timer HeartbeatTimer = new Timer();
        private IntPtr HostWindow;

        /// <summary>
        /// スタイルシートの変更が適用されているか
        /// </summary>
        private bool StyleSheetApplied;

        private bool GamePageLoaded;

        private void SizeAdjuster_SizeChanged(object p, EventArgs eventArgs)
        {
            if (!StyleSheetApplied)
            {
                Browser.Location = new Point(0, 0);
                Browser.Size = SizeAdjuster.Size;
                return;
            }

            ApplyZoom();
        }

        /// <summary>
        /// 艦これが読み込まれているかどうか
        /// </summary>
        private bool IsKanColleLoaded { get; set; }

        private VolumeManager _volumeManager;

        private string _lastScreenShotPath;

        private NumericUpDown ToolMenu_Other_Volume_VolumeControl
        {
            get { return (NumericUpDown)((ToolStripControlHost)ToolMenu_Other_Volume.DropDownItems["ToolMenu_Other_Volume_VolumeControlHost"]).Control; }
        }

        private PictureBox ToolMenu_Other_LastScreenShot_Control
        {
            get { return (PictureBox)((ToolStripControlHost)ToolMenu_Other_LastScreenShot.DropDownItems["ToolMenu_Other_LastScreenShot_ImageHost"]).Control; }
        }

        public FormBrowser(string serverUri)
        {
            ServerUri = serverUri;

            InitializeComponent();
            this.ToolMenu.Renderer = new ToolStripOverride(); // remove stupid rounded corner

            _volumeManager = new VolumeManager((uint)System.Diagnostics.Process.GetCurrentProcess().Id);

            // 音量設定用コントロールの追加
            {
                var control = new NumericUpDown();
                control.Name = "ToolMenu_Other_Volume_VolumeControl";
                control.Maximum = 100;
                control.TextAlign = HorizontalAlignment.Right;
                control.Font = ToolMenu_Other_Volume.Font;

                control.ValueChanged += ToolMenu_Other_Volume_ValueChanged;
                control.Tag = false;

                var host = new ToolStripControlHost(control, "ToolMenu_Other_Volume_VolumeControlHost");

                control.Size = new Size(host.Width - control.Margin.Horizontal, host.Height - control.Margin.Vertical);
                control.Location = new Point(control.Margin.Left, control.Margin.Top);


                ToolMenu_Other_Volume.DropDownItems.Add(host);
            }

            // スクリーンショットプレビューコントロールの追加
            {
                double zoomrate = 0.25;
                var control = new PictureBox();
                control.Name = "ToolMenu_Other_LastScreenShot_Image";
                control.SizeMode = PictureBoxSizeMode.Zoom;
                control.Size = new Size((int)(KanColleSize.Width * zoomrate), (int)(KanColleSize.Height * zoomrate));
                control.Margin = new Padding();
                control.Image = new Bitmap((int)(KanColleSize.Width * zoomrate), (int)(KanColleSize.Height * zoomrate), PixelFormat.Format24bppRgb);
                using (var g = Graphics.FromImage(control.Image))
                {
                    g.Clear(SystemColors.Control);
                    g.DrawString("スクリーンショットをまだ撮影していません。\r\n", Font, Brushes.Black, new Point(4, 4));
                }

                var host = new ToolStripControlHost(control, "ToolMenu_Other_LastScreenShot_ImageHost");

                host.Size = new Size(control.Width + control.Margin.Horizontal, control.Height + control.Margin.Vertical);
                host.AutoSize = false;
                control.Location = new Point(control.Margin.Left, control.Margin.Top);

                host.Click += ToolMenu_Other_LastScreenShot_ImageHost_Click;

                ToolMenu_Other_LastScreenShot.DropDownItems.Insert(0, host);
            }
        }

        private void ToolMenu_Other_LastScreenShot_ImageHost_Click(object sender, EventArgs e)
        {
            if (_lastScreenShotPath != null && System.IO.File.Exists(_lastScreenShotPath))
                System.Diagnostics.Process.Start(_lastScreenShotPath);
        }

        private void ToolMenu_Other_Volume_ValueChanged(object sender, EventArgs e)
        {
            var control = ToolMenu_Other_Volume_VolumeControl;

            try
            {
                if ((bool)control.Tag)
                    _volumeManager.Volume = (float)(control.Value / 100);
                control.BackColor = SystemColors.Window;

            }
            catch (Exception)
            {
                control.BackColor = Color.MistyRose;
            }
        }

        private void FormBrowser_FormClosing(object sender, FormClosingEventArgs e)
        {
            Cef.Shutdown();
        }

        public void ConfigurationChanged(BrowserConfiguration conf)
        {
            Configuration = conf;

            SizeAdjuster.AutoScroll = Configuration.IsScrollable;
            //ToolMenu_Other_Zoom_Fit.Checked = Configuration.ZoomFit;
            //ApplyZoom();
            ToolMenu_Other_AppliesStyleSheet.Checked = Configuration.AppliesStyleSheet;
            ToolMenu.Dock = (DockStyle)Configuration.ToolMenuDockStyle;
            ToolMenu.Visible = Configuration.IsToolMenuVisible;

            //SizeAdjuster.BackColor = System.Drawing.Color.FromArgb(255,29,31,33);
            //ToolMenu.BackColor = System.Drawing.Color.FromArgb(255, 29, 31, 33);
        }

        public void InitialAPIReceived()
        {
            IsKanColleLoaded = true;

            // DestroyDMMreloadDialog();

            //起動直後はまだ音声が鳴っていないのでミュートできないため、この時点で有効化
            SetVolumeState();
        }

        public void SaveScreenShot()
        {
            int savemode = Configuration.ScreenShotSaveMode;
            int format = Configuration.ScreenShotFormat;
            string folderPath = Configuration.ScreenShotPath;
            bool is32bpp = format != 1 && Configuration.AvoidTwitterDeterioration;

            using (var image = TakeScreenShot(is32bpp))
            {

                if (image == null)
                    return;

                // to file
                if ((savemode & 1) != 0)
                {
                    try
                    {

                        if (!System.IO.Directory.Exists(folderPath))
                        {
                            System.IO.Directory.CreateDirectory(folderPath);
                        }

                        string ext;
                        System.Drawing.Imaging.ImageFormat imgFormat;

                        switch (format)
                        {
                            case 1:
                                ext = "jpg";
                                imgFormat = System.Drawing.Imaging.ImageFormat.Jpeg;
                                break;
                            case 2:
                            default:
                                ext = "png";
                                imgFormat = System.Drawing.Imaging.ImageFormat.Png;
                                break;
                        }

                        string path = string.Format("{0}\\{1:yyyyMMdd_HHmmssff}.{2}", folderPath, DateTime.Now, ext);
                        image.Save(path, imgFormat);
                        _lastScreenShotPath = path;

                        AddLog(2, string.Format("スクリーンショットを {0} に保存しました。", path));

                    }
                    catch (Exception ex)
                    {

                        SendErrorReport(ex.ToString(), "スクリーンショットの保存に失敗しました。");
                    }
                }


                // to clipboard
                if ((savemode & 2) != 0)
                {
                    try
                    {

                        Clipboard.SetImage(image);

                        if ((savemode & 3) != 3)
                            AddLog(2, "スクリーンショットをクリップボードにコピーしました。");

                    }
                    catch (Exception ex)
                    {

                        SendErrorReport(ex.ToString(), "スクリーンショットのクリップボードへのコピーに失敗しました。");
                    }
                }
            }
        }



        private Bitmap TakeScreenShot(bool is32bpp)
        {
            if (!StyleSheetApplied)
            {
                MessageBox.Show("スタイルシート適用しないどスクリーンショットできません。\r\nスタイルシートを適用してください。", "CefEOBrowser");
                return null;
            }

            var screenshot = PointToScreen(Browser.Location);
            if (ToolMenu.Visible)
            {
                switch (ToolMenu.Dock)
                {
                    case DockStyle.Left:
                        screenshot.X += ToolMenu.Width;
                        break;
                    case DockStyle.Top:
                        screenshot.Y += ToolMenu.Height;
                        break;
                }
            }

            var image = new Bitmap(Browser.Width, Browser.Height, PixelFormat.Format24bppRgb);
            using (var g = Graphics.FromImage(image))
            {
                g.CopyFromScreen(screenshot.X, screenshot.Y, 0, 0, image.Size);
            }

            return image;
        }

        public void RefreshBrowser()
        {
            GamePageLoaded = false;
            StyleSheetApplied = false;
            if (Browser.Address != KanColleUrl && Browser.Address != Configuration.LogInPageURL)
                Browser.Dock = DockStyle.Fill;
            Browser.Reload();
        }

        public void ApplyZoom()
        {
            int zoomRate = Configuration.ZoomRate;
            bool fit = Configuration.ZoomFit && StyleSheetApplied;
            double zoomFactor;

            try
            {
                zoomFactor = zoomRate / 100.0;
                if (zoomRate == 66)
                    zoomFactor = 2 / 3.0;
                double zoomLevel = Math.Log(zoomFactor, 1.2);
                Browser.SetZoomLevel(zoomLevel);
                

                if (StyleSheetApplied)
                {
                    Browser.Size = new Size(
                        (int)(KanColleSize.Width * zoomFactor),
                        (int)(KanColleSize.Height * zoomFactor)
                        );
                    CenteringBrowser();
                }

                if (fit)
                {
                    ToolMenu_Other_Zoom_Current.Text = string.Format("現在: ぴったり");
                }
                else
                {
                    ToolMenu_Other_Zoom_Current.Text = string.Format("現在: {0}%", zoomRate);
                }


            }
            catch (Exception ex)
            {
                AddLog(3, "ズームの適用に失敗しました。" + ex.Message);
            }
        }

        private void CenteringBrowser()
        {
            SizeAdjuster.SuspendLayout();
            int x = Browser.Location.X, y = Browser.Location.Y;
            bool isScrollable = Configuration.IsScrollable;
            Browser.Dock = DockStyle.None;
            if (!isScrollable || Browser.Width <= SizeAdjuster.Width)
            {
                x = (SizeAdjuster.Width - Browser.Width) / 2;
            }
            if (!isScrollable || Browser.Height <= SizeAdjuster.Height)
            {
                y = (SizeAdjuster.Height - Browser.Height) / 2;
            }
            Browser.Anchor = AnchorStyles.None;
            Browser.Location = new Point(x, y);
            SizeAdjuster.ResumeLayout();
        }

        public void Navigate(string url)
        {
            GamePageLoaded = false;
            StyleSheetApplied = false;
            if (url != KanColleUrl && Browser.Address != Configuration.LogInPageURL)
                Browser.Dock = DockStyle.Fill;
            Browser.Load(url);
        }

        public void SetProxy(string proxy)
        {
            ushort port;
            string proxy_cef;
            if(ushort.TryParse(proxy, out port)){
                proxy_cef = "http=127.0.0.1:" + port;
            } else {
                proxy_cef = proxy;
            }

            if (Cef_started)
            {
                MessageBox.Show("実行中のプロキシ設定の変更はサポートされていません。\r\n七四式電子観測儀を再起動してください。", "CefEOBrowser");
                Cef.Shutdown();
            }
            else
            {
                if (Configuration.IsEnabled)
                {
                    InitializeChromium(proxy_cef, Configuration.LogInPageURL);
                }
                else
                {
                    InitializeChromium(proxy_cef, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"CefEOBrowser\html\default.htm"));
                }
            }

            BrowserHost.AsyncRemoteRun(() => BrowserHost.Proxy.SetProxyCompleted());
        }

        public void ApplyStyleSheet()
        {
            if (!StyleSheetApplied && !Configuration.AppliesStyleSheet)
                return;

            try
            {
                if (StyleSheetApplied)
                {
                    var browser = Browser.GetBrowser();
                    bool has_game_frame = false;
                    foreach (var i in browser.GetFrameIdentifiers())
                    {
                        IFrame frame = browser.GetFrame(i);
                        if (frame.Name == "game_frame")
                        {
                            has_game_frame = true;
                            frame.ExecuteJavaScriptAsync(string.Format(Properties.Resources.Restore_JS, StyleClassID));
                            break;
                        }
                    }
                    if (has_game_frame)
                    {
                        browser.MainFrame.ExecuteJavaScriptAsync(string.Format(Properties.Resources.Restore_JS, StyleClassID));
                        StyleSheetApplied = false;
                        Browser.Dock = DockStyle.Fill;
                    }
                }
                else if (!StyleSheetApplied && Configuration.AppliesStyleSheet)
                {
                    var browser = Browser.GetBrowser();
                    bool has_game_frame = false;
                    foreach (var i in browser.GetFrameIdentifiers())
                    {
                        IFrame frame = browser.GetFrame(i);
                        if (frame.Name == "game_frame")
                        {
                            has_game_frame = true;
                            frame.ExecuteJavaScriptAsync(string.Format(Properties.Resources.Frame_JS, StyleClassID));
                            break;
                        }
                    }
                    if (has_game_frame)
                    {
                        browser.MainFrame.ExecuteJavaScriptAsync(string.Format(Properties.Resources.Page_JS, StyleClassID));
                        StyleSheetApplied = true;
                    }
                }

                ApplyZoom();
            }
            catch (Exception ex)
            {
                SendErrorReport(ex.ToString(), "スタイルシートの適用に失敗しました。");
            }
            
        }

        public void DestroyDMMreloadDialog()
        {
            if (!Configuration.IsDMMreloadDialogDestroyable)
                return;

            try
            {
                /*
                var document = Browser.Document;
                if (document == null) return;

                var swf = getFrameElementById(document, "externalswf");
                if (swf == null) return;

                document.InvokeScript("eval", new object[] { Properties.Resources.DMMScript });
                */
            }
            catch (Exception ex)
            {
                SendErrorReport(ex.ToString(), "DMMによるページ更新ダイアログの非表示に失敗しました。");
            }
        }

        public void CloseBrowser()
        {
            HeartbeatTimer.Stop();
            // リモートコールでClose()呼ぶのばヤバそうなので非同期にしておく
            BeginInvoke((Action)(() => Exit()));
        }

        public void SetIconResource(byte[] canvas)
        {
            string[] keys = new string[] {
                "Browser_ScreenShot",
                "Browser_Zoom",
                "Browser_ZoomIn",
                "Browser_ZoomOut",
                "Browser_Unmute",
                "Browser_Mute",
                "Browser_Refresh",
                "Browser_Navigate",
                "Browser_Other",
            };
            int unitsize = 16 * 16 * 4;

            for (int i = 0; i < keys.Length; i++)
            {
                Bitmap bmp = new Bitmap(16, 16, PixelFormat.Format32bppArgb);

                if (canvas != null)
                {
                    BitmapData bmpdata = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
                    Marshal.Copy(canvas, unitsize * i, bmpdata.Scan0, unitsize);
                    bmp.UnlockBits(bmpdata);
                }

                Icons.Images.Add(keys[i], bmp);
            }


            ToolMenu_ScreenShot.Image = ToolMenu_Other_ScreenShot.Image =
                Icons.Images["Browser_ScreenShot"];
            ToolMenu_Zoom.Image = ToolMenu_Other_Zoom.Image =
                Icons.Images["Browser_Zoom"];
            ToolMenu_Other_Zoom_Increment.Image =
                Icons.Images["Browser_ZoomIn"];
            ToolMenu_Other_Zoom_Decrement.Image =
                Icons.Images["Browser_ZoomOut"];
            ToolMenu_Refresh.Image = ToolMenu_Other_Refresh.Image =
                Icons.Images["Browser_Refresh"];
            ToolMenu_NavigateToLogInPage.Image = ToolMenu_Other_NavigateToLogInPage.Image =
                Icons.Images["Browser_Navigate"];
            ToolMenu_Other.Image =
                Icons.Images["Browser_Other"];

            SetVolumeState();
        }

        private void SetVolumeState()
        {
            bool mute;
            float volume;

            try
            {
                mute = _volumeManager.IsMute;
                volume = _volumeManager.Volume * 100;

            }
            catch (Exception)
            {
                // 音量データ取得不能時
                mute = false;
                volume = 100;
            }

            ToolMenu_Mute.Image = ToolMenu_Other_Mute.Image =
                Icons.Images[mute ? "Browser_Mute" : "Browser_Unmute"];

            {
                var control = ToolMenu_Other_Volume_VolumeControl;
                control.Tag = false;
                control.Value = (decimal)volume;
                control.Tag = true;
            }

            Configuration.Volume = volume;
            Configuration.IsMute = mute;
            ConfigurationUpdated();
        }

        private void ConfigurationUpdated()
        {
            BrowserHost.AsyncRemoteRun(() => BrowserHost.Proxy.ConfigurationUpdated(Configuration));
        }

        private void AddLog(int priority, string message)
        {
            //BrowserHost.AsyncRemoteRun(() => BrowserHost.Proxy.AddLog(priority, message));
        }

        private void SendErrorReport(string exceptionName, string message)
        {
            BrowserHost.AsyncRemoteRun(() => BrowserHost.Proxy.SendErrorReport(exceptionName, message));
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
            // 親と通信できなくなったら終了する
            Exit();
        }

        private void Exit()
        {
            if (!BrowserHost.Closed)
            {
                BrowserHost.Close();
                HeartbeatTimer.Stop();
                Application.Exit();
            }
        }

        private void ToolMenu_NavigateToLogInPage_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("ログインページへ移動します。\r\nよろしいですか？", "確認",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
                == System.Windows.Forms.DialogResult.OK)
            {
                Navigate(Configuration.LogInPageURL);
            }
        }

        private void ToolMenu_Zoom_DropDownOpening(object sender, EventArgs e)
        {
            var list = ToolMenu_Other_Zoom.DropDownItems.Cast<ToolStripItem>().ToArray();
            ToolMenu_Zoom.DropDownItems.AddRange(list);
        }

        private void ToolMenu_Other_Zoom_Fit_Click(object sender, EventArgs e)
        {
            Configuration.ZoomFit = ToolMenu_Other_Zoom_Fit.Checked;
            ApplyZoom();
            ConfigurationUpdated();
        }

        private void ToolMenu_Other_Zoom_Click(object sender, EventArgs e)
        {
            int zoom;

            if (sender == ToolMenu_Other_Zoom_25)
                zoom = 25;
            else if (sender == ToolMenu_Other_Zoom_50)
                zoom = 50;
            else if (sender == ToolMenu_Other_Zoom_66)
                zoom = 66;
            else if (sender == ToolMenu_Other_Zoom_75)
                zoom = 75;
            else if (sender == ToolMenu_Other_Zoom_100)
                zoom = 100;
            else if (sender == ToolMenu_Other_Zoom_150)
                zoom = 150;
            else if (sender == ToolMenu_Other_Zoom_200)
                zoom = 200;
            else if (sender == ToolMenu_Other_Zoom_250)
                zoom = 250;
            else if (sender == ToolMenu_Other_Zoom_300)
                zoom = 300;
            else if (sender == ToolMenu_Other_Zoom_400)
                zoom = 400;
            else
                zoom = 100;

            Configuration.ZoomRate = zoom;
            Configuration.ZoomFit = ToolMenu_Other_Zoom_Fit.Checked = false;
            ApplyZoom();
            ConfigurationUpdated();
        }

        private void ToolMenu_Other_Zoom_Decrement_Click(object sender, EventArgs e)
        {
            Configuration.ZoomRate = Math.Max(Configuration.ZoomRate - 20, 10);
            Configuration.ZoomFit = ToolMenu_Other_Zoom_Fit.Checked = false;
            ApplyZoom();
            ConfigurationUpdated();
        }

        private void ToolMenu_Other_Zoom_Increment_Click(object sender, EventArgs e)
        {
            Configuration.ZoomRate = Math.Min(Configuration.ZoomRate + 20, 1000);
            Configuration.ZoomFit = ToolMenu_Other_Zoom_Fit.Checked = false;
            ApplyZoom();
            ConfigurationUpdated();
        }

        private void ToolMenu_Other_Alignment_Click(object sender, EventArgs e)
        {
            if (sender == ToolMenu_Other_Alignment_Top)
                ToolMenu.Dock = DockStyle.Top;
            else if (sender == ToolMenu_Other_Alignment_Bottom)
                ToolMenu.Dock = DockStyle.Bottom;
            else if (sender == ToolMenu_Other_Alignment_Left)
                ToolMenu.Dock = DockStyle.Left;
            else
                ToolMenu.Dock = DockStyle.Right;

            Configuration.ToolMenuDockStyle = (int)ToolMenu.Dock;

            ConfigurationUpdated();
        }

        private void ToolMenu_Other_Alignment_Invisible_Click(object sender, EventArgs e)
        {
            ToolMenu.Visible =
            Configuration.IsToolMenuVisible = false;
            ConfigurationUpdated();
        }

        private void SizeAdjuster_DoubleClick(object sender, EventArgs e)
        {
            ToolMenu.Visible =
            Configuration.IsToolMenuVisible = true;
            ConfigurationUpdated();
        }

        private void ContextMenuTool_ShowToolMenu_Click(object sender, EventArgs e)
        {
            ToolMenu.Visible =
            Configuration.IsToolMenuVisible = true;
            ConfigurationUpdated();
        }

        private void ToolMenu_Other_AppliesStyleSheet_Click(object sender, EventArgs e)
        {
            Configuration.AppliesStyleSheet = ToolMenu_Other_AppliesStyleSheet.Checked;
            ApplyStyleSheet();
            ConfigurationUpdated();
        }

        private void FormBrowser_Activated(object sender, EventArgs e)
        {
            Browser.Focus();
        }

        private void ToolMenu_Other_LastScreenShot_DropDownOpening(object sender, EventArgs e)
        {
            try
            {
                using (var fs = new FileStream(_lastScreenShotPath, FileMode.Open, FileAccess.Read))
                {
                    if (ToolMenu_Other_LastScreenShot_Control.Image != null)
                        ToolMenu_Other_LastScreenShot_Control.Image.Dispose();

                    ToolMenu_Other_LastScreenShot_Control.Image = Image.FromStream(fs);
                }
            }
            catch (Exception)
            {
                // *ぷちっ*
            }
        }

        private void ToolMenu_Other_Refresh_Click(object sender, EventArgs e)
        {
            if (!Configuration.ConfirmAtRefresh ||
                MessageBox.Show("再読み込みします。\r\nよろしいですか？", "確認",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2)
                == System.Windows.Forms.DialogResult.OK)
            {
                RefreshBrowser();
            }
        }

        private void ToolMenu_Other_ScreenShot_Click(object sender, EventArgs e)
        {
            SaveScreenShot();
        }

        private void ToolMenu_Other_Mute_Click(object sender, EventArgs e)
        {
            try
            {
                _volumeManager.ToggleMute();
            }
            catch (Exception)
            {
                System.Media.SystemSounds.Beep.Play();
            }
            SetVolumeState();
        }

        private void ToolMenu_Other_Navigate_Click(object sender, EventArgs e)
        {
            BrowserHost.AsyncRemoteRun(() => BrowserHost.Proxy.RequestNavigation(Browser.Address));
        }

        private void ToolMenu_Other_ClearCache_Click(object sender, EventArgs e)
        {
            Browser.ShowDevTools();
        }
    }

    public class ToolStripOverride : ToolStripProfessionalRenderer
    {
        public ToolStripOverride()
        {
            this.RoundedEdges = false;
        }
        protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e) { }
    }

    /// <summary>
    /// ウィンドウが非アクティブ状態から1回のクリックでボタンが押せる ToolStrip です。
    /// </summary>
    internal class ExtraToolStrip : ToolStrip
    {
        public ExtraToolStrip() : base() { }

        private const uint WM_MOUSEACTIVATE = 0x21;
        private const uint MA_ACTIVATE = 1;
        private const uint MA_ACTIVATEANDEAT = 2;
        private const uint MA_NOACTIVATE = 3;
        private const uint MA_NOACTIVATEANDEAT = 4;

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == WM_MOUSEACTIVATE && m.Result == (IntPtr)MA_ACTIVATEANDEAT)
                m.Result = (IntPtr)MA_ACTIVATE;
        }
    }

    /// <summary>
    /// ポップアップウィンドウを無効化にする（リンクを元ウィンドウに開く）
    /// </summary>
    public class BrowserLifeSpanHandler : ILifeSpanHandler
    {
        public bool OnBeforePopup(IWebBrowser browserControl, CefSharp.IBrowser browser, IFrame frame, string targetUrl, string targetFrameName,
            WindowOpenDisposition targetDisposition, bool userGesture, IPopupFeatures popupFeatures, IWindowInfo windowInfo,
            IBrowserSettings browserSettings, ref bool noJavascriptAccess, out IWebBrowser newBrowser)
        {
            newBrowser = null;
            browserControl.Load(targetUrl);
            return true;
        }

        public void OnAfterCreated(IWebBrowser browserControl, CefSharp.IBrowser browser)
        {
            //
        }

        public bool DoClose(IWebBrowser browserControl, CefSharp.IBrowser browser)
        {
            return false;
        }

        public void OnBeforeClose(IWebBrowser browserControl, CefSharp.IBrowser browser)
        {
            //
        }
    }

    /// <summary>
    /// コンテキストメニューを無効化にする
    /// </summary>
    public class BrowserMenuHandler : IContextMenuHandler
    {
        public void OnBeforeContextMenu(IWebBrowser browserControl, CefSharp.IBrowser browser, IFrame frame, IContextMenuParams parameters, IMenuModel model)
        {
            model.Clear();
        }

        public bool OnContextMenuCommand(IWebBrowser browserControl, CefSharp.IBrowser browser, IFrame frame, IContextMenuParams parameters, CefMenuCommand commandId, CefEventFlags eventFlags)
        {
            return false;
        }

        public void OnContextMenuDismissed(IWebBrowser browserControl, CefSharp.IBrowser browser, IFrame frame)
        {
            //
        }

        public bool RunContextMenu(IWebBrowser browserControl, CefSharp.IBrowser browser, IFrame frame, IContextMenuParams parameters, IMenuModel model, IRunContextMenuCallback callback)
        {
            return false;
        }
    }

    /// <summary>
    /// ショートカットキー処理
    /// </summary>
    public class BrowserKeyboardHandler : IKeyboardHandler
    {
        public bool OnKeyEvent(IWebBrowser browserControl, CefSharp.IBrowser browser, KeyType type, int windowsKeyCode, int nativeKeyCode, CefEventFlags modifiers, bool isSystemKey)
        {
            return false;
        }

        public bool OnPreKeyEvent(IWebBrowser browserControl, CefSharp.IBrowser browser, KeyType type, int windowsKeyCode, int nativeKeyCode, CefEventFlags modifiers, bool isSystemKey, ref bool isKeyboardShortcut)
        {
            const int WM_SYSKEYDOWN = 0x104;
            const int WM_KEYDOWN = 0x100;
            const int WM_KEYUP = 0x101;
            const int WM_SYSKEYUP = 0x105;
            const int WM_CHAR = 0x102;
            const int WM_SYSCHAR = 0x106;
            const int VK_TAB = 0x9;

            bool result = false;

            isKeyboardShortcut = false;

            // Don't deal with TABs by default:
            // TODO: Are there any additional ones we need to be careful of?
            // i.e. Escape, Return, etc...?
            if (windowsKeyCode == VK_TAB)
            {
                return result;
            }

            Control control = browserControl as Control;
            int msgType = 0;
            switch (type)
            {
                case KeyType.RawKeyDown:
                    if (isSystemKey)
                    {
                        msgType = WM_SYSKEYDOWN;
                    }
                    else
                    {
                        msgType = WM_KEYDOWN;
                    }
                    break;
                case KeyType.KeyUp:
                    if (isSystemKey)
                    {
                        msgType = WM_SYSKEYUP;
                    }
                    else
                    {
                        msgType = WM_KEYUP;
                    }
                    break;
                case KeyType.Char:
                    if (isSystemKey)
                    {
                        msgType = WM_SYSCHAR;
                    }
                    else
                    {
                        msgType = WM_CHAR;
                    }
                    break;
                default:
                    break;
            }
            // We have to adapt from CEF's UI thread message loop to our fronting WinForm control here.
            // So, we have to make some calls that Application.Run usually ends up handling for us:
            PreProcessControlState state = PreProcessControlState.MessageNotNeeded;
            // We can't use BeginInvoke here, because we need the results for the return value
            // and isKeyboardShortcut. In theory this shouldn't deadlock, because
            // atm this is the only synchronous operation between the two threads.
            control.Invoke(new Action(() =>
            {
                Message msg = new Message() { HWnd = control.Handle, Msg = msgType, WParam = new IntPtr(windowsKeyCode), LParam = new IntPtr(nativeKeyCode) };

                // First comes Application.AddMessageFilter related processing:
                // 99.9% of the time in WinForms this doesn't do anything interesting.
                bool processed = Application.FilterMessage(ref msg);
                if (processed)
                {
                    state = PreProcessControlState.MessageProcessed;
                }
                else
                {
                    // Next we see if our control (or one of its parents)
                    // wants first crack at the message via several possible Control methods.
                    // This includes things like Mnemonics/Accelerators/Menu Shortcuts/etc...
                    state = control.PreProcessControlMessage(ref msg);
                }
            }));
            if (state == PreProcessControlState.MessageNeeded)
            {
                // TODO: Determine how to track MessageNeeded for OnKeyEvent.
                isKeyboardShortcut = true;
            }
            else if (state == PreProcessControlState.MessageProcessed)
            {
                // Most of the interesting cases get processed by PreProcessControlMessage.
                result = true;
            }
            return result;
        }
    }
}
