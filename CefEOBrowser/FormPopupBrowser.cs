using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CefEOBrowser
{
    public partial class FormPopupBrowser : Form
    {
        public FormPopupBrowser(string title, int width, int height)
        {
            InitializeComponent();
            Text = title;
            ClientSize = new Size(width, height);
            CenterToScreen();
        }
    }
}
