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
using InternetTest.Models;
using InternetTest.ViewModels.Windows;
using System.Windows.Input;

namespace InternetTest.ViewModels.Oobe;

public class ThemePageViewModel : ViewModelBase
{
	private readonly OobeWindowViewModel _oobe;
	private readonly Settings _settings;

	private Theme _currentTheme = Theme.System;
	public Theme CurrentTheme
	{
		get => _currentTheme; set
		{
			_currentTheme = value;
			_settings.Theme = value;
			_settings.Save();
			ThemeHelper.ChangeTheme(value);

			OnPropertyChanged(nameof(CurrentTheme));
		}
	}

	public ICommand NextCommand => new RelayCommand(o =>
	{
		//_oobe.CurrentViewModel = new FinalPageViewModel(_oobe);
	});
	public ThemePageViewModel(OobeWindowViewModel oobe)
	{
		_oobe = oobe;
		_settings = _oobe.Settings;
		CurrentTheme = _settings.Theme;
	}
}
