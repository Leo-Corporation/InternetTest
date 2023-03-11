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
using PeyrSharp.Core.Converters;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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

		HistoryGrid.Children.Add(Placeholder);
		CheckButton(StatusBtn); // Check the default button
		InitUI();
	}

	Placeholder Placeholder = new(Properties.Resources.HistoryEmpty, "\uF47F");
	internal void InitUI()
	{
		// Clear
		StatusHistory.Children.Clear();
		DownDetectorHistory.Children.Clear();
		Placeholder.Visibility = Visibility.Visible;

		// Load Status history
		for (int i = Global.History.StatusHistory.Count - 1; i >= 0; i--)
		{
			StatusHistory.Children.Add(new StatusHistoryItem(Global.History.StatusHistory[i], Enums.AppPages.Status));
		}

		// Load DownDetector history
		for (int i = Global.History.DownDetectorHistory.Count - 1; i >= 0; i--)
		{
			DownDetectorHistory.Children.Add(new StatusHistoryItem(Global.History.DownDetectorHistory[i], Enums.AppPages.DownDetector));
		}

		if (StatusHistory.Children.Count == 0)
		{
			Placeholder.Visibility = Visibility.Visible;
			EmptyHistoryBtn.Visibility = Visibility.Collapsed;
		}
		else
		{
			Placeholder.Visibility = Visibility.Collapsed;
			EmptyHistoryBtn.Visibility = Visibility.Visible;
		}

		SearchTxt_TextChanged(this, null);
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

		if (StatusHistory.Children.Count == 0)
		{
			Placeholder.Visibility = Visibility.Visible;
			EmptyHistoryBtn.Visibility = Visibility.Collapsed;
		}
		else
		{
			Placeholder.Visibility = Visibility.Collapsed;
			EmptyHistoryBtn.Visibility = Visibility.Visible;
		}
		SearchTxt.Text = "";
		SearchTxt_TextChanged(this, null);
	}

	private void DownDetectorBtn_Click(object sender, RoutedEventArgs e)
	{
		UnCheckAllButtons();
		CheckButton(DownDetectorBtn);
		DownDetectorHistory.Visibility = Visibility.Visible;

		if (DownDetectorHistory.Children.Count == 0)
		{
			Placeholder.Visibility = Visibility.Visible;
			EmptyHistoryBtn.Visibility = Visibility.Collapsed;
		}
		else
		{
			Placeholder.Visibility = Visibility.Collapsed;
			EmptyHistoryBtn.Visibility = Visibility.Visible;
		}
		SearchTxt.Text = "";
		SearchTxt_TextChanged(this, null);
	}

	private void EmptyHistoryBtn_Click(object sender, RoutedEventArgs e)
	{
		// If the user doesn't want to empty the history anymore, stop here.
		if (MessageBox.Show(Properties.Resources.EmptyHistoryMsg, Properties.Resources.EmptyHistory, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
			return;

		// Get the current selected history
		if (StatusHistory.Visibility == Visibility.Visible)
		{
			Global.History.StatusHistory.Clear(); // Empty history
		}
		else if (DownDetectorBtn.Visibility == Visibility.Visible)
		{
			Global.History.DownDetectorHistory.Clear(); // Empty history
		}

		InitUI(); // Refresh the UI
	}

	private void SearchTxt_TextChanged(object sender, TextChangedEventArgs e)
	{
		if (FilterComboBox.SelectedIndex == 1 && FromDatePicker.SelectedDate is not null && ToDatePicker.SelectedDate is not null)
		{
			SearchHistory(SearchTxt.Text, FromDatePicker.SelectedDate.Value, ToDatePicker.SelectedDate.Value);
			return;
		}
		if (FilterComboBox.SelectedIndex == 2)
		{
			SearchHistory(SearchTxt.Text, SuccessfulRadioBtn.IsChecked ?? true);
			return;
		}
		SearchHistory(SearchTxt.Text);
	}

	internal void SearchHistory(string req)
	{
		if (StatusHistory.Visibility == Visibility.Visible) // Search in "Status" history
		{
			for (int i = 0; i < StatusHistory.Children.Count; i++)
			{
				var item = (StatusHistoryItem)StatusHistory.Children[i];
				item.Visibility = item.ContentTxt.Text.Contains(req) ? Visibility.Visible : Visibility.Collapsed;
			}
		}
		else // Search in "DownDetector" history
		{
			for (int i = 0; i < DownDetectorHistory.Children.Count; i++)
			{
				var item = (StatusHistoryItem)DownDetectorHistory.Children[i];
				item.Visibility = item.ContentTxt.Text.Contains(req) ? Visibility.Visible : Visibility.Collapsed;
			}
		}
	}

	internal void SearchHistory(string req, bool sucessful)
	{
		if (StatusHistory.Visibility == Visibility.Visible) // Search in "Status" history
		{
			for (int i = 0; i < StatusHistory.Children.Count; i++)
			{
				var item = (StatusHistoryItem)StatusHistory.Children[i];
				item.Visibility = item.ContentTxt.Text.Contains(req) && ((StatusHistory)item.HistoryItem).Status == sucessful ? Visibility.Visible : Visibility.Collapsed;
			}
		}
		else // Search in "DownDetector" history
		{
			for (int i = 0; i < DownDetectorHistory.Children.Count; i++)
			{
				var item = (StatusHistoryItem)DownDetectorHistory.Children[i];
				item.Visibility = item.ContentTxt.Text.Contains(req) && Global.IsSuccessfulCode(((DownHistory)item.HistoryItem).StatusCode) == sucessful ? Visibility.Visible : Visibility.Collapsed;
			}
		}
	}

	internal void SearchHistory(string req, DateTime start, DateTime end)
	{
		// Convert to Unix Time
		int s = Time.DateTimeToUnixTime(start);
		int e = Time.DateTimeToUnixTime(end) + 24 * 3600;

		if (e - 24 * 3600 < s) MessageBox.Show(Properties.Resources.InvalidDateMsg, Properties.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);

		if (StatusHistory.Visibility == Visibility.Visible) // Search in "Status" history
		{
			for (int i = 0; i < StatusHistory.Children.Count; i++)
			{
				var item = (StatusHistoryItem)StatusHistory.Children[i];

				item.Visibility = item.ContentTxt.Text.Contains(req) && Global.DateIsInRange(s, e, item.HistoryItem.Date) ? Visibility.Visible : Visibility.Collapsed;
			}
		}
		else // Search in "DownDetector" history
		{
			for (int i = 0; i < DownDetectorHistory.Children.Count; i++)
			{
				var item = (StatusHistoryItem)DownDetectorHistory.Children[i];
				item.Visibility = item.ContentTxt.Text.Contains(req) && Global.DateIsInRange(s, e, item.HistoryItem.Date) ? Visibility.Visible : Visibility.Collapsed;
			}
		}
	}

	private void DismissBtn_Click(object sender, RoutedEventArgs e)
	{
		SearchTxt.Text = "";
	}

	private void FilterBtn_Click(object sender, RoutedEventArgs e)
	{
		FilterPopup.IsOpen = true;
	}

	private void FilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		try
		{
			DateFilters.Visibility = (FilterComboBox.SelectedIndex == 1) ? Visibility.Visible : Visibility.Collapsed;
			StateFilters.Visibility = (FilterComboBox.SelectedIndex == 2) ? Visibility.Visible : Visibility.Collapsed;
			SearchTxt_TextChanged(this, null);
		}
		catch { }
	}

	private void DateApplyBtn_Click(object sender, RoutedEventArgs e)
	{
		if (FromDatePicker.SelectedDate is not null && ToDatePicker.SelectedDate is not null)
		{
			SearchHistory(SearchTxt.Text, FromDatePicker.SelectedDate.Value, ToDatePicker.SelectedDate.Value);
			return;
		}
		MessageBox.Show(Properties.Resources.InvalidDateMsg, Properties.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
	}

	private void SuccessfulRadioBtn_Checked(object sender, RoutedEventArgs e)
	{
		try
		{
			SearchHistory(SearchTxt.Text, SuccessfulRadioBtn.IsChecked ?? true);
		}
		catch { }
	}

	private void FailRadioBtn_Checked(object sender, RoutedEventArgs e)
	{
		SearchHistory(SearchTxt.Text, SuccessfulRadioBtn.IsChecked ?? true);
	}
}
