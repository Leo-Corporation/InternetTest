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
using InternetTest.Enums;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace InternetTest
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private Button CheckedButton { get; set; }

		readonly ColorAnimation colorAnimation = new()
		{
			From = (Color)ColorConverter.ConvertFromString(App.Current.Resources["AccentColor"].ToString()),
			To = (Color)ColorConverter.ConvertFromString(App.Current.Resources["Background1"].ToString()),
			Duration = new(TimeSpan.FromSeconds(0.2d))
		};
		public MainWindow()
		{
			InitializeComponent();
			InitUI(); // Init the UI
		}

		private void InitUI()
		{
			HelloTxt.Text = Global.GetHiSentence; // Set the "Hello" message
			PageContent.Content = Global.Settings.StartupPage switch
			{
				StartPages.Connection => Global.ConnectionPage,
				StartPages.LocalizeIP => Global.LocalizeIPPage,
				StartPages.DownDetector => Global.DownDetectorPage,
				_ => Global.ConnectionPage
			}; // Go to the default startup page

			CheckButton(Global.Settings.StartupPage switch
			{
				StartPages.Connection => ConnectionBtn,
				StartPages.LocalizeIP => LocalizeIPBtn,
				StartPages.DownDetector => DownDetectorBtn,
				_ => ConnectionBtn
			}); // Check the start page button
		}

		private void CheckButton(Button button)
		{
			button.Foreground = new SolidColorBrush { Color = (Color)ColorConverter.ConvertFromString(App.Current.Resources["WindowButtonsHoverForeground1"].ToString()) }; // Set the foreground
			button.Background = new SolidColorBrush { Color = (Color)ColorConverter.ConvertFromString(App.Current.Resources["AccentColor"].ToString()) }; // Set the background

			CheckedButton = button; // Set the "checked" button
		}

		private void ResetAllCheckStatus()
		{
			ConnectionBtn.Foreground = new SolidColorBrush { Color = (Color)ColorConverter.ConvertFromString(App.Current.Resources["Foreground1"].ToString()) }; // Set the foreground
			ConnectionBtn.Background = new SolidColorBrush { Color = (Color)ColorConverter.ConvertFromString(App.Current.Resources["Background1"].ToString()) }; // Set the background

			LocalizeIPBtn.Foreground = new SolidColorBrush { Color = (Color)ColorConverter.ConvertFromString(App.Current.Resources["Foreground1"].ToString()) }; // Set the foreground
			LocalizeIPBtn.Background = new SolidColorBrush { Color = (Color)ColorConverter.ConvertFromString(App.Current.Resources["Background1"].ToString()) }; // Set the background

			DownDetectorBtn.Foreground = new SolidColorBrush { Color = (Color)ColorConverter.ConvertFromString(App.Current.Resources["Foreground1"].ToString()) }; // Set the foreground
			DownDetectorBtn.Background = new SolidColorBrush { Color = (Color)ColorConverter.ConvertFromString(App.Current.Resources["Background1"].ToString()) }; // Set the background

			SettingsTabBtn.Foreground = new SolidColorBrush { Color = (Color)ColorConverter.ConvertFromString(App.Current.Resources["Foreground1"].ToString()) }; // Set the foreground
			SettingsTabBtn.Background = new SolidColorBrush { Color = (Color)ColorConverter.ConvertFromString(App.Current.Resources["Background1"].ToString()) }; // Set the background
		}

		private void MinimizeBtn_Click(object sender, RoutedEventArgs e)
		{
			WindowState = WindowState.Minimized; // Minimize window
		}

		private void CloseBtn_Click(object sender, RoutedEventArgs e)
		{
			Environment.Exit(0); // Exit
		}

		private void TabEnter(object sender, MouseEventArgs e)
		{
			Button button = (Button)sender; // Create button

			button.Foreground = new SolidColorBrush { Color = (Color)ColorConverter.ConvertFromString(App.Current.Resources["WindowButtonsHoverForeground1"].ToString()) }; // Set the foreground
		}

		private void TabLeave(object sender, MouseEventArgs e)
		{
			Button button = (Button)sender; // Create button

			if (button != CheckedButton)
			{
				button.Foreground = new SolidColorBrush { Color = (Color)ColorConverter.ConvertFromString(App.Current.Resources["Foreground1"].ToString()) }; // Set the foreground 
				button.Background.BeginAnimation(SolidColorBrush.ColorProperty, colorAnimation); // Play animation
			}
		}

		private void ConnectionBtn_Click(object sender, RoutedEventArgs e)
		{
			ResetAllCheckStatus(); // Reset the background and foreground of all buttons
			CheckButton(ConnectionBtn); // Check the "Settings" button

			PageContent.Navigate(Global.ConnectionPage); // Navigate
		}

		private void LocalizeIPBtn_Click(object sender, RoutedEventArgs e)
		{
			ResetAllCheckStatus(); // Reset the background and foreground of all buttons
			CheckButton(LocalizeIPBtn); // Check the "Settings" button

			PageContent.Navigate(Global.LocalizeIPPage); // Navigate
		}

		private void SettingsTabBtn_Click(object sender, RoutedEventArgs e)
		{
			ResetAllCheckStatus(); // Reset the background and foreground of all buttons
			CheckButton(SettingsTabBtn); // Check the "Settings" button

			PageContent.Navigate(Global.SettingsPage); // Navigate
		}

		private void DownDetectorBtn_Click(object sender, RoutedEventArgs e)
		{
			ResetAllCheckStatus(); // Reset the background and foreground of all buttons
			CheckButton(DownDetectorBtn); // Check the "Settings" button

			PageContent.Navigate(Global.DownDetectorPage); // Navigate
		}
	}
}
