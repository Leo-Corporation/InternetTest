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

namespace InternetTest.Pages.FirstRunPages;

/// <summary>
/// Interaction logic for LanguagePage.xaml
/// </summary>
public partial class LanguagePage : Page
{
	public LanguagePage()
	{
		InitializeComponent();
		InitUI(); // Load the UI
	}

	private void InitUI()
	{
		// Load LangComboBox
		LangComboBox.Items.Clear(); // Clear
		LangComboBox.Items.Add(Properties.Resources.Default); // Add "default"

		for (int i = 0; i < Global.LanguageList.Count; i++)
		{
			LangComboBox.Items.Add(Global.LanguageList[i]);
		}

		LangComboBox.SelectedIndex = (Global.Settings.Language == "_default") ? 0 : Global.LanguageCodeList.IndexOf(Global.Settings.Language) + 1;
		LangApplyBtn.Visibility = Visibility.Collapsed; // Hide
	}

	private void LangComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		LangApplyBtn.Visibility = Visibility.Visible; // Show
	}

	private void LangApplyBtn_Click(object sender, RoutedEventArgs e)
	{
		Global.Settings.Language = LangComboBox.Text switch
		{
			"English (United States)" => Global.LanguageCodeList[0], // Set the settings value
			"Français (France)" => Global.LanguageCodeList[1], // Set the settings value
			"中文（简体）" => Global.LanguageCodeList[2], // Set the settings value
			_ => "_default" // Set the settings value
		};
		SettingsManager.Save(); // Save the changes
		LangApplyBtn.Visibility = Visibility.Hidden; // Hide
	}
}
