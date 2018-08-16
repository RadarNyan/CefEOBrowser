namespace CefEOBrowser
{
    partial class FormBrowser
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.SizeAdjuster = new System.Windows.Forms.Panel();
            this.ToolMenu = new CefEOBrowser.ExtraToolStrip();
            this.ToolMenu_ScreenShot = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolMenu_Zoom = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolMenu_Mute = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolMenu_Refresh = new System.Windows.Forms.ToolStripButton();
            this.ToolMenu_NavigateToLogInPage = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolMenu_Other = new System.Windows.Forms.ToolStripDropDownButton();
            this.Icons = new System.Windows.Forms.ImageList(this.components);
            this.ContextMenuTool = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ContextMenuTool_ShowToolMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolMenu_Other_ScreenShot = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolMenu_Other_LastScreenShot = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolMenu_Other_Zoom = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolMenu_Other_Volume = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolMenu_Other_Mute = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolMenu_Other_Refresh = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolMenu_Other_NavigateToLogInPage = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolMenu_Other_Navigate = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolMenu_Other_AppliesStyleSheet = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolMenu_Other_ClearCache = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolMenu_Other_Alignment = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolMenu_Other_LastScreenShot_OpenScreenShotFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolMenu_Other_LastScreenShot_CopyToClipboard = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolMenu_Other_Zoom_Current = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolMenu_Other_Zoom_Fit = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator12 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolMenu_Other_Zoom_Decrement = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolMenu_Other_Zoom_Increment = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator13 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolMenu_Other_Zoom_25 = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolMenu_Other_Zoom_50 = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolMenu_Other_Zoom_75 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator14 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolMenu_Other_Zoom_100 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator15 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolMenu_Other_Zoom_150 = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolMenu_Other_Zoom_200 = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolMenu_Other_Zoom_250 = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolMenu_Other_Zoom_300 = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolMenu_Other_Zoom_400 = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolMenu_Other_Alignment_Top = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolMenu_Other_Alignment_Bottom = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolMenu_Other_Alignment_Left = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolMenu_Other_Alignment_Right = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolMenu_Other_Alignment_Invisible = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolMenu.SuspendLayout();
            this.ContextMenuTool.SuspendLayout();
            this.SuspendLayout();
            // 
            // SizeAdjuster
            // 
            this.SizeAdjuster.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SizeAdjuster.Location = new System.Drawing.Point(0, 25);
            this.SizeAdjuster.Margin = new System.Windows.Forms.Padding(0);
            this.SizeAdjuster.Name = "SizeAdjuster";
            this.SizeAdjuster.Size = new System.Drawing.Size(800, 425);
            this.SizeAdjuster.TabIndex = 1;
            // 
            // ToolMenu
            // 
            this.ToolMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolMenu_ScreenShot,
            this.toolStripSeparator1,
            this.ToolMenu_Zoom,
            this.toolStripSeparator2,
            this.ToolMenu_Mute,
            this.toolStripSeparator3,
            this.ToolMenu_Refresh,
            this.ToolMenu_NavigateToLogInPage,
            this.toolStripSeparator4,
            this.ToolMenu_Other});
            this.ToolMenu.Location = new System.Drawing.Point(0, 0);
            this.ToolMenu.Name = "ToolMenu";
            this.ToolMenu.Size = new System.Drawing.Size(800, 25);
            this.ToolMenu.TabIndex = 0;
            this.ToolMenu.Text = "toolStrip1";
            // 
            // ToolMenu_ScreenShot
            // 
            this.ToolMenu_ScreenShot.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToolMenu_ScreenShot.Name = "ToolMenu_ScreenShot";
            this.ToolMenu_ScreenShot.Size = new System.Drawing.Size(23, 22);
            this.ToolMenu_ScreenShot.Text = "スクリーンショット";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // ToolMenu_Zoom
            // 
            this.ToolMenu_Zoom.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToolMenu_Zoom.Name = "ToolMenu_Zoom";
            this.ToolMenu_Zoom.Size = new System.Drawing.Size(13, 22);
            this.ToolMenu_Zoom.Text = "ズーム";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // ToolMenu_Mute
            // 
            this.ToolMenu_Mute.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToolMenu_Mute.Name = "ToolMenu_Mute";
            this.ToolMenu_Mute.Size = new System.Drawing.Size(23, 22);
            this.ToolMenu_Mute.Text = "ミュート";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // ToolMenu_Refresh
            // 
            this.ToolMenu_Refresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToolMenu_Refresh.Name = "ToolMenu_Refresh";
            this.ToolMenu_Refresh.Size = new System.Drawing.Size(23, 22);
            this.ToolMenu_Refresh.Text = "更新";
            // 
            // ToolMenu_NavigateToLogInPage
            // 
            this.ToolMenu_NavigateToLogInPage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToolMenu_NavigateToLogInPage.Name = "ToolMenu_NavigateToLogInPage";
            this.ToolMenu_NavigateToLogInPage.Size = new System.Drawing.Size(23, 22);
            this.ToolMenu_NavigateToLogInPage.Text = "ログインページへ移動";
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // ToolMenu_Other
            // 
            this.ToolMenu_Other.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToolMenu_Other.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolMenu_Other_ScreenShot,
            this.ToolMenu_Other_LastScreenShot,
            this.toolStripSeparator5,
            this.ToolMenu_Other_Zoom,
            this.toolStripSeparator6,
            this.ToolMenu_Other_Volume,
            this.ToolMenu_Other_Mute,
            this.toolStripSeparator7,
            this.ToolMenu_Other_Refresh,
            this.ToolMenu_Other_NavigateToLogInPage,
            this.ToolMenu_Other_Navigate,
            this.toolStripSeparator8,
            this.ToolMenu_Other_AppliesStyleSheet,
            this.ToolMenu_Other_ClearCache,
            this.toolStripSeparator9,
            this.ToolMenu_Other_Alignment});
            this.ToolMenu_Other.Name = "ToolMenu_Other";
            this.ToolMenu_Other.Size = new System.Drawing.Size(13, 22);
            this.ToolMenu_Other.Text = "その他";
            // 
            // Icons
            // 
            this.Icons.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.Icons.ImageSize = new System.Drawing.Size(16, 16);
            this.Icons.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // ContextMenuTool
            // 
            this.ContextMenuTool.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ContextMenuTool_ShowToolMenu});
            this.ContextMenuTool.Name = "ContextMenuTool";
            this.ContextMenuTool.Size = new System.Drawing.Size(195, 26);
            // 
            // ContextMenuTool_ShowToolMenu
            // 
            this.ContextMenuTool_ShowToolMenu.Name = "ContextMenuTool_ShowToolMenu";
            this.ContextMenuTool_ShowToolMenu.Size = new System.Drawing.Size(194, 22);
            this.ContextMenuTool_ShowToolMenu.Text = "ツールメニューを表示";
            // 
            // ToolMenu_Other_ScreenShot
            // 
            this.ToolMenu_Other_ScreenShot.Name = "ToolMenu_Other_ScreenShot";
            this.ToolMenu_Other_ScreenShot.Size = new System.Drawing.Size(233, 22);
            this.ToolMenu_Other_ScreenShot.Text = "スクリーンショット(&S)";
            // 
            // ToolMenu_Other_LastScreenShot
            // 
            this.ToolMenu_Other_LastScreenShot.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator10,
            this.ToolMenu_Other_LastScreenShot_OpenScreenShotFolder,
            this.ToolMenu_Other_LastScreenShot_CopyToClipboard});
            this.ToolMenu_Other_LastScreenShot.Name = "ToolMenu_Other_LastScreenShot";
            this.ToolMenu_Other_LastScreenShot.Size = new System.Drawing.Size(233, 22);
            this.ToolMenu_Other_LastScreenShot.Text = "直前のスクリーンショット(&P)";
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(230, 6);
            // 
            // ToolMenu_Other_Zoom
            // 
            this.ToolMenu_Other_Zoom.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolMenu_Other_Zoom_Current,
            this.toolStripSeparator11,
            this.ToolMenu_Other_Zoom_Fit,
            this.toolStripSeparator12,
            this.ToolMenu_Other_Zoom_Decrement,
            this.ToolMenu_Other_Zoom_Increment,
            this.toolStripSeparator13,
            this.ToolMenu_Other_Zoom_25,
            this.ToolMenu_Other_Zoom_50,
            this.ToolMenu_Other_Zoom_75,
            this.toolStripSeparator14,
            this.ToolMenu_Other_Zoom_100,
            this.toolStripSeparator15,
            this.ToolMenu_Other_Zoom_150,
            this.ToolMenu_Other_Zoom_200,
            this.ToolMenu_Other_Zoom_250,
            this.ToolMenu_Other_Zoom_300,
            this.ToolMenu_Other_Zoom_400});
            this.ToolMenu_Other_Zoom.Name = "ToolMenu_Other_Zoom";
            this.ToolMenu_Other_Zoom.Size = new System.Drawing.Size(233, 22);
            this.ToolMenu_Other_Zoom.Text = "ズーム(&Z)";
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(230, 6);
            // 
            // ToolMenu_Other_Volume
            // 
            this.ToolMenu_Other_Volume.Name = "ToolMenu_Other_Volume";
            this.ToolMenu_Other_Volume.Size = new System.Drawing.Size(233, 22);
            this.ToolMenu_Other_Volume.Text = "音量(&V)";
            // 
            // ToolMenu_Other_Mute
            // 
            this.ToolMenu_Other_Mute.Name = "ToolMenu_Other_Mute";
            this.ToolMenu_Other_Mute.Size = new System.Drawing.Size(233, 22);
            this.ToolMenu_Other_Mute.Text = "ミュート(&M)";
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(230, 6);
            // 
            // ToolMenu_Other_Refresh
            // 
            this.ToolMenu_Other_Refresh.Name = "ToolMenu_Other_Refresh";
            this.ToolMenu_Other_Refresh.Size = new System.Drawing.Size(233, 22);
            this.ToolMenu_Other_Refresh.Text = "更新(&R)";
            // 
            // ToolMenu_Other_NavigateToLogInPage
            // 
            this.ToolMenu_Other_NavigateToLogInPage.Name = "ToolMenu_Other_NavigateToLogInPage";
            this.ToolMenu_Other_NavigateToLogInPage.Size = new System.Drawing.Size(233, 22);
            this.ToolMenu_Other_NavigateToLogInPage.Text = "ログインページへ移動(&L)";
            // 
            // ToolMenu_Other_Navigate
            // 
            this.ToolMenu_Other_Navigate.Name = "ToolMenu_Other_Navigate";
            this.ToolMenu_Other_Navigate.Size = new System.Drawing.Size(233, 22);
            this.ToolMenu_Other_Navigate.Text = "移動(&N)...";
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(230, 6);
            // 
            // ToolMenu_Other_AppliesStyleSheet
            // 
            this.ToolMenu_Other_AppliesStyleSheet.Name = "ToolMenu_Other_AppliesStyleSheet";
            this.ToolMenu_Other_AppliesStyleSheet.Size = new System.Drawing.Size(233, 22);
            this.ToolMenu_Other_AppliesStyleSheet.Text = "スタイルシートを適用する";
            // 
            // ToolMenu_Other_ClearCache
            // 
            this.ToolMenu_Other_ClearCache.Name = "ToolMenu_Other_ClearCache";
            this.ToolMenu_Other_ClearCache.Size = new System.Drawing.Size(233, 22);
            this.ToolMenu_Other_ClearCache.Text = "キャッシュのクリア(&C)";
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size(230, 6);
            // 
            // ToolMenu_Other_Alignment
            // 
            this.ToolMenu_Other_Alignment.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolMenu_Other_Alignment_Top,
            this.ToolMenu_Other_Alignment_Bottom,
            this.ToolMenu_Other_Alignment_Left,
            this.ToolMenu_Other_Alignment_Right,
            this.ToolMenu_Other_Alignment_Invisible});
            this.ToolMenu_Other_Alignment.Name = "ToolMenu_Other_Alignment";
            this.ToolMenu_Other_Alignment.Size = new System.Drawing.Size(233, 22);
            this.ToolMenu_Other_Alignment.Text = "配置(&A)";
            // 
            // toolStripSeparator10
            // 
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            this.toolStripSeparator10.Size = new System.Drawing.Size(219, 6);
            // 
            // ToolMenu_Other_LastScreenShot_OpenScreenShotFolder
            // 
            this.ToolMenu_Other_LastScreenShot_OpenScreenShotFolder.Name = "ToolMenu_Other_LastScreenShot_OpenScreenShotFolder";
            this.ToolMenu_Other_LastScreenShot_OpenScreenShotFolder.Size = new System.Drawing.Size(222, 22);
            this.ToolMenu_Other_LastScreenShot_OpenScreenShotFolder.Text = "保存フォルダを開く(&O)";
            // 
            // ToolMenu_Other_LastScreenShot_CopyToClipboard
            // 
            this.ToolMenu_Other_LastScreenShot_CopyToClipboard.Name = "ToolMenu_Other_LastScreenShot_CopyToClipboard";
            this.ToolMenu_Other_LastScreenShot_CopyToClipboard.Size = new System.Drawing.Size(222, 22);
            this.ToolMenu_Other_LastScreenShot_CopyToClipboard.Text = "クリップボードにコピー(&C)";
            // 
            // ToolMenu_Other_Zoom_Current
            // 
            this.ToolMenu_Other_Zoom_Current.Enabled = false;
            this.ToolMenu_Other_Zoom_Current.Name = "ToolMenu_Other_Zoom_Current";
            this.ToolMenu_Other_Zoom_Current.Size = new System.Drawing.Size(180, 22);
            this.ToolMenu_Other_Zoom_Current.Text = "現在%";
            // 
            // toolStripSeparator11
            // 
            this.toolStripSeparator11.Name = "toolStripSeparator11";
            this.toolStripSeparator11.Size = new System.Drawing.Size(177, 6);
            // 
            // ToolMenu_Other_Zoom_Fit
            // 
            this.ToolMenu_Other_Zoom_Fit.Name = "ToolMenu_Other_Zoom_Fit";
            this.ToolMenu_Other_Zoom_Fit.Size = new System.Drawing.Size(180, 22);
            this.ToolMenu_Other_Zoom_Fit.Text = "ぴったり";
            // 
            // toolStripSeparator12
            // 
            this.toolStripSeparator12.Name = "toolStripSeparator12";
            this.toolStripSeparator12.Size = new System.Drawing.Size(177, 6);
            // 
            // ToolMenu_Other_Zoom_Decrement
            // 
            this.ToolMenu_Other_Zoom_Decrement.Name = "ToolMenu_Other_Zoom_Decrement";
            this.ToolMenu_Other_Zoom_Decrement.Size = new System.Drawing.Size(180, 22);
            this.ToolMenu_Other_Zoom_Decrement.Text = "-20%";
            // 
            // ToolMenu_Other_Zoom_Increment
            // 
            this.ToolMenu_Other_Zoom_Increment.Name = "ToolMenu_Other_Zoom_Increment";
            this.ToolMenu_Other_Zoom_Increment.Size = new System.Drawing.Size(180, 22);
            this.ToolMenu_Other_Zoom_Increment.Text = "+20%";
            // 
            // toolStripSeparator13
            // 
            this.toolStripSeparator13.Name = "toolStripSeparator13";
            this.toolStripSeparator13.Size = new System.Drawing.Size(177, 6);
            // 
            // ToolMenu_Other_Zoom_25
            // 
            this.ToolMenu_Other_Zoom_25.Name = "ToolMenu_Other_Zoom_25";
            this.ToolMenu_Other_Zoom_25.Size = new System.Drawing.Size(180, 22);
            this.ToolMenu_Other_Zoom_25.Text = "25%";
            // 
            // ToolMenu_Other_Zoom_50
            // 
            this.ToolMenu_Other_Zoom_50.Name = "ToolMenu_Other_Zoom_50";
            this.ToolMenu_Other_Zoom_50.Size = new System.Drawing.Size(180, 22);
            this.ToolMenu_Other_Zoom_50.Text = "50%";
            // 
            // ToolMenu_Other_Zoom_75
            // 
            this.ToolMenu_Other_Zoom_75.Name = "ToolMenu_Other_Zoom_75";
            this.ToolMenu_Other_Zoom_75.Size = new System.Drawing.Size(180, 22);
            this.ToolMenu_Other_Zoom_75.Text = "75%";
            // 
            // toolStripSeparator14
            // 
            this.toolStripSeparator14.Name = "toolStripSeparator14";
            this.toolStripSeparator14.Size = new System.Drawing.Size(177, 6);
            // 
            // ToolMenu_Other_Zoom_100
            // 
            this.ToolMenu_Other_Zoom_100.Name = "ToolMenu_Other_Zoom_100";
            this.ToolMenu_Other_Zoom_100.Size = new System.Drawing.Size(180, 22);
            this.ToolMenu_Other_Zoom_100.Text = "100%";
            // 
            // toolStripSeparator15
            // 
            this.toolStripSeparator15.Name = "toolStripSeparator15";
            this.toolStripSeparator15.Size = new System.Drawing.Size(177, 6);
            // 
            // ToolMenu_Other_Zoom_150
            // 
            this.ToolMenu_Other_Zoom_150.Name = "ToolMenu_Other_Zoom_150";
            this.ToolMenu_Other_Zoom_150.Size = new System.Drawing.Size(180, 22);
            this.ToolMenu_Other_Zoom_150.Text = "150%";
            // 
            // ToolMenu_Other_Zoom_200
            // 
            this.ToolMenu_Other_Zoom_200.Name = "ToolMenu_Other_Zoom_200";
            this.ToolMenu_Other_Zoom_200.Size = new System.Drawing.Size(180, 22);
            this.ToolMenu_Other_Zoom_200.Text = "200%";
            // 
            // ToolMenu_Other_Zoom_250
            // 
            this.ToolMenu_Other_Zoom_250.Name = "ToolMenu_Other_Zoom_250";
            this.ToolMenu_Other_Zoom_250.Size = new System.Drawing.Size(180, 22);
            this.ToolMenu_Other_Zoom_250.Text = "250%";
            // 
            // ToolMenu_Other_Zoom_300
            // 
            this.ToolMenu_Other_Zoom_300.Name = "ToolMenu_Other_Zoom_300";
            this.ToolMenu_Other_Zoom_300.Size = new System.Drawing.Size(180, 22);
            this.ToolMenu_Other_Zoom_300.Text = "300%";
            // 
            // ToolMenu_Other_Zoom_400
            // 
            this.ToolMenu_Other_Zoom_400.Name = "ToolMenu_Other_Zoom_400";
            this.ToolMenu_Other_Zoom_400.Size = new System.Drawing.Size(180, 22);
            this.ToolMenu_Other_Zoom_400.Text = "400%";
            // 
            // ToolMenu_Other_Alignment_Top
            // 
            this.ToolMenu_Other_Alignment_Top.Name = "ToolMenu_Other_Alignment_Top";
            this.ToolMenu_Other_Alignment_Top.Size = new System.Drawing.Size(180, 22);
            this.ToolMenu_Other_Alignment_Top.Text = "上(&T)";
            // 
            // ToolMenu_Other_Alignment_Bottom
            // 
            this.ToolMenu_Other_Alignment_Bottom.Name = "ToolMenu_Other_Alignment_Bottom";
            this.ToolMenu_Other_Alignment_Bottom.Size = new System.Drawing.Size(180, 22);
            this.ToolMenu_Other_Alignment_Bottom.Text = "下(&B)";
            // 
            // ToolMenu_Other_Alignment_Left
            // 
            this.ToolMenu_Other_Alignment_Left.Name = "ToolMenu_Other_Alignment_Left";
            this.ToolMenu_Other_Alignment_Left.Size = new System.Drawing.Size(180, 22);
            this.ToolMenu_Other_Alignment_Left.Text = "左(&L)";
            // 
            // ToolMenu_Other_Alignment_Right
            // 
            this.ToolMenu_Other_Alignment_Right.Name = "ToolMenu_Other_Alignment_Right";
            this.ToolMenu_Other_Alignment_Right.Size = new System.Drawing.Size(180, 22);
            this.ToolMenu_Other_Alignment_Right.Text = "右(&R)";
            // 
            // ToolMenu_Other_Alignment_Invisible
            // 
            this.ToolMenu_Other_Alignment_Invisible.Name = "ToolMenu_Other_Alignment_Invisible";
            this.ToolMenu_Other_Alignment_Invisible.Size = new System.Drawing.Size(180, 22);
            this.ToolMenu_Other_Alignment_Invisible.Text = "非表示(&I)";
            // 
            // FormBrowser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.SizeAdjuster);
            this.Controls.Add(this.ToolMenu);
            this.Name = "FormBrowser";
            this.Text = "CefEOBrowser";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormBrowser_FormClosing);
            this.Load += new System.EventHandler(this.FormBrowser_Load);
            this.ToolMenu.ResumeLayout(false);
            this.ToolMenu.PerformLayout();
            this.ContextMenuTool.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel SizeAdjuster;
        private System.Windows.Forms.ImageList Icons;
        private ExtraToolStrip ToolMenu;
        private System.Windows.Forms.ContextMenuStrip ContextMenuTool;
        private System.Windows.Forms.ToolStripMenuItem ContextMenuTool_ShowToolMenu;
        private System.Windows.Forms.ToolStripButton ToolMenu_ScreenShot;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripDropDownButton ToolMenu_Zoom;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton ToolMenu_Mute;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton ToolMenu_Refresh;
        private System.Windows.Forms.ToolStripButton ToolMenu_NavigateToLogInPage;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripDropDownButton ToolMenu_Other;
        private System.Windows.Forms.ToolStripMenuItem ToolMenu_Other_ScreenShot;
        private System.Windows.Forms.ToolStripMenuItem ToolMenu_Other_LastScreenShot;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
        private System.Windows.Forms.ToolStripMenuItem ToolMenu_Other_LastScreenShot_OpenScreenShotFolder;
        private System.Windows.Forms.ToolStripMenuItem ToolMenu_Other_LastScreenShot_CopyToClipboard;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem ToolMenu_Other_Zoom;
        private System.Windows.Forms.ToolStripMenuItem ToolMenu_Other_Zoom_Current;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator11;
        private System.Windows.Forms.ToolStripMenuItem ToolMenu_Other_Zoom_Fit;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator12;
        private System.Windows.Forms.ToolStripMenuItem ToolMenu_Other_Zoom_Decrement;
        private System.Windows.Forms.ToolStripMenuItem ToolMenu_Other_Zoom_Increment;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator13;
        private System.Windows.Forms.ToolStripMenuItem ToolMenu_Other_Zoom_25;
        private System.Windows.Forms.ToolStripMenuItem ToolMenu_Other_Zoom_50;
        private System.Windows.Forms.ToolStripMenuItem ToolMenu_Other_Zoom_75;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator14;
        private System.Windows.Forms.ToolStripMenuItem ToolMenu_Other_Zoom_100;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator15;
        private System.Windows.Forms.ToolStripMenuItem ToolMenu_Other_Zoom_150;
        private System.Windows.Forms.ToolStripMenuItem ToolMenu_Other_Zoom_200;
        private System.Windows.Forms.ToolStripMenuItem ToolMenu_Other_Zoom_250;
        private System.Windows.Forms.ToolStripMenuItem ToolMenu_Other_Zoom_300;
        private System.Windows.Forms.ToolStripMenuItem ToolMenu_Other_Zoom_400;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem ToolMenu_Other_Volume;
        private System.Windows.Forms.ToolStripMenuItem ToolMenu_Other_Mute;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripMenuItem ToolMenu_Other_Refresh;
        private System.Windows.Forms.ToolStripMenuItem ToolMenu_Other_NavigateToLogInPage;
        private System.Windows.Forms.ToolStripMenuItem ToolMenu_Other_Navigate;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripMenuItem ToolMenu_Other_AppliesStyleSheet;
        private System.Windows.Forms.ToolStripMenuItem ToolMenu_Other_ClearCache;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStripMenuItem ToolMenu_Other_Alignment;
        private System.Windows.Forms.ToolStripMenuItem ToolMenu_Other_Alignment_Top;
        private System.Windows.Forms.ToolStripMenuItem ToolMenu_Other_Alignment_Bottom;
        private System.Windows.Forms.ToolStripMenuItem ToolMenu_Other_Alignment_Left;
        private System.Windows.Forms.ToolStripMenuItem ToolMenu_Other_Alignment_Right;
        private System.Windows.Forms.ToolStripMenuItem ToolMenu_Other_Alignment_Invisible;
    }
}

