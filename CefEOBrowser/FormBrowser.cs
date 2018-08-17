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
            Cef.Initialize(settings, performDependencyCheck: false, browserProcessHandler: null);

            Browser = new ChromiumWebBrowser(url)
            {
                LifeSpanHandler = new BrowserLifeSpanHandler()
            };
            this.SizeAdjuster.Controls.Add(Browser);
            Browser.Dock = DockStyle.Fill;
            Cef_started = true;

            libraryLoader.Dispose();
        }

        private readonly Size KanColleSize = new Size(800, 480);
        private bool RestoreStyleSheet = false;

        // FormBrowserHostの通信サーバ
        private string ServerUri;

        // FormBrowserの通信サーバ
        private PipeCommunicator<BrowserLib.IBrowserHost> BrowserHost;

        private BrowserLib.BrowserConfiguration Configuration;

        // 親プロセスが生きているか定期的に確認するためのタイマー
        private Timer HeartbeatTimer = new Timer();
        private IntPtr HostWindow;

        private bool _styleSheetApplied;
        /// <summary>
        /// スタイルシートの変更が適用されているか
        /// </summary>
        private bool StyleSheetApplied
        {
            get { return _styleSheetApplied; }
            set
            {

                if (value)
                {
                    //Browser.Anchor = AnchorStyles.None;
                    ApplyZoom();
                    SizeAdjuster_SizeChanged(null, new EventArgs());

                }
                else
                {
                    SizeAdjuster.SuspendLayout();
                    //Browser.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                    Browser.Location = new Point(0, 0);
                    Browser.MinimumSize = new Size(0, 0);
                    Browser.Size = SizeAdjuster.Size;
                    SizeAdjuster.ResumeLayout();
                }

                _styleSheetApplied = value;
            }
        }

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
                double zoomrate = 0.5;
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
            ToolMenu_Other_Zoom_Fit.Checked = Configuration.ZoomFit;
            ApplyZoom();
            ToolMenu_Other_AppliesStyleSheet.Checked = Configuration.AppliesStyleSheet;
            ToolMenu.Dock = (DockStyle)Configuration.ToolMenuDockStyle;
            ToolMenu.Visible = Configuration.IsToolMenuVisible;
        }

        public void InitialAPIReceived()
        {
            IsKanColleLoaded = true;

            //ロード直後の適用ではレイアウトがなぜか崩れるのでこのタイミングでも適用
            ApplyStyleSheet();
            ApplyZoom();
            DestroyDMMreloadDialog();

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
            // throw new NotImplementedException();
            return new Bitmap(100, 100, is32bpp ? PixelFormat.Format32bppArgb : PixelFormat.Format24bppRgb);
        }

        public void RefreshBrowser()
        {
            if (!Configuration.AppliesStyleSheet)
                StyleSheetApplied = false;

            Browser.Reload();
        }

        public void ApplyZoom()
        {
            int zoomRate = Configuration.ZoomRate;
            bool fit = Configuration.ZoomFit && StyleSheetApplied;

            try
            {
                Browser.SetZoomLevel((zoomRate - 100) / 25.0);
                /*
                var wb = Browser.ActiveXInstance as SHDocVw.IWebBrowser2;
                if (wb == null || wb.ReadyState == SHDocVw.tagREADYSTATE.READYSTATE_UNINITIALIZED || wb.Busy) return;

                double zoomFactor;
                object pin;

                if (fit)
                {
                    pin = 100;
                    double rateX = (double)SizeAdjuster.Width / KanColleSize.Width;
                    double rateY = (double)SizeAdjuster.Height / KanColleSize.Height;
                    zoomFactor = Math.Min(rateX, rateY);
                }
                else
                {
                    if (zoomRate < 10)
                        zoomRate = 10;
                    if (zoomRate > 1000)
                        zoomRate = 1000;

                    pin = zoomRate;
                    zoomFactor = zoomRate / 100.0;
                }

                object pout = null;
                wb.ExecWB(SHDocVw.OLECMDID.OLECMDID_OPTICAL_ZOOM, SHDocVw.OLECMDEXECOPT.OLECMDEXECOPT_DODEFAULT, ref pin, ref pout);

                if (StyleSheetApplied)
                {
                    Browser.Size = Browser.MinimumSize = new Size(
                        (int)(KanColleSize.Width * zoomFactor),
                        (int)(KanColleSize.Height * zoomFactor)
                        );
                    CenteringBrowser();
                }*/

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

        public void Navigate(string url)
        {
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

            //AddLog(2, "[CefEOBrowser] Proxy " + proxy);
            //AddLog(2, "[CefEOBrowser] Proxy_cef " + proxy_cef);
            //AddLog(2, "[CefEOBrowser] Page " + Configuration.LogInPageURL);

            if (Cef_started)
            {
                MessageBox.Show("実行中のプロキシ設定の変更はサポートされていません。\r\n七四式電子観測儀を再起動してください。", "CefEOBrowser");
                Cef.Shutdown();
            }
            else
            {
                // Start Cef Browser
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
            if (!Configuration.AppliesStyleSheet && !RestoreStyleSheet)
                return;

            try
            {
                /*
                var document = Browser.Document;
                if (document == null) return;

                if (document.Url.ToString().Contains(".swf?"))
                {

                    document.InvokeScript("eval", new object[] { "document.body.style.margin=0;" });

                }
                else
                {
                    var swf = getFrameElementById(document, "externalswf");
                    if (swf == null) return;

                    if (RestoreStyleSheet)
                    {
                        document.InvokeScript("eval", new object[] { string.Format(RestoreScript, StyleClassID) });
                        swf.Document.InvokeScript("eval", new object[] { string.Format(RestoreScript, StyleClassID) });
                        StyleSheetApplied = false;
                        RestoreStyleSheet = false;
                        return;
                    }
                    // InvokeScriptは関数しか呼べないようなので、スクリプトをevalで渡す
                    document.InvokeScript("eval", new object[] { string.Format(Properties.Resources.PageScript, StyleClassID) });
                    swf.Document.InvokeScript("eval", new object[] { string.Format(Properties.Resources.FrameScript, StyleClassID) });
                }
                */

                StyleSheetApplied = true;
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
            BrowserHost.AsyncRemoteRun(() => BrowserHost.Proxy.AddLog(priority, message));
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

        private void ToolMenu_Refresh_Click(object sender, EventArgs e)
        {
            if (!Configuration.ConfirmAtRefresh ||
                MessageBox.Show("再読み込みします。\r\nよろしいですか？", "確認",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2)
                == System.Windows.Forms.DialogResult.OK)
            {
                RefreshBrowser();
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

        private void ToolMenu_Mute_Click(object sender, EventArgs e)
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
            //nothing
        }
    }
}
