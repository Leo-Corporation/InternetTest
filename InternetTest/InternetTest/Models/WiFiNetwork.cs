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

namespace InternetTest.Models;

public class WiFiNetwork
{
	public string? Ssid { get; set; }
	public int SignalQuality { get; set; }
	public BssType? BssType { get; set; }
	public bool IsSecurityEnabled { get; set; }
	public string? ProfileName { get; set; }
	public string? InterfaceDescription { get; set; }
	public int? Channel { get; set; }
	public int? Frequency { get; set; }
	public double? Band { get; set; }

	public InterfaceInfo? Interface { get; set; }

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
				BssType = x.BssType,
				IsSecurityEnabled = x.IsSecurityEnabled,
				ProfileName = x.ProfileName,
				InterfaceDescription = x.InterfaceInfo.Description,
				Interface = x.InterfaceInfo
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

	public async Task<bool> ConnectAsync(string password = "")
	{
		if (Interface is null || Ssid is null)
			return false;

		if (ProfileName is { Length: 0 })
		{
			var profileXml = GetWpa2PersonalProfileXml(Ssid, password);
			NativeWifi.SetProfile(Interface.Id, ProfileType.AllUser, profileXml, null, false);
		}

		var connectionResult = await NativeWifi.ConnectNetworkAsync(
			interfaceId: Interface.Id,
			profileName: Ssid,
			bssType: BssType ?? ManagedNativeWifi.BssType.Any,
			timeout: TimeSpan.FromSeconds(10));

		if (!connectionResult)
			return false;

		return true;
	}

	public static string GetWpa2PersonalProfileXml(string ssid, string password)
	{
		var profileXml = $@"<?xml version=""1.0""?>
	<WLANProfile xmlns=""http://www.microsoft.com/networking/WLAN/profile/v1"">
		<name>{ssid}</name>
		<SSIDConfig>
			<SSID>
				<name>{ssid}</name>
			</SSID>
		</SSIDConfig>
		<connectionType>ESS</connectionType>
		<connectionMode>auto</connectionMode>
		<MSM>
			<security>
				<authEncryption>
					<authentication>WPA2PSK</authentication>
					<encryption>AES</encryption>
					<useOneX>false</useOneX>
				</authEncryption>
				<sharedKey>
					<keyType>passPhrase</keyType>
					<protected>false</protected>
					<keyMaterial>{password}</keyMaterial>
				</sharedKey>
			</security>
		</MSM>
	</WLANProfile>";
		return profileXml;
	}
}
