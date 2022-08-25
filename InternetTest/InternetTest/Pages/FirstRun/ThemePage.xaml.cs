using InternetTest.Classes;
using InternetTest.Enums;
using InternetTest.Windows;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

namespace InternetTest.Pages.FirstRun;
/// <summary>
/// Interaction logic for ThemePage.xaml
/// </summary>
public partial class ThemePage : Page
{
	private FirstRunWindow FirstRunWindow { get; init; }
	public ThemePage(FirstRunWindow firstRunWindow)
	{
		InitializeComponent();
		FirstRunWindow = firstRunWindow; // Set the first run window instance

		InitUI();
	}

	private void InitUI()
	{
		// Select the default theme border
		ThemeSelectedBorder = Global.Settings.Theme switch
		{
			Themes.Light => LightBorder,
			Themes.Dark => DarkBorder,
			_ => SystemBorder
		};
		Border_MouseEnter(Global.Settings.Theme switch
		{
			Themes.Light => LightBorder,
			Themes.Dark => DarkBorder,
			_ => SystemBorder
		}, null);
	}

	private void NextBtn_Click(object sender, RoutedEventArgs e)
	{
		FirstRunWindow.ChangePage(3);
	}

	Border ThemeSelectedBorder;
	private void Border_MouseEnter(object sender, MouseEventArgs e)
	{
		((Border)sender).BorderBrush = new SolidColorBrush { Color = Global.GetColorFromResource("AccentColor") };
	}

	private void Border_MouseLeave(object sender, MouseEventArgs e)
	{
		if ((Border)sender == ThemeSelectedBorder) return;
		((Border)sender).BorderBrush = new SolidColorBrush { Color = Colors.Transparent };
	}

	private void ResetBorders()
	{
		LightBorder.BorderBrush = new SolidColorBrush { Color = Colors.Transparent };
		DarkBorder.BorderBrush = new SolidColorBrush { Color = Colors.Transparent };
		SystemBorder.BorderBrush = new SolidColorBrush { Color = Colors.Transparent };
	}

	private void LightBorder_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
	{
		ResetBorders();
		ThemeSelectedBorder = (Border)sender;
		((Border)sender).BorderBrush = new SolidColorBrush { Color = Global.GetColorFromResource("AccentColor") };
		Global.Settings.Theme = Themes.Light;
		SettingsManager.Save();
	}

	private void DarkBorder_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
	{
		ResetBorders();
		ThemeSelectedBorder = (Border)sender;
		((Border)sender).BorderBrush = new SolidColorBrush { Color = Global.GetColorFromResource("AccentColor") };
		Global.Settings.Theme = Themes.Dark;
		SettingsManager.Save();
	}

	private void SystemBorder_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
	{
		ResetBorders();
		ThemeSelectedBorder = (Border)sender;
		((Border)sender).BorderBrush = new SolidColorBrush { Color = Global.GetColorFromResource("AccentColor") };
		Global.Settings.Theme = Themes.System;
		SettingsManager.Save();
	}
}
