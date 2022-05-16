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
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace InternetTest.Pages;

/// <summary>
/// Interaction logic for SettingsPage.xaml
/// </summary>
public partial class SettingsPage : Page
{
	bool isAvailable;
	readonly System.Windows.Forms.NotifyIcon notifyIcon = new();
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
			SystemRadioBtn.IsChecked = Global.Settings.IsThemeSystem; // Change IsChecked property
			ConnectionPageRadioBtn.IsChecked = Global.Settings.StartupPage == StartPages.Connection; // Set
			LocateIPPageRadioBtn.IsChecked = Global.Settings.StartupPage == StartPages.LocalizeIP; // Set
			DownDetectorPageRadioBtn.IsChecked = Global.Settings.StartupPage == StartPages.DownDetector; // Set

			// Borders
			if (DarkRadioBtn.IsChecked.Value)
			{
				CheckedBorder = DarkBorder; // Set
			}
			else if (LightRadioBtn.IsChecked.Value)
			{
				CheckedBorder = LightBorder; // Set
			}
			else if (SystemRadioBtn.IsChecked.Value)
			{
				CheckedBorder = SystemBorder; // Set
			}
			RefreshBorders();

			if (!Global.Settings.TestNotification.HasValue)
			{
				Global.Settings.TestNotification = true; // Set default value
				SettingsManager.Save(); // Save changes
			}

			if (!Global.Settings.UseHTTPS.HasValue)
			{
				Global.Settings.UseHTTPS = true; // Set default value
				SettingsManager.Save(); // Save changes
			}

			if (!Global.Settings.DownDetectorNotification.HasValue)
			{
				Global.Settings.DownDetectorNotification = true; // Set default value
				SettingsManager.Save(); // Save chnages
			}

			if (!Global.Settings.IsFirstRun.HasValue)
			{
				Global.Settings.IsFirstRun = true; // Set default value
				SettingsManager.Save();
			}

			HTTPSRadioBtn.IsChecked = Global.Settings.UseHTTPS.Value; // Set
			HTTPRadioBtn.IsChecked = !Global.Settings.UseHTTPS.Value; // Set

			// Load CheckBoxes
			CheckUpdatesOnStartChk.IsChecked = Global.Settings.CheckUpdatesOnStart ?? true; // Set
			NotifyUpdatesChk.IsChecked = Global.Settings.NotifyUpdates ?? true; // Set
			LaunchTestOnStartChk.IsChecked = Global.Settings.LaunchTestOnStart ?? true; // Set
			NotifyTestChk.IsChecked = Global.Settings.TestNotification; // Set
			NotifyDownDetectorChk.IsChecked = Global.Settings.DownDetectorNotification; // Set

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
			MapProviderComboBox.Items.Add("Yandex Maps"); // Add a map provider
			MapProviderComboBox.Items.Add("HERE WeGo"); // Add a map provider

			MapProviderComboBox.SelectedIndex = Global.Settings.MapProvider switch
			{
				MapProviders.OpenStreetMap => 0,
				MapProviders.BingMaps => 1,
				MapProviders.GoogleMaps => 2,
				MapProviders.Yandex => 3,
				MapProviders.HereWeGo => 4,
				_ => 0,
			};

			// Load StartupPageComboBox
			if (Global.Settings.StartupPage is null)
			{
				Global.Settings.StartupPage = StartPages.Connection; // Set default startup page
				SettingsManager.Save(); // Save the changes
			}

			// Borders
			if (ConnectionPageRadioBtn.IsChecked.Value)
			{
				PageCheckedBorder = ConnectionPageBorder; // Set
			}
			else if (LocateIPPageRadioBtn.IsChecked.Value)
			{
				PageCheckedBorder = LocateIPPageBorder; // Set
			}
			else if (DownDetectorPageRadioBtn.IsChecked.Value)
			{
				PageCheckedBorder = DownDetectorPageBorder; // Set
			}
			RefreshStartupBorders();

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

