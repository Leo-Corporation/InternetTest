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
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string path = Application.StartupPath + "/InternetTest.exe";
            string resFR = Application.StartupPath + "/fr-FR/InternetTest.resources.dll";
            string resEN = Application.StartupPath + "/en-US/InternetTest.resources.dll";
            string linkFR = new WebClient().DownloadString("https://dl.dropboxusercontent.com/s/0fyz21noc43t9d3/downloadfr.txt");
            string linkEN = "https://dl.dropboxusercontent.com/s/9ye4dgnvxje4701/downloaden.txt";
            File.Delete(path);
            client = new WebClient();
            WebClient maj = new WebClient();
            string link = maj.DownloadString("https://dl.dropboxusercontent.com/s/hftkddmm0ojn714/download.txt");
            client.DownloadProgressChanged += Client_DownloadProgressChanged;
            client.DownloadFileCompleted += Client_DownloadFileCompleted;
            if (!string.IsNullOrEmpty(link))
            {
                Thread thread = new Thread(() =>
                {
                    Uri uri = new Uri(link);
                    Uri uri1 = new Uri(linkFR);
                    Uri uri2 = new Uri(linkEN);
                    client.DownloadFileAsync(uri, path);
                    client.DownloadFileAsync(uri1, resFR);
                    client.DownloadFileAsync(uri2, resEN);
                });
                thread.Start();
            }
        }

        private void Client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            Invoke(new MethodInvoker(delegate ()
            {
                Process.Start(Application.StartupPath + "/InternetTest.exe");
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
