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
using System.Windows.Input;
using System.Windows.Media;

namespace InternetTest.Pages.FirstRunPages;

/// <summary>
/// Interaction logic for ThemePage.xaml
/// </summary>
public partial class ThemePage : Page
{
	Border CheckedBorder { get; set; }

	public ThemePage()
	{
		InitializeComponent();
		InitUI();
	}

	private void InitUI()
	{
		CheckedBorder = SystemBorder;
		SystemRadioBtn.IsChecked = true;
		RefreshBorders();
		ThemeApplyBtn.Visibility = Visibility.Collapsed;
	}

	private void RefreshBorders()
	{
		LightBorder.BorderBrush = new SolidColorBrush() { Color = Colors.Transparent }; // Set color 
		DarkBorder.BorderBrush = new SolidColorBrush() { Color = Colors.Transparent }; // Set color 
		SystemBorder.BorderBrush = new SolidColorBrush() { Color = Colors.Transparent }; // Set color 

		CheckedBorder.BorderBrush = new SolidColorBrush() { Color = (Color)ColorConverter.ConvertFromString(App.Current.Resources["AccentColor"].ToString()) }; // Set color
	}

	private void Border_MouseEnter(object sender, MouseEventArgs e)
	{
		Border border = (Border)sender;
		border.BorderBrush = new SolidColorBrush() { Color = (Color)ColorConverter.ConvertFromString(App.Current.Resources["AccentColor"].ToString()) }; // Set color

	}

	private void Border_MouseLeave(object sender, MouseEventArgs e)
	{
		Border border = (Border)sender;
		if (border != CheckedBorder)
		{
			border.BorderBrush = new SolidColorBrush() { Color = Colors.Transparent }; // Set color 
		}
	}

	private void LightBorder_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
	{
		LightRadioBtn.IsChecked = true; // Set IsChecked
		CheckedBorder = LightBorder; // Set
		RefreshBorders();
	}

	private void DarkBorder_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
	{
		DarkRadioBtn.IsChecked = true; // Set IsChecked
		CheckedBorder = DarkBorder; // Set
		RefreshBorders();
	}

	private void SystemBorder_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
	{
		SystemRadioBtn.IsChecked = true; // Set IsChecked
		CheckedBorder = SystemBorder; // Set
		RefreshBorders();
	}

	private void LightRadioBtn_Checked(object sender, RoutedEventArgs e)
	{
		ThemeApplyBtn.Visibility = Visibility.Visible; // Show the ThemeApplyBtn button
	}

	private void DarkRadioBtn_Checked(object sender, RoutedEventArgs e)
	{
		ThemeApplyBtn.Visibility = Visibility.Visible; // Show the ThemeApplyBtn button
	}

	private void SystemRadioBtn_Checked(object sender, RoutedEventArgs e)
	{
		ThemeApplyBtn.Visibility = Visibility.Visible; // Show the ThemeApplyBtn button
	}

	private void ThemeApplyBtn_Click(object sender, RoutedEventArgs e)
	{
		Global.Settings.IsDarkTheme = DarkRadioBtn.IsChecked.Value; // Set the settings
		Global.Settings.IsThemeSystem = SystemRadioBtn.IsChecked.Value; // Set the settings
		SettingsManager.Save(); // Save the changes
		ThemeApplyBtn.Visibility = Visibility.Hidden; // Hide
	}
}
