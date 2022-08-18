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
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Windows;
using System.Windows.Controls;

namespace InternetTest.Pages;
/// <summary>
/// Interaction logic for IpConfig.xaml
/// </summary>
public partial class IpConfigPage : Page
{
	bool codeInjected = false;
	public IpConfigPage()
	{
		InitializeComponent();
		InitUI();
		InjectSynethiaCode();
	}

	private void InjectSynethiaCode()
	{
		if (codeInjected) return;
		codeInjected = true;
		foreach (Button b in Global.FindVisualChildren<Button>(this))
		{
			b.Click += (sender, e) =>
			{
				Global.SynethiaConfig.MyIPPageInfo.InteractionCount++;
			};
		}

		// For each TextBox of the page
		foreach (TextBox textBox in Global.FindVisualChildren<TextBox>(this))
		{
			textBox.GotFocus += (o, e) =>
			{
				Global.SynethiaConfig.MyIPPageInfo.InteractionCount++;
			};
		}

		// For each CheckBox/RadioButton of the page
		foreach (CheckBox checkBox in Global.FindVisualChildren<CheckBox>(this))
		{
			checkBox.Checked += (o, e) =>
			{
				Global.SynethiaConfig.MyIPPageInfo.InteractionCount++;
			};
			checkBox.Unchecked += (o, e) =>
			{
				Global.SynethiaConfig.MyIPPageInfo.InteractionCount++;
			};
		}

		foreach (RadioButton radioButton in Global.FindVisualChildren<RadioButton>(this))
		{
			radioButton.Checked += (o, e) =>
			{
				Global.SynethiaConfig.MyIPPageInfo.InteractionCount++;
			};
			radioButton.Unchecked += (o, e) =>
			{
				Global.SynethiaConfig.MyIPPageInfo.InteractionCount++;
			};
		}
	}

	private void InitUI()
	{
		TitleTxt.Text = $"{Properties.Resources.Commands} > {Properties.Resources.IPConfig}";

		IpConfigDisplayer.Children.Clear(); // Clear everything
											// Get network interfaces
		var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
		for (int i = 0; i < networkInterfaces.Length; i++)
		{
			var props = networkInterfaces[i].GetIPProperties();
			IpConfigDisplayer.Children.Add(new IpConfigItem(new
				(
					networkInterfaces[i].Name,
					props.UnicastAddresses.FirstOrDefault(x => x.Address.AddressFamily == AddressFamily.InterNetwork)?.Address.ToString(),
					props.UnicastAddresses.FirstOrDefault(x => x.Address.AddressFamily == AddressFamily.InterNetwork)?.IPv4Mask.ToString(),
					props.GatewayAddresses.FirstOrDefault(x => x.Address.AddressFamily == AddressFamily.InterNetwork)?.Address.ToString(),
					props.UnicastAddresses.FirstOrDefault(x => x.Address.AddressFamily == AddressFamily.InterNetworkV6)?.Address.ToString(),
					props.GatewayAddresses.FirstOrDefault(x => x.Address.AddressFamily == AddressFamily.InterNetworkV6)?.Address.ToString(),
					props.DnsSuffix,
					networkInterfaces[i].OperationalStatus
				)));
		}
	}

	private void RefreshBtn_Click(object sender, RoutedEventArgs e)
	{
		InitUI(); // Refresh the UI
	}
}
