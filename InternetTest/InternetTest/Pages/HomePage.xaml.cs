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
using ManagedNativeWifi;
using PeyrSharp.Env;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Serialization;

namespace InternetTest.Pages;
/// <summary>
/// Interaction logic for HomePage.xaml
/// </summary>
public partial class HomePage : Page
{
	public HomePage()
	{
		InitializeComponent();
		InitUI();
	}

	internal void InitUI()
	{
		// Load "Get started" section
		List<AppPages> relevantPages = Enumerable.Empty<AppPages>().ToList();
		relevantPages = Global.SynethiaConfig is not null ? Global.GetMostRelevantPages(Global.SynethiaConfig) : Global.DefaultRelevantPages;

		for (int i = 0; i < 5; i++)
		{
			GetStartedPanel.Children.Add(new PageCard(relevantPages[i]));
		}
		relevantPages.RemoveRange(0, 5); // Remove already added pages; the least releavnt remains

		// Load "Discover" section
		for (int i = 0; i < relevantPages.Count; i++)
		{
			DiscoverPanel.Children.Add(new PageCard(relevantPages[i]));
		}
	}

	private async void SpeedTest_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
	{
		SpeedTestPopup.IsOpen = true;
		ConnectPopup.IsOpen = false;
		PasswordPopup.IsOpen = false;
		try
		{
			CloseSpeedTestBtn.IsEnabled = false;
			SpeedTestStatus.Text = Properties.Resources.TestInProgress;

			// Test
			string targetUrl = "http://speedtest.tele2.net/10MB.zip";
			SpeedTxt.Text = "...";
			long fileSize = await DownloadFile(targetUrl);
			double speedMbps = fileSize / 1000000.0;

			SpeedTxt.Text = $"{speedMbps:0.00} MB/s";
			SpeedTestStatus.Text = Properties.Resources.SpeedTestSucess;
		}
		catch
		{
			SpeedTestStatus.Text = Properties.Resources.SpeedTestFailed;
		}
		CloseSpeedTestBtn.IsEnabled = true;
	}

	private void CloseSpeedTestBtn_Click(object sender, System.Windows.RoutedEventArgs e)
	{
		SpeedTestPopup.IsOpen = false;
	}

	static async Task<long> DownloadFile(string url)
	{
		Stopwatch stopwatch = Stopwatch.StartNew();

		using HttpClient client = new();
		HttpResponseMessage response = await client.GetAsync(url);

		if (response.IsSuccessStatusCode)
		{
			byte[] data = await response.Content.ReadAsByteArrayAsync();
			stopwatch.Stop();
			return data.Length;
		}
		else
		{
			return 0;
		}
	}

	private async void ConnectWiFi_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
	{
		try
		{
			ConnectPopup.IsOpen = true;
			SpeedTestPopup.IsOpen = false;
			PasswordPopup.IsOpen = false;

			WiFiDisplayer.Children.Clear();
			WiFiQuickActionLoader.Visibility = Visibility.Visible;
			WiFiQuickAction.Visibility = Visibility.Collapsed;
			WiFiQuickActionPlaceholder.Visibility = Visibility.Collapsed;

			string currentSSID = Global.GetCurrentWifiSSID();
			await NativeWifi.ScanNetworksAsync(TimeSpan.FromSeconds(10));
			var wifis = Global.GetWiFis();
			for (int i = 0; i < wifis.Count; i++)
			{
				WiFiDisplayer.Children.Add(new WiFiNetworkItem(wifis[i], currentSSID));
			}
			if (WiFiDisplayer.Children.Count > 0)
			{
				WiFiQuickActionPlaceholder.Visibility = Visibility.Collapsed;
				WiFiQuickAction.Visibility = Visibility.Visible;
			}
			else
			{
				WiFiQuickActionPlaceholder.Visibility = Visibility.Visible;
				WiFiQuickAction.Visibility = Visibility.Collapsed;
			}
			WiFiQuickActionLoader.Visibility = Visibility.Collapsed;
		}
		catch
		{

		}
	}

	private void CloseConnectBtn_Click(object sender, System.Windows.RoutedEventArgs e)
	{
		ConnectPopup.IsOpen = false;
	}

	private async void RecoverWiFi_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
	{
		PasswordPopup.IsOpen = true;
		SpeedTestPopup.IsOpen = false;
		ConnectPopup.IsOpen = false;
		await GetWiFiNetworksInfo();
	}

	private void ClosePasswordBtn_Click(object sender, System.Windows.RoutedEventArgs e)
	{
		PasswordPopup.IsOpen = false;
	}

	async Task GetWiFiNetworksInfo()
	{

		try
		{
			WiFiItemDisplayer.Children.Clear(); // Clear the panel
			PasswordQuickActionLoader.Visibility = Visibility.Visible;
			WiFiPasswordQuickAction.Visibility = Visibility.Collapsed;
			PasswordQuickActionPlaceholder.Visibility = Visibility.Collapsed;

			// Check if the temp directory exists
			string path = FileSys.AppDataPath + @"\Léo Corporation\InternetTest Pro\Temp";

			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}
			;

			// Run "netsh wlan export profile key=clear" command
			Process process = new();
			process.StartInfo.FileName = "cmd.exe";
			process.StartInfo.Arguments = $"/c netsh wlan export profile key=clear folder=\"{path}\"";
			process.StartInfo.UseShellExecute = false;
			process.StartInfo.CreateNoWindow = true;
			process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
			process.Start();
			await process.WaitForExitAsync();

			// Read the files
			string[] files = Directory.GetFiles(path);
			for (int i = 0; i < files.Length; i++)
			{
				XmlSerializer serializer = new(typeof(WLANProfile));
				StreamReader streamReader = new(files[i]); // Where the file is going to be read

				var test = (WLANProfile?)serializer.Deserialize(streamReader);

				if (test != null)
				{
					WiFiItemDisplayer.Children.Add(new WiFiInfoItem(test));
				}
				streamReader.Close();

				File.Delete(files[i]); // Remove the temp file

			}
			Directory.Delete(path); // Delete the temp directory		
			if (WiFiItemDisplayer.Children.Count > 0)
			{
				PasswordQuickActionPlaceholder.Visibility = Visibility.Collapsed;
				WiFiPasswordQuickAction.Visibility = Visibility.Visible;
			}
			else
			{
				PasswordQuickActionPlaceholder.Visibility = Visibility.Visible;
				WiFiPasswordQuickAction.Visibility = Visibility.Collapsed;
			}
			PasswordQuickActionLoader.Visibility = Visibility.Collapsed;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, Properties.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
		}
	}
}
