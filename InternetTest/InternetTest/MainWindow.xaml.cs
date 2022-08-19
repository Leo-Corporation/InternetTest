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
using LeoCorpLibrary;
using System;
using System.Windows;
using System.Windows.Controls;
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
		};
		Closed += (o, e) => LeavePage();
		HelloTxt.Text = Global.GetHiSentence; // Show greeting message to the user

		// Show the appropriate page
		PageDisplayer.Content = Global.HomePage; // Show the Home page
		CheckButton(HomePageBtn, true);

		PageCard.OnCardClick += PageCard_OnCardClick;
		ActionCard.OnCardClick += PageCard_OnCardClick;
	}

	private void PageCard_OnCardClick(object? sender, PageEventArgs e)
	{
		switch (e.AppPage)
		{
			case AppPages.Status:
				PageDisplayer.Content = Global.StatusPage;
				Global.SynethiaConfig.StatusPageInfo.EnterUnixTime = Env.UnixTime;
				UnCheckAllButton();
				CheckButton(StatusPageBtn);
				break;
			case AppPages.DownDetector:
				PageDisplayer.Content = Global.DownDetectorPage;
				Global.SynethiaConfig.DownDetectorPageInfo.EnterUnixTime = Env.UnixTime;
				UnCheckAllButton();
				CheckButton(DownDetectorPageBtn);
				break;
			case AppPages.MyIP:
				PageDisplayer.Content = Global.MyIpPage;
				Global.SynethiaConfig.MyIPPageInfo.EnterUnixTime = Env.UnixTime;
				UnCheckAllButton();
				CheckButton(MyIPPageBtn);
				break;
			case AppPages.LocateIP:
				PageDisplayer.Content = Global.LocateIpPage;
				Global.SynethiaConfig.LocateIPPageInfo.EnterUnixTime = Env.UnixTime;
				UnCheckAllButton();
				CheckButton(LocateIPPageBtn);
				break;
			case AppPages.Ping:
				PageDisplayer.Content = Global.PingPage;
				Global.SynethiaConfig.PingPageInfo.EnterUnixTime = Env.UnixTime;
				UnCheckAllButton();
				CheckButton(PingPageBtn);
				break;
			case AppPages.IPConfig:
				PageDisplayer.Content = Global.IpConfigPage;
				Global.SynethiaConfig.IPConfigPageInfo.EnterUnixTime = Env.UnixTime;
				UnCheckAllButton();
				CheckButton(IPConfigPageBtn);
				break;
			case AppPages.WiFiPasswords:
				PageDisplayer.Content = Global.WiFiPasswordsPage;
				Global.SynethiaConfig.WiFiPasswordsPageInfo.EnterUnixTime = Env.UnixTime;
				UnCheckAllButton();
				CheckButton(WifiPasswordsPageBtn);
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

	private void CheckButton(Button button, bool isSpecial = false)
	{
		if (isSpecial)
		{
			button.Background = new SolidColorBrush(Global.GetColorFromResource("Background1"));
		}
		else
		{
			button.Background = new SolidColorBrush(Global.GetColorFromResource("AccentColor"));
			button.Foreground = new SolidColorBrush(Global.GetColorFromResource("WindowButtonsHoverForeground1"));
		}
	}

	private void UnCheckAllButton()
	{
		// Background
		HomePageBtn.Background = new SolidColorBrush(Colors.Transparent);
		HistoryPageBtn.Background = new SolidColorBrush(Colors.Transparent);
		SettingsPageBtn.Background = new SolidColorBrush(Colors.Transparent);

		StatusPageBtn.Background = new SolidColorBrush(Colors.Transparent);
		DownDetectorPageBtn.Background = new SolidColorBrush(Colors.Transparent);
		MyIPPageBtn.Background = new SolidColorBrush(Colors.Transparent);
		LocateIPPageBtn.Background = new SolidColorBrush(Colors.Transparent);
		PingPageBtn.Background = new SolidColorBrush(Colors.Transparent);
		IPConfigPageBtn.Background = new SolidColorBrush(Colors.Transparent);
		WifiPasswordsPageBtn.Background = new SolidColorBrush(Colors.Transparent);

		StatusPageBtn.Foreground = new SolidColorBrush(Global.GetColorFromResource("AccentColor"));
		DownDetectorPageBtn.Foreground = new SolidColorBrush(Global.GetColorFromResource("AccentColor"));
		MyIPPageBtn.Foreground = new SolidColorBrush(Global.GetColorFromResource("AccentColor"));
		LocateIPPageBtn.Foreground = new SolidColorBrush(Global.GetColorFromResource("AccentColor"));
		PingPageBtn.Foreground = new SolidColorBrush(Global.GetColorFromResource("AccentColor"));
		IPConfigPageBtn.Foreground = new SolidColorBrush(Global.GetColorFromResource("AccentColor"));
		WifiPasswordsPageBtn.Foreground = new SolidColorBrush(Global.GetColorFromResource("AccentColor"));
	}

	private void StatusPageBtn_Click(object sender, RoutedEventArgs e)
	{
		LeavePage();
		UnCheckAllButton(); // Reset all states
		CheckButton(StatusPageBtn);

		PageDisplayer.Content = Global.StatusPage; // Display the status page
		Global.SynethiaConfig.StatusPageInfo.EnterUnixTime = Env.UnixTime; // Update the last entered time
	}

	private void DownDetectorPageBtn_Click(object sender, RoutedEventArgs e)
	{
		LeavePage();
		UnCheckAllButton(); // Reset all states
		CheckButton(DownDetectorPageBtn);

		PageDisplayer.Content = Global.DownDetectorPage; // Display the down detector page
		Global.SynethiaConfig.DownDetectorPageInfo.EnterUnixTime = Env.UnixTime; // Update the last entered time
	}

	private void MyIPPageBtn_Click(object sender, RoutedEventArgs e)
	{
		LeavePage();
		UnCheckAllButton(); // Reset all states
		CheckButton(MyIPPageBtn);

		PageDisplayer.Content = Global.MyIpPage; // Display the my IP page
		Global.SynethiaConfig.MyIPPageInfo.EnterUnixTime = Env.UnixTime; // Update the last entered time
	}

	private void LocateIPPageBtn_Click(object sender, RoutedEventArgs e)
	{
		LeavePage();
		UnCheckAllButton(); // Reset all states
		CheckButton(LocateIPPageBtn);

		PageDisplayer.Content = Global.LocateIpPage; // Display the locate IP page
		Global.SynethiaConfig.LocateIPPageInfo.EnterUnixTime = Env.UnixTime; // Update the last entered time
	}

	private void PingPageBtn_Click(object sender, RoutedEventArgs e)
	{
		LeavePage();
		UnCheckAllButton(); // Reset all states
		CheckButton(PingPageBtn);

		PageDisplayer.Content = Global.PingPage; // Display the ping page
		Global.SynethiaConfig.PingPageInfo.EnterUnixTime = Env.UnixTime; // Update the last entered time
	}

	private void IPConfigPageBtn_Click(object sender, RoutedEventArgs e)
	{
		LeavePage();
		UnCheckAllButton(); // Reset all states
		CheckButton(IPConfigPageBtn);

		PageDisplayer.Content = Global.IpConfigPage; // Display the IP config page
		Global.SynethiaConfig.IPConfigPageInfo.EnterUnixTime = Env.UnixTime; // Update the last entered time
	}

	private void WifiPasswordsPageBtn_Click(object sender, RoutedEventArgs e)
	{
		LeavePage();
		UnCheckAllButton(); // Reset all states
		CheckButton(WifiPasswordsPageBtn);

		PageDisplayer.Content = Global.WiFiPasswordsPage; // Display the wifi passwords page
		Global.SynethiaConfig.WiFiPasswordsPageInfo.EnterUnixTime = Env.UnixTime; // Update the last entered time
	}

	private void HomePageBtn_Click(object sender, RoutedEventArgs e)
	{
		LeavePage();
		UnCheckAllButton(); // Reset all states
		CheckButton(HomePageBtn, true);

		PageDisplayer.Content = Global.HomePage; // Display the home page
	}

	private void HistoryPageBtn_Click(object sender, RoutedEventArgs e)
	{
		LeavePage();
		UnCheckAllButton(); // Reset all states
		CheckButton(HistoryPageBtn, true);

		PageDisplayer.Content = Global.HistoryPage; // Display the history page
	}

	private void SettingsPageBtn_Click(object sender, RoutedEventArgs e)
	{
		UnCheckAllButton(); // Reset all states
		CheckButton(SettingsPageBtn, true);
	}

	private void LeavePage()
	{
		if (PageDisplayer.Content is StatusPage)
		{
			Global.SynethiaConfig.StatusPageInfo.LeaveUnixTime = Env.UnixTime;
			Global.SynethiaConfig.StatusPageInfo.TotalTimeSpent += Global.SynethiaConfig.StatusPageInfo.LeaveUnixTime - Global.SynethiaConfig.StatusPageInfo.EnterUnixTime;
			Global.SynethiaConfig.StatusPageInfo.Score = Global.SynethiaConfig.StatusPageInfo.TotalTimeSpent * (Global.SynethiaConfig.StatusPageInfo.InteractionCount > 0 ? Global.SynethiaConfig.StatusPageInfo.InteractionCount / 2d : 1d); // Calculate the score
		}
		else if (PageDisplayer.Content is DownDetectorPage)
		{
			Global.SynethiaConfig.DownDetectorPageInfo.LeaveUnixTime = Env.UnixTime;
			Global.SynethiaConfig.DownDetectorPageInfo.TotalTimeSpent += Global.SynethiaConfig.DownDetectorPageInfo.LeaveUnixTime - Global.SynethiaConfig.DownDetectorPageInfo.EnterUnixTime;
			Global.SynethiaConfig.DownDetectorPageInfo.Score = Global.SynethiaConfig.DownDetectorPageInfo.TotalTimeSpent * (Global.SynethiaConfig.DownDetectorPageInfo.InteractionCount > 0 ? Global.SynethiaConfig.DownDetectorPageInfo.InteractionCount / 2d : 1d); // Calculate the score
		}
		else if (PageDisplayer.Content is MyIpPage)
		{
			Global.SynethiaConfig.MyIPPageInfo.LeaveUnixTime = Env.UnixTime;
			Global.SynethiaConfig.MyIPPageInfo.TotalTimeSpent += Global.SynethiaConfig.MyIPPageInfo.LeaveUnixTime - Global.SynethiaConfig.MyIPPageInfo.EnterUnixTime;
			Global.SynethiaConfig.MyIPPageInfo.Score = Global.SynethiaConfig.MyIPPageInfo.TotalTimeSpent * (Global.SynethiaConfig.MyIPPageInfo.InteractionCount > 0 ? Global.SynethiaConfig.MyIPPageInfo.InteractionCount / 2d : 1d); // Calculate the score
		}
		else if (PageDisplayer.Content is LocateIpPage)
		{
			Global.SynethiaConfig.LocateIPPageInfo.LeaveUnixTime = Env.UnixTime;
			Global.SynethiaConfig.LocateIPPageInfo.TotalTimeSpent += Global.SynethiaConfig.LocateIPPageInfo.LeaveUnixTime - Global.SynethiaConfig.LocateIPPageInfo.EnterUnixTime;
			Global.SynethiaConfig.LocateIPPageInfo.Score = Global.SynethiaConfig.LocateIPPageInfo.TotalTimeSpent * (Global.SynethiaConfig.LocateIPPageInfo.InteractionCount > 0 ? Global.SynethiaConfig.LocateIPPageInfo.InteractionCount / 2d : 1d); // Calculate the score
		}
		else if (PageDisplayer.Content is PingPage)
		{
			Global.SynethiaConfig.PingPageInfo.LeaveUnixTime = Env.UnixTime;
			Global.SynethiaConfig.PingPageInfo.TotalTimeSpent += Global.SynethiaConfig.PingPageInfo.LeaveUnixTime - Global.SynethiaConfig.PingPageInfo.EnterUnixTime;
			Global.SynethiaConfig.PingPageInfo.Score = Global.SynethiaConfig.PingPageInfo.TotalTimeSpent * (Global.SynethiaConfig.PingPageInfo.InteractionCount > 0 ? Global.SynethiaConfig.PingPageInfo.InteractionCount / 2d : 1d); // Calculate the score
		}
		else if (PageDisplayer.Content is IpConfigPage)
		{
			Global.SynethiaConfig.IPConfigPageInfo.LeaveUnixTime = Env.UnixTime;
			Global.SynethiaConfig.IPConfigPageInfo.TotalTimeSpent += Global.SynethiaConfig.IPConfigPageInfo.LeaveUnixTime - Global.SynethiaConfig.IPConfigPageInfo.EnterUnixTime;
			Global.SynethiaConfig.IPConfigPageInfo.Score = Global.SynethiaConfig.IPConfigPageInfo.TotalTimeSpent * (Global.SynethiaConfig.IPConfigPageInfo.InteractionCount > 0 ? Global.SynethiaConfig.IPConfigPageInfo.InteractionCount / 2d : 1d); // Calculate the score
		}
		else if (PageDisplayer.Content is WiFiPasswordsPage)
		{
			Global.SynethiaConfig.WiFiPasswordsPageInfo.LeaveUnixTime = Env.UnixTime;
			Global.SynethiaConfig.WiFiPasswordsPageInfo.TotalTimeSpent += Global.SynethiaConfig.WiFiPasswordsPageInfo.LeaveUnixTime - Global.SynethiaConfig.WiFiPasswordsPageInfo.EnterUnixTime;
			Global.SynethiaConfig.WiFiPasswordsPageInfo.Score = Global.SynethiaConfig.WiFiPasswordsPageInfo.TotalTimeSpent * (Global.SynethiaConfig.WiFiPasswordsPageInfo.InteractionCount > 0 ? Global.SynethiaConfig.WiFiPasswordsPageInfo.InteractionCount / 2d : 1d); // Calculate the score
		}
	}
}
