using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace OplServer.Interface
{
    public partial class frmAbout : Form
    {
        public frmAbout()
        {
            InitializeComponent();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(linkLabel2.Text);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("mailto:" + linkLabel1.Text);
        }

        private void frmAbout_Load(object sender, EventArgs e)
        {
        }
    }
}