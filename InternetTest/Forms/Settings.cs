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
            ChangePage(SettingsPage.Theme);
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            Icon = new Branches().IconBranch(); // Met l'icône en foncion de la branche
            gunaPictureBox1.Image = new Branches().ImageBranch(); // Met l'image en fonction de la branche
            Guna.UI.Lib.GraphicsHelper.ShadowForm(this);
            ChangeTheme();
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

            form1.ChangeTheme();
            Close();
        }

        private void ChangeTheme()
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
            switch (settingsPage)
            {
                case SettingsPage.Data:
                    data1.Visible = true; // Show
                    languages1.Visible = false; // Hide
                    test1.Visible = false; // Hide
                    theme1.Visible = false; // Hide
                    break;
                case SettingsPage.Theme:
                    data1.Visible = false; // Hide
                    languages1.Visible = false; // Hide
                    test1.Visible = false; // Hide
                    theme1.Visible = true; // Show
                    break;
                case SettingsPage.Test:
                    data1.Visible = false; // Hide
                    languages1.Visible = false; // Hide
                    test1.Visible = true; // Show
                    theme1.Visible = false; // Hide
                    break;
                case SettingsPage.Language:
                    data1.Visible = false; // Hide
                    languages1.Visible = true; // Show
                    test1.Visible = false; // Hide
                    theme1.Visible = false; // Hide
                    break;
            }
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
