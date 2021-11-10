using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIExpansionKit.API;
using UnityEngine;
using UnityEngine.UI;

namespace emmVRC.Functions.ModCompatibility
{
    public class UIExpansionKitIntegration
    {
        public static void Initialize()
        {
            ExpansionKitApi.GetExpandedMenu(ExpandedMenu.QuickMenu).AddToggleButton("Flight", (bool val) => { 
                if (Managers.RiskyFunctionsManager.AreRiskyFunctionsAllowed && Configuration.JSONConfig.RiskyFunctionsEnabled) {
                    if (!val && Functions.PlayerHacks.Flight.IsNoClipEnabled)
                        Functions.PlayerHacks.Flight.SetNoClipActive(false);
                    Functions.PlayerHacks.Flight.SetFlyActive(val);
                } 
            }, GetFlightStatus);
            ExpansionKitApi.GetExpandedMenu(ExpandedMenu.QuickMenu).AddToggleButton("Noclip", (bool val) => {
                if (Managers.RiskyFunctionsManager.AreRiskyFunctionsAllowed && Configuration.JSONConfig.RiskyFunctionsEnabled)
                {
                    if (val && !Functions.PlayerHacks.Flight.IsFlyEnabled)
                        Functions.PlayerHacks.Flight.SetFlyActive(true);
                    Functions.PlayerHacks.Flight.SetNoClipActive(val);
                }
            }, GetNoclipStatus);
            ExpansionKitApi.GetExpandedMenu(ExpandedMenu.QuickMenu).AddToggleButton("Speed", (bool val) => {
                if (Managers.RiskyFunctionsManager.AreRiskyFunctionsAllowed && Configuration.JSONConfig.RiskyFunctionsEnabled)
                {
                    Functions.PlayerHacks.Speed.SetActive(val);
                }
            }, GetSpeedStatus);
            ExpansionKitApi.GetExpandedMenu(ExpandedMenu.QuickMenu).AddToggleButton("ESP", (bool val) => {
                if (Managers.RiskyFunctionsManager.AreRiskyFunctionsAllowed && Configuration.JSONConfig.RiskyFunctionsEnabled)
                {
                    Functions.PlayerHacks.ESP.SetActive(val);
                }
            }, GetESPStatus);
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
            if (Configuration.JSONConfig.UIExpansionKitColorChangingEnabled)
                ExpansionKitApi.RegisterWaitConditionBeforeDecorating(ColorUIExpansionKit());
        }
        public static IEnumerator ColorUIExpansionKit()
        {
            yield return null;
            Color clr = (Configuration.JSONConfig.UIColorChangingEnabled ? Configuration.menuColor() : Configuration.menuColor());
            ColorBlock theme = new ColorBlock()
            {
                colorMultiplier = 1f,
                disabledColor = Color.grey,
                highlightedColor = new Color(clr.r * 1.5f, clr.g * 1.5f, clr.b * 1.5f),
                normalColor = clr,
                pressedColor = Color.gray,
                fadeDuration = 0.1f
            };
            Transform uiExpansionRoot = ExpansionKitApi.GetUiExpansionKitBundleContents().StoredThingsParent.transform;
            //Transform uiExpansionRoot = Libraries.QuickMenuUtils.GetQuickMenuInstance().transform.Find("ModUiPreloadedBundleContents");
            foreach (Image img in uiExpansionRoot.GetComponentsInChildren<Image>(true))
            {
                if (img.transform.parent.name != "PinToggle" && img.transform.parent.parent.name != "PinToggle" && img.transform.name != "Checkmark")
                    img.color = new Color(clr.r * 0.5f, clr.g * 0.5f, clr.b * 0.5f);
            }
            foreach (Button btn in uiExpansionRoot.GetComponentsInChildren<Button>(true))
            {
                btn.colors = theme;
            }
            foreach (Toggle tgl in uiExpansionRoot.GetComponentsInChildren<Toggle>(true))
            {
                if (tgl.gameObject.name != "PinToggle")
                    tgl.colors = theme;
            }
        }
        private static bool GetFlightStatus()
        {
            return Functions.PlayerHacks.Flight.IsFlyEnabled;
        }
        private static bool GetNoclipStatus()
        {
            return Functions.PlayerHacks.Flight.IsNoClipEnabled;
        }
        private static bool GetSpeedStatus()
        {
            return Functions.PlayerHacks.Speed.IsEnabled;
        }
        private static bool GetESPStatus()
        {
            return Functions.PlayerHacks.ESP.IsEnabled;
        }
    }
}
