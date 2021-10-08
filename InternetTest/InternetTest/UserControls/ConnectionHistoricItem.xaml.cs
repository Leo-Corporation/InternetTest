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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace InternetTest.UserControls
{
	/// <summary>
	/// Interaction logic for ConnectionHistoricItem.xaml
	/// </summary>
	public partial class ConnectionHistoricItem : UserControl
	{
		bool Connected { get; init; }
		StackPanel StackPanel { get; init; }
		public ConnectionHistoricItem(bool connected, StackPanel stackPanel)
		{
			InitializeComponent();
			Connected = connected;
			StackPanel = stackPanel;
			InitUI(); // Load the UI
		}

		private void InitUI()
		{
			StateTxt.Text = Connected ? Properties.Resources.ConnectedShort : Properties.Resources.NotConnectedShort; // Set text
			StateIconTxt.Text = Connected ? "\uF299" : "\uF36E"; // Set text
			TimeTxt.Text = DateTime.Now.ToString("hh:mm");

			StateTxt.Foreground = Connected ? new SolidColorBrush { Color = (Color)ColorConverter.ConvertFromString(App.Current.Resources["Green"].ToString()) } : new SolidColorBrush { Color = (Color)ColorConverter.ConvertFromString(App.Current.Resources["Red"].ToString()) };
			StateIconTxt.Foreground = Connected ? new SolidColorBrush { Color = (Color)ColorConverter.ConvertFromString(App.Current.Resources["Green"].ToString()) } : new SolidColorBrush { Color = (Color)ColorConverter.ConvertFromString(App.Current.Resources["Red"].ToString()) };
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
}
