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
using InternetTest.UserControls;
using PeyrSharp.Core;
using PeyrSharp.Core.Converters;
using PeyrSharp.Enums;
using Synethia;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace InternetTest.Pages
{
	/// <summary>
	/// Interaction logic for DownDetectorPage.xaml
	/// </summary>
	public partial class DownDetectorPage : Page
	{
		DownDetectorTestResult CurrentResult { get; set; } = new(0, 0, "");
		internal int TotalWebsites { get; set; } = 1;
		bool codeInjected = !Global.Settings.UseSynethia;

		public DownDetectorPage()
		{
			InitializeComponent();
			InitUI(); // Load the UI
			Loaded += (o, e) => SynethiaManager.InjectSynethiaCode(this, Global.SynethiaConfig.PagesInfo, 0, ref codeInjected);
		}

		private void InitUI()
		{
			TitleTxt.Text = $"{Properties.Resources.WebUtilities} > {Properties.Resources.DownDetector}"; // Set the title of the page
			TimeIntervalTxt.Text = string.Format(Properties.Resources.ScheduledTestInterval, 10); // Set the time interval text
			WebsiteTxt.Text = Global.Settings.TestSite ?? "https://leocorporation.dev";
		}
		
		internal async void TestBtn_Click(object sender, RoutedEventArgs e)
		{
			results.Clear();
			// Check if the URL is valid
			if (!WebsiteTxt.Text.StartsWith("http"))
			{
				WebsiteTxt.Text = Global.Settings.UseHttps ? "https://" : "http://" + WebsiteTxt.Text;
			}

			TotalWebsites = DownDetectorItemDisplayer.Children.Count + ((!string.IsNullOrEmpty(WebsiteTxt.Text)) ? 1 : 0);
			TestBtn.Content = $"{Properties.Resources.Test} ({TotalWebsites})";

			// Test the current website
			CurrentResult = await LaunchTest(WebsiteTxt.Text, true);

			// If there are any ohther websites, test them
			for (int i = 0; i < DownDetectorItemDisplayer.Children.Count; i++)
			{
				if (DownDetectorItemDisplayer.Children[i] is DownDetectorItem item)
				{
					item.WebsiteTxt.Foreground = new SolidColorBrush(Global.GetColorFromResource("Foreground1"));
					item.DownDetectorTestResult = await LaunchTest(item.WebsiteTxt.Text);
					item.WebsiteTxt.Foreground = new SolidColorBrush(Global.GetColorFromResource("DarkGray"));
					item.UpdateIcon();
				}
			}

			SetTestIconResult(results);

			// Increment the interaction count of the ActionInfo in Global.SynethiaConfig
			Global.SynethiaConfig.ActionsInfo.First(a => a.Name == "DownDetector.Test").UsageCount++;
		}

		readonly List<StatusCodes> results = new();
		internal async Task<DownDetectorTestResult> LaunchTest(string url, bool isFirst = false)
		{
			try
			{
				if (!url.StartsWith("http"))
				{
					url = Global.Settings.UseHttps ? "https://" : "http://" + url;
				}

				if (!Global.IsUrlValid(url)) return new(0, 0, "Invalid URL");

				// Show the "Waiting" screen
				StatusIconTxt.Visibility = Visibility.Collapsed;
				Spinner.Visibility = Visibility.Visible;
				StatusTxt.Text = Properties.Resources.TestInProgress;

				int statusCode = await Internet.GetStatusCodeAsync(url);
				if (statusCode < 400)
				{
					results.Add(StatusCodes.Success);

					// Update icon and text
					StatusIconTxt.Text = "\uF299"; // Update the icon
					StatusIconTxt.Foreground = new SolidColorBrush(Global.GetColorFromResource("Green"));
					StatusTxt.Text = Properties.Resources.WebsiteAvailable; // Update the text
					StatusIconTxt.Visibility = Visibility.Visible;
					Spinner.Visibility = Visibility.Collapsed;

					if (isFirst)
					{
						IconTxt.Text = "\uF299"; // Update the icon
						IconTxt.Foreground = new SolidColorBrush(Global.GetColorFromResource("Green"));
					}

					// Update details section
					int time = 0;
					DispatcherTimer dispatcherTimer = new() { Interval = TimeSpan.FromMilliseconds(1) };
					dispatcherTimer.Tick += (o, e) => time++;
					dispatcherTimer.Start();


					dispatcherTimer.Stop();
					DetailsStatusTxt.Text = statusCode.ToString();

					string message = await Internet.GetStatusDescriptionAsync(url);
					DetailsMessageTxt.Text = message;

					DetailsTimeTxt.Text = $"{time}ms"; // Update the time
					DetailsSiteNameTxt.Text = string.Format(Properties.Resources.OfWebsite, url);

					Global.History.DownDetectorHistory.Add(new(Time.DateTimeToUnixTime(DateTime.Now), StatusIconTxt.Text, statusCode, Properties.Resources.Available, url));
					return new(statusCode, time, message);
				}
				else
				{
					results.Add(StatusCodes.ClientError);

					// Update icon and text
					StatusIconTxt.Text = "\uF36E"; // Update the icon
					StatusIconTxt.Foreground = new SolidColorBrush(Global.GetColorFromResource("Red"));
					StatusTxt.Text = Properties.Resources.WebsiteDown; // Update the text
					StatusIconTxt.Visibility = Visibility.Visible;
					Spinner.Visibility = Visibility.Collapsed;

					if (isFirst)
					{
						IconTxt.Text = "\uF36E"; // Update the icon
						IconTxt.Foreground = new SolidColorBrush(Global.GetColorFromResource("Red"));
					}

					// Update details section
					// Update details section
					int time = 0;
					DispatcherTimer dispatcherTimer = new() { Interval = TimeSpan.FromMilliseconds(1) };
					dispatcherTimer.Tick += (o, e) => time++;
					dispatcherTimer.Start();

					dispatcherTimer.Stop();
					DetailsStatusTxt.Text = statusCode.ToString();

					string message = await Internet.GetStatusDescriptionAsync(url);
					DetailsMessageTxt.Text = message;

					DetailsTimeTxt.Text = $"{time}ms"; // Update the time
					DetailsSiteNameTxt.Text = string.Format(Properties.Resources.OfWebsite, url);

					Global.History.DownDetectorHistory.Add(new(Time.DateTimeToUnixTime(DateTime.Now), StatusIconTxt.Text, statusCode, Properties.Resources.Down, url));
					return new(statusCode, time, message);
				}
			}
			catch (Exception ex)
			{
				results.Add(StatusCodes.ClientError);

				// Update icon and text
				StatusIconTxt.Text = "\uF36E"; // Update the icon
				StatusIconTxt.Foreground = new SolidColorBrush(Global.GetColorFromResource("Red"));
				StatusTxt.Text = Properties.Resources.WebsiteDown; // Update the text
				StatusIconTxt.Visibility = Visibility.Visible;
				Spinner.Visibility = Visibility.Collapsed;

				if (isFirst)
				{
					IconTxt.Text = "\uF36E"; // Update the icon
					IconTxt.Foreground = new SolidColorBrush(Global.GetColorFromResource("Red"));
				}

				DetailsMessageTxt.Text = ex.Message;

				DetailsTimeTxt.Text = $"0ms"; // Update the time
				DetailsSiteNameTxt.Text = string.Format(Properties.Resources.OfWebsite, url);

				Global.History.DownDetectorHistory.Add(new(Time.DateTimeToUnixTime(DateTime.Now), StatusIconTxt.Text, 400, Properties.Resources.Down, url));

				return new(0, 0, ex.Message);
			}			
		}

		private void SetTestIconResult(List<StatusCodes> StatusCodess)
		{
			// Init counters
			int success = 0;
			int failed = 0;

			for (int i = 0; i < StatusCodess.Count; i++) // For each test result
			{
				switch (StatusCodess[i])
				{
					case StatusCodes.Success:
						success++;
						break;
					default:
						failed++;
						break;
				}
			}

			// If all tests were sucessful
			if (success > 0 && failed == 0)
			{
				StatusIconTxt.Text = "\uF299"; // Update the icon
				StatusIconTxt.Foreground = new SolidColorBrush(Global.GetColorFromResource("Green"));

				StatusTxt.Text = Properties.Resources.AllSuccess; // Update the text
			}
			else if (success == 0 && failed > 0) // If all tests failed
			{
				StatusIconTxt.Text = "\uF36E"; // Update the icon
				StatusIconTxt.Foreground = new SolidColorBrush(Global.GetColorFromResource("Red"));

				StatusTxt.Text = Properties.Resources.AllFailed; // Update the text
			}
			else // If some tests passed and others failed
			{
				StatusIconTxt.Text = "\uF882"; // Update the icon
				StatusIconTxt.Foreground = new SolidColorBrush(Global.GetColorFromResource("Orange"));

				StatusTxt.Text = Properties.Resources.TestSucessAndFailed; // Update the text
			}
		}

		private void BrowserBtn_Click(object sender, RoutedEventArgs e)
		{
			Process.Start("explorer.exe", WebsiteTxt.Text);
		}

		private void InfoBtn_Click(object sender, RoutedEventArgs e)
		{
			if (CurrentResult is not null)
			{
				DetailsStatusTxt.Text = CurrentResult.Code.ToString(); // Set the text
				DetailsTimeTxt.Text = $"{CurrentResult.Time}ms"; // Set the text
				DetailsMessageTxt.Text = CurrentResult.Message; // Set the text 
				DetailsSiteNameTxt.Text = string.Format(Properties.Resources.OfWebsite, WebsiteTxt.Text); // Set the text
			}
		}

		private void AddBtn_Click(object sender, RoutedEventArgs e)
		{
			if (!WebsiteTxt.Text.StartsWith("http"))
			{
				WebsiteTxt.Text = Global.Settings.UseHttps ? "https://" : "http://" + WebsiteTxt.Text;
			}
			DownDetectorItemDisplayer.Children.Add(new DownDetectorItem(DownDetectorItemDisplayer, WebsiteTxt.Text, CurrentResult));

			TotalWebsites = DownDetectorItemDisplayer.Children.Count + ((!string.IsNullOrEmpty(WebsiteTxt.Text)) ? 1 : 0);
			TestBtn.Content = $"{Properties.Resources.Test} ({TotalWebsites})";
		}

		private async void TestSiteBtn_Click(object sender, RoutedEventArgs e)
		{
			// Check if the URL is valid
			if (!WebsiteTxt.Text.StartsWith("http"))
			{
				WebsiteTxt.Text = Global.Settings.UseHttps ? "https://" : "http://" + WebsiteTxt.Text;
			}

			TotalWebsites = DownDetectorItemDisplayer.Children.Count + ((!string.IsNullOrEmpty(WebsiteTxt.Text)) ? 1 : 0);
			TestBtn.Content = $"{Properties.Resources.Test} ({TotalWebsites})";

			// Test the current website
			CurrentResult = await LaunchTest(WebsiteTxt.Text, true);
		}

		private void IntervalTxt_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			Regex regex = new("[^0-9]+");
			e.Handled = regex.IsMatch(e.Text);
		}

		int secondsRemaining = 0;
		int secondsRemainingFixed = 0;
		int timeCounter = 0;
		bool scheduledStarted = false;
		DispatcherTimer timer = new() { Interval = TimeSpan.FromSeconds(1) }; // Create a new timer

		private void ScheduledTestLaunchBtn_Click(object sender, RoutedEventArgs e)
		{
			secondsRemaining = int.Parse(IntervalTxt.Text); // Get the seconds
			secondsRemainingFixed = int.Parse(IntervalTxt.Text); // Get the seconds			

			if (!scheduledStarted)
			{
				scheduledStarted = true;
				ScheduledTestLaunchBtn.Content = Properties.Resources.StopScheduledTests;
				TimeIntervalTxt.Visibility = Visibility.Visible;

				timer.Tick += async (o, e) =>
				{
					timeCounter++;
					if (timeCounter == secondsRemainingFixed)
					{
						results.Clear();
						_ = await LaunchTest(WebsiteTxt.Text);
						for (int i = 0; i < DownDetectorItemDisplayer.Children.Count; i++)
						{
							if (DownDetectorItemDisplayer.Children[i] is DownDetectorItem item)
							{
								item.WebsiteTxt.Foreground = new SolidColorBrush(Global.GetColorFromResource("Foreground1"));
								item.DownDetectorTestResult = await LaunchTest(item.WebsiteTxt.Text);
								item.WebsiteTxt.Foreground = new SolidColorBrush(Global.GetColorFromResource("DarkGray"));
							}
						}
						SetTestIconResult(results);
					}
					if (secondsRemaining > 0)
					{
						secondsRemaining--;
					}
					else
					{
						secondsRemaining = secondsRemainingFixed;
						timeCounter = 0;
					}
					TimeIntervalTxt.Text = string.Format(Properties.Resources.ScheduledTestInterval, secondsRemaining); // Set the time interval text
				};
				timer.Start();
			}
			else
			{
				scheduledStarted = false;
				timer.Stop();
				ScheduledTestLaunchBtn.Content = Properties.Resources.LaunchScheduledTest;
				TimeIntervalTxt.Visibility = Visibility.Collapsed;
			}
		}

		private void TextBlock_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			Clipboard.SetText(((TextBlock)sender).Text);
		}
	}
}
