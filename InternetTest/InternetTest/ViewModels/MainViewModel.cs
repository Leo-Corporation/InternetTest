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
using Hardcodet.Wpf.TaskbarNotification;
using InternetTest.Commands;
using InternetTest.Enums;
using InternetTest.Helpers;
using InternetTest.Interfaces;
using InternetTest.Models;
using InternetTest.ViewModels.Components;
using PeyrSharp.Core;
using PeyrSharp.Env;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace InternetTest.ViewModels;



public class MainViewModel : ViewModelBase
{
	private SidebarViewModel _sidebarViewModel;
	public SidebarViewModel SidebarViewModel
	{
		get { return _sidebarViewModel; }
		set { _sidebarViewModel = value; OnPropertyChanged(nameof(SidebarViewModel)); }
	}

	private object? _currentView;
	public object? CurrentViewModel
	{
		get { return _currentView; }
		set
		{
			_currentView = value;
			if (value is ISensitiveViewModel sensitiveViewModel) sensitiveViewModel.ToggleConfidentialMode(ConfidentialMode);
			OnPropertyChanged(nameof(CurrentViewModel));
		}
	}

	private bool _pinned;
	public bool Pinned
	{
		get => _pinned;
		set
		{
			_pinned = value;
			_mainWindow.Topmost = value;
			PinnedTooltip = value ? Properties.Resources.Unpin : Properties.Resources.Pin;

			OnPropertyChanged(nameof(Pinned));
		}
	}

	private string _pinnedTooltip = Properties.Resources.Pin;
	public string PinnedTooltip { get => _pinnedTooltip; set { _pinnedTooltip = value; OnPropertyChanged(nameof(PinnedTooltip)); } }

	private bool _confidentialMode;
	public bool ConfidentialMode
	{
		get => _confidentialMode;
		set
		{
			_confidentialMode = value;
			ConfidentialTooltip = value ? Properties.Resources.DisableConfidential : Properties.Resources.EnableConfidential;

			if (CurrentViewModel is ISensitiveViewModel sensitiveViewModel) sensitiveViewModel.ToggleConfidentialMode(value);

			OnPropertyChanged(nameof(ConfidentialMode));
		}
	}

	private string _confidentialTooltip = Properties.Resources.EnableConfidential;
	public string ConfidentialTooltip { get => _confidentialTooltip; set { _confidentialTooltip = value; OnPropertyChanged(nameof(ConfidentialTooltip)); } }

	public Settings Settings { get; set; }
	public ActivityHistory History { get; set; }
#if PORTABLE
	public static string Version => $"{Context.Version} (Portable)";
#else
	public static string Version => Context.Version;
#endif
	private readonly Window _mainWindow;

	public ICommand PinCommand { get; }
	public ICommand ToggleConfidentialModeCommand => new RelayCommand(o =>
	{
		ConfidentialMode = !ConfidentialMode;
	});
	public MainViewModel(Settings settings, ActivityHistory history, Window mainWindow)
	{
		Settings = settings;
		History = history;
		_mainWindow = mainWindow;
		_sidebarViewModel = new(this);

		Pinned = Settings.RememberPinnedState == true && (Settings.Pinned ?? false);
		ConfidentialMode = Settings.ToggleConfidentialMode ?? false;

		CurrentViewModel = Settings.DefaultPage switch
		{
			AppPages.DownDetector => new DownDetectorPageViewModel(Settings, History),
			AppPages.DnsTool => new DnsToolsPageViewModel(),
			AppPages.WiFiNetworks => new WiFiPageViewModel(Settings),
			AppPages.WiFiPasswords => new WiFiPageViewModel(Settings),
			AppPages.LocateIP => new LocateIpPageViewModel(Settings),
			AppPages.IPConfig => new IpConfigPageViewModel(),
			AppPages.Ping => new PingPageViewModel(Settings, History),
			AppPages.Requests => new RequestsPageViewModel(Settings),
			AppPages.TraceRoute => new TraceroutePageViewModel(Settings),
			_ => new HomePageViewModel(Settings, History, this)
		};


		PinCommand = new RelayCommand(Pin);

		CheckUpdates();
	}

	public void Pin(object? obj)
	{
		Pinned = !Pinned;

		if (Settings.RememberPinnedState == true)
		{
			Settings.Pinned = Pinned;
			Settings.Save();
		}
	}

	private async void CheckUpdates()
	{
		if (!Settings.CheckUpdateOnStart) return;
		if (!await Internet.IsAvailableAsync()) return;
		var lastVersion = await Update.GetLastVersionAsync(Context.UpdateVersionUrl);
		if (!Update.IsAvailable(Context.Version, lastVersion)) return;

		var notify = new TaskbarIcon()
		{
			Icon = System.Drawing.Icon.ExtractAssociatedIcon(AppDomain.CurrentDomain.BaseDirectory + @"\InternetTest.exe"),
			ToolTipText = "InternetTest",
		};
		notify.TrayBalloonTipClosed += (s, e) => { notify.Dispose(); };
		notify.TrayBalloonTipClicked += (s, e) =>
		{
#if PORTABLE
			MessageBox.Show(Properties.Resources.PortableNoAutoUpdates, $"{Properties.Resources.InstallVersion} {lastVersion}", MessageBoxButton.OK, MessageBoxImage.Information);
			return;
#else
			if (MessageBox.Show(Properties.Resources.InstallConfirmMsg, $"{Properties.Resources.InstallVersion} {lastVersion}", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
			{
				Sys.ExecuteAsAdmin(Directory.GetCurrentDirectory() + @"\Xalyus Updater.exe"); // Start the updater
				Application.Current.Shutdown(); // Close
			}
#endif
			notify.Dispose();
		};
		notify.ShowBalloonTip(Properties.Resources.Updates, Properties.Resources.AvailableUpdates, BalloonIcon.Info);
	}
}
