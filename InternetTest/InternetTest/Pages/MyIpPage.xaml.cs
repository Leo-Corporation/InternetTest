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
using PeyrSharp.Core;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace InternetTest.Pages;
/// <summary>
/// Interaction logic for MyIpPage.xaml
/// </summary>
public partial class MyIpPage : Page
{
	bool codeInjected = !Global.Settings.UseSynethia;
	public MyIpPage()
	{
		InitializeComponent();
		InitUI();
		InjectSynethiaCode();
	}

	private async void InitUI()
	{
		try
		{
			if (await Internet.IsAvailableAsync())
			{
				GetMyIP(); // Locate the current IP
			}
		}
		catch (Exception) { } // Cancel if there is no internet connection
		TitleTxt.Text = $"{Properties.Resources.IPTools} > {Properties.Resources.MyIP}";
	}

	private void InjectSynethiaCode()
	{
		if (codeInjected) return;
		codeInjected = true;
		foreach (Button b in Global.FindVisualChildren<Button>(this))
		{
			b.Click += (sender, e) =>
			{
				Global.SynethiaConfig.MyIPPageInfo.InteractionCount++;
			};
		}

		// For each TextBox of the page
		foreach (TextBox textBox in Global.FindVisualChildren<TextBox>(this))
		{
			textBox.GotFocus += (o, e) =>
			{
				Global.SynethiaConfig.MyIPPageInfo.InteractionCount++;
			};
		}

		// For each CheckBox/RadioButton of the page
		foreach (CheckBox checkBox in Global.FindVisualChildren<CheckBox>(this))
		{
			checkBox.Checked += (o, e) =>
			{
				Global.SynethiaConfig.MyIPPageInfo.InteractionCount++;
			};
			checkBox.Unchecked += (o, e) =>
			{
				Global.SynethiaConfig.MyIPPageInfo.InteractionCount++;
			};
		}

		foreach (RadioButton radioButton in Global.FindVisualChildren<RadioButton>(this))
		{
			radioButton.Checked += (o, e) =>
			{
				Global.SynethiaConfig.MyIPPageInfo.InteractionCount++;
			};
			radioButton.Unchecked += (o, e) =>
			{
				Global.SynethiaConfig.MyIPPageInfo.InteractionCount++;
			};
		}
	}

	internal void GetMyIPBtn_Click(object sender, RoutedEventArgs e)
	{
		GetMyIP();

		// Increment the interaction count of the ActionInfo in Global.SynethiaConfig
		Global.SynethiaConfig.ActionInfos.First(a => a.Action == Enums.AppActions.MyIP).UsageCount++;
	}

	internal void ToggleConfidentialMode(bool toggle)
	{
		// Change text
		MyIPTxt.Text = toggle ? Properties.Resources.ConfidentialModeEnabled : ip;
		DetailsInfoTxt.Text = !toggle ? Properties.Resources.Details : Properties.Resources.DetailsNotAvailableCM;

		// Toggle the details panel
		DetailsWrap.Visibility = toggle ? Visibility.Collapsed : Visibility.Visible;
	}

	string ip = "";
	internal async void GetMyIP()
	{
		try
		{
			StatusIconTxt.Text = "\uF4AB";
			StatusIconTxt.Foreground = new SolidColorBrush(Global.GetColorFromResource("Gray"));
			MyIPTxt.Text = Properties.Resources.IPShowHere;

			var ipInfo = await Global.GetIPInfoAsync(""); // Giving an empty IP returns the user's current IP
			if (ipInfo is not null)
			{
				ip = ipInfo.Query ?? "";
				MyIPTxt.Text = Global.IsConfidentialModeEnabled ? Properties.Resources.ConfidentialModeEnabled : ip;
				CountryTxt.Text = ipInfo.Country;
				RegionTxt.Text = ipInfo.RegionName;
				CityTxt.Text = ipInfo.City;
				ZipCodeTxt.Text = ipInfo.Zip;
				LatTxt.Text = ipInfo.Lat.ToString().Replace(",", ".");
				LongitudeTxt.Text = ipInfo.Lon.ToString().Replace(",", ".");
				TimezoneTxt.Text = ipInfo.Timezone;
				IspTxt.Text = ipInfo.Isp;

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
			MessageBox.Show(ex.Message, Properties.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
		}
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

	private void CopyBtn_Click(object sender, RoutedEventArgs e)
	{
		try
		{
			Clipboard.SetDataObject(MyIPTxt.Text); // Copy to clipboard
		}
		catch { }
	}

	private void TextBlock_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
	{
		Clipboard.SetText(((TextBlock)sender).Text);
	}
}
