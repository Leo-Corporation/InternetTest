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
using InternetTest.UserControls;
using ManagedNativeWifi;
using PeyrSharp.Core;
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

	internal async void InitUI()
	{
		// Load "Get started" section
		List<AppPages> relevantPages = Enumerable.Empty<AppPages>().ToList();
		if (Global.SynethiaConfig is not null)
		{
			relevantPages = Global.GetMostRelevantPages(Global.SynethiaConfig);
		}
		else
		{
			relevantPages = Global.DefaultRelevantPages;
		}

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

		// Load "Status" section
		if (Global.Settings.TestOnStart) LoadStatusCard();

		// Load "Network" section
		LoadNetworkCard();

		// Load "My IP" section
		ip = (await Global.GetIPInfoAsync("")).Query ?? "";
	}

	private async void RefreshStatusBtn_Click(object sender, System.Windows.RoutedEventArgs e)
	{
		StatusTxt.Text = Properties.Resources.Checking;
		LoadStatusCard();
	}

	bool connected = true;
	internal async void LoadStatusCard()
	{
		connected = await Internet.IsAvailableAsync(Global.Settings.TestSite); // Check if Internet is available
		StatusTxt.Text = connected ? Properties.Resources.ConnectedS : Properties.Resources.NotConnectedS; // Set text
		StatusIconTxt.Text = connected ? "\uF299" : "\uF36E";
		StatusIconTxt.Foreground = connected ? Global.GetBrushFromResource("Green") : Global.GetBrushFromResource("Red");
	}

	internal void LoadNetworkCard()
	{
		try
		{
			string ssid = Global.GetCurrentWifiSSID();

			NetworkTxt.Text = (ssid == null || !connected) ? Properties.Resources.NotConnectedS : ssid;
			NetworkTitleTxt.Text = Properties.Resources.WiFi;
			NetworkIconTxt.Text = (ssid == null || !connected) ? "\uFC27" : "\uF8C5";

		}
		catch // If there is no WiFi
		{
			NetworkIconTxt.Text = connected ? "\uF35A" : "\uFC27";
			NetworkTxt.Text = connected ? Properties.Resources.Ethernet : Properties.Resources.NotConnectedS;
			NetworkTitleTxt.Text = Properties.Resources.Network;
		}
	}

	private void RefreshNetworkBtn_Click(object sender, System.Windows.RoutedEventArgs e)
	{
		LoadNetworkCard();
	}

	string ip = "";
	private async void RefreshMyIpBtn_Click(object sender, System.Windows.RoutedEventArgs e)
	{
		ip = (await Global.GetIPInfoAsync("")).Query ?? "";
	}

	private void MyIpBorder_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
	{
		MyIpTxt.Text = ip;
		RefreshMyIpBtn.Visibility = Visibility.Visible;
	}

	private void MyIpBorder_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
	{
		MyIpTxt.Text = Properties.Resources.HoverToReveal;
		RefreshMyIpBtn.Visibility = Visibility.Hidden;
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

		using (HttpClient client = new HttpClient())
		{
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
	}

	private async void ConnectWiFi_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
	{
		try
		{
			ConnectPopup.IsOpen = true;
			SpeedTestPopup.IsOpen = false;
			PasswordPopup.IsOpen = false;
			WiFiDisplayer.Children.Clear();
			await NativeWifi.ScanNetworksAsync(TimeSpan.FromSeconds(10));
			var wifis = Global.GetWiFis();
			for (int i = 0; i < wifis.Count; i++)
			{
				WiFiDisplayer.Children.Add(new WiFiNetworkItem(wifis[i]));
			}
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

			// Check if the temp directory exists
			string path = FileSys.AppDataPath + @"\Léo Corporation\InternetTest Pro\Temp";

			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			};

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
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, Properties.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
		}
	}

	private void StatusBorder_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
	{
		RefreshStatusBtn.Visibility = Visibility.Visible;
	}

	private void StatusBorder_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
	{
		RefreshStatusBtn.Visibility = Visibility.Hidden;
	}

	private void NetworkBorder_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
	{
		RefreshNetworkBtn.Visibility = Visibility.Visible;
	}

	private void NetworkBorder_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
	{
		RefreshNetworkBtn.Visibility = Visibility.Hidden;
	}
}
