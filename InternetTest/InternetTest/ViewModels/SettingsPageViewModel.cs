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
using InternetTest.Commands;
using InternetTest.Enums;
using InternetTest.Helpers;
using InternetTest.Models;
using Microsoft.Win32;
using PeyrSharp.Env;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace InternetTest.ViewModels;
public class SettingsPageViewModel : ViewModelBase
{
	private readonly MainViewModel _mainViewModel;

	public ICommand LightThemeCommand { get; }
	public ICommand DarkThemeCommand { get; }
	public ICommand SystemThemeCommand { get; }
	public ICommand ImportCommand { get; }
	public ICommand ExportCommand { get; }
	public ICommand ResetCommand { get; }
	public ICommand ResetWiFiCommand { get; }
	public ICommand CheckUpdateCommand { get; }
	public ICommand SeeLicensesCommand { get; }
	public ICommand GitHubCommand { get; }

	private Theme _currentTheme;
	public Theme CurrentTheme { get => _currentTheme; set { _currentTheme = value; OnPropertyChanged(nameof(CurrentTheme)); } }

	private AppPages _defaultPage;
	public AppPages DefaultPage
	{
		get => _defaultPage;
		set
		{
			_defaultPage = value;
			_mainViewModel.Settings.DefaultPage = value;
			_mainViewModel.Settings.Save(); // Save settings after changing default page

			OnPropertyChanged(nameof(DefaultPage));
		}
	}

	private int _currentLanguage;
	public int CurrentLanguage
	{
		get => _currentLanguage;
		set
		{
			_currentLanguage = value;
			_mainViewModel.Settings.Language = (Language)value;
			_mainViewModel.Settings.Save(); // Save settings after changing language

			OnPropertyChanged(nameof(CurrentLanguage));
		}
	}

	private bool _useHttps;
	public bool UseHttps
	{
		get => _useHttps; set
		{
			_useHttps = value;
			_mainViewModel.Settings.UseHttps = value;
			_mainViewModel.Settings.Save(); // Save settings after changing UseHttps

			OnPropertyChanged(nameof(UseHttps));
		}
	}

	private string? _testSite;
	public string TestSite
	{
		get => _testSite ?? "";
		set
		{
			_testSite = value;
			_mainViewModel.Settings.TestSite = value;
			_mainViewModel.Settings.Save();

			OnPropertyChanged(nameof(TestSite));
		}
	}

	private bool _checkUpdateOnStart;
	public bool CheckUpdateOnStart
	{
		get => _checkUpdateOnStart;
		set
		{
			_checkUpdateOnStart = value;
			_mainViewModel.Settings.CheckUpdateOnStart = value;
			_mainViewModel.Settings.Save();

			OnPropertyChanged(nameof(CheckUpdateOnStart));
		}
	}

	private bool _showNotificationWhenUpdateAvailable;
	public bool ShowNotificationWhenUpdateAvailable
	{
		get => _showNotificationWhenUpdateAvailable;
		set
		{
			_showNotificationWhenUpdateAvailable = value;
			_mainViewModel.Settings.ShowNotficationWhenUpdateAvailable = value;
			_mainViewModel.Settings.Save();

			OnPropertyChanged(nameof(ShowNotificationWhenUpdateAvailable));
		}
	}

	private bool _launchTestOnStart;
	public bool LaunchTestOnStart
	{
		get => _launchTestOnStart;
		set
		{
			_launchTestOnStart = value;
			_mainViewModel.Settings.TestOnStart = value;
			_mainViewModel.Settings.Save();

			OnPropertyChanged(nameof(LaunchTestOnStart));
		}
	}

	private bool _locateIpOnStart;
	public bool LocateIpOnStart
	{
		get => _locateIpOnStart;
		set
		{
			_locateIpOnStart = value;
			_mainViewModel.Settings.LaunchIpLocationOnStart = value;
			_mainViewModel.Settings.Save();

			OnPropertyChanged(nameof(LocateIpOnStart));
		}
	}

	private bool _toggleConfidentialMode;
	public bool ToggleConfidentialMode
	{
		get => _toggleConfidentialMode;
		set
		{
			_toggleConfidentialMode = value;
			_mainViewModel.Settings.ToggleConfidentialMode = value;
			_mainViewModel.Settings.Save();

			OnPropertyChanged(nameof(ToggleConfidentialMode));
		}
	}

	private bool _rememberPinState;
	public bool RememberPinState
	{
		get => _rememberPinState;
		set
		{
			_rememberPinState = value;
			_mainViewModel.Settings.RememberPinnedState = value;
			_mainViewModel.Settings.Save();

			OnPropertyChanged(nameof(RememberPinState));
		}
	}

	private int _mapProvider;
	public int MapProvider
	{
		get => _mapProvider;
		set
		{
			_mapProvider = value;
			_mainViewModel.Settings.MapProvider = (MapProvider)value;
			_mainViewModel.Settings.Save();

			OnPropertyChanged(nameof(MapProvider));
		}
	}

