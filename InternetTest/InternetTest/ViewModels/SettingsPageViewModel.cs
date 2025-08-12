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
using InternetTest.Commands;
using InternetTest.Enums;
using InternetTest.Helpers;
using System.Windows.Input;

namespace InternetTest.ViewModels;
public class SettingsPageViewModel : ViewModelBase
{
	private readonly MainViewModel _mainViewModel;

	public ICommand LightThemeCommand { get; }
	public ICommand DarkThemeCommand { get; }
	public ICommand SystemThemeCommand { get; }

	private Theme _currentTheme;
	public Theme CurrentTheme { get => _currentTheme; set { _currentTheme = value; OnPropertyChanged(nameof(CurrentTheme)); } }

	public SettingsPageViewModel(MainViewModel mainViewModel)
	{
		_mainViewModel = mainViewModel;

		CurrentTheme = mainViewModel.Settings.Theme;

		LightThemeCommand = new RelayCommand(LightTheme);
		DarkThemeCommand = new RelayCommand(DarkTheme);
		SystemThemeCommand = new RelayCommand(SystemTheme);
	}

	private void LightTheme(object? obj)
	{
		_mainViewModel.Settings.Theme = Theme.Light;
		_mainViewModel.Settings.Save(); // Save settings after changing theme

		ThemeHelper.ChangeTheme(Theme.Light);
	}

	private void DarkTheme(object? obj)
	{
		_mainViewModel.Settings.Theme = Theme.Dark;
		_mainViewModel.Settings.Save(); // Save settings after changing theme

		ThemeHelper.ChangeTheme(Theme.Dark);
	}

	private void SystemTheme(object? obj)
	{
		_mainViewModel.Settings.Theme = Theme.System;
		_mainViewModel.Settings.Save(); // Save settings after changing theme

		ThemeHelper.ChangeTheme(Theme.System);
	}
}
