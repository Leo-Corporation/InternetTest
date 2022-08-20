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
using LeoCorpLibrary;
using InternetTest.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
	public string? TestSite { get; set; }
}

public static class SettingsManager
{
	private static string SettingsPath => $@"{Env.AppDataPath}\Léo Corporation\InternetTest Pro\Settings.xml";
	public static Settings Load()
	{
		if (!Directory.Exists($@"{Env.AppDataPath}\Léo Corporation\InternetTest Pro\"))
		{
			Directory.CreateDirectory($@"{Env.AppDataPath}\Léo Corporation\InternetTest Pro\");
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
		return (Settings)xmlDeserializer.Deserialize(streamReader);
	}

	public static void Save()
	{
		// Serialize to XML
		XmlSerializer xmlSerializer = new(typeof(Settings));
		StreamWriter streamWriter = new(SettingsPath);
		xmlSerializer.Serialize(streamWriter, Global.Settings);
		streamWriter.Dispose();
	}
}
