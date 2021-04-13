using emmVRC.Hacks;
using emmVRC.Libraries;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Metadata;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using VRC;
using VRC.Core;

namespace emmVRC.Menus
{
    public class VRHUD
    {
        private static Transform ShortcutMenu;
        private static GameObject BackgroundObject;
        private static GameObject TextObject;
        private static GameObject LogoIconContainer;
        private static Image BackgroundImage, emmLogo;
        private static Text TextText;
        public static bool enabled = true;
        public static bool Initialized;
        public static QMSingleButton ToggleHUDButton;
        public static IEnumerator Initialize()
        {
            emmVRCLoader.Logger.Log("[emmVRC] Initializing Quickmenu HUD");
            while (Resources.HUD_Base == null) yield return null;
            // Find the shortcut menu and tie it to a global variable:
            ShortcutMenu = QuickMenuUtils.GetQuickMenuInstance().transform.Find("ShortcutMenu");

            // Setting up the Background object:
            BackgroundObject = new GameObject("Background");
            BackgroundObject.transform.SetParent(ShortcutMenu, false);

            BackgroundObject.AddComponent<CanvasRenderer>();
            BackgroundImage = BackgroundObject.AddComponent<Image>();

            // Yes, the double-if actually saves performance.
            if (Configuration.JSONConfig.MoveVRHUDIfSpaceFree && Configuration.JSONConfig.DisableRankToggleButton && Configuration.JSONConfig.DisableReportWorldButton && (Configuration.JSONConfig.FunctionsButtonX != 5 || Configuration.JSONConfig.TabMode))
            {
                if (!Configuration.JSONConfig.LogoButtonEnabled || Configuration.JSONConfig.LogoButtonX != 5 || Configuration.JSONConfig.TabMode)
                {
                    var rect = BackgroundObject.GetComponent<RectTransform>();
                    //rect.sizeDelta = new Vector2(640, 1920);
                    rect.sizeDelta = new Vector2(900, 1920); // With Player Framerate counter
                    rect.anchoredPosition = new Vector2(1290, 290);
                }
                else
                {
                    var rect = BackgroundObject.GetComponent<RectTransform>();
                    //rect.sizeDelta = new Vector2(640, 1920);
                    rect.sizeDelta = new Vector2(900, 1920); // With Player Framerate counter
                    rect.anchoredPosition = new Vector2(1745, 290);
                }
            }
            else
            {
                var rect = BackgroundObject.GetComponent<RectTransform>();
                //rect.sizeDelta = new Vector2(640, 1920);
                rect.sizeDelta = new Vector2(900, 1920); // With Player Framerate counter
                //rect.anchoredPosition = new Vector2(1590, 280);
                rect.anchoredPosition = new Vector2(1745, 290); // With Player Framerate counter
            }
            BackgroundImage.sprite = Resources.HUD_Base;

            // Setting up the text object:
            TextObject = new GameObject("Text");
            TextObject.transform.SetParent(BackgroundObject.transform, false);

            TextObject.AddComponent<CanvasRenderer>();
            TextText = TextObject.AddComponent<Text>();

            //TextObject.GetComponent<RectTransform>().sizeDelta = new Vector2(630, 1920);
            TextObject.GetComponent<RectTransform>().sizeDelta = new Vector2(860, 1920); // With Player Framerate counter
            TextObject.GetComponent<RectTransform>().localPosition -= new Vector3(0f, 10f, 0f);
            TextText.font = UnityEngine.Resources.GetBuiltinResource<Font>("Arial.ttf");
            TextText.fontSize = 40;
            TextText.text = "";

            ToggleHUDButton = new QMSingleButton("ShortcutMenu", 0, 0, "", () =>
            {
                enabled = !enabled;
            }, "Toggles the Quick Menu HUD", Color.grey);
            var TGO = ToggleHUDButton.getGameObject();
            var TRT = TGO.GetComponent<RectTransform>();
            TRT.sizeDelta = new Vector2(168f, 168f);
            TRT.anchoredPosition = new Vector2(1590f, 60f);
            //GameObject.DestroyImmediate(TGO.GetComponentInChildren<Text>().gameObject);
            TGO.GetComponentInChildren<Image>().sprite = Resources.onlineSprite;

            // Start emmHUD Logo
            LogoIconContainer = new GameObject("emmHUDLogo");
            LogoIconContainer.AddComponent<CanvasRenderer>();
            LogoIconContainer.transform.SetParent(BackgroundObject.transform, false);
            emmLogo = LogoIconContainer.AddComponent<Image>();
            emmLogo.sprite = Resources.emmHUDLogo;
            emmLogo.GetComponent<RectTransform>().localPosition = new Vector2(-365f, 880f);
            emmLogo.GetComponent<RectTransform>().localScale = new Vector3(1.3f, 1.3f, 1.3f);
            // End emmHUD Logo

            Initialized = true;

            MelonLoader.MelonCoroutines.Start(Loop());
        }
        // Its hard to decide between having this called in init 
        // and having the rect a global variable or just doing it
        //  integrated and having a seperate refresher for the hud
        //    \\  move option like this.
        public static void RefreshPosition()
        {
            if (Configuration.JSONConfig.MoveVRHUDIfSpaceFree && Configuration.JSONConfig.DisableRankToggleButton && Configuration.JSONConfig.DisableReportWorldButton && Configuration.JSONConfig.FunctionsButtonX != 5)
            {
                if (!Configuration.JSONConfig.LogoButtonEnabled || Configuration.JSONConfig.LogoButtonX != 5)
                    BackgroundObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(1290, 290);
                else
                    BackgroundObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(1745, 290);
            }
            else
                BackgroundObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(1745, 290);
        }

