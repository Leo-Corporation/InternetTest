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
using InternetTest.UserControls;
using LeoCorpLibrary;
using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace InternetTest.Pages;

/// <summary>
/// Interaction logic for DownDetectorPage.xaml
/// </summary>
public partial class DownDetectorPage : Page
{
	readonly System.Windows.Forms.NotifyIcon notifyIcon = new();
	readonly DispatcherTimer dispatcherTimer = new();
	readonly DispatcherTimer secondsTimer = new();
	readonly DispatcherTimer dispatcherTimer2 = new();
	readonly DispatcherTimer secondsTimer2 = new();

	int secondsCheckTime = 30;
	int updateS = 0;
	readonly DoubleAnimation DoubleAnimation = new()
	{
		From = 80,
		To = 50,
		Duration = TimeSpan.FromMilliseconds(100)
	};

	public DownDetectorPage()
	{
		InitializeComponent();
		notifyIcon.Icon = System.Drawing.Icon.ExtractAssociatedIcon(AppDomain.CurrentDomain.BaseDirectory + @"\InternetTest.exe");

		InitUI();
	}

	readonly Storyboard storyboard = new();
	private void InitUI()
	{
		HistoryBtn.Visibility = Visibility.Collapsed; // Set visibility
		StatusBorder.Visibility = Visibility.Collapsed; // Hide

		secondsTimer.Interval = TimeSpan.FromSeconds(1); // Every second
		secondsTimer2.Interval = TimeSpan.FromSeconds(1); // Every second

		dispatcherTimer.Tick += (o, e) =>
		{
			if (string.IsNullOrEmpty(WebsiteTxt.Text))
			{
				AutoCheckWebsiteDownChk.IsChecked = false; // Stop
				MessageBox.Show(Properties.Resources.PleaseSpecifyWebsiteCheck, Properties.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Warning);
				return;
			}

			WebsiteTxt.Text = FormatURL(WebsiteTxt.Text);
			Test(WebsiteTxt.Text);
		};

		dispatcherTimer2.Tick += (o, e) =>
		{
			for (int i = 0; i < WebsiteItemPanel.Children.Count; i++)
			{
				var websiteItem = (WebsiteItem)WebsiteItemPanel.Children[i];
				websiteItem.Test();
			}
		};

		secondsTimer.Tick += (o, e) =>
		{
			updateS--;
			if (updateS < 0) updateS = secondsCheckTime - 1;
			NextCheckTxt.Text = $"{Properties.Resources.NextCheck} {updateS} {Properties.Resources.SecondsDotM}";
			storyboard.Begin(AlarmIconTxt, true);
		};

		secondsTimer2.Tick += (o, e) =>
		{
			updateSMW--;
			if (updateSMW < 0) updateSMW = secondsMWCheckTime - 1;
			MWNextCheckTxt.Text = $"{Properties.Resources.NextCheck} {updateSMW} {Properties.Resources.SecondsDotM}";
		};

		Storyboard.SetTargetName(DoubleAnimation, AlarmIconTxt.Name);
		Storyboard.SetTargetProperty(DoubleAnimation, new(TextBlock.FontSizeProperty));
		storyboard.Children.Add(DoubleAnimation);
		storyboard.AutoReverse = true;
	}

