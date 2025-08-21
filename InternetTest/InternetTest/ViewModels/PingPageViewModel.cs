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
using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace InternetTest.ViewModels;

public class PingPageViewModel : ViewModelBase
{
	private string _min = string.Empty;
	public string Min { get => _min; set { _min = value; OnPropertyChanged(nameof(Min)); } }

	private string _max = string.Empty;
	public string Max { get => _max; set { _max = value; OnPropertyChanged(nameof(Max)); } }

	private string _avg = string.Empty;
	public string Avg { get => _avg; set { _avg = value; OnPropertyChanged(nameof(Avg)); } }

	private string _startTime = string.Empty;
	public string StartTime { get => _startTime; set { _startTime = value; OnPropertyChanged(nameof(StartTime)); } }

	private string _duration = string.Empty;
	public string Duration { get => _duration; set { _duration = value; OnPropertyChanged(nameof(Duration)); } }

	private string _host = string.Empty;
	public string Host { get => _host; set { _host = value; OnPropertyChanged(nameof(Host)); } }

	private string _query = string.Empty;
	public string Query { get => _query; set { _query = value; OnPropertyChanged(nameof(Query)); } }

	private int _received;
	public int Received { get => _received; set { _received = value; OnPropertyChanged(nameof(Received)); } }

	private int _sent;
	public int Sent { get => _sent; set { _sent = value; OnPropertyChanged(nameof(Sent)); } }

	private int _lost;
	public int Lost { get => _lost; set { _lost = value; OnPropertyChanged(nameof(Lost)); } }

	private string _lossPercentage = string.Empty;
	public string LossPercentage { get => _lossPercentage; set { _lossPercentage = value; OnPropertyChanged(nameof(LossPercentage)); } }

	private int _requestAmount = 4;
	public int RequestAmount { get => _requestAmount; set { _requestAmount = value; OnPropertyChanged(nameof(RequestAmount)); } }

	private bool _empty = true;

	public bool Empty { get => _empty; set { _empty = value; OnPropertyChanged(nameof(Empty)); } }

	public ICommand PingCommand => new RelayCommand(o => Ping());
	public ICommand CopyCommand => new RelayCommand(o =>
	{
		Clipboard.SetDataObject($"\n{Properties.Resources.Ping} {Query}:\n" +
			$"{Properties.Resources.MinTime}: {Min}\n" +
			$"{Properties.Resources.MaxTime}: {Max}\n" +
			$"{Properties.Resources.AverageTime}: {Avg}\n" +
			$"{Properties.Resources.StartTime}: {StartTime}\n" +
			$"{Properties.Resources.Duration}: {Duration}\n" +
			$"{Properties.Resources.PackageSent}: {Sent}\n" +
			$"{Properties.Resources.PackageReceived}: {Received}\n" +
			$"{Properties.Resources.PackageLost}: {Lost} ({LossPercentage})");
	});

	private readonly Settings _settings;
	public PingPageViewModel(Settings settings)
	{
		_settings = settings;

		Query = _settings.TestSite ?? "google.com";
	}

	private async Task Ping()
	{
		Query = Query.Replace("https://", "").Replace("http://", "");

		if (Query is null or { Length: 0 } || string.IsNullOrWhiteSpace(Query))
		{
			MessageBox.Show(Properties.Resources.EnterIP, Properties.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Information);
			return;
		}

		Sent = 0;
		Received = 0;
		Lost = 0;
		StartTime = DateTime.Now.ToString("HH:mm:ss");
		Empty = false;

		long[] times = new long[RequestAmount];
		for (int i = 0; i < RequestAmount; i++)
		{
			try
			{
				var ping = await new Ping().SendPingAsync(Query);
				times[i] = ping.RoundtripTime;
				Sent++;

				if (ping.Status == IPStatus.Success)
				{
					Received++;
					Lost = Sent - Received;
					LossPercentage = $"{(double)Lost / Sent * 100:0.00}%";
					Min = $"{times.Min()} ms";
					Max = $"{times.Max()} ms";
					Avg = $"{times.Average()} ms";
					Duration = $"{times.Sum()} ms";
				}
				else
				{
					Lost++;
					LossPercentage = $"{(double)Lost / Sent * 100:0.00}%";
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, Properties.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}
		}

		Host = Dns.GetHostEntry(Query).HostName;
	}
}
