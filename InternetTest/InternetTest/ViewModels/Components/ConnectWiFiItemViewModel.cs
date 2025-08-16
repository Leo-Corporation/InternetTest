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
using InternetTest.Helpers;
using InternetTest.Models;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace InternetTest.ViewModels.Components;

public class ConnectWiFiItemViewModel : ViewModelBase
{
	private string? _name;
	public string? Name { get => _name; set { _name = value; OnPropertyChanged(nameof(Name)); } }

	private string? _strengthIcon;
	public string? StrengthIcon { get => _strengthIcon; set { _strengthIcon = value; OnPropertyChanged(nameof(StrengthIcon)); } }

	private SolidColorBrush? _strengthColor;
	public SolidColorBrush? StrengthColor { get => _strengthColor; set { _strengthColor = value; OnPropertyChanged(nameof(StrengthColor)); } }

	public ObservableCollection<GridItemViewModel> Details { get; }

	private readonly WiFiNetwork _wiFiNetwork;

	public ICommand CopyCommand => new RelayCommand(Copy);
	public ConnectWiFiItemViewModel(WiFiNetwork wiFiNetwork)
	{
		_wiFiNetwork = wiFiNetwork;
		Name = _wiFiNetwork.Ssid;
		StrengthIcon = _wiFiNetwork.SignalQuality switch
		{
			int n when (n is >= 0 and < 25) => "\uF8B3",
			int n when (n is >= 25 and < 50) => "\uF8B1",
			int n when (n is >= 50 and < 75) => "\uF8AF",
			int n when (n is >= 75 and <= 100) => "\uF8AD",
			_ => "\uF8AD",
		};

		StrengthColor = _wiFiNetwork.SignalQuality switch
		{
			int n when (n is >= 0 and < 25) => ThemeHelper.GetSolidColorBrush("Red"),
			int n when (n is >= 25 and < 50) => ThemeHelper.GetSolidColorBrush("Orange"),
			int n when (n is >= 50 and < 75) => ThemeHelper.GetSolidColorBrush("Accent"),
			int n when (n is >= 75 and <= 100) => ThemeHelper.GetSolidColorBrush("Green"),
			_ => ThemeHelper.GetSolidColorBrush("Green"), // Default to green
		};

		Details = [
			new(Properties.Resources.SignalQuality, _wiFiNetwork.SignalQuality.ToString(), 0, 0),
			new(Properties.Resources.ProfileName, _wiFiNetwork.ProfileName ?? "", 0, 1),
			new(Properties.Resources.Interface, _wiFiNetwork.InterfaceDescription ?? "", 0, 2),
			new(Properties.Resources.BssType, _wiFiNetwork.BssType ?? "", 1, 0),
			new(Properties.Resources.SecurityEnabled, _wiFiNetwork.IsSecurityEnabled ? Properties.Resources.Yes : Properties.Resources.No, 1, 1),
			new(Properties.Resources.Channel, _wiFiNetwork.Channel.ToString() ?? "", 1,2),
			new(Properties.Resources.Band, $"{_wiFiNetwork.Band:0.0} GHz", 2, 0),
			new(Properties.Resources.Frequency	, $"{_wiFiNetwork.Frequency} kHz", 2, 1),
		];
	}

	private void Copy(object? o)
	{
		Clipboard.SetDataObject(_wiFiNetwork.ToString());
	}
}
