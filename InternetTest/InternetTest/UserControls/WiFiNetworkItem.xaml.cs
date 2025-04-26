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
using System.Windows;
using System.Windows.Controls;

namespace InternetTest.UserControls;
/// <summary>
/// Interaction logic for WiFiNetworkItem.xaml
/// </summary>
public partial class WiFiNetworkItem : UserControl
{
	internal NetworkInfo NetworkInfo { get; init; }
	public string CurrentSSID { get; init; }

	public WiFiNetworkItem(NetworkInfo networkInfo, string currentSSID)
	{
		InitializeComponent();
		NetworkInfo = networkInfo;
		CurrentSSID = currentSSID;
		InitUI();
	}

	private void InitUI()
	{
		StrengthTxt.Text = NetworkInfo.SignalQuality.ToString();
		SsidTxt.Text = NetworkInfo.Ssid;

		StrengthIconTxt.Text = NetworkInfo.SignalQuality switch
		{
			int n when (n >= 0 && n < 25) => "\uF8B3",
			int n when (n >= 25 && n < 50) => "\uF8B1",
			int n when (n >= 50 && n < 75) => "\uF8AF",
			int n when (n >= 75 && n <= 100) => "\uF8AD",
			_ => "\uF8AD",
		};

		ProfileTxt.Text = NetworkInfo.ProfileName;
		InterfaceTxt.Text = NetworkInfo.InterfaceDescription;
		BSSTxt.Text = NetworkInfo.BssType;
		SecurityEnabledTxt.Text = NetworkInfo.IsSecurityEnabled ? Properties.Resources.Yes : Properties.Resources.No;
		ChannelTxt.Text = $"{NetworkInfo.Channel}";
		BandTxt.Text = $"{NetworkInfo.Band:0.0} GHz";
		FrequencyTxt.Text = $"{NetworkInfo.Frequency} kHz";

		ConnectedIndicator.Visibility = NetworkInfo.Ssid == CurrentSSID ? Visibility.Visible : Visibility.Collapsed;
		ConnectBtn.Visibility = NetworkInfo.Ssid == CurrentSSID ? Visibility.Collapsed : Visibility.Visible;
	}

	private void CopyBtn_Click(object sender, RoutedEventArgs e)
	{
		Clipboard.SetText(NetworkInfo.ToString());
	}

	private void ExpanderBtn_Click(object sender, RoutedEventArgs e)
	{
		CollapseGrid.Visibility = CollapseGrid.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
		ExpanderBtn.Content = CollapseGrid.Visibility != Visibility.Visible ? "\uF2A4" : "\uF2B7";
	}

	private async void ConnectBtn_Click(object sender, RoutedEventArgs e)
	{
		if (!string.IsNullOrEmpty(NetworkInfo.ProfileName))
		{
			if (await Global.ConnectAsync(NetworkInfo.Ssid, ""))
			{
				MessageBox.Show(Properties.Resources.Connected);
			}
			else
			{
				MessageBox.Show(Properties.Resources.NotConnected);
			}
		}
		else
		{
			PasswordPopup.IsOpen = true;
		}
	}

	private async void ConnectPopupBtn_Click(object sender, RoutedEventArgs e)
	{
		try
		{
			if (await Global.ConnectAsync(NetworkInfo.Ssid, PasswordTxt.Password))
			{
				MessageBox.Show(Properties.Resources.Connected);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show($"{Properties.Resources.NotConnected}\n\n{ex.Message}");
		}
	}
}
