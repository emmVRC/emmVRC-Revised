using emmVRC.Libraries;
using emmVRC.Hacks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRC.Core;
using UnityEngine;
using emmVRC.Objects;
using emmVRC.Objects.ModuleBases;

namespace emmVRC.Menus
{
    public class PlayerHistoryMenuLegacy : MelonLoaderEvents
    {
        public static PaginatedMenu baseMenu;
        private static PageItem toggleHistory;
        private static PageItem toggleJoinLeaveLog;
        //public static List<string> currentPlayersNames;
        public static int Timeout = 0;
        private static List<PageItem> currentInstancePlayers;
        public override void OnUiManagerInit()
        {
            baseMenu = new PaginatedMenu(FunctionsMenuLegacy.baseMenu.menuBase, 10293, 12934, "Player\nHistory", "If you're reading this, hi!", null);
            baseMenu.menuEntryButton.DestroyMe();
            toggleHistory = new PageItem("Enable", () => {
                Configuration.WriteConfigOption("PlayerHistoryEnable", true);
            }, "Disable", () => {
                Configuration.WriteConfigOption("PlayerHistoryEnable", false);
            }, "TOGGLE: Enables or disables the Player History");
            toggleJoinLeaveLog = new PageItem("Log Join\nand Leaves", () =>
            {
                Configuration.WriteConfigOption("LogPlayerJoin", true);
            }, "Disable", () => {
                Configuration.WriteConfigOption("LogPlayerJoin", false);
            }, "TOGGLE: Enables the logging of the names of players to the console and the emmVRC log upon joining and leaving. Please disable this before sending support requests!");
            baseMenu.pageItems.Add(toggleHistory);
            baseMenu.pageItems.Add(toggleJoinLeaveLog);
            currentInstancePlayers = new List<PageItem>();
            //currentPlayersNames = new List<string>();
            
        }
        public static void ShowMenu()
        {
            toggleHistory.SetToggleState(Configuration.JSONConfig.PlayerHistoryEnable);
            toggleJoinLeaveLog.SetToggleState(Configuration.JSONConfig.LogPlayerJoin);
            if (currentInstancePlayers.Count > 0)
            {
                foreach (PageItem itm in currentInstancePlayers)
                {
                    baseMenu.pageItems.Remove(itm);
                }
            }
            currentInstancePlayers = new List<PageItem>();
            foreach (InstancePlayer plr in Functions.PlayerHacks.PlayerHistory.currentPlayers)
            {
                PageItem player = new PageItem(plr.Name, () => {
                    if (Timeout == 0 && NetworkConfig.Instance.APICallsAllowed)
                    {
                        APIUser.FetchUser(plr.UserID, new System.Action<APIUser>((VRC.Core.APIUser usr) => { QuickMenu.prop_QuickMenu_0.field_Private_APIUser_0 = usr; QuickMenu.prop_QuickMenu_0.Method_Public_Void_EnumNPublicSealedvaUnWoAvSoSeUsDeSaCuUnique_Boolean_0((QuickMenu.EnumNPublicSealedvaUnWoAvSoSeUsDeSaCuUnique)4, false); }), new System.Action<string>((string str) => {
                            emmVRCLoader.Logger.LogError("API returned an error: " + str);
                        }));
                        Timeout = 5;
                        MelonLoader.MelonCoroutines.Start(WaitForTimeout());
                    }
                }, "Joined "+plr.TimeJoinedStamp, true);
                baseMenu.pageItems.Add(player);
                currentInstancePlayers.Add(player);
            }
            baseMenu.OpenMenu();
        }
        public static IEnumerator WaitForTimeout()
        {
            while (Timeout != 0)
            {
                yield return new WaitForSeconds(1f);
                Timeout--;
            }
        }
    }
    
}
