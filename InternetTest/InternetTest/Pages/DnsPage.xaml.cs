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
using Microsoft.Win32;
using Synethia;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Whois;

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
		Loaded += (o, e) => SynethiaManager.InjectSynethiaCode(this, Global.SynethiaConfig.PagesInfo, 1, ref codeInjected);
	}

	private void InitUI()
	{
		TitleTxt.Text = $"{Properties.Resources.WebUtilities} > {Properties.Resources.DNSTool}"; // Set the title
		Placeholder.Visibility = Visibility.Visible;
		InformationHeader.Visibility = Visibility.Collapsed;
		DetailsGrid.Visibility = Visibility.Collapsed;
	}

	string csvFile = "";
	private async void GetDnsInfo(string website)
	{
		csvFile = "Type,Value\n";
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
			Placeholder.Visibility = Visibility.Collapsed;
			InformationHeader.Visibility = Visibility.Visible;
			DetailsGrid.Visibility = Visibility.Visible;
		}
		catch { }

		List<string> availableTypes = [];
		FiltersDisplayer.Children.Clear();
		try
		{
			// Get DNS records
			var lookup = new LookupClient();
			var result = lookup.QueryAsync(website, QueryType.ANY).Result;
			foreach (var record in result.AllRecords)
			{
				RecordDisplayer.Children.Add(new DnsRecordItem(record.RecordType.ToString(), record.ToString()));
				csvFile += $"{record.RecordType},{record}\n";
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

		SaveCsvBtn.Visibility = (FiltersDisplayer.Children.Count > 0) ? Visibility.Visible : Visibility.Collapsed;
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
			Foreground = Global.GetBrushFromResource("Accent"),
			Background = Global.GetBrushFromResource("LightAccent"),
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
		if (string.IsNullOrEmpty(SiteTxt.Text) || string.IsNullOrWhiteSpace(SiteTxt.Text))
		{
			MessageBox.Show(Properties.Resources.InvalidURLMsg, Properties.Resources.GetDnsInfo, MessageBoxButton.OK, MessageBoxImage.Error);
			return;
		}
		GetDnsInfo(SiteTxt.Text);

		// Increment the interaction count of the ActionInfo in Global.SynethiaConfig
		Global.SynethiaConfig.ActionsInfo.First(a => a.Name == "DNS.GetInfo").UsageCount++;
	}

	private void SiteTxt_KeyUp(object sender, KeyEventArgs e)
	{
		if (e.Key == Key.Enter)
		{
			GetDnsInfoBtn_Click(sender, e);
		}
	}

	private void SiteTxt_TextChanged(object sender, TextChangedEventArgs e)
	{
		DismissBtn.Visibility = SiteTxt.Text.Length > 0 ? Visibility.Visible : Visibility.Collapsed;
	}

	private void SaveCsvBtn_Click(object sender, RoutedEventArgs e)
	{
		SaveFileDialog saveFileDialog = new()
		{
			FileName = $"{SiteTxt.Text}.csv",
			Filter = "CSV|*.csv",
			Title = Properties.Resources.ExportToCSV
		}; // Create file dialog

		if (saveFileDialog.ShowDialog() ?? true)
		{
			using StreamWriter outputFile = new(saveFileDialog.FileName);
			outputFile.Write(csvFile);
		}
	}

	private void DnsInfoBtn_Checked(object sender, RoutedEventArgs e)
	{
		DnsInfoGrid.Visibility = Visibility.Visible;
		DnsCacheGrid.Visibility = Visibility.Collapsed;
	}

	private void DnsCacheBtn_Checked(object sender, RoutedEventArgs e)
	{
		DnsInfoGrid.Visibility = Visibility.Collapsed;
		DnsCacheGrid.Visibility = Visibility.Visible;
	}

	private async void GetDnsCacheBtn_Click(object sender, RoutedEventArgs e)
	{
		// Show Loading UI
		ItemDisplayer.Children.Clear();
		StatusSection.Visibility = Visibility.Visible;
		SuccessBorder.Visibility = Visibility.Collapsed;
		ErrorBorder.Visibility = Visibility.Collapsed;
		TotalTxt.Text = Properties.Resources.Loading;

		int success = 0;
		int error = 0;
		int total = 0;

		var cache = await Global.GetDnsCache();
		if (cache != null)
		{
			for (int i = 0; i < cache.Length; i++)
			{
				ItemDisplayer.Children.Add(new DnsCacheItem(cache[i]));
				total++;

				if (cache[i].Status == 0) success++; else error++;
			}
		}

		// Update Status section
		SuccessBorder.Visibility = Visibility.Visible;
		ErrorBorder.Visibility = Visibility.Visible;

		SuccessNbTxt.Text = success.ToString();
		ErrorNbTxt.Text = error.ToString();
		TotalTxt.Text = total.ToString();
	}

	private void FlushDnsBtn_Click(object sender, RoutedEventArgs e)
	{
		if (MessageBox.Show(Properties.Resources.FlushDNSMessage, Properties.Resources.FlushDNS, MessageBoxButton.YesNoCancel, MessageBoxImage.Question) == MessageBoxResult.Yes)
		{
			ItemDisplayer.Children.Clear();
			ProcessStartInfo processInfo = new()
			{
				FileName = "ipconfig",
				Arguments = "/flushdns",
				RedirectStandardOutput = true,
				RedirectStandardError = true,
				UseShellExecute = false,
				CreateNoWindow = true
			};

			using Process process = Process.Start(processInfo);
			// Read the output from the command
			string output = process.StandardOutput.ReadToEnd();
			string error = process.StandardError.ReadToEnd();

			// Wait for the process to exit
			process.WaitForExit();
			MessageBox.Show(Properties.Resources.FlushDNSSuccess, Properties.Resources.FlushDNS, MessageBoxButton.OK, MessageBoxImage.Information);

			// Clear Status section
			SuccessNbTxt.Text = "0";
			ErrorNbTxt.Text = "0";
			TotalTxt.Text = "0";

			StatusSection.Visibility = Visibility.Collapsed;
		}
	}
}
