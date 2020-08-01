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
using System.Runtime;

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
            gunaLabel11.Visible = false;
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
            Properties.Settings.Default.IsThemeDark = gunaWinSwitch1.Checked; // Mettre à jour le paramètre
            Properties.Settings.Default.NotifyUpdate = gunaWinSwitch2.Checked; // Mettre à jour le paramètre
            Properties.Settings.Default.TestOnStart = gunaWinSwitch3.Checked; // Mettre à jour le paramètre
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
            Properties.Settings.Default.MapsProvider = gunaComboBox2.Text; // Mettre à jour le paramètre
            Properties.Settings.Default.Save(); // Sauvegarder
        }

        private void LoadSettings() // Charger les paramètres et mettre à jour l'interface
        {
            gunaWinSwitch1.Checked = Properties.Settings.Default.IsThemeDark;
            gunaWinSwitch2.Checked = Properties.Settings.Default.NotifyUpdate;
            gunaWinSwitch3.Checked = Properties.Settings.Default.TestOnStart;
            if (Properties.Settings.Default.Language == "fr-FR")
            {
                gunaComboBox1.Text = new Language().GetName("fr");
            }
            else if (Properties.Settings.Default.Language == "EN")
            {
                gunaComboBox1.Text = new Language().GetName("en");
            }
            gunaComboBox2.Text = Properties.Settings.Default.MapsProvider; // Mettre à jour le texte
            gunaLabel14.Text = Properties.Settings.Default.TestSite;
            gunaLinkLabel2.Location = new Point(gunaLabel14.Width + 15, 288);
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
                gunaLabel11.ForeColor = Color.White;
                gunaLabel12.ForeColor = Color.White;
                gunaLabel13.ForeColor = Color.White;
                gunaLabel14.ForeColor = Color.White;
                gunaLabel15.ForeColor = Color.White;
                gunaLabel16.ForeColor = Color.White;
                gunaLabel17.ForeColor = Color.White;
                gunaLabel18.ForeColor = Color.White;
                gunaLabel19.ForeColor = Color.White;
                gunaComboBox1.ForeColor = Color.White;
                gunaComboBox2.ForeColor = Color.White;
                gunaComboBox1.BaseColor = Color.FromArgb(50, 50, 72);
                gunaComboBox2.BaseColor = Color.FromArgb(50, 50, 72);
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
                gunaLabel11.ForeColor = Color.Black;
                gunaLabel12.ForeColor = Color.Black;
                gunaLabel13.ForeColor = Color.Black;
                gunaLabel14.ForeColor = Color.Black;
                gunaLabel15.ForeColor = Color.Black;
                gunaLabel16.ForeColor = Color.Black;
                gunaLabel17.ForeColor = Color.Black;
                gunaLabel18.ForeColor = Color.Black;
                gunaLabel19.ForeColor = Color.Black;
                gunaComboBox1.ForeColor = Color.Black;
                gunaComboBox2.ForeColor = Color.Black;
                gunaComboBox1.BaseColor = Color.White;
                gunaComboBox2.BaseColor = Color.White;
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
            UpdateSite();
            if (gunaComboBox1.Text.Contains("fr-FR"))
            {
                gunaComboBox1.Text = "Français (fr-FR)";
                gunaWinSwitch1.Checked = false;
                gunaWinSwitch2.Checked = true;
                MessageBox.Show("Les données ont été effacées.", "Terminé", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (gunaComboBox1.Text.Contains("EN"))
            {
                gunaComboBox1.Text = "French (fr-FR)";
                gunaWinSwitch1.Checked = false;
                gunaWinSwitch2.Checked = true;
                MessageBox.Show("Data has been removed.", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            gunaComboBox2.Text = Properties.Settings.Default.MapsProvider; // Mettre à jour le texte
        }

        private void gunaComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (new Language().GetCode() == "fr-FR")
            {
                if (gunaComboBox1.Text != new Language().GetName("fr"))
                {
                    gunaLabel11.Visible = true;
                }
            }else if (new Language().GetCode() == "EN")
            {
                if (gunaComboBox1.Text != new Language().GetName("en"))
                {
                    gunaLabel11.Visible = true;
                }
            }
        }

        private void gunaLinkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new SelectDefaultSite(this).Show();
        }

        public void UpdateSite()
        {
            gunaLabel14.Text = Properties.Settings.Default.TestSite;
            gunaLinkLabel2.Location = new Point(gunaLabel14.Width + 15, 288);
        }

        private void gunaWinSwitch3_CheckedChanged(object sender, EventArgs e)
        {
            if (new Language().GetCode() == "fr-FR") // Si la langue est française
            {
                if (gunaWinSwitch3.Checked)
                {
                    gunaLabel17.Text = "Activé";
                }
                else
                {
                    gunaLabel17.Text = "Désactivé";
                }
            }
            else if (new Language().GetCode() == "EN") // Si la langue est anglaise
            {
                if (gunaWinSwitch3.Checked)
                {
                    gunaLabel17.Text = "Enabled";
                }
                else
                {
                    gunaLabel17.Text = "Disabled";
                }
            }
        }
    }
}
