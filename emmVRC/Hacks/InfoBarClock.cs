using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
            Transform baseTextTransform = Libraries.QuickMenuUtils.GetQuickMenuInstance().transform.Find("ShortcutMenu/PingText");
            if (baseTextTransform != null)
            {
                Transform emmVRCTransform = new GameObject("emmVRCClock", new [] { RectTransform.Il2CppType, Text.Il2CppType }).transform;
                emmVRCTransform.SetParent(baseTextTransform.parent, false);
                emmVRCTransform.SetSiblingIndex(baseTextTransform.GetSiblingIndex() + 1);
                clockText = emmVRCTransform.GetComponent<Text>();
                RectTransform emmVRCRT = emmVRCTransform.GetComponent<RectTransform>();

                emmVRCRT.localScale = baseTextTransform.localScale;

                emmVRCRT.anchorMin = baseTextTransform.GetComponent<RectTransform>().anchorMin;
                emmVRCRT.anchorMax = baseTextTransform.GetComponent<RectTransform>().anchorMax;
                emmVRCRT.anchoredPosition = baseTextTransform.GetComponent<RectTransform>().anchoredPosition;
                emmVRCRT.sizeDelta = new Vector2(2000, baseTextTransform.GetComponent<RectTransform>().sizeDelta.y);
                emmVRCRT.pivot = baseTextTransform.GetComponent<RectTransform>().pivot;

                Vector3 newPos = baseTextTransform.localPosition;
                newPos.x -= baseTextTransform.GetComponent<RectTransform>().sizeDelta.x * 0.5f;
                newPos.x += 1400 * 0.5f;
                newPos.y += -90;

                emmVRCRT.localPosition = newPos;
                clockText.text = "(00:00:00) NA:NA PM";

                clockText.color = baseTextTransform.GetComponent<Text>().color;
                clockText.font = baseTextTransform.GetComponent<Text>().font;
                clockText.fontSize = baseTextTransform.GetComponent<Text>().fontSize - 8;
                clockText.fontStyle = baseTextTransform.GetComponent<Text>().fontStyle;
                clockText.gameObject.SetActive(Configuration.JSONConfig.ClockEnabled);
                MelonLoader.MelonCoroutines.Start(Loop());
            }
            else
            {
                emmVRCLoader.Logger.LogError("QuickMenu/ShortcutMenu/PingText is null");
            }
        }
        private static IEnumerator Loop()
        {
            while (Enabled)
            {
                yield return new WaitForSeconds(1f);
                if (Configuration.JSONConfig.ClockEnabled)
                {
                    var timeString = DateTime.Now.ToShortTimeString();
                    string instanceTimeString = "00:00:00";
                    if (RoomManager.field_ApiWorld_0 != null)
                    {
                        var instanceTimeSpan = TimeSpan.FromSeconds(instanceTime);
                        instanceTimeString = string.Format("{0:D2}:{1:D2}:{2:D2}", instanceTimeSpan.Hours, instanceTimeSpan.Minutes, instanceTimeSpan.Seconds);
                        instanceTime += 1;
                    }
                    clockText.text = "(" + instanceTimeString + ") " + timeString;
                }
            }
        }
    }
}
