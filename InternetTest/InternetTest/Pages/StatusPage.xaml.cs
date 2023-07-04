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
using PeyrSharp.Core.Converters;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace InternetTest.Pages;
/// <summary>
/// Interaction logic for StatusPage.xaml
/// </summary>
public partial class StatusPage : Page
{
	bool codeInjected = !Global.Settings.UseSynethia;
	public StatusPage()
	{
		InitializeComponent();
		InitUI(); // Load the UI
		Loaded += (o, e) => InjectSynethiaCode();
	}

	private void InjectSynethiaCode()
	{
		if (codeInjected) return;
		codeInjected = true;
		foreach (Button b in Global.FindVisualChildren<Button>(this))
		{
			b.Click += (sender, e) =>
			{
				Global.SynethiaConfig.StatusPageInfo.InteractionCount++;
			};
		}

		// For each TextBox of the page
		foreach (TextBox textBox in Global.FindVisualChildren<TextBox>(this))
		{
			textBox.GotFocus += (o, e) =>
			{
				Global.SynethiaConfig.StatusPageInfo.InteractionCount++;
			};
		}

		// For each CheckBox/RadioButton of the page
		foreach (CheckBox checkBox in Global.FindVisualChildren<CheckBox>(this))
		{
			checkBox.Checked += (o, e) =>
			{
				Global.SynethiaConfig.StatusPageInfo.InteractionCount++;
			};
			checkBox.Unchecked += (o, e) =>
			{
				Global.SynethiaConfig.StatusPageInfo.InteractionCount++;
			};
		}

		foreach (RadioButton radioButton in Global.FindVisualChildren<RadioButton>(this))
		{
			radioButton.Checked += (o, e) =>
			{
				Global.SynethiaConfig.StatusPageInfo.InteractionCount++;
			};
			radioButton.Unchecked += (o, e) =>
			{
				Global.SynethiaConfig.StatusPageInfo.InteractionCount++;
			};
		}
	}

	private void InitUI()
	{
		TitleTxt.Text = $"{Properties.Resources.WebUtilities} > {Properties.Resources.Status}"; // Set the title

		if (!Global.Settings.TestOnStart) return;
		LaunchTest(Global.Settings.TestSite ?? "https://leocorporation"); // Launch the test
	}

	private async void LaunchTest(string customSite)
	{
		try
		{
			// Show the waiting screen
			StatusIconTxt.Text = "\uF2DE";
			StatusIconTxt.Foreground = new SolidColorBrush(Global.GetColorFromResource("Gray"));
			StatusTxt.Text = Properties.Resources.TestInProgress;
			TestBtn.IsEnabled = false;
			SpeedTestBtn.IsEnabled = false;

			// Launch the test
			// Part 1: Get the status code and start timer
			int time = 0;
			DispatcherTimer dispatcherTimer = new() { Interval = TimeSpan.FromMilliseconds(1) };
			dispatcherTimer.Tick += (o, e) => time++;
			dispatcherTimer.Start();

			HttpResponseMessage response = await new HttpClient().GetAsync(customSite);

			int code = (int)response.StatusCode;
			string message = response.ReasonPhrase;
			InfoTxt.Text = response.Headers.ToString();

			dispatcherTimer.Stop();

			DetailsStatusTxt.Text = code.ToString();
			DetailsMessageTxt.Text = message;

			// Part 2: Display time
			DetailsTimeTxt.Text = $"{time}ms";

			// Part 3: Display the result
			if (code != 400)
			{
				StatusIconTxt.Text = "\uF299";
				StatusIconTxt.Foreground = new SolidColorBrush(Global.GetColorFromResource("Green"));
				StatusTxt.Text = Properties.Resources.Connected;
			}
			else
			{
				StatusIconTxt.Text = "\uF36E";
				StatusIconTxt.Foreground = new SolidColorBrush(Global.GetColorFromResource("Red"));
				StatusTxt.Text = Properties.Resources.NotConnected;
			}

			Global.History.StatusHistory.Add(new StatusHistory(Time.DateTimeToUnixTime(DateTime.Now), StatusIconTxt.Text, true));
		}
		catch (HttpRequestException)
		{
			StatusIconTxt.Text = "\uF36E";
			StatusIconTxt.Foreground = new SolidColorBrush(Global.GetColorFromResource("Red"));
			StatusTxt.Text = Properties.Resources.NotConnected;
			DetailsStatusTxt.Text = "N/A";
			DetailsMessageTxt.Text = Properties.Resources.Error;
			DetailsTimeTxt.Text = "0ms";
			Global.History.StatusHistory.Add(new StatusHistory(Time.DateTimeToUnixTime(DateTime.Now), StatusIconTxt.Text, false));

		}

		TestBtn.IsEnabled = true;
		SpeedTestBtn.IsEnabled = true;
	}

	internal void TestBtn_Click(object sender, RoutedEventArgs e)
	{
		LaunchTest(Global.Settings.TestSite ?? "https://leocorporation.dev");
		// Increment the interaction count of the ActionInfo in Global.SynethiaConfig
		Global.SynethiaConfig.ActionInfos.First(a => a.Action == Enums.AppActions.Test).UsageCount++;
	}

	private void BrowserBtn_Click(object sender, RoutedEventArgs e)
	{
		Process.Start("explorer.exe", Global.Settings.TestSite ?? "https://leocorporation.dev");
	}

	private void TextBlock_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
	{
		Clipboard.SetText(((TextBlock)sender).Text);
	}

	private void AdvancedBtn_Click(object sender, RoutedEventArgs e)
	{
		AdvancedOptions.Visibility = AdvancedOptions.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
		AdvancedBtn.Content = AdvancedOptions.Visibility == Visibility.Visible ? "\uF36A" : "\uF4A4";
	}

	private async void SpeedTestBtn_Click(object sender, RoutedEventArgs e)
	{
		try
		{
			// Show the waiting screen
			StatusIconTxt.Text = "\uF2DE";
			StatusIconTxt.Foreground = new SolidColorBrush(Global.GetColorFromResource("Gray"));
			StatusTxt.Text = Properties.Resources.TestInProgress;
			TestBtn.IsEnabled = false;
			SpeedTestBtn.IsEnabled = false;

			// Test
			string targetUrl = "http://speedtest.tele2.net/10MB.zip";

			long fileSize = await DownloadFile(targetUrl);
			double speedMbps = fileSize / 1000000.0;

			SpeedTxt.Text = $"{speedMbps} MB/s";

			// Case of sucess
			StatusIconTxt.Text = "\uF299";
			StatusIconTxt.Foreground = new SolidColorBrush(Global.GetColorFromResource("Green"));
			StatusTxt.Text = Properties.Resources.SpeedTestSucess;
		}
		catch
		{
			StatusIconTxt.Text = "\uF36E";
			StatusIconTxt.Foreground = new SolidColorBrush(Global.GetColorFromResource("Red"));
			StatusTxt.Text = Properties.Resources.SpeedTestFailed;
		}

		TestBtn.IsEnabled = true;
		SpeedTestBtn.IsEnabled = true;
	}

	static async Task<long> DownloadFile(string url)
	{
		Stopwatch stopwatch = Stopwatch.StartNew();

		using (HttpClient client = new HttpClient())
		{
			HttpResponseMessage response = await client.GetAsync(url);

			if (response.IsSuccessStatusCode)
			{
				byte[] data = await response.Content.ReadAsByteArrayAsync();
				stopwatch.Stop();
				return data.Length;
			}
			else
			{				
				return 0;
			}
		}
	}
}
