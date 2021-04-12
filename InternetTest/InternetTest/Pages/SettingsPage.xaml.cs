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
using InternetTest.Enums;
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
		System.Windows.Forms.NotifyIcon notifyIcon = new System.Windows.Forms.NotifyIcon();
		public SettingsPage()
		{
			InitializeComponent();
			notifyIcon.Icon = System.Drawing.Icon.ExtractAssociatedIcon(AppDomain.CurrentDomain.BaseDirectory + @"\InternetTest.exe");
			notifyIcon.BalloonTipClicked += async (o, e) =>
			{
				string lastVersion = await Update.GetLastVersionAsync(Global.LastVersionLink); // Get last version
				if (MessageBox.Show(Properties.Resources.InstallConfirmMsg, $"{Properties.Resources.InstallVersion} {lastVersion}", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
				{
					Env.ExecuteAsAdmin(Directory.GetCurrentDirectory() + @"\Xalyus Updater.exe"); // Start the updater
					Environment.Exit(0); // Close
				}
			};
			InitUI();
		}

		private async void InitUI()
		{
			try
			{
				// Load RadioButtons
				DarkRadioBtn.IsChecked = Global.Settings.IsDarkTheme; // Change IsChecked property
				LightRadioBtn.IsChecked = !Global.Settings.IsDarkTheme; // Change IsChecked property

				// Load CheckBoxes
				CheckUpdatesOnStartChk.IsChecked = Global.Settings.CheckUpdatesOnStart.HasValue ? Global.Settings.CheckUpdatesOnStart.Value : true; // Set
				NotifyUpdatesChk.IsChecked = Global.Settings.NotifyUpdates.HasValue ? Global.Settings.NotifyUpdates.Value : true; // Set

				// Load LangComboBox
				LangComboBox.Items.Add(Properties.Resources.Default); // Add "default"

				for (int i = 0; i < Global.LanguageList.Count; i++)
				{
					LangComboBox.Items.Add(Global.LanguageList[i]);
				}

				LangComboBox.SelectedIndex = (Global.Settings.Language == "_default") ? 0 : Global.LanguageCodeList.IndexOf(Global.Settings.Language) + 1;

				// Load MapProviderComboBox
				MapProviderComboBox.Items.Add("OpenStreetMap"); // Add a map provider
				MapProviderComboBox.Items.Add("Bing Maps"); // Add a map provider
				MapProviderComboBox.Items.Add("Google Maps"); // Add a map provider

				MapProviderComboBox.SelectedIndex = Global.Settings.MapProvider switch
				{
					MapProviders.OpenStreetMap => 0,
					MapProviders.BingMaps => 1,
					MapProviders.GoogleMaps => 2,
					_ => 0,
				};

				// Load the TestSiteTxt
				TestSiteTxt.Text = Global.Settings.TestSite; // Set text

				// Update the UpdateStatusTxt
				if (Global.Settings.CheckUpdatesOnStart.Value)
				{
					if (await NetworkConnection.IsAvailableAsync())
					{
						isAvailable = Update.IsAvailable(Global.Version, await Update.GetLastVersionAsync(Global.LastVersionLink));

						UpdateStatusTxt.Text = isAvailable ? Properties.Resources.AvailableUpdates : Properties.Resources.UpToDate; // Set the text
						InstallIconTxt.Text = isAvailable ? "\uF152" : "\uF191"; // Set text 
						InstallMsgTxt.Text = isAvailable ? Properties.Resources.Install : Properties.Resources.CheckUpdate; // Set text

						if (isAvailable)
						{
							notifyIcon.Visible = true; // Show
							notifyIcon.ShowBalloonTip(5000, Properties.Resources.InternetTest, Properties.Resources.AvailableUpdates, System.Windows.Forms.ToolTipIcon.Info);
							notifyIcon.Visible = false; // Hide
						}
					}
					else
					{
						UpdateStatusTxt.Text = Properties.Resources.UnableToCheckUpdates; // Set text
						InstallMsgTxt.Text = Properties.Resources.CheckUpdate; // Set text
						InstallIconTxt.Text = "\uF191"; // Set text 
					}
				}
				else
				{
					UpdateStatusTxt.Text = Properties.Resources.UnableToCheckUpdates; // Set text
					InstallMsgTxt.Text = Properties.Resources.CheckUpdate; // Set text
					InstallIconTxt.Text = "\uF191"; // Set text 
				}
				LangApplyBtn.Visibility = Visibility.Hidden; // Hide
				ThemeApplyBtn.Visibility = Visibility.Hidden; // Hide
				TestSiteApplyBtn.Visibility = Visibility.Hidden; // Hide
				MapProviderApplyBtn.Visibility = Visibility.Hidden; // Hide 
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
				if (await NetworkConnection.IsAvailableAsync())
				{
					// Update the UpdateStatusTxt
					isAvailable = Update.IsAvailable(Global.Version, await Update.GetLastVersionAsync(Global.LastVersionLink));

					UpdateStatusTxt.Text = isAvailable ? Properties.Resources.AvailableUpdates : Properties.Resources.UpToDate; // Set the text
					InstallIconTxt.Text = isAvailable ? "\uF152" : "\uF191"; // Set text 
					InstallMsgTxt.Text = isAvailable ? Properties.Resources.Install : Properties.Resources.CheckUpdate; // Set text 

					if (isAvailable)
					{
						notifyIcon.Visible = true; // Show
						notifyIcon.ShowBalloonTip(5000, Properties.Resources.InternetTest, Properties.Resources.AvailableUpdates, System.Windows.Forms.ToolTipIcon.Info);
						notifyIcon.Visible = false; // Hide
					}
				}
				else
				{
					UpdateStatusTxt.Text = Properties.Resources.UnableToCheckUpdates; // Set text
					InstallMsgTxt.Text = Properties.Resources.CheckUpdate; // Set text
					InstallIconTxt.Text = "\uF191"; // Set text 
				}
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
				"Français (France)" => Global.LanguageCodeList[1], // Set the settings value
				_ => "_default" // Set the settings value
			};
			SettingsManager.Save(); // Save the changes
			LangApplyBtn.Visibility = Visibility.Hidden; // Hide
			DisplayRestartMessage();
		}

		private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			MessageBox.Show($"{Properties.Resources.Licenses}\n\n" +
				"Fluent System Icons - MIT License - © 2020 Microsoft Corporation\n" +
				"LeoCorpLibrary - MIT License - © 2020-2021 Léo Corporation\n" +
				"InternetTest - MIT License - © 2021 Léo Corporation", $"{Properties.Resources.InternetTest} - {Properties.Resources.Licenses}", MessageBoxButton.OK, MessageBoxImage.Information);
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

		private string FormatURL(string url)
		{
			if (!url.Contains("https://") && !url.Contains("http://")) // If there isn't http(s)
			{
				return "https://" + url; // Add the https://
			}
			else
			{
				return url; // Do nothing
			}
		}

		private void TestSiteApplyBtn_Click(object sender, RoutedEventArgs e)
		{
			TestSiteTxt.Text = FormatURL(TestSiteTxt.Text); // Format url
			Global.Settings.TestSite = TestSiteTxt.Text; // Define
			SettingsManager.Save(); // Save the changes
			TestSiteApplyBtn.Visibility = Visibility.Hidden; // Hide
		}

		private void MapProviderComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			MapProviderApplyBtn.Visibility = Visibility.Visible; // Visible
		}

		private void TestSiteTxt_TextChanged(object sender, TextChangedEventArgs e)
		{
			TestSiteApplyBtn.Visibility = Visibility.Visible; // Visible
		}

		private void MapProviderApplyBtn_Click(object sender, RoutedEventArgs e)
		{
			Global.Settings.MapProvider = MapProviderComboBox.Text switch
			{
				"OpenStreetMap" => MapProviders.OpenStreetMap,
				"Bing Maps" => MapProviders.BingMaps,
				"Google Maps" => MapProviders.GoogleMaps,
				_ => MapProviders.OpenStreetMap
			};
			SettingsManager.Save(); // Save the changes
			MapProviderApplyBtn.Visibility = Visibility.Hidden; // Hide
		}

		private void CheckUpdatesOnStartChk_Checked(object sender, RoutedEventArgs e)
		{
			Global.Settings.CheckUpdatesOnStart = CheckUpdatesOnStartChk.IsChecked; // Set
			SettingsManager.Save(); // Save changes
		}

		private void NotifyUpdatesChk_Checked(object sender, RoutedEventArgs e)
		{
			Global.Settings.NotifyUpdates = NotifyUpdatesChk.IsChecked; // Set
			SettingsManager.Save(); // Save changes
		}

		private void ResetSettingsLink_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			if (MessageBox.Show(Properties.Resources.ResetSettingsConfirmMsg, Properties.Resources.Settings, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
			{
				Global.Settings = new()
				{
					CheckUpdatesOnStart = true,
					IsDarkTheme = false,
					Language = "_default",
					NotifyUpdates = true,
					TestSite = "https://bing.com",
					MapProvider = MapProviders.OpenStreetMap
				}; // Create default settings

				SettingsManager.Save(); // Save the changes
				InitUI(); // Reload the page

				MessageBox.Show(Properties.Resources.SettingsReset, Properties.Resources.InternetTest, MessageBoxButton.OK, MessageBoxImage.Information);
				Process.Start(Directory.GetCurrentDirectory() + @"\InternetTest.exe");
				Environment.Exit(0); // Quit
			}
		}
	}
}
