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

using System.Net.NetworkInformation;

namespace InternetTest.Classes
{
	public class AdapterInfo
	{
		public string Name { get; set; }
		public NetworkInterfaceType NetworkInterfaceType { get; set; }
		public OperationalStatus Status { get; set; }
		public string IpVersion { get; set; }
		public string DnsSuffix { get; set; }
		public int Mtu { get; set; }
		public bool DnsEnabled { get; set; }
		public bool IsDynamicDnsEnabled { get; set; }
		public bool IsReceiveOnly { get; set; }
		public bool SupportsMulticast { get; set; }
		public long Speed { get; set; }
		public long BytesReceived { get; set; }
		public long BytesSent { get; set; }
		public long IncomingPacketsDiscarded { get; set; }
		public long IncomingPacketsWithErrors { get; set; }
		public long IncomingUnknownProtocolPackets { get; set; }
		public long NonUnicastPacketsReceived { get; set; }
		public long NonUnicastPacketsSent { get; set; }
		public long OutgoingPacketsDiscarded { get; set; }
		public long OutgoingPacketsWithErrors { get; set; }
		public long OutputQueueLength { get; set; }
		public long UnicastPacketsReceived { get; set; }
		public long UnicastPacketsSent { get; set; }

		public AdapterInfo(NetworkInterface adapter)
		{
			IPInterfaceProperties properties = adapter.GetIPProperties();
			IPInterfaceStatistics statistics = adapter.GetIPStatistics();
			Name = adapter.Description;
			NetworkInterfaceType = adapter.NetworkInterfaceType;
			Status = adapter.OperationalStatus;
			DnsSuffix = properties.DnsSuffix;
			Mtu = properties.GetIPv4Properties().Mtu;
			DnsEnabled = properties.IsDnsEnabled;
			IsDynamicDnsEnabled = properties.IsDynamicDnsEnabled;
			IsReceiveOnly = adapter.IsReceiveOnly;
			SupportsMulticast = adapter.SupportsMulticast;
			Speed = adapter.Speed;
			BytesReceived = statistics.BytesReceived;
			BytesSent = statistics.BytesSent;
			IncomingPacketsDiscarded = statistics.IncomingPacketsDiscarded;
			IncomingPacketsWithErrors = statistics.IncomingPacketsWithErrors;
			IncomingUnknownProtocolPackets = statistics.IncomingUnknownProtocolPackets;
			NonUnicastPacketsReceived = statistics.NonUnicastPacketsReceived;
			NonUnicastPacketsSent = statistics.NonUnicastPacketsSent;
			OutgoingPacketsDiscarded = statistics.OutgoingPacketsDiscarded;
			OutgoingPacketsWithErrors = statistics.OutgoingPacketsWithErrors;
			OutputQueueLength = statistics.OutputQueueLength;
			UnicastPacketsReceived = statistics.UnicastPacketsReceived;
			UnicastPacketsSent = statistics.UnicastPacketsSent;

			if (adapter.Supports(NetworkInterfaceComponent.IPv4))
			{
				IpVersion = "IPv4";
			}
			if (adapter.Supports(NetworkInterfaceComponent.IPv6))
			{
				if (IpVersion.Length > 0)
				{
					IpVersion += ", ";
				}
				IpVersion += "IPv6";
			}
		}

