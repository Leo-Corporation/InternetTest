using InternetTest.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

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

        private void gunaGradientButton5_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(gunaLineTextBox1.Text) && !string.IsNullOrWhiteSpace(gunaLineTextBox1.Text)) // Si la textbox n'est pas vide
            {
                gunaLabel2.Visible = false; // Cacher
                gunaLabel3.Text = string.Empty; // Effacer
                try // Essayer
                {
                    Task task = new Task(() => GetIPInfo(gunaLineTextBox1.Text));

                    task.Start(); // Localiser IP
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

        private async void GetIPInfo(string ip) // Localiser une IP
        {
            Invoke(new MethodInvoker(async delegate ()
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

                    // Download all the informations

                    WebClient webClient = new WebClient(); // Create WebClient
                    webClient.Encoding = Encoding.UTF8; // Change the encoding

                    string infos = await webClient.DownloadStringTaskAsync($"http://ip-api.com/line/{ip}?fields=9209&lang=fr");

                    StringReader reader = new StringReader(infos);
                    List<string> lines = new List<string>();
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        lines.Add(line);
                    }

                    // Treat informations

                    for (int i = 0; i < lines.Count - 1; i++)
                    {
                        gunaLabel3.Text += NameFR[i] + lines[i] + "\n";
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

                    // Download all the informations

                    WebClient webClient = new WebClient(); // Create WebClient
                    webClient.Encoding = Encoding.UTF8; // Change the encoding

                    string infos = await webClient.DownloadStringTaskAsync($"http://ip-api.com/line/{ip}?fields=9209&lang=en");

                    StringReader reader = new StringReader(infos);
                    List<string> lines = new List<string>();
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        lines.Add(line);
                    }

                    // Treat informations

                    for (int i = 0; i < lines.Count - 1; i++)
                    {
                        gunaLabel3.Text += NameEN[i] + lines[i] + "\n";
                    }
                }
                lat = await new WebClient().DownloadStringTaskAsync(string.Format("http://ip-api.com/line/{0}?fields=lat", ip)); // Latitude
                lon = await new WebClient().DownloadStringTaskAsync(string.Format("http://ip-api.com/line/{0}?fields=lon", ip)); // Longitude
            }));
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
                gunaLabel2.Visible = false; // Cacher

                //Task task = new Task(() => GetIPInfo(""));
                //task.Start(); // Localiser l'IP de l'utilisateur si le paramètre est vide
                GetIPInfo("");
                
                gunaLineTextBox1.Text = await new WebClient().DownloadStringTaskAsync("http://ip-api.com/line/?fields=query");
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
                switch (Properties.Settings.Default.MapsProvider) // For each case
                {
                    case "Bing Maps": // If Bing
                        Process.Start(string.Format("https://www.bing.com/maps?q={0} {1}", lat, lon)); // Open the map in the browser
                        break;
                    case "Google Maps": // If Google
                        Process.Start(string.Format("https://www.google.com/maps/place/{0},{1}", lat, lon)); // Open the map in the browser
                        break;
                    case "OpenStreetMap": // If OpenStreetMap
                        Process.Start($"https://www.openstreetmap.org/#map=12/{lat}/{lon}"); // Open the map in the browser
                        break;
                }
            }
        }

        private void ChangeTheme()
        {
            if (Theme.IsDark()) // Si le thème est sombre
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
