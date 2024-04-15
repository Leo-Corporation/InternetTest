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
using InternetTest.Enums;
using InternetTest.Pages;
using ManagedNativeWifi;
using Microsoft.Win32;
using PeyrSharp.Enums;
using PeyrSharp.Env;
using Synethia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace InternetTest.Classes;
public static class Global
{

#if NIGHTLY
	private static DateTime Date => System.IO.File.GetLastWriteTime(System.Reflection.Assembly.GetEntryAssembly().Location);

	public static string Version => $"8.2.1.2404-nightly{Date:yyMM.dd@HHmm}";

#else
	public static string Version => "8.2.1.2404";
#endif
	public static string LastVersionLink => "https://raw.githubusercontent.com/Leo-Corporation/LeoCorp-Docs/master/Liens/Update%20System/InternetTest/7.0/Version.txt";
	internal static string SynethiaPath => $@"{FileSys.AppDataPath}\Léo Corporation\InternetTest Pro\NewSynethiaConfig.json";
	public static bool IsConfidentialModeEnabled { get; set; } = false;
	public static Settings Settings { get; set; } = SettingsManager.Load();
	public static SynethiaConfig SynethiaConfig { get; set; } = LoadConfig();
	public static SynethiaConfig DefaultConfig => new()
	{
		PagesInfo =
		[
			new("DownDetector"),
			new("DNS"),
			new("WiFiNetworks"),
			new("LocateIP"),
			new("IPConfig"),
			new("Ping"),
			new("Traceroute"),
			new("WiFiPasswords"),
			new("Requests"),
		],
		ActionsInfo =
		[
			new(0, "DownDetector.Test"),
			new(1, "DNS.GetInfo"),
			new(2, "WiFiNetworks.Scan"),
			new(3, "LocateIP.Locate"),
			new(4, "IPConfig.Get"),
			new(5, "Ping.Execute"),
			new(6, "Traceroute.Execute"),
			new(7, "WiFiPasswords.Get"),
			new(8, "Request.Make"),
		]
	};

	public static SynethiaConfig LoadConfig()
	{
		var config = SynethiaManager.Load(SynethiaPath, DefaultConfig);

		bool hasRequest = false;
		for (int i = 0; i < config.PagesInfo.Count; i++)
		{
			if (config.PagesInfo[i].Name == "Requests")
			{
				hasRequest = true;
				break;
			}
		}

		if (!hasRequest)
		{
			config.PagesInfo.Add(new("Requests"));
			config.ActionsInfo.Add(new(8, "Request.Make"));
		}

		return config;
	}

	public static History History { get; set; } = HistoryManager.Load();

	public static HomePage? HomePage { get; set; }
	public static HistoryPage? HistoryPage { get; set; }
	public static SettingsPage? SettingsPage { get; set; }
	public static DownDetectorPage? DownDetectorPage { get; set; }
	public static LocateIpPage? LocateIpPage { get; set; }
	public static PingPage? PingPage { get; set; }
	public static IpConfigPage? IpConfigPage { get; set; }
	public static WiFiPasswordsPage? WiFiPasswordsPage { get; set; }
	public static DnsPage? DnsPage { get; set; }
	public static TraceroutePage? TraceroutePage { get; set; }
	public static WiFiNetworksPage? WiFiNetworksPage { get; set; }
	public static RequestsPage? RequestsPage { get; set; }

	public static string GetHiSentence
	{
		get
		{
			if (DateTime.Now.Hour >= 21 && DateTime.Now.Hour <= 7) // If between 9PM & 7AM
			{
				return Properties.Resources.GoodNight + ", " + Environment.UserName + "."; // Return the correct value
			}
			else if (DateTime.Now.Hour >= 7 && DateTime.Now.Hour <= 12) // If between 7AM - 12PM
			{
				return Properties.Resources.Hi + ", " + Environment.UserName + "."; // Return the correct value
			}
			else if (DateTime.Now.Hour >= 12 && DateTime.Now.Hour <= 17) // If between 12PM - 5PM
			{
				return Properties.Resources.GoodAfternoon + ", " + Environment.UserName + "."; // Return the correct value
			}
			else if (DateTime.Now.Hour >= 17 && DateTime.Now.Hour <= 21) // If between 5PM - 9PM
			{
				return Properties.Resources.GoodEvening + ", " + Environment.UserName + "."; // Return the correct value
			}
			else
			{
				return Properties.Resources.Hi + ", " + Environment.UserName + "."; // Return the correct value
			}
		}
	}

