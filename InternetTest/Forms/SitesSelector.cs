using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using InternetTest.Classes;

namespace InternetTest.Forms
{
    public partial class SitesSelector : Form
    {
        public SitesSelector()
        {
            InitializeComponent();
        }

        private void SitesSelector_Load(object sender, EventArgs e)
        {
            Icon = new Branches().IconBranch(); // Met l'icône en foncion de la branche
            gunaPictureBox1.Image = new Branches().ImageBranch(); // Met l'image en fonction de la branche
            Guna.UI.Lib.GraphicsHelper.ShadowForm(this);
        }

        private void gunaGradientTileButton1_Click(object sender, EventArgs e)
        {
            new Browser("https://bing.com").Show();
            Close();
        }

        private void gunaGradientTileButton2_Click(object sender, EventArgs e)
        {
            new Browser("https://google.com").Show();
            Close();
        }

        private void gunaGradientTileButton4_Click(object sender, EventArgs e)
        {
            new Browser("https://twitter.com").Show();
            Close();
        }

        private void gunaGradientTileButton3_Click(object sender, EventArgs e)
        {
            new Browser("https://youtube.com").Show();
            Close();
        }

        private void gunaAdvenceTileButton1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
