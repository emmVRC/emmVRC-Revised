﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;


namespace emmVRC.Hacks
{
    public class InfoBarStatus
    {
        private static bool Enabled = true;
        internal static Text emmVRCStatusText;
        private static Thread InfoBarStatusThread;

        public static void Initialize()
        {
            Transform transform = Libraries.QuickMenuUtils.GetQuickMenuInstance().transform.Find("ShortcutMenu/BuildNumText");
            if (transform != null)
            {
                Transform transform2 = new GameObject("emmVRCStatus", new [] { RectTransform.Il2CppType, Text.Il2CppType }).transform;
                transform2.SetParent(transform.parent, false);
                transform2.SetSiblingIndex(transform.GetSiblingIndex() + 1);
                emmVRCStatusText = transform2.GetComponent<Text>();
                RectTransform component = transform2.GetComponent<RectTransform>();
                component.localScale = transform.localScale;
                component.anchorMin = transform.GetComponent<RectTransform>().anchorMin;
                component.anchorMax = transform.GetComponent<RectTransform>().anchorMax;
                component.anchoredPosition = transform.GetComponent<RectTransform>().anchoredPosition;
                component.sizeDelta = new Vector2(2000f, transform.GetComponent<RectTransform>().sizeDelta.y);
                component.pivot = transform.GetComponent<RectTransform>().pivot;
                Vector3 localPosition = transform.localPosition;
                localPosition.x -= transform.GetComponent<RectTransform>().sizeDelta.x * 0.5f;
                localPosition.x += 1000f;
                /*if (ModCompatibility.vrcTools)
                    localPosition.y += -170f;
                else*/
                localPosition.y += -85f;
                component.localPosition = localPosition;
                emmVRCStatusText.text = "emmVRC v" + Objects.Attributes.Version;
                emmVRCStatusText.color = transform.GetComponent<Text>().color;
                emmVRCStatusText.font = transform.GetComponent<Text>().font;
                emmVRCStatusText.fontSize = transform.GetComponent<Text>().fontSize;
                emmVRCStatusText.fontStyle = transform.GetComponent<Text>().fontStyle;
                RectTransform component2 = Libraries.QuickMenuUtils.GetQuickMenuInstance().transform.Find("QuickMenu_NewElements/_InfoBar/Panel").GetComponent<RectTransform>();
                component2.sizeDelta = new Vector2(component2.sizeDelta.x, component2.sizeDelta.y + 80f);
                component2.anchoredPosition = new Vector2(component2.anchoredPosition.x, component2.anchoredPosition.y - 40f);
                if (!Configuration.JSONConfig.InfoBarDisplayEnabled)
                {
                    emmVRCStatusText.gameObject.SetActive(false);
                    component2.sizeDelta = new Vector2(component2.sizeDelta.x, component2.sizeDelta.y - 80f);
                    component2.anchoredPosition = new Vector2(component2.anchoredPosition.x, component2.anchoredPosition.y + 40f);
                }
                InfoBarStatusThread = new Thread(Loop)
                {
                    Name = "emmVRC Info Bar Status Thread",
                    IsBackground = true
                };
                InfoBarStatusThread.Start();
            }
            else
            {
                emmVRCLoader.Logger.LogError("QuickMenu/ShortcutMenu/PingText is null");
            }
        }
        private static void Loop()
        {
            while (Enabled)
            {
                Thread.Sleep(5000);
                if (Configuration.JSONConfig.InfoBarDisplayEnabled)
                {
                    /*if (Configuration.JSONConfig.emmVRCNetworkEnabled)
                    {
                        emmVRCStatusText.text = emmVRCNet.loggedIn ? "<color=#FF69B4>emmVRC</color> v" + emmVRC.version + "    Network Status: <color=lime>Connected</color>" : "emmVRC v" + emmVRC.version + "    Network Status: <color=red> Disconnected </color>";
                    }*/

                    emmVRCStatusText.text = "<color=#FF69B4>emmVRC</color> v" + Objects.Attributes.Version;
                }
            }
        }
    }
}
