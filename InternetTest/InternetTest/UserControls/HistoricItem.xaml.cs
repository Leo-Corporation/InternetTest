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

namespace InternetTest.UserControls
{
	/// <summary>
	/// Interaction logic for HistoricItem.xaml
	/// </summary>
	public partial class HistoricItem : UserControl
	{
		string URL { get; init; }
		string Status { get; init; }
		StackPanel StackPanel { get; init; }
		public HistoricItem(string url, string status, StackPanel stackPanel)
		{
			InitializeComponent();
			URL = url;
			Status = status;
			StackPanel = stackPanel;
			InitUI();
		}

		private void InitUI()
		{
			SiteNameTxt.Text = URL; // Set text
			StateTxt.Text = Status; // Set text
			StatusIconTxt.Text = Status == Properties.Resources.WebsiteAvailable ? "\uF299" : "\uF36E"; // Set text
			StatusIconTxt.Foreground = Status == Properties.Resources.WebsiteAvailable ? new SolidColorBrush { Color = (Color)ColorConverter.ConvertFromString(App.Current.Resources["Green"].ToString()) } : new SolidColorBrush { Color = (Color)ColorConverter.ConvertFromString(App.Current.Resources["Red"].ToString()) };
		}

		private void DismissBtn_Click(object sender, RoutedEventArgs e)
		{
			StackPanel.Children.Remove(this); // Remove
			if (StackPanel.Children.Count == 0)
			{
				Global.DownDetectorPage.HistoryBtn_Click(this, null);
			}
		}
	}
}
