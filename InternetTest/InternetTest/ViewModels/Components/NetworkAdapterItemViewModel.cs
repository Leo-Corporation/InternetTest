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
using InternetTest.Services;
using InternetTest.Windows;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Windows.Input;
using System.Windows.Media;

namespace InternetTest.ViewModels.Components;

public class NetworkAdapterItemViewModel : ViewModelBase
{
	private string? _name;
	public string? Name { get => _name; set { _name = value; OnPropertyChanged(nameof(Name)); } }

	private string? _interfaceType;
	public string? InterfaceType { get => _interfaceType; set { _interfaceType = value; OnPropertyChanged(nameof(InterfaceType)); } }

	private string? _status;
	public string? Status { get => _status; set { _status = value; OnPropertyChanged(nameof(Status)); } }

	private string? _totalBytesReceived;
	public string? TotalBytesReceived { get => _totalBytesReceived; set { _totalBytesReceived = value; OnPropertyChanged(nameof(TotalBytesReceived)); } }

	private string? _totalBytesSent;
	public string? TotalBytesSent { get => _totalBytesSent; set { _totalBytesSent = value; OnPropertyChanged(nameof(TotalBytesSent)); } }

	private string? _speed;
	public string? Speed { get => _speed; set { _speed = value; OnPropertyChanged(nameof(Speed)); } }

	private string? _icon;
	public string? Icon { get => _icon; set { _icon = value; OnPropertyChanged(nameof(Icon)); } }

	private SolidColorBrush? _statusColor;
	public SolidColorBrush? StatusColor
	{
		get => _statusColor;
		set
		{
			_statusColor = value;
			OnPropertyChanged(nameof(StatusColor));
		}
	}

	private SolidColorBrush? _statusBackgroundColor;
	public SolidColorBrush? StatusBackgroundColor
	{
		get => _statusBackgroundColor;
		set
		{
			_statusBackgroundColor = value;
			OnPropertyChanged(nameof(StatusBackgroundColor));
		}
	}

	public ICommand SettingsCommand { get; }
	public ICommand DetailsCommand { get; }

	public NetworkAdapterItemViewModel(NetworkAdapter networkAdapter)
	{
		Name = networkAdapter.Name;
		InterfaceType = NetworkAdapter.GetInterfaceTypeName(networkAdapter.NetworkInterfaceType);
		StatusBackgroundColor = networkAdapter.Status switch
		{
			OperationalStatus.Up => ThemeHelper.GetSolidColorBrush("LightGreen"),
			OperationalStatus.Down => ThemeHelper.GetSolidColorBrush("LightRed"),
			_ => ThemeHelper.GetSolidColorBrush("LightAccent")
		};

		StatusColor = networkAdapter.Status switch
		{
			OperationalStatus.Up => ThemeHelper.GetSolidColorBrush("ForegroundGreen"),
			OperationalStatus.Down => ThemeHelper.GetSolidColorBrush("ForegroundRed"),
			_ => ThemeHelper.GetSolidColorBrush("DarkFAccent")
		};

		Status = networkAdapter.Status switch
		{
			OperationalStatus.Up => Properties.Resources.ConnectedS,
			OperationalStatus.Down => Properties.Resources.Disconnected,
			_ => networkAdapter.Status.ToString()
		};

		Icon = networkAdapter.Icon;

		Speed = $"{StorageUnitHelper.GetStorageUnit(networkAdapter.Speed).Item2:0.00} {StorageUnitHelper.UnitToString(StorageUnitHelper.GetStorageUnit(networkAdapter.Speed).Item1)}/s";
		TotalBytesSent = $"{StorageUnitHelper.GetStorageUnit(networkAdapter.BytesSent).Item2:0.00} {StorageUnitHelper.UnitToString(StorageUnitHelper.GetStorageUnit(networkAdapter.BytesSent).Item1)}";
		TotalBytesReceived = $"{StorageUnitHelper.GetStorageUnit(networkAdapter.BytesReceived).Item2:0.00} {StorageUnitHelper.UnitToString(StorageUnitHelper.GetStorageUnit(networkAdapter.BytesReceived).Item1)}";

		SettingsCommand = new RelayCommand(o =>
		{
			Process.Start("control.exe", "ncpa.cpl");
		});

		DetailsCommand = new RelayCommand(o =>
		{
			WindowService windowService = new();
			windowService.ShowWindow<AdapterDetailsWindow>(new Windows.AdapterDetailsWindowViewModel(networkAdapter));
		});
	}
}
