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

using InternetTest.Classes;
using InternetTest.UserControls;
using Microsoft.Win32;
using RestSharp;
using Synethia;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace InternetTest.Pages;
/// <summary>
/// Interaction logic for RequestsPage.xaml
/// </summary>
public partial class RequestsPage : Page
{
	bool codeInjected = !Global.Settings.UseSynethia;

	internal List<(string, string)> Parameters { get; set; } = [];

	public RequestsPage()
	{
		InitializeComponent();
		InitUI(); // Load the UI
		Loaded += (o, e) => SynethiaManager.InjectSynethiaCode(this, Global.SynethiaConfig.PagesInfo, 8, ref codeInjected);
	}

	private void InitUI()
	{
		TitleTxt.Text = $"{Properties.Resources.Commands} > {Properties.Resources.Requests}";
		RequestTypeComboBox.SelectedIndex = 0;
		ResponseBtn.IsChecked = true;
	}

	private void SendBtn_Click(object sender, RoutedEventArgs e)
	{
		try
		{
			if (!UrlTxt.Text.Contains("http")) return;
			ExecuteRequest();
		}
		catch (Exception ex)
		{
			ResponseTxt.Text = ex.Message;
		}
	}

	string _headers = "";
	string _baseUrl = "";
	private async void ExecuteRequest()
	{
		try
		{
			var options = new RestClientOptions(UrlTxt.Text);
			var client = new RestClient(options);
			var request = new RestRequest("", (Method)RequestTypeComboBox.SelectedIndex);

			var response = await client.ExecuteAsync(request);
			ResponseTxt.Text = response.Content;

			HeadersPanel.Children.Clear();
			_headers = "";

			foreach (var item in response.Headers)
			{
				_headers += item.ToString() + "\n";
				var header = item.ToString().Split("=", 2);
				if (header.Length < 2) continue;

				HeadersPanel.Children.Add(new TextBlock() { Text = header[0], FontWeight = FontWeights.Bold, TextWrapping = TextWrapping.Wrap, Margin = new(0, 5, 0, 0) });
				HeadersPanel.Children.Add(new TextBlock() { Text = header[1], TextWrapping = TextWrapping.Wrap });
			}
		}
		catch {	}
	}

	private void UrlTxt_KeyUp(object sender, KeyEventArgs e)
	{
		if (e.Key == Key.Enter) SendBtn_Click(sender, e);
	}
	bool _reloadParameters = true;
	private void UrlTxt_TextChanged(object sender, TextChangedEventArgs e)
	{
		if (!_reloadParameters) return;
		_baseUrl = UrlTxt.Text.Split("?", 2)[0];
		ParametersPanel.Children.Clear();
		ParametersBorder.Visibility = Visibility.Collapsed;
		try
		{
			var parameters = ParseUrl(UrlTxt.Text);
			Parameters = parameters;

			for (int i = 0; i < Parameters.Count; i++) // Render the parameter section
			{
				ParametersPanel.Children.Add(new ParameterItem(Parameters[i].Item1, Parameters[i].Item2, i, UpdateParameters));
			}
		}
		catch { }
		ParametersBorder.Visibility = ParametersPanel.Children.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
	}

	readonly List<int> _hiddenIds = [];
	void UpdateParameters(string name, string value, int id, bool included)
	{
		if (!included) _hiddenIds.Add(id); else _hiddenIds.Remove(id);
		Parameters[id] = (name, value);

		string url = _baseUrl; // Create the new URL
		int parametersCount = 0;
		for (int i = 0; i < Parameters.Count; i++) // Append each "included" parameter to the base URL
		{
			if (_hiddenIds.Contains(i)) continue;
			url += parametersCount == 0 ? $"?{Parameters[i].Item1}={Parameters[i].Item2}" : $"&{Parameters[i].Item1}={Parameters[i].Item2}";
			parametersCount++;
		}
		_reloadParameters = false; // Prevent rerender of the Parameters section
		UrlTxt.Text = url; // Update the URL
		_reloadParameters = true;
	}

	private void DismissBtn_Click(object sender, RoutedEventArgs e)
	{
		UrlTxt.Text = string.Empty;
	}

	private static List<(string, string)> ParseUrl(string url)
	{
		var elements = url.Split(["/", "//"], StringSplitOptions.None);
		var parameters = elements[^1].Split(["?", "&"], StringSplitOptions.None);
		List<(string, string)> values = [];
		for (int i = 0; i < parameters.Length; i++)
		{
			var kv = parameters[i].Split(["="], StringSplitOptions.None);
			if (kv.Length < 2) continue;
			values.Add((kv[0], kv[1]));
		}

		return values;
	}

	private void SaveBtn_Click(object sender, RoutedEventArgs e)
	{
		if (string.IsNullOrEmpty(ResponseTxt.Text)) return;
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
			File.WriteAllText(dialog.FileName, ResponseTxt.Text);
		}
	}

	private void CopyBtn_Click(object sender, RoutedEventArgs e)
	{
		Clipboard.SetText(ResponseTxt.Text);
	}

	private void CopyHeadersBtn_Click(object sender, RoutedEventArgs e)
	{
		if (_headers != "") Clipboard.SetText(_headers);
	}

	private void ResponseBtn_Checked(object sender, RoutedEventArgs e)
	{
		ResponseSection.Visibility = Visibility.Visible;
		HeadersSection.Visibility = Visibility.Collapsed;
	}

	private void HeadersBtn_Checked(object sender, RoutedEventArgs e)
	{
		ResponseSection.Visibility = Visibility.Collapsed;
		HeadersSection.Visibility = Visibility.Visible;
	}
}
