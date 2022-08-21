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
/// Interaction logic for WiFiInfoItem.xaml
/// </summary>
public partial class WiFiInfoItem : UserControl
{
    bool codeInjected = !Global.Settings.UseSynethia;
    private WLANProfile WLANProfile { get; init; }
    public WiFiInfoItem(WLANProfile profile)
    {
        InitializeComponent();
        WLANProfile = profile;

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
                Global.SynethiaConfig.WiFiPasswordsPageInfo.InteractionCount++;
            };
        }

        InterfaceNameTxt.Text = WLANProfile.SSIDConfig?.SSID?.Name;
        ConnectionModeTxt.Text = WLANProfile.ConnectionMode == "auto" ? Properties.Resources.Automatic : WLANProfile.ConnectionMode;
        ConnectionTypeTxt.Text = WLANProfile.ConnectionType switch
        {
            "ESS" => Properties.Resources.InfrastructureNetwork,
            "IBSS" => Properties.Resources.AdHocNetwork,
            _ => WLANProfile.ConnectionType
        };
        AuthTxt.Text = WLANProfile.MSM?.Security?.AuthEncryption?.Authentication;
        EncryptionTxt.Text = WLANProfile.MSM?.Security?.AuthEncryption?.Encryption;
        PasswordTxt.Text = new string('*', WLANProfile.MSM?.Security?.SharedKey?.KeyMaterial?.Length ?? 0);
    }

    private void CopyBtn_Click(object sender, RoutedEventArgs e)
    {
        Clipboard.SetDataObject(WLANProfile.ToString());
    }

    private void ExpanderBtn_Click(object sender, RoutedEventArgs e)
    {
        CollapseGrid.Visibility = CollapseGrid.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        ExpanderBtn.Content = CollapseGrid.Visibility != Visibility.Visible ? "\uF2A4" : "\uF2B7";
    }

    bool keyShown = false;
    private void ShowKeyBtn_Click(object sender, RoutedEventArgs e)
    {
        keyShown = !keyShown; // toggle
        PasswordTxt.Text = !keyShown ? new string('*', WLANProfile.MSM?.Security?.SharedKey?.KeyMaterial?.Length ?? 0) : WLANProfile.MSM?.Security?.SharedKey?.KeyMaterial;
        ShowKeyBtn.Content = keyShown ? "\uF3F8" : "\uF3FC"; // Set the icon
    }

    private void CopyKeyBtn_Click(object sender, RoutedEventArgs e)
    {
        Clipboard.SetDataObject(WLANProfile.MSM?.Security?.SharedKey?.KeyMaterial);
    }

    public override string ToString() => WLANProfile.SSIDConfig?.SSID?.Name;
}
