using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using InternetTest.Classes;
using LeoCorpLibrary;

namespace InternetTest.Forms
{
    public partial class AboutForm : Form
    {
        public AboutForm()
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
        }

        private void AboutForm_Load(object sender, EventArgs e)
        {
            Icon = new Branches().IconBranch(); // Met l'icône en foncion de la branche
            gunaPictureBox1.Image = new Branches().ImageBranch(); // Met l'image en fonction de la branche
            gunaPictureBox2.Image = new Branches().ImageBranch(); // Met l'image en fonction de la branche

            ChangeTheme(); // Changer le thème

            Guna.UI.Lib.GraphicsHelper.ShadowForm(this);

            gunaLabel3.Text = Definitions.Version; // Mettre la version
            gunaLabel3.Left = (ClientSize.Width - gunaLabel3.Width) / 2; // Centrer le label horizontalement
        }

        private void gunaLinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://github.com/Leo-Corporation/InternetTest/"); // Ouvrir le dépôt GitHub
        }

        private void gunaAdvenceTileButton1_Click(object sender, EventArgs e)
        {
            Close(); // Fermer
        }

        private void ChangeTheme()
        {
            if (new Theme().IsDark())
            {
                BackColor = Color.FromArgb(50, 50, 72); // Modifier la couleur d'arrière-plan
                gunaLabel1.ForeColor = Color.White; // Modifier la couleur des labels
                gunaLabel2.ForeColor = Color.White; // Modifier la couleur des labels
                gunaLabel3.ForeColor = Color.White; // Modifier la couleur des labels
                gunaAdvenceTileButton1.Image = Properties.Resources.icons8_delete_32px; // Modifier l'image
            }
            else
            {
                BackColor = Color.White; // Modifier la couleur d'arrière-plan
                gunaLabel1.ForeColor = Color.Black; // Modifier la couleur des labels
                gunaLabel2.ForeColor = Color.Black; // Modifier la couleur des labels
                gunaLabel3.ForeColor = Color.Black; // Modifier la couleur des labels
                gunaAdvenceTileButton1.Image = Properties.Resources.icons8_delete_100px_1; // Modifier l'image
            }
        }

        private async void gunaGradientButton2_Click(object sender, EventArgs e)
        {
            var fileInfoVersion = FileVersionInfo.GetVersionInfo(Application.StartupPath + "/Xalyus Updater.exe");
            string version = fileInfoVersion.FileVersion;
            if (LeoCorpLibrary.Update.IsAvailable(version, await LeoCorpLibrary.Update.GetLastVersionAsync("https://raw.githubusercontent.com/Leo-Corporation/LeoCorp-Docs/master/Liens/Update%20System/InternetTest/4.0/Xalyus%20Updater/version.txt"))) // Xalyus Updater
            {
                new UpdateXalyusUpdater(false).Show();
            }
            else // InternetTest 4
            {
                LeoCorpLibrary.Update.Check(Definitions.Version, await LeoCorpLibrary.Update.GetLastVersionAsync("https://raw.githubusercontent.com/Leo-Corporation/LeoCorp-Docs/master/Liens/Update%20System/InternetTest/4.0/version.txt"), new AvailableUpdate(), new UnavailableUpdate());
            }
        }
    }
}
