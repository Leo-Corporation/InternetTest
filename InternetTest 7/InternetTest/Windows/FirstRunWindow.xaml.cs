﻿/*
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
using InternetTest.Pages.FirstRun;
using System.Windows;

namespace InternetTest.Windows;
/// <summary>
/// Interaction logic for FirstRunWindow.xaml
/// </summary>
public partial class FirstRunWindow : Window
{
	internal WelcomePage welcomePage;
	internal FeaturesPage featuresPage;
	internal ThemePage themePage;
	internal SynethiaPage synethiaPage;
	internal JumpInPage jumpInPage = new();
	public FirstRunWindow()
	{
		InitializeComponent();
		welcomePage = new(this);
		featuresPage = new(this);
		themePage = new(this);
		synethiaPage = new(this);
		ChangePage(0);
	}

	internal void ChangePage(int pageID)
	{
		WindowFrame.Content = pageID switch
		{
			0 => welcomePage,
			1 => featuresPage,
			2 => themePage,
			3 => synethiaPage,
			4 => jumpInPage,
			_ => welcomePage
		};
	}

	private void CloseBtn_Click(object sender, RoutedEventArgs e)
	{
		if (MessageBox.Show(Properties.Resources.FirstRunQuitMsg, Properties.Resources.InternetTestPro, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
		{
			new MainWindow().Show();
			Global.Settings.IsFirstRun = false;
			Close();
		}
		else
		{
			Application.Current.Shutdown();
		}
	}
}
