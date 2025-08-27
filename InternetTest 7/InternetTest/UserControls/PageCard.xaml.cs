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
using InternetTest.Enums;
using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace InternetTest.UserControls;
/// <summary>
/// Interaction logic for PageCard.xaml
/// </summary>
public partial class PageCard : UserControl
{

	internal AppPages AppPage { get; init; }
	public PageCard(AppPages appPages)
	{
		InitializeComponent();
		AppPage = appPages; // Set value

		InitUI();
	}

	private void InitUI()
	{
		IconTxt.Text = Global.AppPagesFilledIcons[AppPage]; // Set text
		PageNameTxt.Text = Global.AppPagesName[AppPage]; // Set text
	}
	public static event EventHandler<PageEventArgs> OnCardClick;

	private void Border_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
	{
		OnCardClick?.Invoke(this, new(AppPage)); // Invoke event
	}
}
