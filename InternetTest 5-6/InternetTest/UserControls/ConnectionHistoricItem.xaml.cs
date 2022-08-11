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
/// Interaction logic for ConnectionHistoricItem.xaml
/// </summary>
public partial class ConnectionHistoricItem : UserControl
{
	WebInfo WebInfo { get; init; }
	StackPanel StackPanel { get; init; }
	public ConnectionHistoricItem(WebInfo webInfo, StackPanel stackPanel)
	{
		InitializeComponent();
		WebInfo = webInfo;
		StackPanel = stackPanel;
		InitUI(); // Load the UI
	}

	private void InitUI()
	{
		StateTxt.Text = WebInfo.Connected ? Properties.Resources.ConnectedShort : Properties.Resources.NotConnectedShort; // Set text
		StateIconTxt.Text = WebInfo.Connected ? "\uF299" : "\uF36E"; // Set text
		TimeTxt.Text = WebInfo.Timestamp.ToString("hh:mm");

		StateTxt.Foreground = WebInfo.Connected ? new SolidColorBrush { Color = (Color)ColorConverter.ConvertFromString(App.Current.Resources["Green"].ToString()) } : new SolidColorBrush { Color = (Color)ColorConverter.ConvertFromString(App.Current.Resources["Red"].ToString()) };
		StateIconTxt.Foreground = WebInfo.Connected ? new SolidColorBrush { Color = (Color)ColorConverter.ConvertFromString(App.Current.Resources["Green"].ToString()) } : new SolidColorBrush { Color = (Color)ColorConverter.ConvertFromString(App.Current.Resources["Red"].ToString()) };

		// Load tooltip
		ToolTip.Content = $"{Properties.Resources.Status} - {(WebInfo.Connected ? Properties.Resources.Connected : Properties.Resources.NotConnected)}\n" +
			$"{Properties.Resources.URL} - {WebInfo.URL}\n" +
			$"{Properties.Resources.StatusCode} - {WebInfo.StatusCode}\n" +
			$"{Properties.Resources.Time} - {WebInfo.Timestamp:G}";
	}

	private void DismissBtn_Click(object sender, RoutedEventArgs e)
	{
		StackPanel.Children.Remove(this); // Remove
		if (StackPanel.Children.Count == 0)
		{
			Global.ConnectionPage.HistoryBtn_Click(this, null);
		}
	}
}
