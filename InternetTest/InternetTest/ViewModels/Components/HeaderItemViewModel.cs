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
using System.Windows.Input;

namespace InternetTest.ViewModels.Components;

public class HeaderItemViewModel : ViewModelBase
{
	private string _title = string.Empty;
	public string Title { get => _title; set { _title = value; OnPropertyChanged(nameof(Title)); } }

	private string _value = string.Empty;
	public string Value { get => _value; set { _value = value; OnPropertyChanged(nameof(Value)); } }

	private readonly string _fullValue = string.Empty;

	public ICommand CopyCommand => new RelayCommand(o =>
	{
		if (string.IsNullOrEmpty(Value)) return;
		System.Windows.Clipboard.SetDataObject($"{Title}={_fullValue}");
	});
	public HeaderItemViewModel(string title, string value)
	{
		Title = title;
		Value = value.Length > 50 ? value[..50] + "..." : value;
		_fullValue = value;
	}
}
