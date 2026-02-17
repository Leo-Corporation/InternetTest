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
using Microsoft.Win32;
using RestSharp;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace InternetTest.ViewModels;

public class RequestsPageViewModel : ViewModelBase
{
	private ObservableCollection<HeaderItemViewModel> _headers = [];
	public ObservableCollection<HeaderItemViewModel> Headers { get => _headers; set { _headers = value; OnPropertyChanged(nameof(Headers)); } }

	private ObservableCollection<RequestParamItemViewModel> _requestParams = [];
	public ObservableCollection<RequestParamItemViewModel> RequestParams { get => _requestParams; set { _requestParams = value; OnPropertyChanged(nameof(RequestParams)); } }

	private int _requestType = 0;
	public int RequestType { get => _requestType; set { _requestType = value; OnPropertyChanged(nameof(RequestType)); } }

	private int _bodyType = 0;
	public int BodyType
	{
		get => _bodyType; set
		{
			_bodyType = value;
			BodyVisibility = (value != 0) ? Visibility.Visible : Visibility.Collapsed;
			OnPropertyChanged(nameof(BodyType));
		}
	}

	private int _authType = 0;
	public int AuthType
	{
		get => _authType; set
		{
			_authType = value;
			BasicVisibility = value == 1 ? Visibility.Visible : Visibility.Collapsed;
			BearerVisibility = value == 2 ? Visibility.Visible : Visibility.Collapsed;
			OnPropertyChanged(nameof(AuthType));
		}
	}

	private string _url = string.Empty;
	public string Url { get => _url; set { _url = value; ParseUrl(value); OnPropertyChanged(nameof(Url)); } }

	private string _body = string.Empty;
	public string Body { get => _body; set { _body = value; OnPropertyChanged(nameof(Body)); } }

	private string _responseText = string.Empty;
	public string ResponseText { get => _responseText; set { _responseText = value; OnPropertyChanged(nameof(ResponseText)); } }

	private string _authToken = string.Empty;
	public string AuthToken { get => _authToken; set { _authToken = value; OnPropertyChanged(nameof(AuthToken)); } }

	private string _username = string.Empty;
	public string Username { get => _username; set { _username = value; OnPropertyChanged(nameof(Username)); } }

	private string _password = string.Empty;
	public string Password { get => _password; set { _password = value; OnPropertyChanged(nameof(Password)); } }

	private string _responseStatus = string.Empty;
	public string ResponseStatus { get => _responseStatus; set { _responseStatus = value; OnPropertyChanged(nameof(ResponseStatus)); } }

	private Visibility _basicVisibility = Visibility.Collapsed;
	public Visibility BasicVisibility { get => _basicVisibility; set { _basicVisibility = value; OnPropertyChanged(nameof(BasicVisibility)); } }

	private Visibility _bearerVisibility = Visibility.Collapsed;
	public Visibility BearerVisibility { get => _bearerVisibility; set { _bearerVisibility = value; OnPropertyChanged(nameof(BearerVisibility)); } }

	private Visibility _bodyVisibility = Visibility.Collapsed;
	public Visibility BodyVisibility { get => _bodyVisibility; set { _bodyVisibility = value; OnPropertyChanged(nameof(BodyVisibility)); } }

	private Visibility _paramsVisibility = Visibility.Collapsed;
	public Visibility ParamsVisibility { get => _paramsVisibility; set { _paramsVisibility = value; OnPropertyChanged(nameof(ParamsVisibility)); } }

	public bool _responseVisible = false;
	public bool ResponseVisible { get => _responseVisible; set { _responseVisible = value; OnPropertyChanged(nameof(ResponseVisible)); } }

	internal bool TriggerParseUrl = true;
	public ICommand SendRequestCommand => new RelayCommand(async o =>
	{
		if (string.IsNullOrEmpty(Url)) return;
		if (!Url.StartsWith(_settings.UseHttps ? "https://" : "http://") && !Url.Contains("http")) Url = (_settings.UseHttps ? "https://" : "http://") + Url;

		var options = new RestClientOptions(Url);
		var client = new RestClient(options);
		var request = new RestRequest("", (Method)RequestType);

		// Body
		if (!string.IsNullOrEmpty(Body))
		{
			request.AddParameter(GetContentType(), Body, ParameterType.RequestBody);
		}

		// Auth
		if (AuthType != 0)
		{
			request.AddHeader("Authorization", GetAuthValue());
		}

		ResponseVisible = false;

		// Send the request
		var response = await client.ExecuteAsync(request);
		ResponseText = response.Content ?? string.Empty;
		ResponseStatus = string.Format(Properties.Resources.ResponseDesc, $"{(int)response.StatusCode} {response.StatusDescription}");

		// Load headers section
		Headers.Clear();
		_headersText = "";
		foreach (var header in response.Headers ?? [])
		{
			Headers.Add(new(header.Name, header.Value?.ToString() ?? string.Empty));
			_headersText += $"{header}\n";
		}

		ResponseVisible = true;
	});

	public ICommand CopyCommand => new RelayCommand(o =>
	{
		Clipboard.SetDataObject(ResponseText);
	});

	public ICommand CopyHeadersCommand => new RelayCommand(o =>
	{
		Clipboard.SetDataObject(_headersText);
	});

	public ICommand SaveCommand => new RelayCommand(o =>
	{
		if (string.IsNullOrEmpty(ResponseText)) return;
		SaveFileDialog dialog = new()
		{
			Title = Properties.Resources.Save,
			Filter = Properties.Resources.TxtFiles + " (*.txt)|*.txt|JSON (*.json)|*.json|All Files|",
			InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
			FileName = Properties.Resources.Requests + ".txt",
			DefaultExt = ".txt"
		};

		if (dialog.ShowDialog() ?? false)
		{
			File.WriteAllText(dialog.FileName, ResponseText);
		}
	});

	private readonly Settings _settings;
	private string _headersText = "";
	public RequestsPageViewModel(Settings settings)
	{
		_settings = settings;
	}

	private string GetContentType()
	{
		return BodyType switch
		{
			1 => "application/json",
			2 => "application/xml",
			3 => "text/plain",
			4 => "application/x-www-form-urlencoded",
			_ => "text/plain"
		};
	}

	private string GetAuthValue()
	{
		return AuthType switch
		{
			1 => $"Basic {Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"{Username}:{Password}"))}",
			2 => $"Bearer {AuthToken}",
			_ => string.Empty
		};
	}

	private void ParseUrl(string url)
	{
		if (!TriggerParseUrl) return;
		RequestParams.Clear();
		ParamsVisibility = Visibility.Collapsed;

		if (string.IsNullOrEmpty(url)) return;

		var elements = url.Split(["/", "//"], StringSplitOptions.None);
		var parameters = elements[^1].Split(["?", "&"], StringSplitOptions.None);

		for (int i = 0; i < parameters.Length; i++)
		{
			var kv = parameters[i].Split(["="], StringSplitOptions.None);
			if (kv.Length < 2) continue;
			RequestParams.Add(new(this, new(kv[0], kv[1])));
		}

		if (RequestParams.Count > 0) ParamsVisibility = Visibility.Visible;
	}
}
