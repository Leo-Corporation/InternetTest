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
    public partial class SelectCustomDefaultSite : Form
    {
        Test testFrm;
        public SelectCustomDefaultSite(Test test)
        {
            if (new Language().GetCode() == "fr-FR")
            {
                Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("fr-FR");
            }
            else if (new Language().GetCode() == "EN")
            {
                Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            }
            testFrm = test;
            InitializeComponent();
        }

        private void SelectCustomDefaultSite_Load(object sender, EventArgs e)
        {
            Icon = new Branches().IconBranch(); // Met l'icône en foncion de la branche
            gunaPictureBox1.Image = new Branches().ImageBranch(); // Met l'image en fonction de la branche
            ChangeTheme(); // Changer le thème à celui qui est sélectionné
            Guna.UI.Lib.GraphicsHelper.ShadowForm(this); // Ajouter une ombre sur la fenêtre
            gunaComboBox1.Text = "https://"; // Chnager le texte de la comboBox
        }

        private void gunaAdvenceTileButton1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ChangeTheme() // Change le thème
        {
            if (new Classes.Theme().IsDark()) // Si le thème est sombre
            {
                BackColor = Color.FromArgb(50, 50, 72);
                gunaLabel1.ForeColor = Color.White;
                gunaLabel2.ForeColor = Color.White;
                gunaAdvenceTileButton1.Image = Properties.Resources.icons8_delete_32px;
                gunaComboBox1.BaseColor = Color.FromArgb(50, 50, 72);
                gunaLineTextBox1.BackColor = Color.FromArgb(50, 50, 72);
            }
            else // Si le thème est clair
            {
                BackColor = Color.White;
                gunaLabel1.ForeColor = Color.Black;
                gunaLabel2.ForeColor = Color.Black;
                gunaAdvenceTileButton1.Image = Properties.Resources.icons8_delete_100px_1;
                gunaComboBox1.BaseColor = Color.White;
                gunaLineTextBox1.BackColor = Color.White;
            }
        }

        private void gunaGradientButton1_Click(object sender, EventArgs e)
        {
            SaveSite(gunaComboBox1.Text, gunaLineTextBox1.Text); // Sauvegarder le site
            Close(); // Ferme la fenêtre
        }

        private void SaveSite(string str, string url)
        {
            if (url.Contains("https://"))
            {
                string newUrl = url.Replace("https://", "");
                Properties.Settings.Default.TestSite = str + newUrl;
            }
            else if (url.Contains("http://"))
            {
                string newUrl = url.Replace("http://", "");
                Properties.Settings.Default.TestSite = str + newUrl;
            }
            else
            {
                Properties.Settings.Default.TestSite = str + url;
            }
            Properties.Settings.Default.Save();
            testFrm.UpdateSite();
        }
    }
}