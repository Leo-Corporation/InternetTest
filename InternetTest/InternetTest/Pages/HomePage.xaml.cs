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
using InternetTest.UserControls;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace InternetTest.Pages;
/// <summary>
/// Interaction logic for HomePage.xaml
/// </summary>
public partial class HomePage : Page
{
	public HomePage()
	{
		InitializeComponent();
		InitUI();
	}

	internal void InitUI()
	{
		// Load "Get started" section
		List<AppPages> relevantPages = Enumerable.Empty<AppPages>().ToList();
		List<AppActions> relevantActions = Enumerable.Empty<AppActions>().ToList();
		if (Global.SynethiaConfig is not null)
		{
			relevantPages = Global.GetMostRelevantPages(Global.SynethiaConfig);			
			Global.GetMostRelevantActions(Global.SynethiaConfig).ForEach((ActionInfo actionInfo) => relevantActions.Add(actionInfo.Action));
		}
		else
		{
			relevantPages = Global.DefaultRelevantPages;
			Global.DefaultRelevantActions.ForEach((ActionInfo actionInfo) => relevantActions.Add(actionInfo.Action));
		}

		for (int i = 0; i < 5; i++)
		{
			GetStartedPanel.Children.Add(new PageCard(relevantPages[i]));
		}
		relevantPages.RemoveRange(0, 5); // Remove already added pages; the least releavnt remains

		// Load "Discover" section
		for (int i = 0; i < relevantPages.Count; i++)
		{
			DiscoverPanel.Children.Add(new PageCard(relevantPages[i]));
		}

		// Load "Suggested actions" section
		for (int i = 0; i < 4; i++)
		{
			SuggestedActionsPanel.Children.Add(new ActionCard(relevantActions[i]));
		}
	}
}
