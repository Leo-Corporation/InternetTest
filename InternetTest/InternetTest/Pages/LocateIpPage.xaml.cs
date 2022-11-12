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
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace InternetTest.Pages;
/// <summary>
/// Interaction logic for LocateIpPage.xaml
/// </summary>
public partial class LocateIpPage : Page
{
	bool codeInjected = !Global.Settings.UseSynethia;
	private IPInfo? CurrentIP { get; set; }
	public LocateIpPage()
	{
		InitializeComponent();
		InitUI(); // Load the UI
		InjectSynethiaCode();
	}

	private async void InitUI()
	{
		TitleTxt.Text = $"{Properties.Resources.IPTools} > {Properties.Resources.LocateIP}";
		try
		{
			if (await NetworkConnection.IsAvailableAsync())
			{
				LocateIP(""); // Get the current IP of the user
			}
		}
		catch (Exception) { } // Cancel if there is no internet connection
	}

	private void InjectSynethiaCode()
	{
		if (codeInjected) return;
		codeInjected = true;
		foreach (Button b in Global.FindVisualChildren<Button>(this))
		{
			b.Click += (sender, e) =>
			{
				Global.SynethiaConfig.LocateIPPageInfo.InteractionCount++;
			};
		}

		// For each TextBox of the page
		foreach (TextBox textBox in Global.FindVisualChildren<TextBox>(this))
		{
			textBox.GotFocus += (o, e) =>
			{
				Global.SynethiaConfig.LocateIPPageInfo.InteractionCount++;
			};
		}

		// For each CheckBox/RadioButton of the page
		foreach (CheckBox checkBox in Global.FindVisualChildren<CheckBox>(this))
		{
			checkBox.Checked += (o, e) =>
			{
				Global.SynethiaConfig.LocateIPPageInfo.InteractionCount++;
			};
			checkBox.Unchecked += (o, e) =>
			{
				Global.SynethiaConfig.LocateIPPageInfo.InteractionCount++;
			};
		}

		foreach (RadioButton radioButton in Global.FindVisualChildren<RadioButton>(this))
		{
			radioButton.Checked += (o, e) =>
			{
				Global.SynethiaConfig.LocateIPPageInfo.InteractionCount++;
			};
			radioButton.Unchecked += (o, e) =>
			{
				Global.SynethiaConfig.LocateIPPageInfo.InteractionCount++;
			};
		}
	}

	internal void ToggleConfidentialMode(bool toggle)
	{
		// Change the text
		MyIPTxt.Text = toggle ? Properties.Resources.ConfidentialModeEnabled : lIp;
		DetailsInfoTxt.Text = !toggle ? Properties.Resources.Details : Properties.Resources.DetailsNotAvailableCM;

		// Toggle visibility
		IpTxt.Visibility = toggle ? Visibility.Collapsed : Visibility.Visible;
		IpPassword.Visibility = !toggle ? Visibility.Collapsed : Visibility.Visible;
		DetailsWrap.Visibility = toggle ? Visibility.Collapsed : Visibility.Visible;

		// Toggle the password box
		if (toggle) IpPassword.Password = IpTxt.Text;
		else IpTxt.Text = IpPassword.Password;
	}

	internal void LocateIPBtn_Click(object sender, RoutedEventArgs e)
	{
		if (!Global.IsIpValid(Global.IsConfidentialModeEnabled ? IpPassword.Password : IpTxt.Text)) return; // Cancel if the IP isn't valid
		LocateIP(Global.IsConfidentialModeEnabled ? IpPassword.Password : IpTxt.Text); // Locate IP

		// Increment the interaction count of the ActionInfo in Global.SynethiaConfig
		Global.SynethiaConfig.ActionInfos.First(a => a.Action == Enums.AppActions.LocateIP).UsageCount++;
	}

	private void MapBtn_Click(object sender, RoutedEventArgs e)
	{
		var lat = LatTxt.Text;
		var lon = LongitudeTxt.Text;
		_ = double.TryParse(lat.Replace(".", ","), out double dLat);
		_ = double.TryParse(lon.Replace(".", ","), out double dLon);
		Process.Start("explorer.exe", Global.Settings.MapProvider switch
		{
			MapProvider.OpenStreetMap => $"\"https://www.openstreetmap.org/directions?engine=graphhopper_foot&route={lat}%2C{lon}%3B{lat}%2C{lon}#map=12/{lat}/{lon}\"",
			MapProvider.Google => $"\"https://www.google.com/maps/place/{Global.GetGoogleMapsPoint(dLat, dLon)}\"",
			MapProvider.Microsoft => $"\"https://www.bing.com/maps?q={lat} {lon}\"",
			MapProvider.Here => $"\"https://wego.here.com/directions/mix/{lat},{lon}/?map={lat},{lon},12\"",
			MapProvider.Yandex => $"\"https://yandex.com/maps/?ll={lon}%2C{lat}&z=12\"",
			_ => $"\"https://www.openstreetmap.org/directions?engine=graphhopper_foot&route={lat}%2C{lon}%3B{lat}%2C{lon}#map=12/{lat}/{lon}\""
		});
	}

	string lIp = "";
	internal async void LocateIP(string ip)
	{
		try
		{
			StatusIconTxt.Text = "\uF4AB";
			StatusIconTxt.Foreground = new SolidColorBrush(Global.GetColorFromResource("Gray"));
			MyIPTxt.Text = Properties.Resources.IPShowHere2;

			var ipInfo = await Global.GetIPInfoAsync(ip); // Giving an empty IP returns the user's current IP
			CurrentIP = ipInfo;
			if (ipInfo is not null)
			{
				lIp = ipInfo.Query ?? "";
				MyIPTxt.Text = Global.IsConfidentialModeEnabled ? Properties.Resources.ConfidentialModeEnabled : lIp;
				CountryTxt.Text = ipInfo.Country;
				RegionTxt.Text = ipInfo.RegionName;
				CityTxt.Text = ipInfo.City;
				ZipCodeTxt.Text = ipInfo.Zip;
				LatTxt.Text = ipInfo.Lat.ToString().Replace(",", ".");
				LongitudeTxt.Text = ipInfo.Lon.ToString().Replace(",", ".");
				TimezoneTxt.Text = ipInfo.Timezone;
				IspTxt.Text = ipInfo.Isp;

				IpTxt.Text = IpTxt.Text is { Length: 0 } ? ipInfo.Query : IpTxt.Text; // If the IP is empty, use the user's current IP

				StatusIconTxt.Text = "\uF299";
				StatusIconTxt.Foreground = new SolidColorBrush(Global.GetColorFromResource("Green"));
			}
			else
			{
				StatusIconTxt.Text = "\uF36E";
				StatusIconTxt.Foreground = new SolidColorBrush(Global.GetColorFromResource("Red"));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, Properties.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error); // Show error message
		}
	}

	private void SaveBtn_Click(object sender, RoutedEventArgs e)
	{
		if (CurrentIP is null) return;
		SaveFileDialog dialog = new()
		{
			Title = Properties.Resources.Save,
			Filter = Properties.Resources.TxtFiles + " (*.txt)|*.txt",
			InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
			FileName = IpTxt.Text + ".txt",
			DefaultExt = ".txt"
		};

		if (dialog.ShowDialog().Value)
		{
			File.WriteAllText(dialog.FileName, CurrentIP?.ToString());
		}
	}
}