		public string ToFormattedString()
		{
			return $"{Name}\n" +
				   $"{Global.GetInterfaceTypeName(NetworkInterfaceType)}\n" +
				   $"{Status switch { OperationalStatus.Up => Properties.Resources.ConnectedS, OperationalStatus.Down => Properties.Resources.Disconnected, _ => Status.ToString() }}\n" +
				   $"{IpVersion}\n" +
				   $"{DnsSuffix}\n" +
				   $"{Mtu}\n" +
				   $"{BoolToString(DnsEnabled)}\n" +
				   $"{BoolToString(IsDynamicDnsEnabled)}\n" +
				   $"{BoolToString(IsReceiveOnly)}\n" +
				   $"{BoolToString(SupportsMulticast)}\n" +
				   $"{Global.GetStorageUnit(Speed).Item2:0.00} {Global.UnitToString(Global.GetStorageUnit(Speed).Item1)}/s\n" +
				   $"{Global.GetStorageUnit(BytesReceived).Item2:0.00} {Global.UnitToString(Global.GetStorageUnit(BytesReceived).Item1)}\n" +
				   $"{Global.GetStorageUnit(BytesSent).Item2:0.00} {Global.UnitToString(Global.GetStorageUnit(BytesSent).Item1)}\n" +
				   $"{IncomingPacketsDiscarded}\n" +
				   $"{IncomingPacketsWithErrors}\n" +
				   $"{IncomingUnknownProtocolPackets}\n" +
				   $"{NonUnicastPacketsReceived}\n" +
				   $"{NonUnicastPacketsSent}\n" +
				   $"{OutgoingPacketsDiscarded}\n" +
				   $"{OutgoingPacketsWithErrors}\n" +
				   $"{OutputQueueLength}\n" +
				   $"{UnicastPacketsReceived}\n" +
				   $"{UnicastPacketsSent}";
		}

		public string ToLongFormattedString()
		{
			return $"{Properties.Resources.Name}: {Name}\n" +
				   $"{Properties.Resources.InterfaceType}: {Global.GetInterfaceTypeName(NetworkInterfaceType)}\n" +
				   $"{Properties.Resources.Status}: {Status switch { OperationalStatus.Up => Properties.Resources.ConnectedS, OperationalStatus.Down => Properties.Resources.Disconnected, _ => Status.ToString() }}\n" +
				   $"{Properties.Resources.IpVersion}: {IpVersion}\n" +
				   $"{Properties.Resources.DNSSuffix}: {DnsSuffix}\n" +
				   $"{Properties.Resources.MTU}: {Mtu}\n" +
				   $"{Properties.Resources.DnsEnabled}: {BoolToString(DnsEnabled)}\n" +
				   $"{Properties.Resources.DnsDynamicConfigured}: {BoolToString(IsDynamicDnsEnabled)}\n" +
				   $"{Properties.Resources.ReceiveOnly}: {BoolToString(IsReceiveOnly)}\n" +
				   $"{Properties.Resources.Multicast}: {BoolToString(SupportsMulticast)}\n" +
				   $"{Properties.Resources.Speed}: {Global.GetStorageUnit(Speed).Item2:0.00} {Global.UnitToString(Global.GetStorageUnit(Speed).Item1)}/s\n" +
				   $"{Properties.Resources.TotalBytesReceived}: {Global.GetStorageUnit(BytesReceived).Item2:0.00} {Global.UnitToString(Global.GetStorageUnit(BytesReceived).Item1)}\n" +
				   $"{Properties.Resources.TotalBytesSent}: {Global.GetStorageUnit(BytesSent).Item2:0.00} {Global.UnitToString(Global.GetStorageUnit(BytesSent).Item1)}\n" +
				   $"{Properties.Resources.IncomingPacketsDiscarded}: {IncomingPacketsDiscarded}\n" +
				   $"{Properties.Resources.IncomingPacketsWithErrors}: {IncomingPacketsWithErrors}\n" +
				   $"{Properties.Resources.IncomingUnknownProtocolPackets}: {IncomingUnknownProtocolPackets}\n" +
				   $"{Properties.Resources.NonUnicastPacketsReceived}: {NonUnicastPacketsReceived}\n" +
				   $"{Properties.Resources.NonUnicastPacketsSent}: {NonUnicastPacketsSent}\n" +
				   $"{Properties.Resources.OutgoingPacketsDiscarded}: {OutgoingPacketsDiscarded}\n" +
				   $"{Properties.Resources.OutgoingPacketsWithErrors}: {OutgoingPacketsWithErrors}\n" +
				   $"{Properties.Resources.OutputQueueLength}: {OutputQueueLength}\n" +
				   $"{Properties.Resources.UnicastPacketsReceived}: {UnicastPacketsReceived}\n" +
				   $"{Properties.Resources.UnicastPacketsSent}: {UnicastPacketsSent}";
		}

		private string BoolToString(bool b) => b ? Properties.Resources.Yes : Properties.Resources.No;
	}
}
