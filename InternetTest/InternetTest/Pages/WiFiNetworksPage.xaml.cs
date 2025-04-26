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
using Synethia;
using System;
using System.Net.NetworkInformation;
using System.Windows;
using System.Windows.Controls;

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
		Loaded += (o, e) => SynethiaManager.InjectSynethiaCode(this, Global.SynethiaConfig.PagesInfo, 2, ref codeInjected);

	}

	bool loaded = false;
	private async void InitUI()
	{
		try
		{
			TitleTxt.Text = $"{Properties.Resources.WebUtilities} > {Properties.Resources.WiFiNetworks}"; // Set the title

			WiFiDisplayer.Children.Clear();
			ShowHiddenChk.IsChecked = !Global.Settings.HideDisabledAdapters;

			WiFiDisplayer.Visibility = Visibility.Collapsed;
			ScanningPanel.Visibility = Visibility.Visible;
			NoNetworksPanel.Visibility = Visibility.Collapsed;

			string currentSSID = Global.GetCurrentWifiSSID();
			await NativeWifi.ScanNetworksAsync(TimeSpan.FromSeconds(10));
			var wifis = Global.GetWiFis();
			for (int i = 0; i < wifis.Count; i++)
			{
				WiFiDisplayer.Children.Add(new WiFiNetworkItem(wifis[i], currentSSID));
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

			NetworksBtn.IsChecked = true;
			loaded = true;
		}
		catch
		{
			WiFiDisplayer.Visibility = Visibility.Collapsed;
			ScanningPanel.Visibility = Visibility.Collapsed;
			NoNetworksPanel.Visibility = Visibility.Visible;
		}
	}

	private void GetAdapters()
	{
		try
		{
			AdaptersPanel.Children.Clear();

			NetworkInterface[] networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
			for (int i = 0; i < networkInterfaces.Length; i++)
			{
				if (!(ShowHiddenChk.IsChecked ?? true) && networkInterfaces[i].OperationalStatus == OperationalStatus.Down) continue;

				// .NET 9+ get the same behavior as .NET 8
				if (!(Global.Settings.ShowAdaptersNoIpv4Support ?? false) && !networkInterfaces[i].Supports(NetworkInterfaceComponent.IPv4)) continue;
				AdaptersPanel.Children.Add(new AdapterItem(new(networkInterfaces[i])));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, Properties.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
		}
	}

	private void RefreshBtn_Click(object sender, RoutedEventArgs e)
	{
		SearchTxt.Text = "";
		if (AdaptersPage.Visibility == Visibility.Collapsed)
		{
			InitUI();
			return;
		}
		GetAdapters();
	}

	private void NetworksBtn_Click(object sender, RoutedEventArgs e)
	{
		AdaptersPage.Visibility = Visibility.Collapsed;
		NetworksPage.Visibility = Visibility.Visible;
		ShowHiddenChk.Visibility = Visibility.Collapsed;
		SearchBorder.Visibility = Visibility.Visible;
	}

	private void AdaptersBtn_Click(object sender, RoutedEventArgs e)
	{
		AdaptersPage.Visibility = Visibility.Visible;
		NetworksPage.Visibility = Visibility.Collapsed;
		ShowHiddenChk.Visibility = Visibility.Visible;
		SearchBorder.Visibility = Visibility.Collapsed;

		GetAdapters();
	}

	private void ShowHiddenChk_Checked(object sender, RoutedEventArgs e)
	{
		if (loaded)
			GetAdapters();
	}

	private void SearchTxt_TextChanged(object sender, TextChangedEventArgs e)
	{
		Search(SearchTxt.Text);
		DismissBtn.Visibility = SearchTxt.Text.Length > 0 ? Visibility.Visible : Visibility.Collapsed;
	}

	private void Search(string query)
	{
		int nbVis = 0;
		foreach (WiFiNetworkItem item in WiFiDisplayer.Children)
		{
			if (query == "")
			{
				item.Visibility = Visibility.Visible;
				nbVis++;
				continue;
			}
			bool match = item.NetworkInfo.Ssid.ToLower().Contains(query.ToLower());
			item.Visibility = match ? Visibility.Visible : Visibility.Collapsed;
			nbVis += match ? 1 : 0;
		}
		NoNetworksPanel.Visibility = nbVis == 0 ? Visibility.Visible : Visibility.Collapsed;
	}

	private void DismissBtn_Click(object sender, RoutedEventArgs e)
	{
		SearchTxt.Text = "";
	}
}
