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
using InternetTest.UserControls;
using PeyrSharp.Core;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;

namespace InternetTest.Pages;
/// <summary>
/// Interaction logic for HomePage.xaml
/// </summary>
public partial class HomePage : Page
{
	public HomePage()
	{
		InitializeComponent();
		InitUI();
	}

	internal async void InitUI()
	{
		// Load "Get started" section
		List<AppPages> relevantPages = Enumerable.Empty<AppPages>().ToList();
		if (Global.SynethiaConfig is not null)
		{
			relevantPages = Global.GetMostRelevantPages(Global.SynethiaConfig);
		}
		else
		{
			relevantPages = Global.DefaultRelevantPages;
		}

		for (int i = 0; i < 5; i++)
		{
			GetStartedPanel.Children.Add(new PageCard(relevantPages[i]));
		}
		relevantPages.RemoveRange(0, 5); // Remove already added pages; the least releavnt remains

		// Load "Discover" section
		for (int i = 0; i < relevantPages.Count; i++)
		{
			DiscoverPanel.Children.Add(new PageCard(relevantPages[i]));
		}

		// Load "Status" section
		LoadStatusCard();

		// Load "Network" section
		LoadNetworkCard();

		// Load "My IP" section
		ip = (await Global.GetIPInfoAsync("")).Query ?? "";
	}

	private async void RefreshStatusBtn_Click(object sender, System.Windows.RoutedEventArgs e)
	{
		StatusTxt.Text = Properties.Resources.Checking;
		LoadStatusCard();
	}

	internal async void LoadStatusCard()
	{
		bool connected = await Internet.IsAvailableAsync(Global.Settings.TestSite); // Check if Internet is available
		StatusTxt.Text = connected ? Properties.Resources.ConnectedS : Properties.Resources.NotConnectedS; // Set text
		StatusIconTxt.Text = connected ? "\uF299" : "\uF36E";
		StatusIconTxt.Foreground = connected ? new SolidColorBrush(Global.GetColorFromResource("Green")) : new SolidColorBrush(Global.GetColorFromResource("Red"));
	}

	internal void LoadNetworkCard()
	{
		string ssid = Global.GetCurrentWifiSSID();
		if (ssid == null)
		{
			NetworkTxt.Text = Properties.Resources.NotConnectedS;
			NetworkIconTxt.Text = "\uFB71";
			return;
		}
		NetworkTxt.Text = ssid;
		NetworkIconTxt.Text = "\uF8C5";
	}

	private void RefreshNetworkBtn_Click(object sender, System.Windows.RoutedEventArgs e)
	{
		LoadNetworkCard();
	}

	string ip = "";
	private async void RefreshMyIpBtn_Click(object sender, System.Windows.RoutedEventArgs e)
	{
		ip = (await Global.GetIPInfoAsync("")).Query ?? "";
	}

	private void MyIpBorder_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
	{
		MyIpTxt.Text = ip;
	}

	private void MyIpBorder_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
	{
		MyIpTxt.Text = Properties.Resources.HoverToReveal;
	}
}
