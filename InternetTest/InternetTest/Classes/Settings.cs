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
using PeyrSharp.Env;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Documents;
using System.Xml.Serialization;

namespace InternetTest.Classes;
public class Settings
{
	public Settings()
	{
		Theme = Themes.System;
		Language = Languages.Default;
		MapProvider = MapProvider.OpenStreetMap;
		DefaultPage = AppPages.Home;
		ShowNotficationWhenUpdateAvailable = true;
		CheckUpdateOnStart = true;
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
	}

	public Themes Theme { get; set; }
	public Languages Language { get; set; }
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
}

public static class SettingsManager
{
	private static string SettingsPath => $@"{FileSys.AppDataPath}\Léo Corporation\InternetTest Pro\Settings.xml";
	public static Settings Load()
	{
		if (!Directory.Exists($@"{FileSys.AppDataPath}\Léo Corporation\InternetTest Pro\"))
		{
			Directory.CreateDirectory($@"{FileSys.AppDataPath}\Léo Corporation\InternetTest Pro\");
		}

		if (!File.Exists(SettingsPath))
		{
			Global.Settings = new();

			// Serialize to XML
			XmlSerializer xmlSerializer = new(typeof(Settings));
			StreamWriter streamWriter = new(SettingsPath);
			xmlSerializer.Serialize(streamWriter, Global.Settings);
			streamWriter.Dispose();
			return new();
		}

		// If there's already a setting file
		// Deserialize from xml
		XmlSerializer xmlDeserializer = new(typeof(Settings));

		StreamReader streamReader = new(SettingsPath);
		var settings = (Settings?)xmlDeserializer.Deserialize(streamReader) ?? new();

		// Upgrade the settings file if it comes from an older version
		settings.IsMaximized ??= false; // Set the default value if none is specified.
		settings.ToggleConfidentialMode ??= false; // Set the default value if none is specified.
		settings.Pinned ??= false; // Set the default value if none is specified.
		settings.RememberPinnedState ??= true; // Set the default value if none is specified.
		settings.TraceRouteMaxHops ??= 30;
		settings.TraceRouteMaxTimeOut ??= 5000;
		settings.MainWindowSize ??= (950, 600);
		settings.LaunchIpLocationOnStart ??= true;
		settings.DefaultPage = (settings.DefaultPage == AppPages.Status || settings.DefaultPage == AppPages.MyIP) ? AppPages.Home : settings.DefaultPage;
		settings.DownDetectorWebsites ??= new();
		settings.DefaultTimeInterval ??= 10;

		return settings;
	}

	public static void Save()
	{
		// Serialize to XML
		XmlSerializer xmlSerializer = new(typeof(Settings));
		StreamWriter streamWriter = new(SettingsPath);
		xmlSerializer.Serialize(streamWriter, Global.Settings);
		streamWriter.Dispose();
	}

	/// <summary>
	/// Exports current settings.
	/// </summary>
	/// <param name="path">The path where the settings file should be exported.</param>
	public static void Export(string path)
	{
		try
		{
			XmlSerializer xmlSerializer = new(typeof(Settings)); // Create XML Serializer

			StreamWriter streamWriter = new(path); // The place where the file is going to be written
			xmlSerializer.Serialize(streamWriter, Global.Settings);

			streamWriter.Dispose();

			MessageBox.Show(Properties.Resources.SettingsExportedSucessMsg, Properties.Resources.InternetTest, MessageBoxButton.OK, MessageBoxImage.Information); // Show message
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, Properties.Resources.InternetTest, MessageBoxButton.OK, MessageBoxImage.Error); // Show error message
		}
	}

	/// <summary>
	/// Imports settings.
	/// </summary>
	/// <param name="path">The path to the settings file.</param>
	public static void Import(string path)
	{
		try
		{
			if (File.Exists(path)) // If the file exist
			{
				XmlSerializer xmlSerializer = new(typeof(Settings)); // XML Serializer
				StreamReader streamReader = new(path); // Where the file is going to be read

				var settings = (Settings?)xmlSerializer.Deserialize(streamReader) ?? new();

				// Upgrade the settings file if it comes from an older version
				settings.IsMaximized ??= false; // Set the default value if none is specified.
				settings.ToggleConfidentialMode ??= false; // Set the default value if none is specified.
				settings.Pinned ??= false; // Set the default value if none is specified.
				settings.RememberPinnedState ??= true; // Set the default value if none is specified.
				settings.TraceRouteMaxHops ??= 30;
				settings.TraceRouteMaxTimeOut ??= 5000;
				settings.MainWindowSize ??= (950, 600);
				settings.LaunchIpLocationOnStart ??= true;
				settings.DefaultPage = (settings.DefaultPage == AppPages.Status || settings.DefaultPage == AppPages.MyIP) ? AppPages.Home : settings.DefaultPage;
				settings.DownDetectorWebsites ??= new();
				settings.DefaultTimeInterval ??= 10;

				Global.Settings = settings;

				streamReader.Dispose();
				Save(); // Save
				MessageBox.Show(Properties.Resources.SettingsImportedMsg, Properties.Resources.InternetTest, MessageBoxButton.OK, MessageBoxImage.Information); // Show error message

				// Restart app
				Process.Start(Directory.GetCurrentDirectory() + @"\InternetTest.exe"); // Start app
				Environment.Exit(0); // Quit
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, Properties.Resources.InternetTest, MessageBoxButton.OK, MessageBoxImage.Error); // Show error message
		}
	}
}
