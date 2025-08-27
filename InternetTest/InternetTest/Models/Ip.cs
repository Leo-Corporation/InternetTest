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
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace InternetTest.Models;
public class Ip
{
	[JsonPropertyName("query")]
	public string? Query { get; set; }

	[JsonPropertyName("status")]
	public string? Status { get; set; }

	[JsonPropertyName("country")]
	public string? Country { get; set; }

	[JsonPropertyName("countryCode")]
	public string? CountryCode { get; set; }

	[JsonPropertyName("district")]
	public string? District { get; set; }

	[JsonPropertyName("region")]
	public string? Region { get; set; }

	[JsonPropertyName("regionName")]
	public string? RegionName { get; set; }

	[JsonPropertyName("city")]
	public string? City { get; set; }

	[JsonPropertyName("zip")]
	public string? Zip { get; set; }

	[JsonPropertyName("lat")]
	public double Lat { get; set; }

	[JsonPropertyName("lon")]
	public double Lon { get; set; }

	[JsonPropertyName("timezone")]
	public string? Timezone { get; set; }

	[JsonPropertyName("isp")]
	public string? Isp { get; set; }

	[JsonPropertyName("org")]
	public string? Org { get; set; }

	[JsonPropertyName("as")]
	public string? As { get; set; }

	[JsonPropertyName("mobile")]
	public bool? Mobile { get; set; }

	[JsonPropertyName("proxy")]
	public bool? Proxy { get; set; }

	[JsonPropertyName("hosting")]
	public bool? Hosting { get; set; }

	public override string ToString() => $"{Properties.Resources.Country}: {Country}\n" +
			$"{Properties.Resources.Region}: {RegionName}\n" +
			$"{Properties.Resources.City}: {City}\n" +
			$"{Properties.Resources.ZIPCode}: {Zip}\n" +
			$"{Properties.Resources.Latitude}: {Lat}\n" +
			$"{Properties.Resources.Longitude}: {Lon}\n" +
			$"{Properties.Resources.Timezone}: {Timezone}\n" +
			$"{Properties.Resources.ISP}: {Isp}";

	public static async Task<Ip> GetIp(string ip)
	{
		try
		{
			HttpClient httpClient = new();
			string result = await httpClient.GetStringAsync($"http://ip-api.com/json/{ip}");
			return JsonSerializer.Deserialize<Ip>(result) ?? new();
		}
		catch
		{
			return new();
		}
	}

	public static bool IsValid(string ip)
	{
		if (ip == "") return true; // This is valid, it will return the user's current IP

		if (IsUrlValid(ip)) return true; // This is valid, it is possible to get IP info from a URL

		// Initialize a regex that checks if an IP is valid
		Regex ipRegex = new(@"^(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$");

		// Check if the IP is valid
		return ipRegex.IsMatch(ip);
	}

	private static bool IsUrlValid(string url)
	{
		if (!url.StartsWith("http://") || !url.StartsWith("https://"))
		{
			url = "https://" + url;
		}
		return Uri.TryCreate(url, UriKind.Absolute, out Uri? uriResult)
			&& (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
	}
}