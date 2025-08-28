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
using InternetTest.Enums;
using MicaWPF.Core.Enums;
using MicaWPF.Core.Services;
using Microsoft.Win32;
using PeyrSharp.Enums;
using PeyrSharp.Env;
using System.Windows;
using System.Windows.Media;

namespace InternetTest.Helpers;

public class ThemeHelper
{
	public static SolidColorBrush GetSolidColorBrush(string resource) => (SolidColorBrush)Application.Current.Resources[resource];

	public static void ChangeTheme(Theme theme)
	{
		bool isDark = theme switch
		{
			Theme.Dark => true,
			Theme.Light => false,
			Theme.System => IsSystemThemeDark(),
			_ => false
		};

		var dictionary = new ResourceDictionary
		{
			Source = new Uri($"..\\Themes\\{(isDark ? "Dark" : "Light")}.xaml", UriKind.Relative)
		};

		Application.Current.Resources.MergedDictionaries.Clear();
		Application.Current.Resources.MergedDictionaries.Add(dictionary);

		MicaWPFServiceUtility.ThemeService.ChangeTheme(isDark ? WindowsTheme.Dark : WindowsTheme.Light);
	}

	public static bool IsSystemThemeDark()
	{
		if (Sys.CurrentWindowsVersion is not WindowsVersion.Windows10 and not WindowsVersion.Windows11)
		{
			return false; // Avoid errors on older OSs
		}

		var t = Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", "SystemUsesLightTheme", "1");
		return t switch
		{
			0 => true,
			1 => false,
			_ => false
		};
	}
}
