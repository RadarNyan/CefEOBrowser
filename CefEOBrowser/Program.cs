using System;
using System.Drawing;
using System.Windows.Forms;

namespace CefEOBrowser
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			// FormBrowserHostから起動された時は引数に通信用URLが渡される
			if (args.Length == 0) {
				MessageBox.Show(
					"これは七四式電子観測儀のサブプログラムであり、単体では起動できません。\r\n" +
					"本体から起動してください。\r\n\r\n" +
					"本程序是七四式电子观测仪的专用浏览器，无法单独启动。\r\n" +
					"请直接启动七四式电子观测仪以调用本程序。", "CefEOBrowser",
					MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			if (args.Length == 1) {
				Application.Run(new FormBrowser(args[0], "ja", SystemColors.Control));
			} else {
				Application.Run(new FormBrowser(args[0], args[1], ColorTranslator.FromHtml(args[2])));
			}
		}
	}
}
