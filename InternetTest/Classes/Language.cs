using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetTest.Classes
{
    public class Language
    {
        public string GetName(string returnLang)
        {
            string res = "";
            if (returnLang == "fr") // Si la langue est française
            {
                if (Properties.Settings.Default.Language == "fr-FR")
                {
                    res = "Français (fr-FR)";
                }
                else if (Properties.Settings.Default.Language == "EN")
                {
                    res = "Anglais (EN)";
                }
            }
            else if (returnLang == "en") // Si la langue est anglaise
            {
                if (Properties.Settings.Default.Language == "fr-FR")
                {
                    res = "French (fr-FR)";
                }
                else if (Properties.Settings.Default.Language == "EN")
                {
                    res = "English (EN)";
                }
            }
            return res;
        }
        public string GetCode()
        {
            return Properties.Settings.Default.Language;
        }
    }
}
