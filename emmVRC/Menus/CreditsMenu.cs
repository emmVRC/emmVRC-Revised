using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using emmVRC.Libraries;
using UnityEngine;
using UnityEngine.Networking;


namespace emmVRC.Menus
{
    public class CreditsMenu
    {
        public static PaginatedMenu baseMenu;
        public static void Initialize()
        {
            baseMenu = new PaginatedMenu(FunctionsMenu.baseMenu.menuBase, 7628, 12024, "Supporters", "", null);
            baseMenu.menuEntryButton.DestroyMe();
            baseMenu.pageItems.Add(new PageItem("Emilia", null, "Main developer of emmVRC"));
            baseMenu.pageItems.Add(new PageItem("Hordini", null, "Supporter, community manager, and major cutie"));
            baseMenu.pageItems.Add(new PageItem("Herp\nDerpinstine", null, "Major coding help, developer of MelonLoader"));
            baseMenu.pageItems.Add(new PageItem("knah", null, "Developer of the Unhollower, the most essential part of IL2CPP modding"));
            baseMenu.pageItems.Add(new PageItem("DubyaDude", null, "Developer of the Ruby Button API"));
            baseMenu.pageItems.Add(new PageItem("DltDat", null, "Developer of the hooking method used for various functions across emmVRC"));
            baseMenu.pageItems.Add(new PageItem("Kitsune\nof\nNight", null, "Major supporter, and major cutie"));
            baseMenu.pageItems.Add(new PageItem("Xhail", null, "Network developer"));
            baseMenu.pageItems.Add(new PageItem("404", null, "Major helper and mod developer"));
            baseMenu.pageItems.Add(new PageItem("mrgw98", null, "Supporter and tester"));
            baseMenu.pageItems.Add(new PageItem("Janni9009", null, "Developer and supporter"));
            baseMenu.pageItems.Add(new PageItem("SupahMario", null, "Major supporter and moderator"));
            baseMenu.pageItems.Add(new PageItem("Snow", null, "Major supporter and a cutie"));
        }
    }
}
