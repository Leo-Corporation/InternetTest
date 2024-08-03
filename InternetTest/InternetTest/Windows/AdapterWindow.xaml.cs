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
using PeyrSharp.Core.Converters;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Windows;
using System.Windows.Controls;

namespace InternetTest.Windows
{
	/// <summary>
	/// Interaction logic for AdapterWindow.xaml
	/// </summary>
	public partial class AdapterWindow : Window
	{
		public AdapterInfo AdapterInfo { get; }
		public AdapterWindow(AdapterInfo adapterInfo)
		{
			InitializeComponent();
			AdapterInfo = adapterInfo;
			InitUI();
		}
		private string BoolToString(bool b) => b ? Properties.Resources.Yes : Properties.Resources.No;
		private void InitUI()
		{
			NameTxt.Text = AdapterInfo.Name;
			AdapterIcon.Text = AdapterInfo.NetworkInterfaceType switch
			{
				NetworkInterfaceType.Tunnel => "\uF18E",
				NetworkInterfaceType.Ethernet => "\uFB32",
				NetworkInterfaceType.Ethernet3Megabit => "\uFB32",
				NetworkInterfaceType.FastEthernetFx => "\uFB32",
				NetworkInterfaceType.FastEthernetT => "\uFB32",
				NetworkInterfaceType.GigabitEthernet => "\uFB32",
				_ => "\uF8AC"
			};

			// Category 1: Most Important Items
			var category1 = new Dictionary<string, object>
			{
				{ Properties.Resources.DataConsumption, $"{Global.GetStorageUnit(AdapterInfo.BytesReceived + AdapterInfo.BytesSent).Item2:0.00} {Global.UnitToString(Global.GetStorageUnit(AdapterInfo.BytesReceived + AdapterInfo.BytesSent).Item1)}" },
				{ Properties.Resources.InterfaceType, Global.GetInterfaceTypeName(AdapterInfo.NetworkInterfaceType) },
				{ Properties.Resources.Status, AdapterInfo.Status switch { OperationalStatus.Up => Properties.Resources.ConnectedS, OperationalStatus.Down => Properties.Resources.Disconnected, _ => AdapterInfo.Status.ToString() } },
				{ Properties.Resources.IpVersion, AdapterInfo.IpVersion },
				{ Properties.Resources.Speed, $"{Global.GetStorageUnit(AdapterInfo.Speed).Item2:0.00} {Global.UnitToString(Global.GetStorageUnit(AdapterInfo.Speed).Item1)}/s" }
			};

			// Category 2: Important Configuration Details
			var category2 = new Dictionary<string, object>
			{
				{ Properties.Resources.DNSSuffix, AdapterInfo.DnsSuffix },
				{ Properties.Resources.MTU, AdapterInfo.Mtu },
				{ Properties.Resources.DnsEnabled, BoolToString(AdapterInfo.DnsEnabled) },
				{ Properties.Resources.DnsDynamicConfigured, BoolToString(AdapterInfo.IsDynamicDnsEnabled) },
				{ Properties.Resources.Multicast, BoolToString(AdapterInfo.SupportsMulticast) }
			};

			// Category 3: Performance and Error Metrics
			var category3 = new Dictionary<string, object>
			{
				{ Properties.Resources.TotalBytesReceived, $"{Global.GetStorageUnit(AdapterInfo.BytesReceived).Item2:0.00} {Global.UnitToString(Global.GetStorageUnit(AdapterInfo.BytesReceived).Item1)}" },
				{ Properties.Resources.TotalBytesSent, $"{Global.GetStorageUnit(AdapterInfo.BytesSent).Item2:0.00} {Global.UnitToString(Global.GetStorageUnit(AdapterInfo.BytesSent).Item1)}" },
				{ Properties.Resources.IncomingPacketsDiscarded, AdapterInfo.IncomingPacketsDiscarded },
				{ Properties.Resources.IncomingPacketsWithErrors, AdapterInfo.IncomingPacketsWithErrors },
				{ Properties.Resources.IncomingUnknownProtocolPackets, AdapterInfo.IncomingUnknownProtocolPackets },
				{ Properties.Resources.NonUnicastPacketsReceived, AdapterInfo.NonUnicastPacketsReceived },
				{ Properties.Resources.NonUnicastPacketsSent, AdapterInfo.NonUnicastPacketsSent },
				{ Properties.Resources.OutgoingPacketsDiscarded, AdapterInfo.OutgoingPacketsDiscarded },
				{ Properties.Resources.OutgoingPacketsWithErrors, AdapterInfo.OutgoingPacketsWithErrors },
				{ Properties.Resources.OutputQueueLength, AdapterInfo.OutputQueueLength },
				{ Properties.Resources.UnicastPacketsReceived, AdapterInfo.UnicastPacketsReceived },
				{ Properties.Resources.UnicastPacketsSent, AdapterInfo.UnicastPacketsSent }
			};

			int i = 0;
			foreach (var category in category1)
			{
				StackPanel stackPanel = new() { Margin = new(5) };
				stackPanel.Children.Add(new TextBlock() { FontSize = 12, Text = category.Key, Foreground = Global.GetBrushFromResource("Foreground2") });
				stackPanel.Children.Add(new TextBlock() { FontSize = 14, FontWeight = FontWeights.SemiBold, Text = category.Value.ToString() });
				Cat1Grid.Children.Add(stackPanel);
				Grid.SetColumn(stackPanel, i % 2);
				Grid.SetRow(stackPanel, i / 2);
				i++;
			}

			i = 0;
			foreach (var category in category2)
			{
				StackPanel stackPanel = new() { Margin = new(5) };
				stackPanel.Children.Add(new TextBlock() { FontSize = 12, Text = category.Key, Foreground = Global.GetBrushFromResource("Foreground2") });
				stackPanel.Children.Add(new TextBlock() { FontSize = 14, FontWeight = FontWeights.SemiBold, Text = category.Value.ToString() });
				Cat2Grid.Children.Add(stackPanel);
				Grid.SetColumn(stackPanel, i % 2);
				Grid.SetRow(stackPanel, i / 2);
				i++;
			}

			i = 0;
			foreach (var category in category3)
			{
				StackPanel stackPanel = new() { Margin = new(5) };
				stackPanel.Children.Add(new TextBlock() { FontSize = 12, Text = category.Key, Foreground = Global.GetBrushFromResource("Foreground2") });
				stackPanel.Children.Add(new TextBlock() { FontSize = 14, FontWeight = FontWeights.SemiBold, Text = category.Value.ToString() });
				Cat3Grid.Children.Add(stackPanel);
				Grid.SetColumn(stackPanel, i % 2);
				Grid.SetRow(stackPanel, i / 2);
				i++;
			}
		}

		private void CloseBtn_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}

		private void CopyBtn_Click(object sender, RoutedEventArgs e)
		{
			Clipboard.SetText(AdapterInfo.ToLongFormattedString());
		}

		bool cat2Expanded = false;
		private void ExpandCat2Btn_Click(object sender, RoutedEventArgs e)
		{
			cat2Expanded = !cat2Expanded;
			ExpandCat2Btn.Content = cat2Expanded ? "\uF2B7" : "\uF2A4";
			Cat2Grid.Visibility = cat2Expanded ? Visibility.Visible : Visibility.Collapsed;
		}

		bool cat3Expanded = false;
		private void ExpandCat3Btn_Click(object sender, RoutedEventArgs e)
		{
			cat3Expanded = !cat3Expanded;
			ExpandCat3Btn.Content = cat3Expanded ? "\uF2B7" : "\uF2A4";
			Cat3Grid.Visibility = cat3Expanded ? Visibility.Visible : Visibility.Collapsed;
		}
	}
}