	private bool _hideDisabledAdapters;
	public bool HideDisabledAdapters
	{
		get => _hideDisabledAdapters;
		set
		{
			_hideDisabledAdapters = value;
			_mainViewModel.Settings.HideDisabledAdapters = value;
			_mainViewModel.Settings.Save();

			OnPropertyChanged(nameof(HideDisabledAdapters));
		}
	}

	private bool _showAdaptersNoIpv4Support;
	public bool ShowAdaptersNoIpv4Support
	{
		get => _showAdaptersNoIpv4Support;
		set
		{
			_showAdaptersNoIpv4Support = value;
			_mainViewModel.Settings.ShowAdaptersNoIpv4Support = value;
			_mainViewModel.Settings.Save();

			OnPropertyChanged(nameof(ShowAdaptersNoIpv4Support));
		}
	}

	private int _interval;
	public int Interval
	{
		get => _interval;
		set
		{
			_interval = value;
			_mainViewModel.Settings.DefaultTimeInterval = value;
			_mainViewModel.Settings.Save();

			OnPropertyChanged(nameof(Interval));
		}
	}

	private int _zoomLevel;
	public int ZoomLevel
	{
		get => _zoomLevel;
		set
		{
			_zoomLevel = value;
			_mainViewModel.Settings.MapZoomLevel = value;
			_mainViewModel.Settings.Save();

			OnPropertyChanged(nameof(ZoomLevel));
		}
	}

	private int _maxHops;
	public int MaxHops
	{
		get => _maxHops;
		set
		{
			_maxHops = value;
			_mainViewModel.Settings.TraceRouteMaxHops = value;
			_mainViewModel.Settings.Save();

			OnPropertyChanged(nameof(MaxHops));
		}
	}

	private int _maxTimeOut;
	public int MaxTimeOut
	{
		get => _maxTimeOut;
		set
		{
			_maxTimeOut = value;
			_mainViewModel.Settings.TraceRouteMaxTimeOut = value;
			_mainViewModel.Settings.Save();

			OnPropertyChanged(nameof(MaxTimeOut));
		}
	}

	private string _updateText;
	public string UpdateText { get => _updateText; set { _updateText = value; OnPropertyChanged(nameof(UpdateText)); } }

	private string _updateIcon;
	public string UpdateIcon { get => _updateIcon; set { _updateIcon = value; OnPropertyChanged(nameof(UpdateIcon)); } }

	private string _updateBtnIcon;
	public string UpdateBtnIcon { get => _updateBtnIcon; set { _updateBtnIcon = value; OnPropertyChanged(nameof(UpdateBtnIcon)); } }

	private SolidColorBrush _updateBorderColor;
	public SolidColorBrush UpdateBorderColor { get => _updateBorderColor; set { _updateBorderColor = value; OnPropertyChanged(nameof(UpdateBorderColor)); } }

	private SolidColorBrush _updateTextColor;
	public SolidColorBrush UpdateTextColor { get => _updateTextColor; set { _updateTextColor = value; OnPropertyChanged(nameof(UpdateTextColor)); } }

	public string Version => Context.Version;

	public SettingsPageViewModel(MainViewModel mainViewModel)
	{
		_mainViewModel = mainViewModel;

		CurrentTheme = mainViewModel.Settings.Theme;
		CurrentLanguage = (int)mainViewModel.Settings.Language;
		UseHttps = mainViewModel.Settings.UseHttps;
		TestSite = mainViewModel.Settings.TestSite ?? "https://leocorporation.dev"; // Default test site
		CheckUpdateOnStart = mainViewModel.Settings.CheckUpdateOnStart;
		ShowNotificationWhenUpdateAvailable = mainViewModel.Settings.ShowNotficationWhenUpdateAvailable;
		LaunchTestOnStart = mainViewModel.Settings.TestOnStart;
		LocateIpOnStart = mainViewModel.Settings.LaunchIpLocationOnStart ?? true;
		ToggleConfidentialMode = mainViewModel.Settings.ToggleConfidentialMode ?? false;
		RememberPinState = mainViewModel.Settings.RememberPinnedState ?? true;
		MapProvider = (int)mainViewModel.Settings.MapProvider;
		HideDisabledAdapters = mainViewModel.Settings.HideDisabledAdapters ?? false;
		ShowAdaptersNoIpv4Support = mainViewModel.Settings.ShowAdaptersNoIpv4Support ?? false;
		Interval = mainViewModel.Settings.DefaultTimeInterval ?? 10;
		ZoomLevel = mainViewModel.Settings.MapZoomLevel ?? 12;
		MaxHops = mainViewModel.Settings.TraceRouteMaxHops ?? 30;
		MaxTimeOut = mainViewModel.Settings.TraceRouteMaxTimeOut ?? 5000;

		LightThemeCommand = new RelayCommand(LightTheme);
		DarkThemeCommand = new RelayCommand(DarkTheme);
		SystemThemeCommand = new RelayCommand(SystemTheme);
		ImportCommand = new RelayCommand(ImportSettings);
		ExportCommand = new RelayCommand(ExportSettings);
		ResetCommand = new RelayCommand(ResetSettings);
		ResetWiFiCommand = new RelayCommand(ResetWiFiSettings);
		CheckUpdateCommand = new RelayCommand(CheckUpdate);
		SeeLicensesCommand = new RelayCommand(SeeLicenses);
		GitHubCommand = new RelayCommand(GitHub);

		if (_mainViewModel.Settings.CheckUpdateOnStart)
		{
			CheckUpdate(true); // Check for updates on settings page load
		}
		else
		{
			UpdateText = Properties.Resources.UnableToCheckUpdates;
			UpdateBorderColor = ThemeHelper.GetSolidColorBrush("LightAccent");
			UpdateTextColor = ThemeHelper.GetSolidColorBrush("Accent");
			UpdateIcon = "\uF2B1"; // Default icon
			UpdateBtnIcon = "\uF191"; // Default button icon
		}
	}

