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
using InternetTest.Helpers;
using InternetTest.Models;
using InternetTest.ViewModels.Components;
using ManagedNativeWifi;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Net.NetworkInformation;
using System.Windows;
using System.Windows.Input;

namespace InternetTest.ViewModels;
public class WiFiPageViewModel : ViewModelBase
{
	private readonly Settings _settings;

	private ObservableCollection<NetworkAdapterItemViewModel> _adapters = [];
	public ObservableCollection<NetworkAdapterItemViewModel> Adapters
	{
		get => _adapters;
		set { _adapters = value; OnPropertyChanged(nameof(Adapters)); }
	}

	public ObservableCollection<ConnectWiFiItemViewModel> WiFiNetworks { get; set; } = [];
	public ObservableCollection<WlanProfileItemViewModel> WlanProfiles { get; set; } = [];

	private bool _showHidden = false;
	public bool ShowHidden { get => _showHidden; set { _showHidden = value; OnPropertyChanged(nameof(ShowHidden)); GetAdapters(); } }

	private bool _isRefrshing = false;
	public bool IsRefreshing { get => _isRefrshing; set { _isRefrshing = value; OnPropertyChanged(nameof(IsRefreshing)); } }

	private bool _noNetworks = false;
	public bool NoNetworks { get => _noNetworks; set { _noNetworks = value; OnPropertyChanged(nameof(NoNetworks)); } }

	private bool _profileLoading = false;
	public bool ProfileLoading { get => _profileLoading; set { _profileLoading = value; OnPropertyChanged(nameof(ProfileLoading)); } }

	private bool _noProfiles = false;
	public bool NoProfiles { get => _noProfiles; set { _noProfiles = value; OnPropertyChanged(nameof(NoProfiles)); } }

	public ICommand RefreshCommand => new RelayCommand(o => GetAdapters());
	public ICommand RefreshWiFiCommand { get; set; }
	public ICommand RefreshProfilesCommand => new RelayCommand(o => RefreshProfiles(true));
	public ICommand ExportCommand => new RelayCommand(async o =>
	{
		if (MessageBox.Show(Properties.Resources.ExportWlanProfilesMsg, Properties.Resources.InternetTest, MessageBoxButton.YesNoCancel, MessageBoxImage.Question) != MessageBoxResult.Yes)
			return;

		OpenFolderDialog openFolderDialog = new()
		{
			Title = Properties.Resources.Export,
			InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
			Multiselect = false
		};

		if (openFolderDialog.ShowDialog() == true)
		{
			await WlanProfile.ExportProfilesAsync(openFolderDialog.FolderNames[0], true);
		}
	});

	public WiFiPageViewModel(Settings settings)
	{
		_settings = settings;
		Adapters = [];
		ShowHidden = !_settings.HideDisabledAdapters ?? false;

		GetAdapters();

		string? currentSsid = NetworkHelper.GetCurrentWifiSSID();
		WiFiNetworks = [.. WiFiNetwork.GetWiFis().Select(x => new ConnectWiFiItemViewModel(x, currentSsid))];
		NoNetworks = WiFiNetworks.Count == 0;

		RefreshProfiles();

		RefreshWiFiCommand = new RelayCommand(async o =>
		{
			IsRefreshing = true;
			WiFiNetworks.Clear();

			await NativeWifi.ScanNetworksAsync(TimeSpan.FromSeconds(10));

			string? currentSsid = NetworkHelper.GetCurrentWifiSSID();
			WiFiNetwork.GetWiFis().ForEach(x => WiFiNetworks.Add(new ConnectWiFiItemViewModel(x, currentSsid)));
			IsRefreshing = false;
			NoNetworks = WiFiNetworks.Count == 0;
		});
	}

	internal void GetAdapters()
	{
		try
		{
			Adapters = [];
			NetworkInterface[] networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
			for (int i = 0; i < networkInterfaces.Length; i++)
			{
				if (!ShowHidden && networkInterfaces[i].OperationalStatus == OperationalStatus.Down) continue;

				// .NET 9+ get the same behavior as .NET 8
				if (!(_settings.ShowAdaptersNoIpv4Support ?? false) && !networkInterfaces[i].Supports(NetworkInterfaceComponent.IPv4)) continue;
				Adapters.Add(new(new(networkInterfaces[i])));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, Properties.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
		}
	}

	private async void RefreshProfiles(bool forceRefresh = false)
	{
		ProfileLoading = true;
		var profiles = await WlanProfile.GetProfilesAsync(forceRefresh);
		WlanProfiles.Clear();
		profiles.ForEach(x => WlanProfiles.Add(new WlanProfileItemViewModel(x)));
		ProfileLoading = false;
		NoProfiles = WlanProfiles.Count == 0;
	}
}
