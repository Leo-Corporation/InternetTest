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
using LeoCorpLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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

namespace InternetTest.Pages;
/// <summary>
/// Interaction logic for StatusPage.xaml
/// </summary>
public partial class StatusPage : Page
{
	public StatusPage()
	{
		InitializeComponent();
		InitUI(); // Load the UI
	}

	private void InitUI()
	{
		TitleTxt.Text = $"{Properties.Resources.WebUtilities} > {Properties.Resources.Status}"; // Set the title
	}
	
	private async void LaunchTest(string? customSite = null)
	{
		try
		{
			// Show the wating screen
			StatusIconTxt.Text = "\uF2DE";
			StatusIconTxt.Foreground = new SolidColorBrush(Global.GetColorFromResource("Gray"));
			StatusTxt.Text = Properties.Resources.TestInProgress;

			// Launch the test
			// Part 1: Get the status code and start timer
			int time = 0;
			DispatcherTimer dispatcherTimer = new() { Interval = TimeSpan.FromMilliseconds(1) };
			dispatcherTimer.Tick += (o, e) => time++;
			dispatcherTimer.Start();

			int code = await NetworkConnection.GetWebPageStatusCodeAsync(customSite ?? "https://bing.com");
			string message = await NetworkConnection.GetWebPageStatusDescriptionAsync(customSite ?? "https://bing.com");

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
		}
		catch (HttpRequestException)
		{
			StatusIconTxt.Text = "\uF36E";
			StatusIconTxt.Foreground = new SolidColorBrush(Global.GetColorFromResource("Red"));
			StatusTxt.Text = Properties.Resources.NotConnected;
			DetailsStatusTxt.Text = "N/A";
			DetailsMessageTxt.Text = Properties.Resources.Error;
			DetailsTimeTxt.Text = "0ms";
		}
	}

	private void TestBtn_Click(object sender, RoutedEventArgs e)
	{
		LaunchTest();
	}

	private void BrowserBtn_Click(object sender, RoutedEventArgs e)
	{

	}
}
