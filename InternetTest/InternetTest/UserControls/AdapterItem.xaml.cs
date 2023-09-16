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
using InternetTest.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InternetTest.UserControls
{
	/// <summary>
	/// Interaction logic for AdapterItem.xaml
	/// </summary>
	public partial class AdapterItem : UserControl
	{
		public AdapterInfo AdapterInfo { get; }
		public AdapterItem(AdapterInfo adapterInfo)
		{
			InitializeComponent();
			AdapterInfo = adapterInfo;
			InitUI();
		}

		private void InitUI()
		{
			NameTxt.Text = AdapterInfo.Name;
			AdapterIcon.Text = AdapterInfo.NetworkInterfaceType switch
			{
				NetworkInterfaceType.Tunnel => "\uF18E",
				NetworkInterfaceType.Ethernet => "\uFB32",
				NetworkInterfaceType.Ethernet3Megabit => "\uFB32",
				NetworkInterfaceType.FastEthernetFx => "\uFB32",
				NetworkInterfaceType.FastEthernetT => "\uFB32",
				NetworkInterfaceType.GigabitEthernet => "\uFB32",
				_ => "\uF8AC"
			};

			StatusTxt.Text = AdapterInfo.Status switch
			{
				OperationalStatus.Up => Properties.Resources.ConnectedS,
				OperationalStatus.Down => Properties.Resources.Disconnected,
				_ => AdapterInfo.Status.ToString()
			};
			InterfaceTypeTxt.Text = Global.GetInterfaceTypeName(AdapterInfo.NetworkInterfaceType);
			SpeedTxt.Text = $"{Global.GetStorageUnit(AdapterInfo.Speed).Item2:0.00} {Global.UnitToString(Global.GetStorageUnit(AdapterInfo.Speed).Item1)}/s";
			SentBytesTxt.Text = $"{Global.GetStorageUnit(AdapterInfo.BytesSent).Item2:0.00} {Global.UnitToString(Global.GetStorageUnit(AdapterInfo.BytesSent).Item1)}";
			ReceivedBytesTxt.Text = $"{Global.GetStorageUnit(AdapterInfo.BytesReceived).Item2:0.00} {Global.UnitToString(Global.GetStorageUnit(AdapterInfo.BytesReceived).Item1)}";
		}

		private void AdvancedBtn_Click(object sender, RoutedEventArgs e)
		{
			new AdapterWindow(AdapterInfo).Show();
		}
	}
}
