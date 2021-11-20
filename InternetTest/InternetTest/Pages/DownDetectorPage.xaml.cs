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
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace InternetTest.Pages
{
	/// <summary>
	/// Interaction logic for DownDetectorPage.xaml
	/// </summary>
	public partial class DownDetectorPage : Page
	{
		public DownDetectorPage()
		{
			InitializeComponent();
			HistoryBtn.Visibility = Visibility.Collapsed; // Set visibility
			StatusBorder.Visibility = Visibility.Collapsed; // Hide
		}

		/// <summary>
		/// Launch a network test.
		/// </summary>
		/// <param name="customSite">Leave empty if you don't want to specify a custom website.</param>
		private async void Test(string customSite)
		{
			StatusBorder.Visibility = Visibility.Collapsed; // Hide
			ConnectionStatusTxt.Text = Properties.Resources.Testing; // Set text of the label
			HistoryBtn.Visibility = Visibility.Visible; // Set visibility
			InternetIconTxt.Text = "\uF45F"; // Set the icon
			InternetIconTxt.Foreground = new SolidColorBrush { Color = (Color)ColorConverter.ConvertFromString(App.Current.Resources["Gray"].ToString()) }; // Set the foreground

			if (await NetworkConnection.IsAvailableTestSiteAsync(customSite)) // If there is Internet
			{
				ConnectionStatusTxt.Text = Properties.Resources.WebsiteAvailable; // Set text of the label
				InternetIconTxt.Text = "\uF299"; // Set the icon
				InternetIconTxt.Foreground = new SolidColorBrush { Color = (Color)ColorConverter.ConvertFromString(App.Current.Resources["Green"].ToString()) }; // Set the foreground
			}
			else
			{
				ConnectionStatusTxt.Text = Properties.Resources.WebsiteDown; // Set text of the label
				InternetIconTxt.Text = "\uF36E"; // Set the icon
				InternetIconTxt.Foreground = new SolidColorBrush { Color = (Color)ColorConverter.ConvertFromString(App.Current.Resources["Red"].ToString()) }; // Set the foreground
			}

			StatusInfo statusInfo = GetStatusCode(customSite);
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
			StatusMsgTxt.Text = $"- {statusInfo.StatusMessage}"; // Set text

			HistoricDisplayer.Children.Add(new HistoricItem(customSite, ConnectionStatusTxt.Text, HistoricDisplayer)); // Add
		}

		private StatusInfo GetStatusCode(string website)
		{
			try
			{
				// Create a web request for an invalid site. Substitute the "invalid site" strong in the Create call with a invalid name.
				HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(website);

				// Get the associated response for the above request.
				HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
				myHttpWebResponse.Close();
				return new StatusInfo(200, "OK"); // Return
			}
			catch (WebException e)
			{
				if (e.Status == WebExceptionStatus.ProtocolError)
				{
					var status = ((HttpWebResponse)e.Response).StatusCode;
					return new StatusInfo((int)status, ((HttpWebResponse)e.Response).StatusDescription); // Return;
				}
				return new StatusInfo(400, "Bad Request"); // Return
			}
			catch
			{
				return new StatusInfo(400, "Bad Request"); // Return
			}
		}

		private string FormatURL(string url)
		{
			if (!url.Contains("https://") && !url.Contains("http://")) // If there isn't http(s)
			{
				return (Global.Settings.UseHTTPS.Value ? "https://" : "http://") + url; // Add the https://
			}
			else
			{
				return url; // Do nothing
			}
		}

		private void CheckBtn_Click(object sender, RoutedEventArgs e)
		{
			if (!string.IsNullOrEmpty(WebsiteTxt.Text) && !string.IsNullOrWhiteSpace(WebsiteTxt.Text))
			{
				Test(FormatURL(WebsiteTxt.Text)); // Test
				WebsiteTxt.Text = FormatURL(WebsiteTxt.Text); // Format 
			}
		}

		private void OpenBrowserBtn_Click(object sender, RoutedEventArgs e)
		{
			if (!string.IsNullOrEmpty(WebsiteTxt.Text) && !string.IsNullOrWhiteSpace(WebsiteTxt.Text))
			{
				Global.OpenLinkInBrowser(FormatURL(WebsiteTxt.Text)); // Open in a browser 
			}
		}

		internal void HistoryBtn_Click(object sender, RoutedEventArgs e)
		{
			if (HistoricDisplayer.Children.Count > 0)
			{
				HistoryBtn.Visibility = Visibility.Visible; // Set visibility
				if (HistoricPanel.Visibility == Visibility.Visible)
				{
					HistoricPanel.Visibility = Visibility.Collapsed; // Set
					ContentGrid.Visibility = Visibility.Visible; // Set
					HistoryBtn.Content = "\uF47F"; // Set text
				}
				else
				{
					HistoricPanel.Visibility = Visibility.Visible; // Set
					ContentGrid.Visibility = Visibility.Collapsed; // Set
					HistoryBtn.Content = "\uF36A"; // Set text
				}
			}
			else
			{
				HistoryBtn.Visibility = Visibility.Collapsed; // Set visibility
				HistoricPanel.Visibility = Visibility.Collapsed; // Set
				ContentGrid.Visibility = Visibility.Visible; // Set
				HistoryBtn.Content = "\uF47F"; // Set text
				if (sender is not HistoricItem)
				{
					MessageBox.Show(Properties.Resources.EmptyHistory, Properties.Resources.InternetTest, MessageBoxButton.OK, MessageBoxImage.Information); // Show message 
				}
			}
		}

		private void StatusBorder_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			StatusMsgTxt.Visibility = StatusMsgTxt.Visibility == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed; // Show/hide
		}
	}
}
