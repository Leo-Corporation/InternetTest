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
using ManagedNativeWifi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetTest.Models;

public class WiFiNetwork
{
	public string? Ssid { get; set; }
	public int SignalQuality { get; set; }
	public string? BssType { get; set; }
	public bool IsSecurityEnabled { get; set; }
	public string? ProfileName { get; set; }
	public string? InterfaceDescription { get; set; }
	public int? Channel { get; set; }
	public int? Frequency { get; set; }
	public double? Band { get; set; }

	public override string ToString()
	{
		return $"SSID: {Ssid}\n" +
			new string('=', (Ssid ?? "").Length + 6) + "\n" +
			$"Signal Strength: {SignalQuality}\n" +
			$"ProfileName: {ProfileName}\n" +
			$"Interface: {InterfaceDescription}\n" +
			$"BssType: {BssType}\n" +
			$"IsSecurityEnabled: {IsSecurityEnabled}\n" +
			$"Channel: {Channel}\n" +
			$"Band: {Band} GHz\n" +
			$"Frequency: {Frequency} kHz\n";
	}

	public static List<WiFiNetwork> GetWiFis()
	{
		var availableNetworks = NativeWifi.EnumerateAvailableNetworks()
			.Select(x => new WiFiNetwork
			{
				Ssid = x.Ssid.ToString(),
				SignalQuality = x.SignalQuality,
				BssType = x.BssType.ToString(),
				IsSecurityEnabled = x.IsSecurityEnabled,
				ProfileName = x.ProfileName,
				InterfaceDescription = x.InterfaceInfo.Description
			})
			.ToList();

		var bssNetworks = NativeWifi.EnumerateBssNetworks()
			.Select(x => new { x.Ssid, x.Channel, x.Band, x.Frequency })
			.ToList();

		foreach (var network in availableNetworks)
		{
			var bssNetwork = bssNetworks.FirstOrDefault(x => x.Ssid.ToString() == network.Ssid);
			if (bssNetwork != null)
			{
				network.Channel = bssNetwork.Channel;
				network.Frequency = bssNetwork.Frequency;
				network.Band = bssNetwork.Band;
			}
		}
		return availableNetworks;
	}
}
