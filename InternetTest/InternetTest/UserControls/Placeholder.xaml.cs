using System;
using System.Collections.Generic;
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

namespace InternetTest.UserControls;
/// <summary>
/// Interaction logic for Placeholder.xaml
/// </summary>
public partial class Placeholder : UserControl
{
	public Placeholder(string text, string icon)
	{
		InitializeComponent();
		Text.Text = text;
		IconTxt.Text = icon;
	}
}
