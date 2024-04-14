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
			ExecuteRequest();
		}
		catch (Exception ex)
		{
			ResponseTxt.Text = ex.Message;
		}
	}

	string headers = "";
	private async void ExecuteRequest()
	{
		var options = new RestClientOptions(UrlTxt.Text);
		var client = new RestClient(options);
		var request = new RestRequest("", (Method)RequestTypeComboBox.SelectedIndex);

		var response = await client.ExecuteAsync(request);
		ResponseTxt.Text = response.Content;

		HeadersPanel.Children.Clear();
		headers = "";

		foreach (var item in response.Headers)
		{
			headers += item.ToString() + "\n";
			var header = item.ToString().Split("=", 2);
			if (header.Length < 2) continue;

			HeadersPanel.Children.Add(new TextBlock() { Text = header[0], FontWeight = FontWeights.Bold, TextWrapping = TextWrapping.Wrap, Margin = new(0, 5, 0, 0) });
			HeadersPanel.Children.Add(new TextBlock() { Text = header[1], TextWrapping = TextWrapping.Wrap });
		}
    }

	private void UrlTxt_KeyUp(object sender, KeyEventArgs e)
	{
		if (e.Key == Key.Enter) SendBtn_Click(sender, e);
	}

	private void UrlTxt_TextChanged(object sender, TextChangedEventArgs e)
	{
		ParametersPanel.Children.Clear();
		ParametersBorder.Visibility = Visibility.Collapsed;
		try
		{
			var parameters = ParseUrl(UrlTxt.Text);
			for (int i = 0; i < parameters.Count; i++)
			{
				ParametersPanel.Children.Add(new ParameterItem(parameters[i].Item1, parameters[i].Item2));
			}
		}
		catch { }
		ParametersBorder.Visibility = ParametersPanel.Children.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
	}

	private void DismissBtn_Click(object sender, RoutedEventArgs e)
	{
		UrlTxt.Text = string.Empty;
	}

	private List<(string, string)> ParseUrl(string url)
	{
		var elements = url.Split(new string[] { "/", "//" }, StringSplitOptions.None);
		var parameters = elements[^1].Split(new string[] { "?", "&" }, StringSplitOptions.None);
		List<(string, string)> values = new();
		for (int i = 0; i < parameters.Length; i++)
		{
			var kv = parameters[i].Split(new string[] { "=" }, StringSplitOptions.None);
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
		if (headers != "") Clipboard.SetText(headers);
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
