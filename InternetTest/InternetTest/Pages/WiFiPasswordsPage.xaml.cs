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
using InternetTest.Classes;
using InternetTest.UserControls;
using LeoCorpLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Serialization;

namespace InternetTest.Pages;
/// <summary>
/// Interaction logic for WiFiPasswordsPage.xaml
/// </summary>
public partial class WiFiPasswordsPage : Page
{
    bool codeInjected = !Global.Settings.UseSynethia;
    public WiFiPasswordsPage()
    {
        InitializeComponent();
        InitUI();
        InjectSynethiaCode();
    }

    Placeholder Placeholder = new(Properties.Resources.NothingToShow, "\uF227");
    private void InitUI()
    {
        TitleTxt.Text = $"{Properties.Resources.Commands} > {Properties.Resources.WifiPasswords}";
        PlaceholderGrid.Children.Add(Placeholder); // Show the placeholder instead of an empty page
    }

    private void InjectSynethiaCode()
    {
        if (codeInjected) return;
        codeInjected = true;
        foreach (Button b in Global.FindVisualChildren<Button>(this))
        {
            b.Click += (sender, e) =>
            {
                Global.SynethiaConfig.WiFiPasswordsPageInfo.InteractionCount++;
            };
        }

        // For each TextBox of the page
        foreach (TextBox textBox in Global.FindVisualChildren<TextBox>(this))
        {
            textBox.GotFocus += (o, e) =>
            {
                Global.SynethiaConfig.WiFiPasswordsPageInfo.InteractionCount++;
            };
        }

        // For each CheckBox/RadioButton of the page
        foreach (CheckBox checkBox in Global.FindVisualChildren<CheckBox>(this))
        {
            checkBox.Checked += (o, e) =>
            {
                Global.SynethiaConfig.WiFiPasswordsPageInfo.InteractionCount++;
            };
            checkBox.Unchecked += (o, e) =>
            {
                Global.SynethiaConfig.WiFiPasswordsPageInfo.InteractionCount++;
            };
        }

        foreach (RadioButton radioButton in Global.FindVisualChildren<RadioButton>(this))
        {
            radioButton.Checked += (o, e) =>
            {
                Global.SynethiaConfig.WiFiPasswordsPageInfo.InteractionCount++;
            };
            radioButton.Unchecked += (o, e) =>
            {
                Global.SynethiaConfig.WiFiPasswordsPageInfo.InteractionCount++;
            };
        }
    }

    internal async void GetWiFiBtn_Click(object sender, RoutedEventArgs e)
    {
        await GetWiFiNetworksInfo(); // Update the UI
        if (WiFiItemDisplayer.Children.Count == 0)
        {
            PlaceholderGrid.Visibility = Visibility.Visible;
            Placeholder.Visibility = Visibility.Visible;
        }
        else
        {
            PlaceholderGrid.Visibility = Visibility.Collapsed;
            Placeholder.Visibility = Visibility.Collapsed;
        }

        // Increment the interaction count of the ActionInfo in Global.SynethiaConfig
        Global.SynethiaConfig.ActionInfos.First(a => a.Action == Enums.AppActions.GetWiFiPasswords).UsageCount++;
    }

    internal async Task GetWiFiNetworksInfo()
    {

        try
        {
            WiFiItemDisplayer.Children.Clear(); // Clear the panel
            PlaceholderGrid.Visibility = Visibility.Collapsed; // Hide the placeholder
            Placeholder.Visibility = Visibility.Collapsed; // Hide the placeholder

            // Check if the temp directory exists
            string path = Env.AppDataPath + @"\Léo Corporation\InternetTest Pro\Temp";

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            };

            // Run "netsh wlan export profile key=clear" command
            Process process = new();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = $"/c netsh wlan export profile key=clear folder=\"{path}\"";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.Start();
            await process.WaitForExitAsync();

            // Read the files
            string[] files = Directory.GetFiles(path);
            for (int i = 0; i < files.Length; i++)
            {
                XmlSerializer serializer = new(typeof(WLANProfile));
                StreamReader streamReader = new(files[i]); // Where the file is going to be read

                var test = (WLANProfile?)serializer.Deserialize(streamReader);

                if (test != null)
                {
                    WiFiItemDisplayer.Children.Add(new WiFiInfoItem(test));
                }
                streamReader.Close();

                File.Delete(files[i]); // Remove the temp file

            }
            Directory.Delete(path); // Delete the temp directory			
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, Properties.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        PlaceholderGrid.Visibility = Visibility.Collapsed;
        Placeholder.Visibility = Visibility.Collapsed;
        if (!(WiFiItemDisplayer.Children.Count > 0)) return;

        List<bool> vis = Enumerable.Empty<bool>().ToList();
        for (int i = 0; i < WiFiItemDisplayer.Children.Count; i++)
        {
            if (SearchTxt.Text == "")
            {
                ((WiFiInfoItem)WiFiItemDisplayer.Children[i]).Visibility = Visibility.Visible;
                continue;
            }

            if (WiFiItemDisplayer.Children[i] is WiFiInfoItem wiFiInfoItem)
            {
                bool b = wiFiInfoItem.ToString().ToLower().Contains(SearchTxt.Text.ToLower());
                wiFiInfoItem.Visibility = !b ? Visibility.Collapsed : Visibility.Visible;
                vis.Add(wiFiInfoItem.Visibility == Visibility.Visible);
            }
        }

        if (vis.Count == 0 || vis is null) return;
        if (vis.Contains(true)) return;
        PlaceholderGrid.Visibility = Visibility.Visible;
        Placeholder.Visibility = Visibility.Visible;
    }

    private void DismissBtn_Click(object sender, RoutedEventArgs e)
    {
        SearchTxt.Text = "";
        if (WiFiItemDisplayer.Children.Count > 0)
        {
            Placeholder.Visibility = Visibility.Collapsed;
            PlaceholderGrid.Visibility = Visibility.Collapsed;
        }
        else
        {
            Placeholder.Visibility = Visibility.Visible;
            PlaceholderGrid.Visibility = Visibility.Visible;
        }
    }
}
