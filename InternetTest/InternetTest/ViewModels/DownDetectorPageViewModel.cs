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

namespace InternetTest.ViewModels;
public class DownDetectorPageViewModel : ViewModelBase
{
	private ObservableCollection<WebsiteItemViewModel> _websites = [];
	public ObservableCollection<WebsiteItemViewModel> Websites { get => _websites; set { _websites = value; OnPropertyChanged(nameof(Websites)); } }

	private string _site = string.Empty;
	public string Site { get => _site; set { _site = value; OnPropertyChanged(nameof(Site)); } }

	private int _timeInterval = 10;
	public int TimeInterval { get => _timeInterval; set { _timeInterval = value; OnPropertyChanged(nameof(TimeInterval)); } }

	private bool _isTesting = false;
	public bool IsTesting { get => _isTesting; set { _isTesting = value; OnPropertyChanged(nameof(IsTesting)); } }
	public bool HasWebsites => Websites != null && Websites.Count > 0;
	public ICommand AddWebsiteCommand => new RelayCommand(o => {
		if (string.IsNullOrEmpty(Site)) return;
		if ((!Site.StartsWith("https://") && !Site.StartsWith("http://")) || (!Site.StartsWith("http://") && !Site.StartsWith("https://")))
		{
			Site = $"{(_settings.UseHttps ? "https://" : "http://")}{Site}"; // default to https
		}

		if (Websites.Any(x => x.Url == Site)) return; // already exists

		Websites.Add(new WebsiteItemViewModel(Site, this));
	});

	public ICommand TestWebsitesCommand => new RelayCommand(o => {
		IsTesting = true;
		foreach (var site in Websites)
		{
			site.TestAsync();
		}
		IsTesting = false;
	});

	public ICommand LaunchScheduledCommand => new RelayCommand(o =>
	{
		IsTesting = true;
		//TODO: Implement scheduled testing with a timer
		IsTesting = false;
	});

	public ICommand ClearCommand => new RelayCommand(o => Websites.Clear());

	private readonly Settings _settings;
	public DownDetectorPageViewModel(Settings settings)
	{
		_settings = settings;
		TimeInterval = _settings.DefaultTimeInterval ?? 10;
		Websites.CollectionChanged += (s, e) => { OnPropertyChanged(nameof(HasWebsites)); };
	}
}
