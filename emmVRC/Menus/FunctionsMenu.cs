using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using emmVRC.Libraries;
using emmVRC.Managers;
using emmVRC.Objects;
using UnityEngine.Events;

namespace emmVRC.Menus
{
    public class FunctionsMenu
    {
        public static PaginatedMenu baseMenu;
        private static PageItem worldTweaksButton;
        private static PageItem playerTweaksButton;
        private static PageItem instanceHistoryButton;
        private static PageItem disabledButtonsButton;
        private static PageItem programsButton;
        private static PageItem settingsButton;
        private static PageItem creditsButton;
        private static PageItem changelogButton;
        private static PageItem debugMenuButton;
        private static PageItem supporterButton;
        private static PageItem forceQuitButton;
        private static PageItem instantRestartButton;
        public static void Initialize()
        {
            // Initialize the base menu for the Functions menu
            baseMenu = new PaginatedMenu("ShortcutMenu", Configuration.JSONConfig.FunctionsButtonX, Configuration.JSONConfig.FunctionsButtonY, "<color=#FF69B4>emmVRC</color>\nFunctions", "Extra functions that can enhance the user experience or provide practical features", null);

            // Add the World Tweaks button
            worldTweaksButton = new PageItem("World\nTweaks", () => { QuickMenuUtils.ShowQuickmenuPage(WorldTweaksMenu.baseMenu.getMenuName()); }, "Contains tweaks to affect the world around you");
            baseMenu.pageItems.Add(worldTweaksButton);

            // Add the Player Tweaks button
            playerTweaksButton = new PageItem("Player\nTweaks", () => { QuickMenuUtils.ShowQuickmenuPage(PlayerTweaksMenu.baseMenu.getMenuName()); }, "Contains tweaks to affect your movement, as well as the players around you");
            baseMenu.pageItems.Add(playerTweaksButton);

            // Add the Instance History button
            instanceHistoryButton = new PageItem("Instance\nHistory", () => { InstanceHistoryMenu.baseMenu.OpenMenu(); InstanceHistoryMenu.LoadMenu(); }, "Allows you to join an instance you were previously in, so long as you have not been kicked from it");
            baseMenu.pageItems.Add(instanceHistoryButton);

            // Add the Disabled Buttons button
            disabledButtonsButton = new PageItem("Disabled\nButtons", () => { DisabledButtonMenu.LoadMenu(); }, "Contains buttons from the Quick Menu that were disabled by emmVRC");
            baseMenu.pageItems.Add(disabledButtonsButton);

            // Add the Programs button
            programsButton = new PageItem("Programs", () => { ProgramMenu.baseMenu.OpenMenu(); }, "Lets you launch external programs from within VRChat.");
            baseMenu.pageItems.Add(programsButton);

            // Add the Settings button
            settingsButton = new PageItem("Settings", () => {
                SettingsMenu.LoadMenu();
            }, "Access the Settings for emmVRC, including Risky Functions, color changes, etc.");
            baseMenu.pageItems.Add(settingsButton);
            for (int i=0; i <= 5; i++)
            {
                baseMenu.pageItems.Add(PageItem.Space);
            }
            creditsButton = new PageItem("<color=#ee006c>emmVRC\nTeam</color>", () => { CreditsMenu.baseMenu.OpenMenu(); }, "View all the users that make this project possible! <3");
            debugMenuButton = new PageItem("Debug", () => { QuickMenuUtils.ShowQuickmenuPage(DebugMenu.menuBase.getMenuName()); }, "Contains debug actions to test emmVRC and the modding environment as a whole");
            baseMenu.pageItems.Add(creditsButton);
            if (Attributes.Debug)
                baseMenu.pageItems.Add(debugMenuButton);
            else
                baseMenu.pageItems.Add(PageItem.Space);
            changelogButton = new PageItem("Changelog", () => { ChangelogMenu.baseMenu.OpenMenu(); }, "Check the changes with the current build of emmVRC");
            baseMenu.pageItems.Add(changelogButton);



            supporterButton = new PageItem("<color=#eac81e>Supporters</color>", () => { SupporterMenu.LoadMenu(); }, "Shows all the current supporters of the emmVRC project! Thank you to everyone who has donated so far! <3");
            baseMenu.pageItems.Add(supporterButton);

            forceQuitButton = new PageItem("Force\nQuit", () => { DestructiveActions.ForceQuit(); }, "Quits the game, instantly.");
            baseMenu.pageItems.Add(forceQuitButton);
            instantRestartButton = new PageItem("Instant\nRestart", () => { DestructiveActions.ForceRestart(); }, "Restarts the game, instantly.");
            baseMenu.pageItems.Add(instantRestartButton);
        }
    }
}
