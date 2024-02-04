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
using Synethia;
using System.Linq;
using System.Net.NetworkInformation;
using System.Windows;
using System.Windows.Controls;

namespace InternetTest.Pages;
/// <summary>
/// Interaction logic for TraceroutePage.xaml
/// </summary>
public partial class TraceroutePage : Page
{
	bool codeInjected = !Global.Settings.UseSynethia;
	public TraceroutePage()
	{
		InitializeComponent();
		Loaded += (o, e) => SynethiaManager.InjectSynethiaCode(this, Global.SynethiaConfig.PagesInfo, 6, ref codeInjected);
		InitUI();
	}


	private void InitUI()
	{
		TitleTxt.Text = $"{Properties.Resources.Commands} > {Properties.Resources.TraceRoute}"; // Set the title
	}

	private void DismissBtn_Click(object sender, RoutedEventArgs e)
	{
		AddressTxt.Text = "";
	}

	private async void TraceBtn_Click(object sender, RoutedEventArgs e)
	{
		// Increment the interaction count of the ActionInfo in Global.SynethiaConfig
		Global.SynethiaConfig.ActionsInfo.First(a => a.Name == "Traceroute.Execute").UsageCount++;

		if (string.IsNullOrEmpty(AddressTxt.Text) || string.IsNullOrWhiteSpace(AddressTxt.Text))
		{
			MessageBox.Show(Properties.Resources.InvalidURLMsg, Properties.Resources.GetDnsInfo, MessageBoxButton.OK, MessageBoxImage.Error);
			return;
		}

		// Show the waiting screen
		TraceBtn.IsEnabled = false;
		StatusPanel.Visibility = Visibility.Collapsed;
		WaitingScreen.Visibility = Visibility.Visible;
		WaitTxt.Text = Properties.Resources.TraceProgress;
		WaitIconTxt.Visibility = Visibility.Collapsed;
		Spinner.Visibility = Visibility.Visible;
		TracertPanel.Visibility = Visibility.Collapsed;
		TracertPanel.Children.Clear();

		try
		{
			// Get traceroute
			var route = await Global.Trace(AddressTxt.Text, Global.Settings.TraceRouteMaxHops ?? 30, Global.Settings.TraceRouteMaxTimeOut ?? 5000);
			int success = 0; int failed = 0; long time = 0;

			// Update the UI with each step
			for (int i = 0; i < route.Count; i++)
			{
				TracertPanel.Children.Add(new TraceRouteItem(route[i], i == route.Count - 1));
				if (route[i].Status == IPStatus.Success || route[i].Status == IPStatus.TtlExpired) success++;
				else failed++;
				time += route[i].RoundtripTime;
			}

			// Set the values of the overview panel
			SucessTxt.Text = success.ToString();
			FailedTxt.Text = failed.ToString();
			DurationTxt.Text = $"{time}ms";
			HopsTxt.Text = $"{route.Count} {Properties.Resources.HopsLower}";

			// Show the overview and the traceroute
			StatusPanel.Visibility = Visibility.Visible;
			WaitingScreen.Visibility = Visibility.Collapsed;
			TracertPanel.Visibility = Visibility.Visible;

			WaitTxt.Text = Properties.Resources.TraceRouteInformation; // Reset text to default
			WaitIconTxt.Visibility = Visibility.Visible;
			Spinner.Visibility = Visibility.Collapsed;
		}
		catch { }

		TraceBtn.IsEnabled = true;
	}

	private void AddressTxt_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
	{
		if (e.Key == System.Windows.Input.Key.Enter)
		{
			TraceBtn_Click(sender, e);
		}
	}

	private void AddressTxt_TextChanged(object sender, TextChangedEventArgs e)
	{
		DismissBtn.Visibility = AddressTxt.Text.Length > 0 ? Visibility.Visible : Visibility.Collapsed;
	}
}
