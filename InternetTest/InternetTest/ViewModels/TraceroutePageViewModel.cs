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

	private int _totalHops = 0;
	public int TotalHops { get => _totalHops; set { _totalHops = value; OnPropertyChanged(nameof(TotalHops)); } }

	private int _successfullHops = 0;
	public int SuccessfullHops { get => _successfullHops; set { _successfullHops = value; OnPropertyChanged(nameof(SuccessfullHops)); } }

	private string _totalHopsDesc = string.Empty;
	public string TotalHopsDesc { get => _totalHopsDesc; set { _totalHopsDesc = value; OnPropertyChanged(nameof(TotalHopsDesc)); } }

	private string _successfullHopsDesc = string.Empty;
	public string SuccessfullHopsDesc { get => _successfullHopsDesc; set { _successfullHopsDesc = value; OnPropertyChanged(nameof(SuccessfullHopsDesc)); } }

	private string _duration = string.Empty;
	public string Duration { get => _duration; set { _duration = value; OnPropertyChanged(nameof(Duration)); } }

	private string _startTime = string.Empty;
	public string StartTime { get => _startTime; set { _startTime = value; OnPropertyChanged(nameof(StartTime)); } }

	private string _targetFinal = string.Empty;
	public string TargetFinal { get => _targetFinal; set { _targetFinal = value; OnPropertyChanged(nameof(TargetFinal)); } }

	private string _staticTarget = string.Empty;
	public string StaticTarget { get => _staticTarget; set { _staticTarget = value; OnPropertyChanged(nameof(StaticTarget)); } }

	private bool _detailsVisible = false;
	public bool DetailsVisible { get => _detailsVisible; set { _detailsVisible = value; OnPropertyChanged(nameof(DetailsVisible)); } }

	private bool _empty = true;
	public bool Empty { get => _empty; set { _empty = value; OnPropertyChanged(nameof(Empty)); } }

	private bool _loading = false;
	public bool Loading { get => _loading; set { _loading = value; OnPropertyChanged(nameof(Loading)); } }

	public ICommand TraceCommand => new RelayCommand(async o =>
	{
		if (Target is { Length: 0 } || Loading) return;
		Loading = true;
		Empty = false;
		DetailsVisible = false;
		TracerouteItems.Clear();
		TotalHops = 0;
		SuccessfullHops = 0;
		TraceRouteDesc = string.Format(Properties.Resources.RouteTraceDesc, Target);
		Duration = string.Empty;

		var startTime = DateTime.Now;
		await TraceAsync(Target, _settings.TraceRouteMaxHops ?? 30, _settings.TraceRouteMaxTimeOut ?? 5000);
		var endTime = DateTime.Now;

		Loading = false;
		TotalHops = TracerouteItems.Count;
		SuccessfullHops = TracerouteItems.Count(x => x.Host != Properties.Resources.TimedOut);
		Duration = $"{(endTime - startTime).TotalSeconds:0.0} s";
		TotalHopsDesc = string.Format(Properties.Resources.MaxHopsS, _settings.TraceRouteMaxHops ?? 30);
		SuccessfullHopsDesc = $"{SuccessfullHops / (double)TotalHops * 100d:0.0}%";
		StartTime = startTime.ToString("HH:mm:ss");
		TargetFinal = TracerouteItems.LastOrDefault()?.Host ?? Properties.Resources.Unknown;
		StaticTarget = Target;
		DetailsVisible = true;
	});

	private readonly Settings _settings;
	public TraceroutePageViewModel(Settings settings)
	{
		_settings = settings;
	}

	private async Task TraceAsync(string target, int maxHops, int timeout)
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
