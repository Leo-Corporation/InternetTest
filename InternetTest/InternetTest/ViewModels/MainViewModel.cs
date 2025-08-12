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
using InternetTest.Helpers;
using InternetTest.Models;
using InternetTest.ViewModels.Components;
using System.Windows;
using System.Windows.Input;

namespace InternetTest.ViewModels;



public class MainViewModel : ViewModelBase
{
	private SidebarViewModel _sidebarViewModel;
	public SidebarViewModel SidebarViewModel
	{
		get { return _sidebarViewModel; }
		set { _sidebarViewModel = value; OnPropertyChanged(nameof(SidebarViewModel)); }
	}

	private object? _currentView;
	public object? CurrentViewModel
	{
		get { return _currentView; }
		set { _currentView = value; OnPropertyChanged(nameof(CurrentViewModel)); }
	}

	public Settings Settings { get; set; }

	public string Version => Context.Version;

	private bool _pinned;
	private readonly Window _mainWindow;

	public ICommand PinCommand { get; }

	public bool Pinned
	{
		get => _pinned;
		set
		{
			_pinned = value;
			_mainWindow.Topmost = value;

			OnPropertyChanged(nameof(Pinned));
		}
	}

	public MainViewModel(Settings settings, Window mainWindow)
	{
		_sidebarViewModel = new(this);
		Settings = settings;
		_mainWindow = mainWindow;

		if (Settings.RememberPinnedState == true)
		{
			Pinned = Settings.Pinned ?? false;
		}
		else
		{
			Pinned = false;
		}

		PinCommand = new RelayCommand(Pin);
	}

	public void Pin(object? obj)
	{
		Pinned = !Pinned;

		if (Settings.RememberPinnedState == true)
		{
			Settings.Pinned = Pinned;
			Settings.Save();
		}
	}
}
