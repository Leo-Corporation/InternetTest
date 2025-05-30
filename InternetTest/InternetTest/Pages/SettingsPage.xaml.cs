﻿/*
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
using Microsoft.Win32;
using PeyrSharp.Core;
using PeyrSharp.Env;
using Synethia;
using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
					SynethiaManager.Save(Global.SynethiaConfig, Global.SynethiaPath);
					HistoryManager.Save(Global.History);
					SettingsManager.Save();

					Sys.ExecuteAsAdmin(Directory.GetCurrentDirectory() + @"\Xalyus Updater.exe"); // Start the updater
					Application.Current.Shutdown(); // Close
				}
			}
			catch { }
		};
		InitUI();
	}

	readonly System.Windows.Forms.NotifyIcon notifyIcon = new();
	bool updatesAvailable = false;
	private async void InitUI()
	{
		// About section
#if PORTABLE
		VersionTxt.Text = Global.Version + " (Portable)";
#else
		VersionTxt.Text = Global.Version;
#endif

		// Select the default Theme radiobutton
		switch (Global.Settings.Theme)
		{
			case Themes.Light:
				LightBtn.IsChecked = true;
				break;
			case Themes.Dark:
				DarkBtn.IsChecked = true;
				break;
			case Themes.System:
				SystemBtn.IsChecked = true;
				break;
		}


		// Select the language
		LangComboBox.SelectedIndex = (int)Global.Settings.Language;

		// Select the map provider
		MapProviderComboBox.SelectedIndex = (int)Global.Settings.MapProvider;
		ZoomLevelTxt.Text = Global.Settings.MapZoomLevel.ToString();

		// Notfication section
		UpdateNotifChk.IsChecked = Global.Settings.ShowNotficationWhenUpdateAvailable;

		// On Start section
		UpdateOnStartChk.IsChecked = Global.Settings.CheckUpdateOnStart;
		TestOnStartChk.IsChecked = Global.Settings.TestOnStart;
		LocateIpOnStartChk.IsChecked = Global.Settings.LaunchIpLocationOnStart ?? true;
		ToggleConfModeOnStartChk.IsChecked = Global.Settings.ToggleConfidentialMode;
		RememberPinOnStartChk.IsChecked = Global.Settings.RememberPinnedState;
		PageComboBox.SelectedIndex = (int)Global.Settings.DefaultPage;

		// Web related settings section
		HttpsRadio.IsChecked = Global.Settings.UseHttps;
		HttpRadio.IsChecked = !Global.Settings.UseHttps;
		SiteTxt.Text = Global.Settings.TestSite;

		// DownDetector section
		IntervalTxt.Text = Global.Settings.DefaultTimeInterval.ToString();

		// Trace route section
		HopsTxt.Text = Global.Settings.TraceRouteMaxHops.ToString();
		TimeOutTxt.Text = Global.Settings.TraceRouteMaxTimeOut.ToString();

		// Adapters section
		HideNetworkAdaptersChk.IsChecked = Global.Settings.HideDisabledAdapters;
		ShowNoIpv4Chk.IsChecked = Global.Settings.ShowAdaptersNoIpv4Support;

		// Data section
		UseSynethiaChk.IsChecked = Global.Settings.UseSynethia;

		// Check for updates
		if (!Global.Settings.CheckUpdateOnStart) return;
		try
		{
			if (!await Internet.IsAvailableAsync()) return;
			if (!Update.IsAvailable(Global.Version, await Update.GetLastVersionAsync(Global.LastVersionLink))) return;
		}
		catch { return; }

		// If updates are available
		// Update the UI
		updatesAvailable = true;
		CheckUpdateBtn.Content = Properties.Resources.Install;
		LoadUpdateSection();

		// Show notification
		if (!Global.Settings.ShowNotficationWhenUpdateAvailable) return;
		notifyIcon.Visible = true; // Show
		notifyIcon.ShowBalloonTip(5000, Properties.Resources.InternetTest, Properties.Resources.AvailableUpdates, System.Windows.Forms.ToolTipIcon.Info);
		notifyIcon.Visible = false; // Hide
	}

	internal void LoadUpdateSection()
	{
		if (updatesAvailable)
		{
			UpdateTxt.Text = Properties.Resources.AvailableUpdates;
			UpdateIconTxt.Text = "\uF86A";
			UpdateTxt.Foreground = Global.GetBrushFromResource("ForegroundOrange");
			UpdateIconTxt.Foreground = Global.GetBrushFromResource("ForegroundOrange");
			UpdateBorder.Background = Global.GetBrushFromResource("LightOrange");
			CheckUpdateBtn.Foreground = Global.GetBrushFromResource("ForegroundOrange");
			CheckUpdateBtn.Content = Properties.Resources.Install;
			CheckUpdateBtn.FontFamily = new(new Uri("pack://application:,,,/"), "./Fonts/#Hauora");
			CheckUpdateBtn.FontSize = 12;
			CheckUpdateBtn.FontWeight = FontWeights.ExtraBold;
		}
		else
		{
			UpdateTxt.Text = Properties.Resources.UpToDate;
			UpdateIconTxt.Text = "\uF299";
			UpdateTxt.Foreground = Global.GetBrushFromResource("ForegroundGreen");
			UpdateIconTxt.Foreground = Global.GetBrushFromResource("ForegroundGreen");
			UpdateBorder.Background = Global.GetBrushFromResource("LightGreen");
			CheckUpdateBtn.Foreground = Global.GetBrushFromResource("ForegroundGreen");
			CheckUpdateBtn.Content = "\uF191";
			CheckUpdateBtn.FontFamily = new(new Uri("pack://application:,,,/"), "./Fonts/#FluentSystemIcons-Regular");
			CheckUpdateBtn.FontSize = 14;
			CheckUpdateBtn.FontWeight = FontWeights.Normal;
		}
	}


	private async void CheckUpdateBtn_Click(object sender, RoutedEventArgs e)
	{
		try
		{
			string lastVersion = await Update.GetLastVersionAsync(Global.LastVersionLink);
			if (Update.IsAvailable(Global.Version, lastVersion))
			{
				updatesAvailable = true;
				LoadUpdateSection();

#if PORTABLE
				MessageBox.Show(Properties.Resources.PortableNoAutoUpdates, $"{Properties.Resources.InstallVersion} {lastVersion}", MessageBoxButton.OK, MessageBoxImage.Information);
				return;
#else
				if (MessageBox.Show(Properties.Resources.InstallConfirmMsg, $"{Properties.Resources.InstallVersion} {lastVersion}", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.No)
				{
					return;
				}
#endif
				// If the user wants to proceed.
				SynethiaManager.Save(Global.SynethiaConfig, Global.SynethiaPath);
				HistoryManager.Save(Global.History);
				SettingsManager.Save();

				Sys.ExecuteAsAdmin(Directory.GetCurrentDirectory() + @"\Xalyus Updater.exe"); // Start the updater
				Application.Current.Shutdown(); // Close
			}
			else
			{
				updatesAvailable = false;
				LoadUpdateSection();
			}
		}
		catch
		{
			UpdateTxt.Text = Properties.Resources.UnableToCheckUpdates;
		}
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

		SynethiaManager.Save(Global.SynethiaConfig, Global.SynethiaPath);
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

		SynethiaManager.Save(Global.SynethiaConfig, Global.SynethiaPath);
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
		SynethiaManager.Save(Global.SynethiaConfig, Global.SynethiaPath);

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

	private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
	{
		Regex regex = new("[^0-9]+");
		e.Handled = regex.IsMatch(e.Text);
	}

	private void ToggleConfModeOnStartChk_Checked(object sender, RoutedEventArgs e)
	{
		Global.Settings.ToggleConfidentialMode = ToggleConfModeOnStartChk.IsChecked;
		SettingsManager.Save();
	}

	private void RememberPinOnStartChk_Checked(object sender, RoutedEventArgs e)
	{
		Global.Settings.RememberPinnedState = RememberPinOnStartChk.IsChecked;
		SettingsManager.Save();
	}

	private void HopsApplyBtn_Click(object sender, RoutedEventArgs e)
	{
		Global.Settings.TraceRouteMaxHops = int.Parse(HopsTxt.Text);
		SettingsManager.Save();
	}

	private void TimeOutApplyBtn_Click(object sender, RoutedEventArgs e)
	{
		Global.Settings.TraceRouteMaxTimeOut = int.Parse(TimeOutTxt.Text);
		SettingsManager.Save();
	}

	private void LocateIpOnStartChk_Checked(object sender, RoutedEventArgs e)
	{
		Global.Settings.LaunchIpLocationOnStart = LocateIpOnStartChk.IsChecked;
		SettingsManager.Save();
	}

	private void EraseWiFisLink_Click(object sender, RoutedEventArgs e)
	{
		if (!Directory.Exists(FileSys.AppDataPath + @"\Léo Corporation\InternetTest Pro\WiFis")) return;

		if (MessageBox.Show(Properties.Resources.EraseWiFiPasswordsFilesMsg, Properties.Resources.InternetTestPro, MessageBoxButton.YesNoCancel, MessageBoxImage.Question) != MessageBoxResult.Yes) return;

		string path = FileSys.AppDataPath + @"\Léo Corporation\InternetTest Pro\WiFis";
		string[] files = Directory.GetFiles(path);
		for (int i = 0; i < files.Length; i++)
		{
			File.Delete(files[i]); // Remove the temp file
		}
		Directory.Delete(path); // Delete the temp directory		
	}

	private void IntervalApplyBtn_Click(object sender, RoutedEventArgs e)
	{
		if (string.IsNullOrEmpty(IntervalTxt.Text)) return;

		Global.Settings.DefaultTimeInterval = int.Parse(IntervalTxt.Text);
		SettingsManager.Save();
	}

	private void NetworkAdaptersChk_Checked(object sender, RoutedEventArgs e)
	{
		Global.Settings.HideDisabledAdapters = HideNetworkAdaptersChk.IsChecked;
		SettingsManager.Save();
	}

	private void SeeLicensesBtn_Click(object sender, RoutedEventArgs e)
	{
		MessageBox.Show($"{Properties.Resources.Licenses}\n\n" +
		"DnsClient - Apache License Version 2.0 - © Michael Conrad\n" +
		"Fluent System Icons - MIT License - © 2020 Microsoft Corporation\n" +
		"ManagedNativeWifi - MIT License - © 2015-2019 emoacht\n" +
		"PeyrSharp - MIT License - © 2022-2025 Devyus\n" +
		"QRCoder - MIT License - © 2013-2018 Raffael Herrmann\n" +
		"RestSharp - Apache License Version 2.0 - © .NET Foundation and Contributors\n" +
		"Synethia - MIT License - © 2023-2025 Devyus\n" +
		"Whois - MIT License - © 2012 Chris Wood\n" +
		"InternetTest - MIT License - © 2021-2025 Léo Corporation", $"{Properties.Resources.InternetTestPro} - {Properties.Resources.Licenses}", MessageBoxButton.OK, MessageBoxImage.Information);
	}

	private void GitHubBtn_Click(object sender, RoutedEventArgs e)
	{
		Process.Start("explorer.exe", "https://github.com/Leo-Corporation/InternetTest/");
	}

	private void ZoomLevelTxt_TextChanged(object sender, TextChangedEventArgs e)
	{
		if (string.IsNullOrEmpty(ZoomLevelTxt.Text)) return;
		Global.Settings.MapZoomLevel = int.Parse(ZoomLevelTxt.Text);
		SettingsManager.Save();
	}

	private void ShowNoIpv4Chk_Checked(object sender, RoutedEventArgs e)
	{
		Global.Settings.ShowAdaptersNoIpv4Support = ShowNoIpv4Chk.IsChecked;
		SettingsManager.Save();
	}

	private void LightBtn_Checked(object sender, RoutedEventArgs e)
	{
		if (LightBtn.IsChecked ?? false)
			Global.Settings.Theme = Themes.Light;
		else Global.Settings.Theme = DarkBtn.IsChecked ?? false ? Themes.Dark : Themes.System;

		SettingsManager.Save();
		Global.ChangeTheme();
		LoadUpdateSection();
	}
}
