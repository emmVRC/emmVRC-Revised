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
    public class InfoBarStatus
    {
        private static bool Enabled = true;
        internal static Text emmVRCStatusText;

        public static void Initialize()
        {
            // Grab the Ping text from the Quick Menu, as the base position for our new object
            Transform transform = Libraries.QuickMenuUtils.GetQuickMenuInstance().transform.Find("ShortcutMenu/BuildNumText");
            if (transform != null)
            {
                // Instantiate the status object with a RectTransform and UnityEngine.UI.Text
                Transform transform2 = new GameObject("emmVRCStatus", new [] { Il2CppType.Of<RectTransform>(), Il2CppType.Of<Text>() }).transform;

                // Parent the clock object to the base transform
                transform2.SetParent(transform.parent, false);
                transform2.SetSiblingIndex(transform.GetSiblingIndex() + 1);

                // Cache the status text object
                emmVRCStatusText = transform2.GetComponent<Text>();
                RectTransform component = transform2.GetComponent<RectTransform>();

                // Clone the base object's properties, so that ours matches the original VRChat design
                component.localScale = transform.localScale;
                component.anchorMin = transform.GetComponent<RectTransform>().anchorMin;
                component.anchorMax = transform.GetComponent<RectTransform>().anchorMax;
                component.anchoredPosition = transform.GetComponent<RectTransform>().anchoredPosition;
                component.sizeDelta = new Vector2(2000f, transform.GetComponent<RectTransform>().sizeDelta.y);
                component.pivot = transform.GetComponent<RectTransform>().pivot;

                // Offset our status from the base object
                Vector3 localPosition = transform.localPosition;
                localPosition.x -= transform.GetComponent<RectTransform>().sizeDelta.x * 0.5f;
                localPosition.x += 1000f;
                /*if (ModCompatibility.vrcTools)
                    localPosition.y += -170f;
                else*/ // Leftover code from VRCTools compatibility; use if something else also takes this space
                localPosition.y += -85f;
                component.localPosition = localPosition;

                // Set a placeholder text, as this will be updated later anyway
                emmVRCStatusText.text = "emmVRC v" + Objects.Attributes.Version;

                // Set the styling for the text, so that ours matches the original VRChat design
                emmVRCStatusText.color = transform.GetComponent<Text>().color;
                emmVRCStatusText.font = transform.GetComponent<Text>().font;
                emmVRCStatusText.fontSize = transform.GetComponent<Text>().fontSize;
                emmVRCStatusText.fontStyle = transform.GetComponent<Text>().fontStyle;

                // Expand the info bar, so that the text is not floating in space
                RectTransform component2 = Libraries.QuickMenuUtils.GetQuickMenuInstance().transform.Find("QuickMenu_NewElements/_InfoBar/Panel").GetComponent<RectTransform>();
                component2.sizeDelta = new Vector2(component2.sizeDelta.x, component2.sizeDelta.y + 80f);
                component2.anchoredPosition = new Vector2(component2.anchoredPosition.x, component2.anchoredPosition.y - 40f);

                // Shrink the info bar, if the status is not being displayed... this is kinda jank, we should fix this later
                if (!Configuration.JSONConfig.InfoBarDisplayEnabled)
                {
                    emmVRCStatusText.gameObject.SetActive(false);
                    component2.sizeDelta = new Vector2(component2.sizeDelta.x, component2.sizeDelta.y - 80f);
                    component2.anchoredPosition = new Vector2(component2.anchoredPosition.x, component2.anchoredPosition.y + 40f);
                }
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

                // Wait five seconds to update the status
                yield return new WaitForSeconds(5f);

                if (Configuration.JSONConfig.InfoBarDisplayEnabled)
                {
                    // Enable the status text, if it is disabled
                    emmVRCStatusText.gameObject.SetActive(true);

                    // If the network is enabled...
                    if (Configuration.JSONConfig.emmVRCNetworkEnabled)
                    {
                        // If the client is actually logged in, set the status to "Connected"
                        if (Network.NetworkClient.authToken != null)
                           emmVRCStatusText.text = "<color=#FF69B4>emmVRC</color> v" + Objects.Attributes.Version + "    Network Status: <color=lime>Connected</color>";
                        // If the client is not logged in, set the status to "Disconnected"
                        else
                            emmVRCStatusText.text = "<color=#FF69B4>emmVRC</color> v" + Objects.Attributes.Version + "    Network Status: <color=red> Disconnected</color>";
                    }
                    // If the network is disabled, set up a basic status instead
                    else
                         emmVRCStatusText.text = "<color=#FF69B4>emmVRC</color> v" + Objects.Attributes.Version;
                }
                else
                {
                    // Disable the status
                    emmVRCStatusText.gameObject.SetActive(false);
                }
            }
        }
    }
}
 