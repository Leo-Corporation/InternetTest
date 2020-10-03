using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using InternetTest.Forms;
using InternetTest.Classes;

namespace InternetTest.UserControls
{
    public partial class Menu : UserControl
    {
        public Menu()
        {
            InitializeComponent();
            ChangeLanguage();
            ChangeTheme();
        }

        private void gunaGradientButton3_Click(object sender, EventArgs e)
        {
            new LocalizeIP().Show(); // Open the form
            Hide(); // Hide the menu
        }

        private void gunaGradientButton1_Click(object sender, EventArgs e)
        {
            new SitesSelector().Show(); // Open the form
            Hide(); // Hide the menu
        }

        private void ChangeLanguage()
        {
            switch (new Language().GetCode())
            {
                case "fr-FR": // If the langiuage is french
                    gunaGradientButton3.Text = "Localiser IP"; // Change text
                    gunaGradientButton1.Text = "Tester dans un navigateur"; // Change text

                    Width = 232; // Change width
                    break;
                case "EN": // If the language is english
                    gunaGradientButton3.Text = "Localize IP"; // Change text
                    gunaGradientButton1.Text = "Test in a browser"; // Change text

                    Width = 182; // Change width
                    break;
            }
        }

        internal void ChangeTheme()
        {
            if (new Theme().IsDark()) // If the dark theme is on
            {
                BackColor = Color.FromArgb(50, 50, 72); // Change the color

                gunaGradientButton1.BaseColor1 = Color.FromArgb(50, 50, 72); // Change the base color
                gunaGradientButton1.BaseColor2 = Color.FromArgb(50, 50, 72); // Change the base color
                gunaGradientButton3.BaseColor1 = Color.FromArgb(50, 50, 72); // Change the base color
                gunaGradientButton3.BaseColor2 = Color.FromArgb(50, 50, 72); // Change the base color

                gunaGradientButton1.ForeColor = Color.White; // Change the Forecolor
                gunaGradientButton3.ForeColor = Color.White; // Change the Forecolor

                gunaGradientButton1.Image = Properties.Resources.globe; // Change image
                gunaGradientButton3.Image = Properties.Resources.location; // Change image
            }
            else // If the light theme is on
            {
                BackColor = Color.White; // Change the color

                gunaGradientButton1.BaseColor1 = Color.White; // Change the base color
                gunaGradientButton1.BaseColor2 = Color.White; // Change the base color
                gunaGradientButton3.BaseColor1 = Color.White; // Change the base color
                gunaGradientButton3.BaseColor2 = Color.White; // Change the base color

                gunaGradientButton1.ForeColor = Color.Black; // Change the Forecolor
                gunaGradientButton3.ForeColor = Color.Black; // Change the Forecolor

                gunaGradientButton1.Image = Properties.Resources.globe_black; // Change image
                gunaGradientButton3.Image = Properties.Resources.localize_black; // Change image
            }
        }

        private void Menu_Load(object sender, EventArgs e)
        {
            ChangeTheme();
        }
    }
}