	public static Dictionary<AppPages, string> AppPagesFilledIcons => new()
	{
		{ AppPages.Home, "\uF488" },
		{ AppPages.History, "\uF486" },
		{ AppPages.Settings, "\uF6B3" },
		{ AppPages.Status, "\uF462" },
		{ AppPages.DownDetector, "\uFB71" },
		{ AppPages.MyIP, "\uF503" },
		{ AppPages.LocateIP, "\uF538" },
		{ AppPages.Ping, "\uF4FB" },
		{ AppPages.IPConfig, "\uF848" },
		{ AppPages.WiFiPasswords, "\uF8CC" },
		{ AppPages.DnsTool, "\uF464" },
		{ AppPages.TraceRoute, "\uF683" },
		{ AppPages.WiFiNetworks, "\uF8C5" },
		{ AppPages.Requests, "\uF6A3" },
	};
	public static Dictionary<AppPages, string> AppPagesName => new()
	{
		{ AppPages.Home, Properties.Resources.Home },
		{ AppPages.History, Properties.Resources.History },
		{ AppPages.Settings, Properties.Resources.Settings },
		{ AppPages.Status, Properties.Resources.Status },
		{ AppPages.DownDetector, Properties.Resources.DownDetector },
		{ AppPages.MyIP, Properties.Resources.MyIP },
		{ AppPages.LocateIP, Properties.Resources.LocateIP },
		{ AppPages.Ping, Properties.Resources.Ping },
		{ AppPages.IPConfig, Properties.Resources.IPConfig },
		{ AppPages.WiFiPasswords, Properties.Resources.WifiPasswords },
		{ AppPages.DnsTool, Properties.Resources.DNSTool},
		{ AppPages.TraceRoute, Properties.Resources.TraceRoute},
		{ AppPages.WiFiNetworks, Properties.Resources.WiFiNetworks},
		{ AppPages.Requests, Properties.Resources.Requests},
	};

	public static List<AppPages> GetMostRelevantPages(SynethiaConfig synethiaConfig)
	{
		Dictionary<AppPages, double> appScores = new()
		{
			{ AppPages.DownDetector, synethiaConfig.PagesInfo[0].Score },
			{ AppPages.DnsTool, synethiaConfig.PagesInfo[1].Score },
			{ AppPages.WiFiNetworks, synethiaConfig.PagesInfo[2].Score },
			{ AppPages.LocateIP, synethiaConfig.PagesInfo[3].Score },
			{ AppPages.IPConfig, synethiaConfig.PagesInfo[4].Score },
			{ AppPages.Ping, synethiaConfig.PagesInfo[5].Score },
			{ AppPages.TraceRoute, synethiaConfig.PagesInfo[6].Score },
			{ AppPages.WiFiPasswords, synethiaConfig.PagesInfo[7].Score },
			{ AppPages.Requests, synethiaConfig.PagesInfo[8].Score },
		};

		var sorted = appScores.OrderByDescending(x => x.Value);

		return (from item in sorted select item.Key).ToList();
	}

	public static List<AppPages> DefaultRelevantPages =>
	[
		AppPages.LocateIP,
		AppPages.WiFiPasswords,
		AppPages.DownDetector,
		AppPages.WiFiNetworks,
		AppPages.Ping,
		AppPages.TraceRoute,
		AppPages.IPConfig,
		AppPages.DnsTool,
		AppPages.Requests,
	];

	public static List<ActionInfo> DefaultRelevantActions =>
	[
		new(4, "IPConfig.Get"),
		new(2, "WiFiNetworks.Scan"),
		new(3, "LocateIP.Locate"),
		new(7, "WiFiPasswords.Get"),
		new(0, "DownDetector.Test"),
		new(5, "Ping.Execute"),
		new(1, "DNS.GetInfo"),
		new(6, "Traceroute.Execute"),
		new(7, "Request.Make"),
	];

	public static Dictionary<AppActions, string> ActionsIcons => new()
	{
		{ AppActions.DownDetectorRequest, "\uF2E9" },
		{ AppActions.GetIPConfig, "\uF8D1" },
		{ AppActions.GetWiFiPasswords, "\uF5A8" },
		{ AppActions.LocateIP, "\uF506" },
		{ AppActions.MyIP, "\uF569" },
		{ AppActions.Ping, "\uF4FB" },
		{ AppActions.Test, "\uF612" },
		{ AppActions.GetDnsInfo, "\uF69C" },
		{ AppActions.TraceRoute, "\uF683" },
		{ AppActions.ConnectWiFi, "\uF614" },
		{ AppActions.MakeRequest, "\uF6A3" },
	};

