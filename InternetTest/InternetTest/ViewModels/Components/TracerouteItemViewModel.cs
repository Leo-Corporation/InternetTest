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
using System.Net;
using System.Net.NetworkInformation;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace InternetTest.ViewModels.Components;

public class TracerouteItemViewModel : ViewModelBase
{
	private string _host = string.Empty;
	public string Host { get => _host; set { _host = value; OnPropertyChanged(nameof(Host)); } }

	private string _duration = string.Empty;
	public string Duration { get => _duration; set { _duration = value; OnPropertyChanged(nameof(Duration)); } }

	private int _index = -1;
	public int Index { get => _index; set { _index = value; OnPropertyChanged(nameof(Index)); } }

	private SolidColorBrush? _foregroundBrush;
	public SolidColorBrush? ForegroundBrush { get => _foregroundBrush; set { _foregroundBrush = value; OnPropertyChanged(nameof(ForegroundBrush)); } }

	private SolidColorBrush? _backgroundBrush;
	public SolidColorBrush? BackgroundBrush { get => _backgroundBrush; set { _backgroundBrush = value; OnPropertyChanged(nameof(BackgroundBrush)); } }

	public ICommand CopyCommand => new RelayCommand(o =>
	{
		Clipboard.SetDataObject(Host);
	});

	public TracerouteItemViewModel(TracerouteStep tracerouteStep)
	{
		Host = tracerouteStep.Address?.ToString() ?? Properties.Resources.Unknown;
		if (tracerouteStep.Address != null)
		{
			LoadHostName(tracerouteStep.Address);
		}

		if (tracerouteStep.Status == IPStatus.TimedOut) Host = Properties.Resources.TimedOut;

		Duration = tracerouteStep.RoundtripTime >= 0 ? $"{tracerouteStep.RoundtripTime} ms" : "N/A";
		Index = tracerouteStep.TTL;
		ForegroundBrush = tracerouteStep.Status is IPStatus.TtlExpired or IPStatus.Success ? ThemeHelper.GetSolidColorBrush("ForegroundGreen") : ThemeHelper.GetSolidColorBrush("ForegroundOrange");
		BackgroundBrush = tracerouteStep.Status is IPStatus.TtlExpired or IPStatus.Success ? ThemeHelper.GetSolidColorBrush("LightGreen") : ThemeHelper.GetSolidColorBrush("LightOrange");
	}

	private async void LoadHostName(IPAddress address)
	{
		try
		{
			var hostEntry = await Dns.GetHostEntryAsync(address);
			if (hostEntry.HostName != address.ToString())
			{
				Host += $" ({hostEntry.HostName})";
			}
		}
		catch { }
	}
}
