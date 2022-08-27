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
using System.Windows;
using System.Windows.Controls;

namespace InternetTest.UserControls;
/// <summary>
/// Interaction logic for IpHistoryItem.xaml
/// </summary>
public partial class IpHistoryItem : UserControl
{
	IPInfo IPInfo { get; init; }

	StackPanel ParentElement { get; init; }

	public IpHistoryItem(IPInfo iPInfo, StackPanel parentElement)
	{
		InitializeComponent();
		IPInfo = iPInfo; // Set the IPInfo
		ParentElement = parentElement; // Set the parent element

		// Add the IP to the list of Located IPs
		Global.LocatedIPs.Add(IPInfo.Query);

		InitUI(); // Initialize the UI
	}

	private void InitUI()
	{
		QueryTxt.Text = IPInfo.Query; // Set the query text
	}

	private void LocateBtn_Click(object sender, RoutedEventArgs e)
	{
		Global.LocalizeIPPage.IPTxt.Text = IPInfo.Query; // Set the IP text
		Global.LocalizeIPPage.HistoryBtn_Click(this, null); // Hide the history
		Global.LocalizeIPPage.IPRadioBtn.IsChecked = true; // Set the IP radio button as checked
		Global.LocalizeIPPage.LocalizeBtn_Click(this, null); // Locate the IP
	}

	private void DismissBtn_Click(object sender, RoutedEventArgs e)
	{
		ParentElement.Children.Remove(this); // Remove the item from the parent element
		Global.LocatedIPs.Remove(IPInfo.Query);

		if (ParentElement.Children.Count == 0) // If there are no more items in the parent element
		{
			Global.LocalizeIPPage.HistoryBtn_Click(this, null); // Hide the history
		}
	}
}
