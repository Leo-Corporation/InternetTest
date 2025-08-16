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
using InternetTest.Models;
using InternetTest.ViewModels.Components;
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

	public ObservableCollection<ConnectWiFiItemViewModel> WiFiNetworks { get; } = [];

	private bool _showHidden = false;
	public bool ShowHidden { get => _showHidden; set { _showHidden = value; OnPropertyChanged(nameof(ShowHidden)); GetAdapters(); } }

	public ICommand RefreshCommand { get; }
	public WiFiPageViewModel(Settings settings)
	{
		_settings = settings;
		Adapters = [];
		ShowHidden = !_settings.HideDisabledAdapters ?? false;

		GetAdapters();

		WiFiNetworks = [.. WiFiNetwork.GetWiFis().Select(x => new ConnectWiFiItemViewModel(x))];
		RefreshCommand = new RelayCommand(o => GetAdapters());
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
}
