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
using InternetTest.Commands;
using InternetTest.Enums;
using InternetTest.Interfaces;
using InternetTest.Models;
using InternetTest.ViewModels.Components;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace InternetTest.ViewModels;

public class LocateIpPageViewModel : ViewModelBase, ISensitiveViewModel
{
	private readonly Settings _settings;

	private ObservableCollection<GridItemViewModel> _details = [];
	public ObservableCollection<GridItemViewModel> Details { get => _details; set { _details = value; OnPropertyChanged(nameof(Details)); } }

	private ObservableCollection<GridItemViewModel> _ispInfo = [];
	public ObservableCollection<GridItemViewModel> IspInfo { get => _ispInfo; set { _ispInfo = value; OnPropertyChanged(nameof(IspInfo)); } }

	private string? _ipAddress;
	public string? IpAddress { get => _ipAddress; set { _ipAddress = value; OnPropertyChanged(nameof(IpAddress)); } }

	private string _placeholderText = Properties.Resources.NothingToShow;
	public string PlaceholderText { get => _placeholderText; set { _placeholderText = value; OnPropertyChanged(nameof(PlaceholderText)); } }

	private bool _empty = true;
	public bool Empty { get => _empty; set { _empty = value; OnPropertyChanged(nameof(Empty)); } }

	private bool _confidentialMode = false;
	public bool ConfidentialMode
	{
		get => _confidentialMode; set
		{
			_confidentialMode = value;
			PlaceholderText = value ? Properties.Resources.DetailsNotAvailableCM : Properties.Resources.NothingToShow;
			OnPropertyChanged(nameof(ConfidentialMode));
		}
	}

	private Ip? _ip;

	public ICommand LocateIpCommand => new RelayCommand(async o => await GetIp());
	public ICommand MyIpCommand => new RelayCommand(async o => { IpAddress = ""; await GetIp(); });
	public ICommand ShowMapCommand => new RelayCommand(o =>
	{
		if (_ip is null) return;
		double lat = _ip.Lat;
		double lon = _ip.Lon;

		Process.Start("explorer.exe", _settings.MapProvider switch
		{
			MapProvider.OpenStreetMap => $"\"https://www.openstreetmap.org/directions?engine=graphhopper_foot&route={lat}%2C{lon}%3B{lat}%2C{lon}#map={_settings.MapZoomLevel}/{lat}/{lon}\"",
			MapProvider.Google => $"\"https://www.google.com/maps/place/{GetGoogleMapsPoint(lat, lon)}\"",
			MapProvider.Microsoft => $"\"https://www.bing.com/maps?q={lat} {lon}&lvl={_settings.MapZoomLevel}&cp={lat}~{lon}\"",
			MapProvider.Here => $"\"https://wego.here.com/directions/mix/{lat},{lon}/?map={lat},{lon},{_settings.MapZoomLevel}\"",
			MapProvider.Yandex => $"\"https://yandex.com/maps/?ll={lon}%2C{lat}&z={_settings.MapZoomLevel}\"",
			_ => $"\"https://www.openstreetmap.org/directions?engine=graphhopper_foot&route={lat}%2C{lon}%3B{lat}%2C{lon}#map={_settings.MapZoomLevel}/{lat}/{lon}\""
		});
	});
	public ICommand ResetCommand => new RelayCommand(o => { IpAddress = ""; Empty = true; });
	public ICommand SaveCommand => new RelayCommand(o =>
	{
		if (_ip is null) return;
		SaveFileDialog dialog = new()
		{
			Title = Properties.Resources.Save,
			Filter = Properties.Resources.TxtFiles + " (*.txt)|*.txt",
			InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
			FileName = _ip.Query + ".txt",
			DefaultExt = ".txt"
		};

		if (dialog.ShowDialog() ?? false)
		{
			File.WriteAllText(dialog.FileName, _ip?.ToString());
		}
	});

	public LocateIpPageViewModel(Settings settings)
	{
		_settings = settings;
	}

	private async Task GetIp()
	{
		if (!Ip.IsValid(IpAddress ?? ""))
		{
			MessageBox.Show(Properties.Resources.InvalidURLMsg);
			return;
		}

		_ip = await Ip.GetIp(IpAddress ?? "");

		IpAddress = _ip.Query ?? Properties.Resources.Unknown;

		Details = [
			new(Properties.Resources.Country, _ip.Country ?? Properties.Resources.Unknown, 0, 0),
			new(Properties.Resources.Region, _ip.RegionName ?? Properties.Resources.Unknown, 1, 0),
			new(Properties.Resources.ZIPCode, _ip.Zip ?? Properties.Resources.Unknown, 2, 0),
			new(Properties.Resources.Latitude, _ip.Lat.ToString(), 3, 0),
			new(Properties.Resources.City, _ip.City ?? Properties.Resources.Unknown, 0, 1),
			new(Properties.Resources.District, _ip.District ?? Properties.Resources.Unknown, 1, 1),
			new(Properties.Resources.Status, _ip.Status == "success" ? Properties.Resources.Successful : Properties.Resources.Failed, 2, 1),
			new(Properties.Resources.Longitude, _ip.Lon.ToString(), 3, 1),
		];

		IspInfo = [
			new (Properties.Resources.ISP, _ip.Isp ?? Properties.Resources.Unknown, 0, 0),
			new (Properties.Resources.AsNumber, _ip.As ?? Properties.Resources.Unknown, 1, 0),
			new (Properties.Resources.Mobile, (_ip.Mobile ?? false ) ? Properties.Resources.Yes : Properties.Resources.No, 2, 0),
			new (Properties.Resources.Organization, _ip.Org ?? Properties.Resources.Unknown, 0, 1),
			new (Properties.Resources.Timezone, _ip.Timezone ?? Properties.Resources.Unknown, 1, 1),
			new (Properties.Resources.Proxy, (_ip.Proxy ?? false) ? Properties.Resources.Yes : Properties.Resources.No, 2, 1),
		];

		if (!ConfidentialMode) Empty = false;
	}

	private static string GetGoogleMapsPoint(double lat, double lon)
	{
		int deg = (int)lat; // Get integer
		int deg2 = (int)lon; // Get integer

		double d = (lat - deg) * 60d;
		double d2 = (lon - deg2) * 60d;

		string fDir = lat >= 0 ? "N" : "S"; // Get if the location is in the North or South
		string sDir = lon >= 0 ? "E" : "W"; // Get if the location is in the East or West

		string sD = d.ToString().Replace(",", "."); // Ensure to use . instead of ,
		string sD2 = d2.ToString().Replace(",", "."); // Ensure to use . instead of ,

		return $"{deg}° {sD}' {fDir}, {deg2}° {sD2}' {sDir}".Replace("-", "");
	}

	void ISensitiveViewModel.ToggleConfidentialMode(bool confidentialMode)
	{
		ConfidentialMode = confidentialMode;
		if (Details.Count > 0) Empty = confidentialMode;
	}
}
