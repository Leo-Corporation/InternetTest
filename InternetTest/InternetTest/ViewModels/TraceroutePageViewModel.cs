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
using System.Net.NetworkInformation;
using System.Windows;
using System.Windows.Input;

namespace InternetTest.ViewModels;
public class TraceroutePageViewModel : ViewModelBase
{
	private ObservableCollection<TracerouteItemViewModel> _tracerouteItems = [];
	public ObservableCollection<TracerouteItemViewModel> TracerouteItems { get => _tracerouteItems; set { _tracerouteItems = value; OnPropertyChanged(nameof(TracerouteItems)); } }

	private string _target = string.Empty;
	public string Target { get => _target; set { _target = value; OnPropertyChanged(nameof(Target)); } }

	private string _traceRouteDesc = string.Empty;
	public string TraceRouteDesc { get => _traceRouteDesc; set { _traceRouteDesc = value; OnPropertyChanged(nameof(TraceRouteDesc)); } }

	public ICommand TraceCommand => new RelayCommand(o =>
	{
		if (Target is { Length: 0 }) return;
		TracerouteItems.Clear();
		Trace(Target, _settings.TraceRouteMaxHops ?? 30, _settings.TraceRouteMaxTimeOut ?? 5000);
		TraceRouteDesc = string.Format(Properties.Resources.RouteTraceDesc, Target);
	});

	private readonly Settings _settings;
	public TraceroutePageViewModel(Settings settings)
	{
		_settings = settings;
	}

	private async void Trace(string target, int maxHops, int timeout)
	{
		try
		{
			for (int ttl = 1; ttl <= maxHops; ttl++)
			{
				var startTime = DateTime.Now;
				PingReply reply = await TraceRoute(target, ttl, timeout);
				var endTime = DateTime.Now;

				var duration = endTime - startTime;

				TracerouteStep step = new(ttl, reply.Address, (long)duration.TotalMilliseconds, reply.Status);

				TracerouteItems.Add(new(step));

				if (reply.Status == IPStatus.Success)
					break;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, Properties.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
		}
	}

	private static Task<PingReply> TraceRoute(string targetAddress, int ttl, int timeout)
	{
		using Ping pingSender = new();
		PingOptions options = new()
		{
			Ttl = ttl
		};

		byte[] buffer = new byte[32];
		return pingSender.SendPingAsync(targetAddress, timeout, buffer, options);
	}

}
