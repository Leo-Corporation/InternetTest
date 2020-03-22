using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using LeoCorpLibrary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InternetTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Guna.UI.Lib.GraphicsHelper.ShadowForm(this);
        }

        private void gunaGradientButton1_Click(object sender, EventArgs e)
        {
            bool connectionAvailable = new NetworkConnection().IsAvailable();
            if (connectionAvailable) // Si internet est disponible
            {
                MessageBox.Show("Internet est disponible");
                //TODO
            }
            else
            {
                //TODO
            }
        }
    }
}
