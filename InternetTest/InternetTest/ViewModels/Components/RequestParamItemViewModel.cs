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
public class RequestParamItemViewModel : ViewModelBase
{
	private string _key = string.Empty;
	public string Key { get => _key; set { _key = value; Update(); OnPropertyChanged(nameof(Key)); } }

	private string _value = string.Empty;
	public string Value { get => _value; set { _value = value; Update(); OnPropertyChanged(nameof(Value)); } }

	private bool _isChecked = true;
	public bool IsChecked { get => _isChecked; set { _isChecked = value; Update(); OnPropertyChanged(nameof(IsChecked)); } }

	private readonly RequestsPageViewModel _requestsPageViewModel;
	private readonly bool _init = false;
	public RequestParamItemViewModel(RequestsPageViewModel requestsPageViewModel, RequestParam requestParam)
	{
		_requestsPageViewModel = requestsPageViewModel;
		Key = requestParam.Key;
		Value = requestParam.Value;
		_init = true;
	}

	private void Update()
	{
		if (!_init) return; // prevent updating on initialization
		_requestsPageViewModel.TriggerParseUrl = false; // prevent loop
		var baseUrl = _requestsPageViewModel.Url.Split("?", 2)[0];

		_requestsPageViewModel.Url = baseUrl + "?" + string.Join("&", _requestsPageViewModel.RequestParams.Where(x => x.IsChecked).Select(x => $"{x.Key}={x.Value}"));
		_requestsPageViewModel.TriggerParseUrl = true;
	}
}
