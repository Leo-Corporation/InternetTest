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
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace InternetTest.Pages;
/// <summary>
/// Interaction logic for MyIpPage.xaml
/// </summary>
public partial class MyIpPage : Page
{
	bool codeInjected = false;
	public MyIpPage()
	{
		InitializeComponent();
		InitUI();
		InjectSynethiaCode();
	}

	private void InitUI()
	{
		GetMyIP(); // Locate the current IP
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

	private void GetMyIPBtn_Click(object sender, RoutedEventArgs e)
	{
		GetMyIP();

		// Increment the interaction count of the ActionInfo in Global.SynethiaConfig
		Global.SynethiaConfig.ActionInfos.First(a => a.Action == Enums.AppActions.MyIP).UsageCount++;
	}

	internal async void GetMyIP()
	{
		StatusIconTxt.Text = "\uF4AB";
		StatusIconTxt.Foreground = new SolidColorBrush(Global.GetColorFromResource("Gray"));
		MyIPTxt.Text = Properties.Resources.IPShowHere;

		var ipInfo = await Global.GetIPInfoAsync(""); // Giving an empty IP returns the user's current IP
		if (ipInfo is not null)
		{
			MyIPTxt.Text = ipInfo.Query;
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

	private void MapBtn_Click(object sender, RoutedEventArgs e)
	{
		//TODO: Add different map providers
		var lat = LatTxt.Text;
		var lon = LongitudeTxt.Text;
		Process.Start("explorer.exe", $"\"https://www.openstreetmap.org/directions?engine=graphhopper_foot&route={lat}%2C{lon}%3B{lat}%2C{lon}#map=12/{lat}/{lon}\"");
	}

	private void CopyBtn_Click(object sender, RoutedEventArgs e)
	{
		try
		{
			Clipboard.SetDataObject(MyIPTxt.Text); // Copy to clipboard
		}
		catch { }
	}
}
