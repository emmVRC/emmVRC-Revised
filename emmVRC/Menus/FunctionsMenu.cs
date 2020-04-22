using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using emmVRC.Libraries;
using UnityEngine.Events;

namespace emmVRC.Menus
{
    public class FunctionsMenu
    {
        public static PaginatedMenu baseMenu;
        private static PageItem instanceHistoryButton;
        private static PageItem disabledButtonsButton;
        private static PageItem settingsButton;
        private static PageItem forceQuitButton;
        private static PageItem instantRestartButton;
        public static void Initialize()
        {
            // Initialize the base menu for the Functions menu
            baseMenu = new PaginatedMenu("ShortcutMenu", Configuration.JSONConfig.FunctionsButtonX, Configuration.JSONConfig.FunctionsButtonY, "<color=#FF69B4>emmVRC</color>\nFunctions", "Extra functions that can enhance the user experience or provide practical features", null);

            disabledButtonsButton = new PageItem("Disabled\nButtons", () => { DisabledButtonMenu.LoadMenu(); }, "Contains buttons from the Quick Menu that were disabled by emmVRC");
            baseMenu.pageItems.Add(disabledButtonsButton);
            // TODO: Add buttons here. For now, this adds an empty space
            settingsButton = new PageItem("Settings", () => {
                SettingsMenu.LoadMenu();
            }, "Access the Settings for emmVRC, including Risky Functions, color changes, etc.");
            baseMenu.pageItems.Add(settingsButton);
            for (int i=0; i <= 8; i++)
            {
                baseMenu.pageItems.Add(PageItem.Space());
            }
            forceQuitButton = new PageItem("Force\nQuit", () => { DestructiveActions.ForceQuit(); }, "Quits the game, instantly.");
            baseMenu.pageItems.Add(forceQuitButton);
            instantRestartButton = new PageItem("Instant\nRestart", () => { DestructiveActions.ForceRestart(); }, "Restarts the game, instantly.");
            baseMenu.pageItems.Add(instantRestartButton);
        }
    }
}
