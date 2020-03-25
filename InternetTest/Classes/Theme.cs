using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetTest.Classes
{
    public class Theme
    {
        public bool IsDark()
        {
            return Properties.Settings.Default.IsThemeDark; // Retourne false si le thème est clair
        }
    }
}
