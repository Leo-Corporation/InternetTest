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
using System;
using System.Windows.Controls;

namespace InternetTest.UserControls;
/// <summary>
/// Interaction logic for ActionCard.xaml
/// </summary>
public partial class ActionCard : UserControl
{
	AppActions AppAction { get; init; }
	public ActionCard(AppActions appActions)
	{
		InitializeComponent();
		AppAction = appActions; // Set value

		InitUI(); // Load the UI
	}

	private void InitUI()
	{
		IconTxt.Text = Global.ActionsIcons[AppAction]; // Set text
		PageNameTxt.Text = Global.ActionsString[AppAction]; // Set text
	}
	public static event EventHandler<PageEventArgs> OnCardClick;

	private void Border_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
	{
		switch (AppAction)
		{
			case AppActions.Test:
				Global.StatusPage.TestBtn_Click(this, null);
				OnCardClick?.Invoke(this, new(AppPages.Status));
				break;
			case AppActions.DownDetectorRequest:
				Global.DownDetectorPage.TestBtn_Click(this, null);
				OnCardClick?.Invoke(this, new(AppPages.DownDetector));
				break;
			case AppActions.MyIP:
				Global.MyIpPage.GetMyIPBtn_Click(this, null);
				OnCardClick?.Invoke(this, new(AppPages.MyIP));
				break;
			case AppActions.LocateIP:
				Global.LocateIpPage.LocateIPBtn_Click(this, null);
				OnCardClick?.Invoke(this, new(AppPages.LocateIP));
				break;
			case AppActions.Ping:
				Global.PingPage.IpTxt.Text = Global.PingPage.IpTxt.Text == ""
					? Global.Settings.TestSite ?? "https://leocorporation.dev"
					: Global.PingPage.IpTxt.Text;
				Global.PingPage.PingBtn_Click(this, null);
				OnCardClick?.Invoke(this, new(AppPages.Ping));
				break;
			case AppActions.GetWiFiPasswords:
				Global.WiFiPasswordsPage.GetWiFiBtn_Click(this, null);
				OnCardClick?.Invoke(this, new(AppPages.WiFiPasswords));
				break;
			case AppActions.GetIPConfig:
				Global.IpConfigPage.RefreshBtn_Click(this, null);
				OnCardClick?.Invoke(this, new(AppPages.IPConfig));
				break;
			case AppActions.GetDnsInfo:
				Global.DnsPage.SiteTxt.Text = string.IsNullOrEmpty(Global.DnsPage.SiteTxt.Text) ? Global.Settings.TestSite.Replace("https://", "").Replace("http://","") : Global.DnsPage.SiteTxt.Text;
				Global.DnsPage.GetDnsInfoBtn_Click(this, null);
				OnCardClick?.Invoke(this, new(AppPages.DnsTool));
				break;
			default:
				break;
		}
	}
}
