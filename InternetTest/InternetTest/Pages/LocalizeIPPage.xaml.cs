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
using LeoCorpLibrary;
using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace InternetTest.Pages
{
	/// <summary>
	/// Interaction logic for LocalizeIPPage.xaml
	/// </summary>
	public partial class LocalizeIPPage : Page
	{
		IPInfo IPInfo { get; set; }
		public LocalizeIPPage()
		{
			InitializeComponent();
		}
		private string lat, lon = "";

		private async void LocalizeBtn_Click(object sender, RoutedEventArgs e)
		{
			if (await NetworkConnection.IsAvailableAsync())
			{
				if (IPRadioBtn.IsChecked.Value)
				{
					if (Global.IsIPValid(IPTxt.Text))
					{
						var ip = await Global.GetIPInfo(IPTxt.Text); // Get IP info
						lat = ip.Lat; // Define
						lon = ip.Lon; // Define
						IPInfoTxt.Text = ip.ToString(); // Show IP info

						if (string.IsNullOrEmpty(IPTxt.Text))
						{
							IPTxt.Text = ip.Query;
						}
						IPInfo = ip; // Set
					}
					else
					{
						MessageBox.Show(Properties.Resources.InvalidIP, Properties.Resources.InternetTest, MessageBoxButton.OK, MessageBoxImage.Error); // Show error
					}
				}
				else
				{
					if (!Global.IsIPValid(IPTxt.Text))
					{
						var ip = await Global.GetIPInfo(IPTxt.Text); // Get IP info
						lat = ip.Lat; // Define
						lon = ip.Lon; // Define
						IPInfoTxt.Text = ip.ToString(); // Show IP info

						if (string.IsNullOrEmpty(IPTxt.Text))
						{
							IPTxt.Text = ip.Query;
						}
						IPInfo = ip; // Set
					}
					else
					{
						MessageBox.Show(Properties.Resources.InvalidURL, Properties.Resources.InternetTest, MessageBoxButton.OK, MessageBoxImage.Error); // Show error
					}
				}
			}
		}

		private void OpenMapBtn_Click(object sender, RoutedEventArgs e)
		{
			if (lat != "" && lon != "")
			{
				double dLat, dLon;
				double.TryParse(lat.Replace(".", ","), out dLat);
				double.TryParse(lon.Replace(".", ","), out dLon);

				switch (Global.Settings.MapProvider)
				{
					case MapProviders.BingMaps: Global.OpenLinkInBrowser($"https://www.bing.com/maps?q={lat} {lon}"); break;
					case MapProviders.GoogleMaps: Global.OpenLinkInBrowser($"https://www.google.com/maps/place/{Global.GetGoogleMapsPoint(dLat, dLon)}"); break;
					case MapProviders.OpenStreetMap: Global.OpenLinkInBrowser($"https://www.openstreetmap.org/directions?engine=fossgis_osrm_car&route={lat},{lon}%3B{lat},{lon}#map=12/{lat}/{lon}"); break;
					case MapProviders.Yandex: Global.OpenLinkInBrowser($"https://yandex.com/maps/?ll={lon}%2C{lat}&z=12"); break;
					case MapProviders.HereWeGo: Global.OpenLinkInBrowser($"https://wego.here.com/directions/mix/{lat},{lon}/?map={lat},{lon},12"); break;
					default: Global.OpenLinkInBrowser($"https://www.openstreetmap.org/#map=12/{lat}/{lon}"); break;
				}
			}
		}

		private void SaveBtn_Click(object sender, RoutedEventArgs e)
		{
			if (IPInfo is not null)
			{
				SaveFileDialog saveFileDialog = new()
				{
					FileName = $"{IPInfo.Query}.txt",
					Filter = "TXT|*.txt"
				}; // Create SaveFileDialog

				if (saveFileDialog.ShowDialog() ?? true)
				{
					using (StreamWriter sw = File.CreateText(saveFileDialog.FileName))
					{
						sw.WriteLine(IPInfo.ToString()); // Create file
					}
					MessageBox.Show(Properties.Resources.IpSavedSuccess, Properties.Resources.InternetTest, MessageBoxButton.OK, MessageBoxImage.Information);
				}
			}
		}

		private async void MyIPBtn_Click(object sender, RoutedEventArgs e)
		{
			if (await NetworkConnection.IsAvailableAsync())
			{
				var ip = await Global.GetIPInfo(""); // Get IP info
				lat = ip.Lat; // Define
				lon = ip.Lon; // Define
				IPInfoTxt.Text = ip.ToString(); // Show IP info
				IPTxt.Text = ip.Query;
				IPRadioBtn.IsChecked = true;
				IPInfo = ip; // Set
			}
		}
	}
}
