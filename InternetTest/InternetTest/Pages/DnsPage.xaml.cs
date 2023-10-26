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

using DnsClient;
using InternetTest.Classes;
using InternetTest.UserControls;
using System.DirectoryServices.ActiveDirectory;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Whois;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Input;

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

	private async void GetDnsInfo(string website)
	{
		try
		{
			RecordDisplayer.Children.Clear();
			CreationTxt.Text = "";
			ExpTxt.Text = "";
			StatusTxt.Text = "";
			RegistrantTxt.Text = "";
			// Get IP
			IPHostEntry host = Dns.GetHostEntry(website);
			IPAddress ip = host.AddressList[0];
			UrlTxt.Text = website;
			IpTxt.Text = ip.ToString();
		}
		catch { }

		List<string> availableTypes = new();
		FiltersDisplayer.Children.Clear();
		try
		{
			// Get DNS records
			var lookup = new LookupClient();
			var result = lookup.QueryAsync(website, QueryType.ANY).Result;
			foreach (var record in result.AllRecords)
			{
				RecordDisplayer.Children.Add(new DnsRecordItem(record.RecordType.ToString(), record.ToString()));
				if (!availableTypes.Contains(record.RecordType.ToString()))
					availableTypes.Add(record.RecordType.ToString());
			}

			availableTypes.Sort();
			availableTypes.Insert(0, "ANY");
			for (int i = 0; i < availableTypes.Count; i++)
			{
				FiltersDisplayer.Children.Add(CreateFilterButton(availableTypes[i]));
			}
		}
		catch { }

		try
		{
			// Get WHOIS
			var whois = new WhoisLookup();
			var response = await whois.LookupAsync(website);
			CreationTxt.Text = response.Registered.ToString();
			ExpTxt.Text = response.Expiration.ToString();
			StatusTxt.Text = string.Join("\n", response.DomainStatus);
			string regInfo = "";
			foreach (var prop in response.Registrant.GetType().GetProperties())
			{
				var val = prop.GetValue(response.Registrant, null);
				if (val is null || prop.Name == "Address") continue;
				regInfo += $"{prop.Name} - {val}\n";
			}
			RegistrantTxt.Text = regInfo;
		}
		catch { }
	}

	private RadioButton CreateFilterButton(string recordType)
	{
		RadioButton filterBtn = new()
		{
			Margin = new(5),
			Padding = new(5),
			BorderThickness = new(0),
			Content = recordType,
			Style = (Style)FindResource("CheckButton"),
			Foreground = new SolidColorBrush { Color = Global.GetColorFromResource("AccentColor") },
			Background = new SolidColorBrush { Color = Global.GetColorFromResource("LightAccentColor") },
			Cursor = Cursors.Hand,
			FontWeight = FontWeights.Bold,
			GroupName = "Filters",
			IsChecked = recordType == "ANY"
		};
		filterBtn.Click += (o, e) => { filterBtn.IsChecked = true; Filter(recordType); };
		return filterBtn;
	}

	private void Filter(string query)
	{
		foreach (DnsRecordItem item in RecordDisplayer.Children)
		{
			if (query == "ANY")
			{
				item.Visibility = Visibility.Visible;
				continue;
			}
			item.Visibility = query == item.Type ? Visibility.Visible : Visibility.Collapsed;
		}
	}

	private void DismissBtn_Click(object sender, RoutedEventArgs e)
	{
		SiteTxt.Text = string.Empty;
	}

	internal void GetDnsInfoBtn_Click(object sender, RoutedEventArgs e)
	{
		GetDnsInfo(SiteTxt.Text);

		// Increment the interaction count of the ActionInfo in Global.SynethiaConfig
		Global.SynethiaConfig.ActionInfos.First(a => a.Action == Enums.AppActions.GetDnsInfo).UsageCount++;
	}
}
