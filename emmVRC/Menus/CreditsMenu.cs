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
        //public static TextDisplayMenu licensesMenu;
        public static void Initialize()
        {
            baseMenu = new PaginatedMenu(FunctionsMenu.baseMenu.menuBase, 7628, 12024, "Supporters", "", null);
            baseMenu.menuEntryButton.DestroyMe();
            baseMenu.pageItems.Add(new PageItem("<color=#E91E63>Emilia</color>", null, "Main developer of emmVRC"));
            baseMenu.pageItems.Add(new PageItem("<color=#E91E63>Hordini</color>", null, "Supporter, community manager, and major cutie"));
            baseMenu.pageItems.Add(new PageItem("<color=#E91E63>Brandon</color>", null, "Network developer"));
            baseMenu.pageItems.Add(new PageItem("<color=#E91E63>Janni9009</color>", null, "Developer and supporter"));
            baseMenu.pageItems.Add(new PageItem("<color=#E91E63>SupahMario</color>", null, "Major supporter and moderator"));
            baseMenu.pageItems.Add(new PageItem("<color=#C67228>mrgw98</color>", null, "Helper in the community"));
            baseMenu.pageItems.Add(new PageItem("<color=#C67228>Provania</color>", null, "Helper in the community"));
            baseMenu.pageItems.Add(new PageItem("<color=#C67228>Cutie\nLewko</color>", null, "Helper in the community"));
            baseMenu.pageItems.Add(new PageItem("<color=#C67228>RiskiVR</color>", null, "Helper in the community"));
            baseMenu.pageItems.Add(new PageItem("<color=#71368A>Herp\nDerpinstine</color>", null, "Major coding help, developer of MelonLoader"));
            baseMenu.pageItems.Add(new PageItem("<color=#71368A>knah</color>", null, "Developer of the Unhollower, the most essential part of IL2CPP modding"));
            baseMenu.pageItems.Add(new PageItem("<color=#71368A>Slaynash</color>", null, "Original developer of VRCModLoader, the entire reason why emmVRC exists today"));
            baseMenu.pageItems.Add(new PageItem("<color=#71368A>DubyaDude</color>", null, "Developer of the Ruby Button API"));
            baseMenu.pageItems.Add(new PageItem("<color=#88d184>DltDat</color>", null, "Developer of the hooking method used for various functions across emmVRC"));
            
            //baseMenu.pageItems.Add(new PageItem("404", null, "Major helper and mod developer"));
            /*for (int i = 0; i < 7; i++)
                baseMenu.pageItems.Add(PageItem.Space);
            baseMenu.pageItems.Add(new PageItem("Licenses", () => { 
            
            }, "Licenses for the components used in emmVRC"));*/
        }
    }
}
