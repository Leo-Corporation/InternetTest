using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using LeoCorpLibrary;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using InternetTest.Classes;
using InternetTest.Forms;
using System.Xml;
using System.Diagnostics;

namespace InternetTest
{
    public partial class Form1 : Form
    {
        bool isTestLaunched = false;
        public Form1()
        {
            if (new Language().GetCode() == "fr-FR")
            {
                Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("fr-FR");
            }
            else if (new Language().GetCode() == "EN")
            {
                Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            }
            InitializeComponent();
            Definitions.Form1 = this;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Icon = new Branches().IconBranch(); // Met l'icône en foncion de la branche
            gunaPictureBox1.Image = new Branches().ImageBranch(); // Met l'image en fonction de la branche
            Guna.UI.Lib.GraphicsHelper.ShadowForm(this);
            ChangeTheme(); // Change le thème en fonction des préférences de l'utilisateur
            if (Properties.Settings.Default.TestOnStart)
            {
                LaunchTest(); // Lancer un test
            }
            CheckUpdateOnStart();
        }

        private async void CheckUpdateOnStart()
        {
            if (Properties.Settings.Default.NotifyUpdate)
            {
                string lastVersion = await LeoCorpLibrary.Update.GetLastVersionAsync("https://raw.githubusercontent.com/Leo-Corporation/LeoCorp-Docs/master/Liens/Update%20System/InternetTest/4.0/version.txt");
                if (LeoCorpLibrary.Update.IsAvailable(Definitions.Version, lastVersion))
                {
                    notifyIcon1.Visible = true;
                    notifyIcon1.BalloonTipTitle = "InternetTest";
                    if (new Language().GetCode() == "fr-FR")
                    {
                        notifyIcon1.BalloonTipText = "Des mises à jour sont disponibles";
                    }
                    else if (new Language().GetCode() == "EN")
                    {
                        notifyIcon1.BalloonTipText = "Updates are availables";
                    }

                    notifyIcon1.ShowBalloonTip(5000);
                }
            }
        }

        private void gunaGradientButton1_Click(object sender, EventArgs e)
        {
            LaunchTest();
            menu1.Visible = false; // Hid the menu
        }

        private async void LaunchTest()
        {
            bool connectionAvailable = false;

            connectionAvailable = await NetworkConnection.IsAvailableTestSiteAsync(Properties.Settings.Default.TestSite);

            if (connectionAvailable) // Si internet est disponible
            {
                gunaPictureBox2.Image = Properties.Resources.check; // Mettre à jour la picture box avec le check
                if (new Language().GetCode() == "fr-FR") // Si la langue est française
                {
                    gunaLabel2.Visible = true; // Afficher le label
                    gunaLabel2.Text = "Vous êtes connecté à Internet"; // Mettre à jour le label
                    gunaLabel2.Left = (ClientSize.Width - gunaLabel2.Width) / 2; // Centrer le label
                }
                else if (new Language().GetCode() == "EN") // Si la langue est anglaise
                {
                    gunaLabel2.Visible = true; // Afficher le label
                    gunaLabel2.Text = "You're connected to Internet"; // Mettre à jour le label
                    gunaLabel2.Left = (ClientSize.Width - gunaLabel2.Width) / 2; // Centrer le label
                }
            }
            else // Si non
            {
                gunaPictureBox2.Image = Properties.Resources.cancel; // Mettre à jour la picture box avec le cancel
                if (new Language().GetCode() == "fr-FR") // Si la langue est française
                {
                    gunaLabel2.Visible = true; // Afficher le label
                    gunaLabel2.Text = "Vous n'êtes pas connecté à Internet"; // Mettre à jour le label
                    gunaLabel2.Left = (ClientSize.Width - gunaLabel2.Width) / 2; // Centrer le label
                }
                else if (new Language().GetCode() == "EN") // Si la langue est anglaise
                {
                    gunaLabel2.Visible = true; // Afficher le label
                    gunaLabel2.Text = "You aren't connected to Internet"; // Mettre à jour le label
                    gunaLabel2.Left = (ClientSize.Width - gunaLabel2.Width) / 2; // Centrer le label
                }
            }
            
            isTestLaunched = true;
        }

        private void gunaAdvenceTileButton1_Click(object sender, EventArgs e)
        {
            Close(); // Fermer
        }

        private void gunaAdvenceTileButton2_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized; // Minimiser la fenêtre
        }

        private void gunaGradientButton4_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/Leo-Corporation/InternetTest");
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
            new Settings().Show();
        }

        public void ChangeTheme() // Changer le thème
        {
            if (Theme.IsDark()) // Si le thème est sombre
            {
                BackColor = Color.FromArgb(50, 50, 72);// Modifier la couleur d'arrière-plan
                gunaLabel1.ForeColor = Color.White; // Modifier la couleur des labels
                gunaLabel2.ForeColor = Color.White; // Modifier la couleur des labels
                gunaAdvenceTileButton1.Image = Properties.Resources.icons8_delete_32px; // Modifier l'image
                gunaAdvenceTileButton2.Image = Properties.Resources.icons8_subtract_100px; // Modifier l'image
                gunaGradientButton4.BaseColor1 = Color.FromArgb(70, 70, 92); // Change color
                gunaGradientButton4.BaseColor2 = Color.FromArgb(70, 70, 92); // Change color
                gunaGradientButton4.ForeColor = Color.White; // Change color
                gunaGradientButton4.Image = Properties.Resources.globe; // Change image
                if (!isTestLaunched)
                {
                    gunaPictureBox2.Image = Properties.Resources.network_test; // Modifier l'image
                }
            }
            else // Si le thème est clair
            {
                BackColor = Color.White;// Modifier la couleur d'arrière-plan
                gunaLabel1.ForeColor = Color.Black; // Modifier la couleur des labels
                gunaLabel2.ForeColor = Color.Black; // Modifier la couleur des labels
                gunaAdvenceTileButton1.Image = Properties.Resources.icons8_delete_100px_1; // Modifier l'image
                gunaAdvenceTileButton2.Image = Properties.Resources.icons8_subtract_100px_1; // Modifier l'image
                gunaGradientButton4.BaseColor1 = Color.FromArgb(247, 247, 247); // Change color
                gunaGradientButton4.BaseColor2 = Color.FromArgb(247, 247, 247); // Change color
                gunaGradientButton4.ForeColor = Color.Black; // Change color
                gunaGradientButton4.Image = Properties.Resources.globe_black; // Change image
                if (!isTestLaunched)
                {
                    gunaPictureBox2.Image = Properties.Resources.network_test_black; // Modifier l'image
                }
            }
        }

        private void gunaLinkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new LocalizeIP().Show();
        }

        private void gunaGradientButton5_Click(object sender, EventArgs e)
        {
            menu1.Visible = !menu1.Visible; // Change the visibility
            menu1.ChangeTheme(); // Change the theme
        }

        private void Form1_Click(object sender, EventArgs e)
        {
            menu1.Visible = false; // Hid the menu
        }

        private void gunaGradientButton4_Click_1(object sender, EventArgs e)
        {
            Process.Start(Properties.Settings.Default.TestSite); // Open the browser
        }

        private void gunaGradientButton6_Click(object sender, EventArgs e)
        {
            new DownDetector().Show(); // Show
        }
    }
}