	public static Dictionary<AppActions, string> ActionsString => new()
	{
		{ AppActions.DownDetectorRequest, Properties.Resources.TestWebsite },
		{ AppActions.GetIPConfig, Properties.Resources.GetIPConfig },
		{ AppActions.GetWiFiPasswords, Properties.Resources.GetWiFi },
		{ AppActions.LocateIP, Properties.Resources.LocateAnIP },
		{ AppActions.MyIP, Properties.Resources.GetMyIP },
		{ AppActions.Ping, Properties.Resources.MakePing },
		{ AppActions.Test, Properties.Resources.TestConnection },
		{ AppActions.GetDnsInfo, Properties.Resources.GetDnsInfo },
		{ AppActions.TraceRoute, Properties.Resources.ExecuteTraceRoute },
		{ AppActions.ConnectWiFi, Properties.Resources.ConnectWiFi },
		{ AppActions.MakeRequest, Properties.Resources.Send },
	};

	public static SolidColorBrush GetBrushFromResource(string resourceName) => (SolidColorBrush)Application.Current.Resources[resourceName];

	public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
	{
		if (depObj == null) yield return (T)Enumerable.Empty<T>();
		for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
		{
			DependencyObject ithChild = VisualTreeHelper.GetChild(depObj, i);
			if (ithChild == null) continue;
			if (ithChild is T t) yield return t;
			foreach (T childOfChild in FindVisualChildren<T>(ithChild)) yield return childOfChild;
		}
	}

