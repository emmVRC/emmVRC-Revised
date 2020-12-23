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
        public static TextDisplayMenu noticeMenu;
        //public static TextDisplayMenu licensesMenu;
        public static void Initialize()
        {
            baseMenu = new PaginatedMenu(FunctionsMenu.baseMenu.menuBase, 7628, 12024, "Supporters", "", null);
            baseMenu.menuEntryButton.DestroyMe();
            baseMenu.pageItems.Add(new PageItem("<color=#E91E63>Emilia</color>", null, "Main developer of emmVRC"));
            baseMenu.pageItems.Add(new PageItem("<color=#E91E63>Hordini</color>", null, "Supporter, community manager, and major cutie"));
            baseMenu.pageItems.Add(new PageItem("<color=#E91E63>Brandon</color>", null, "Network developer"));
            baseMenu.pageItems.Add(new PageItem("<color=#E91E63>Janni9009</color>", null, "Developer and supporter"));
            baseMenu.pageItems.Add(new PageItem("<color=#E91E63>Supah</color>", null, "Major supporter and moderator"));
            baseMenu.pageItems.Add(new PageItem("<color=#C67228>mrgw98</color>", null, "Helper in the community"));
            baseMenu.pageItems.Add(new PageItem("<color=#C67228>Provania</color>", null, "Helper in the community"));
            baseMenu.pageItems.Add(new PageItem("<color=#C67228>rakosi2</color>", null, "Helper in the community"));
            baseMenu.pageItems.Add(new PageItem("<color=#C67228>RiskiVR</color>", null, "Helper in the community"));
            baseMenu.pageItems.Add(new PageItem("<color=#71368A>Herp\nDerpinstine</color>", null, "Major coding help, developer of MelonLoader"));
            baseMenu.pageItems.Add(new PageItem("<color=#71368A>knah</color>", null, "Developer of the Unhollower, the most essential part of IL2CPP modding"));
            baseMenu.pageItems.Add(new PageItem("<color=#71368A>Slaynash</color>", null, "Original developer of VRCModLoader, the entire reason why emmVRC exists today"));
            baseMenu.pageItems.Add(new PageItem("<color=#71368A>DubyaDude</color>", null, "Developer of the Ruby Button API"));
            baseMenu.pageItems.Add(new PageItem("<color=#88d184>DltDat</color>", null, "Developer of the hooking method used for various functions across emmVRC"));
            for (int i = 0; i < 3; i++)
                baseMenu.pageItems.Add(PageItem.Space);
            noticeMenu = new TextDisplayMenu(baseMenu.menuBase, 10293, 10221, "", "", null, "Notice to you",
            //  "This is an pre-release build! Do not distribute to others\n" +
                "emmVRC started off as a simple proof-of-concept for modding\n" +
                "VRChat. It originally only had a primitive Global Dynamic Bones\n" +
                "setup, and that was all. But seeing the potential of mods,\n" +
                "I set off to test the boundaries of what a simple Mono mod\n" +
                "could do.\n" +
                "\n" +
                "Fast forward an entire year later, and emmVRC is a far bigger\n" +
                "project than I ever could have imagined. Every day, we see\n" +
                "more and more users joining the Discord, and using the mod.\n" +
                "It's amazing to see, and I couldn't have done it without\n" +
                "all of you guys.\n" +
                "\n" +
                "Thank you for supporting emmVRC throughout the year,\n" +
                "and helping me keep doing what I do! -Emilia");
            noticeMenu.menuEntryButton.DestroyMe();
            baseMenu.pageItems.Add(new PageItem("<color=#FF69B4>Notice</color>", () => { noticeMenu.OpenMenu(); }, "..."));
            //baseMenu.pageItems.Add(new PageItem("404", null, "Major helper and mod developer"));
            /*for (int i = 0; i < 7; i++)
                baseMenu.pageItems.Add(PageItem.Space);
            baseMenu.pageItems.Add(new PageItem("Licenses", () => { 
            
            }, "Licenses for the components used in emmVRC"));*/
        }
    }
}
