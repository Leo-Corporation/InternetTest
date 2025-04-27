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
using InternetTest.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace InternetTest.Windows;
/// <summary>
/// Interaction logic for DnsCacheInfoWindow.xaml
/// </summary>
public partial class DnsCacheInfoWindow : Window
{
	public DnsCacheInfo DnsCacheInfo { get; }

	public DnsCacheInfoWindow(DnsCacheInfo dnsCacheInfo)
	{
		InitializeComponent();
		DnsCacheInfo = dnsCacheInfo;
		InitUI();
	}

	private void InitUI()
	{
		NameTxt.Text = DnsCacheInfo.Name;

		// Category 1: Most Important Items
		var category1 = new Dictionary<string, object>
		{
			{ Properties.Resources.Entry, DnsCacheInfo.Entry },
			{ Properties.Resources.RecordName, DnsCacheInfo.Name },
			{ Properties.Resources.Data, DnsCacheInfo.Data },
		};

		var category1bis = new Dictionary<string, object>
		{
			{ Properties.Resources.Type, ((Types)DnsCacheInfo.Type).ToString() },
			{ Properties.Resources.Status, ((Status)DnsCacheInfo.Status).ToString() },
		};

		// Category 2: Other Information
		var category2 = new Dictionary<string, object>
		{
			{ Properties.Resources.Description, DnsCacheInfo.Description ?? "N/A" },
			{ Properties.Resources.DataLength, DnsCacheInfo.DataLength.ToString() },
			{ Properties.Resources.Section, ((Enums.Section)DnsCacheInfo.Section).ToString() },
			{ Properties.Resources.TimeToLive, $"{DnsCacheInfo.TimeToLive} ms" },
			{ Properties.Resources.Caption, DnsCacheInfo.Caption ?? "N/A" },
			{ Properties.Resources.PsComputerName, DnsCacheInfo.PsComputerName ?? "N/A" }
		};

		int i = 0;
		foreach (var category in category1)
		{
			StackPanel stackPanel = new() { Margin = new(5) };

			stackPanel.Children.Add(new TextBlock() { FontSize = 12, Text = category.Key, Foreground = Global.GetBrushFromResource("Foreground2") });
			stackPanel.Children.Add(new TextBlock() { FontSize = 14, FontWeight = FontWeights.SemiBold, Text = category.Value?.ToString() });

			Cat1Grid.Children.Add(stackPanel);
			Grid.SetRow(stackPanel, i);
			Grid.SetColumnSpan(stackPanel, 2);
			i++;
		}
		foreach (var category in category1bis)
		{
			StackPanel stackPanel = new() { Margin = new(5) };

			stackPanel.Children.Add(new TextBlock
			{
				Text = category.Key,
				FontSize = 12,
				Foreground = Global.GetBrushFromResource("Foreground2"),
				FontWeight = FontWeights.Bold
			});
			stackPanel.Children.Add(new TextBlock
			{
				Text = category.Value?.ToString(),
				FontSize = 14,
				FontWeight = FontWeights.SemiBold
			});
			Cat1Grid.Children.Add(stackPanel);
			Grid.SetColumn(stackPanel, i % 2);
			Grid.SetRow(stackPanel, 3);
			i++;
		}
		i = 0;
		foreach (var category in category2)
		{
			StackPanel stackPanel = new() { Margin = new(5) };
			stackPanel.Children.Add(new TextBlock() { FontSize = 12, Text = category.Key, Foreground = Global.GetBrushFromResource("Foreground2") });
			stackPanel.Children.Add(new TextBlock() { FontSize = 14, FontWeight = FontWeights.SemiBold, Text = category.Value?.ToString() });
			Cat2Grid.Children.Add(stackPanel);
			Grid.SetColumn(stackPanel, i % 2);
			Grid.SetRow(stackPanel, i / 2);
			i++;
		}
	}

	private void CopyBtn_Click(object sender, RoutedEventArgs e)
	{
		Clipboard.SetText(DnsCacheInfo.ToString());
	}

	private void CloseBtn_Click(object sender, RoutedEventArgs e)
	{
		Close();
	}

	bool cat2Expanded = false;
	private void ExpandCat2Btn_Click(object sender, RoutedEventArgs e)
	{
		cat2Expanded = !cat2Expanded;
		ExpandCat2Btn.Content = cat2Expanded ? "\uF2B7" : "\uF2A4";
		Cat2Grid.Visibility = cat2Expanded ? Visibility.Visible : Visibility.Collapsed;
	}
}
