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
using InternetTest.Models;
using ManagedNativeWifi;
using System.Net.NetworkInformation;

namespace InternetTest.Helpers;
public static class NetworkHelper
{
	public static string? GetCurrentWifiSSID()
	{
		var connections = NativeWifi.EnumerateConnectedNetworkSsids().Select(x => x.ToString());
		return connections.Any() ? connections.First() : null;
	}

	public static Network GetCurrentNetwork()
	{
		foreach (var interfaceId in NativeWifi.EnumerateInterfaces().Where(x => x.State is InterfaceState.Connected).Select(x => x.Id))
		{
			var (result, cc) = NativeWifi.GetCurrentConnection(interfaceId);
			if (result == ActionResult.Success)
			{
				return new()
				{
					Ssid = cc.Ssid.ToString(),
					SignalQuality = cc.SignalQuality,
					BssType = cc.BssType.ToString(),
					IsSecurityEnabled = cc.IsSecurityEnabled,
					ProfileName = cc.ProfileName,
				};
			}
		}
		return new Network();
	}

	public static int GetCurrentSpeed()
	{
		NetworkInterface[] adapters = [.. NetworkInterface.GetAllNetworkInterfaces().OrderByDescending(x => x.GetIPStatistics().BytesReceived)];
		foreach (NetworkInterface adapter in adapters)
		{
			if (adapter.OperationalStatus == OperationalStatus.Up && adapter.NetworkInterfaceType != NetworkInterfaceType.Loopback)
			{
				long speed = adapter.Speed;
				if (speed > 0)
				{
					return (int)(speed / 1_000_000); // Convert to Mbps
				}
			}
		}
		return 0; // No active network found or speed is zero
	}
}
