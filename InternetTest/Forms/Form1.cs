using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using LeoCorpLibrary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using InternetTest.Classes;
using InternetTest.Forms;

namespace InternetTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Icon = new Branches().IconBranch(); // Met l'icône en foncion de la branche
            gunaPictureBox1.Image = new Branches().ImageBranch(); // Met l'image en fonction de la branche
            Guna.UI.Lib.GraphicsHelper.ShadowForm(this);
        }

        private void gunaGradientButton1_Click(object sender, EventArgs e)
        {
            bool connectionAvailable = new NetworkConnection().IsAvailable();
            if (connectionAvailable) // Si internet est disponible
            {
                gunaLabel2.Visible = true;
                gunaLabel2.Text = "Vous êtes connecté à Internet";
                gunaLabel2.Left = (ClientSize.Width - gunaLabel2.Width) / 2;
            }
            else
            {
                gunaLabel2.Visible = true;
                gunaLabel2.Text = "Vous n'êtes pas connecté à Internet";
                gunaLabel2.Left = (ClientSize.Width - gunaLabel2.Width) / 2;
            }
        }

        private void gunaAdvenceTileButton1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void gunaAdvenceTileButton2_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void gunaGradientButton4_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/Leo-Corporation/InternetTest");
        }

        private void gunaLinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new SitesSelector().Show();
        }
    }
}
