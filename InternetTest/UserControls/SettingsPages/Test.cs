using InternetTest.Classes;
using InternetTest.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InternetTest.UserControls.SettingsPages
{
    public partial class Test : UserControl
    {
        public Test()
        {
            InitializeComponent();
            LoadSettings();
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

        private void gunaWinSwitch2_CheckedChanged(object sender, EventArgs e)
        {
            if (new Language().GetCode() == "fr-FR") // Si la langue est française
            {
                if (gunaWinSwitch2.Checked)
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
                if (gunaWinSwitch2.Checked)
                {
                    gunaLabel17.Text = "Enabled";
                }
                else
                {
                    gunaLabel17.Text = "Disabled";
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
            gunaLinkLabel2.Location = new Point(gunaLabel14.Width + 15, 116);
        }

        private void LoadSettings()
        {
            gunaWinSwitch2.Checked = Properties.Settings.Default.NotifyUpdate;
            gunaWinSwitch3.Checked = Properties.Settings.Default.TestOnStart;

            gunaLabel14.Text = Properties.Settings.Default.TestSite;
            gunaLinkLabel2.Location = new Point(gunaLabel14.Width + 15, 116);

            gunaComboBox2.Text = Properties.Settings.Default.MapsProvider;
        }

        private void gunaGradientButton1_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.MapsProvider = gunaComboBox2.Text;
            Properties.Settings.Default.NotifyUpdate = gunaWinSwitch2.Checked; // Update
            Properties.Settings.Default.TestOnStart = gunaWinSwitch3.Checked; // Update

            Properties.Settings.Default.Save(); // Save
        }
    }
}
