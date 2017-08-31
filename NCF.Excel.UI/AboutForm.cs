using System.Windows.Forms;

namespace NCF.Excel.Forms
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
        }

        public AboutForm(string version) : this()
        {
            labelVersion.Text = version;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("mailto:franco.tiveron@northparkes.com");
        }
    }
}
