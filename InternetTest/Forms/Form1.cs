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
            ChangeTheme(); // Change me thème en fonction des préférences de l'utilisateur
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

        private void gunaGradientButton3_Click(object sender, EventArgs e)
        {
            new AboutForm().Show();
        }

        private void gunaGradientButton2_Click(object sender, EventArgs e)
        {
            new Settings(this).Show();
        }

        public void ChangeTheme() // Changer le thème
        {
            if (new Theme().IsDark()) // Si le thème est sombre
            {
                BackColor = Color.FromArgb(50, 50, 72);
                gunaLabel1.ForeColor = Color.White;
                gunaLabel2.ForeColor = Color.White;
                gunaAdvenceTileButton1.Image = Properties.Resources.icons8_delete_32px;
                gunaAdvenceTileButton2.Image = Properties.Resources.icons8_subtract_100px;
            }
            else // Si le thème est clair
            {
                BackColor = Color.White;
                gunaLabel1.ForeColor = Color.Black;
                gunaLabel2.ForeColor = Color.Black;
                gunaAdvenceTileButton1.Image = Properties.Resources.icons8_delete_100px_1;
                gunaAdvenceTileButton2.Image = Properties.Resources.icons8_subtract_100px_1;
            }
        }
    }
}