	private void LightTheme(object? obj)
	{
		_mainViewModel.Settings.Theme = Theme.Light;
		_mainViewModel.Settings.Save(); // Save settings after changing theme

		ThemeHelper.ChangeTheme(Theme.Light);
	}

	private void DarkTheme(object? obj)
	{
		_mainViewModel.Settings.Theme = Theme.Dark;
		_mainViewModel.Settings.Save(); // Save settings after changing theme

		ThemeHelper.ChangeTheme(Theme.Dark);
	}

	private void SystemTheme(object? obj)
	{
		_mainViewModel.Settings.Theme = Theme.System;
		_mainViewModel.Settings.Save(); // Save settings after changing theme

		ThemeHelper.ChangeTheme(Theme.System);
	}

	private void ImportSettings(object? obj)
	{
		OpenFileDialog openFileDialog = new()
		{
			Filter = "XML|*.xml",
			Title = Properties.Resources.Import
		}; // Create file dialog

		if (openFileDialog.ShowDialog() ?? true)
		{
			_mainViewModel.Settings.Import(openFileDialog.FileName); // Import games
			_mainViewModel.CurrentViewModel = new SettingsPageViewModel(_mainViewModel); // Refresh the view model
		}
	}

	private void ExportSettings(object? obj)
	{
		SaveFileDialog saveFileDialog = new()
		{
			FileName = "InternetTestSettings.xml",
			Filter = "XML|*.xml",
			Title = Properties.Resources.Export
		}; // Create file dialog

		if (saveFileDialog.ShowDialog() ?? true)
		{
			_mainViewModel.Settings.Export(saveFileDialog.FileName); // Export games
		}
	}

	private void ResetSettings(object? obj)
	{
		if (MessageBox.Show(Properties.Resources.ResetSettingsConfirmation, Properties.Resources.Settings, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
		{
			return;
		}

		_mainViewModel.Settings = new Settings(); // Reset settings to default
		_mainViewModel.Settings.Save(); // Save the reset settings

		if (MessageBox.Show(Properties.Resources.NeedRestartToApplyChanges, Properties.Resources.Settings, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
		{
			return;
		}

		Process.Start(Directory.GetCurrentDirectory() + @"\InternetTest.exe");
		Application.Current.Shutdown();
	}

	private void ResetWiFiSettings(object? obj)
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

	private void SeeLicenses(object? obj)
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

	private void GitHub(object? obj)
	{
		Process.Start("explorer.exe", "https://github.com/Leo-Corporation/InternetTest/");
	}

	private async void CheckUpdate(object? obj)
	{
		try
		{
			string lastVersion = await Update.GetLastVersionAsync(Context.UpdateVersionUrl);
			if (Update.IsAvailable(Context.Version, lastVersion))
			{
				UpdateBorderColor = ThemeHelper.GetSolidColorBrush("LightOrange");
				UpdateTextColor = ThemeHelper.GetSolidColorBrush("ForegroundOrange");
				UpdateText = Properties.Resources.AvailableUpdates;
				UpdateIcon = "\uF86A"; // Update icon
				UpdateBtnIcon = "\uF151"; // Update button icon

				if (obj is bool value && value)
				{
					// If the update check was triggered on startup, we don't show the message box.
					return;
				}

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

				Sys.ExecuteAsAdmin(Directory.GetCurrentDirectory() + @"\Xalyus Updater.exe"); // Start the updater
				Application.Current.Shutdown(); // Close
			}
			else
			{
				UpdateBorderColor = ThemeHelper.GetSolidColorBrush("LightGreen");
				UpdateTextColor = ThemeHelper.GetSolidColorBrush("ForegroundGreen");
				UpdateText = Properties.Resources.UpToDate;
				UpdateIcon = "\uF299"; // Update icon
				UpdateBtnIcon = "\uF191"; // Update button icon
			}
		}
		catch
		{
			UpdateText = Properties.Resources.UnableToCheckUpdates;
		}
	}
}