			VersionTxt.Text = Global.Version; // Set text
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
		Global.Settings.IsThemeSystem = SystemRadioBtn.IsChecked; // Set the settings
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
			"中文（简体）" => Global.LanguageCodeList[2], // Set the settings value
			_ => "_default" // Set the settings value
		};
		SettingsManager.Save(); // Save the changes
		LangApplyBtn.Visibility = Visibility.Hidden; // Hide
		DisplayRestartMessage();
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
	private static void DisplayRestartMessage()
	{
		if (MessageBox.Show(Properties.Resources.NeedRestartToApplyChanges, Properties.Resources.InternetTest, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
		{
			Process.Start(Directory.GetCurrentDirectory() + @"\InternetTest.exe"); // Start
			Environment.Exit(0); // Close
		}
	}

	private static string FormatURL(string url)
	{
		if (!url.Contains("https://") && !url.Contains("http://")) // If there isn't http(s)
		{
			return (Global.Settings.UseHTTPS.Value ? "https://" : "http://") + url; // Add the https://
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
			"Yandex Maps" => MapProviders.Yandex,
			"HERE WeGo" => MapProviders.HereWeGo,
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

	private void LaunchTestOnStartChk_Checked(object sender, RoutedEventArgs e)
	{
		Global.Settings.LaunchTestOnStart = LaunchTestOnStartChk.IsChecked; // Set
		SettingsManager.Save(); // Save changes
	}

	private void HTTPSRadioBtn_Checked(object sender, RoutedEventArgs e)
	{
		Global.Settings.UseHTTPS = HTTPSRadioBtn.IsChecked; // Set value
		SettingsManager.Save(); // Save changes
	}

	private void HTTPRadioBtn_Checked(object sender, RoutedEventArgs e)
	{
		Global.Settings.UseHTTPS = HTTPSRadioBtn.IsChecked; // Set value
		SettingsManager.Save(); // Save changes
	}

	private void ImportBtn_Click(object sender, RoutedEventArgs e)
	{
		OpenFileDialog openFileDialog = new()
		{
			Filter = "XML|*.xml",
			Title = Properties.Resources.Import
		}; // Create file dialog

		if (openFileDialog.ShowDialog() ?? true)
		{
			SettingsManager.Import(openFileDialog.FileName); // Import games
		}
	}

	private void ExportBtn_Click(object sender, RoutedEventArgs e)
	{
		SaveFileDialog saveFileDialog = new()
		{
			FileName = "InternetTestSettings.xml",
			Filter = "XML|*.xml",
			Title = Properties.Resources.Export
		}; // Create file dialog

		if (saveFileDialog.ShowDialog() ?? true)
		{
			SettingsManager.Export(saveFileDialog.FileName); // Export games
		}
	}

	private void BtnEnter(object sender, MouseEventArgs e)
	{
		Button button = (Button)sender; // Create button
		button.Foreground = new SolidColorBrush { Color = (Color)ColorConverter.ConvertFromString(App.Current.Resources["WindowButtonsHoverForeground1"].ToString()) }; // Set the foreground
	}

	private void BtnLeave(object sender, MouseEventArgs e)
	{
		Button button = (Button)sender; // Create button
		button.Foreground = new SolidColorBrush { Color = (Color)ColorConverter.ConvertFromString(App.Current.Resources["Foreground1"].ToString()) }; // Set the foreground 
	}

	private void SystemRadioBtn_Checked(object sender, RoutedEventArgs e)
	{
		ThemeApplyBtn.Visibility = Visibility.Visible; // Show
	}

	Border CheckedBorder { get; set; }
	private void LightBorder_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
	{
		LightRadioBtn.IsChecked = true; // Set IsChecked
		CheckedBorder = LightBorder; // Set
		RefreshBorders();
	}

	private void Border_MouseEnter(object sender, MouseEventArgs e)
	{
		Border border = (Border)sender;
		border.BorderBrush = new SolidColorBrush() { Color = (Color)ColorConverter.ConvertFromString(App.Current.Resources["AccentColor"].ToString()) }; // Set color

	}

	private void Border_MouseLeave(object sender, MouseEventArgs e)
	{
		Border border = (Border)sender;
		if (border != CheckedBorder && border != PageCheckedBorder)
		{
			border.BorderBrush = new SolidColorBrush() { Color = Colors.Transparent }; // Set color 
		}
	}

	private void DarkBorder_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
	{
		DarkRadioBtn.IsChecked = true; // Set IsChecked
		CheckedBorder = DarkBorder; // Set
		RefreshBorders();
	}

	private void SystemBorder_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
	{
		SystemRadioBtn.IsChecked = true; // Set IsChecked
		CheckedBorder = SystemBorder; // Set
		RefreshBorders();
	}

	private void RefreshBorders()
	{
		LightBorder.BorderBrush = new SolidColorBrush() { Color = Colors.Transparent }; // Set color 
		DarkBorder.BorderBrush = new SolidColorBrush() { Color = Colors.Transparent }; // Set color 
		SystemBorder.BorderBrush = new SolidColorBrush() { Color = Colors.Transparent }; // Set color 

		CheckedBorder.BorderBrush = new SolidColorBrush() { Color = (Color)ColorConverter.ConvertFromString(App.Current.Resources["AccentColor"].ToString()) }; // Set color
	}

	private void RefreshStartupBorders()
	{
		ConnectionPageBorder.BorderBrush = new SolidColorBrush() { Color = Colors.Transparent }; // Set color 
		LocateIPPageBorder.BorderBrush = new SolidColorBrush() { Color = Colors.Transparent }; // Set color 
		DownDetectorPageBorder.BorderBrush = new SolidColorBrush() { Color = Colors.Transparent }; // Set color 

		PageCheckedBorder.BorderBrush = new SolidColorBrush() { Color = (Color)ColorConverter.ConvertFromString(App.Current.Resources["AccentColor"].ToString()) }; // Set color
	}

	private void NotifyTestChk_Checked(object sender, RoutedEventArgs e)
	{
		Global.Settings.TestNotification = NotifyTestChk.IsChecked; // Set
		SettingsManager.Save(); // Save changes
	}

	Border PageCheckedBorder { get; set; }
	private void ConnectionPageRadioBtn_Checked(object sender, RoutedEventArgs e)
	{

	}

	private void LocateIPPageRadioBtn_Checked(object sender, RoutedEventArgs e)
	{

	}

	private void DownDetectorPageRadioBtn_Checked(object sender, RoutedEventArgs e)
	{

	}

	private void ConnectionPageBorder_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
	{
		PageCheckedBorder = ConnectionPageBorder; // Set
		ConnectionPageRadioBtn.IsChecked = true;
		RefreshStartupBorders(); // Refresh

		Global.Settings.StartupPage = StartPages.Connection;
		SettingsManager.Save(); // Save changes
	}

	private void LocateIPPageBorder_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
	{
		PageCheckedBorder = LocateIPPageBorder; // Set
		LocateIPPageRadioBtn.IsChecked = true;
		RefreshStartupBorders(); // Refresh

		Global.Settings.StartupPage = StartPages.LocalizeIP;
		SettingsManager.Save(); // Save changes
	}

	private void DownDetectorPageBorder_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
	{
		PageCheckedBorder = DownDetectorPageBorder; // Set
		DownDetectorPageRadioBtn.IsChecked = true;
		RefreshStartupBorders(); // Refresh

		Global.Settings.StartupPage = StartPages.DownDetector;
		SettingsManager.Save(); // Save changes
	}

	private void NotifyDownDetectorChk_Checked(object sender, RoutedEventArgs e)
	{
		Global.Settings.DownDetectorNotification = NotifyDownDetectorChk.IsChecked; // Set value
		SettingsManager.Save(); // Save changes
	}

	private void ResetSettingsLink_Click(object sender, RoutedEventArgs e)
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
				MapProvider = MapProviders.OpenStreetMap,
				LaunchTestOnStart = true,
				StartupPage = StartPages.Connection,
				UseHTTPS = true,
				IsThemeSystem = true,
				TestNotification = true,
				DownDetectorNotification = true,
				IsFirstRun = false, // Default is true but the user just want to reset their settings to default, so there is no need to put them back to the start
			}; // Create default settings

			SettingsManager.Save(); // Save the changes
			InitUI(); // Reload the page

			MessageBox.Show(Properties.Resources.SettingsReset, Properties.Resources.InternetTest, MessageBoxButton.OK, MessageBoxImage.Information);
			Process.Start(Directory.GetCurrentDirectory() + @"\InternetTest.exe");
			Environment.Exit(0); // Quit
		}
	}

	private void Button_Click(object sender, RoutedEventArgs e)
	{
		MessageBox.Show($"{Properties.Resources.Licenses}\n\n" +
			"Fluent System Icons - MIT License - © 2020 Microsoft Corporation\n" +
			"LeoCorpLibrary - MIT License - © 2020-2022 Léo Corporation\n" +
			"InternetTest - MIT License - © 2021-2022 Léo Corporation", $"{Properties.Resources.InternetTest} - {Properties.Resources.Licenses}", MessageBoxButton.OK, MessageBoxImage.Information);
	}

	private void CreditsBtn_Click(object sender, RoutedEventArgs e)
	{
		MessageBox.Show($"{Properties.Resources.CreditsAndThanks}\n\n" +
			$"@dependabot\n" +
			$"@Leo-Peyronnet\n" +
			$"@wcxu21",
			Properties.Resources.CreditsAndThanks, MessageBoxButton.OK, MessageBoxImage.Information);
	}

	private void UseHistoryLocateIPChk_Checked(object sender, RoutedEventArgs e)
	{

	}
}
