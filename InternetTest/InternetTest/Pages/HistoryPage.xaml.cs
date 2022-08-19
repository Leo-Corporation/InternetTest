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

namespace InternetTest.Pages;
/// <summary>
/// Interaction logic for HistoryPage.xaml
/// </summary>
public partial class HistoryPage : Page
{
	public HistoryPage()
	{
		InitializeComponent();
		Loaded += (o, e) => InitUI();
		
		CheckButton(StatusBtn); // Check the default button
		InitUI();
	}

	private void InitUI()
	{
		// Clear
		StatusHistory.Children.Clear();
		DownDetectorHistory.Children.Clear();
		
		// Load Status history
		for (int i = Global.History.StatusHistory.Count - 1; i >= 0; i--)
		{
			StatusHistory.Children.Add(new StatusHistoryItem(Global.History.StatusHistory[i], StatusHistory, Enums.AppPages.Status));
		}

		// Load DownDetector history
		for (int i = Global.History.DownDetectorHistory.Count - 1; i >= 0; i--)
		{
			DownDetectorHistory.Children.Add(new StatusHistoryItem(Global.History.DownDetectorHistory[i], DownDetectorHistory, Enums.AppPages.DownDetector));
		}
	}

	private void UnCheckAllButtons()
	{
		StatusBtn.Background = new SolidColorBrush { Color = Colors.Transparent };
		DownDetectorBtn.Background = new SolidColorBrush { Color = Colors.Transparent };

		StatusHistory.Visibility = Visibility.Collapsed;
		DownDetectorHistory.Visibility = Visibility.Collapsed;
	}

	private void CheckButton(Button button) => button.Background = new SolidColorBrush { Color = Global.GetColorFromResource("LightAccentColor") };

	private void StatusBtn_Click(object sender, RoutedEventArgs e)
	{
		UnCheckAllButtons();
		CheckButton(StatusBtn);
		StatusHistory.Visibility = Visibility.Visible;
	}

	private void DownDetectorBtn_Click(object sender, RoutedEventArgs e)
	{
		UnCheckAllButtons();
		CheckButton(DownDetectorBtn);
		DownDetectorHistory.Visibility = Visibility.Visible;
	}
}
