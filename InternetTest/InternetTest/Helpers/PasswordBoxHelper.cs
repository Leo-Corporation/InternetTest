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
using System.Windows;
using System.Windows.Controls;

namespace InternetTest.Helpers;


public static class PasswordBoxHelper
{
	public static readonly DependencyProperty BoundPassword =
		DependencyProperty.RegisterAttached("BoundPassword", typeof(string), typeof(PasswordBoxHelper),
		new PropertyMetadata(string.Empty, OnBoundPasswordChanged));

	public static string GetBoundPassword(DependencyObject obj)
	{
		return (string)obj.GetValue(BoundPassword);
	}

	public static void SetBoundPassword(DependencyObject obj, string value)
	{
		obj.SetValue(BoundPassword, value);
	}

	private static void OnBoundPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		if (d is PasswordBox passwordBox)
		{
			passwordBox.PasswordChanged -= PasswordChanged;
			passwordBox.Password = e.NewValue as string ?? string.Empty;
			passwordBox.PasswordChanged += PasswordChanged;
		}
	}

	private static void PasswordChanged(object sender, RoutedEventArgs e)
	{
		var passwordBox = sender as PasswordBox;
#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8604 // Possible null reference argument.
		SetBoundPassword(passwordBox, passwordBox.Password);
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CS8602 // Dereference of a possibly null reference.
	}
}
