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
using System;
using System.Linq;
using System.Net.NetworkInformation;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace InternetTest.Pages;
/// <summary>
/// Interaction logic for PingPage.xaml
/// </summary>
public partial class PingPage : Page
{
	bool codeInjected = false;
	public PingPage()
	{
		InitializeComponent();
		InitUI(); // Load the UI
		InjectSynethiaCode();
	}

	private void InitUI()
	{
		TitleTxt.Text = $"{Properties.Resources.Commands} > {Properties.Resources.Ping}";
	}

	private void InjectSynethiaCode()
	{
		if (codeInjected) return;
		codeInjected = true;
		foreach (Button b in Global.FindVisualChildren<Button>(this))
		{
			b.Click += (sender, e) =>
			{
				Global.SynethiaConfig.PingPageInfo.InteractionCount++;
			};
		}

		// For each TextBox of the page
		foreach (TextBox textBox in Global.FindVisualChildren<TextBox>(this))
		{
			textBox.GotFocus += (o, e) =>
			{
				Global.SynethiaConfig.PingPageInfo.InteractionCount++;
			};
		}

		// For each CheckBox/RadioButton of the page
		foreach (CheckBox checkBox in Global.FindVisualChildren<CheckBox>(this))
		{
			checkBox.Checked += (o, e) =>
			{
				Global.SynethiaConfig.PingPageInfo.InteractionCount++;
			};
			checkBox.Unchecked += (o, e) =>
			{
				Global.SynethiaConfig.PingPageInfo.InteractionCount++;
			};
		}

		foreach (RadioButton radioButton in Global.FindVisualChildren<RadioButton>(this))
		{
			radioButton.Checked += (o, e) =>
			{
				Global.SynethiaConfig.PingPageInfo.InteractionCount++;
			};
			radioButton.Unchecked += (o, e) =>
			{
				Global.SynethiaConfig.PingPageInfo.InteractionCount++;
			};
		}
	}

	internal void PingBtn_Click(object sender, RoutedEventArgs e)
	{
		IpTxt.Text = IpTxt.Text.Replace("https://", "").Replace("http://", "").TrimEnd('/'); // Remove the http:// or https://
		MakePing(IpTxt.Text); // Make a ping to the specified IP

		// Increment the interaction count of the ActionInfo in Global.SynethiaConfig
		Global.SynethiaConfig.ActionInfos.First(a => a.Action == Enums.AppActions.Ping).UsageCount++;
	}

	private async void MakePing(string address)
	{
		try
		{
			if (address is null or { Length: 0 } || string.IsNullOrWhiteSpace(address))
			{
				MessageBox.Show(Properties.Resources.EnterIP, Properties.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Information);
				return;
			}

			// Update UI
			StatusIconTxt.Text = "\uF2DE";
			StatusIconTxt.Foreground = new SolidColorBrush(Global.GetColorFromResource("Gray"));
			StatusTxt.Text = Properties.Resources.PingWait;
			PingBtn.IsEnabled = false;

			int sent = 0, received = 0;

			long[] times = new long[4]; // Create an array
			for (int i = 0; i < 4; i++)
			{
				var ping = await new Ping().SendPingAsync(address); // Send a ping
				sent++;
				times[i] = ping.RoundtripTime; // Get the time of the ping
				IPAddressTxt.Text = ping.Address.ToString(); // Get the address of the ping

				string nl = (i + 1 < 4) ? $"\n{i + 1}/4" : ""; // Add a new line if it's not the last ping
				if (ping.Status == IPStatus.Success)
				{
					received++;
					StatusIconTxt.Text = "\uF299";
					StatusIconTxt.Foreground = new SolidColorBrush(Global.GetColorFromResource("Green"));
					StatusTxt.Text = $"{Properties.Resources.PingSuccess}{nl}";
				}
				else
				{
					StatusIconTxt.Text = "\uF36E";
					StatusIconTxt.Foreground = new SolidColorBrush(Global.GetColorFromResource("Red"));
					StatusTxt.Text = $"{Properties.Resources.PingFail}{nl}";
					IPAddressTxt.Text = ping.Status.ToString();
				}
			}

			AverageTimeTxt.Text = $"{times.Average()}ms"; // Get the average of the times
			MinTimeTxt.Text = $"{times.Min()}ms"; // Get the minimum of the times
			MaxTimeTxt.Text = $"{times.Max()}ms"; // Get the maximum of the times

			SentTxt.Text = sent.ToString(); // Get the number of sent pings
			ReceivedTxt.Text = received.ToString(); // Get the number of received pings
			LostTxt.Text = (sent - received).ToString(); // Get the number of lost pings
		}
		catch (Exception ex)
		{
			StatusIconTxt.Text = "\uF4AB";
			StatusIconTxt.Foreground = new SolidColorBrush(Global.GetColorFromResource("Gray"));
			StatusTxt.Text = Properties.Resources.PingStatus;
			MessageBox.Show(ex.Message, Properties.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
		}
		PingBtn.IsEnabled = true;
	}
}
