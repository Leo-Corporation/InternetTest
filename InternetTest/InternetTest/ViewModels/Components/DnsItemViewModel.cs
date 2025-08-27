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
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace InternetTest.ViewModels.Components;
public class DnsItemViewModel : ViewModelBase
{
	private string _value = string.Empty;
	public string Value { get => _value; set { _value = value; OnPropertyChanged(nameof(Value)); } }

	private string _ttl = string.Empty;
	public string Ttl { get => _ttl; set { _ttl = value; OnPropertyChanged(nameof(Ttl)); } }

	private string _class = string.Empty;
	public string Class { get => _class; set { _class = value; OnPropertyChanged(nameof(Class)); } }

	private string _recordType = string.Empty;
	public string RecordType { get => _recordType; set { _recordType = value; OnPropertyChanged(nameof(RecordType)); } }

	private string _domain = string.Empty;
	public string Domain { get => _domain; set { _domain = value; OnPropertyChanged(nameof(Domain)); } }

	private SolidColorBrush? _typeBackground;

	public SolidColorBrush? TypeBackground { get => _typeBackground; set { _typeBackground = value; OnPropertyChanged(nameof(RecordType)); } }

	private readonly DnsRecord _dnsRecord;
	public ICommand CopyCommand => new RelayCommand(o =>
	{
		Clipboard.SetDataObject(_dnsRecord.ToString());
	});

	public DnsItemViewModel(DnsRecord dnsRecord)
	{
		RecordType = dnsRecord.Type.ToString();
		Domain = dnsRecord.Name;
		Value = dnsRecord.Value;
		Ttl = dnsRecord.Ttl;
		Class = dnsRecord.Class;

		TypeBackground = dnsRecord.Type switch
		{
			DnsClient.QueryType.AAAA => new SolidColorBrush(Colors.BlueViolet),
			DnsClient.QueryType.A => new SolidColorBrush(Colors.DeepSkyBlue),
			DnsClient.QueryType.NS => new SolidColorBrush(Colors.DarkCyan),
			DnsClient.QueryType.SOA => new SolidColorBrush(Colors.Red),
			DnsClient.QueryType.MX => new SolidColorBrush(Colors.DarkOrange),
			DnsClient.QueryType.TXT => new SolidColorBrush(Colors.DarkGoldenrod),
			DnsClient.QueryType.CNAME => new SolidColorBrush(Colors.ForestGreen),

			_ => ThemeHelper.GetSolidColorBrush("Accent")

		};

		_dnsRecord = dnsRecord;
	}
}
