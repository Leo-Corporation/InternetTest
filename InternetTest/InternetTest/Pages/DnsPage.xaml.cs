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
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;

namespace InternetTest.Pages;
/// <summary>
/// Interaction logic for DnsPage.xaml
/// </summary>
public partial class DnsPage : Page
{
	bool codeInjected = !Global.Settings.UseSynethia;

	public DnsPage()
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
				Global.SynethiaConfig.DnsPageInfo.InteractionCount++;
			};
		}

		// For each TextBox of the page
		foreach (TextBox textBox in Global.FindVisualChildren<TextBox>(this))
		{
			textBox.GotFocus += (o, e) =>
			{
				Global.SynethiaConfig.DnsPageInfo.InteractionCount++;
			};
		}

		// For each CheckBox/RadioButton of the page
		foreach (CheckBox checkBox in Global.FindVisualChildren<CheckBox>(this))
		{
			checkBox.Checked += (o, e) =>
			{
				Global.SynethiaConfig.DnsPageInfo.InteractionCount++;
			};
			checkBox.Unchecked += (o, e) =>
			{
				Global.SynethiaConfig.DnsPageInfo.InteractionCount++;
			};
		}

		foreach (RadioButton radioButton in Global.FindVisualChildren<RadioButton>(this))
		{
			radioButton.Checked += (o, e) =>
			{
				Global.SynethiaConfig.DnsPageInfo.InteractionCount++;
			};
			radioButton.Unchecked += (o, e) =>
			{
				Global.SynethiaConfig.DnsPageInfo.InteractionCount++;
			};
		}
	}

	private void InitUI()
	{
		TitleTxt.Text = $"{Properties.Resources.WebUtilities} > {Properties.Resources.DNSTool}"; // Set the title
	}

	private void GetDnsInfo(string website)
	{
		IPHostEntry host = Dns.GetHostEntry(website);
		IPAddress ip = host.AddressList[0];
		UrlTxt.Text = website;
		IpTxt.Text = ip.ToString();
	}

	private void DismissBtn_Click(object sender, RoutedEventArgs e)
	{
		SiteTxt.Text = string.Empty;
	}

	private void GetDnsInfoBtn_Click(object sender, RoutedEventArgs e)
	{
		GetDnsInfo(SiteTxt.Text);
		// Increment the interaction count of the ActionInfo in Global.SynethiaConfig
		Global.SynethiaConfig.ActionInfos.First(a => a.Action == Enums.AppActions.GetDnsInfo).UsageCount++;
	}
}