        private static IEnumerator Loop()
        {
            while (true)
            {
                if (Configuration.JSONConfig.HUDEnabled && enabled && !BackgroundObject.activeSelf)
                {
                    BackgroundObject.SetActive(true);
                }
                else if (!Configuration.JSONConfig.HUDEnabled && BackgroundObject.activeSelf || !enabled && BackgroundObject.activeSelf)
                {
                    BackgroundObject.SetActive(false);
                }
                yield return new WaitForEndOfFrame();
                if (TextText)
                {
                    string userList = CommonHUD.RenderPlayerList();
                    TextText.text = "\n            <color=#FF69B4>emmVRC</color> v" + Objects.Attributes.Version +
                        "\n" +
                        "\n" +
                        "\nUsers in room" + (RoomManager.field_Internal_Static_ApiWorldInstance_0 != null ? " (" + PlayerManager.field_Private_Static_PlayerManager_0.field_Private_List_1_Player_0.Count + ")" : "") + ":\n" + userList + "" +
                        "\n" +
                        "\nPosition in world:\n" + CommonHUD.RenderWorldInfo() +
                        "\n" +
                        "\n" +
                        (Configuration.JSONConfig.emmVRCNetworkEnabled ? (Network.NetworkClient.webToken != null ? "<color=lime>Connected to the\nemmVRC Network</color>" : "<color=red>Not connected to the\nemmVRC Network</color>") : "") +
                        "\n" +
                        "\n" +
                        (Objects.Attributes.Debug ? (
                            "Current frame time: " + (Hacks.FrameTimeCalculator.frameTimes[Hacks.FrameTimeCalculator.iterator == 0 ? Hacks.FrameTimeCalculator.frameTimes.Length - 1 : (Hacks.FrameTimeCalculator.iterator - 1)]) + "ms\n" +
                            "Average frame time: " + Hacks.FrameTimeCalculator.frameTimeAvg + "ms\n"
                        ) : "");
                    if (APIUser.CurrentUser != null && (Configuration.JSONConfig.InfoSpoofingEnabled))
                        TextText.text = TextText.text.Replace(APIUser.CurrentUser.GetName(), (NameSpoofGenerator.spoofedName));
                }
            }
        }
    }
}