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
using LeoCorpLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace InternetTest.Classes
{
    /// <summary>
    /// Settings of InternetTest
    /// </summary>
    public class Settings
    {
        /// <summary>
        /// True if the theme of InternetTest is set to dark.
        /// </summary>
        public bool IsDarkTheme { get; set; }

        /// <summary>
        /// The language of the app (country code). Can be _default, en-US, fr-FR...
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// The site where the connection is going to be tested.
        /// </summary>
        public string TestSite { get; set; }

        /// <summary>
        /// The map provider for localizing an IP.
        /// </summary>
        public MapProviders MapProvider { get; set; }
    }

    /// <summary>
    /// Class that contains methods that can manage InternetTest' settings.
    /// </summary>
    public static class SettingsManager
    {
        /// <summary>
        /// Loads InternetTest settings.
        /// </summary>
        public static void Load()
        {
            string path = Env.AppDataPath + @"\Léo Corporation\InternetTest\Settings.xml"; // The path of the settings file

            if (File.Exists(path)) // If the file exist
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(Settings)); // XML Serializer
                StreamReader streamReader = new StreamReader(path); // Where the file is going to be read

                Global.Settings = (Settings)xmlSerializer.Deserialize(streamReader); // Read

                streamReader.Dispose();
            }
            else
            {
                Global.Settings = new Settings 
                {
                    IsDarkTheme = false, 
                    Language = "_default", 
                    TestSite = "https://bing.com", 
                    MapProvider = MapProviders.OpenStreetMap 
                }; // Create a new settings file

                Save(); // Save the changes
            }
        }

        /// <summary>
        /// Saves InternetTest settings.
        /// </summary>
        public static void Save()
        {
            string path = Env.AppDataPath + @"\Léo Corporation\InternetTest\Settings.xml"; // The path of the settings file

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Settings)); // Create XML Serializer

            if (!Directory.Exists(Env.AppDataPath + @"\Léo Corporation\InternetTest")) // If the directory doesn't exist
            {
                Directory.CreateDirectory(Env.AppDataPath + @"\Léo Corporation\"); // Create the directory
                Directory.CreateDirectory(Env.AppDataPath + @"\Léo Corporation\InternetTest"); // Create the directory
            }

            StreamWriter streamWriter = new StreamWriter(path); // The place where the file is going to be written
            xmlSerializer.Serialize(streamWriter, Global.Settings);

            streamWriter.Dispose();
        }
    }
}
