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
using InternetTest.UserControls;
using ManagedNativeWifi;
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

namespace InternetTest.Pages;
/// <summary>
/// Interaction logic for WiFiNetworksPage.xaml
/// </summary>
public partial class WiFiNetworksPage : Page
{
	bool codeInjected = !Global.Settings.UseSynethia;

	public WiFiNetworksPage()
	{
		InitializeComponent();
		InitUI(); // Load the UI
		Loaded += (o, e) => InjectSynethiaCode();
	}

	private void InjectSynethiaCode()
	{
		if (codeInjected) return;
		codeInjected = true;
		foreach (Button b in Global.FindVisualChildren<Button>(this))
		{
			b.Click += (sender, e) =>
			{
				Global.SynethiaConfig.DnsPageInfo.InteractionCount++;
			};
		}

		// For each TextBox of the page
		foreach (TextBox textBox in Global.FindVisualChildren<TextBox>(this))
		{
			textBox.GotFocus += (o, e) =>
			{
				Global.SynethiaConfig.DnsPageInfo.InteractionCount++;
			};
		}

		// For each CheckBox/RadioButton of the page
		foreach (CheckBox checkBox in Global.FindVisualChildren<CheckBox>(this))
		{
			checkBox.Checked += (o, e) =>
			{
				Global.SynethiaConfig.DnsPageInfo.InteractionCount++;
			};
			checkBox.Unchecked += (o, e) =>
			{
				Global.SynethiaConfig.DnsPageInfo.InteractionCount++;
			};
		}

		foreach (RadioButton radioButton in Global.FindVisualChildren<RadioButton>(this))
		{
			radioButton.Checked += (o, e) =>
			{
				Global.SynethiaConfig.DnsPageInfo.InteractionCount++;
			};
			radioButton.Unchecked += (o, e) =>
			{
				Global.SynethiaConfig.DnsPageInfo.InteractionCount++;
			};
		}
	}

	private async void InitUI()
	{
		try
		{
			TitleTxt.Text = $"{Properties.Resources.WebUtilities} > {Properties.Resources.WiFiNetworks}"; // Set the title

			WiFiDisplayer.Children.Clear();

			WiFiDisplayer.Visibility = Visibility.Collapsed;
			ScanningPanel.Visibility = Visibility.Visible;
			NoNetworksPanel.Visibility = Visibility.Collapsed;

			await NativeWifi.ScanNetworksAsync(TimeSpan.FromSeconds(10));
			var wifis = Global.GetWiFis();
			for (int i = 0; i < wifis.Count; i++)
			{
				WiFiDisplayer.Children.Add(new WiFiNetworkItem(wifis[i]));
			}

			if (WiFiDisplayer.Children.Count == 0)
			{
				WiFiDisplayer.Visibility = Visibility.Collapsed;
				ScanningPanel.Visibility = Visibility.Collapsed;
				NoNetworksPanel.Visibility = Visibility.Visible;
				return;
			}
			WiFiDisplayer.Visibility = Visibility.Visible;
			ScanningPanel.Visibility = Visibility.Collapsed;
			NoNetworksPanel.Visibility = Visibility.Collapsed;
		}
		catch
		{
			WiFiDisplayer.Visibility = Visibility.Collapsed;
			ScanningPanel.Visibility = Visibility.Collapsed;
			NoNetworksPanel.Visibility = Visibility.Visible;
		}
	}

	private void RefreshBtn_Click(object sender, RoutedEventArgs e)
	{
		InitUI();
    }
}