	/// <summary>
	/// Launch a network test.
	/// </summary>
	/// <param name="customSite">Leave empty if you don't want to specify a custom website.</param>
	private async void Test(string customSite)
	{
		StatusBorder.Visibility = Visibility.Collapsed; // Hide
		ConnectionStatusTxt.Text = Properties.Resources.Testing; // Set text of the label
		HistoryBtn.Visibility = Visibility.Visible; // Set visibility
		InternetIconTxt.Text = "\uF45F"; // Set the icon
		InternetIconTxt.Foreground = new SolidColorBrush { Color = (Color)ColorConverter.ConvertFromString(App.Current.Resources["Gray"].ToString()) }; // Set the foreground

		if (await NetworkConnection.IsAvailableAsync(customSite)) // If there is Internet
		{
			ConnectionStatusTxt.Text = Properties.Resources.WebsiteAvailable; // Set text of the label
			InternetIconTxt.Text = "\uF299"; // Set the icon
			InternetIconTxt.Foreground = new SolidColorBrush { Color = (Color)ColorConverter.ConvertFromString(App.Current.Resources["Green"].ToString()) }; // Set the foreground

			if (Global.Settings.DownDetectorNotification.Value)
			{
				notifyIcon.Visible = true; // Show
				notifyIcon.ShowBalloonTip(5000, Properties.Resources.InternetTest, Properties.Resources.WebsiteAvailable, System.Windows.Forms.ToolTipIcon.Info);
				notifyIcon.Visible = false; // Hide  
			}
		}
		else
		{
			ConnectionStatusTxt.Text = Properties.Resources.WebsiteDown; // Set text of the label
			InternetIconTxt.Text = "\uF36E"; // Set the icon
			InternetIconTxt.Foreground = new SolidColorBrush { Color = (Color)ColorConverter.ConvertFromString(App.Current.Resources["Red"].ToString()) }; // Set the foreground

			if (Global.Settings.DownDetectorNotification.Value)
			{
				notifyIcon.Visible = true; // Show
				notifyIcon.ShowBalloonTip(5000, Properties.Resources.InternetTest, Properties.Resources.WebsiteDown, System.Windows.Forms.ToolTipIcon.Info);
				notifyIcon.Visible = false; // Hide  
			}
		}

		StatusInfo statusInfo = await Global.GetStatusCodeAsync(customSite);
		int status = statusInfo.StatusCode;
		if (status >= 200 && status <= 299)
		{
			StatusBorder.Background = new SolidColorBrush { Color = (Color)ColorConverter.ConvertFromString(App.Current.Resources["Green"].ToString()) }; // Set the foreground
			StatusIconTxt.Text = "\uF299"; // Set text
		}
		else if (status >= 400 && status <= 599)
		{
			StatusBorder.Background = new SolidColorBrush { Color = (Color)ColorConverter.ConvertFromString(App.Current.Resources["Red"].ToString()) }; // Set the foreground
			StatusIconTxt.Text = "\uF36E"; // Set text
		}
		else
		{
			StatusBorder.Background = new SolidColorBrush { Color = (Color)ColorConverter.ConvertFromString(App.Current.Resources["AccentColor"].ToString()) }; // Set the foreground
			StatusIconTxt.Text = "\uF4AB"; // Set text
		}

		StatusCodeTxt.Text = status.ToString(); // Set text
		StatusBorder.Visibility = Visibility.Visible; // Show
		StatusToolTip.Content = $"{statusInfo.StatusCode} - {statusInfo.StatusMessage}"; // Set text
		StatusMsgTxt.Text = $"- {statusInfo.StatusMessage}"; // Set text

		HistoricDisplayer.Children.Add(new HistoricItem(customSite, ConnectionStatusTxt.Text, HistoricDisplayer)); // Add
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

	private void CheckBtn_Click(object sender, RoutedEventArgs e)
	{
		if (!string.IsNullOrEmpty(WebsiteTxt.Text) && !string.IsNullOrWhiteSpace(WebsiteTxt.Text))
		{
			Test(FormatURL(WebsiteTxt.Text)); // Test
			WebsiteTxt.Text = FormatURL(WebsiteTxt.Text); // Format 
		}
	}

	private void OpenBrowserBtn_Click(object sender, RoutedEventArgs e)
	{
		if (!string.IsNullOrEmpty(WebsiteTxt.Text) && !string.IsNullOrWhiteSpace(WebsiteTxt.Text))
		{
			Global.OpenLinkInBrowser(FormatURL(WebsiteTxt.Text)); // Open in a browser 
		}
	}

	internal void HistoryBtn_Click(object sender, RoutedEventArgs e)
	{
		if (HistoricDisplayer.Children.Count > 0)
		{
			HistoryBtn.Visibility = Visibility.Visible; // Set visibility
			if (HistoricPanel.Visibility == Visibility.Visible)
			{
				HistoricPanel.Visibility = Visibility.Collapsed; // Set
				TimerPanel.Visibility = Visibility.Collapsed; // Set
				MultipleWebsitesPanel.Visibility = Visibility.Collapsed; // Hide 

				ContentGrid.Visibility = Visibility.Visible; // Set
				HistoryBtn.Content = "\uF47F"; // Set text
			}
			else
			{
				HistoricPanel.Visibility = Visibility.Visible; // Set
				ContentGrid.Visibility = Visibility.Collapsed; // Set
				TimerPanel.Visibility = Visibility.Collapsed; // Set
				MultipleWebsitesPanel.Visibility = Visibility.Collapsed; // Hide 

				HistoryBtn.Content = "\uF36A"; // Set text
			}
		}
		else
		{
			HistoryBtn.Visibility = Visibility.Collapsed; // Set visibility
			HistoricPanel.Visibility = Visibility.Collapsed; // Set
			TimerPanel.Visibility = Visibility.Collapsed; // Set
			MultipleWebsitesPanel.Visibility = Visibility.Collapsed; // Hide 

			ContentGrid.Visibility = Visibility.Visible; // Set
			HistoryBtn.Content = "\uF47F"; // Set text
			if (sender is not HistoricItem)
			{
				MessageBox.Show(Properties.Resources.EmptyHistory, Properties.Resources.InternetTest, MessageBoxButton.OK, MessageBoxImage.Information); // Show message 
			}
		}
		TimeIntervalBtn.Content = "\uF827"; // Set text
		MultipleWebsitesBtn.Content = "\uF788"; // Set text

	}

	private void StatusBorder_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
	{
		StatusMsgTxt.Visibility = StatusMsgTxt.Visibility == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed; // Show/hide
	}

	private void AutoCheckWebsiteDownChk_Checked(object sender, RoutedEventArgs e)
	{
		try
		{
			if (AutoCheckWebsiteDownChk.IsChecked.Value)
			{
				if (string.IsNullOrEmpty(SecondsTxt.Text))
				{
					MessageBox.Show(Properties.Resources.PleaseSpecifyIntervalMsg, Properties.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
					AutoCheckWebsiteDownChk.IsChecked = false;
					return;
				}

				int seconds = int.Parse(SecondsTxt.Text);

				if (seconds < 2)
				{
					MessageBox.Show(Properties.Resources.CannotLessThanTwoSec, Properties.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Warning);
					AutoCheckWebsiteDownChk.IsChecked = false;
					return;
				}
				secondsCheckTime = seconds;
				updateS = seconds;

				dispatcherTimer.Interval = TimeSpan.FromSeconds(seconds);
				dispatcherTimer.Start(); // Start the task
				secondsTimer.Start(); // Start the task
				SecondsTxt.IsEnabled = false;
			}
			else
			{
				dispatcherTimer.Stop();
				secondsTimer.Stop();
				NextCheckTxt.Text = Properties.Resources.NoNextCheck;
				SecondsTxt.IsEnabled = true;

				storyboard.Stop();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, Properties.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
		}
	}

	private void TimeIntervalBtn_Click(object sender, RoutedEventArgs e)
	{
		if (TimerPanel.Visibility == Visibility.Visible)
		{
			HistoricPanel.Visibility = Visibility.Collapsed; // Hide
			TimerPanel.Visibility = Visibility.Collapsed; // Hide
			ContentGrid.Visibility = Visibility.Visible; // Show
			MultipleWebsitesPanel.Visibility = Visibility.Collapsed; // Hide 

			TimeIntervalBtn.Content = "\uF827"; // Set text
		}
		else
		{
			HistoricPanel.Visibility = Visibility.Collapsed; // Hide
			TimerPanel.Visibility = Visibility.Visible; // Show
			ContentGrid.Visibility = Visibility.Collapsed; // Hide
			MultipleWebsitesPanel.Visibility = Visibility.Collapsed; // Hide 

			TimeIntervalBtn.Content = "\uF36A"; // Set text
		}
		HistoryBtn.Content = "\uF47F"; // Set text
		MultipleWebsitesBtn.Content = "\uF788"; // Set text

	}

	private void SecondsTxt_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
	{
		Regex regex = new("[^0-9]+");
		e.Handled = regex.IsMatch(e.Text);
	}

	private void MultipleWebsitesBtn_Click(object sender, RoutedEventArgs e)
	{
		if (MultipleWebsitesPanel.Visibility == Visibility.Collapsed)
		{
			HistoricPanel.Visibility = Visibility.Collapsed; // Hide
			TimerPanel.Visibility = Visibility.Collapsed; // Hide
			ContentGrid.Visibility = Visibility.Collapsed; // Hide
			MultipleWebsitesPanel.Visibility = Visibility.Visible; // Show 

			MultipleWebsitesBtn.Content = "\uF36A"; // Set text
		}
		else
		{
			MultipleWebsitesPanel.Visibility = Visibility.Collapsed; // Hide 
			HistoricPanel.Visibility = Visibility.Collapsed; // Hide
			TimerPanel.Visibility = Visibility.Collapsed; // Hide
			ContentGrid.Visibility = Visibility.Visible; // Show

			MultipleWebsitesBtn.Content = "\uF788"; // Set text

		}
		HistoryBtn.Content = "\uF47F"; // Set text

	}

	private void AddBtn_Click(object sender, RoutedEventArgs e)
	{
		if (!string.IsNullOrEmpty(WebsiteTextBox.Text) && WebsiteTextBox.Text.Split(new string[] { "." }, StringSplitOptions.None).Length >= 2)
		{
			WebsiteItemPanel.Children.Add(new WebsiteItem(FormatURL(WebsiteTextBox.Text))); // Add website item
			WebsiteTextBox.Text = ""; // Empty
		}
		else
		{
			MessageBox.Show(Properties.Resources.PleaseSpecifyWebsiteCheck, Properties.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Exclamation); // Show error
		}
	}

	private void LaunchTestsBtn_Click(object sender, RoutedEventArgs e)
	{
		for (int i = 0; i < WebsiteItemPanel.Children.Count; i++)
		{
			var websiteItem = (WebsiteItem)WebsiteItemPanel.Children[i];
			websiteItem.Test();
		}
	}

	int secondsMWCheckTime = 30;
	int updateSMW = 0;
	private void AutoCheckMultipleWebsiteDownChk_Checked(object sender, RoutedEventArgs e)
	{
		try
		{
			if (AutoCheckMultipleWebsiteDownChk.IsChecked.Value)
			{
				if (string.IsNullOrEmpty(MWSecondsTxt.Text))
				{
					MessageBox.Show(Properties.Resources.PleaseSpecifyIntervalMsg, Properties.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
					AutoCheckMultipleWebsiteDownChk.IsChecked = false;
					return;
				}

				int seconds = int.Parse(MWSecondsTxt.Text);

				if (seconds < 2)
				{
					MessageBox.Show(Properties.Resources.CannotLessThanTwoSec, Properties.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Warning);
					AutoCheckMultipleWebsiteDownChk.IsChecked = false;
					return;
				}
				secondsMWCheckTime = seconds;
				updateSMW = seconds;

				dispatcherTimer2.Interval = TimeSpan.FromSeconds(seconds);
				dispatcherTimer2.Start(); // Start the task
				secondsTimer2.Start(); // Start the task
				MWSecondsTxt.IsEnabled = false;
			}
			else
			{
				dispatcherTimer2.Stop();
				secondsTimer2.Stop();
				MWNextCheckTxt.Text = Properties.Resources.NoNextCheck;
				MWSecondsTxt.IsEnabled = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, Properties.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
		}
	}
}
