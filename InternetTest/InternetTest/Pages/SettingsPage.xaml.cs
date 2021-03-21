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
using LeoCorpLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InternetTest.Pages
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {
        bool isAvailable;
        public SettingsPage()
        {
            InitializeComponent();
            InitUI();
        }

        private async void InitUI()
        {
            try
            {
                // Load RadioButtons
                DarkRadioBtn.IsChecked = Global.Settings.IsDarkTheme; // Change IsChecked property
                LightRadioBtn.IsChecked = !Global.Settings.IsDarkTheme; // Change IsChecked property

                // Load LangComboBox
                LangComboBox.Items.Add(Properties.Resources.Default); // Add "default"

                for (int i = 0; i < Global.LanguageList.Count; i++)
                {
                    LangComboBox.Items.Add(Global.LanguageList[i]);
                }

                LangComboBox.SelectedIndex = (Global.Settings.Language == "_default") ? 0 : Global.LanguageCodeList.IndexOf(Global.Settings.Language) + 1;

                LangApplyBtn.Visibility = Visibility.Hidden; // Hide
                ThemeApplyBtn.Visibility = Visibility.Hidden; // Hide

                // Update the UpdateStatusTxt
                isAvailable = Update.IsAvailable(Global.Version, await Update.GetLastVersionAsync(Global.LastVersionLink));

                UpdateStatusTxt.Text = isAvailable ? Properties.Resources.AvailableUpdates : Properties.Resources.UpToDate; // Set the text
                InstallIconTxt.Text = isAvailable ? "\uF152" : "\uF191"; // Set text 
                InstallMsgTxt.Text = isAvailable ? Properties.Resources.Install : Properties.Resources.CheckUpdate; // Set text
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.StackTrace, MessageBoxButton.OK, MessageBoxImage.Error); // Show error
            }
        }

        private async void RefreshInstallBtn_Click(object sender, RoutedEventArgs e)
        {
            if (isAvailable) // If there is updates
            {
                string lastVersion = await Update.GetLastVersionAsync(Global.LastVersionLink); // Get last version
                if (MessageBox.Show(Properties.Resources.InstallConfirmMsg, $"{Properties.Resources.InstallVersion} {lastVersion}", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                {
                    Env.ExecuteAsAdmin(Directory.GetCurrentDirectory() + @"\Xalyus Updater.exe"); // Start the updater
                    Environment.Exit(0); // Close
                }
            }
            else
            {
                // Update the UpdateStatusTxt
                isAvailable = Update.IsAvailable(Global.Version, await Update.GetLastVersionAsync(Global.LastVersionLink));

                UpdateStatusTxt.Text = isAvailable ? Properties.Resources.AvailableUpdates : Properties.Resources.UpToDate; // Set the text
                InstallIconTxt.Text = isAvailable ? "\uF152" : "\uF191"; // Set text 
                InstallMsgTxt.Text = isAvailable ? Properties.Resources.Install : Properties.Resources.CheckUpdate; // Set text
            }
        }

        private void ThemeApplyBtn_Click(object sender, RoutedEventArgs e)
        {
            Global.Settings.IsDarkTheme = DarkRadioBtn.IsChecked.Value; // Set the settings
            SettingsManager.Save(); // Save the changes
            ThemeApplyBtn.Visibility = Visibility.Hidden; // Hide
            DisplayRestartMessage();
        }

        private void LangComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LangApplyBtn.Visibility = Visibility.Visible; // Show the LangApplyBtn button
        }

        private void LangApplyBtn_Click(object sender, RoutedEventArgs e)
        {
            Global.Settings.Language = LangComboBox.Text switch
            {
                "English (United States)" => Global.LanguageCodeList[0], // Set the settings value
                "Français (France)"       => Global.LanguageCodeList[1], // Set the settings value
                _                         => "_default" // Set the settings value
            };
            SettingsManager.Save(); // Save the changes
            LangApplyBtn.Visibility = Visibility.Hidden; // Hide
            DisplayRestartMessage();
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //TODO: Add licenses
        }

        private void LightRadioBtn_Checked(object sender, RoutedEventArgs e)
        {
            ThemeApplyBtn.Visibility = Visibility.Visible; // Show the ThemeApplyBtn button
        }

        private void DarkRadioBtn_Checked(object sender, RoutedEventArgs e)
        {
            ThemeApplyBtn.Visibility = Visibility.Visible; // Show the ThemeApplyBtn button
        }

        /// <summary>
        /// Restarts InternetTest.
        /// </summary>
        private void DisplayRestartMessage()
        {
            if (MessageBox.Show(Properties.Resources.NeedRestartToApplyChanges, Properties.Resources.InternetTest, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Process.Start(Directory.GetCurrentDirectory() + @"\InternetTest.exe"); // Start
                Environment.Exit(0); // Close
            }
        }
    }
}
