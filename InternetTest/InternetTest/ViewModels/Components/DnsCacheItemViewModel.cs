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
using InternetTest.Models;

namespace InternetTest.ViewModels.Components;

public class DnsCacheItemViewModel : ViewModelBase
{
	private string _entry = string.Empty;
	public string Entry { get => _entry; set { _entry = value; OnPropertyChanged(nameof(Entry)); } }

	private string _recordName = string.Empty;
	public string RecordName { get => _recordName; set { _recordName = value; OnPropertyChanged(nameof(RecordName)); } }

	private string _recordType = string.Empty;
	public string RecordType { get => _recordType; set { _recordType = value; OnPropertyChanged(nameof(RecordType)); } }

	private string _status = string.Empty;
	public string Status { get => _status; set { _status = value; OnPropertyChanged(nameof(Status)); } }

	private string _data = string.Empty;
	public string Data { get => _data; set { _data = value; OnPropertyChanged(nameof(Data)); } }

	public DnsCacheItemViewModel(DnsCacheInfo dnsCacheInfo)
	{
		Entry = dnsCacheInfo.Entry;
		RecordName = dnsCacheInfo.Name;
		RecordType = ((Enums.Types)dnsCacheInfo.Type).ToString();
		Status = ((Enums.Status)dnsCacheInfo.Status).ToString();
		Data = string.IsNullOrEmpty(dnsCacheInfo.Data) ? Properties.Resources.NA : dnsCacheInfo.Data;
	}
}
