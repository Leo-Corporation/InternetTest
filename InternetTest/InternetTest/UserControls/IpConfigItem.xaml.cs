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

namespace InternetTest.UserControls;
/// <summary>
/// Interaction logic for IpConfigItem.xaml
/// </summary>
public partial class IpConfigItem : UserControl
{
    bool codeInjected = !Global.Settings.UseSynethia;
    private WindowsIpConfig WindowsIpConfig { get; init; }
    public IpConfigItem(WindowsIpConfig windowsIpConfig)
    {
        InitializeComponent();
        WindowsIpConfig = windowsIpConfig;

        InitUI();
    }

    private void InitUI()
    {
        if (codeInjected) return;
        codeInjected = true;
        foreach (Button b in Global.FindVisualChildren<Button>(this))
        {
            b.Click += (sender, e) =>
            {
                Global.SynethiaConfig.StatusPageInfo.InteractionCount++;
            };
        }

        StatusTxt.Text = WindowsIpConfig.Status == OperationalStatus.Up ? Properties.Resources.ConnectedS : Properties.Resources.Disconnected;
        Ipv4Txt.Text = WindowsIpConfig.IPv4Address;
        GatewayIpv4Txt.Text = WindowsIpConfig.IPv4Gateway;
        MaskIpTxt.Text = WindowsIpConfig.IPv4Mask;
        Ipv6Txt.Text = WindowsIpConfig.IPv6Address;
        GatewayIpv6Txt.Text = WindowsIpConfig.IPv6Gateway;
        InterfaceNameTxt.Text = WindowsIpConfig.Name;
        DNSPrefixTxt.Text = WindowsIpConfig.DNSSuffix;

        GatewayIpv4TitleTxt.Visibility = GatewayIpv4Txt.Text == "" ? Visibility.Collapsed : Visibility.Visible;
        GatewayIpv6TitleTxt.Visibility = GatewayIpv6Txt.Text == "" ? Visibility.Collapsed : Visibility.Visible;
        GatewayIpv4Txt.Visibility = GatewayIpv4Txt.Text == "" ? Visibility.Collapsed : Visibility.Visible;
        GatewayIpv6Txt.Visibility = GatewayIpv6Txt.Text == "" ? Visibility.Collapsed : Visibility.Visible;

        if (WindowsIpConfig.Status == OperationalStatus.Up)
        {
            CollapseGrid.Visibility = CollapseGrid.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            ExpanderBtn.Content = CollapseGrid.Visibility != Visibility.Visible ? "\uF2A4" : "\uF2B7";
        }
    }

    private void ExpanderBtn_Click(object sender, RoutedEventArgs e)
    {
        CollapseGrid.Visibility = CollapseGrid.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        ExpanderBtn.Content = CollapseGrid.Visibility != Visibility.Visible ? "\uF2A4" : "\uF2B7";
    }

    private void CopyBtn_Click(object sender, RoutedEventArgs e)
    {
        Clipboard.SetDataObject(WindowsIpConfig.ToString());
    }
}
