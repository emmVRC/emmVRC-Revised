using emmVRC.Hacks;
using emmVRC.Managers;
using emmVRC.Menus;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using emmVRC.Objects.ModuleBases;

namespace emmVRC.Functions.Core
{
    [Priority(0)]
    public class ModCompatibility : MelonLoaderEvents
    {

        public static bool MultiplayerDynamicBones = false;
        public static bool PortalConfirmation = false;
        public static bool MControl = false;
        public static bool UIExpansionKit = false;
        public static bool FBTSaver = false;
        public static bool IKTweaks = false;
        public static bool BetterLoadingScreen = false;
        public static bool VRCMinus = false;
        public static bool ActionMenuAPI = false;

        public static GameObject FlightButton;
        public static GameObject NoclipButton;
        public static GameObject SpeedButton;
        public static GameObject ESPButton;

        public override void OnUiManagerInit()
        {
            if (MelonLoader.MelonHandler.Mods.Any(i => i.Info.Name == "MultiplayerDynamicBones" || i.Info.Name == "MultiplayerDynamicBonesMod"))
                MultiplayerDynamicBones = true;
            if (MelonLoader.MelonHandler.Mods.Any(i => i.Info.Name == "Portal Confirmation"))
                PortalConfirmation = true;
            if (MelonLoader.MelonHandler.Mods.Any(i => i.Info.Name == "MControl"))
                MControl = true;
            if (MelonLoader.MelonHandler.Mods.Any(i => i.Info.Name == "UI Expansion Kit"))
                UIExpansionKit = true;
            if (MelonLoader.MelonHandler.Mods.Any(i => i.Info.Name == "FBT Saver"))
                FBTSaver = true;
            if (MelonLoader.MelonHandler.Mods.Any(i => i.Info.Name == "IKTweaks"))
                IKTweaks = true;
            if (MelonLoader.MelonHandler.Mods.Any(i => i.Info.Name == "BetterLoadingScreen"))
                BetterLoadingScreen = true;
            if (MelonLoader.MelonHandler.Mods.Any(i => i.Info.Name == "ActionMenuApi"))
                ActionMenuAPI = true;

            if (MultiplayerDynamicBones)
                emmVRCLoader.Logger.LogDebug("Detected MultiplayerDynamicBones");
            if (PortalConfirmation)
                emmVRCLoader.Logger.LogDebug("Detected PortalConfirmation");
            if (UIExpansionKit)
            {
                emmVRCLoader.Logger.LogDebug("Detected UIExpansionKit");
                Functions.ModCompatibility.UIExpansionKitIntegration.Initialize();
            }
            if (FBTSaver)
                emmVRCLoader.Logger.LogDebug("Detected FBTSaver");
            if (IKTweaks)
                emmVRCLoader.Logger.LogDebug("Detected IKTweaks");
            if (BetterLoadingScreen)
                emmVRCLoader.Logger.LogDebug("Detected BetterLoadingScreen");

            if (ActionMenuAPI)
                emmVRCLoader.Logger.LogDebug("Detected ActionMenuApi");
        }
        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            if (buildIndex != -1) return;
            if (Functions.Core.ModCompatibility.MultiplayerDynamicBones && Configuration.JSONConfig.GlobalDynamicBonesEnabled)
            {
                Configuration.WriteConfigOption("GlobalDynamicBonesEnabled", false);
                Managers.emmVRCNotificationsManager.AddNotification(new Objects.Notification("emmVRC", null, "You are currently using MultiplayerDynamicBones. emmVRC's Global Dynamic Bones have been disabled, as only one can be used at a time.", true, false, null, "", "", true, null, "Dismiss"));
            }
            //if (ActionMenuAPI)
            //    Functions.ModCompatibility.ActionMenuAPIIntegration.Initialize();
        }
       
    }
}
