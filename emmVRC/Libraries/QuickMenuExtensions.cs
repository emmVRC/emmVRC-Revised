using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace emmVRC.Libraries
{
    public static class QuickMenuExtensions
    {
        public static void MainMenu(this QuickMenu quickMenu, int page, bool dunnolol = false) =>
            quickMenu.Method_Public_Void_Int32_Boolean_0(page, dunnolol);
    }
}
