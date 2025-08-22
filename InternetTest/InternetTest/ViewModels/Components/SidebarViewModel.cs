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
using System.Windows.Input;

namespace InternetTest.ViewModels.Components;

public class SidebarViewModel : ViewModelBase
{
	private readonly MainViewModel _mainViewModel;

	public ICommand HomePageCommand { get; }
	public ICommand SettingsPageCommand { get; }
	public ICommand WiFiPageCommand { get; }
	public ICommand LocateIpPageCommand { get; }
	public ICommand IpConfigPageCommand { get; }
	public ICommand PingPageCommand { get; }
	public ICommand RequestsPageCommand { get; }

	public SidebarViewModel(MainViewModel mainViewModel)
	{
		_mainViewModel = mainViewModel;

		HomePageCommand = new RelayCommand(HomePage);
		SettingsPageCommand = new RelayCommand(SettingsPage);
		WiFiPageCommand = new RelayCommand(WiFiPage);
		LocateIpPageCommand = new RelayCommand(LocateIpPage);
		IpConfigPageCommand = new RelayCommand(IpConfigPage);
		PingPageCommand = new RelayCommand(PingPage);
		RequestsPageCommand = new RelayCommand(RequestsPage);
	}

	private void HomePage(object? obj)
	{
		_mainViewModel.CurrentViewModel = new HomePageViewModel(_mainViewModel.Settings);
	}

	private void SettingsPage(object? obj)
	{
		_mainViewModel.CurrentViewModel = new SettingsPageViewModel(_mainViewModel);
	}

	private void WiFiPage(object? obj)
	{
		_mainViewModel.CurrentViewModel = new WiFiPageViewModel(_mainViewModel.Settings);
	}

	private void LocateIpPage(object? obj)
	{
		_mainViewModel.CurrentViewModel = new LocateIpPageViewModel(_mainViewModel.Settings);
	}

	private void IpConfigPage(object? obj)
	{
		_mainViewModel.CurrentViewModel = new IpConfigPageViewModel();
	}

	private void PingPage(object? obj)
	{
		_mainViewModel.CurrentViewModel = new PingPageViewModel(_mainViewModel.Settings);
	}

	private void RequestsPage(object? obj)
	{
		_mainViewModel.CurrentViewModel = new RequestsPageViewModel(_mainViewModel.Settings);
	}
}
