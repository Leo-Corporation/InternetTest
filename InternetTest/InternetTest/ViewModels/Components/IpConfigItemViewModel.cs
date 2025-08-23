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
using System.Collections.ObjectModel;
using System.Net.NetworkInformation;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace InternetTest.ViewModels.Components;
public class IpConfigItemViewModel : ViewModelBase
{
	private ObservableCollection<GridItemViewModel> _details = [];
	public ObservableCollection<GridItemViewModel> Details { get => _details; set { _details = value; OnPropertyChanged(nameof(Details)); } }

	private string _name = string.Empty;
	public string Name { get => _name; set { _name = value; OnPropertyChanged(nameof(Name)); } }

	private string _adapterIcon = string.Empty;
	public string AdapterIcon { get => _adapterIcon; set { _adapterIcon = value; OnPropertyChanged(nameof(AdapterIcon)); } }

	private string _statusText = string.Empty;
	public string StatusText { get => _statusText; set { _statusText = value; OnPropertyChanged(nameof(StatusText)); } }

	private string _description = string.Empty;
	public string Description { get => _description; set { _description = value; OnPropertyChanged(nameof(Description)); } }

	private bool _isExpanded = false;
	public bool IsExpanded { get => _isExpanded; set { _isExpanded = value; OnPropertyChanged(nameof(IsExpanded)); } }

	private bool _confidentialMode = false;
	public bool ConfidentialMode
	{
		get => _confidentialMode;
		set
		{
			_confidentialMode = value;
			OnPropertyChanged(nameof(ConfidentialMode));
			foreach (var item in Details)
			{
				item.ConfidentialMode = value;
			}
			IsExpanded = !value && IsExpanded;
		}
	}

	private SolidColorBrush? _statusBrush;
	public SolidColorBrush? StatusBrush { get => _statusBrush; set { _statusBrush = value; OnPropertyChanged(nameof(StatusBrush)); } }

	public ICommand CopyCommand => new RelayCommand(o =>
	{
		Clipboard.SetDataObject(_ipConfig.ToString());
	});

	private readonly WindowsIpConfig _ipConfig;
	public IpConfigItemViewModel(WindowsIpConfig ipConfig)
	{
		_ipConfig = ipConfig;

		Name = _ipConfig.Name ?? Properties.Resources.Unknown;
		Description = _ipConfig.Description ?? Properties.Resources.Unknown;

		StatusText = _ipConfig.Status == OperationalStatus.Up ? Properties.Resources.ConnectedS : Properties.Resources.NotConnectedS;
		StatusBrush = _ipConfig.Status == OperationalStatus.Up
			? ThemeHelper.GetSolidColorBrush("Green")
			: ThemeHelper.GetSolidColorBrush("Red");

		IsExpanded = _ipConfig.Status == OperationalStatus.Up;

		AdapterIcon = _ipConfig.NetworkInterfaceType switch
		{
			NetworkInterfaceType.Tunnel => "\uF18E",
			NetworkInterfaceType.Ethernet => "\uFB32",
			NetworkInterfaceType.Ethernet3Megabit => "\uFB32",
			NetworkInterfaceType.FastEthernetFx => "\uFB32",
			NetworkInterfaceType.FastEthernetT => "\uFB32",
			NetworkInterfaceType.GigabitEthernet => "\uFB32",
			_ => "\uF8AC"
		};

		if (_ipConfig.Description != null && _ipConfig.Description.Contains("Bluetooth")) AdapterIcon = "\uF1DF";

		Details = [
			new (Properties.Resources.DNSSuffix, _ipConfig.DNSSuffix ?? Properties.Resources.Unknown, 0, 0),
			new (Properties.Resources.IPv4Address, _ipConfig.IPv4Address ?? Properties.Resources.Unknown, 1, 0, true),
			new (Properties.Resources.IPv6Address, _ipConfig.IPv6Address ?? Properties.Resources.Unknown, 0, 1, true),
			new (Properties.Resources.SubnetMask, _ipConfig.IPv4Mask ?? Properties.Resources.Unknown, 1, 1, true),
		];

		if (!string.IsNullOrEmpty(_ipConfig.IPv4Gateway)) Details.Add(new GridItemViewModel(Properties.Resources.GatewayIPv4, _ipConfig.IPv4Gateway, 2, 0, true));
		if (!string.IsNullOrEmpty(_ipConfig.IPv6Gateway)) Details.Add(new GridItemViewModel(Properties.Resources.GatewayIPv6, _ipConfig.IPv6Gateway, 2, 1, true));
	}
}
