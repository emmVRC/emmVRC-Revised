using emmVRC.Libraries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace emmVRC.Menus
{
    public class PlayerHistoryMenu
    {
        public static PaginatedMenu baseMenu;
        private static PageItem toggleHistory;
        public static List<string> currentPlayersNames;
        private static List<PageItem> currentInstancePlayers;
        public static void Initialize()
        {
            baseMenu = new PaginatedMenu(FunctionsMenu.baseMenu.menuBase, 10293, 12934, "Player\nHistory", "If you're reading this, hi!", null);
            baseMenu.menuEntryButton.DestroyMe();
            toggleHistory = new PageItem("Enable", () => {
                Configuration.JSONConfig.PlayerHistoryEnable = true;
                Configuration.SaveConfig();
            }, "Disable", () => {
                Configuration.JSONConfig.PlayerHistoryEnable = false;
                Configuration.SaveConfig();
            }, "TOGGLE: Enables or disables the Player History");
            baseMenu.pageItems.Add(toggleHistory);
            currentInstancePlayers = new List<PageItem>();
            currentPlayersNames = new List<string>();
        }
        public static void ShowMenu()
        {
            toggleHistory.SetToggleState(Configuration.JSONConfig.PlayerHistoryEnable);
            if (currentInstancePlayers.Count > 0)
            {
                foreach (PageItem itm in currentInstancePlayers)
                {
                    baseMenu.pageItems.Remove(itm);
                }
            }
            currentInstancePlayers = new List<PageItem>();
            foreach (string str in currentPlayersNames)
            {
                PageItem player = new PageItem(str, null, "", true);
                baseMenu.pageItems.Add(player);
                currentInstancePlayers.Add(player);
            }

            baseMenu.OpenMenu();
        }
    }
    
}
