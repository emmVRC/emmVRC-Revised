using emmVRC.Libraries;
using emmVRC.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace emmVRC.Menus
{
    public class ChangelogMenu
    {
        public static TextDisplayMenu baseMenu;
        public static void Initialize()
        {
            baseMenu = new TextDisplayMenu(FunctionsMenu.baseMenu.menuBase, 1049, 1023, "Changelog", "ree", null, "<color=#FF69B4>emmVRC</color> version " + Attributes.Version + " (" + Attributes.DateUpdated+")", Attributes.Changelog);
            baseMenu.menuEntryButton.DestroyMe();
        }
    }
}
