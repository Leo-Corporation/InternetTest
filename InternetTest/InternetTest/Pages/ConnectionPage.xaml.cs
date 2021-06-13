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

namespace InternetTest.Pages
{
	/// <summary>
	/// Interaction logic for ConnectionPage.xaml
	/// </summary>
	public partial class ConnectionPage : Page
	{
		public ConnectionPage()
		{
			InitializeComponent();
			if (!Global.Settings.LaunchTestOnStart.HasValue ? true : Global.Settings.LaunchTestOnStart.Value)
			{
				Test(Global.Settings.TestSite); // Launch a test 
			}
			else
			{
				ConnectionStatusTxt.Text = Properties.Resources.LaunchTestToCheckConnection; // Set text of the label
				InternetIconTxt.Text = "\uF4AB"; // Set the icon
				InternetIconTxt.Foreground = new SolidColorBrush { Color = (Color)ColorConverter.ConvertFromString(App.Current.Resources["Gray"].ToString()) }; // Set the foreground
			}
		}

		/// <summary>
		/// Launch a network test.
		/// </summary>
		/// <param name="customSite">Leave empty if you don't want to specify a custom website.</param>
		private async void Test(string customSite)
		{
			if (string.IsNullOrEmpty(customSite)) // If a custom site isn't specified
			{
				if (await NetworkConnection.IsAvailableAsync()) // If there is Internet
				{
					ConnectionStatusTxt.Text = Properties.Resources.Connected; // Set text of the label
					InternetIconTxt.Text = "\uF299"; // Set the icon
					InternetIconTxt.Foreground = new SolidColorBrush { Color = (Color)ColorConverter.ConvertFromString(App.Current.Resources["Green"].ToString()) }; // Set the foreground
					HistoricDisplayer.Children.Add(new ConnectionHistoricItem(true, HistoricDisplayer));
				}
				else
				{
					ConnectionStatusTxt.Text = Properties.Resources.NotConnected; // Set text of the label
					InternetIconTxt.Text = "\uF36E"; // Set the icon
					InternetIconTxt.Foreground = new SolidColorBrush { Color = (Color)ColorConverter.ConvertFromString(App.Current.Resources["Red"].ToString()) }; // Set the foreground
					HistoricDisplayer.Children.Add(new ConnectionHistoricItem(false, HistoricDisplayer));
				}
			}
			else
			{
				if (await NetworkConnection.IsAvailableTestSiteAsync(customSite)) // If there is Internet
				{
					ConnectionStatusTxt.Text = Properties.Resources.Connected; // Set text of the label
					InternetIconTxt.Text = "\uF299"; // Set the icon
					InternetIconTxt.Foreground = new SolidColorBrush { Color = (Color)ColorConverter.ConvertFromString(App.Current.Resources["Green"].ToString()) }; // Set the foreground
					HistoricDisplayer.Children.Add(new ConnectionHistoricItem(true, HistoricDisplayer));
				}
				else
				{
					ConnectionStatusTxt.Text = Properties.Resources.NotConnected; // Set text of the label
					InternetIconTxt.Text = "\uF36E"; // Set the icon
					InternetIconTxt.Foreground = new SolidColorBrush { Color = (Color)ColorConverter.ConvertFromString(App.Current.Resources["Red"].ToString()) }; // Set the foreground
					HistoricDisplayer.Children.Add(new ConnectionHistoricItem(false, HistoricDisplayer));
				}
			}
		}

		private void TestBtn_Click(object sender, RoutedEventArgs e)
		{
			Test(Global.Settings.TestSite); // Launch a test
		}

		private void OpenBrowserBtn_Click(object sender, RoutedEventArgs e)
		{
			Global.OpenLinkInBrowser(Global.Settings.TestSite); // Open in a browser
		}

		internal void HistoryBtn_Click(object sender, RoutedEventArgs e)
		{
			if (HistoricDisplayer.Children.Count > 0)
			{
				if (HistoricPanel.Visibility == Visibility.Visible)
				{
					HistoricPanel.Visibility = Visibility.Collapsed; // Set
					HistoryBtn.Content = "\uF47F"; // Set text
				}
				else
				{
					HistoricPanel.Visibility = Visibility.Visible; // Set
					HistoryBtn.Content = "\uF36A"; // Set text
				}
			}
			else
			{
				HistoricPanel.Visibility = Visibility.Collapsed; // Set
				HistoryBtn.Content = "\uF47F"; // Set text
				if (sender is not ConnectionHistoricItem)
				{
					MessageBox.Show(Properties.Resources.EmptyHistory, Properties.Resources.InternetTest, MessageBoxButton.OK, MessageBoxImage.Information); // Show message 
				}
			}
		}
	}
}
