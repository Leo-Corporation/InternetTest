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
using InternetTest.Models;
using InternetTest.ViewModels.Components;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace InternetTest.ViewModels.Windows;
public class DnsDetailsWindowViewModel : ViewModelBase
{
	private ObservableCollection<GridItemViewModel> _details = [];
	public ObservableCollection<GridItemViewModel> Details { get => _details; set { _details = value; OnPropertyChanged(nameof(Details)); } }

	private ObservableCollection<GridItemViewModel> _advancedDetails = [];
	public ObservableCollection<GridItemViewModel> AdvancedDetails { get => _advancedDetails; set { _advancedDetails = value; OnPropertyChanged(nameof(AdvancedDetails)); } }

	private string _name = string.Empty;
	public string Name { get => _name; set { _name = value; OnPropertyChanged(nameof(Name)); } }

	public ICommand CopyCommand { get; init; }
	public DnsDetailsWindowViewModel(DnsCacheInfo dnsCacheInfo)
	{
		Name = dnsCacheInfo.Name;
		Details = [
			new (Properties.Resources.Entry, dnsCacheInfo.Entry, 0, 0),
			new (Properties.Resources.RecordName, dnsCacheInfo.Name, 0, 1),
			new(Properties.Resources.Data, dnsCacheInfo.Data, 1, 0),
			new(Properties.Resources.Status, ((Enums.Status)dnsCacheInfo.Status).ToString(), 1, 1),
			new(Properties.Resources.Type, ((Enums.Types)dnsCacheInfo.Type).ToString(), 2, 0),
		];

		AdvancedDetails = [
			new(Properties.Resources.Description, dnsCacheInfo.Description?.ToString() ?? Properties.Resources.NA, 0, 0),
			new(Properties.Resources.DataLength, dnsCacheInfo.DataLength.ToString(), 0, 1),
			new(Properties.Resources.Section, ((Enums.Section)dnsCacheInfo.Section).ToString(), 1, 0),
			new(Properties.Resources.TimeToLive, $"{dnsCacheInfo.TimeToLive} ms", 1, 1),
			new(Properties.Resources.Caption, dnsCacheInfo.Caption?.ToString() ?? Properties.Resources.NA, 2, 0),
			new(Properties.Resources.PsComputerName, dnsCacheInfo.PsComputerName?.ToString() ?? Properties.Resources.NA, 2, 1),
		];

		CopyCommand = new RelayCommand(o =>
		{
			Clipboard.SetDataObject(dnsCacheInfo.ToString());
		});
	}
}
