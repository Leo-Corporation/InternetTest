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
using InternetTest.Pages;
using InternetTest.UserControls;
using PeyrSharp.Env;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace InternetTest;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
	public MainWindow()
	{
		InitializeComponent();
		InitUI();
		GC.Collect();
	}

	DoubleAnimation expandAnimation = new()
	{
		From = 0,
		To = 180,
		Duration = new Duration(TimeSpan.FromSeconds(0.2)),
	};

	DoubleAnimation collapseAnimation = new()
	{
		From = 180,
		To = 0,
		Duration = new Duration(TimeSpan.FromSeconds(0.2)),
	};

	private void InitUI()
	{
		StateChanged += (o, e) => HandleWindowStateChanged();
		Loaded += (o, e) => HandleWindowStateChanged();
		LocationChanged += (o, e) => HandleWindowStateChanged();
		SizeChanged += (o, e) =>
		{
			PageScroller.Height = (ActualHeight - (GridRow1.ActualHeight + 68) > 0) ? ActualHeight - (GridRow1.ActualHeight + 68) : 0; // Set the scroller height
			ActionsScrollViewer.Height = ActualHeight - SideBarTop.ActualHeight - GridRow1.ActualHeight - 60;
			Global.Settings.MainWindowSize = (ActualWidth, ActualHeight);
		};
		Closed += (o, e) => LeavePage();
		HelloTxt.Text = Global.GetHiSentence; // Show greeting message to the user

		// Show the appropriate page
		switch (Global.Settings.DefaultPage)
		{
			case AppPages.History:
				PageDisplayer.Content = Global.HistoryPage;
				HistoryPageBtn.IsChecked = true;
				break;
			case AppPages.DownDetector:
				PageDisplayer.Content = Global.DownDetectorPage;
				Global.SynethiaConfig.PagesInfo[0].EnterUnixTime = Sys.UnixTime;
				DownDetectorPageBtn.IsChecked = true;
				break;
			case AppPages.LocateIP:
				PageDisplayer.Content = Global.LocateIpPage;
				Global.SynethiaConfig.PagesInfo[3].EnterUnixTime = Sys.UnixTime;
				LocateIPPageBtn.IsChecked = true;
				break;
			case AppPages.Ping:
				PageDisplayer.Content = Global.PingPage;
				Global.SynethiaConfig.PagesInfo[5].EnterUnixTime = Sys.UnixTime;
				PingPageBtn.IsChecked = true;
				break;
			case AppPages.IPConfig:
				PageDisplayer.Content = Global.IpConfigPage;
				Global.SynethiaConfig.PagesInfo[4].EnterUnixTime = Sys.UnixTime;
				IPConfigPageBtn.IsChecked = true;
				break;
			case AppPages.WiFiPasswords:
				PageDisplayer.Content = Global.WiFiPasswordsPage;
				Global.SynethiaConfig.PagesInfo[7].EnterUnixTime = Sys.UnixTime;
				WifiPasswordsPageBtn.IsChecked = true;
				break;
			case AppPages.DnsTool:
				PageDisplayer.Content = Global.DnsPage;
				Global.SynethiaConfig.PagesInfo[1].EnterUnixTime = Sys.UnixTime;
				DnsPageBtn.IsChecked = true;
				break;
			case AppPages.TraceRoute:
				PageDisplayer.Content = Global.TraceroutePage;
				Global.SynethiaConfig.PagesInfo[6].EnterUnixTime = Sys.UnixTime;
				TraceroutePageBtn.IsChecked = true;
				break;
			case AppPages.WiFiNetworks:
				PageDisplayer.Content = Global.WiFiNetworksPage;
				Global.SynethiaConfig.PagesInfo[2].EnterUnixTime = Sys.UnixTime;
				WiFiPageBtn.IsChecked = true;
				break;
			default:
				PageDisplayer.Content = Global.HomePage;
				HomePageBtn.IsChecked = true;
				break;
		}

		// Register event handlers
		PageCard.OnCardClick += PageCard_OnCardClick;

		// Restore the previous Window state
		WindowState = Global.Settings.IsMaximized ?? false ? WindowState.Maximized : WindowState.Normal;

		// Toggle on or off Confidential Mode
		if (Global.Settings.ToggleConfidentialMode ?? false) ConfidentialModeBtn_Click(this, null);

		// Toggle the "pin" state
		if ((Global.Settings.RememberPinnedState ?? true) && (Global.Settings.Pinned ?? false)) PinBtn_Click(this, null);

		// Restore the previous size
		if (!Global.Settings.IsMaximized ?? false)
		{
			Width = Global.Settings.MainWindowSize?.Item1 ?? 950;
			Height = Global.Settings.MainWindowSize?.Item2 ?? 600;
		}

		// Version
		VersionTxt.Text = Global.Version;
	}

	private void PageCard_OnCardClick(object? sender, PageEventArgs e)
	{
		switch (e.AppPage)
		{
			case AppPages.DownDetector:
				PageDisplayer.Content = Global.DownDetectorPage;
				Global.SynethiaConfig.PagesInfo[0].EnterUnixTime = Sys.UnixTime;
				DownDetectorPageBtn.IsChecked = true;
				break;
			case AppPages.LocateIP:
				PageDisplayer.Content = Global.LocateIpPage;
				Global.SynethiaConfig.PagesInfo[3].EnterUnixTime = Sys.UnixTime;
				LocateIPPageBtn.IsChecked = true;
				break;
			case AppPages.Ping:
				PageDisplayer.Content = Global.PingPage;
				Global.SynethiaConfig.PagesInfo[5].EnterUnixTime = Sys.UnixTime;
				PingPageBtn.IsChecked = true;
				break;
			case AppPages.IPConfig:
				PageDisplayer.Content = Global.IpConfigPage;
				Global.SynethiaConfig.PagesInfo[4].EnterUnixTime = Sys.UnixTime;
				IPConfigPageBtn.IsChecked = true;
				break;
			case AppPages.WiFiPasswords:
				PageDisplayer.Content = Global.WiFiPasswordsPage;
				Global.SynethiaConfig.PagesInfo[7].EnterUnixTime = Sys.UnixTime;
				WifiPasswordsPageBtn.IsChecked = true;
				break;
			case AppPages.DnsTool:
				PageDisplayer.Content = Global.DnsPage;
				Global.SynethiaConfig.PagesInfo[1].EnterUnixTime = Sys.UnixTime;
				DnsPageBtn.IsChecked = true;
				break;
			case AppPages.TraceRoute:
				PageDisplayer.Content = Global.TraceroutePage;
				Global.SynethiaConfig.PagesInfo[6].EnterUnixTime = Sys.UnixTime;
				TraceroutePageBtn.IsChecked = true;
				break;
			case AppPages.WiFiNetworks:
				PageDisplayer.Content = Global.WiFiNetworksPage;
				Global.SynethiaConfig.PagesInfo[2].EnterUnixTime = Sys.UnixTime;
				WiFiPageBtn.IsChecked = true;
				break;
			default:
				break;
		}
	}

	private void MinimizeBtn_Click(object sender, RoutedEventArgs e)
	{
		WindowState = WindowState.Minimized; // Minimize the window
	}

	private void MaximizeRestoreBtn_Click(object sender, RoutedEventArgs e)
	{
		WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;

		HandleWindowStateChanged();
	}

	private void CloseBtn_Click(object sender, RoutedEventArgs e)
	{
		LeavePage();
		Application.Current.Shutdown(); // Close the application
	}

	private void HandleWindowStateChanged()
	{
		MaximizeRestoreBtn.Content = WindowState == WindowState.Maximized
			? "\uF670" // Restore icon
			: "\uFA40"; // Maximize icon
		MaximizeRestoreBtn.FontSize = WindowState == WindowState.Maximized
			? 18
			: 14;

		DefineMaximumSize();

		WindowBorder.Margin = WindowState == WindowState.Maximized ? new(10, 10, 0, 0) : new(10); // Set
		WindowBorder.CornerRadius = WindowState == WindowState.Maximized ? new(0) : new(5); // Set

		// Update settings information
		Global.Settings.IsMaximized = WindowState == WindowState.Maximized;
		SettingsManager.Save();
	}

	private void DefineMaximumSize()
	{
		System.Windows.Forms.Screen currentScreen = System.Windows.Forms.Screen.FromHandle(new System.Windows.Interop.WindowInteropHelper(this).Handle); // The current screen

		float dpiX, dpiY;
		double scaling = 100; // Default scaling = 100%

		using (System.Drawing.Graphics graphics = System.Drawing.Graphics.FromHwnd(IntPtr.Zero))
		{
			dpiX = graphics.DpiX; // Get the DPI
			dpiY = graphics.DpiY; // Get the DPI

			scaling = dpiX switch
			{
				96 => 100, // Get the %
				120 => 125, // Get the %
				144 => 150, // Get the %
				168 => 175, // Get the %
				192 => 200, // Get the % 
				_ => 100
			};
		}

		double factor = scaling / 100d; // Calculate factor

		MaxHeight = currentScreen.WorkingArea.Height / factor + 5; // Set max size
		MaxWidth = currentScreen.WorkingArea.Width / factor + 5; // Set max size
	}

	private void WebUtilitiesBtn_Click(object sender, RoutedEventArgs e)
	{
		bool expanded = UtilitiesPanel.Visibility == Visibility.Visible;
		UtilitiesPanel.Visibility = expanded ? Visibility.Collapsed : Visibility.Visible; // Show/hide the utilities panel

		Storyboard storyboard = new(); // Create a storyboard

		storyboard.Children.Add(expanded ? collapseAnimation : expandAnimation);
		Storyboard.SetTargetName(expanded ? collapseAnimation : expandAnimation, "UtilRotator");
		Storyboard.SetTargetProperty(expanded ? collapseAnimation : expandAnimation, new(RotateTransform.AngleProperty));

		storyboard.Begin(this); // Animate the utilities panel
	}

	private void IPToolsBtn_Click(object sender, RoutedEventArgs e)
	{
		bool expanded = IPPanel.Visibility == Visibility.Visible;
		IPPanel.Visibility = expanded ? Visibility.Collapsed : Visibility.Visible; // Show/hide the IP tools panel

		Storyboard storyboard = new(); // Create a storyboard

		storyboard.Children.Add(expanded ? collapseAnimation : expandAnimation);
		Storyboard.SetTargetName(expanded ? collapseAnimation : expandAnimation, "IPRotator");
		Storyboard.SetTargetProperty(expanded ? collapseAnimation : expandAnimation, new(RotateTransform.AngleProperty));

		storyboard.Begin(this); // Animate the utilities panel
	}

	private void CommandsBtn_Click(object sender, RoutedEventArgs e)
	{
		bool expanded = CommandPanel.Visibility == Visibility.Visible;
		CommandPanel.Visibility = expanded ? Visibility.Collapsed : Visibility.Visible; // Show/hide the commands panel
		Storyboard storyboard = new(); // Create a storyboard

		storyboard.Children.Add(expanded ? collapseAnimation : expandAnimation);
		Storyboard.SetTargetName(expanded ? collapseAnimation : expandAnimation, "CommandRotator");
		Storyboard.SetTargetProperty(expanded ? collapseAnimation : expandAnimation, new(RotateTransform.AngleProperty));

		storyboard.Begin(this); // Animate the utilities panel
	}


	private void DownDetectorPageBtn_Click(object sender, RoutedEventArgs e)
	{
		LeavePage();

		PageDisplayer.Content = Global.DownDetectorPage; // Display the down detector page
		Global.SynethiaConfig.PagesInfo[0].EnterUnixTime = Sys.UnixTime; // Update the last entered time
	}

	private void LocateIPPageBtn_Click(object sender, RoutedEventArgs e)
	{
		LeavePage();

		PageDisplayer.Content = Global.LocateIpPage; // Display the locate IP page
		Global.SynethiaConfig.PagesInfo[3].EnterUnixTime = Sys.UnixTime; // Update the last entered time
	}

	private void PingPageBtn_Click(object sender, RoutedEventArgs e)
	{
		LeavePage();

		PageDisplayer.Content = Global.PingPage; // Display the ping page
		Global.SynethiaConfig.PagesInfo[5].EnterUnixTime = Sys.UnixTime; // Update the last entered time
	}

	private void IPConfigPageBtn_Click(object sender, RoutedEventArgs e)
	{
		LeavePage();

		PageDisplayer.Content = Global.IpConfigPage; // Display the IP config page
		Global.SynethiaConfig.PagesInfo[4].EnterUnixTime = Sys.UnixTime; // Update the last entered time
	}

	private void WifiPasswordsPageBtn_Click(object sender, RoutedEventArgs e)
	{
		LeavePage();

		PageDisplayer.Content = Global.WiFiPasswordsPage; // Display the wifi passwords page
		Global.SynethiaConfig.PagesInfo[7].EnterUnixTime = Sys.UnixTime; // Update the last entered time
	}

	private void HomePageBtn_Click(object sender, RoutedEventArgs e)
	{
		LeavePage();
		HomePageBtn.IsChecked = true;
		PageDisplayer.Content = Global.HomePage; // Display the home page
	}

	private void HistoryPageBtn_Click(object sender, RoutedEventArgs e)
	{
		LeavePage();

		PageDisplayer.Content = Global.HistoryPage; // Display the history page
	}

	private void SettingsPageBtn_Click(object sender, RoutedEventArgs e)
	{
		LeavePage();

		PageDisplayer.Content = Global.SettingsPage;
	}

	private void LeavePage()
	{
		if (!Global.Settings.UseSynethia) return;
		switch (PageDisplayer.Content)
		{
			case DownDetectorPage:
				Global.SynethiaConfig.PagesInfo[0].LeaveUnixTime = Sys.UnixTime;
				Global.SynethiaConfig.PagesInfo[0].TotalTimeSpent += Global.SynethiaConfig.PagesInfo[0].LeaveUnixTime - Global.SynethiaConfig.PagesInfo[0].EnterUnixTime;
				break;
			case LocateIpPage:
				Global.SynethiaConfig.PagesInfo[3].LeaveUnixTime = Sys.UnixTime;
				Global.SynethiaConfig.PagesInfo[3].TotalTimeSpent += Global.SynethiaConfig.PagesInfo[3].LeaveUnixTime - Global.SynethiaConfig.PagesInfo[3].EnterUnixTime;
				break;
			case PingPage:
				Global.SynethiaConfig.PagesInfo[5].LeaveUnixTime = Sys.UnixTime;
				Global.SynethiaConfig.PagesInfo[5].TotalTimeSpent += Global.SynethiaConfig.PagesInfo[5].LeaveUnixTime - Global.SynethiaConfig.PagesInfo[5].EnterUnixTime;
				break;
			case IpConfigPage:
				Global.SynethiaConfig.PagesInfo[4].LeaveUnixTime = Sys.UnixTime;
				Global.SynethiaConfig.PagesInfo[4].TotalTimeSpent += Global.SynethiaConfig.PagesInfo[4].LeaveUnixTime - Global.SynethiaConfig.PagesInfo[4].EnterUnixTime;
				break;
			case WiFiPasswordsPage:
				Global.SynethiaConfig.PagesInfo[7].LeaveUnixTime = Sys.UnixTime;
				Global.SynethiaConfig.PagesInfo[7].TotalTimeSpent += Global.SynethiaConfig.PagesInfo[7].LeaveUnixTime - Global.SynethiaConfig.PagesInfo[7].EnterUnixTime;
				break;
			case DnsPage:
				Global.SynethiaConfig.PagesInfo[1].LeaveUnixTime = Sys.UnixTime;
				Global.SynethiaConfig.PagesInfo[1].TotalTimeSpent += Global.SynethiaConfig.PagesInfo[1].LeaveUnixTime - Global.SynethiaConfig.PagesInfo[1].EnterUnixTime;
				break;
			case TraceroutePage:
				Global.SynethiaConfig.PagesInfo[6].LeaveUnixTime = Sys.UnixTime;
				Global.SynethiaConfig.PagesInfo[6].TotalTimeSpent += Global.SynethiaConfig.PagesInfo[6].LeaveUnixTime - Global.SynethiaConfig.PagesInfo[6].EnterUnixTime;
				break;
			case WiFiNetworksPage:
				Global.SynethiaConfig.PagesInfo[2].LeaveUnixTime = Sys.UnixTime;
				Global.SynethiaConfig.PagesInfo[2].TotalTimeSpent += Global.SynethiaConfig.PagesInfo[2].LeaveUnixTime - Global.SynethiaConfig.PagesInfo[2].EnterUnixTime;
				break;
		}
	}

	private void ConfidentialModeBtn_Click(object sender, RoutedEventArgs e)
	{
		Global.IsConfidentialModeEnabled = !Global.IsConfidentialModeEnabled; // Toggle

		RegularLockTxt.Visibility = Global.IsConfidentialModeEnabled ? Visibility.Collapsed : Visibility.Visible;
		FilledLockTxt.Visibility = !Global.IsConfidentialModeEnabled ? Visibility.Collapsed : Visibility.Visible;

		Global.LocateIpPage?.ToggleConfidentialMode(Global.IsConfidentialModeEnabled);
		Global.IpConfigPage?.ToggleConfidentialMode(Global.IsConfidentialModeEnabled);
		Global.WiFiPasswordsPage?.ToggleConfidentialMode();
	}

	private void PinBtn_Click(object sender, RoutedEventArgs e)
	{
		Topmost = !Topmost; // Toggle
		PinBtn.Content = Topmost ? "\uF604" : "\uF602"; // Set text
		Global.Settings.Pinned = Topmost;
	}

	private void DnsPageBtn_Click(object sender, RoutedEventArgs e)
	{
		LeavePage();

		PageDisplayer.Content = Global.DnsPage; // Display the IP config page
		Global.SynethiaConfig.PagesInfo[1].EnterUnixTime = Sys.UnixTime; // Update the last entered time
	}

	private void TraceroutePageBtn_Click(object sender, RoutedEventArgs e)
	{
		LeavePage();

		PageDisplayer.Content = Global.TraceroutePage; // Display the ping page
		Global.SynethiaConfig.PagesInfo[6].EnterUnixTime = Sys.UnixTime; // Update the last entered time
	}

	private void WiFiPageBtn_Click(object sender, RoutedEventArgs e)
	{
		LeavePage();

		PageDisplayer.Content = Global.WiFiNetworksPage;
		Global.SynethiaConfig.PagesInfo[2].EnterUnixTime = Sys.UnixTime;
	}
}
