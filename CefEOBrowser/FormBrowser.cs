using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;

namespace CefEOBrowser
{
    public partial class FormBrowser : Form
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

        public FormBrowser()
        {
            InitializeComponent();
            InitializeChromium();
        }

        private void FormBrowser_FormClosing(object sender, FormClosingEventArgs e)
        {
            Cef.Shutdown();
        }
    }
}
