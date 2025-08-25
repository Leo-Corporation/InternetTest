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
using DnsClient;
using DnsClient.Protocol;
using InternetTest.Commands;
using InternetTest.Models;
using InternetTest.ViewModels.Components;
using System.Collections.ObjectModel;
using System.Net;
using System.Windows;
using System.Windows.Input;
using Whois;

namespace InternetTest.ViewModels;
public class DnsToolsPageViewModel : ViewModelBase
{
	private ObservableCollection<DnsRecordTabBtnViewModel> _dnsTabs = [];
	public ObservableCollection<DnsRecordTabBtnViewModel> DnsTabs { get => _dnsTabs; set { _dnsTabs = value; OnPropertyChanged(nameof(DnsTabs)); } }

	private ObservableCollection<DnsItemViewModel> _dnsRecordsItems = [];
	public ObservableCollection<DnsItemViewModel> DnsRecordsItems { get => _dnsRecordsItems; set { _dnsRecordsItems = value; OnPropertyChanged(nameof(DnsRecordsItems)); } }

	private string _query = string.Empty;
	public string Query { get => _query; set { _query = value; OnPropertyChanged(nameof(Query)); } }

	private string _domain = string.Empty;
	public string Domain { get => _domain; set { _domain = value; OnPropertyChanged(nameof(Domain)); } }

	private string _ipAddress = string.Empty;
	public string IpAddress { get => _ipAddress; set { _ipAddress = value; OnPropertyChanged(nameof(IpAddress)); } }

	private string _createDate = string.Empty;
	public string CreateDate { get => _createDate; set { _createDate = value; OnPropertyChanged(nameof(CreateDate)); } }

	private string _expirationDate = string.Empty;
	public string ExpirationDate { get => _expirationDate; set { _expirationDate = value; OnPropertyChanged(nameof(ExpirationDate)); } }

	private string _registrant = string.Empty;
	public string Registrant { get => _registrant; set { _registrant = value; OnPropertyChanged(nameof(Registrant)); } }

	private string _status = string.Empty;
	public string Status { get => _status; set { _status = value; OnPropertyChanged(nameof(Status)); } }

	public ICommand GetDnsInfoCommand => new RelayCommand(async o =>
	{
		try
		{
			IsRefreshing = true;
			await GetDnsInfo();
			await GetDnsRecords();
			HasInfo = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, Properties.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
			HasInfo = false;
		}
		IsRefreshing = false;
	});

	private bool _hasInfo = false;
	public bool HasInfo { get => _hasInfo; set { _hasInfo = value; OnPropertyChanged(nameof(HasInfo)); OnPropertyChanged(nameof(PlaceholderVis)); } }

	private bool _isRefreshing = false;
	public bool IsRefreshing { get => _isRefreshing; set { _isRefreshing = value; OnPropertyChanged(nameof(IsRefreshing)); OnPropertyChanged(nameof(PlaceholderVis)); } }

	public bool PlaceholderVis => !IsRefreshing && !HasInfo;

	public List<DnsRecord> DnsRecords { get; set; } = [];
	public DnsToolsPageViewModel()
	{

	}

	private async Task GetDnsInfo()
	{
		if (string.IsNullOrEmpty(Query)) return;
		Query = Query.Replace("https://", "").Replace("http://", "").Replace("www.", "").Split("/")[0];

		IPHostEntry host = Dns.GetHostEntry(Query);
		IPAddress ip = host.AddressList[0];

		var whois = new WhoisLookup();
		var response = await whois.LookupAsync(Query);

		IpAddress = ip.ToString();
		Domain = Query;

		if (response is null) return;
		CreateDate = response.Registered.ToString() ?? Properties.Resources.Unknown;
		ExpirationDate = response.Expiration.ToString() ?? Properties.Resources.Unknown;
		Registrant = response.Registrant.Name ?? Properties.Resources.Unknown;
		Status = string.Join("\n", response.DomainStatus) ?? Properties.Resources.Unknown;
	}

	private async Task GetDnsRecords()
	{
		var lookup = new LookupClient();
		var result = await lookup.QueryAsync(Query, QueryType.ANY);

		DnsRecords.Clear();
		DnsTabs.Clear();
		DnsTabs.Add(new("ANY", this, QueryType.ANY, true));

		foreach (var record in result.AllRecords)
		{
			DnsRecords.Add(new(record.DomainName, record.ToString(), (QueryType)record.RecordType));

			if (DnsTabs.FirstOrDefault(x => x.Title == record.RecordType.ToString()) == null)
				DnsTabs.Add(new(record.RecordType.ToString(), this, (QueryType)record.RecordType));
		}

		DnsRecordsItems = [.. DnsRecords.Select(x => new DnsItemViewModel(x))];
	}
}

public class DnsRecordTabBtnViewModel(string title, DnsToolsPageViewModel dnsToolsPage, QueryType type, bool isChecked = false) : ViewModelBase
{
	public string Title { get; set; } = title;
	public bool IsChecked { get; set; } = isChecked;
	public ICommand SelectCommand { get; set; } = new RelayCommand(o =>
	{
		if (type == QueryType.ANY)
		{
			dnsToolsPage.DnsRecordsItems = [.. dnsToolsPage.DnsRecords.Select(x => new DnsItemViewModel(x))];
			return;
		}

		dnsToolsPage.DnsRecordsItems = [.. dnsToolsPage.DnsRecords.Where(x => x.Type == type).Select(x => new DnsItemViewModel(x))];
	});
}
