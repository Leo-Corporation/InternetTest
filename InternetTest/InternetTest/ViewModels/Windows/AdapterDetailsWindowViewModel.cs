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
using System.Collections.ObjectModel;
using System.Net.NetworkInformation;
using System.Windows;
using System.Windows.Input;

namespace InternetTest.ViewModels.Windows;

public class AdapterDetailsWindowViewModel : ViewModelBase
{
	private readonly NetworkAdapter _networkAdapter;

	private string? _name;
	public string Name { get => _name ?? string.Empty; set { _name = value; OnPropertyChanged(nameof(Name)); } }

	private string? _icon;
	public string? Icon { get => _icon; set { _icon = value; OnPropertyChanged(nameof(Icon)); } }
	public ObservableCollection<GridItemViewModel> Details { get; } = [];
	public ObservableCollection<GridItemViewModel> Cat2 { get; } = [];
	public ObservableCollection<GridItemViewModel> Cat3 { get; } = [];

	public ICommand CopyCommand { get; }
	public AdapterDetailsWindowViewModel(NetworkAdapter networkAdapter)
	{
		Name = networkAdapter.Name;
		Icon = networkAdapter.Icon;

		Details = [
			new(Properties.Resources.DataConsumption, $"{StorageUnitHelper.GetStorageUnit(networkAdapter.BytesReceived + networkAdapter.BytesSent).Item2:0.00} {StorageUnitHelper.UnitToString(StorageUnitHelper.GetStorageUnit(networkAdapter.BytesReceived + networkAdapter.BytesSent).Item1)}", 0, 0),
			new(Properties.Resources.InterfaceType, NetworkAdapter.GetInterfaceTypeName(networkAdapter.NetworkInterfaceType), 1, 0),
			new(Properties.Resources.Status, networkAdapter.Status switch {
				OperationalStatus.Up => Properties.Resources.ConnectedS,
				OperationalStatus.Down => Properties.Resources.Disconnected,
				_ => networkAdapter.Status.ToString()
			}, 0, 1),
			new(Properties.Resources.IpVersion, networkAdapter.IpVersion ?? Properties.Resources.Unknown, 1, 1),
			new(Properties.Resources.Speed, $"{StorageUnitHelper.GetStorageUnit(networkAdapter.Speed).Item2:0.00} {StorageUnitHelper.UnitToString(StorageUnitHelper.GetStorageUnit(networkAdapter.Speed).Item1)}/s", 2, 0),
		];

		Cat2 = [
			new (Properties.Resources.DNSSuffix, networkAdapter.DnsSuffix, 0, 0),
			new(Properties.Resources.MTU, networkAdapter.Mtu.ToString(), 1, 0),
			new( Properties.Resources.DnsEnabled, BoolToString(networkAdapter.DnsEnabled), 0, 1),
			new( Properties.Resources.DnsDynamicConfigured, BoolToString(networkAdapter.IsDynamicDnsEnabled), 1, 1),
			new( Properties.Resources.Multicast, BoolToString(networkAdapter.SupportsMulticast), 2, 0)
		];

		Cat3 = [
			new (Properties.Resources.TotalBytesReceived, $"{StorageUnitHelper.GetStorageUnit(networkAdapter.BytesReceived).Item2:0.00} {StorageUnitHelper.UnitToString(StorageUnitHelper.GetStorageUnit(networkAdapter.BytesReceived).Item1)}", 0, 0),
			new (Properties.Resources.TotalBytesSent, $"{StorageUnitHelper.GetStorageUnit(networkAdapter.BytesSent).Item2:0.00} {StorageUnitHelper.UnitToString(StorageUnitHelper.GetStorageUnit(networkAdapter.BytesSent).Item1)}", 1, 0),
			new (Properties.Resources.IncomingPacketsDiscarded, networkAdapter.IncomingPacketsDiscarded.ToString(), 2, 0),
			new (Properties.Resources.IncomingPacketsWithErrors, networkAdapter.IncomingPacketsWithErrors.ToString(), 3, 0),
			new (Properties.Resources.IncomingUnknownProtocolPackets, networkAdapter.IncomingUnknownProtocolPackets.ToString(), 4, 0),
			new (Properties.Resources.NonUnicastPacketsReceived, networkAdapter.NonUnicastPacketsReceived.ToString(), 5, 0),
			new (Properties.Resources.NonUnicastPacketsSent, networkAdapter.NonUnicastPacketsSent.ToString(), 0, 1),
			new (Properties.Resources.OutgoingPacketsDiscarded, networkAdapter.OutgoingPacketsDiscarded.ToString(), 1, 1),
			new (Properties.Resources.OutgoingPacketsWithErrors, networkAdapter.OutgoingPacketsWithErrors.ToString(), 2, 1),
			new (Properties.Resources.OutputQueueLength, networkAdapter.OutputQueueLength.ToString(), 3, 1),
			new (Properties.Resources.UnicastPacketsReceived, networkAdapter.UnicastPacketsReceived.ToString(), 4, 1),
			new (Properties.Resources.UnicastPacketsSent, networkAdapter.UnicastPacketsSent.ToString(), 5, 1)
		];

		CopyCommand = new RelayCommand(Copy);
		_networkAdapter = networkAdapter;
	}

	private void Copy(object? o)
	{
		Clipboard.SetDataObject(_networkAdapter.ToFormattedString());
	}

	private static string BoolToString(bool b) => b ? Properties.Resources.Yes : Properties.Resources.No;
}