using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using emmVRC.Libraries;
using emmVRC.Managers;
using emmVRC.Objects;
using UnityEngine.Events;
using UnityEngine;
using UnityEngine.UI;
using UnhollowerRuntimeLib;
using System.Collections;

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
        private static PageItem playerHistoryButton;
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

            // If stealth mode is initialized, use the Report World menu instead
            if (Configuration.JSONConfig.StealthMode)
            {
                baseMenu.menuEntryButton.DestroyMe();
                QMSingleButton FunctionsButton = new QMSingleButton(ReportWorldMenu.baseMenu, 1, 0, "<color=#FF69B4>emmVRC</color>\nFunctions", () => { baseMenu.OpenMenu(); }, "Extra functions that can enhance the user experience or provide practical features");
            }

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

            playerHistoryButton = new PageItem("Player\nHistory", () => { PlayerHistoryMenu.ShowMenu(); }, "Allows you to view players who have entered this instance since you joined");
            baseMenu.pageItems.Add(playerHistoryButton);

            // Add the Settings button
            settingsButton = new PageItem("Settings", () => {
                SettingsMenu.LoadMenu();
            }, "Access the Settings for emmVRC, including Risky Functions, color changes, etc.");
            baseMenu.pageItems.Add(settingsButton);
            for (int i = 0; i <= 4; i++)
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

            if (!Libraries.ModCompatibility.MControl)
                MelonLoader.MelonCoroutines.Start(AddMediaKeys());
        }

        private static QMSingleButton PrevTrackButton;
        private static QMSingleButton PlayTrackButton;
        private static QMSingleButton StopTrackButton;
        private static QMSingleButton NextTrackButton;

        //Seperate method to more easily read or disable temporarily. Could also help with a cfg option.
        private static IEnumerator AddMediaKeys() {
            QuickMenuUtils.ResizeQuickMenuCollider();
            PrevTrackButton = new QMSingleButton(baseMenu.menuBase, 1, -2, "", MediaControl.PrevTrack, "Go to the previous song in your Playlist (click twice)\n or restart the current song");
            while (Resources.Media_Nav == null) yield return null;
            PrevTrackButton.getGameObject().GetComponent<Image>().sprite = Resources.Media_Nav;
            PrevTrackButton.getGameObject().transform.rotation *= Quaternion.Euler(0f, 0f, 180f);
            PlayTrackButton = new QMSingleButton(baseMenu.menuBase, 2, -2, "", MediaControl.PlayPause, "Pause or continue listening to the current song");
            while (Resources.Media_PlayPause == null) yield return null;
            PlayTrackButton.getGameObject().GetComponent<Image>().sprite = Resources.Media_PlayPause;
            StopTrackButton = new QMSingleButton(baseMenu.menuBase, 3, -2, "", MediaControl.Stop, "Stop the current song completely");
            while (Resources.Media_Stop == null) yield return null;
            StopTrackButton.getGameObject().GetComponent<Image>().sprite = Resources.Media_Stop;
            NextTrackButton = new QMSingleButton(baseMenu.menuBase, 4, -2, "", MediaControl.NextTrack, "Go to the next song in your Playlist");
            NextTrackButton.getGameObject().GetComponent<Image>().sprite = Resources.Media_Nav;
        }
    }
}
