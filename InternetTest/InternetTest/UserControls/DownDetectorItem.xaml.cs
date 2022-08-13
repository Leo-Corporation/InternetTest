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
using System.Collections.Generic;
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

namespace InternetTest.UserControls;
/// <summary>
/// Interaction logic for DownDetectorItem.xaml
/// </summary>
public partial class DownDetectorItem : UserControl
{
	StackPanel ParentStackPanel { get; init; }
	internal DownDetectorTestResult DownDetectorTestResult { get; set; }
	string URL { get; set; }
	public DownDetectorItem(StackPanel parentElement, string url, DownDetectorTestResult downDetectorTestResult)
	{
		InitializeComponent();
		ParentStackPanel = parentElement; // Save the parent element
		URL = url; // Save the URL
		DownDetectorTestResult = downDetectorTestResult; // Save the result

		InitUI(); // load the UI
	}

	private void InitUI()
	{
		WebsiteTxt.Text = URL; // Set text
	}

	private async void TestSiteBtn_Click(object sender, RoutedEventArgs e)
	{
		if (!WebsiteTxt.Text.StartsWith("http"))
		{
			WebsiteTxt.Text = "https://" + WebsiteTxt.Text;
		}
		DownDetectorTestResult = await Global.DownDetectorPage.LaunchTest(WebsiteTxt.Text); // Launch the test
	}

	private void InfoBtn_Click(object sender, RoutedEventArgs e)
	{
		Global.DownDetectorPage.DetailsStatusTxt.Text = DownDetectorTestResult.Code.ToString(); // Set the text
		Global.DownDetectorPage.DetailsTimeTxt.Text = $"{DownDetectorTestResult.Time}ms"; // Set the text
		Global.DownDetectorPage.DetailsMessageTxt.Text = DownDetectorTestResult.Message; // Set the text
	}

	private void DeleteBtn_Click(object sender, RoutedEventArgs e)
	{
		ParentStackPanel.Children.Remove(this); // Delete this element from the parent element
		Global.DownDetectorPage.TotalWebsites--; // Update the total websites
		Global.DownDetectorPage.TestBtn.Content = $"{Properties.Resources.Test} ({Global.DownDetectorPage.TotalWebsites})"; // Update the text
	}
}
