using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnhollowerRuntimeLib;
using UnityEngine;
using UnityEngine.UI;
using emmVRC.Objects.ModuleBases;


namespace emmVRC.Functions.UI
{
    public class InfoBarClock : MelonLoaderEvents, IWithGUI
    {
        private static bool Enabled = true;
        public static Text clockText;
        public static DateTime instanceJoinedTime;
        private static bool amPm = false;

        public override void OnUiManagerInit()
        {
            if (Configuration.JSONConfig.StealthMode) return;

            // Determine if time should be AM or PM, based on the system registry. There is no better way to do this.
            amPm = Microsoft.Win32.Registry.GetValue(@"HKEY_CURRENT_USER\Control Panel\International", "sShortTime", "x").ToString().Contains("h");

            // Grab the Ping text from the Quick Menu, as the base position for our new object
            Transform baseTextTransform = Libraries.QuickMenuUtils.GetQuickMenuInstance().transform.Find("QuickMenu_NewElements/_InfoBar/PingText");
            if (baseTextTransform != null)
            {
                // Instantiate the clock object with a RectTransform and UnityEngine.UI.Text
                Transform emmVRCTransform = new GameObject("emmVRCClock", new[] { Il2CppType.Of<RectTransform>(), Il2CppType.Of<Text>() }).transform;

                // Parent the clock object to the base transform
                emmVRCTransform.SetParent(baseTextTransform.parent, false);
                emmVRCTransform.SetSiblingIndex(baseTextTransform.GetSiblingIndex() + 1);

                // Cache the clock text object
                clockText = emmVRCTransform.GetComponent<Text>();
                RectTransform emmVRCRT = emmVRCTransform.GetComponent<RectTransform>();

                // Clone the base object's scale, so that ours matches the original VRChat design
                emmVRCRT.localScale = baseTextTransform.localScale;

                // Clone the base object's properties, so that ours matches the original VRChat design
                emmVRCRT.anchorMin = baseTextTransform.GetComponent<RectTransform>().anchorMin;
                emmVRCRT.anchorMax = baseTextTransform.GetComponent<RectTransform>().anchorMax;
                emmVRCRT.anchoredPosition = baseTextTransform.GetComponent<RectTransform>().anchoredPosition;
                emmVRCRT.sizeDelta = new Vector2(2000, 92);
                emmVRCRT.pivot = baseTextTransform.GetComponent<RectTransform>().pivot;

                // Offset our clock from the base object
                Vector3 newPos = baseTextTransform.localPosition;
                newPos.x -= baseTextTransform.GetComponent<RectTransform>().sizeDelta.x * 0.5f;
                newPos.x += 1375 * 0.5f;
                newPos.y += -90;
                emmVRCRT.localPosition = newPos;

                // Set a placeholder text, as this will be updated later anyway
                clockText.text = "(00:00:00) NA:NA PM";

                // Set the styling for the text, so that ours matches the original VRChat design
                clockText.color = baseTextTransform.GetComponent<Text>().color;
                clockText.font = baseTextTransform.GetComponent<Text>().font;
                clockText.fontSize = baseTextTransform.GetComponent<Text>().fontSize - 8;
                clockText.fontStyle = baseTextTransform.GetComponent<Text>().fontStyle;

                // Enable the clock, if it is turned on, otherwise disable it
                clockText.gameObject.SetActive(Configuration.JSONConfig.ClockEnabled);
            }
            else
            {
                // Our base object doesn't exist. Sad times, but VRChat probably updated to move things around
                emmVRCLoader.Logger.LogError("QuickMenu/ShortcutMenu/PingText is null");
            }
        }
        public void OnGUI()
        {
            if (Configuration.JSONConfig.ClockEnabled && !Configuration.JSONConfig.StealthMode && clockText != null)
            {
                // Enable the clock text, if it is disabled
                clockText.gameObject.SetActive(true);

                // Build a time string based on the current time
                var timeString = DateTime.Now.ToString(amPm ? "hh:mm tt" : "HH:mm");

                // Set up the instance timer string to be updated later
                string instanceTimeString = "00:00:00";

                // If the player is in a world...
                if (RoomManager.field_Internal_Static_ApiWorld_0 != null)
                {
                    // Gather the time that you have been in a world from instanceJoinedTime, which literally just holds the time you joined the instance.
                    var instanceTimeSpan = DateTime.Now - instanceJoinedTime;

                    // Fill out the instance timer string from the timespan
                    instanceTimeString = string.Format("{0:D2}:{1:D2}:{2:D2}", instanceTimeSpan.Hours, instanceTimeSpan.Minutes, instanceTimeSpan.Seconds);


                }

                // Apply the current time strings to the clock
                clockText.text = "(" + instanceTimeString + ") " + timeString;
            }
            else
            {
                // Disable the clock
                if (clockText != null)
                clockText.gameObject.SetActive(false);
            }
        }
        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            instanceJoinedTime = DateTime.Now;
        }
    }
}
