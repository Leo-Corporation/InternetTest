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
using LeoCorpLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Policy;
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
using System.Windows.Threading;

namespace InternetTest.Pages
{
	/// <summary>
	/// Interaction logic for DownDetectorPage.xaml
	/// </summary>
	public partial class DownDetectorPage : Page
	{
		DownDetectorTestResult CurrentResult { get; set; }
		internal int TotalWebsites { get; set; } = 1;
		public DownDetectorPage()
		{
			InitializeComponent();
			InitUI(); // Load the UI
		}

		private void InitUI()
		{
			TitleTxt.Text = $"{Properties.Resources.WebUtilities} > {Properties.Resources.DownDetector}"; // Set the title of the page
		}

		private async void TestBtn_Click(object sender, RoutedEventArgs e)
		{
			// Check if the URL is valid
			if (!WebsiteTxt.Text.StartsWith("http"))
			{
				WebsiteTxt.Text = "https://" + WebsiteTxt.Text;
			}

			TotalWebsites = DownDetectorItemDisplayer.Children.Count + ((!string.IsNullOrEmpty(WebsiteTxt.Text)) ? 1 : 0);
			TestBtn.Content = $"{Properties.Resources.Test} ({TotalWebsites})";

			// Test the current website
			CurrentResult = await LaunchTest(WebsiteTxt.Text);

			// If there are any ohther websites, test them
			for (int i = 0; i < DownDetectorItemDisplayer.Children.Count; i++)
			{
				if (DownDetectorItemDisplayer.Children[i] is DownDetectorItem item)
				{
					item.WebsiteTxt.Foreground = new SolidColorBrush(Global.GetColorFromResource("Foreground1"));
					item.DownDetectorTestResult = await LaunchTest(item.WebsiteTxt.Text);
					item.WebsiteTxt.Foreground = new SolidColorBrush(Global.GetColorFromResource("DarkGray"));
				}
			}
		}

		internal async Task<DownDetectorTestResult> LaunchTest(string url)
		{
			if (!url.StartsWith("http"))
			{
				url = "https://" + url;
			}

			if (!Global.IsUrlValid(url)) return new(0, 0, "Invalid URL");

			// Show the "Waiting" screen
			StatusIconTxt.Text = "\uF2DE";
			StatusIconTxt.Foreground = new SolidColorBrush(Global.GetColorFromResource("Gray"));
			StatusTxt.Text = Properties.Resources.TestInProgress;

			if (await NetworkConnection.IsAvailableAsync(url))
			{
				// Update icon and text
				StatusIconTxt.Text = "\uF299"; // Update the icon
				StatusIconTxt.Foreground = new SolidColorBrush(Global.GetColorFromResource("Green"));
				StatusTxt.Text = Properties.Resources.WebsiteAvailable; // Update the text

				// Update details section
				int time = 0;
				DispatcherTimer dispatcherTimer = new() { Interval = TimeSpan.FromMilliseconds(1) };
				dispatcherTimer.Tick += (o, e) => time++;
				dispatcherTimer.Start();

				int statusCode = await NetworkConnection.GetWebPageStatusCodeAsync(url);

				dispatcherTimer.Stop();
				DetailsStatusTxt.Text = statusCode.ToString();

				string message = await NetworkConnection.GetWebPageStatusDescriptionAsync(url);
				DetailsMessageTxt.Text = message;

				DetailsTimeTxt.Text = $"{time}ms"; // Update the time
				return new(statusCode, time, message);
			}
			else
			{
				// Update icon and text
				StatusIconTxt.Text = "\uF2A4"; // Update the icon
				StatusIconTxt.Foreground = new SolidColorBrush(Global.GetColorFromResource("Red"));
				StatusTxt.Text = Properties.Resources.WebsiteDown; // Update the text

				// Update details section
				// Update details section
				int time = 0;
				DispatcherTimer dispatcherTimer = new() { Interval = TimeSpan.FromMilliseconds(1) };
				dispatcherTimer.Tick += (o, e) => time++;
				dispatcherTimer.Start();

				int statusCode = await NetworkConnection.GetWebPageStatusCodeAsync(url);

				dispatcherTimer.Stop();
				DetailsStatusTxt.Text = statusCode.ToString();

				string message = await NetworkConnection.GetWebPageStatusDescriptionAsync(url);
				DetailsMessageTxt.Text = message;

				DetailsTimeTxt.Text = $"{time}ms"; // Update the time

				return new(statusCode, time, message);
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
			}
		}

		private void AddBtn_Click(object sender, RoutedEventArgs e)
		{
			if (!WebsiteTxt.Text.StartsWith("http"))
			{
				WebsiteTxt.Text = "https://" + WebsiteTxt.Text;
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
				WebsiteTxt.Text = "https://" + WebsiteTxt.Text;
			}

			TotalWebsites = DownDetectorItemDisplayer.Children.Count + ((!string.IsNullOrEmpty(WebsiteTxt.Text)) ? 1 : 0);
			TestBtn.Content = $"{Properties.Resources.Test} ({TotalWebsites})";

			// Test the current website
			CurrentResult = await LaunchTest(WebsiteTxt.Text);
		}
	}
}
