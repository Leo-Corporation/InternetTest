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

namespace InternetTest.Models;
public record WindowsIpConfig(
	string? Name,
	string? IPv4Address,
	string? IPv4Mask,
	string? IPv4Gateway,
	string? IPv6Address,
	string? IPv6Gateway,
	string? DNSSuffix,
	OperationalStatus? Status)
{
	public override string ToString() => $"{Properties.Resources.Name}: {Name}\n" +
		$"{Properties.Resources.IPv4Address}: {IPv4Address}\n" +
		$"{Properties.Resources.SubnetMask}: {IPv4Mask}\n" +
		$"{Properties.Resources.GatewayIPv4}: {IPv4Gateway ?? Properties.Resources.None}\n" +
		$"{Properties.Resources.IPv6Address}: {IPv6Address}\n" +
		$"{Properties.Resources.GatewayIPv6}: {IPv6Gateway ?? Properties.Resources.None}\n" +
		$"{Properties.Resources.DNSSuffix}: {DNSSuffix ?? Properties.Resources.None}\n" +
		$"{Properties.Resources.Status}: {(Status == OperationalStatus.Up ? Properties.Resources.ConnectedS : Properties.Resources.Disconnected)}";

	public static WindowsIpConfig? FromNetworkInterface(NetworkInterface networkInterface)
	{
		var props = networkInterface.GetIPProperties();
		return props.UnicastAddresses.Count == 0
			? null
			: new WindowsIpConfig(
			networkInterface.Name,
			props.UnicastAddresses.FirstOrDefault(x => x.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)?.Address.ToString(),
			props.UnicastAddresses.FirstOrDefault(x => x.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)?.IPv4Mask.ToString(),
			props.GatewayAddresses.FirstOrDefault(x => x.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)?.Address.ToString(),
			props.UnicastAddresses.FirstOrDefault(x => x.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)?.Address.ToString(),
			props.GatewayAddresses.FirstOrDefault(x => x.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)?.Address.ToString(),
			props.DnsSuffix,
			networkInterface.OperationalStatus);
	}
}