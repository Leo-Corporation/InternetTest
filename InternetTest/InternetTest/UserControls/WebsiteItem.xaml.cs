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
using PeyrSharp.Core;
using PeyrSharp.Core.Converters;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace InternetTest.UserControls;

public partial class WebsiteItem : UserControl
{
	public string URL { get; }

	public WebsiteItem(string url)
	{
		InitializeComponent();
		URL = url;

		InitUI();
	}

	private void InitUI()
	{
		WebsiteNameTxt.Text = URL;
	}

	private void TestBtn_Click(object sender, RoutedEventArgs e)
	{
		LaunchTestAsync();
	}

	private void ExpanderBtn_Click(object sender, RoutedEventArgs e)
	{
		CollapseGrid.Visibility = CollapseGrid.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
		ExpanderBtn.Content = CollapseGrid.Visibility != Visibility.Visible ? "\uF2A4" : "\uF2B7";
	}

	internal async void LaunchTestAsync()
	{
		try
		{
			// Loading UI
			StatusIcon.Text = "\uF2DE";
			StatusIcon.Foreground = Global.GetBrushFromResource("Accent");

			// Chronometer
			int time = 0;
			DispatcherTimer dispatcherTimer = new() { Interval = TimeSpan.FromMilliseconds(1) };
			dispatcherTimer.Tick += (o, e) => time++;

			dispatcherTimer.Start();
			var statusInfo = await Internet.GetStatusInfoAsync(URL); // Makes a request to the specified website and saves the status code and message
			dispatcherTimer.Stop();

			// Load Information
			TimeTxt.Text = $"{time}ms";
			StatusCodeTxt.Text = statusInfo.StatusCode.ToString();
			StatusCodeBisTxt.Text = statusInfo.StatusCode.ToString();
			StatusMsgTxt.Text = statusInfo.StatusDescription;
			StatusIcon.Text = statusInfo.StatusCode switch
			{
				>= 400 => "\uF36E",
				>= 300 => "\uF4AB",
				_ => "\uF299",
			};
			StatusIcon.Foreground = statusInfo.StatusCode switch
			{
				>= 400 => Global.GetBrushFromResource("Red"),
				>= 300 => Global.GetBrushFromResource("Accent"),
				_ => Global.GetBrushFromResource("Green"),
			};

			StatusSection.Visibility = Visibility.Visible;

			// Save the test to the history
			Global.History.DownDetectorHistory.Add(new(Time.DateTimeToUnixTime(DateTime.Now), StatusIcon.Text, statusInfo.StatusCode, statusInfo.StatusDescription, URL));
		}
		catch (Exception ex)
		{
			StatusSection.Visibility = Visibility.Visible;
			StatusIcon.Text = "\uF36E";
			StatusIcon.Foreground = Global.GetBrushFromResource("Red");
			StatusCodeTxt.Text = "4xx";
			StatusCodeBisTxt.Text = "4xx";
			StatusMsgTxt.Text = ex.Message;
		}
	}

	private void DeleteBtn_Click(object sender, RoutedEventArgs e)
	{
#pragma warning disable CS8602 // DownDetectorPage cannot be null in this context
		Global.DownDetectorPage.WebsiteDisplayer.Children.Remove(this);
		Global.DownDetectorPage.Websites.Remove(URL);
		Global.DownDetectorPage.Placeholder.Visibility = Global.DownDetectorPage.WebsiteDisplayer.Children.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
#pragma warning restore CS8602

		Global.Settings.DownDetectorWebsites = Global.DownDetectorPage.Websites;
		SettingsManager.Save();
	}
}
