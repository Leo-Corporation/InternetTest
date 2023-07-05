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
using System.Net.NetworkInformation;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace InternetTest.UserControls;
/// <summary>
/// Interaction logic for TraceRouteItem.xaml
/// </summary>
public partial class TraceRouteItem : UserControl
{
	TracertStep TracertStep { get; init; }
	bool IsLast { get; init; }
	public TraceRouteItem(TracertStep tracertStep, bool isLast)
	{
		InitializeComponent();
		TracertStep = tracertStep;
		IsLast = isLast;
		InitUI();
	}

	void InitUI()
	{
		IdTxt.Text = $"#{TracertStep.TTL}";
		NameTxt.Text = $"{TracertStep.Address}";
		TimeTxt.Text = $"{TracertStep.RoundtripTime}ms";
		IconTxt.Text = TracertStep.Status switch
		{
			IPStatus.Success => "\uF299",
			IPStatus.TtlExpired => "\uF299",
			_ => "\uF36E"
		};
		IconTxt.Foreground = TracertStep.Status switch
		{
			IPStatus.Success => new SolidColorBrush { Color = Global.GetColorFromResource("Green") },
			IPStatus.TtlExpired => new SolidColorBrush { Color = Global.GetColorFromResource("Green") },
			_ => new SolidColorBrush { Color = Global.GetColorFromResource("Red") }
		};

		if (TracertStep.Status == IPStatus.TimedOut)
		{
			NameTxt.Text = Properties.Resources.TimedOut;
		}

		if (TracertStep.Status != IPStatus.Success && TracertStep.Status != IPStatus.TtlExpired && TracertStep.Status != IPStatus.TimedOut)
		{
			NameTxt.Text += $" ({TracertStep.Status})";
		}

		TopElipse.Visibility = TracertStep.TTL == 1 ? Visibility.Collapsed : Visibility.Visible;
		BottomElipse.Visibility = IsLast ? Visibility.Collapsed : Visibility.Visible;
	}

	private void NameTxt_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
	{
		Clipboard.SetText(((TextBlock)sender).Text);
	}
}
