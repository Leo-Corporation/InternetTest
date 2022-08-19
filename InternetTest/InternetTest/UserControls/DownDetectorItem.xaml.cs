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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace InternetTest.UserControls;
/// <summary>
/// Interaction logic for DownDetectorItem.xaml
/// </summary>
public partial class DownDetectorItem : UserControl
{
	bool codeInjected = false;
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

		WebsiteTxt.Text = URL; // Set text

		if (DownDetectorTestResult.Code == 0) return;
		UpdateIcon();
		
	}

	internal void UpdateIcon()
	{
		if (DownDetectorTestResult.Code >= 400) // If the website is down
		{
			IconTxt.Foreground = new SolidColorBrush(Global.GetColorFromResource("Red")); // Set color to red
			IconTxt.Text = "\uF36E"; // Add (down) to the text
		}
		else // If the website is up
		{
			IconTxt.Foreground = new SolidColorBrush(Global.GetColorFromResource("Green")); // Set color to green
			IconTxt.Text = "\uF299"; // Add (up) to the text
		}
	}
	
	private async void TestSiteBtn_Click(object sender, RoutedEventArgs e)
	{
		if (!WebsiteTxt.Text.StartsWith("http"))
		{
			WebsiteTxt.Text = "https://" + WebsiteTxt.Text;
		}
		DownDetectorTestResult = await Global.DownDetectorPage.LaunchTest(WebsiteTxt.Text); // Launch the test
		URL = WebsiteTxt.Text;

		UpdateIcon(); // Update the icon
	}

	private void InfoBtn_Click(object sender, RoutedEventArgs e)
	{
		if (DownDetectorTestResult is not null)
		{
			Global.DownDetectorPage.DetailsStatusTxt.Text = DownDetectorTestResult.Code.ToString(); // Set the text
			Global.DownDetectorPage.DetailsTimeTxt.Text = $"{DownDetectorTestResult.Time}ms"; // Set the text
			Global.DownDetectorPage.DetailsMessageTxt.Text = DownDetectorTestResult.Message; // Set the text 
			Global.DownDetectorPage.DetailsSiteNameTxt.Text = string.Format(Properties.Resources.OfWebsite, URL); // Set the text 
		}
	}

	private void DeleteBtn_Click(object sender, RoutedEventArgs e)
	{
		ParentStackPanel.Children.Remove(this); // Delete this element from the parent element
		Global.DownDetectorPage.TotalWebsites--; // Update the total websites
		Global.DownDetectorPage.TestBtn.Content = $"{Properties.Resources.Test} ({Global.DownDetectorPage.TotalWebsites})"; // Update the text
	}
}
