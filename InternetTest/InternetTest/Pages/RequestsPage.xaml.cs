﻿/*
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
using RestSharp;
using Synethia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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

	private async void ExecuteRequest()
	{
		var options = new RestClientOptions(UrlTxt.Text);
		var client = new RestClient(options);
		var request = new RestRequest("", ((Method)RequestTypeComboBox.SelectedIndex));

		var response = await client.GetAsync(request);
		ResponseTxt.Text = response.Content;
	}

	private void UrlTxt_KeyUp(object sender, KeyEventArgs e)
	{
		if (e.Key == Key.Enter) SendBtn_Click(sender, e);
	}

	private void UrlTxt_TextChanged(object sender, TextChangedEventArgs e)
	{

	}

	private void DismissBtn_Click(object sender, RoutedEventArgs e)
	{
		UrlTxt.Text = string.Empty;
	}
}
