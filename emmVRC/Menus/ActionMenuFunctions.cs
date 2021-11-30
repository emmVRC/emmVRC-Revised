using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using emmVRC.Hacks;
using emmVRC.Libraries;
using emmVRC.Managers;
using emmVRC.Objects.ModuleBases;
using UnityEngine;

namespace emmVRC.Menus
{
    public class ActionMenuFunctions : MelonLoaderEvents, IWithLateUpdate
    {
        public static CustomActionMenu.Page functionsMenu;
        public static CustomActionMenu.Page favouriteEmojisMenu;
        public static CustomActionMenu.Page riskyFunctionsMenu;
        public static List<CustomActionMenu.Button> favouriteEmojiButtons;
        public static CustomActionMenu.Button flightButton;
        public static CustomActionMenu.Button noclipButton;
        public static CustomActionMenu.Button speedButton;
        public static CustomActionMenu.Button espButton;
        public static CustomActionMenu.Page avatarParametersMenu;
        public static CustomActionMenu.Button saveAvatarParameters;
        public static CustomActionMenu.Button clearAvatarParameters;
        private static EmojiMenu emojiMenu;
        private static bool playingEmoji = false;
        private static int currentEmojiPlaying = -1;

        //private static bool justBecameActive = true;
        public override void OnUiManagerInit()
        {
            MelonLoader.MelonCoroutines.Start(Initialize());
        }
        public static IEnumerator Initialize()
        {
            while (Functions.Core.Resources.onlineSprite == null || Functions.Core.Resources.onlineSprite.texture == null)
                yield return new WaitForEndOfFrame();
            functionsMenu = new CustomActionMenu.Page(CustomActionMenu.BaseMenu.MainMenu, "emmVRC\nFunctions", Functions.Core.Resources.onlineSprite.texture);
            //if (Configuration.JSONConfig.ActionMenuAPIIntegration)
            //    functionsMenu.menuEntryButton.SetVisible(false);
            riskyFunctionsMenu = new CustomActionMenu.Page(functionsMenu, "Risky\nFunctions", Functions.Core.Resources.flyTexture);
            favouriteEmojisMenu = new CustomActionMenu.Page(functionsMenu, "Favorite\nEmojis", Functions.Core.Resources.rpSprite.texture);
            favouriteEmojiButtons = new List<CustomActionMenu.Button>();
            for (int i = 0; i < 8; i++)
            {
                int currentEmojiButtonOption = i;
                favouriteEmojiButtons.Add(new CustomActionMenu.Button(favouriteEmojisMenu, "", () =>
                {
                    emmVRCLoader.Logger.LogDebug("Trying to spawn Emoji " + Configuration.JSONConfig.FavouritedEmojis[currentEmojiButtonOption]);
                    emojiMenu.TriggerEmoji(Configuration.JSONConfig.FavouritedEmojis[currentEmojiButtonOption]);
                    playingEmoji = true;
                    currentEmojiPlaying = Configuration.JSONConfig.FavouritedEmojis[currentEmojiButtonOption];
                    MelonLoader.MelonCoroutines.Start(EmojiTimeout());
                }));
            };

            flightButton = new CustomActionMenu.Button(riskyFunctionsMenu, "Flight:\nOff", () =>
            {
                if (RiskyFunctionsManager.AreRiskyFunctionsAllowed && Configuration.JSONConfig.RiskyFunctionsEnabled)
                {
                    if (Functions.PlayerHacks.Flight.IsNoClipEnabled && Functions.PlayerHacks.Flight.IsFlyEnabled)
                        Functions.PlayerHacks.Flight.SetNoClipActive(false);
                    Functions.PlayerHacks.Flight.SetFlyActive(!Functions.PlayerHacks.Flight.IsFlyEnabled);
                }
            }, CustomActionMenu.ToggleOffTexture);
            noclipButton = new CustomActionMenu.Button(riskyFunctionsMenu, "Noclip:\nOff", () =>
            {
                if (RiskyFunctionsManager.AreRiskyFunctionsAllowed && Configuration.JSONConfig.RiskyFunctionsEnabled)
                {
                    if (!Functions.PlayerHacks.Flight.IsFlyEnabled && !Functions.PlayerHacks.Flight.IsNoClipEnabled)
                        Functions.PlayerHacks.Flight.SetFlyActive(true);
                    Functions.PlayerHacks.Flight.SetNoClipActive(!Functions.PlayerHacks.Flight.IsNoClipEnabled);
                }
            }, CustomActionMenu.ToggleOffTexture);
            speedButton = new CustomActionMenu.Button(riskyFunctionsMenu, "Speed:\nOff", () =>
            {
                if (RiskyFunctionsManager.AreRiskyFunctionsAllowed && Configuration.JSONConfig.RiskyFunctionsEnabled)
                    Functions.PlayerHacks.Speed.SetActive(!Functions.PlayerHacks.Speed.IsEnabled);
            }, CustomActionMenu.ToggleOffTexture);
            espButton = new CustomActionMenu.Button(riskyFunctionsMenu, "ESP:\nOff", () =>
            {
                if (RiskyFunctionsManager.AreRiskyFunctionsAllowed && Configuration.JSONConfig.RiskyFunctionsEnabled)
                    Functions.PlayerHacks.ESP.SetActive(!Functions.PlayerHacks.ESP.IsEnabled);
            }, CustomActionMenu.ToggleOffTexture);

            /*avatarParametersMenu = new CustomActionMenu.Page(functionsMenu, "Avatar 3.0\nParameters", Resources.rpSprite.texture);
            saveAvatarParameters = new CustomActionMenu.Button(avatarParametersMenu, "Save", () =>
            {
                Hacks.AvatarPropertySaving.SaveAvatarParameters();
                VRCUiManager.prop_VRCUiManager_0.QueueHUDMessage("Avatar parameters have been saved.");
            }, Resources.saveTexture);
            clearAvatarParameters = new CustomActionMenu.Button(avatarParametersMenu, "Clear", () =>
            {
                Hacks.AvatarPropertySaving.ClearAvatarParameters();
                VRCUiManager.prop_VRCUiManager_0.QueueHUDMessage("Avatar parameters have been cleared.");
            }, Resources.deleteTexture);*/
        }
        public void LateUpdate()
        {
            if (functionsMenu == null) return;
            if (emojiMenu == null)
            {
               // emojiMenu = QuickMenu.prop_QuickMenu_0.transform.Find("EmojiMenu").GetComponent<EmojiMenu>();
            }
            if (flightButton.currentPedalOption != null)
            {
                if (RiskyFunctionsManager.AreRiskyFunctionsAllowed && Configuration.JSONConfig.RiskyFunctionsEnabled)
                {
                    flightButton.SetButtonText("Flight:\n" + (Functions.PlayerHacks.Flight.IsFlyEnabled ? "On" : "Off"));
                    flightButton.SetIcon(Functions.PlayerHacks.Flight.IsFlyEnabled ? Functions.Core.Resources.toggleOnTexture : Functions.Core.Resources.toggleOffTexture);
                    noclipButton.SetButtonText("Noclip:\n" + (Functions.PlayerHacks.Flight.IsNoClipEnabled ? "On" : "Off"));
                    noclipButton.SetIcon(Functions.PlayerHacks.Flight.IsNoClipEnabled ? Functions.Core.Resources.toggleOnTexture : Functions.Core.Resources.toggleOffTexture);
                    speedButton.SetButtonText("Speed:\n" + (Functions.PlayerHacks.Speed.IsEnabled ? "On" : "Off"));
                    speedButton.SetIcon(Functions.PlayerHacks.Speed.IsEnabled ? Functions.Core.Resources.toggleOnTexture : Functions.Core.Resources.toggleOffTexture);
                    espButton.SetButtonText("ESP:\n" + (Functions.PlayerHacks.ESP.IsEnabled ? "On" : "Off"));
                    espButton.SetIcon(Functions.PlayerHacks.ESP.IsEnabled ? Functions.Core.Resources.toggleOnTexture : Functions.Core.Resources.toggleOffTexture);
                    flightButton.SetEnabled(true);
                    noclipButton.SetEnabled(true);
                    speedButton.SetEnabled(true);
                    espButton.SetEnabled(true);
                }
                else
                {
                    flightButton.SetEnabled(false);
                    noclipButton.SetEnabled(false);
                    speedButton.SetEnabled(false);
                    espButton.SetEnabled(false);
                }
            }
            /*if (EmojiFavourites.AvailableEmojis != null && EmojiFavourites.AvailableEmojis.Count != 0)
            {
                for (int i = 0; i < Configuration.JSONConfig.FavouritedEmojis.Count; i++)
                    favouriteEmojiButtons[i].SetIcon(EmojiFavourites.AvailableEmojis[Configuration.JSONConfig.FavouritedEmojis[i]].GetComponent<ParticleSystemRenderer>().material.mainTexture.Cast<Texture2D>());
                for (int i = 0; i < 8 - Configuration.JSONConfig.FavouritedEmojis.Count; i++)
                    favouriteEmojiButtons[Configuration.JSONConfig.FavouritedEmojis.Count + i].SetIcon(null);
            }*/
            if (favouriteEmojiButtons.All(a => a.currentPedalOption != null))
            {
                for (int i = 0; i < Configuration.JSONConfig.FavouritedEmojis.Count; i++)
                    if (favouriteEmojiButtons[i].currentPedalOption != null)
                        favouriteEmojiButtons[i].SetEnabled(!playingEmoji);
                for (int i = 0; i < 8 - Configuration.JSONConfig.FavouritedEmojis.Count; i++)
                    if (favouriteEmojiButtons[Configuration.JSONConfig.FavouritedEmojis.Count + i].currentPedalOption != null)
                        favouriteEmojiButtons[Configuration.JSONConfig.FavouritedEmojis.Count + i].SetEnabled(false);
            }
        }
        private static IEnumerator EmojiTimeout()
        {
            yield return new WaitForSeconds(2f);
            playingEmoji = false;
            currentEmojiPlaying = -1;
        }
    }
}
