//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using emmVRC.Libraries;
//using emmVRC.Managers;
//using emmVRC.Objects;
//using UnityEngine.Events;
//using UnityEngine;
//using UnityEngine.UI;
//using UnhollowerRuntimeLib;
//using System.Collections;
//using emmVRC.Objects.ModuleBases;

//namespace emmVRC.Menus
//{
//    [Priority(-1)]
//    public class FunctionsMenuLegacy : MelonLoaderEvents
//    {
//        public static PaginatedMenu baseMenu;
//        public override void OnUiManagerInit()
//        {
//            emmVRCLoader.Logger.LogDebug("Initializing Functions menu...");

//            // Initialize the base menu for the Functions menu
//            baseMenu = new PaginatedMenu("ShortcutMenu", Configuration.JSONConfig.FunctionsButtonX, Configuration.JSONConfig.FunctionsButtonY, "<color=#FF69B4>emmVRC</color>\nFunctions", "Extra functions that can enhance the user experience or provide practical features", null);

//            // If stealth mode is initialized, use the Report World menu instead
//            //if (Configuration.JSONConfig.StealthMode)
//            //{
//            //    baseMenu.menuEntryButton.DestroyMe();
//            //    QMSingleButton FunctionsButton = new QMSingleButton(ReportWorldMenu.baseMenu, 1, 0, "<color=#FF69B4>emmVRC</color>\nFunctions", () => { baseMenu.OpenMenu(); }, "Extra functions that can enhance the user experience or provide practical features");
//            //}

//            // Add the World Tweaks button
//            baseMenu.pageItems.Add(PageItem.Space);

//            // Add the Player Tweaks button
//            baseMenu.pageItems.Add(PageItem.Space);

//            // Add the Instance History button
//            baseMenu.pageItems.Add(PageItem.Space);

//            // Add the Disabled Buttons button
//            baseMenu.pageItems.Add(PageItem.Space);

//            // Add the Programs button
//            baseMenu.pageItems.Add(PageItem.Space);

//            baseMenu.pageItems.Add(PageItem.Space);

//            // Add the Settings button
//            baseMenu.pageItems.Add(PageItem.Space);
//            baseMenu.pageItems.Add(PageItem.Space);
//            for (int i = 0; i <= 3; i++)
//            {
//                baseMenu.pageItems.Add(PageItem.Space);
//            }
//            baseMenu.pageItems.Add(new PageItem("<color=#ee006c>emmVRC\nTeam</color>", () => { CreditsMenu.baseMenu.OpenMenu(); }, "View all the users that make this project possible! <3"));

//            baseMenu.pageItems.Add(PageItem.Space);
//            baseMenu.pageItems.Add(PageItem.Space);

//            baseMenu.pageItems.Add(new PageItem("<color=#eac81e>Supporters</color>", () => { SupporterMenu.LoadMenu(); }, "Shows all the current supporters of the emmVRC project! Thank you to everyone who has donated so far! <3"));

//            baseMenu.pageItems.Add(new PageItem("Force\nQuit", () => { DestructiveActions.ForceQuit(); }, "Quits the game, instantly."));
//            baseMenu.pageItems.Add(new PageItem("Instant\nRestart", () => { DestructiveActions.ForceRestart(); }, "Restarts the game, instantly."));

//            //if (!Functions.Core.ModCompatibility.MControl)
//            //    MelonLoader.MelonCoroutines.Start(AddMediaKeys());
//        }

//        //private static QMSingleButton PrevTrackButton;
//        //private static QMSingleButton PlayTrackButton;
//        //private static QMSingleButton StopTrackButton;
//        //private static QMSingleButton NextTrackButton;

//        //Seperate method to more easily read or disable temporarily. Could also help with a cfg option.
//        //private static IEnumerator AddMediaKeys()
//        //{
//        //    QuickMenuUtils.ResizeQuickMenuCollider();
//        //    PrevTrackButton = new QMSingleButton(baseMenu.menuBase, 1, -2, "", MediaControl.PrevTrack, "Go to the previous song in your Playlist (click twice)\n or restart the current song");
//        //    while (Functions.Core.Resources.Media_Nav == null) yield return null;
//        //    PrevTrackButton.getGameObject().GetComponent<Image>().sprite = Functions.Core.Resources.Media_Nav;
//        //    PrevTrackButton.getGameObject().transform.rotation *= Quaternion.Euler(0f, 0f, 180f);
//        //    PlayTrackButton = new QMSingleButton(baseMenu.menuBase, 2, -2, "", MediaControl.PlayPause, "Pause or continue listening to the current song");
//        //    while (Functions.Core.Resources.Media_PlayPause == null) yield return null;
//        //    PlayTrackButton.getGameObject().GetComponent<Image>().sprite = Functions.Core.Resources.Media_PlayPause;
//        //    StopTrackButton = new QMSingleButton(baseMenu.menuBase, 3, -2, "", MediaControl.Stop, "Stop the current song completely");
//        //    while (Functions.Core.Resources.Media_Stop == null) yield return null;
//        //    StopTrackButton.getGameObject().GetComponent<Image>().sprite = Functions.Core.Resources.Media_Stop;
//        //    NextTrackButton = new QMSingleButton(baseMenu.menuBase, 4, -2, "", MediaControl.NextTrack, "Go to the next song in your Playlist");
//        //    NextTrackButton.getGameObject().GetComponent<Image>().sprite = Functions.Core.Resources.Media_Nav;
//        //}
//    }
//}
