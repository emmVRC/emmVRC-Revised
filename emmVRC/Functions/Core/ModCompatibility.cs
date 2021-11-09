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
        public static bool OGTrustRank = false;
        public static bool UIExpansionKit = false;
        public static bool FBTSaver = false;
        public static bool IKTweaks = false;
        public static bool BetterLoadingScreen = false;
        public static bool VRCMinus = false;

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
            if (MelonLoader.MelonHandler.Mods.Any(i => i.Info.Name == "OGTrustRanks"))
                OGTrustRank = true;
            if (MelonLoader.MelonHandler.Mods.Any(i => i.Info.Name == "UI Expansion Kit"))
                UIExpansionKit = true;
            if (MelonLoader.MelonHandler.Mods.Any(i => i.Info.Name == "FBT Saver"))
                FBTSaver = true;
            if (MelonLoader.MelonHandler.Mods.Any(i => i.Info.Name == "IKTweaks"))
                IKTweaks = true;
            if (MelonLoader.MelonHandler.Mods.Any(i => i.Info.Name == "BetterLoadingScreen"))
                BetterLoadingScreen = true;
            if (MelonLoader.MelonHandler.Mods.Any(i => i.Info.Name == "VRC-Minus"))
                VRCMinus = true;


            if (MultiplayerDynamicBones)
                emmVRCLoader.Logger.LogDebug("Detected MultiplayerDynamicBones");
            if (PortalConfirmation)
                emmVRCLoader.Logger.LogDebug("Detected PortalConfirmation");
            if (MControl)
                emmVRCLoader.Logger.LogDebug("Detected MControl");
            if (OGTrustRank)
                emmVRCLoader.Logger.LogDebug("Detected OGTrustRank");
            if (UIExpansionKit)
            {
                emmVRCLoader.Logger.LogDebug("Detected UIExpansionKit");
                Configuration.onConfigUpdated.Add(new System.Collections.Generic.KeyValuePair<string, Action>("UIColorChangingEnabled", () => {
                    if (Configuration.JSONConfig.UIExpansionKitColorChangingEnabled)
                        MelonLoader.MelonCoroutines.Start(ColorUIExpansionKit());
                    }));
                Configuration.onConfigUpdated.Add(new System.Collections.Generic.KeyValuePair<string, Action>("UIColorHex", () => {
                    if (Configuration.JSONConfig.UIExpansionKitColorChangingEnabled)
                        MelonLoader.MelonCoroutines.Start(ColorUIExpansionKit());
                }));
                Configuration.onConfigUpdated.Add(new System.Collections.Generic.KeyValuePair<string, Action>("UIExpansionKitColorChangingEnabled", () => {
                    if (Configuration.JSONConfig.UIExpansionKitColorChangingEnabled)
                        MelonLoader.MelonCoroutines.Start(ColorUIExpansionKit());
                }));
                MelonLoader.MelonHandler.Mods.First(i => i.Info.Name == "UI Expansion Kit").Assembly.GetType("UIExpansionKit.API.ExpansionKitApi").GetMethod("RegisterSimpleMenuButton", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static).Invoke(null, new object[] { 0, "Toggle\nFlight", new System.Action(() => {
                            if (RiskyFunctionsManager.AreRiskyFunctionsAllowed && Configuration.JSONConfig.RiskyFunctionsEnabled)
                        Functions.PlayerHacks.Flight.SetFlyActive(!Functions.PlayerHacks.Flight.IsFlyEnabled);
                        }), new Action<GameObject>((GameObject obj) => { FlightButton = obj; obj.SetActive(Configuration.JSONConfig.UIExpansionKitIntegration); })});
                MelonLoader.MelonHandler.Mods.First(i => i.Info.Name == "UI Expansion Kit").Assembly.GetType("UIExpansionKit.API.ExpansionKitApi").GetMethod("RegisterSimpleMenuButton", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static).Invoke(null, new object[] { 0, "Toggle\nNoclip", new System.Action(() => {
                                if (RiskyFunctionsManager.AreRiskyFunctionsAllowed && Configuration.JSONConfig.RiskyFunctionsEnabled)
                        Functions.PlayerHacks.Flight.SetNoClipActive(!Functions.PlayerHacks.Flight.IsNoClipEnabled);
                        }), new Action<GameObject>((GameObject obj) => { NoclipButton = obj; obj.SetActive(Configuration.JSONConfig.UIExpansionKitIntegration); }) });
                MelonLoader.MelonHandler.Mods.First(i => i.Info.Name == "UI Expansion Kit").Assembly.GetType("UIExpansionKit.API.ExpansionKitApi").GetMethod("RegisterSimpleMenuButton", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static).Invoke(null, new object[] { 0, "Toggle\nSpeed", new System.Action(() => {
                                if (RiskyFunctionsManager.AreRiskyFunctionsAllowed && Configuration.JSONConfig.RiskyFunctionsEnabled)
                        Functions.PlayerHacks.Speed.SetActive(!Functions.PlayerHacks.Speed.IsEnabled);
                        }), new Action<GameObject>((GameObject obj) => { SpeedButton = obj; obj.SetActive(Configuration.JSONConfig.UIExpansionKitIntegration); }) });
                MelonLoader.MelonHandler.Mods.First(i => i.Info.Name == "UI Expansion Kit").Assembly.GetType("UIExpansionKit.API.ExpansionKitApi").GetMethod("RegisterSimpleMenuButton", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static).Invoke(null, new object[] { 0, "Toggle\nESP", new System.Action(() => {
                                if (RiskyFunctionsManager.AreRiskyFunctionsAllowed && Configuration.JSONConfig.RiskyFunctionsEnabled)
                        Functions.PlayerHacks.ESP.SetActive(!Functions.PlayerHacks.ESP.IsEnabled);
                        }), new Action<GameObject>((GameObject obj) => { ESPButton = obj; obj.SetActive(Configuration.JSONConfig.UIExpansionKitIntegration); }) });
                if (Configuration.JSONConfig.UIExpansionKitColorChangingEnabled)
                    MelonLoader.MelonHandler.Mods.First(i => i.Info.Name == "UI Expansion Kit").Assembly.GetType("UIExpansionKit.API.ExpansionKitApi").GetMethod("RegisterWaitConditionBeforeDecorating", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static).Invoke(null, new object[] { ColorUIExpansionKit() });

            }
            if (FBTSaver)
                emmVRCLoader.Logger.LogDebug("Detected FBTSaver");
            if (IKTweaks)
                emmVRCLoader.Logger.LogDebug("Detected IKTweaks");
            if (BetterLoadingScreen)
                emmVRCLoader.Logger.LogDebug("Detected BetterLoadingScreen");
            if (VRCMinus)
                emmVRCLoader.Logger.LogDebug("Detected VRCMinus");
        }
        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            if (buildIndex != -1) return;
            if (Functions.Core.ModCompatibility.MultiplayerDynamicBones && Configuration.JSONConfig.GlobalDynamicBonesEnabled)
            {
                Configuration.WriteConfigOption("GlobalDynamicBonesEnabled", false);
                Managers.emmVRCNotificationsManager.AddNotification(new Objects.Notification("emmVRC", null, "You are currently using MultiplayerDynamicBones. emmVRC's Global Dynamic Bones have been disabled, as only one can be used at a time.", true, false, null, "", "", true, null, "Dismiss"));
            }
        }
        public static IEnumerator ColorUIExpansionKit()
        {
            yield return null;
            //Color clr = (Configuration.JSONConfig.UIColorChangingEnabled ? Configuration.menuColor() : Configuration.menuColor());
            //ColorBlock theme = new ColorBlock()
            //{
            //    colorMultiplier = 1f,
            //    disabledColor = Color.grey,
            //    highlightedColor = new Color(clr.r*1.5f,clr.g*1.5f, clr.b*1.5f),
            //    normalColor = clr,
            //    pressedColor = Color.gray,
            //    fadeDuration = 0.1f
            //};
            //Transform uiExpansionRoot = GameObject.Find("UserInterface")
            ////Transform uiExpansionRoot = Libraries.QuickMenuUtils.GetQuickMenuInstance().transform.Find("ModUiPreloadedBundleContents");
            //foreach (Image img in uiExpansionRoot.GetComponentsInChildren<Image>(true))
            //{
            //    if (img.transform.parent.name != "PinToggle" && img.transform.parent.parent.name != "PinToggle" && img.transform.name != "Checkmark")
            //        img.color = new Color(clr.r*0.5f, clr.g*0.5f, clr.b*0.5f);
            //}
            //foreach (Button btn in uiExpansionRoot.GetComponentsInChildren<Button>(true))
            //{
            //    btn.colors = theme;
            //}
            //foreach (Toggle tgl in uiExpansionRoot.GetComponentsInChildren<Toggle>(true))
            //{
            //    if (tgl.gameObject.name != "PinToggle")
            //        tgl.colors = theme;
            //}
        }
    }
}
