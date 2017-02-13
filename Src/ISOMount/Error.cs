using System;
using System.Windows.Forms;

namespace ISOMount
{
    public partial class Error : Form
    {
        public Error(Exception ex)
        {
            InitializeComponent();

            txtError.Text = ex.InnerException != null
                                ? ex.Message + ex.StackTrace + Environment.NewLine + Environment.NewLine + ex.InnerException.Message + ex.InnerException.StackTrace
                                : ex.Message;
        }

        private void lnkCopyToClipboard_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Clipboard.SetText(txtError.Text);
        }
    }
}