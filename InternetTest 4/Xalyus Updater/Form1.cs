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

namespace Xalyus_Updater
{
    public partial class Form1 : Form
    {
        WebClient client;
        string path = Application.StartupPath + "/InternetTest.exe";
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Guna.UI.Lib.GraphicsHelper.ShadowForm(this);
            try
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
                client = new WebClient();
                WebClient maj = new WebClient();
                string link = maj.DownloadString("https://raw.githubusercontent.com/Leo-Corporation/LeoCorp-Docs/master/Liens/Update%20System/InternetTest/4.0/download.txt");
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
            catch (Exception ex)
            {
                MessageBox.Show("Erreur :" + Environment.NewLine + ex.Message, "Erreur",  MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            Invoke(new MethodInvoker(delegate ()
            {
                Close();
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
                gunaLabel6.Text = $"{string.Format("{0:0.##}", percentage)}%";
                gunaLabel6.Left = (this.ClientSize.Width - gunaLabel6.Width) / 2;
                gunaProgressBar1.Value = int.Parse(Math.Truncate(percentage).ToString());
            }));
        }
    }
}
