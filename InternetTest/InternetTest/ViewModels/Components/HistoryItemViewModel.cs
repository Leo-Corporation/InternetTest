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
using InternetTest.Helpers;
using InternetTest.Models;
using System.Windows.Media;

namespace InternetTest.ViewModels.Components;
public class HistoryItemViewModel : ViewModelBase
{
	private string _name = string.Empty;
	public string Name { get => _name; set { _name = value; OnPropertyChanged(nameof(Name)); } }

	private string _statusText = string.Empty;
	public string StatusText { get => _statusText; set { _statusText = value; OnPropertyChanged(nameof(StatusText)); } }

	private string _dateText = string.Empty;
	public string DateText { get => _dateText; set { _dateText = value; OnPropertyChanged(nameof(DateText)); } }

	private SolidColorBrush? _statusForeground;
	public SolidColorBrush? StatusForeground { get => _statusForeground; set { _statusForeground = value; OnPropertyChanged(nameof(StatusForeground)); } }

	private SolidColorBrush? _statusBackground;
	public SolidColorBrush? StatusBackground { get => _statusBackground; set { _statusBackground = value; OnPropertyChanged(nameof(StatusBackground)); } }

	public HistoryItemViewModel(Activity activity)
	{
		Name = activity.Name;
		StatusText = activity.Result;
		DateText = activity.Date.ToString("g");

		switch (activity.Success)
		{
			case true:
				StatusBackground = ThemeHelper.GetSolidColorBrush("LightGreen");
				StatusForeground = ThemeHelper.GetSolidColorBrush("ForegroundGreen");
				break;
			case false:
				StatusBackground = ThemeHelper.GetSolidColorBrush("LightOrange");
				StatusForeground = ThemeHelper.GetSolidColorBrush("ForegroundOrange");
				break;
			default:
				StatusBackground = ThemeHelper.GetSolidColorBrush("LightFAccent");
				StatusForeground = ThemeHelper.GetSolidColorBrush("DarkFAccent");
				break;
		}
	}
}
