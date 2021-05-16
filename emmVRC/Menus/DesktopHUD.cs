using emmVRC.Hacks;
using emmVRC.Libraries;
using emmVRC.Network;
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
    public class CommonHUD
    {
        public static string RenderPlayerList()
        {
            string userList = "";
            if (RoomManager.field_Internal_Static_ApiWorld_0 != null)
            {
                int tempCount = 0;
                if (PlayerManager.field_Private_Static_PlayerManager_0 != null && PlayerManager.field_Private_Static_PlayerManager_0.field_Private_List_1_Player_0 != null)
                    try
                    {
                        foreach (Player plr in PlayerManager.field_Private_Static_PlayerManager_0.field_Private_List_1_Player_0)
                        {
                            if (plr != null && plr.field_Private_VRCPlayerApi_0 != null)
                                if (tempCount != 22)
                                {
                                    userList += (plr.field_Private_VRCPlayerApi_0.isMaster ? "♕ " : "     ") + "<color=#" + Libraries.ColorConversion.ColorToHex(VRCPlayer.Method_Public_Static_Color_APIUser_0(plr.field_Private_APIUser_0)) + ">" + plr.field_Private_APIUser_0.GetName() + "</color> - " + plr._vrcplayer.prop_Int16_0 + " ms - <color=" + ColorConversion.ColorToHex(plr.prop_PlayerNet_0.LerpFramerateColor(), true) + ">" + (plr.prop_PlayerNet_0.GetFramerate() != -1f ? plr.prop_PlayerNet_0.GetFramerate()+"" : "N/A") + "</color> fps\n";
                                    tempCount++;
                                }
                        }
                    }
                    catch (Exception ex)
                    {
                        ex = new Exception();
                    }
            }
            return userList;
        }

        public static string RenderWorldInfo()
        {
            string positionstr = "";
            if (RoomManager.field_Internal_Static_ApiWorld_0 != null)
            {
                if (VRCPlayer.field_Internal_Static_VRCPlayer_0 != null)
                {
                    positionstr += "<b><color=red>X: " + (Mathf.Floor(VRCPlayer.field_Internal_Static_VRCPlayer_0.gameObject.GetComponent<VRC.Player>().transform.position.x * 10)) / 10 + "</color></b>  ";
                    positionstr += "<b><color=lime>Y: " + (Mathf.Floor(VRCPlayer.field_Internal_Static_VRCPlayer_0.gameObject.GetComponent<VRC.Player>().transform.position.y * 10)) / 10 + "</color></b>  ";
                    positionstr += "<b><color=cyan>Z: " + (Mathf.Floor(VRCPlayer.field_Internal_Static_VRCPlayer_0.gameObject.GetComponent<VRC.Player>().transform.position.z * 10)) / 10 + "</color></b>  ";
                }
            }
            return positionstr + "\n\n";
        }

        // Note to Korty - This is not used (or at least it doesn't seem like it)
        public static GameObject[] InitializeHUD(Transform parent)
        {
            GameObject BackgroundObject = new GameObject("Background");

            BackgroundObject.AddComponent<CanvasRenderer>();

            BackgroundObject.AddComponent<RawImage>();
            BackgroundObject.GetComponent<RectTransform>().sizeDelta = new Vector2(256, 768);
            BackgroundObject.GetComponent<RectTransform>().position = new Vector2(130 - (Screen.width / 2), (Screen.height / 6) - 64);
            BackgroundObject.transform.SetParent(parent, false);
            BackgroundObject.GetComponent<Image>().sprite = Resources.HUD_Minimized;
            GameObject TextObject = new GameObject("Text");
            TextObject.AddComponent<CanvasRenderer>();
            TextObject.transform.SetParent(BackgroundObject.transform, false);
            Text text = TextObject.AddComponent<Text>();
            //text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            text.font = UnityEngine.Resources.GetBuiltinResource<Font>("Arial.ttf");
            text.fontSize = 15;
            text.text = "            emmVRClient  fps: 90";
            TextObject.GetComponent<RectTransform>().sizeDelta = new Vector2(250, 768);
            GameObject[] returnArr = { BackgroundObject, TextObject };
            return returnArr;
        }
    }
    public class DesktopHUD
    {
        private static GameObject CanvasObject;
        private static GameObject BackgroundObject;
        private static GameObject TextObject;
        private static GameObject LogoIconContainer;
        private static Image BackgroundImage, emmLogo;
        private static Text TextText;
        private static bool keyFlag;
        public static bool UIExpanded = false;
        public static bool Initialized = false;
        public static bool enabled = true;
        private static CanvasScaler scaler;
        public static IEnumerator Initialize()
        {
            emmVRCLoader.Logger.LogDebug("Initializing Desktop HUD");
            // UI Init
            while (Resources.HUD_Minimized == null) yield return null;
            CanvasObject = new GameObject("emmVRCUICanvas");
            GameObject.DontDestroyOnLoad(CanvasObject);
            Canvas canvas = CanvasObject.AddComponent<Canvas>();

            canvas.renderMode = RenderMode.ScreenSpaceOverlay;

            CanvasObject.transform.position = Vector3.zero;
                    scaler = CanvasObject.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
            //          scaler.scaleFactor = 10.0f;
            //          scaler.dynamicPixelsPerUnit = 10.0f;

            //            canvasObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 1.0f);
            //            canvasObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 1.0f);
            BackgroundObject = new GameObject("Background");

            BackgroundObject.AddComponent<CanvasRenderer>();

            BackgroundImage = BackgroundObject.AddComponent<Image>();
            BackgroundObject.GetComponent<RectTransform>().anchorMin = new Vector2(0, 1);
            BackgroundObject.GetComponent<RectTransform>().anchorMax = new Vector2(0, 1);
            BackgroundObject.GetComponent<RectTransform>().pivot = new Vector2(0, 1);
            BackgroundObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -5f);
            BackgroundObject.GetComponent<RectTransform>().sizeDelta = new Vector2(325, 768); // With Player Framerate counter
            BackgroundObject.transform.SetParent(CanvasObject.transform, false);
            BackgroundImage.sprite = Resources.HUD_Minimized;
            TextObject = new GameObject("Text");
            TextObject.AddComponent<CanvasRenderer>();
            TextObject.transform.SetParent(BackgroundObject.transform, false);
            TextText = TextObject.AddComponent<Text>();
            //text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            TextText.font = UnityEngine.Resources.GetBuiltinResource<Font>("Arial.ttf");
            TextText.fontSize = 15;
            TextText.text = "";
            //TextObject.GetComponent<RectTransform>().sizeDelta = new Vector2(250, 768);
            TextObject.GetComponent<RectTransform>().sizeDelta = new Vector2(310, 768); // With Player Framerate counter
            TextObject.GetComponent<RectTransform>().localPosition += new Vector3(0f, 3f, 0f);

            // Start emmHUD Logo
            LogoIconContainer = new GameObject("emmHUDLogo");
            LogoIconContainer.AddComponent<CanvasRenderer>();
            LogoIconContainer.transform.SetParent(BackgroundObject.transform, false);
            emmLogo = LogoIconContainer.AddComponent<Image>();
            emmLogo.sprite = Resources.emmHUDLogo;
            emmLogo.GetComponent<RectTransform>().anchorMin = new Vector2(0, 1);
            emmLogo.GetComponent<RectTransform>().anchorMax = new Vector2(0, 1);
            emmLogo.GetComponent<RectTransform>().pivot = new Vector2(0, 1);
            emmLogo.GetComponent<RectTransform>().anchoredPosition = new Vector2(10, -2.5f);
            emmLogo.GetComponent<RectTransform>().localScale = new Vector3(0.58f, 0.58f, 0.58f);
            // End emmHUD Logo

            CanvasObject.SetActive(false);

            Initialized = true;
            emmVRCLoader.Logger.LogDebug("Desktop HUD initialized fully.");
            MelonLoader.MelonCoroutines.Start(Loop());
        }

        private static IEnumerator Loop()
        {
            while (true)
            {
                if (Configuration.JSONConfig.HUDEnabled && enabled)
                {
                    CanvasObject.SetActive(true);

                    
                    //BackgroundObject.GetComponent<RectTransform>().position = new Vector2(130 - (Screen.width / 2), (Screen.height / 6) - 64);
                    //BackgroundObject.GetComponent<RectTransform>().anchor
                    //BackgroundObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(-797f, 111f); // With Player Framerate counter
                }
                else
                {
                    CanvasObject.SetActive(false);
                }
                yield return new WaitForEndOfFrame();
                if (((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) && Input.GetKey(KeyCode.E)) && !keyFlag)
                {
                    UIExpanded = !UIExpanded;
                    if (UIExpanded)
                        BackgroundImage.sprite = Resources.HUD_Base;
                    else
                        BackgroundImage.sprite = Resources.HUD_Minimized;
                    keyFlag = true;
                }
                if ((Input.GetKey((KeyCode)Configuration.JSONConfig.ToggleHUDEnabledKeybind[1]) || (KeyCode)Configuration.JSONConfig.ToggleHUDEnabledKeybind[1] == KeyCode.None) && Input.GetKey((KeyCode)Configuration.JSONConfig.ToggleHUDEnabledKeybind[0]) && !keyFlag && Configuration.JSONConfig.EnableKeybinds)
                {
                    Configuration.JSONConfig.HUDEnabled = !Configuration.JSONConfig.HUDEnabled;
                    Configuration.SaveConfig();
                    keyFlag = true;
                }
                if (!Input.GetKey(KeyCode.E) && !Input.GetKey((KeyCode)Configuration.JSONConfig.ToggleHUDEnabledKeybind[0]) && keyFlag)
                    keyFlag = false;
                if (TextText)
                {
                    if (UIExpanded)
                    {
                        string userList = "";
                        userList = CommonHUD.RenderPlayerList();
                        TextText.text =
                            "\n                  <color=#FF69B4>emmVRC</color>                        fps: " + Mathf.Floor(1.0f / Time.deltaTime) +
                            "\n                  press 'CTRL+E' to close" +
                            "\n" +
                            "\n" +
                            "\nUsers in room"+(RoomManager.field_Internal_Static_ApiWorldInstance_0 != null ? " ("+PlayerManager.field_Private_Static_PlayerManager_0.field_Private_List_1_Player_0.Count+")" : "")+":\n" + userList + "" +
                            "\n" +
                            "\nPosition in world:\n" + CommonHUD.RenderWorldInfo() +
                            "\n" +
                            "\n" +
                            (Configuration.JSONConfig.emmVRCNetworkEnabled ? (NetworkClient.webToken != null ? "<color=lime>Connected to the\nemmVRC Network</color>" : "<color=red>Not connected to the\nemmVRC Network</color>") : "") +
                            "\n" +
                            "\n" +
                            (Objects.Attributes.Debug ? (
                                "Current frame time: "+(Hacks.FrameTimeCalculator.frameTimes[Hacks.FrameTimeCalculator.iterator == 0 ? Hacks.FrameTimeCalculator.frameTimes.Length-1 : (Hacks.FrameTimeCalculator.iterator -1)])+"ms\n"+
                                "Average frame time: "+Hacks.FrameTimeCalculator.frameTimeAvg+"ms\n"
                            ) : "");
                        if (APIUser.CurrentUser != null && (Configuration.JSONConfig.InfoSpoofingEnabled))
                            TextText.text = TextText.text.Replace(APIUser.CurrentUser.GetName(), (NameSpoofGenerator.spoofedName));
                    }
                    else if (!UIExpanded)
                    {
                        TextText.text =
                            "\n                  <color=#FF69B4>emmVRC</color>                        fps: " + Mathf.Floor(1.0f / Time.deltaTime) +
                            "\n                  press 'CTRL+E' to open";
                    }
                }
            }
        }
    }
}

