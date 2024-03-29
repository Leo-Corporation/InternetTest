﻿/*
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
using InternetTest.UserControls;
using LeoCorpLibrary;
using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace InternetTest.Pages;

/// <summary>
/// Interaction logic for LocalizeIPPage.xaml
/// </summary>
public partial class LocalizeIPPage : Page
{
	IPInfo IPInfo { get; set; }
	string lastIP, lastWebsite = "";
	public LocalizeIPPage()
	{
		InitializeComponent();
	}
	private string lat, lon = "";

	internal async void LocalizeBtn_Click(object sender, RoutedEventArgs e)
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
					lastIP = IPTxt.Text; // Define

					if (string.IsNullOrEmpty(IPTxt.Text))
					{
						IPTxt.Text = ip.Query;
					}
					IPInfo = ip; // Set

					// Update dashboard UI
					CountryTxt.Text = ip.Country; // Set text
					RegionTxt.Text = ip.RegionName; // Set text
					CityTxt.Text = ip.City; // Set text
					ZipTxt.Text = ip.Zip; // Set text
					LatitudeTxt.Text = ip.Lat; // Set text
					LongitudeTxt.Text = ip.Lon; // Set text
					TimezoneTxt.Text = ip.TimeZone; // Set text
					ISPTxt.Text = ip.ISP; // Set text

					DashboardPanel.Visibility = Visibility.Visible;
					DashPlaceholder.Visibility = Visibility.Collapsed; // Hide placeholder

					if (Global.Settings.UseIpHistory.Value && !Global.LocatedIPs.Contains(ip.Query)) // Avoid duplicates
					{
						HistoryContent.Children.Add(new IpHistoryItem(ip, HistoryContent)); // Add to the history stack panel
						HistoryBtn.Visibility = Visibility.Visible; // Show history button
					}
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
					lastWebsite = IPTxt.Text; // Define

					if (string.IsNullOrEmpty(IPTxt.Text))
					{
						IPTxt.Text = ip.Query;
					}
					IPInfo = ip; // Set

					// Update dashboard UI
					CountryTxt.Text = ip.Country; // Set text
					RegionTxt.Text = ip.RegionName; // Set text
					CityTxt.Text = ip.City; // Set text
					ZipTxt.Text = ip.Zip; // Set text
					LatitudeTxt.Text = ip.Lat; // Set text
					LongitudeTxt.Text = ip.Lon; // Set text
					TimezoneTxt.Text = ip.TimeZone; // Set text
					ISPTxt.Text = ip.ISP; // Set text

					DashboardPanel.Visibility = Visibility.Visible;
					DashPlaceholder.Visibility = Visibility.Collapsed; // Hide placeholder

					if (Global.Settings.UseIpHistory.Value && !Global.LocatedIPs.Contains(ip.Query)) // Avoid duplicates
					{
						HistoryContent.Children.Add(new IpHistoryItem(ip, HistoryContent)); // Add to the history stack panel
						HistoryBtn.Visibility = Visibility.Visible; // Show history button
					}
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
			_ = double.TryParse(lat.Replace(".", ","), out double dLat);
			_ = double.TryParse(lon.Replace(".", ","), out double dLon);

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

	private void HideShowPassword_Click(object sender, RoutedEventArgs e)
	{
		if (IPPwrBox.Visibility == Visibility.Collapsed) // If the password is visible
		{
			IPPwrBox.Visibility = Visibility.Visible; // Change the visibility
			IPTxt.Visibility = Visibility.Collapsed; // Change the visibility
			IPPwrBox.Password = IPTxt.Text; // Set text
			HideShowPassword.Content = "\uF3F8"; // Change text
		}
		else // If the password is hidden
		{
			IPPwrBox.Visibility = Visibility.Collapsed; // Change the visibility
			IPTxt.Visibility = Visibility.Visible; // Change the visibility
			IPTxt.Text = IPPwrBox.Password; // Set text
			HideShowPassword.Content = "\uF3FC"; // Change text
		}
	}

	private void IPTxt_TextChanged(object sender, TextChangedEventArgs e)
	{
		IPPwrBox.Password = IPTxt.Text; // Set text
	}

	internal void HistoryBtn_Click(object sender, RoutedEventArgs e)
	{
		HistoryBtn.Visibility = HistoryContent.Children.Count > 0 ? Visibility.Visible : Visibility.Collapsed; // Show history button

		if (MainContent.Visibility == Visibility.Visible)
		{
			MainContent.Visibility = Visibility.Collapsed; // Show history
			HistoryScrollContent.Visibility = Visibility.Visible; // Hide main content
			HistoryBtn.Content = "\uF36A"; // Change text
		}
		else
		{
			HistoryScrollContent.Visibility = Visibility.Collapsed; // Hide history
			MainContent.Visibility = Visibility.Visible; // Show main content
			HistoryBtn.Content = "\uF47F"; // Set text
		}
	}

	private void IPRadioBtn_Checked(object sender, RoutedEventArgs e)
	{
		IPTxt.Text = IPRadioBtn.IsChecked.Value ? lastIP : lastWebsite; // Clear text
	}

	private async void MyIPBtn_Click(object sender, RoutedEventArgs e)
	{
		if (await NetworkConnection.IsAvailableAsync())
		{
			var ip = await Global.GetIPInfo(""); // Get IP info
			lat = ip.Lat; // Define
			lon = ip.Lon; // Define
			IPTxt.Text = ip.Query;
			lastIP = ip.Query; // Define
			IPRadioBtn.IsChecked = true;
			IPInfo = ip; // Set

			// Update dashboard UI
			CountryTxt.Text = ip.Country; // Set text
			RegionTxt.Text = ip.RegionName; // Set text
			CityTxt.Text = ip.City; // Set text
			ZipTxt.Text = ip.Zip; // Set text
			LatitudeTxt.Text = ip.Lat; // Set text
			LongitudeTxt.Text = ip.Lon; // Set text
			TimezoneTxt.Text = ip.TimeZone; // Set text
			ISPTxt.Text = ip.ISP; // Set text

			DashboardPanel.Visibility = Visibility.Visible;
			DashPlaceholder.Visibility = Visibility.Collapsed; // Hide placeholder
		}
	}
}