	public static bool IsUrlValid(string url)
	{
		if (!url.StartsWith("http://") || !url.StartsWith("https://"))
		{
			url = "https://" + url;
		}
		return Uri.TryCreate(url, UriKind.Absolute, out Uri? uriResult)
			&& (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
	}

	public async static Task<IPInfo?> GetIPInfoAsync(string ip)
	{
		HttpClient httpClient = new();
		string result = await httpClient.GetStringAsync($"http://ip-api.com/json/{ip}");

		return JsonSerializer.Deserialize<IPInfo>(result);
	}

	public static bool IsIpValid(string ip)
	{
		if (ip == "") return true; // This is valid, it will return the user's current IP

		if (IsUrlValid(ip)) return true; // This is valid, it is possible to get IP info from a URL

		// Initialize a regex that checks if an IP is valid
		Regex ipRegex = new(@"^(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$");

		// Check if the IP is valid
		return ipRegex.IsMatch(ip);
	}

	/// <summary>
	/// Changes the application's theme.
	/// </summary>
	public static void ChangeTheme()
	{
		App.Current.Resources.MergedDictionaries.Clear();
		ResourceDictionary resourceDictionary = []; // Create a resource dictionary

		bool isDark = Settings.Theme == Themes.Dark;
		if (Settings.Theme == Themes.System)
		{
			isDark = IsSystemThemeDark(); // Set
		}

		if (isDark) // If the dark theme is on
		{
			resourceDictionary.Source = new Uri("..\\Themes\\Dark.xaml", UriKind.Relative); // Add source
		}
		else
		{
			resourceDictionary.Source = new Uri("..\\Themes\\Light.xaml", UriKind.Relative); // Add source
		}

		App.Current.Resources.MergedDictionaries.Add(resourceDictionary); // Add the dictionary
	}

	public static bool IsSystemThemeDark()
	{
		if (Sys.CurrentWindowsVersion != WindowsVersion.Windows10 && Sys.CurrentWindowsVersion != WindowsVersion.Windows11)
		{
			return false; // Avoid errors on older OSs
		}

		var t = Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", "SystemUsesLightTheme", "1");
		return t switch
		{
			0 => true,
			1 => false,
			_ => false
		}; // Return
	}

	public static void ChangeLanguage()
	{
		switch (Settings.Language) // For each case
		{
			case Languages.Default: // No language
				break;
			case Languages.en_US: // English (US)
				Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US"); // Change
				break;

			case Languages.fr_FR: // French (FR)
				Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("fr-FR"); // Change
				break;

			case Languages.zh_CN: // Chinese (CN)
				Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("zh-CN"); // Change
				break;

			case Languages.it_IT: // Italian (Italy)
				Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("it-IT"); // Change
				break;
			default: // No language
				break;
		}
	}

	/// <summary>
	/// Get the point for the Google Maps map provider.
	/// </summary>
	/// <param name="lat">The latitude of the point.</param>
	/// <param name="lon">The longitude of the point.</param>
	/// <returns>A <see cref="string"/> value like <c>XX° XX.XXX' N/S, XX° XX' E/W</c>.</returns>
	public static string GetGoogleMapsPoint(double lat, double lon)
	{
		int deg = (int)lat; // Get integer
		int deg2 = (int)lon; // Get integer

		double d = (lat - deg) * 60d;
		double d2 = (lon - deg2) * 60d;

		string fDir = lat >= 0 ? "N" : "S"; // Get if the location is in the North or South
		string sDir = lon >= 0 ? "E" : "W"; // Get if the location is in the East or West

		string sD = d.ToString().Replace(",", "."); // Ensure to use . instead of ,
		string sD2 = d2.ToString().Replace(",", "."); // Ensure to use . instead of ,

		return $"{deg}° {sD}' {fDir}, {deg2}° {sD2}' {sDir}".Replace("-", "");
	}

	public static string ReplaceAllCharactersByAnotherOne(string chars, string replaceChar)
	{
		string r = "";
		for (int i = 0; i < chars.Length; i++)
		{
			r += replaceChar;
		}
		return r;
	}

	public static bool DateIsInRange(int startDate, int endDate, int checkDate) => (startDate <= checkDate && checkDate <= endDate);

	public static bool IsSuccessfulCode(int code) => code >= 200 && code < 400;

	public static async Task<List<TracertStep>> Trace(string target, int maxHops, int timeout)
	{
		List<TracertStep> steps = [];

		for (int ttl = 1; ttl <= maxHops; ttl++)
		{
			PingReply reply = await TraceRoute(target, ttl, timeout);

			TracertStep step = new()
			{
				TTL = ttl,
				Address = reply.Address,
				RoundtripTime = reply.RoundtripTime,
				Status = reply.Status
			};

			steps.Add(step);

			if (reply.Status == IPStatus.Success)
				break;
		}

		return steps;
	}

	public static Task<PingReply> TraceRoute(string targetAddress, int ttl, int timeout)
	{
		using Ping pingSender = new();
		PingOptions options = new()
		{
			Ttl = ttl
		};

		byte[] buffer = new byte[32];
		return pingSender.SendPingAsync(targetAddress, timeout, buffer, options);
	}

	internal static List<NetworkInfo> GetWiFis()
	{
		var availableNetworks = NativeWifi.EnumerateAvailableNetworks()
			.Select(x => new NetworkInfo
			{
				Ssid = x.Ssid.ToString(),
				SignalQuality = x.SignalQuality,
				BssType = x.BssType.ToString(),
				IsSecurityEnabled = x.IsSecurityEnabled,
				ProfileName = x.ProfileName,
				InterfaceDescription = x.Interface.Description
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

	public static async Task<bool> ConnectAsync(string ssid, string password)
	{
		var availableNetwork = NativeWifi.EnumerateAvailableNetworks()
			.Where(x => x.Ssid.ToString() == ssid)
			.FirstOrDefault();

		if (availableNetwork is null)
			return false;

		if (availableNetwork.ProfileName is { Length: 0 })
		{
			var profileXml = GetWpa2PersonalProfileXml(ssid, password);
			NativeWifi.SetProfile(availableNetwork.Interface.Id, ProfileType.AllUser, profileXml, null, false); ;
		}

		var connectionResult = await NativeWifi.ConnectNetworkAsync(
			interfaceId: availableNetwork.Interface.Id,
			profileName: ssid,
			bssType: availableNetwork.BssType,
			timeout: TimeSpan.FromSeconds(10));

		if (!connectionResult)
		{
			Console.WriteLine("Failed to connect.");
			return false;
		}

		Console.WriteLine("Connected successfully.");
		return true;
	}

	public static (StorageUnits, double) GetStorageUnit(long size)
	{
		StorageUnits unit = size switch
		{
			long s when s >= Math.Pow(1024, 5) => StorageUnits.Petabyte,
			long s when s >= Math.Pow(1024, 4) => StorageUnits.Terabyte,
			long s when s >= 1073741824 => StorageUnits.Gigabyte,
			long s when s >= 1048576 => StorageUnits.Megabyte,
			long s when s >= 1024 => StorageUnits.Kilobyte,
			_ => StorageUnits.Byte,
		};

		double convertedSize = size / Math.Pow(1024, (int)unit);

		return (unit, convertedSize);
	}

	public static string UnitToString(StorageUnits unit)
	{
		try
		{
			string[] units = Properties.Resources.Units.Split(",");
			return units[(int)unit];
		}
		catch
		{
			return "";
		}
	}

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
			return types[i];
		}
		catch
		{
			return "";
		}
	}

	public static string GetCurrentWifiSSID()
	{
		var connections = NativeWifi.EnumerateInterfaceConnections();

		foreach (var connection in connections)
		{
			if (connection.IsConnected)
			{
				return connection.ProfileName;
			}
		}
		return null;
	}

}