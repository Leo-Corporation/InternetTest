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

namespace InternetTest.Forms
{
    public partial class SelectDefaultSite : Form
    {
        public SelectDefaultSite()
        {
            InitializeComponent();
        }

        private void SelectDefaultSite_Load(object sender, EventArgs e)
        {
            Icon = new Branches().IconBranch(); // Met l'icône en foncion de la branche
            gunaPictureBox1.Image = new Branches().ImageBranch(); // Met l'image en fonction de la branche
            ChangeTheme();
            Guna.UI.Lib.GraphicsHelper.ShadowForm(this);
        }

        private void ChangeTheme()
        {
            if (new Theme().IsDark())
            {
                BackColor = Color.FromArgb(50, 50, 72);
                gunaLabel1.ForeColor = Color.White;
                gunaAdvenceTileButton1.Image = Properties.Resources.icons8_delete_32px;
            }
            else
            {
                BackColor = Color.White;
                gunaLabel1.ForeColor = Color.Black;
                gunaAdvenceTileButton1.Image = Properties.Resources.icons8_delete_100px_1;
            }
        }

        private void gunaGradientTileButton1_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.TestSite = "https://bing.com";
            Properties.Settings.Default.Save();
            Close();
        }

        private void gunaGradientTileButton2_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.TestSite = "https://google.com";
            Properties.Settings.Default.Save();
            Close();
        }

        private void gunaGradientTileButton4_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.TestSite = "https://twitter.com";
            Properties.Settings.Default.Save();
            Close();
        }

        private void gunaGradientTileButton3_Click(object sender, EventArgs e)
        {
            //TODO
        }

        private void gunaAdvenceTileButton1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
