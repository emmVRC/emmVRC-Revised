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
using emmVRC.Objects.ModuleBases;

namespace emmVRC.Menus
{
    [Priority(-1)]
    public class FunctionsMenu : MelonLoaderEvents
    {
        public static PaginatedMenu baseMenu;
        public override void OnUiManagerInit()
        {
            emmVRCLoader.Logger.LogDebug("Initializing Functions menu...");

            // Initialize the base menu for the Functions menu
            baseMenu = new PaginatedMenu("ShortcutMenu", Configuration.JSONConfig.FunctionsButtonX, Configuration.JSONConfig.FunctionsButtonY, "<color=#FF69B4>emmVRC</color>\nFunctions", "Extra functions that can enhance the user experience or provide practical features", null);

            // If stealth mode is initialized, use the Report World menu instead
            if (Configuration.JSONConfig.StealthMode)
            {
                baseMenu.menuEntryButton.DestroyMe();
                QMSingleButton FunctionsButton = new QMSingleButton(ReportWorldMenu.baseMenu, 1, 0, "<color=#FF69B4>emmVRC</color>\nFunctions", () => { baseMenu.OpenMenu(); }, "Extra functions that can enhance the user experience or provide practical features");
            }
            if (Configuration.JSONConfig.TabMode && !Configuration.JSONConfig.StealthMode)
                baseMenu.menuEntryButton.getGameObject().transform.localScale = Vector3.zero;

            // Add the World Tweaks button
            baseMenu.pageItems.Add(new PageItem("World\nTweaks", () => { WorldTweaksMenu.baseMenu.Open(); }, "Contains tweaks to affect the world around you"));

            // Add the Player Tweaks button
            baseMenu.pageItems.Add(new PageItem("Player\nTweaks", () => { PlayerTweaksMenu.baseMenu.Open(); }, "Contains tweaks to affect your movement, as well as the players around you"));

            // Add the Instance History button
            baseMenu.pageItems.Add(new PageItem("Instance\nHistory", () => { InstanceHistoryMenu.baseMenu.OpenMenu(); InstanceHistoryMenu.LoadMenu(); }, "Allows you to join an instance you were previously in, so long as you have not been kicked from it"));

            // Add the Disabled Buttons button
            baseMenu.pageItems.Add(new PageItem("Disabled\nButtons", () => { DisabledButtonMenu.LoadMenu(); }, "Contains buttons from the Quick Menu that were disabled by emmVRC"));

            // Add the Programs button
            baseMenu.pageItems.Add(new PageItem("Programs", () => { ProgramMenu.baseMenu.OpenMenu(); }, "Lets you launch external programs from within VRChat."));

            baseMenu.pageItems.Add(new PageItem("Player\nHistory", () => { PlayerHistoryMenu.ShowMenu(); }, "Allows you to view players who have entered this instance since you joined"));

            // Add the Settings button
            baseMenu.pageItems.Add(new PageItem("Settings", () => { SettingsMenu.LoadMenu(); }, "Access the Settings for emmVRC, including Risky Functions, color changes, etc."));
            baseMenu.pageItems.Add(new PageItem("Alarm\nClocks", () => { Hacks.AlarmClock.baseMenu.Open(); }, "Configure emmVRC's alarm clocks"));
            for (int i = 0; i <= 3; i++)
            {
                baseMenu.pageItems.Add(PageItem.Space);
            }
            baseMenu.pageItems.Add(new PageItem("<color=#ee006c>emmVRC\nTeam</color>", () => { CreditsMenu.baseMenu.OpenMenu(); }, "View all the users that make this project possible! <3"));
            if (Attributes.Debug)
                baseMenu.pageItems.Add(new PageItem("Debug", () => { DebugMenu.menuBase.Open(); }, "Contains debug actions to test emmVRC and the modding environment as a whole"));
            else
                baseMenu.pageItems.Add(PageItem.Space);
            baseMenu.pageItems.Add(new PageItem("Changelog", () => { ChangelogMenu.baseMenu.OpenMenu(); }, "Check the changes with the current build of emmVRC"));



            baseMenu.pageItems.Add(new PageItem("<color=#eac81e>Supporters</color>", () => { SupporterMenu.LoadMenu(); }, "Shows all the current supporters of the emmVRC project! Thank you to everyone who has donated so far! <3"));

            baseMenu.pageItems.Add(new PageItem("Force\nQuit", () => { DestructiveActions.ForceQuit(); }, "Quits the game, instantly."));
            baseMenu.pageItems.Add(new PageItem("Instant\nRestart", () => { DestructiveActions.ForceRestart(); }, "Restarts the game, instantly."));

            if (!Functions.Core.ModCompatibility.MControl)
                MelonLoader.MelonCoroutines.Start(AddMediaKeys());
        }

        private static QMSingleButton PrevTrackButton;
        private static QMSingleButton PlayTrackButton;
        private static QMSingleButton StopTrackButton;
        private static QMSingleButton NextTrackButton;

        //Seperate method to more easily read or disable temporarily. Could also help with a cfg option.
        private static IEnumerator AddMediaKeys()
        {
            QuickMenuUtils.ResizeQuickMenuCollider();
            PrevTrackButton = new QMSingleButton(baseMenu.menuBase, 1, -2, "", MediaControl.PrevTrack, "Go to the previous song in your Playlist (click twice)\n or restart the current song");
            while (Functions.Core.Resources.Media_Nav == null) yield return null;
            PrevTrackButton.getGameObject().GetComponent<Image>().sprite = Functions.Core.Resources.Media_Nav;
            PrevTrackButton.getGameObject().transform.rotation *= Quaternion.Euler(0f, 0f, 180f);
            PlayTrackButton = new QMSingleButton(baseMenu.menuBase, 2, -2, "", MediaControl.PlayPause, "Pause or continue listening to the current song");
            while (Functions.Core.Resources.Media_PlayPause == null) yield return null;
            PlayTrackButton.getGameObject().GetComponent<Image>().sprite = Functions.Core.Resources.Media_PlayPause;
            StopTrackButton = new QMSingleButton(baseMenu.menuBase, 3, -2, "", MediaControl.Stop, "Stop the current song completely");
            while (Functions.Core.Resources.Media_Stop == null) yield return null;
            StopTrackButton.getGameObject().GetComponent<Image>().sprite = Functions.Core.Resources.Media_Stop;
            NextTrackButton = new QMSingleButton(baseMenu.menuBase, 4, -2, "", MediaControl.NextTrack, "Go to the next song in your Playlist");
            NextTrackButton.getGameObject().GetComponent<Image>().sprite = Functions.Core.Resources.Media_Nav;
        }
    }
}
