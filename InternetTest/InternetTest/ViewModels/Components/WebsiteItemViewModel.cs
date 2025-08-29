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
using PeyrSharp.Core;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Media;

namespace InternetTest.ViewModels.Components;
public class WebsiteItemViewModel : ViewModelBase
{
	private ObservableCollection<GridItemViewModel> _details = [];
	public ObservableCollection<GridItemViewModel> Details { get => _details; set { _details = value; OnPropertyChanged(nameof(Details)); } }

	private string _url = string.Empty;
	public string Url { get => _url; set { _url = value; OnPropertyChanged(nameof(Url)); } }

	private int _statusCode;
	public int StatusCode { get => _statusCode; set { _statusCode = value; OnPropertyChanged(nameof(StatusCode)); } }

	private SolidColorBrush? _statusForeground;
	public SolidColorBrush? StatusForeground { get => _statusForeground; set { _statusForeground = value; OnPropertyChanged(nameof(StatusForeground)); } }

	private SolidColorBrush? _statusBackground;
	public SolidColorBrush? StatusBackground { get => _statusBackground; set { _statusBackground = value; OnPropertyChanged(nameof(StatusBackground)); } }

	private bool _showStatusCode = false;
	public bool ShowStatusCode { get => _showStatusCode; set { _showStatusCode = value; OnPropertyChanged(nameof(ShowStatusCode)); } }

	public ICommand TestCommand => new RelayCommand(o => TestAsync());
	public ICommand DeleteCommand => new RelayCommand(o =>
	{
		_downDetectorPageViewModel.Websites.Remove(this);
	});

	private readonly DownDetectorPageViewModel _downDetectorPageViewModel;
	private readonly ActivityHistory _history;
	public WebsiteItemViewModel(string url, DownDetectorPageViewModel downDetectorPageViewModel, ActivityHistory history)
	{
		_downDetectorPageViewModel = downDetectorPageViewModel;
		_history = history;

		Url = url;
		StatusBackground = ThemeHelper.GetSolidColorBrush("LightAccent");
		StatusForeground = ThemeHelper.GetSolidColorBrush("DarkFAccent");

		Details = [
			new(Properties.Resources.StatusMessage, Properties.Resources.NA, 0, 0),
			new(Properties.Resources.TimeElapsed,  $"{Properties.Resources.NA}", 0, 1),
		];
	}

	internal async void TestAsync()
	{
		try
		{
			var startTime = DateTime.Now;
			var statusInfo = await Internet.GetStatusInfoAsync(Url);
			var endTime = DateTime.Now;

			StatusCode = statusInfo?.StatusCode ?? 400;

			StatusBackground = StatusCode switch
			{
				>= 400 => ThemeHelper.GetSolidColorBrush("LightOrange"),
				>= 300 => ThemeHelper.GetSolidColorBrush("DarkFAccent"),
				_ => ThemeHelper.GetSolidColorBrush("LightGreen"),
			};

			StatusForeground = StatusCode switch
			{
				>= 400 => ThemeHelper.GetSolidColorBrush("ForegroundOrange"),
				>= 300 => ThemeHelper.GetSolidColorBrush("LightAccent"),
				_ => ThemeHelper.GetSolidColorBrush("ForegroundGreen"),
			};

			Details = [
				new(Properties.Resources.StatusMessage, statusInfo?.StatusDescription ?? Properties.Resources.Failed, 0, 0),
				new(Properties.Resources.TimeElapsed,  $"{(endTime - startTime).TotalMilliseconds:0} ms", 0,1 ),
			];

			_history.Activity.Add(new Activity(Url, (StatusCode).ToString(), StatusCode switch
			{
				>= 400 => false,
				>= 300 or <= 100 => null,
				_ => true,
			}, DateTime.Now));
		}
		catch
		{
			StatusCode = 400;

			StatusBackground = StatusCode switch
			{
				>= 400 => ThemeHelper.GetSolidColorBrush("LightOrange"),
				>= 300 => ThemeHelper.GetSolidColorBrush("DarkFAccent"),
				_ => ThemeHelper.GetSolidColorBrush("LightGreen"),
			};

			StatusForeground = StatusCode switch
			{
				>= 400 => ThemeHelper.GetSolidColorBrush("ForegroundOrange"),
				>= 300 => ThemeHelper.GetSolidColorBrush("LightAccent"),
				_ => ThemeHelper.GetSolidColorBrush("ForegroundGreen"),
			};
		}
		ShowStatusCode = true;
	}

	public override bool Equals(object? obj)
	{
		return base.Equals(obj) && obj is WebsiteItemViewModel wvm && wvm.Url == Url;
	}

	public override int GetHashCode()
	{
		return base.GetHashCode() ^ Url.GetHashCode();
	}
}
