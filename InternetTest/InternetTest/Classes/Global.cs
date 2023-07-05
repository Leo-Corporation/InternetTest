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
using Microsoft.Win32;
using PeyrSharp.Core.Maths;
using PeyrSharp.Enums;
using PeyrSharp.Env;
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

	public static string Version => $"7.6.0.2307-nightly{Date:yyMM.dd@HHmm}";

#else
	public static string Version => "7.6.0.2307-pre1";
#endif
	public static string LastVersionLink => "https://raw.githubusercontent.com/Leo-Corporation/LeoCorp-Docs/master/Liens/Update%20System/InternetTest/7.0/Version.txt";
	public static bool IsConfidentialModeEnabled { get; set; } = false;
	public static Settings Settings { get; set; } = SettingsManager.Load();
	public static SynethiaConfig SynethiaConfig { get; set; } = SynethiaManager.Load();
	public static History History { get; set; } = HistoryManager.Load();

	public static HomePage? HomePage { get; set; }
	public static HistoryPage? HistoryPage { get; set; }
	public static SettingsPage? SettingsPage { get; set; }
	public static StatusPage? StatusPage { get; set; }
	public static DownDetectorPage? DownDetectorPage { get; set; }
	public static MyIpPage? MyIpPage { get; set; }
	public static LocateIpPage? LocateIpPage { get; set; }
	public static PingPage? PingPage { get; set; }
	public static IpConfigPage? IpConfigPage { get; set; }
	public static WiFiPasswordsPage? WiFiPasswordsPage { get; set; }
	public static DnsPage? DnsPage { get; set; }
	public static TraceroutePage? TraceroutePage { get; set; }

	internal static string SynethiaPath => $@"{FileSys.AppDataPath}\Léo Corporation\InternetTest Pro\SynethiaConfig.json";

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
	};

	public static List<AppPages> GetMostRelevantPages(SynethiaConfig synethiaConfig)
	{
		Dictionary<AppPages, double> appScores = new()
		{
			{ AppPages.Status, synethiaConfig.StatusPageInfo.Score },
			{ AppPages.DownDetector, synethiaConfig.DownDetectorPageInfo.Score },
			{ AppPages.MyIP, synethiaConfig.MyIPPageInfo.Score },
			{ AppPages.LocateIP, synethiaConfig.LocateIPPageInfo.Score },
			{ AppPages.Ping, synethiaConfig.PingPageInfo.Score },
			{ AppPages.IPConfig, synethiaConfig.IPConfigPageInfo.Score },
			{ AppPages.WiFiPasswords, synethiaConfig.WiFiPasswordsPageInfo.Score },
			{ AppPages.DnsTool, synethiaConfig.DnsPageInfo.Score },
			{ AppPages.TraceRoute, synethiaConfig.TraceRoutePageInfo.Score },
		};

		var sorted = appScores.OrderByDescending(x => x.Value);

		return (from item in sorted select item.Key).ToList();
	}

	public static List<ActionInfo> GetMostRelevantActions(SynethiaConfig synethiaConfig)
	{
		Dictionary<ActionInfo, int> relevantActions = new();
		for (int i = 0; i < synethiaConfig.ActionInfos.Count; i++)
		{
			relevantActions.Add(synethiaConfig.ActionInfos[i], synethiaConfig.ActionInfos[i].UsageCount);
		}

		// Sort each action with its usage count descending
		var sorted = relevantActions.OrderByDescending(x => x.Value);
		return (from item in sorted select item.Key).ToList();
	}

	public static List<AppPages> DefaultRelevantPages => new()
	{
		AppPages.Status,
		AppPages.LocateIP,
		AppPages.WiFiPasswords,
		AppPages.DownDetector,
		AppPages.MyIP,
		AppPages.Ping,
		AppPages.TraceRoute,
		AppPages.IPConfig,
		AppPages.DnsTool,
	};

	public static List<ActionInfo> DefaultRelevantActions => new()
	{
		new() { Action = AppActions.MyIP, UsageCount = 0 },
		new() { Action = AppActions.Test, UsageCount = 0 },
		new() { Action = AppActions.DownDetectorRequest, UsageCount = 0 },
		new() { Action = AppActions.Ping, UsageCount = 0 },
		new() { Action = AppActions.LocateIP, UsageCount = 0 },
		new() { Action = AppActions.GetIPConfig, UsageCount = 0 },
		new() { Action = AppActions.GetWiFiPasswords, UsageCount = 0 },
		new() { Action = AppActions.GetDnsInfo, UsageCount = 0 },
		new() { Action = AppActions.TraceRoute, UsageCount = 0 },
	};

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
	};

	public static Color GetColorFromResource(string resourceName) => (Color)ColorConverter.ConvertFromString(Application.Current.Resources[resourceName].ToString());

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
		ResourceDictionary resourceDictionary = new(); // Create a resource dictionary

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
		List<TracertStep> steps = new();

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
}