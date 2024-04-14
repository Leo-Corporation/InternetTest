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

using System;
using System.Windows.Controls;

namespace InternetTest.UserControls;
/// <summary>
/// Interaction logic for ParameterItem.xaml
/// </summary>
public partial class ParameterItem : UserControl
{
	bool init = true;

	public ParameterItem(string name, string value, int id, Action<string, string, int> updateParameter)
	{
		InitializeComponent();
		VarName = name;
		Value = value;
		Id = id;
		UpdateParameter = updateParameter;
		NameTxt.Text = VarName;
		ValueTxt.Text = Value;
		init = false;
	}

	public string VarName { get; }
	public string Value { get; }
	public int Id { get; }
	public Action<string, string, int> UpdateParameter { get; }

	private void NameTxt_TextChanged(object sender, TextChangedEventArgs e)
	{
		if (init) return;
		UpdateParameter(NameTxt.Text, ValueTxt.Text, Id);
	}
}
