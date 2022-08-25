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
using InternetTest.Windows;
using LeoCorpLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
/// Interaction logic for WelcomePage.xaml
/// </summary>
public partial class WelcomePage : Page
{
	private FirstRunWindow FirstRunWindow { get; init; }
	public WelcomePage(FirstRunWindow firstRunWindow)
	{
		InitializeComponent();
		FirstRunWindow = firstRunWindow;
		LangComboBox.SelectedIndex = (int)Global.Settings.Language;
	}

	private void NextBtn_Click(object sender, RoutedEventArgs e)
	{
		FirstRunWindow.ChangePage(1);
	}

	private void SkipBtn_Click(object sender, RoutedEventArgs e)
	{
		Global.Settings.IsFirstRun = false;
		SettingsManager.Save();

		Process.Start(Env.CurrentAppDirectory + @"\InternetTest.exe");
		Application.Current.Shutdown();
	}

	private void LangComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		Global.Settings.Language = (Languages)LangComboBox.SelectedIndex;
	}
}
