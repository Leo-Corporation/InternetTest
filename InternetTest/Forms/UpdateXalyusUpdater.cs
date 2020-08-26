using InternetTest.Classes;
using LeoCorpLibrary;
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

namespace InternetTest.Forms
{
    public partial class UpdateXalyusUpdater : Form
    {
        WebClient client;
        bool isAdmin;
        public UpdateXalyusUpdater(bool fromAdmin)
        {
            ChangeLanguage();
            InitializeComponent();
            isAdmin = fromAdmin;
        }

        private void UpdateXalyusUpdater_Load(object sender, EventArgs e)
        {
            ChangeTheme();
            Icon = new Branches().IconBranch(); // Met l'icône en foncion de la branche
            gunaPictureBox1.Image = new Branches().ImageBranch(); // Met l'image en fonction de la branche
            Guna.UI.Lib.GraphicsHelper.ShadowForm(this);
            if (isAdmin)
            {

            }
            else
            {
                if (new Language().GetCode() == "fr-FR")
                {
                    MessageBox.Show("Des mises à jour pour un des services de Xalyus Store sont disponibles" + Environment.NewLine + "L'application va redémarrer en tant qu'administrateur.", "Mise à jour d'un service", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (new Language().GetCode() == "EN")
                {
                    MessageBox.Show("Updates are availables for a Xalyus' Service" + Environment.NewLine + "The app will restart in admin mode.", "Service Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                Application.Exit();
                ExecuteAsAdmin(Application.StartupPath + "/InternetTest.exe");
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

        private void ChangeTheme() // Changer le thème
        {
            if (new Theme().IsDark()) // Si thème sombre
            {
                BackColor = Color.FromArgb(50, 50, 72);
                gunaProgressBar1.IdleColor = Color.FromArgb(80, 80, 92);
                gunaLabel1.ForeColor = Color.White;
                gunaLabel3.ForeColor = Color.White;
                gunaLabel4.ForeColor = Color.White;
            }
            else // Si thème clair
            {
                BackColor = Color.White;
                gunaProgressBar1.IdleColor = Color.Gainsboro;
                gunaLabel1.ForeColor = Color.Black;
                gunaLabel3.ForeColor = Color.Black;
                gunaLabel4.ForeColor = Color.Black;
            }
        }

        private void gunaGradientButton1_Click(object sender, EventArgs e)
        {
            gunaGradientButton1.Enabled = false;
            string path = Application.StartupPath + "/Xalyus Updater.exe";
            File.Delete(path);
            client = new WebClient();
            WebClient maj = new WebClient();
            string link = maj.DownloadString("https://raw.githubusercontent.com/Leo-Corporation/LeoCorp-Docs/master/Liens/Update%20System/InternetTest/4.0/Xalyus%20Updater/download.txt");
            client.DownloadProgressChanged += Client_DownloadProgressChanged;
            client.DownloadFileCompleted += Client_DownloadFileCompleted;
            if (!string.IsNullOrEmpty(link))
            {
                Thread thread = new Thread(() =>
                {
                    Uri uri = new Uri(link);
                    client.DownloadFileAsync(uri, path);
                });
                thread.Start();
            }
        }

        private void Client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            Invoke(new MethodInvoker(delegate ()
            {
                if (LeoCorpLibrary.Update.IsAvailable(Definitions.Version, LeoCorpLibrary.Update.GetLastVersion("https://raw.githubusercontent.com/Leo-Corporation/LeoCorp-Docs/master/Liens/Update%20System/InternetTest/4.0/version.txt")))
                {
                    new AvailableUpdate().Show();
                    Close();
                }
                else
                {
                    Application.Exit();
                    Process.Start(Application.StartupPath + "/InternetTest.exe");
                }
            }));
        }

        private void Client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            Invoke(new MethodInvoker(delegate ()
            {
                gunaProgressBar1.Minimum = 0;
                double receive = double.Parse(e.BytesReceived.ToString());
                double total = double.Parse(e.TotalBytesToReceive.ToString());
                double percentage = receive / total * 100;
                gunaLabel4.Text = $"{string.Format("{0:0.##}", percentage)}%";
                gunaLabel4.Left = (this.ClientSize.Width - gunaLabel4.Width) / 2;
                gunaProgressBar1.Value = int.Parse(Math.Truncate(percentage).ToString());
            }));
        }

        public void ExecuteAsAdmin(string fileName)
        {
            Process proc = new Process();
            proc.StartInfo.FileName = fileName;
            proc.StartInfo.UseShellExecute = true;
            proc.StartInfo.Verb = "runas";
            proc.StartInfo.Arguments = "adminmode";
            proc.Start();
        }
    }
}
