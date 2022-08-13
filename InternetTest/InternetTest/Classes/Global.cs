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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace InternetTest.Classes;
public static class Global
{
	public static string Version => "7.0.0.2208-pre1";

	public static HomePage HomePage { get; set; } = new();

	public static SynethiaConfig SynethiaConfig { get; set; }

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
		AppPages.IPConfig		
	};

	public static List<ActionInfo> DefaultRelevantActions => new()
	{
		new(AppActions.MyIP, 12),
		new(AppActions.Test, 10),
		new(AppActions.DownDetectorRequest, 7),
		new(AppActions.LocateIP, 6),
		new(AppActions.GetWiFiPasswords, 5),
		new(AppActions.GetIPConfig, 2),
		new(AppActions.Ping, 1),
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
	};

	public static Dictionary<AppActions, string> ActionsString => new()
	{
		{ AppActions.DownDetectorRequest, "TEXT_HERE" },
		{ AppActions.GetIPConfig, "TEXT_HERE" },
		{ AppActions.GetWiFiPasswords, "TEXT_HERE" },
		{ AppActions.LocateIP, "TEXT_HERE" },
		{ AppActions.MyIP, "TEXT_HERE" },
		{ AppActions.Ping, "TEXT_HERE" },
		{ AppActions.Test, "TEXT_HERE" },
	};

	public static Color GetColorFromResource(string resourceName)
	{
		return (Color)ColorConverter.ConvertFromString(Application.Current.Resources[resourceName].ToString());
	}
}
