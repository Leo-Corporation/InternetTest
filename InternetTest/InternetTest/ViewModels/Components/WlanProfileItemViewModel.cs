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
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace InternetTest.ViewModels.Components;
public class WlanProfileItemViewModel : ViewModelBase
{
	private readonly WlanProfile _wlanProfile;

	public ObservableCollection<GridItemViewModel> Details { get; }

	private string _name = string.Empty;
	public string Name { get => _name; set { _name = value; OnPropertyChanged(nameof(Name)); } }

	private string _key = string.Empty;
	public string Key { get => _key; set { _key = value; OnPropertyChanged(nameof(Key)); } }

	private bool _keyVisible;
	public bool KeyVisible
	{
		get => _keyVisible;
		set
		{
			_keyVisible = value;
			Key = !value ? new string('*', _wlanProfile.MSM?.Security?.SharedKey?.KeyMaterial?.Length ?? 0) : _wlanProfile.MSM?.Security?.SharedKey?.KeyMaterial ?? Properties.Resources.Unknown;
			OnPropertyChanged(nameof(KeyVisible));
		}
	}

	public ICommand CopyCommand => new RelayCommand(o =>
	{
		Clipboard.SetDataObject(_wlanProfile.ToString());
	});

	public ICommand CopyKeyCommand => new RelayCommand(o =>
	{
		Clipboard.SetDataObject(_wlanProfile.MSM?.Security?.SharedKey?.KeyMaterial ?? Properties.Resources.Unknown);
	});

	public WlanProfileItemViewModel(WlanProfile wlanProfile)
	{
		_wlanProfile = wlanProfile;

		Name = _wlanProfile.Name ?? Properties.Resources.Unknown;
		KeyVisible = false;

		Details = [
			new (Properties.Resources.Authentication, _wlanProfile.MSM?.Security?.AuthEncryption?.Authentication switch
				{
					"open" => Properties.Resources.OpenNetwork,
					"shared" => Properties.Resources.SharedNetwork,
					"WPA" => Properties.Resources.WPANetwork,
					"WPAPSK" => Properties.Resources.WPAPSKNetwork,
					"WPA2" => Properties.Resources.WPA2Network,
					"WPA2PSK" => Properties.Resources.WPA2PSKNetwork,
					_ => _wlanProfile.MSM?.Security?.AuthEncryption?.Authentication
				} ?? Properties.Resources.Unknown, 0, 0),
			new(Properties.Resources.Encryption, _wlanProfile.MSM?.Security?.AuthEncryption?.Encryption ?? Properties.Resources.Unknown, 1, 0),
			new(Properties.Resources.ConnectionMode, _wlanProfile.ConnectionMode switch
				{
					"auto" => Properties.Resources.Automatic,
					"manual" => Properties.Resources.Manual,
					_ => _wlanProfile.ConnectionMode
				} ?? Properties.Resources.Unknown, 0, 1),
			new(Properties.Resources.ConnectionType, _wlanProfile.ConnectionType switch
				{
					"ESS" => Properties.Resources.InfrastructureNetwork,
					"IBSS" => Properties.Resources.AdHocNetwork,
					_ => _wlanProfile.ConnectionType
				} ?? Properties.Resources.Unknown, 1, 1)
		];
	}
}
