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
using System.Net.NetworkInformation;

namespace InternetTest.Models;

public class NetworkAdapter
{
	public string Name { get; set; }
	public NetworkInterfaceType NetworkInterfaceType { get; set; }
	public OperationalStatus Status { get; set; }
	public string? IpVersion { get; set; }
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

	public NetworkAdapter(NetworkInterface adapter)
	{
		IPInterfaceProperties properties = adapter.GetIPProperties();
		IPInterfaceStatistics statistics = adapter.GetIPStatistics();
		Name = adapter.Description;
		NetworkInterfaceType = adapter.NetworkInterfaceType;
		Status = adapter.OperationalStatus;
		DnsSuffix = properties.DnsSuffix;
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
			Mtu = properties.GetIPv4Properties().Mtu;
		}
		if (adapter.Supports(NetworkInterfaceComponent.IPv6))
		{
			if (IpVersion?.Length > 0)
			{
				IpVersion += ", ";
			}
			IpVersion += "IPv6";
		}
	}

	public string ToFormattedString()
	{
		return $"{Name}\n" +
			   $"{GetInterfaceTypeName(NetworkInterfaceType)}\n" +
			   $"{Status switch { OperationalStatus.Up => Properties.Resources.ConnectedS, OperationalStatus.Down => Properties.Resources.Disconnected, _ => Status.ToString() }}\n" +
			   $"{IpVersion}\n" +
			   $"{DnsSuffix}\n" +
			   $"{Mtu}\n" +
			   $"{BoolToString(DnsEnabled)}\n" +
			   $"{BoolToString(IsDynamicDnsEnabled)}\n" +
			   $"{BoolToString(IsReceiveOnly)}\n" +
			   $"{BoolToString(SupportsMulticast)}\n" +
			   $"{StorageUnitHelper.GetStorageUnit(Speed).Item2:0.00} {StorageUnitHelper.UnitToString(StorageUnitHelper.GetStorageUnit(Speed).Item1)}/s\n" +
			   $"{StorageUnitHelper.GetStorageUnit(BytesReceived).Item2:0.00} {StorageUnitHelper.UnitToString(StorageUnitHelper.GetStorageUnit(BytesReceived).Item1)}\n" +
			   $"{StorageUnitHelper.GetStorageUnit(BytesSent).Item2:0.00} {StorageUnitHelper.UnitToString(StorageUnitHelper.GetStorageUnit(BytesSent).Item1)}\n" +
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
			   $"{Properties.Resources.InterfaceType}: {GetInterfaceTypeName(NetworkInterfaceType)}\n" +
			   $"{Properties.Resources.Status}: {Status switch { OperationalStatus.Up => Properties.Resources.ConnectedS, OperationalStatus.Down => Properties.Resources.Disconnected, _ => Status.ToString() }}\n" +
			   $"{Properties.Resources.IpVersion}: {IpVersion}\n" +
			   $"{Properties.Resources.DNSSuffix}: {DnsSuffix}\n" +
			   $"{Properties.Resources.MTU}: {Mtu}\n" +
			   $"{Properties.Resources.DnsEnabled}: {BoolToString(DnsEnabled)}\n" +
			   $"{Properties.Resources.DnsDynamicConfigured}: {BoolToString(IsDynamicDnsEnabled)}\n" +
			   $"{Properties.Resources.ReceiveOnly}: {BoolToString(IsReceiveOnly)}\n" +
			   $"{Properties.Resources.Multicast}: {BoolToString(SupportsMulticast)}\n" +
			   $"{Properties.Resources.Speed}: {StorageUnitHelper.GetStorageUnit(Speed).Item2:0.00} {StorageUnitHelper.UnitToString(StorageUnitHelper.GetStorageUnit(Speed).Item1)}/s\n" +
			   $"{Properties.Resources.TotalBytesReceived}: {StorageUnitHelper.GetStorageUnit(BytesReceived).Item2:0.00} {StorageUnitHelper.UnitToString(StorageUnitHelper.GetStorageUnit(BytesReceived).Item1)}\n" +
			   $"{Properties.Resources.TotalBytesSent}: {StorageUnitHelper.GetStorageUnit(BytesSent).Item2:0.00} {StorageUnitHelper.UnitToString(StorageUnitHelper.GetStorageUnit(BytesSent).Item1)}\n" +
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

	private static string BoolToString(bool b) => b ? Properties.Resources.Yes : Properties.Resources.No;

	public static string GetInterfaceTypeName(NetworkInterfaceType networkInterfaceType)
	{
		try
		{
			string[] types = Properties.Resources.Types.Split(",");
			int i = networkInterfaceType switch
			{
				NetworkInterfaceType.Unknown => 0,
				NetworkInterfaceType.Ethernet => 1,
				NetworkInterfaceType.TokenRing => 2,
				NetworkInterfaceType.Fddi => 3,
				NetworkInterfaceType.BasicIsdn => 4,
				NetworkInterfaceType.PrimaryIsdn => 5,
				NetworkInterfaceType.Ppp => 6,
				NetworkInterfaceType.Loopback => 7,
				NetworkInterfaceType.Ethernet3Megabit => 8,
				NetworkInterfaceType.Slip => 9,
				NetworkInterfaceType.Atm => 10,
				NetworkInterfaceType.GenericModem => 11,
				NetworkInterfaceType.FastEthernetT => 12,
				NetworkInterfaceType.Isdn => 13,
				NetworkInterfaceType.FastEthernetFx => 14,
				NetworkInterfaceType.Wireless80211 => 15,
				NetworkInterfaceType.AsymmetricDsl => 16,
				NetworkInterfaceType.RateAdaptDsl => 17,
				NetworkInterfaceType.SymmetricDsl => 18,
				NetworkInterfaceType.VeryHighSpeedDsl => 19,
				NetworkInterfaceType.IPOverAtm => 20,
				NetworkInterfaceType.GigabitEthernet => 21,
				NetworkInterfaceType.Tunnel => 22,
				NetworkInterfaceType.MultiRateSymmetricDsl => 23,
				NetworkInterfaceType.HighPerformanceSerialBus => 24,
				NetworkInterfaceType.Wman => 25,
				NetworkInterfaceType.Wwanpp => 26,
				NetworkInterfaceType.Wwanpp2 => 27,
				_ => 0 // Handle any other cases or invalid values as needed.
			};

			if ((int)networkInterfaceType == 53)
				return Properties.Resources.PropVirtual;

			return types[i];
		}
		catch
		{
			return Properties.Resources.Unknown;
		}
	}


	public string Icon
	{
		get
		{
			if ((int)NetworkInterfaceType == 53) // Virtual
				return "\uFCA7";


			if (Name.Contains("Bluetooth")) return "\uF1DF";
			if (Name.Contains("Tunnel")) return "\uFCA7";

			return NetworkInterfaceType switch
			{
				NetworkInterfaceType.Tunnel => "\uFCA7",
				NetworkInterfaceType.Ethernet => "\uFB32",
				NetworkInterfaceType.Ethernet3Megabit => "\uFB32",
				NetworkInterfaceType.FastEthernetFx => "\uFB32",
				NetworkInterfaceType.FastEthernetT => "\uFB32",
				NetworkInterfaceType.GigabitEthernet => "\uFB32",
				NetworkInterfaceType.AsymmetricDsl => "\uF864",
				NetworkInterfaceType.RateAdaptDsl => "\uF864",
				NetworkInterfaceType.SymmetricDsl => "\uF864",
				NetworkInterfaceType.VeryHighSpeedDsl => "\uF864",
				NetworkInterfaceType.Loopback => "\uF18E",
				NetworkInterfaceType.BasicIsdn => "\uF57E",
				NetworkInterfaceType.PrimaryIsdn => "\uF57E",
				NetworkInterfaceType.Isdn => "\uF57E",
				_ => "\uF8AC"
			};

		}
	}
}
