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
using System.Net.NetworkInformation;
using System.Windows;

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

		private void InitUI()
		{
			NameTxt.Text = AdapterInfo.Name;
			InfoTxt.Text = AdapterInfo.ToFormattedString();
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

			HeadTxt.Text = $"{Properties.Resources.Name}\n" +
				$"{Properties.Resources.InterfaceType}\n" +
				$"{Properties.Resources.Status}\n" +
				$"{Properties.Resources.IpVersion}\n" +
				$"{Properties.Resources.DNSSuffix}\n" +
				$"{Properties.Resources.MTU}\n" +
				$"{Properties.Resources.DnsEnabled}\n" +
				$"{Properties.Resources.DnsDynamicConfigured}\n" +
				$"{Properties.Resources.ReceiveOnly}\n" +
				$"{Properties.Resources.Multicast}\n" +
				$"{Properties.Resources.Speed}\n" +
				$"{Properties.Resources.TotalBytesReceived}\n" +
				$"{Properties.Resources.TotalBytesSent}\n" +
				$"{Properties.Resources.IncomingPacketsDiscarded}\n" +
				$"{Properties.Resources.IncomingPacketsWithErrors}\n" +
				$"{Properties.Resources.IncomingUnknownProtocolPackets}\n" +
				$"{Properties.Resources.NonUnicastPacketsReceived}\n" +
				$"{Properties.Resources.NonUnicastPacketsSent}\n" +
				$"{Properties.Resources.OutgoingPacketsDiscarded}\n" +
				$"{Properties.Resources.OutgoingPacketsWithErrors}\n" +
				$"{Properties.Resources.OutputQueueLength}\n" +
				$"{Properties.Resources.UnicastPacketsReceived}\n" +
				$"{Properties.Resources.UnicastPacketsSent}";

			StatusTxt.Text = AdapterInfo.Status switch { OperationalStatus.Up => Properties.Resources.ConnectedS, OperationalStatus.Down => Properties.Resources.Disconnected, _ => AdapterInfo.Status.ToString() };
			DataTxt.Text = $"{Global.GetStorageUnit(AdapterInfo.BytesReceived + AdapterInfo.BytesSent).Item2:0.00} {Global.UnitToString(Global.GetStorageUnit(AdapterInfo.BytesReceived + AdapterInfo.BytesSent).Item1)}";
		}

		private void CloseBtn_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}

		private void CopyBtn_Click(object sender, RoutedEventArgs e)
		{
			Clipboard.SetText(AdapterInfo.ToLongFormattedString());
		}
	}
}
