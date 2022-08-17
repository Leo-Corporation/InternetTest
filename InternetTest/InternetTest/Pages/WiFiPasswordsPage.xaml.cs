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
using LeoCorpLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
using System.Xml.Serialization;

namespace InternetTest.Pages;
/// <summary>
/// Interaction logic for WiFiPasswordsPage.xaml
/// </summary>
public partial class WiFiPasswordsPage : Page
{
	bool codeInjected = false;
	public WiFiPasswordsPage()
	{
		InitializeComponent();
		InitUI();
		InjectSynethiaCode();
	}

	private void InitUI()
	{
		TitleTxt.Text = $"{Properties.Resources.Commands} > {Properties.Resources.WifiPasswords}";
	}

	private void InjectSynethiaCode()
	{
		if (codeInjected) return;
		codeInjected = true;
		foreach (Button b in Global.FindVisualChildren<Button>(this))
		{
			b.Click += (sender, e) =>
			{
				Global.SynethiaConfig.StatusPageInfo.InteractionCount++;
			};
		}

		// For each TextBox of the page
		foreach (TextBox textBox in Global.FindVisualChildren<TextBox>(this))
		{
			textBox.GotFocus += (o, e) =>
			{
				Global.SynethiaConfig.StatusPageInfo.InteractionCount++;
			};
		}

		// For each CheckBox/RadioButton of the page
		foreach (CheckBox checkBox in Global.FindVisualChildren<CheckBox>(this))
		{
			checkBox.Checked += (o, e) =>
			{
				Global.SynethiaConfig.StatusPageInfo.InteractionCount++;
			};
			checkBox.Unchecked += (o, e) =>
			{
				Global.SynethiaConfig.StatusPageInfo.InteractionCount++;
			};
		}

		foreach (RadioButton radioButton in Global.FindVisualChildren<RadioButton>(this))
		{
			radioButton.Checked += (o, e) =>
			{
				Global.SynethiaConfig.StatusPageInfo.InteractionCount++;
			};
			radioButton.Unchecked += (o, e) =>
			{
				Global.SynethiaConfig.StatusPageInfo.InteractionCount++;
			};
		}
	}

	private void GetWiFiBtn_Click(object sender, RoutedEventArgs e)
	{
		GetWiFiNetworksInfo(); // Update the UI
	}
	
	internal async void GetWiFiNetworksInfo()
	{
		
		try
		{
			WiFiItemDisplayer.Children.Clear(); // Clear the panel

			// Check if the temp directory exists
			string path = Env.AppDataPath + @"\Léo Corporation\InternetTest Pro\Temp";
			
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
				
				File.Delete(files[i]); // Remove the temp file

			}
			Directory.Delete(path); // Delete the temp directory			
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, Properties.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
		}
	}
}
