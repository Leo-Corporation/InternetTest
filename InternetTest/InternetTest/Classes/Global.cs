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
using InternetTest.Pages;
using LeoCorpLibrary;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace InternetTest.Classes
{
	/// <summary>
	/// The <see cref="Global"/> class contains various methods and properties.
	/// </summary>
	public static class Global
	{
		/// <summary>
		/// The <see cref="Pages.ConnectionPage"/>.
		/// </summary>
		public static ConnectionPage ConnectionPage { get; set; }

		/// <summary>
		/// The <see cref="Pages.LocalizeIPPage"/>.
		/// </summary>
		public static LocalizeIPPage LocalizeIPPage { get; set; }

		/// <summary>
		/// The <see cref="Pages.DownDetectorPage"/>.
		/// </summary>
		public static DownDetectorPage DownDetectorPage { get; set; }

		/// <summary>
		/// The <see cref="Pages.SettingsPage"/>.
		/// </summary>
		public static SettingsPage SettingsPage { get; set; }

		/// <summary>
		/// The <see cref="Classes.Settings"/> of InternetTest
		/// </summary>
		public static Settings Settings { get; set; }

		/// <summary>
		/// The current version of InternetTest.
		/// </summary>
		public static string Version => "5.8.0.2111";

		/// <summary>
		/// List of the available languages.
		/// </summary>
		public static List<string> LanguageList => new() { "English (United States)", "Français (France)" };

		/// <summary>
		/// List of the available languages codes.
		/// </summary>
		public static List<string> LanguageCodeList => new() { "en-US", "fr-FR" };

		/// <summary>
		/// GitHub link for the last version (<see cref="string"/>).
		/// </summary>
		public static string LastVersionLink { get => "https://raw.githubusercontent.com/Leo-Corporation/LeoCorp-Docs/master/Liens/Update%20System/InternetTest/5.0/Version.txt"; }

		/// <summary>
		/// Gets the "Hi" sentence message.
		/// </summary>
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

		/// <summary>
		/// Opens a link in a web browser.
		/// </summary>
		/// <param name="url">The URL to open.</param>
		public static void OpenLinkInBrowser(string url)
		{
			var ps = new ProcessStartInfo(url)
			{
				UseShellExecute = true,
				Verb = "open"
			};

			Process.Start(ps); // Open the URL
		}

		/// <summary>
		/// Gets IP informations.
		/// </summary>
		/// <param name="ip">The IP.</param>
		/// <returns>An <see cref="IPInfo"/> object.</returns>
		public async static Task<IPInfo> GetIPInfo(string ip)
		{
			try
			{
				// Get language
				string language = Settings.Language switch
				{
					"fr-FR" => "fr", // French
					"en-US" => "en", // English
					"_default" => System.Globalization.CultureInfo.CurrentUICulture.TwoLetterISOLanguageName, // System language
					_ => "en" // English
				};


				// Get IP infos
				string content = await new WebClient().DownloadStringTaskAsync($"http://ip-api.com/line/{ip}?lang={language}"); // Get the content
				string[] lines = content.Split(new string[] { "\n" }, StringSplitOptions.None); // Lines

				return new IPInfo
				{
					Status = lines[0],
					Country = lines[1],
					CountryCode = lines[2],
					Region = lines[3],
					RegionName = lines[4],
					City = lines[5],
					Zip = lines[6],
					Lat = lines[7],
					Lon = lines[8],
					TimeZone = lines[9],
					ISP = lines[10],
					Org = lines[12],
					Query = lines[13]
				};
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, Properties.Resources.InternetTest, MessageBoxButton.OK, MessageBoxImage.Error); // Error
				return new IPInfo();
			}
		}

		/// <summary>
		/// Checks if an IP is valid.
		/// </summary>
		/// <param name="ip">The IP to check.</param>
		/// <returns>A <see cref="bool"/> value.</returns>
		public static bool IsIPValid(string ip)
		{
			try
			{
				if (ip == "")
				{
					return true; // Gets your IP, so it's valid
				}

				if (ip.Contains("http://") || ip.Contains("https://"))
				{
					return false; // If it is a website
				}

				string[] splittedIP = ip.Split(new string[] { "." }, StringSplitOptions.None); // Split

				if (splittedIP.Length != 4)
				{
					return false; // IP Invalid
				}

				for (int i = 0; i < splittedIP.Length; i++)
				{
					if (string.IsNullOrEmpty(splittedIP[i]) && string.IsNullOrWhiteSpace(splittedIP[i])) // Check if there is a value
					{
						return false; // There is no value, then the IP is invalid
					}

					if (int.Parse(splittedIP[i]) > 255 || int.Parse(splittedIP[i]) < 0)
					{
						return false; // IP Adress is between 0 & 255
					}
				}

				return true; // No problems were found, the IP is valid
			}
			catch
			{
				return false; // The IP isnt't valid because the parser has thrown an exception
			}
		}

		/// <summary>
		/// Changes the application's theme.
		/// </summary>
		public static void ChangeTheme()
		{
			App.Current.Resources.MergedDictionaries.Clear();
			ResourceDictionary resourceDictionary = new(); // Create a resource dictionary

			if (!Settings.IsThemeSystem.HasValue)
			{
				Settings.IsThemeSystem = false;
			}

			if (Settings.IsThemeSystem.Value)
			{
				Settings.IsDarkTheme = IsSystemThemeDark(); // Set
			}

			if (Settings.IsDarkTheme) // If the dark theme is on
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
			if (Env.WindowsVersion != WindowsVersion.Windows10)
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

		/// <summary>
		/// Changes the application's language.
		/// </summary>
		public static void ChangeLanguage()
		{
			switch (Global.Settings.Language) // For each case
			{
				case "_default": // No language
					break;
				case "en-US": // English (US)
					Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US"); // Change
					break;

				case "fr-FR": // French (FR)
					Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("fr-FR"); // Change
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
	}
}
