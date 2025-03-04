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

using System;
using System.Windows.Controls;

namespace InternetTest.UserControls;
/// <summary>
/// Interaction logic for ParameterItem.xaml
/// </summary>
public partial class ParameterItem : UserControl
{
	readonly bool init = true;
	readonly bool hasRemoved = false;
	public ParameterItem(string name, string value, int id, Action<string, string, int, bool> updateParameter)
	{
		InitializeComponent();
		VarName = name;
		Value = value;
		Id = id;
		UpdateParameter = updateParameter;
		NameTxt.Text = VarName;
		ValueTxt.Text = Value;
		Toggle.IsChecked = true;
		IsHidden = ContainsSensitiveData(VarName);
		HideContent(IsHidden);
		init = false;
	}

	public string VarName { get; }
	public string Value { get; }
	public int Id { get; }
	public bool IsHidden { get; set; } = false;
	public Action<string, string, int, bool> UpdateParameter { get; }

	private void NameTxt_TextChanged(object sender, TextChangedEventArgs e)
	{
		if (init) return;
		UpdateParameter(NameTxt.Text, ValueTxt.Text, Id, Toggle.IsChecked == true);
	}

	private void Toggle_Checked(object sender, System.Windows.RoutedEventArgs e)
	{
		if (init) return;
		UpdateParameter(NameTxt.Text, ValueTxt.Text, Id, Toggle.IsChecked == true);
	}

	private void HideBtn_Click(object sender, System.Windows.RoutedEventArgs e)
	{
		IsHidden = !IsHidden;
		HideContent(IsHidden);
	}

	private void ValuePwr_PasswordChanged(object sender, System.Windows.RoutedEventArgs e)
	{
		ValueTxt.Text = ValuePwr.Password;
	}

	private void HideContent(bool isHidden)
	{
		ValuePwr.Visibility = isHidden ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
		ValueTxt.Visibility = isHidden ? System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible;
		HideBtn.Content = isHidden ? "\uF3FC" : "\uF3F8";
		ValuePwr.Password = ValueTxt.Text;
	}

	private bool ContainsSensitiveData(string name)
	{
		string[] sensitiveWords = [
			"password", "token", "key", "secret","private", "protected", "auth", "login", "username", "email"
		];

		foreach (string word in sensitiveWords)
		{
			if (name.ToLower().Contains(word)) return true;
		}
		return false;
	}
}
