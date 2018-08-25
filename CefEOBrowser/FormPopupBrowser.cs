using System.Drawing;
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
        }
    }
}
