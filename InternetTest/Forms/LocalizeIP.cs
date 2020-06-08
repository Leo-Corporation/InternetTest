using InternetTest.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace InternetTest.Forms
{
    public partial class LocalizeIP : Form
    {
        public LocalizeIP()
        {
            InitializeComponent();
        }

        private void gunaGradientButton5_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(gunaLineTextBox1.Text) && !string.IsNullOrWhiteSpace(gunaLineTextBox1.Text)) // Si la textbox n'est pas vide
            {
                try // Essayer
                {
                    gunaLabel2.Visible = false; // Cacher
                    gunaLabel3.Text = string.Empty; // Effacer
                    GetIPInfo(gunaLineTextBox1.Text); // Localiser IP
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erreur :\n" + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error); // Erreur
                }
            }
        }

        private void GetIPInfo(string ip) // Localiser une IP
        {
            string[] nodes = new string[] // Informations à obtenir
            {
                "country",
                "regionName",
                "city",
                "zip",
                "lat",
                "lon",
                "timezone",
                "isp"
            };

            string[] NameFR = new string[] // Noms en français
            {
                "Pays : ",
                "Region : ",
                "Ville : ",
                "Code postal : ",
                "Latitude : ",
                "Longitude : ",
                "Timezone : ",
                "FAI : "
            };

            for (int i = 0; i < nodes.Length; i++) // Obtenir les infos
            {
                // Ouvrir document XML de puis l'API
                XmlTextReader xmlTextReader = new XmlTextReader(string.Format("http://ip-api.com/xml/{0}?lang=fr", ip));
                while (xmlTextReader.Read())
                {
                    // Exemple si i = 0 (pays), vérifier  si le node "country" existe
                    if (xmlTextReader.NodeType == XmlNodeType.Element && xmlTextReader.Name == nodes[i])
                    {
                        gunaLabel3.Text += NameFR[i] + xmlTextReader.ReadElementContentAsString() + Environment.NewLine;
                    }
                }
            }
        }

        private void LocalizeIP_Load(object sender, EventArgs e)
        {
            Icon = new Branches().IconBranch(); // Met l'icône en foncion de la branche
            gunaPictureBox1.Image = new Branches().ImageBranch(); // Met l'image en fonction de la branche
            Guna.UI.Lib.GraphicsHelper.ShadowForm(this);
        }

        private void gunaAdvenceTileButton2_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized; // Minimiser la fenêtre
        }

        private void gunaAdvenceTileButton1_Click(object sender, EventArgs e)
        {
            Close(); // Fermer la fenêtre
        }

        private void gunaGradientButton1_Click(object sender, EventArgs e)
        {
            gunaLabel3.Text = string.Empty; // Effacer
            try // Essayer
            {
                gunaLabel2.Visible = false; // Cacher
                GetIPInfo(""); // Localiser l'IP de l'utilisateur si le paramètre est vide
                gunaLineTextBox1.Text = new WebClient().DownloadString("http://ip-api.com/line/?fields=query");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur :\n" + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error); // Erreur
            }
        }
    }
}
