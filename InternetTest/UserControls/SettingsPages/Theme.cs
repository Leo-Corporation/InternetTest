using InternetTest.Classes;
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
    public partial class Theme : UserControl
    {
        public Theme()
        {
            InitializeComponent();
            LoadSettings();
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

        private void LoadSettings()
        {
            gunaWinSwitch1.Checked = Properties.Settings.Default.IsThemeDark;
        }

        private void gunaGradientButton1_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.IsThemeDark = gunaWinSwitch1.Checked; // Set
            Properties.Settings.Default.Save(); // Save
        }
    }
}
