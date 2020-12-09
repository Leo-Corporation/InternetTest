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
        public Settings()
        {
            InitializeComponent();
            ChangePage(SettingsPage.Theme);
            Definitions.Settings = this;
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            Icon = new Branches().IconBranch(); // Met l'icône en foncion de la branche
            gunaPictureBox1.Image = new Branches().ImageBranch(); // Met l'image en fonction de la branche
            Guna.UI.Lib.GraphicsHelper.ShadowForm(this);
            ChangeTheme();
        }

        internal void LoadSettings()
        {
            test1.LoadSettings(); // Load
            theme1.LoadSettings(); // Load
            languages1.LoadSettings(); // Load
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

        internal void ChangeTheme()
        {
            if (Properties.Settings.Default.IsThemeDark) // Si le thème est sombre
            {
                BackColor = Color.FromArgb(50, 50, 72);
                gunaLabel1.ForeColor = Color.White;
                gunaAdvenceTileButton1.Image = Properties.Resources.icons8_delete_32px;
                gunaAdvenceTileButton2.Image = Properties.Resources.icons8_subtract_100px;
            }
            else
            {
                BackColor = Color.White;
                gunaLabel1.ForeColor = Color.Black;
                gunaAdvenceTileButton1.Image = Properties.Resources.icons8_delete_100px_1;
                gunaAdvenceTileButton2.Image = Properties.Resources.icons8_subtract_100px_1;
            }
            data1.ChangeTheme(); // Change theme
            languages1.ChangeTheme(); // Change theme
            test1.ChangeTheme(); // Change theme
            theme1.ChangeTheme(); // Change theme
            ChangePage(SettingsPage.Theme);
        }

        private void gunaGradientButton3_Click(object sender, EventArgs e)
        {
            ChangePage(SettingsPage.Theme); // Change page
        }

        private void gunaGradientButton4_Click(object sender, EventArgs e)
        {
            ChangePage(SettingsPage.Language); // Change page
        }

        private void gunaGradientButton5_Click(object sender, EventArgs e)
        {
            ChangePage(SettingsPage.Test); // Change page
        }

        private void gunaGradientButton6_Click(object sender, EventArgs e)
        {
            ChangePage(SettingsPage.Data); // Change page
        }

        private void ChangePage(SettingsPage settingsPage)
        {
            UnCheckAll(); // Uncheck all buttons
            switch (settingsPage)
            {
                case SettingsPage.Data:
                    data1.Visible = true; // Show
                    languages1.Visible = false; // Hide
                    test1.Visible = false; // Hide
                    theme1.Visible = false; // Hide

                    gunaGradientButton6.BaseColor1 = Color.DeepSkyBlue; // Change BaseColor1
                    gunaGradientButton6.BaseColor2 = Color.RoyalBlue; // Change BaseColor2

                    gunaGradientButton6.ForeColor = Color.White; // Change the ForeColor
                    break;
                case SettingsPage.Theme:
                    data1.Visible = false; // Hide
                    languages1.Visible = false; // Hide
                    test1.Visible = false; // Hide
                    theme1.Visible = true; // Show

                    gunaGradientButton3.BaseColor1 = Color.DeepSkyBlue; // Change BaseColor1
                    gunaGradientButton3.BaseColor2 = Color.RoyalBlue; // Change BaseColor2

                    gunaGradientButton3.ForeColor = Color.White; // Change the ForeColor
                    break;
                case SettingsPage.Test:
                    data1.Visible = false; // Hide
                    languages1.Visible = false; // Hide
                    test1.Visible = true; // Show
                    theme1.Visible = false; // Hide

                    gunaGradientButton5.BaseColor1 = Color.DeepSkyBlue; // Change BaseColor1
                    gunaGradientButton5.BaseColor2 = Color.RoyalBlue; // Change BaseColor2

                    gunaGradientButton5.ForeColor = Color.White; // Change the ForeColor
                    break;
                case SettingsPage.Language:
                    data1.Visible = false; // Hide
                    languages1.Visible = true; // Show
                    test1.Visible = false; // Hide
                    theme1.Visible = false; // Hide

                    gunaGradientButton4.BaseColor1 = Color.DeepSkyBlue; // Change BaseColor1
                    gunaGradientButton4.BaseColor2 = Color.RoyalBlue; // Change BaseColor2

                    gunaGradientButton4.ForeColor = Color.White; // Change the ForeColor
                    break;
            }
        }

        private void UnCheckAll()
        {
            gunaGradientButton3.BaseColor1 = !Theme.IsDark() ? Color.White : Color.FromArgb(50, 50, 72); // Change BaseColor1
            gunaGradientButton3.BaseColor2 = !Theme.IsDark() ? Color.White : Color.FromArgb(50, 50, 72); // Change BaseColor2

            gunaGradientButton4.BaseColor1 = !Theme.IsDark() ? Color.White : Color.FromArgb(50, 50, 72); // Change BaseColor1
            gunaGradientButton4.BaseColor2 = !Theme.IsDark() ? Color.White : Color.FromArgb(50, 50, 72); // Change BaseColor2

            gunaGradientButton5.BaseColor1 = !Theme.IsDark() ? Color.White : Color.FromArgb(50, 50, 72); // Change BaseColor1
            gunaGradientButton5.BaseColor2 = !Theme.IsDark() ? Color.White : Color.FromArgb(50, 50, 72); // Change BaseColor2

            gunaGradientButton6.BaseColor1 = !Theme.IsDark() ? Color.White : Color.FromArgb(50, 50, 72); // Change BaseColor1
            gunaGradientButton6.BaseColor2 = !Theme.IsDark() ? Color.White : Color.FromArgb(50, 50, 72); // Change BaseColor2

            gunaGradientButton3.ForeColor = !Theme.IsDark() ? Color.Black : Color.White; // Change ForeColor
            gunaGradientButton4.ForeColor = !Theme.IsDark() ? Color.Black : Color.White; // Change ForeColor
            gunaGradientButton5.ForeColor = !Theme.IsDark() ? Color.Black : Color.White; // Change ForeColor
            gunaGradientButton6.ForeColor = !Theme.IsDark() ? Color.Black : Color.White; // Change ForeColor
        }

        enum SettingsPage
        {
            Theme,
            Language,
            Test,
            Data
        }
    }
}
