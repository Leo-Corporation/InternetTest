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
    public partial class Data : UserControl
    {
        public Data()
        {
            InitializeComponent();
            ChangeTheme();
        }

        internal void ChangeTheme()
        {
            if (Classes.Theme.IsDark()) // If the theme is dark
            {
                gunaLabel10.ForeColor = Color.White; // Change the ForeColor
                BackColor = Color.FromArgb(50, 50, 72); // Change the BackColor
            }
            else
            {
                gunaLabel10.ForeColor = Color.Black; // Change the ForeColor
                BackColor = Color.White; // Change the BackColor
            }
        }

        private void gunaLinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string promptBeforeRemove = "";

            if (new Language().GetCode() == "fr-FR") // If the lang is french
            {
                promptBeforeRemove = "Êtes-vous sûr de vouloir effacer toutes les données d'InternetTest ?";
            }
            else if (new Language().GetCode() == "EN") // If the lang is english
            {
                promptBeforeRemove = "Are you sure you wanna erase all InternetTest's data?";
            }

            if (MessageBox.Show(promptBeforeRemove, "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Properties.Settings.Default.Reset();
                Properties.Settings.Default.Save();
                if (new Language().GetCode() == "fr-FR")
                {
                    
                    MessageBox.Show("Les données ont été effacées.", "Terminé", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (new Language().GetCode() == "EN")
                {
                   
                    MessageBox.Show("Data has been removed.", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
    }
}
