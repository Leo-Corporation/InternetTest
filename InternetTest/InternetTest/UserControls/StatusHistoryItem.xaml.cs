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
using PeyrSharp.Core.Converters;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace InternetTest.UserControls;
/// <summary>
/// Interaction logic for StatusHistoryItem.xaml
/// </summary>
public partial class StatusHistoryItem : UserControl
{
	HistoryItem HistoryItem { get; set; }
	AppPages AppPage { get; init; }
	public StatusHistoryItem(HistoryItem historyItem, AppPages appPages)
	{
		InitializeComponent();
		HistoryItem = historyItem;
		AppPage = appPages;

		InitUI();
	}

	private void InitUI()
	{
		IconTxt.Text = HistoryItem.Icon;
		ContentTxt.Text = HistoryItem switch
		{
			DownHistory => $"{Time.UnixTimeToDateTime(HistoryItem.Date):g} - {((DownHistory)HistoryItem).Website} - {((DownHistory)HistoryItem).StatusText} ({((DownHistory)HistoryItem).StatusCode})",
			_ => $"{Time.UnixTimeToDateTime(HistoryItem.Date):g} - {(((StatusHistory)HistoryItem).Status ? Properties.Resources.Connected : Properties.Resources.NotConnected)}"
		};

		IconTxt.Foreground = IconTxt.Text switch
		{
			"\uF299" => new SolidColorBrush { Color = Global.GetColorFromResource("Green") },
			"\uF36E" => new SolidColorBrush { Color = Global.GetColorFromResource("Red") },
			_ => new SolidColorBrush { Color = Global.GetColorFromResource("Gray") }
		};
	}

	private void DeleteBtn_Click(object sender, RoutedEventArgs e)
	{
		try
		{
			switch (AppPage)
			{
				case AppPages.DownDetector:
					Global.History.DownDetectorHistory.Remove((DownHistory)HistoryItem);
					break;
				case AppPages.Status:
					Global.History.StatusHistory.Remove((StatusHistory)HistoryItem);
					break;
				default:
					break;
			}
			Global.HistoryPage?.InitUI();
		}
		catch { }
	}
}
