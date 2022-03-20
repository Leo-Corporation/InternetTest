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
/// Interaction logic for WebsiteItem.xaml
/// </summary>
public partial class WebsiteItem : UserControl
{
	public string URL { get; init; }
	public WebsiteItem(string url)
	{
		InitializeComponent();
		URL = url;

		InitUI();
	}

	private void InitUI()
	{
		StatusBorder.Visibility = Visibility.Collapsed; // Hide
		WebsiteTxt.Text = URL;
	}

	private void CheckBtn_Click(object sender, RoutedEventArgs e)
	{
		Test();
	}

	internal async void Test()
	{
		StatusInfo statusInfo = await Global.GetStatusCodeAsync(URL);
		int status = statusInfo.StatusCode;
		if (status >= 200 && status <= 299)
		{
			StatusBorder.Background = new SolidColorBrush { Color = (Color)ColorConverter.ConvertFromString(App.Current.Resources["Green"].ToString()) }; // Set the foreground
			StatusIconTxt.Text = "\uF299"; // Set text
		}
		else if (status >= 400 && status <= 599)
		{
			StatusBorder.Background = new SolidColorBrush { Color = (Color)ColorConverter.ConvertFromString(App.Current.Resources["Red"].ToString()) }; // Set the foreground
			StatusIconTxt.Text = "\uF36E"; // Set text
		}
		else
		{
			StatusBorder.Background = new SolidColorBrush { Color = (Color)ColorConverter.ConvertFromString(App.Current.Resources["AccentColor"].ToString()) }; // Set the foreground
			StatusIconTxt.Text = "\uF4AB"; // Set text
		}

		StatusCodeTxt.Text = status.ToString(); // Set text
		StatusBorder.Visibility = Visibility.Visible; // Show
		StatusToolTip.Content = $"{statusInfo.StatusCode} - {statusInfo.StatusMessage}"; // Set text
	}

	private void DismissBtn_Click(object sender, RoutedEventArgs e)
	{
		Global.DownDetectorPage.WebsiteItemPanel.Children.Remove(this); // Delete
	}
}
