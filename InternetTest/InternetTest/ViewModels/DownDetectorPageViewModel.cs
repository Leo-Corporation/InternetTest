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
using InternetTest.Models;
using InternetTest.ViewModels.Components;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Threading;

namespace InternetTest.ViewModels;

public class DownDetectorPageViewModel : ViewModelBase
{
	private ObservableCollection<WebsiteItemViewModel> _websites = [];
	public ObservableCollection<WebsiteItemViewModel> Websites { get => _websites; set { _websites = value; OnPropertyChanged(nameof(Websites)); } }

	private string _site = string.Empty;
	public string Site { get => _site; set { _site = value; OnPropertyChanged(nameof(Site)); } }

	private string _scheduledText = string.Empty;
	public string ScheduledText { get => _scheduledText; set { _scheduledText = value; OnPropertyChanged(nameof(ScheduledText)); } }

	private string _scheduledButtonText = Properties.Resources.LaunchScheduledTest;
	public string ScheduledButtonText { get => _scheduledButtonText; set { _scheduledButtonText = value; OnPropertyChanged(nameof(ScheduledButtonText)); } }

	private int _timeInterval = 10;
	public int TimeInterval { get => _timeInterval; set { _timeInterval = value; OnPropertyChanged(nameof(TimeInterval)); } }

	private bool _isTesting = false;
	public bool IsTesting { get => _isTesting; set { _isTesting = value; OnPropertyChanged(nameof(IsTesting)); } }

	private bool _isScheduledInProgress = false;
	public bool IsScheduledInProgress { get => _isScheduledInProgress; set { _isScheduledInProgress = value; OnPropertyChanged(nameof(IsScheduledInProgress)); } }

	public bool HasWebsites => Websites != null && Websites.Count > 0;

	private DispatcherTimer? _timer;
	public ICommand AddWebsiteCommand => new RelayCommand(o =>
	{
		if (string.IsNullOrEmpty(Site)) return;
		if ((!Site.StartsWith("https://") && !Site.StartsWith("http://")) || (!Site.StartsWith("http://") && !Site.StartsWith("https://")))
		{
			Site = $"{(_settings.UseHttps ? "https://" : "http://")}{Site}"; // default to https
		}

		if (Websites.Any(x => x.Url == Site)) return; // already exists

		Websites.Add(new WebsiteItemViewModel(Site, this, _history));
		Site = string.Empty;
	});

	public ICommand TestWebsitesCommand => new RelayCommand(o =>
	{
		IsTesting = true;
		foreach (var site in Websites)
		{
			site.TestAsync();
		}
		IsTesting = false;
		_history.Save();
	});

	public ICommand LaunchScheduledCommand => new RelayCommand(o =>
	{
		if (Websites.Count == 0) return;
		IsScheduledInProgress = !IsScheduledInProgress;
		ScheduledText = string.Format(Properties.Resources.ScheduledTestInterval, TimeInterval);

		int i = 0;
		if (_timer is null)
		{
			_timer = new DispatcherTimer
			{
				Interval = TimeSpan.FromSeconds(1)
			};
			_timer.Tick += (s, e) =>
			{
				if (i < TimeInterval)
				{
					i++;
					ScheduledText = string.Format(Properties.Resources.ScheduledTestInterval, TimeInterval - i);

					return;
				}
				foreach (var site in Websites)
				{
					site.TestAsync();
				}
				i = 0;
			};
		}

		if (!IsScheduledInProgress)
		{
			_timer.Stop();
			_timer = null;
			IsTesting = false;
			ScheduledButtonText = Properties.Resources.LaunchScheduledTest;
		}
		else
		{
			_timer.Start();
			IsTesting = true;
			ScheduledButtonText = Properties.Resources.StopScheduledTests;
		}
	});

	public ICommand ClearCommand => new RelayCommand(o => Websites.Clear());

	private readonly Settings _settings;
	private readonly ActivityHistory _history;
	public DownDetectorPageViewModel(Settings settings, ActivityHistory history)
	{
		_settings = settings;
		_history = history;
		TimeInterval = _settings.DefaultTimeInterval ?? 10;
		Websites = [.. _settings.DownDetectorWebsites?.Select(x => new WebsiteItemViewModel(x, this, _history)) ?? []];
		Websites.CollectionChanged += (s, e) =>
		{
			_settings.DownDetectorWebsites = [.. Websites.Select(x => x.Url)];
			_settings.Save();
			OnPropertyChanged(nameof(HasWebsites));
		};
	}
}
