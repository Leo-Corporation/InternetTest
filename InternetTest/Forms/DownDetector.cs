using InternetTest.Classes;
using LeoCorpLibrary;
using LeoCorpLibrary.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InternetTest.Forms
{
    public partial class DownDetector : Form
    {
        public DownDetector()
        {
            InitializeComponent();
            Icon = new Branches().IconBranch(); // Icon
            gunaPictureBox1.Image = new Branches().ImageBranch(); // Image
            ChangeTheme(); // Change the window theme
        }

        private async void gunaGradientButton1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(gunaLineTextBox1.Text) && !string.IsNullOrWhiteSpace(gunaLineTextBox1.Text)) // If the textbox isn't null
            {
                if (!gunaLineTextBox1.Text.Contains("https://") && !gunaLineTextBox1.Text.Contains("http://")) // If the box doesn't contains https or http
                {
                    gunaLineTextBox1.Text = "https://" + gunaLineTextBox1.Text; // Add the https
                }

                if (await NetworkConnection.IsAvailableTestSiteAsync(gunaLineTextBox1.Text)) // Check if the site is available
                {
                    gunaLabel2.Text = Language.WebSiteNotDownMessage; // Set the text
                    WinFormsHelpers.CenterControlOnForm(gunaLabel2, this, ControlAlignement.Horizontal); // Center
                    gunaPictureBox2.Image = Properties.Resources.check; // Set the image
                }
                else
                {
                    gunaLabel2.Text = Language.WebSiteDownMessage; // Set the text
                    WinFormsHelpers.CenterControlOnForm(gunaLabel2, this, ControlAlignement.Horizontal); // Center
                    gunaPictureBox2.Image = Properties.Resources.cancel; // Set the image
                }
            }
        }

        private void ChangeTheme()
        {
            if (Theme.IsDark())
            {
                BackColor = Color.FromArgb(50, 50, 72); // BackColor
                gunaLabel1.ForeColor = Color.White; // ForeColor
                gunaLabel2.ForeColor = Color.White; // ForeColor
                gunaLabel3.ForeColor = Color.White; // ForeColor
                gunaLineTextBox1.ForeColor = Color.White; // ForeColor
                gunaLineTextBox1.BackColor = Color.FromArgb(50, 50, 72); // BackColor
                gunaPictureBox2.Image = Properties.Resources.network_test; // Image
                gunaAdvenceTileButton1.Image = Properties.Resources.icons8_delete_32px; // Change image
                gunaAdvenceTileButton2.Image = Properties.Resources.icons8_subtract_100px; // Change image
            }
            else
            {
                BackColor = Color.White; // BackColor
                gunaLabel1.ForeColor = Color.Black; // ForeColor
                gunaLabel2.ForeColor = Color.Black; // ForeColor
                gunaLabel3.ForeColor = Color.Black; // ForeColor
                gunaLineTextBox1.ForeColor = Color.Black; // ForeColor
                gunaLineTextBox1.BackColor = Color.White; // BackColor
                gunaPictureBox2.Image = Properties.Resources.network_test_black; // Image
                gunaAdvenceTileButton1.Image = Properties.Resources.icons8_delete_100px_1; // Change image
                gunaAdvenceTileButton2.Image = Properties.Resources.icons8_subtract_100px_1; // Change image
            }
        }

        private void gunaAdvenceTileButton2_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized; // Change the WindowState
        }

        private void gunaAdvenceTileButton1_Click(object sender, EventArgs e)
        {
            Close(); // Close the window
        }
    }
}
