using InternetTest.Classes;
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

namespace InternetTest.UserControls.SettingsPages
{
    public partial class Languages : UserControl
    {
        public Languages()
        {
            InitializeComponent();
            LoadSettings(); // Load the settings
            ChangeTheme(); // Change the theme
        }

        internal void ChangeTheme()
        {
            if (Classes.Theme.IsDark()) // If the theme is dark
            {
                gunaLabel8.ForeColor = Color.White; // Change the ForeColor
                gunaLabel9.ForeColor = Color.White; // Change the ForeColor
                gunaLabel11.ForeColor = Color.White; // Change the ForeColor
                BackColor = Color.FromArgb(50, 50, 72); // Change the BackColor
                gunaComboBox1.BaseColor = Color.FromArgb(50, 50, 72); // Change the base color
                gunaComboBox1.ForeColor = Color.White; // Change the ForeColor
            }
            else
            {
                gunaLabel8.ForeColor = Color.Black; // Change the ForeColor
                gunaLabel9.ForeColor = Color.Black; // Change the ForeColor
                gunaLabel11.ForeColor = Color.Black; // Change the ForeColor
                BackColor = Color.White; // Change the BackColor
                gunaComboBox1.BaseColor = Color.White; // Change the base color
                gunaComboBox1.ForeColor = Color.Black; // Change the ForeColor
            }
        }

        private void gunaComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (new Language().GetCode() == "fr-FR")
            {
                if (gunaComboBox1.Text != new Language().GetName("fr"))
                {
                    gunaLabel11.Visible = true;
                }
            }
            else if (new Language().GetCode() == "EN")
            {
                if (gunaComboBox1.Text != new Language().GetName("en"))
                {
                    gunaLabel11.Visible = true;
                }
            }
        }

        private void gunaGradientButton1_Click(object sender, EventArgs e)
        {
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
            Properties.Settings.Default.Save(); // Save
        }

        private void LoadSettings()
        {
            gunaLabel11.Visible = false; // Hide
            if (Properties.Settings.Default.Language == "fr-FR")
            {
                gunaComboBox1.Text = new Language().GetName("fr");
            }
            else if (Properties.Settings.Default.Language == "EN")
            {
                gunaComboBox1.Text = new Language().GetName("en");
            }
        }
    }
}
