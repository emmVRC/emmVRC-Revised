using emmVRC.Libraries;
using emmVRC.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using emmVRC.Objects.ModuleBases;

namespace emmVRC.Menus
{
    public class ChangelogMenu : MelonLoaderEvents
    {
        public static ScrollingTextMenu baseMenu;
        public override void OnUiManagerInit()
        {
            baseMenu = new ScrollingTextMenu(FunctionsMenuLegacy.baseMenu.menuBase, 1049, 1023, "Changelog", "ree", null, "<color=#FF69B4>emmVRC</color> version " + Attributes.Version + " (" + Attributes.DateUpdated+")", Attributes.Changelog, "See the full Changelog on Discord", () => { System.Diagnostics.Process.Start("https://discord.gg/SpZSH5Z"); }, "Click to join the Discord (launches in a browser window)");
            baseMenu.menuEntryButton.DestroyMe();
        }
    }
}
