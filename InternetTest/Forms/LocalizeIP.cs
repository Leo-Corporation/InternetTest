using InternetTest.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace InternetTest.Forms
{
    public partial class LocalizeIP : Form
    {
        string lat, lon = ""; // Latitude et longitude
        public LocalizeIP()
        {
            InitializeComponent();
            ChangeTheme(); // Changer le thème
            ChangeLanguage(); // Changer la langue
        }

        private async void gunaGradientButton5_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(gunaLineTextBox1.Text) && !string.IsNullOrWhiteSpace(gunaLineTextBox1.Text)) // Si la textbox n'est pas vide
            {
                try // Essayer
                {
                    await Task.Run(() =>
                    {
                        Invoke(new MethodInvoker(delegate ()
                        {
                            gunaLabel2.Visible = false; // Cacher
                            gunaLabel3.Text = string.Empty; // Effacer
                            GetIPInfo(gunaLineTextBox1.Text); // Localiser IP
                        }));
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erreur :\n" + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error); // Erreur
                }
            }
        }

        private void ChangeLanguage()
        {
            if (new Language().GetCode() == "fr-FR")
            {
                Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("fr-FR");
            }
            else if (new Language().GetCode() == "EN")
            {
                Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            }
        }

        private void GetIPInfo(string ip) // Localiser une IP
        {
            lon = string.Empty; // Effacer
            lat = string.Empty; // Effacer
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
            if (new Language().GetCode() == "fr-FR")
            {
                string[] NameFR = new string[] // Noms en français
            {
                "Pays : ",
                "Région : ",
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
            else if (new Language().GetCode() == "EN")
            {
                string[] NameEN = new string[] // Noms en français
            {
                "Country : ",
                "Region : ",
                "City : ",
                "ZIP Code : ",
                "Latitude : ",
                "Longitude : ",
                "Timezone : ",
                "ISP : "
            };

                for (int i = 0; i < nodes.Length; i++) // Obtenir les infos
                {
                    // Ouvrir document XML de puis l'API
                    XmlTextReader xmlTextReader = new XmlTextReader(string.Format("http://ip-api.com/xml/{0}?lang=en", ip));
                    while (xmlTextReader.Read())
                    {
                        // Exemple si i = 0 (pays), vérifier  si le node "country" existe
                        if (xmlTextReader.NodeType == XmlNodeType.Element && xmlTextReader.Name == nodes[i])
                        {
                            gunaLabel3.Text += NameEN[i] + xmlTextReader.ReadElementContentAsString() + Environment.NewLine;
                        }
                    }
                }
            }
            lat = new WebClient().DownloadString(string.Format("http://ip-api.com/line/{0}?fields=lat", ip)); // Latitude
            lon = new WebClient().DownloadString(string.Format("http://ip-api.com/line/{0}?fields=lon", ip)); // Longitude
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

        private async void gunaGradientButton1_Click(object sender, EventArgs e)
        {
            gunaLabel3.Text = string.Empty; // Effacer
            try // Essayer
            {
                await Task.Run(() =>
                {
                    Invoke(new MethodInvoker(delegate ()
                    {
                        gunaLabel2.Visible = false; // Cacher
                        GetIPInfo(""); // Localiser l'IP de l'utilisateur si le paramètre est vide
                        gunaLineTextBox1.Text = new WebClient().DownloadString("http://ip-api.com/line/?fields=query");
                    }));
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur :\n" + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error); // Erreur
            }
        }

        private void gunaGradientButton2_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(lat) && !string.IsNullOrEmpty(lon)) // Vérifier que la localisation a été trouvée
            {
                switch (Properties.Settings.Default.MapsProvider) // Pouir chaque cas
                {
                    case "Bing": // Si Bing
                        Process.Start(string.Format("https://www.bing.com/maps?q={0} {1}", lat, lon)); // Ouvrir dans une carte
                        break;
                    case "Google": // Si Google
                        Process.Start(string.Format("https://www.google.com/maps/place/{0},{1}", lat, lon)); // Ouvrir dans une carte
                        break;
                }
            }
        }

        private void ChangeTheme()
        {
            if (new Theme().IsDark()) // Si le thème est sombre
            {
                BackColor = Color.FromArgb(50, 50, 72);
                gunaAdvenceTileButton1.Image = Properties.Resources.icons8_delete_32px;
                gunaAdvenceTileButton2.Image = Properties.Resources.icons8_subtract_100px;
                gunaLabel1.ForeColor = Color.White;
                gunaLabel2.ForeColor = Color.White;
                gunaLabel3.ForeColor = Color.White;
                gunaGradientButton1.BaseColor1 = Color.FromArgb(70, 70, 92);
                gunaGradientButton1.BaseColor2 = Color.FromArgb(70, 70, 92);
                gunaGradientButton1.Image = Properties.Resources.person_white;
                gunaGradientButton1.ForeColor = Color.White;
                gunaLineTextBox1.BackColor = Color.FromArgb(50, 50, 72);
                gunaLineTextBox1.ForeColor = Color.White;
            }
            else // Si le thème est clair
            {
                BackColor = Color.White;
                gunaAdvenceTileButton1.Image = Properties.Resources.icons8_delete_100px_1;
                gunaAdvenceTileButton2.Image = Properties.Resources.icons8_subtract_100px_1;
                gunaLabel1.ForeColor = Color.Black;
                gunaLabel2.ForeColor = Color.Black;
                gunaLabel3.ForeColor = Color.Black;
                gunaGradientButton1.BaseColor1 = Color.FromArgb(247, 247, 247);
                gunaGradientButton1.BaseColor2 = Color.FromArgb(247, 247, 247);
                gunaGradientButton1.Image = Properties.Resources.person_black;
                gunaGradientButton1.ForeColor = Color.Black;
                gunaLineTextBox1.BackColor = Color.White;
                gunaLineTextBox1.ForeColor = Color.Black;
            }
        }
    }
}
