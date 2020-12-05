using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetTest.Classes
{
    public static class Theme
    {
        public static bool IsDark()
        {
            return Properties.Settings.Default.IsThemeDark; // Retourne false si le thème est clair
        }
    }
}
