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
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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

namespace InternetTest.Pages
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {
        bool isAvailable;
        public SettingsPage()
        {
            InitializeComponent();
        }



        private void RefreshInstallBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ThemeApplyBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void LangComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void LangApplyBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void LightRadioBtn_Checked(object sender, RoutedEventArgs e)
        {
            ThemeApplyBtn.Visibility = Visibility.Visible; // Show the ThemeApplyBtn button
        }

        private void DarkRadioBtn_Checked(object sender, RoutedEventArgs e)
        {
            ThemeApplyBtn.Visibility = Visibility.Visible; // Show the ThemeApplyBtn button
        }

        /// <summary>
        /// Restarts InternetTest.
        /// </summary>
        private void DisplayRestartMessage()
        {
            if (MessageBox.Show(Properties.Resources.NeedRestartToApplyChanges, Properties.Resources.InternetTest, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Process.Start(Directory.GetCurrentDirectory() + @"\Gerayis.exe"); // Start
                Environment.Exit(0); // Close
            }
        }
    }
}
