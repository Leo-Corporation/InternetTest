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
	public SettingsPage()
	{
		InitializeComponent();
		notifyIcon.Icon = System.Drawing.Icon.ExtractAssociatedIcon(AppDomain.CurrentDomain.BaseDirectory + @"\InternetTest.exe");
		notifyIcon.BalloonTipClicked += async (o, e) =>
		{
			try
			{
				string lastVersion = await Update.GetLastVersionAsync(Global.LastVersionLink); // Get last version
				if (MessageBox.Show(Properties.Resources.InstallConfirmMsg, $"{Properties.Resources.InstallVersion} {lastVersion}", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
				{
					SynethiaManager.Save(Global.SynethiaConfig);
					HistoryManager.Save(Global.History);
					SettingsManager.Save();

					Env.ExecuteAsAdmin(Directory.GetCurrentDirectory() + @"\Xalyus Updater.exe"); // Start the updater
					Application.Current.Shutdown(); // Close
				}
			}
			catch { }
		};
		InitUI();
	}

	readonly System.Windows.Forms.NotifyIcon notifyIcon = new();
	private async void InitUI()
	{
		// About section
		VersionTxt.Text = Global.Version; // Update the current version label

		// Select the default theme border
		ThemeSelectedBorder = Global.Settings.Theme switch
		{
			Themes.Light => LightBorder,
			Themes.Dark => DarkBorder,
			_ => SystemBorder
		};
		Border_MouseEnter(Global.Settings.Theme switch
		{
			Themes.Light => LightBorder,
			Themes.Dark => DarkBorder,
			_ => SystemBorder
		}, null);

		// Select the language
		LangComboBox.SelectedIndex = (int)Global.Settings.Language;

		// Select the map provider
		MapProviderComboBox.SelectedIndex = (int)Global.Settings.MapProvider;

		// Notfication section
		UpdateNotifChk.IsChecked = Global.Settings.ShowNotficationWhenUpdateAvailable;

		// On Start section
		UpdateOnStartChk.IsChecked = Global.Settings.CheckUpdateOnStart;
		TestOnStartChk.IsChecked = Global.Settings.TestOnStart;
		PageComboBox.SelectedIndex = (int)Global.Settings.DefaultPage;

		// Web related settings section
		HttpsRadio.IsChecked = Global.Settings.UseHttps;
		HttpRadio.IsChecked = !Global.Settings.UseHttps;
		SiteTxt.Text = Global.Settings.TestSite;

		// Data section
		UseSynethiaChk.IsChecked = Global.Settings.UseSynethia;

		// Check for updates
		if (!Global.Settings.CheckUpdateOnStart) return;
		try
		{
			if (!await NetworkConnection.IsAvailableAsync()) return;
			if (!Update.IsAvailable(Global.Version, await Update.GetLastVersionAsync(Global.LastVersionLink))) return;
		}
		catch { return; }

		// If updates are available
		// Update the UI
		CheckUpdateBtn.Content = Properties.Resources.Install;
		UpdateTxt.Text = Properties.Resources.AvailableUpdates;

		// Show notification
		if (!Global.Settings.ShowNotficationWhenUpdateAvailable) return;
		notifyIcon.Visible = true; // Show
		notifyIcon.ShowBalloonTip(5000, Properties.Resources.InternetTest, Properties.Resources.AvailableUpdates, System.Windows.Forms.ToolTipIcon.Info);
		notifyIcon.Visible = false; // Hide
	}

	private async void CheckUpdateBtn_Click(object sender, RoutedEventArgs e)
	{
		string lastVersion = await Update.GetLastVersionAsync(Global.LastVersionLink);
		if (Update.IsAvailable(Global.Version, lastVersion))
		{
			UpdateTxt.Text = Properties.Resources.AvailableUpdates;

			if (MessageBox.Show(Properties.Resources.InstallConfirmMsg, $"{Properties.Resources.InstallVersion} {lastVersion}", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.No)
			{
				return;
			}

			// If the user wants to proceed.
			SynethiaManager.Save(Global.SynethiaConfig);
			HistoryManager.Save(Global.History);
			SettingsManager.Save();

			Env.ExecuteAsAdmin(Directory.GetCurrentDirectory() + @"\Xalyus Updater.exe"); // Start the updater
			Application.Current.Shutdown(); // Close
		}
		else
		{
			UpdateTxt.Text = Properties.Resources.UpToDate;
		}
	}

	Border ThemeSelectedBorder;
	private void Border_MouseEnter(object sender, MouseEventArgs e)
	{
		((Border)sender).BorderBrush = new SolidColorBrush { Color = Global.GetColorFromResource("AccentColor") };
	}

	private void Border_MouseLeave(object sender, MouseEventArgs e)
	{
		if ((Border)sender == ThemeSelectedBorder) return;
		((Border)sender).BorderBrush = new SolidColorBrush { Color = Colors.Transparent };
	}

	private void ResetBorders()
	{
		LightBorder.BorderBrush = new SolidColorBrush { Color = Colors.Transparent };
		DarkBorder.BorderBrush = new SolidColorBrush { Color = Colors.Transparent };
		SystemBorder.BorderBrush = new SolidColorBrush { Color = Colors.Transparent };
	}

	private void LightBorder_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
	{
		ResetBorders();
		ThemeSelectedBorder = (Border)sender;
		((Border)sender).BorderBrush = new SolidColorBrush { Color = Global.GetColorFromResource("AccentColor") };
		Global.Settings.Theme = Themes.Light;
		SettingsManager.Save();

		if (MessageBox.Show(Properties.Resources.NeedRestartToApplyChanges, Properties.Resources.Settings, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
		{
			return;
		}

		SynethiaManager.Save(Global.SynethiaConfig);
		HistoryManager.Save(Global.History);

		Process.Start(Directory.GetCurrentDirectory() + @"\InternetTest.exe");
		Application.Current.Shutdown();
	}

	private void DarkBorder_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
	{
		ResetBorders();
		ThemeSelectedBorder = (Border)sender;
		((Border)sender).BorderBrush = new SolidColorBrush { Color = Global.GetColorFromResource("AccentColor") };
		Global.Settings.Theme = Themes.Dark;
		SettingsManager.Save();

		if (MessageBox.Show(Properties.Resources.NeedRestartToApplyChanges, Properties.Resources.Settings, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
		{
			return;
		}

		SynethiaManager.Save(Global.SynethiaConfig);
		HistoryManager.Save(Global.History);

		Process.Start(Directory.GetCurrentDirectory() + @"\InternetTest.exe");
		Application.Current.Shutdown();
	}

	private void SystemBorder_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
	{
		ResetBorders();
		ThemeSelectedBorder = (Border)sender;
		((Border)sender).BorderBrush = new SolidColorBrush { Color = Global.GetColorFromResource("AccentColor") };
		Global.Settings.Theme = Themes.System;
		SettingsManager.Save();

		if (MessageBox.Show(Properties.Resources.NeedRestartToApplyChanges, Properties.Resources.Settings, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
		{
			return;
		}

		SynethiaManager.Save(Global.SynethiaConfig);
		HistoryManager.Save(Global.History);

		Process.Start(Directory.GetCurrentDirectory() + @"\InternetTest.exe");
		Application.Current.Shutdown();
	}

	private void LangComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		LangApplyBtn.Visibility = Visibility.Visible; // Show apply button
	}

	private void LangApplyBtn_Click(object sender, RoutedEventArgs e)
	{
		Global.Settings.Language = (Languages)LangComboBox.SelectedIndex;
		SettingsManager.Save();
		LangApplyBtn.Visibility = Visibility.Hidden; // Hide apply button

		if (MessageBox.Show(Properties.Resources.NeedRestartToApplyChanges, Properties.Resources.Settings, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
		{
			return;
		}

		SynethiaManager.Save(Global.SynethiaConfig);
		HistoryManager.Save(Global.History);

		Process.Start(Directory.GetCurrentDirectory() + @"\InternetTest.exe");
		Application.Current.Shutdown();
	}

	private void MapProviderComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		Global.Settings.MapProvider = (MapProvider)MapProviderComboBox.SelectedIndex;
		SettingsManager.Save();
	}

	private void UpdateNotifChk_Checked(object sender, RoutedEventArgs e)
	{
		Global.Settings.ShowNotficationWhenUpdateAvailable = UpdateNotifChk.IsChecked ?? true;
		SettingsManager.Save();
	}

	private void PageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		Global.Settings.DefaultPage = (AppPages)PageComboBox.SelectedIndex;
		SettingsManager.Save();
	}

	private void UpdateOnStartChk_Checked(object sender, RoutedEventArgs e)
	{
		Global.Settings.CheckUpdateOnStart = UpdateOnStartChk.IsChecked ?? true;
		SettingsManager.Save();
	}

	private void HttpsRadio_Checked(object sender, RoutedEventArgs e)
	{
		Global.Settings.UseHttps = HttpsRadio.IsChecked ?? true;
		SettingsManager.Save();
	}

	private void HttpRadio_Checked(object sender, RoutedEventArgs e)
	{
		Global.Settings.UseHttps = !HttpRadio.IsChecked ?? false;
		SettingsManager.Save();
	}

	private void SiteApplyBtn_Click(object sender, RoutedEventArgs e)
	{
		Global.Settings.TestSite = SiteTxt.Text;
		SettingsManager.Save();
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

	private void ResetSettingsLink_Click(object sender, RoutedEventArgs e)
	{
		if (MessageBox.Show(Properties.Resources.ResetSettingsConfirmation, Properties.Resources.Settings, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
		{
			return;
		}

		Global.Settings = new() { IsFirstRun = false };
		SettingsManager.Save();

		if (MessageBox.Show(Properties.Resources.NeedRestartToApplyChanges, Properties.Resources.Settings, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
		{
			return;
		}

		SynethiaManager.Save(Global.SynethiaConfig);
		HistoryManager.Save(Global.History);
		Process.Start(Directory.GetCurrentDirectory() + @"\InternetTest.exe");
		Application.Current.Shutdown();
	}

	private void UseSynethiaChk_Checked(object sender, RoutedEventArgs e)
	{
		Global.Settings.UseSynethia = UseSynethiaChk.IsChecked ?? true;
		SettingsManager.Save();
	}

	private void TestOnStartChk_Checked(object sender, RoutedEventArgs e)
	{
		Global.Settings.TestOnStart = TestOnStartChk.IsChecked ?? true;
		SettingsManager.Save();
	}

	private void ResetSynethiaLink_Click(object sender, RoutedEventArgs e)
	{
		// Ask the user a confirmation
		if (MessageBox.Show(Properties.Resources.SynethiaDeleteMsg, Properties.Resources.Settings, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
		{
			return;
		}

		// If the user wants to proceed, reset Syenthia config file.
		Global.SynethiaConfig = new();
		SynethiaManager.Save(Global.SynethiaConfig);

		// Ask the user if they want to restart the application to apply changes.
		if (MessageBox.Show(Properties.Resources.NeedRestartToApplyChanges, Properties.Resources.Settings, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
		{
			return;
		}

		// If the user wants to restart the app, save and quit the app
		SettingsManager.Save(); // Save settings
		HistoryManager.Save(Global.History); // Save history content
		Process.Start(Directory.GetCurrentDirectory() + @"\InternetTest.exe"); // Start a new instance
		Application.Current.Shutdown(); // Quit this current instance
	}
}
