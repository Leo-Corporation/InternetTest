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
using InternetTest.Helpers;
using InternetTest.Models;
using PeyrSharp.Core;
using System.Windows.Media;

namespace InternetTest.ViewModels;

public class HomePageViewModel : ViewModelBase
{
	public string HelloText => DateTime.Now.Hour switch
	{
		>= 21 or <= 7 => $"{Properties.Resources.GoodNight}, {Environment.UserName}.",
		>= 7 and <= 12 => $"{Properties.Resources.Hi}, {Environment.UserName}.",
		>= 12 and <= 17 => $"{Properties.Resources.GoodAfternoon}, {Environment.UserName}.",
		>= 17 and <= 21 => $"{Properties.Resources.GoodEvening}, {Environment.UserName}.",
	};

	private string _statusText;
	public string StatusText { get => _statusText; set { _statusText = value; OnPropertyChanged(nameof(StatusText)); } }

	private string _wiFiName;
	public string WiFiName { get => _wiFiName; set { _wiFiName = value; OnPropertyChanged(nameof(WiFiName)); } }

	private string _wiFiStrengthText;
	public string WiFiStrengthText { get => _wiFiStrengthText; set { _wiFiStrengthText = value; OnPropertyChanged(nameof(WiFiStrengthText)); } }

	private string _wiFiIcon;
	public string WiFiIcon { get => _wiFiIcon; set { _wiFiIcon = value; OnPropertyChanged(nameof(WiFiIcon)); } }

	private string _ipAddress;
	public string IpAddress { get => _ipAddress; set { _ipAddress = value; OnPropertyChanged(nameof(IpAddress)); } }

	private string _ipLocation;
	public string IpLocation { get => _ipLocation; set { _ipLocation = value; OnPropertyChanged(nameof(IpLocation)); } }

	private string _speed;
	public string Speed { get => _speed; set { _speed = value; OnPropertyChanged(nameof(Speed)); } }

	private SolidColorBrush _statusColor;
	private readonly Settings _settings;

	public SolidColorBrush StatusColor { get => _statusColor; set { _statusColor = value; OnPropertyChanged(nameof(StatusColor)); } }

	bool connected = true;
	public HomePageViewModel(Settings settings)
	{
		_settings = settings;

		// Get status
		LoadStatusCard();

		// Get WiFi information
		string? ssid = NetworkHelper.GetCurrentWifiSSID();
		int signalQuality = ssid != null ? NetworkHelper.GetCurrentNetwork().SignalQuality : 0;
		WiFiName = ssid ?? (connected ? Properties.Resources.Ethernet : Properties.Resources.NotConnectedS);
		WiFiIcon = ssid != null ? GetWiFiIcon(signalQuality) : (connected ? "\uF35A" : "\uFB69");
		WiFiStrengthText = $"{Properties.Resources.SignalQuality} - {(ssid != null ? signalQuality : 100)}%";

		// Get IP address
		LoadIpAddress();

		// Network speed
		Speed = $"~{NetworkHelper.GetCurrentSpeed()} Mbps";
	}

	internal async void LoadIpAddress()
	{
		Ip ip = await Ip.GetIp("");
		IpAddress = ip.Query ?? Properties.Resources.Unknown;
		IpLocation = $"{ip.City}, {ip.Country}" ?? Properties.Resources.Unknown;
	}

	internal async void LoadStatusCard()
	{
		connected = await Internet.IsAvailableAsync(_settings.TestSite);
		StatusText = connected ? Properties.Resources.ConnectedS : Properties.Resources.NotConnectedS;
		StatusColor = connected ? ThemeHelper.GetSolidColorBrush("Green") : ThemeHelper.GetSolidColorBrush("Red");
	}

	private static string GetWiFiIcon(int signalQuality) => signalQuality switch
	{
		>= 75 => "\uF8AD",
		>= 50 => "\uF8AF",
		>= 25 => "\uF8B1",
		_ => "\uF8B3"
	};
}
