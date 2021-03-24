/*
MIT License

Copyright (c) Léo Corporation

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE. 
*/
using InternetTest.Classes;
using InternetTest.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InternetTest.Pages
{
    /// <summary>
    /// Interaction logic for LocalizeIPPage.xaml
    /// </summary>
    public partial class LocalizeIPPage : Page
    {
        public LocalizeIPPage()
        {
            InitializeComponent();
        }
        private string lat, lon = "";

        private async void LocalizeBtn_Click(object sender, RoutedEventArgs e)
        {
            var ip = await Global.GetIPInfo(IPTxt.Text); // Get IP info
            lat = ip.Lat; // Define
            lon = ip.Lon; // Define
            IPInfoTxt.Text = ip.ToString(); // Show IP info

            if (string.IsNullOrEmpty(IPTxt.Text))
            {
                IPTxt.Text = ip.Query;
            }
        }

        private void OpenMapBtn_Click(object sender, RoutedEventArgs e)
        {
            if (lat != "" && lon != "")
            {
                switch (Global.Settings.MapProvider)
                {
                    case MapProviders.BingMaps: Global.OpenLinkInBrowser($"https://www.bing.com/maps?q={ lat} {lon}"); break;
                    case MapProviders.GoogleMaps: Global.OpenLinkInBrowser($"https://www.google.com/maps/place/{lat},{lon}"); break;
                    case MapProviders.OpenStreetMap: Global.OpenLinkInBrowser($"https://www.openstreetmap.org/#map=12/{lat}/{lon}"); break;
                    default: Global.OpenLinkInBrowser($"https://www.openstreetmap.org/#map=12/{lat}/{lon}"); break;
                }
            }
        }

        private async void MyIPBtn_Click(object sender, RoutedEventArgs e)
        {
            var ip = await Global.GetIPInfo(""); // Get IP info
            lat = ip.Lat; // Define
            lon = ip.Lon; // Define
            IPInfoTxt.Text = ip.ToString(); // Show IP info
            IPTxt.Text = ip.Query;
        }
    }
}
