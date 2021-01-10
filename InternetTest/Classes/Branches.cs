using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetTest.Classes
{
    public class Branches
    {
        string branch = "master";
        // branch can be equal to:
        //  - master
        //  - Version_Next
        public string GetBranch()
        {
            return branch; // Obtient la branche actuelle du logiciel
        }
        public System.Drawing.Icon IconBranch() // Retourne l'icône à afficher en fonction de la branche
        {
            System.Drawing.Icon icon;
            if (branch == "Version_Next")
            {
                icon = Properties.Resources.InternetTest_Preview1; // Si la branche est "Version_Next", alors mettre icône "Preview"
            }
            else
            {
                icon = Properties.Resources.InternetTest_Logo; // Sinon, mettre l'icône normal
            }
            return icon;
        }
        public System.Drawing.Bitmap ImageBranch()
        {
            System.Drawing.Bitmap bitmap;
            if (branch == "Version_Next")
            {
                bitmap = Properties.Resources.InternetTest_Preview;
            }
            else
            {
                bitmap = Properties.Resources.InternetTestLogo1;
            }
            return bitmap;
        }
    }
}
