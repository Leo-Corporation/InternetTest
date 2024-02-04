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
using PeyrSharp.Env;
using Synethia;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace InternetTest.Pages;
/// <summary>
/// Interaction logic for WiFiPasswordsPage.xaml
/// </summary>
public partial class WiFiPasswordsPage : Page
{
	bool codeInjected = !Global.Settings.UseSynethia;
	public WiFiPasswordsPage()
	{
		InitializeComponent();
		InitUI();
		Loaded += (o, e) => SynethiaManager.InjectSynethiaCode(this, Global.SynethiaConfig.PagesInfo, 7, ref codeInjected);
	}

	Placeholder Placeholder = new(Properties.Resources.NothingToShow, "\uF227");
	private void InitUI()
	{
		TitleTxt.Text = $"{Properties.Resources.Commands} > {Properties.Resources.WifiPasswords}";
		PlaceholderGrid.Children.Add(Placeholder); // Show the placeholder instead of an empty page

		try
		{
			if (!Directory.Exists(FileSys.AppDataPath + @"\Léo Corporation\InternetTest Pro\WiFis")) return;
			LoadWiFiInfo(FileSys.AppDataPath + @"\Léo Corporation\InternetTest Pro\WiFis");
		}
		catch { }
	}

	internal async void GetWiFiBtn_Click(object sender, RoutedEventArgs e)
	{
		await GetWiFiNetworksInfo(); // Update the UI		

		// Increment the interaction count of the ActionInfo in Global.SynethiaConfig
		Global.SynethiaConfig.ActionsInfo.First(a => a.Name == "WiFiPasswords.Get").UsageCount++;
	}

	internal async Task GetWiFiNetworksInfo()
	{

		try
		{
			WiFiItemDisplayer.Children.Clear(); // Clear the panel

			// Check if the temp directory exists
			string path = FileSys.AppDataPath + @"\Léo Corporation\InternetTest Pro\WiFis";

			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			};

			// Run "netsh wlan export profile key=clear" command
			Process process = new();
			process.StartInfo.FileName = "cmd.exe";
			process.StartInfo.Arguments = $"/c netsh wlan export profile key=clear folder=\"{path}\"";
			process.StartInfo.UseShellExecute = false;
			process.StartInfo.CreateNoWindow = true;
			process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
			process.Start();
			await process.WaitForExitAsync();

			// Read the files
			LoadWiFiInfo(path);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, Properties.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
		}
	}

	internal async Task ExportWiFiNetworkInfo(string path,bool includePasswords)
	{
		try
		{
			Process process = new();
			process.StartInfo.FileName = "cmd.exe";
			process.StartInfo.Arguments = $"/c netsh wlan export profile {(includePasswords ? "key=clear" : "")} folder=\"{path}\"";
			process.StartInfo.UseShellExecute = false;
			process.StartInfo.CreateNoWindow = true;
			process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
			process.Start();
			await process.WaitForExitAsync();

			MessageBox.Show(Properties.Resources.WiFiExportSuccessful, Properties.Resources.Export, MessageBoxButton.OK, MessageBoxImage.Information);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, Properties.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
		}
	}

	internal void LoadWiFiInfo(string path)
	{
		string[] files = Directory.GetFiles(path);
		for (int i = 0; i < files.Length; i++)
		{
			XmlSerializer serializer = new(typeof(WLANProfile));
			StreamReader streamReader = new(files[i]); // Where the file is going to be read

			var test = (WLANProfile?)serializer.Deserialize(streamReader);

			if (test != null)
			{
				WiFiItemDisplayer.Children.Add(new WiFiInfoItem(test));
			}
			streamReader.Close();
		}

		if (WiFiItemDisplayer.Children.Count == 0)
		{
			PlaceholderGrid.Visibility = Visibility.Visible;
			Placeholder.Visibility = Visibility.Visible;
		}
		else
		{
			PlaceholderGrid.Visibility = Visibility.Collapsed;
			Placeholder.Visibility = Visibility.Collapsed;
		}
	}

	private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
	{
		DismissBtn.Visibility = SearchTxt.Text.Length > 0 ? Visibility.Visible : Visibility.Collapsed;

		PlaceholderGrid.Visibility = Visibility.Collapsed;
		Placeholder.Visibility = Visibility.Collapsed;
		if (!(WiFiItemDisplayer.Children.Count > 0)) return;

		List<bool> vis = Enumerable.Empty<bool>().ToList();
		for (int i = 0; i < WiFiItemDisplayer.Children.Count; i++)
		{
			if (SearchTxt.Text == "")
			{
				((WiFiInfoItem)WiFiItemDisplayer.Children[i]).Visibility = Visibility.Visible;
				continue;
			}

			if (WiFiItemDisplayer.Children[i] is WiFiInfoItem wiFiInfoItem)
			{
				bool b = wiFiInfoItem.ToString().ToLower().Contains(SearchTxt.Text.ToLower());
				wiFiInfoItem.Visibility = !b ? Visibility.Collapsed : Visibility.Visible;
				vis.Add(wiFiInfoItem.Visibility == Visibility.Visible);
			}
		}

		if (vis.Count == 0 || vis is null) return;
		if (vis.Contains(true)) return;
		PlaceholderGrid.Visibility = Visibility.Visible;
		Placeholder.Visibility = Visibility.Visible;
	}

	private void DismissBtn_Click(object sender, RoutedEventArgs e)
	{
		SearchTxt.Text = "";
		if (WiFiItemDisplayer.Children.Count > 0)
		{
			Placeholder.Visibility = Visibility.Collapsed;
			PlaceholderGrid.Visibility = Visibility.Collapsed;
		}
		else
		{
			Placeholder.Visibility = Visibility.Visible;
			PlaceholderGrid.Visibility = Visibility.Visible;
		}
	}

	internal void ToggleConfidentialMode()
	{
		try
		{
			for (int i = 0; i < WiFiItemDisplayer.Children.Count; i++)
			{
				if (WiFiItemDisplayer.Children[i] is WiFiInfoItem wiFiInfoItem)
				{
					wiFiInfoItem.InitUI();
				}
			}
		}
		catch { }
	}

	private void ExportBtn_Click(object sender, RoutedEventArgs e)
	{
		ExportPopup.IsOpen = true;
    }

	private async void ExportWithPasswordBtn_Click(object sender, RoutedEventArgs e)
	{
        System.Windows.Forms.FolderBrowserDialog folderBrowserDialog = new() { UseDescriptionForTitle = true, Description = Properties.Resources.ExportWithPasswords };
		if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
		{
			await ExportWiFiNetworkInfo(folderBrowserDialog.SelectedPath, true);
		}
    }

	private async void ExportWithoutPasswordBtn_Click(object sender, RoutedEventArgs e)
	{
		System.Windows.Forms.FolderBrowserDialog folderBrowserDialog = new() { UseDescriptionForTitle = true, Description = Properties.Resources.ExportWithoutPasswords };
		if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
		{
			await ExportWiFiNetworkInfo(folderBrowserDialog.SelectedPath, false);
		}
	}
}
