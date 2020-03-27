using InternetTest.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace InternetTest.Forms
{
    public partial class Settings : Form
    {
        Form1 form1;
        public Settings(Form1 frm)
        {
            InitializeComponent();
            form1 = frm;
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            Icon = new Branches().IconBranch(); // Met l'icône en foncion de la branche
            gunaPictureBox1.Image = new Branches().ImageBranch(); // Met l'image en fonction de la branche
            Guna.UI.Lib.GraphicsHelper.ShadowForm(this);
            ChangeTheme();
            LoadSettings();
        }

        private void gunaGradientButton2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void gunaAdvenceTileButton1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void gunaAdvenceTileButton2_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized; // Minimiser la fenêtre
        }

        private void gunaGradientButton1_Click(object sender, EventArgs e)
        {
            ChangeSettings();
            form1.ChangeTheme();
            Close();
        }

        private void ChangeSettings() // Appliquer les changements
        {
            if (gunaWinSwitch1.Checked) // Utiliser thème sombre est activé
            {
                Properties.Settings.Default.IsThemeDark = true;
            }
            else
            {
                Properties.Settings.Default.IsThemeDark = false;
            }
            if (gunaWinSwitch2.Checked) // Notifier des mises à jour est activé
            {
                Properties.Settings.Default.NotifyUpdate = true;
            }
            else
            {
                Properties.Settings.Default.NotifyUpdate = false;
            }
            if (gunaComboBox1.Text.Contains("fr-FR")) // Si la langue est française
            {
                Properties.Settings.Default.Language = "fr-FR";
                Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("fr-FR");
            }
            else if (gunaComboBox1.Text.Contains("EN")) // Si la langue est anglaise
            {
                Properties.Settings.Default.Language = "EN";
                Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            }
            Properties.Settings.Default.Save();
        }

        private void LoadSettings() // Charger les paramètres et mettre à jour l'interface
        {
            gunaWinSwitch1.Checked = Properties.Settings.Default.IsThemeDark;
            gunaWinSwitch2.Checked = Properties.Settings.Default.NotifyUpdate;
            if (Properties.Settings.Default.Language == "fr-FR")
            {
                gunaComboBox1.Text = new Language().GetName("fr");
            }
            else if (Properties.Settings.Default.Language == "EN")
            {
                gunaComboBox1.Text = new Language().GetName("en");
            }
        }

        private void ChangeTheme()
        {
            if (Properties.Settings.Default.IsThemeDark) // Si le thème est sombre
            {
                BackColor = Color.FromArgb(50, 50, 72);
                gunaLabel1.ForeColor = Color.White;
                gunaLabel2.ForeColor = Color.White;
                gunaLabel3.ForeColor = Color.White;
                gunaLabel4.ForeColor = Color.White;
                gunaLabel5.ForeColor = Color.White;
                gunaLabel6.ForeColor = Color.White;
                gunaLabel7.ForeColor = Color.White;
                gunaLabel8.ForeColor = Color.White;
                gunaLabel9.ForeColor = Color.White;
                gunaLabel10.ForeColor = Color.White;
                gunaComboBox1.ForeColor = Color.White;
                gunaComboBox1.BaseColor = Color.FromArgb(50, 50, 72);
                gunaAdvenceTileButton1.Image = Properties.Resources.icons8_delete_32px;
                gunaAdvenceTileButton2.Image = Properties.Resources.icons8_subtract_100px;
            }
            else
            {
                BackColor = Color.White;
                gunaLabel1.ForeColor = Color.Black;
                gunaLabel2.ForeColor = Color.Black;
                gunaLabel3.ForeColor = Color.Black;
                gunaLabel4.ForeColor = Color.Black;
                gunaLabel5.ForeColor = Color.Black;
                gunaLabel6.ForeColor = Color.Black;
                gunaLabel7.ForeColor = Color.Black;
                gunaLabel8.ForeColor = Color.Black;
                gunaLabel9.ForeColor = Color.Black;
                gunaLabel10.ForeColor = Color.Black;
                gunaComboBox1.ForeColor = Color.Black;
                gunaComboBox1.BaseColor = Color.White;
                gunaAdvenceTileButton1.Image = Properties.Resources.icons8_delete_100px_1;
                gunaAdvenceTileButton2.Image = Properties.Resources.icons8_subtract_100px_1;
            }
        }

        private void gunaWinSwitch1_CheckedChanged(object sender, EventArgs e)
        {
            if (new Language().GetCode() == "fr-FR")
            {
                if (gunaWinSwitch1.Checked)
                {
                    gunaLabel4.Text = "Activé";
                }
                else
                {
                    gunaLabel4.Text = "Désactivé";
                }
            }
            else if (new Language().GetCode() == "EN")
            {
                if (gunaWinSwitch1.Checked)
                {
                    gunaLabel4.Text = "Enabled";
                }
                else
                {
                    gunaLabel4.Text = "Disabled";
                }
            }
        }

        private void gunaWinSwitch2_CheckedChanged(object sender, EventArgs e)
        {
            if (new Language().GetCode() == "fr-FR") // Si la langue est française
            {
                if (gunaWinSwitch2.Checked)
                {
                    gunaLabel7.Text = "Activé";
                }
                else
                {
                    gunaLabel7.Text = "Désactivé";
                }
            }
            else if (new Language().GetCode() == "EN") // Si la langue est anglaise
            {
                if (gunaWinSwitch2.Checked)
                {
                    gunaLabel7.Text = "Enabled";
                }
                else
                {
                    gunaLabel7.Text = "Disabled";
                }
            }
        }

        private void gunaLinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Properties.Settings.Default.Reset();
            Properties.Settings.Default.Save();
            if (new Language().GetCode() == "fr-FR")
            {
                gunaComboBox1.Text = "Français (fr-FR)";
                gunaWinSwitch1.Checked = true;
                gunaWinSwitch2.Checked = false;
                MessageBox.Show("Terminé !");
            }
            else if (new Language().GetCode() == "EN")
            {
                gunaComboBox1.Text = "French (fr-FR)";
                gunaWinSwitch1.Checked = true;
                gunaWinSwitch2.Checked = false;
                MessageBox.Show("Done !");
            }
        }
    }
}
