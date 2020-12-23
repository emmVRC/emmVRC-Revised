using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using emmVRC.Hacks;
using emmVRC.Libraries;
using emmVRC.Managers;
using emmVRC.Network.Objects;
using UnityEngine;

namespace emmVRC.Menus
{
    public class ActionMenuFunctions
    {
        public static CustomActionMenu.Page functionsMenu;
        public static CustomActionMenu.Page riskyFunctionsMenu;
        public static CustomActionMenu.Button flightButton;
        public static CustomActionMenu.Button noclipButton;
        public static CustomActionMenu.Button speedButton;
        public static CustomActionMenu.Button espButton;
        public static CustomActionMenu.Page avatarParametersMenu;
        public static CustomActionMenu.Button saveAvatarParameters;
        public static CustomActionMenu.Button clearAvatarParameters;

        private static bool justBecameActive = true;
        public static IEnumerator Initialize()
        {
            while (Resources.onlineSprite == null || Resources.onlineSprite.texture == null)
                yield return new WaitForEndOfFrame();
            functionsMenu = new CustomActionMenu.Page(CustomActionMenu.BaseMenu.MainMenu, "emmVRC\nFunctions", Resources.onlineSprite.texture);
            riskyFunctionsMenu = new CustomActionMenu.Page(functionsMenu, "Risky\nFunctions", Resources.flyTexture);
            flightButton = new CustomActionMenu.Button(riskyFunctionsMenu, "Flight:\nOff", () => {
                if (RiskyFunctionsManager.RiskyFuncsAreAllowed && Configuration.JSONConfig.RiskyFunctionsEnabled)
                    PlayerTweaksMenu.FlightToggle.setToggleState(!Flight.FlightEnabled, true);
            }, CustomActionMenu.ToggleOffTexture);
            noclipButton = new CustomActionMenu.Button(riskyFunctionsMenu, "Noclip:\nOff", () =>
            {
                if (RiskyFunctionsManager.RiskyFuncsAreAllowed && Configuration.JSONConfig.RiskyFunctionsEnabled)
                    PlayerTweaksMenu.NoclipToggle.setToggleState(!Flight.NoclipEnabled, true);
            }, CustomActionMenu.ToggleOffTexture);
            speedButton = new CustomActionMenu.Button(riskyFunctionsMenu, "Speed:\nOff", () =>
            {
                if (RiskyFunctionsManager.RiskyFuncsAreAllowed && Configuration.JSONConfig.RiskyFunctionsEnabled)
                    PlayerTweaksMenu.SpeedToggle.setToggleState(!Speed.SpeedModified, true);
            }, CustomActionMenu.ToggleOffTexture);
            espButton = new CustomActionMenu.Button(riskyFunctionsMenu, "ESP:\nOff", () =>
            {
                if (RiskyFunctionsManager.RiskyFuncsAreAllowed && Configuration.JSONConfig.RiskyFunctionsEnabled)
                    PlayerTweaksMenu.ESPToggle.setToggleState(!ESP.ESPEnabled, true);
            }, CustomActionMenu.ToggleOffTexture);

            avatarParametersMenu = new CustomActionMenu.Page(functionsMenu, "Avatar 3.0\nParameters", Resources.rpSprite.texture);
            saveAvatarParameters = new CustomActionMenu.Button(avatarParametersMenu, "Save", () =>
            {
                Hacks.AvatarPropertySaving.SaveAvatarParameters();
                VRCUiManager.prop_VRCUiManager_0.QueueHUDMessage("Avatar parameters have been saved.");
            }, Resources.saveTexture);
            clearAvatarParameters = new CustomActionMenu.Button(avatarParametersMenu, "Clear", () =>
            {
                Hacks.AvatarPropertySaving.ClearAvatarParameters();
                VRCUiManager.prop_VRCUiManager_0.QueueHUDMessage("Avatar parameters have been cleared.");
            }, Resources.deleteTexture);
            MelonLoader.MelonCoroutines.Start(Loop());
        }
        public static IEnumerator Loop()
        {
            while (true)
            {
                yield return new WaitForEndOfFrame();
                if (flightButton.currentPedalOption != null)
                {
                    if (RiskyFunctionsManager.RiskyFuncsAreAllowed && Configuration.JSONConfig.RiskyFunctionsEnabled)
                    {
                        flightButton.SetButtonText("Flight:\n" + (Flight.FlightEnabled ? "On" : "Off"));
                        flightButton.SetIcon(Flight.FlightEnabled ? Resources.toggleOnTexture : Resources.toggleOffTexture);
                        noclipButton.SetButtonText("Noclip:\n" + (Flight.NoclipEnabled ? "On" : "Off"));
                        noclipButton.SetIcon(Flight.NoclipEnabled ? Resources.toggleOnTexture : Resources.toggleOffTexture);
                        speedButton.SetButtonText("Speed:\n" + (Speed.SpeedModified ? "On" : "Off"));
                        speedButton.SetIcon(Speed.SpeedModified ? Resources.toggleOnTexture : Resources.toggleOffTexture);
                        espButton.SetButtonText("ESP:\n" + (ESP.ESPEnabled ? "On" : "Off"));
                        espButton.SetIcon(ESP.ESPEnabled ? Resources.toggleOnTexture : Resources.toggleOffTexture);
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
            }
        }
    }
}
