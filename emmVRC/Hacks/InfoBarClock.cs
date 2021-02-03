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


namespace emmVRC.Hacks
{
    public class InfoBarClock
    {
        private static bool Enabled = true;
        public static Text clockText;
        public static uint instanceTime = 0;
        
        public static void Initialize()
        {
            // Grab the Ping text from the Quick Menu, as the base position for our new object
            Transform baseTextTransform = Libraries.QuickMenuUtils.GetQuickMenuInstance().transform.Find("QuickMenu_NewElements/_InfoBar/PingText");
            if (baseTextTransform != null)
            {
                // Instantiate the clock object with a RectTransform and UnityEngine.UI.Text
                Transform emmVRCTransform = new GameObject("emmVRCClock", new [] { Il2CppType.Of<RectTransform>(), Il2CppType.Of<Text>() }).transform;

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
                emmVRCRT.sizeDelta = new Vector2(2000, baseTextTransform.GetComponent<RectTransform>().sizeDelta.y);
                emmVRCRT.pivot = baseTextTransform.GetComponent<RectTransform>().pivot;

                // Offset our clock from the base object
                Vector3 newPos = baseTextTransform.localPosition;
                newPos.x -= baseTextTransform.GetComponent<RectTransform>().sizeDelta.x * 0.5f;
                newPos.x += 1400 * 0.5f;
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

                MelonLoader.MelonCoroutines.Start(Loop());
            }
            else
            {
                // Our base object doesn't exist. Sad times, but VRChat probably updated to move things around
                emmVRCLoader.Logger.LogError("QuickMenu/ShortcutMenu/PingText is null");
            }
        }
        private static IEnumerator Loop()
        {
            while (Enabled)
            {
                // Wait one realtime second to update the clock
                yield return new WaitForSecondsRealtime(1f);

                if (Configuration.JSONConfig.ClockEnabled)
                {
                    // Enable the clock text, if it is disabled
                    clockText.gameObject.SetActive(true);

                    // Build a time string based on the current time
                    var timeString = DateTime.Now.ToShortTimeString();

                    // Set up the instance timer string to be updated later
                    string instanceTimeString = "00:00:00";

                    // If the player is in a world...
                    if (RoomManager.field_Internal_Static_ApiWorld_0 != null)
                    {
                        // Gather the time that you have been in a world from instanceTime, which holds the time in seconds since joining the instance
                        var instanceTimeSpan = TimeSpan.FromSeconds(instanceTime);

                        // Fill out the instance timer string from the timespan
                        instanceTimeString = string.Format("{0:D2}:{1:D2}:{2:D2}", instanceTimeSpan.Hours, instanceTimeSpan.Minutes, instanceTimeSpan.Seconds);

                        // Add one second to the instance time
                        instanceTime += 1;
                    }

                    // Apply the current time strings to the clock
                    clockText.text = "(" + instanceTimeString + ") " + timeString;
                }
                else
                {
                    // Disable the clock
                    clockText.gameObject.SetActive(false);
                }
            }
        }
    } 
}
