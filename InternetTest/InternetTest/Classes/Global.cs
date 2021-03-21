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
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
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
        public static string Version => "1.0.0.2103";

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
        public static string LastVersionLink { get => "https://raw.githubusercontent.com/Leo-Corporation/LeoCorp-Docs/master/Liens/Update%20System/Gerayis/Version.txt"; }

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
            Process.Start("explorer.exe", url); // Open the URL
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
                string content = await new WebClient().DownloadStringTaskAsync($"http://ip-api.com/line/{ip}"); // Get the content
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
        /// Changes the application's theme.
        /// </summary>
        public static void ChangeTheme()
        {
            App.Current.Resources.MergedDictionaries.Clear();
            ResourceDictionary resourceDictionary = new(); // Create a resource dictionary

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
    }
}
