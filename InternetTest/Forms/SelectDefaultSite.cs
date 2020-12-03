using InternetTest.Classes;
using InternetTest.UserControls.SettingsPages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InternetTest.Forms
{
    public partial class SelectDefaultSite : Form
    {
        Test testFrm; // Fenêtre "Paramètres"
        public SelectDefaultSite(Test test)
        {
            testFrm = test;
            if (new Language().GetCode() == "fr-FR") // Change la langue en français (France)
            {
                Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("fr-FR");
            }
            else if (new Language().GetCode() == "EN") // Change la langue en anglais (Etats-Unis)
            {
                Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            }
            InitializeComponent();
        }

        private void SelectDefaultSite_Load(object sender, EventArgs e)
        {
            Icon = new Branches().IconBranch(); // Met l'icône en foncion de la branche
            gunaPictureBox1.Image = new Branches().ImageBranch(); // Met l'image en fonction de la branche
            ChangeTheme(); // Changer le thème à celui qui est sélectionné
            Guna.UI.Lib.GraphicsHelper.ShadowForm(this); // Ajouter une ombre sur la fenêtre
        }

        private void ChangeTheme() // Change le thème
        {
            if (new Classes.Theme().IsDark()) // Si le thème est sombre
            {
                BackColor = Color.FromArgb(50, 50, 72);
                gunaLabel1.ForeColor = Color.White;
                gunaAdvenceTileButton1.Image = Properties.Resources.icons8_delete_32px;
            }
            else // Si le thème est clair
            {
                BackColor = Color.White;
                gunaLabel1.ForeColor = Color.Black;
                gunaAdvenceTileButton1.Image = Properties.Resources.icons8_delete_100px_1;
            }
        }

        private void gunaGradientTileButton1_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.TestSite = "https://bing.com"; // Met à jour le paramètre
            Properties.Settings.Default.Save(); // Enregistre les paramètres
            testFrm.UpdateSite(); // Met à jour le label dans la fenêtre "Paramètres"
            Close(); // Ferme la fenêtre
        }

        private void gunaGradientTileButton2_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.TestSite = "https://google.com"; // Met à jour le paramètre
            Properties.Settings.Default.Save(); // Enregistre les paramètres
            testFrm.UpdateSite(); // Met à jour le label dans la fenêtre "Paramètres"
            Close(); // Ferme la fenêtre
        }

        private void gunaGradientTileButton4_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.TestSite = "https://twitter.com"; // Met à jour le paramètre
            Properties.Settings.Default.Save(); // Enregistre les paramètres
            testFrm.UpdateSite(); // Met à jour le label dans la fenêtre "Paramètres"
            Close(); // Ferme la fenêtre
        }

        private void gunaGradientTileButton3_Click(object sender, EventArgs e)
        {
            new SelectCustomDefaultSite(testFrm).Show(); // Site personnalisé
            Close(); // Ferme la fenêtre
        }

        private void gunaAdvenceTileButton1_Click(object sender, EventArgs e)
        {
            Close(); // Ferme la fenêtre
        }
    }
}
