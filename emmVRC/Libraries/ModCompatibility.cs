using emmVRC.Hacks;
using emmVRC.Managers;
using emmVRC.Menus;
using MelonLoader;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace emmVRC.Libraries
{
    public class ModCompatibility
    {

        public static bool MultiplayerDynamicBones = false;
        public static bool PortalConfirmation = false;
        public static bool MControl = false;
        public static bool OGTrustRank = false;
        public static bool UIExpansionKit = false;
        public static bool FBTSaver = false;

        public static GameObject FlightButton;
        public static GameObject NoclipButton;
        public static GameObject SpeedButton;
        public static GameObject ESPButton;

        public static void Initialize()
        {
            if (MelonLoader.MelonHandler.Mods.FindIndex(i => i.Info.Name == "MultiplayerDynamicBones") != -1)
                MultiplayerDynamicBones = true;
            if (MelonLoader.MelonHandler.Mods.FindIndex(i => i.Info.Name == "Portal Confirmation") != -1)
                PortalConfirmation = true;
            if (MelonLoader.MelonHandler.Mods.FindIndex(i => i.Info.Name == "MControl") != -1)
                MControl = true;
            if (MelonLoader.MelonHandler.Mods.FindIndex(i => i.Info.Name == "OGTrustRanks") != -1)
                OGTrustRank = true;
            if (MelonLoader.MelonHandler.Mods.FindIndex(i => i.Info.Name == "UI Expansion Kit") != -1)
                UIExpansionKit = true;
            if (MelonLoader.MelonHandler.Mods.FindIndex(i => i.Info.Name == "FBT Saver") != -1)
                FBTSaver = true;

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
                MelonLoader.MelonHandler.Mods.First(i => i.Info.Name == "UI Expansion Kit").Assembly.GetType("UIExpansionKit.API.ExpansionKitApi").GetMethod("RegisterSimpleMenuButton", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static).Invoke(null, new object[] { 0, "Toggle\nFlight", new System.Action(() => {
                            if (RiskyFunctionsManager.RiskyFunctionsAllowed && Configuration.JSONConfig.RiskyFunctionsEnabled)
                        PlayerTweaksMenu.FlightToggle.setToggleState(!Flight.FlightEnabled, true);
                        }), new Action<GameObject>((GameObject obj) => { FlightButton = obj; obj.SetActive(Configuration.JSONConfig.UIExpansionKitIntegration); })});
                MelonLoader.MelonHandler.Mods.First(i => i.Info.Name == "UI Expansion Kit").Assembly.GetType("UIExpansionKit.API.ExpansionKitApi").GetMethod("RegisterSimpleMenuButton", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static).Invoke(null, new object[] { 0, "Toggle\nNoclip", new System.Action(() => {
                                if (RiskyFunctionsManager.RiskyFunctionsAllowed && Configuration.JSONConfig.RiskyFunctionsEnabled)
                        PlayerTweaksMenu.NoclipToggle.setToggleState(!Flight.NoclipEnabled, true);
                        }), new Action<GameObject>((GameObject obj) => { NoclipButton = obj; obj.SetActive(Configuration.JSONConfig.UIExpansionKitIntegration); }) });
                MelonLoader.MelonHandler.Mods.First(i => i.Info.Name == "UI Expansion Kit").Assembly.GetType("UIExpansionKit.API.ExpansionKitApi").GetMethod("RegisterSimpleMenuButton", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static).Invoke(null, new object[] { 0, "Toggle\nSpeed", new System.Action(() => {
                                if (RiskyFunctionsManager.RiskyFunctionsAllowed && Configuration.JSONConfig.RiskyFunctionsEnabled)
                        PlayerTweaksMenu.SpeedToggle.setToggleState(!Speed.SpeedModified, true);
                        }), new Action<GameObject>((GameObject obj) => { SpeedButton = obj; obj.SetActive(Configuration.JSONConfig.UIExpansionKitIntegration); }) });
                MelonLoader.MelonHandler.Mods.First(i => i.Info.Name == "UI Expansion Kit").Assembly.GetType("UIExpansionKit.API.ExpansionKitApi").GetMethod("RegisterSimpleMenuButton", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static).Invoke(null, new object[] { 0, "Toggle\nESP", new System.Action(() => {
                                if (RiskyFunctionsManager.RiskyFunctionsAllowed && Configuration.JSONConfig.RiskyFunctionsEnabled)
                        PlayerTweaksMenu.ESPToggle.setToggleState(!ESP.ESPEnabled, true);
                        }), new Action<GameObject>((GameObject obj) => { ESPButton = obj; obj.SetActive(Configuration.JSONConfig.UIExpansionKitIntegration); }) });
                if (Configuration.JSONConfig.UIExpansionKitColorChangingEnabled)
                    MelonLoader.MelonHandler.Mods.First(i => i.Info.Name == "UI Expansion Kit").Assembly.GetType("UIExpansionKit.API.ExpansionKitApi").GetMethod("RegisterWaitConditionBeforeDecorating", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static).Invoke(null, new object[] { ColorUIExpansionKit() });

            }
            if (FBTSaver)
            {
                emmVRCLoader.Logger.LogDebug("Detected FBTSaver");
            }
        }
        public static IEnumerator ColorUIExpansionKit()
        {
            yield return null;
            Color clr = (Configuration.JSONConfig.UIColorChangingEnabled ? Configuration.menuColor() : Configuration.menuColor());
            ColorBlock theme = new ColorBlock()
            {
                colorMultiplier = 1f,
                disabledColor = Color.grey,
                highlightedColor = new Color(clr.r*1.5f,clr.g*1.5f, clr.b*1.5f),
                normalColor = clr,
                pressedColor = Color.gray,
                fadeDuration = 0.1f
            };
            Transform uiExpansionRoot = QuickMenuUtils.GetQuickMenuInstance().transform.Find("ModUiPreloadedBundleContents");
            foreach (Image img in uiExpansionRoot.GetComponentsInChildren<Image>(true))
            {
                img.color = new Color(clr.r*0.5f, clr.g*0.5f, clr.b*0.5f);
            }
            foreach (Button btn in uiExpansionRoot.GetComponentsInChildren<Button>(true))
            {
                btn.colors = theme;
            }
            foreach (Toggle tgl in uiExpansionRoot.GetComponentsInChildren<Toggle>(true))
            {
                tgl.colors = theme;
            }
        }
    }
}
