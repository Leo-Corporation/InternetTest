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
using InternetTest.Helpers;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Xml.Serialization;

namespace InternetTest.Models;

public class Settings
{
	public Settings()
	{
		Theme = Theme.System;
		Language = Language.Default;
		MapProvider = MapProvider.OpenStreetMap;
		DefaultPage = AppPages.Home;
		ShowNotficationWhenUpdateAvailable = true;
		CheckUpdateOnStart = false;
		UseHttps = true;
		IsFirstRun = true;
		TestSite = "https://leocorporation.dev";
		UseSynethia = true;
		TestOnStart = true;
		IsMaximized = false;
		ToggleConfidentialMode = false;
		Pinned = false;
		RememberPinnedState = true;
		TraceRouteMaxHops = 30;
		TraceRouteMaxTimeOut = 5000;
		MainWindowSize = (950, 600);
		LaunchIpLocationOnStart = true;
		DownDetectorWebsites = [];
		DefaultTimeInterval = 10;
		HideDisabledAdapters = false;
		MapZoomLevel = 12;
		ShowAdaptersNoIpv4Support = false;
	}

	public Theme Theme { get; set; }
	public Language Language { get; set; }
	public MapProvider MapProvider { get; set; }
	public AppPages DefaultPage { get; set; }
	public bool ShowNotficationWhenUpdateAvailable { get; set; }
	public bool CheckUpdateOnStart { get; set; }
	public bool UseHttps { get; set; }
	public bool UseSynethia { get; set; }
	public bool IsFirstRun { get; set; }
	public bool TestOnStart { get; set; }
	public string? TestSite { get; set; }
	public bool? IsMaximized { get; set; }
	public bool? ToggleConfidentialMode { get; set; }
	public bool? Pinned { get; set; }
	public bool? RememberPinnedState { get; set; }
	public int? TraceRouteMaxHops { get; set; }
	public int? TraceRouteMaxTimeOut { get; set; }
	public (double, double)? MainWindowSize { get; set; }
	public bool? LaunchIpLocationOnStart { get; set; }
	public List<string>? DownDetectorWebsites { get; set; }
	public int? DefaultTimeInterval { get; set; }
	public bool? HideDisabledAdapters { get; set; }
	public int? MapZoomLevel { get; set; }
	public bool? ShowAdaptersNoIpv4Support { get; set; }

	public void Save(string? path = null)
	{
		XmlSerializer xmlSerializer = new(typeof(Settings));
		StreamWriter streamWriter = new(path ?? $@"{Context.DefaultStoragePath}\Settings.xml");
		xmlSerializer.Serialize(streamWriter, this);
		streamWriter.Dispose();
	}

	public void Load(string? path = null)
	{
		var filePath = path ?? $"{Context.DefaultStoragePath}\\Settings.xml";
		Directory.CreateDirectory(Context.DefaultStoragePath);

		// Create default file if it doesn't exist
		if (!File.Exists(filePath))
		{
			using var writer = new StreamWriter(filePath);
			new XmlSerializer(typeof(Settings)).Serialize(writer, new Settings());
		}

		Settings settings;
		using (var reader = new StreamReader(filePath))
		{
			settings = (Settings?)new XmlSerializer(typeof(Settings)).Deserialize(reader) ?? new Settings();
		}

		// Upgrade defaults in one go if they're null
		settings.IsMaximized ??= false;
		settings.ToggleConfidentialMode ??= false;
		settings.Pinned ??= false;
		settings.RememberPinnedState ??= true;
		settings.TraceRouteMaxHops ??= 30;
		settings.TraceRouteMaxTimeOut ??= 5000;
		settings.MainWindowSize ??= (950, 600);
		settings.LaunchIpLocationOnStart ??= true;
		settings.DefaultPage = (settings.DefaultPage is AppPages.Status or AppPages.MyIP) ? AppPages.Home : settings.DefaultPage;
		settings.DownDetectorWebsites ??= [];
		settings.DefaultTimeInterval ??= 10;
		settings.HideDisabledAdapters ??= false;
		settings.MapZoomLevel ??= 12;
		settings.ShowAdaptersNoIpv4Support ??= false;

		// Assign properties from deserialized settings conveniently
		Theme = settings.Theme;
		Language = settings.Language;
		MapProvider = settings.MapProvider;
		DefaultPage = settings.DefaultPage;
		ShowNotficationWhenUpdateAvailable = settings.ShowNotficationWhenUpdateAvailable;
		CheckUpdateOnStart = settings.CheckUpdateOnStart;
		UseHttps = settings.UseHttps;
		UseSynethia = settings.UseSynethia;
		IsFirstRun = settings.IsFirstRun;
		TestOnStart = settings.TestOnStart;
		TestSite = settings.TestSite;
		IsMaximized = settings.IsMaximized;
		ToggleConfidentialMode = settings.ToggleConfidentialMode;
		Pinned = settings.Pinned;
		RememberPinnedState = settings.RememberPinnedState;
		TraceRouteMaxHops = settings.TraceRouteMaxHops;
		TraceRouteMaxTimeOut = settings.TraceRouteMaxTimeOut;
		MainWindowSize = settings.MainWindowSize;
		LaunchIpLocationOnStart = settings.LaunchIpLocationOnStart;
		DownDetectorWebsites = settings.DownDetectorWebsites;
		DefaultTimeInterval = settings.DefaultTimeInterval;
		HideDisabledAdapters = settings.HideDisabledAdapters;
		MapZoomLevel = settings.MapZoomLevel;
		ShowAdaptersNoIpv4Support = settings.ShowAdaptersNoIpv4Support;
	}

	public void Export(string path)
	{
		try
		{
			Save(path); // Save settings to the specified path

			MessageBox.Show(Properties.Resources.SettingsExportedSucessMsg, Properties.Resources.InternetTest, MessageBoxButton.OK, MessageBoxImage.Information); // Show message
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, Properties.Resources.InternetTest, MessageBoxButton.OK, MessageBoxImage.Error); // Show error message
		}
	}

	public void Import(string path)
	{
		try
		{
			Load(path);
			Save(); // Save
			MessageBox.Show(Properties.Resources.SettingsImportedMsg, Properties.Resources.InternetTest, MessageBoxButton.OK, MessageBoxImage.Information); // Show error message

			// Restart app
			Process.Start(Directory.GetCurrentDirectory() + @"\InternetTest.exe"); // Start app
			Environment.Exit(0); // Quit
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, Properties.Resources.InternetTest, MessageBoxButton.OK, MessageBoxImage.Error); // Show error message
		}
	}
}