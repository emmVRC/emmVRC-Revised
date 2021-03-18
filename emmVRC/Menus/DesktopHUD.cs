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
                                    userList += (plr.field_Private_VRCPlayerApi_0.isMaster ? "♕ " : "     ") + "<color=#" + Libraries.ColorConversion.ColorToHex(VRCPlayer.Method_Public_Static_Color_APIUser_0(plr.field_Private_APIUser_0)) + ">" + plr.field_Private_APIUser_0.GetName() + "</color> - " + plr.field_Internal_VRCPlayer_0.prop_Int16_0 + " ms - " + plr.GetVRCPlayer().GetFrames() + " fps\n"; // or GetFramesColored()
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
            string worldinfo = "";
            if (RoomManager.field_Internal_Static_ApiWorld_0 != null)
            {
                worldinfo += "\nWorld name:\n" + RoomManager.field_Internal_Static_ApiWorld_0.name;
                worldinfo += "\n\nWorld creator:\n" + RoomManager.field_Internal_Static_ApiWorld_0.authorName;
                if (VRCPlayer.field_Internal_Static_VRCPlayer_0 != null)
                {
                    positionstr += "<b><color=red>X: " + (Mathf.Floor(VRCPlayer.field_Internal_Static_VRCPlayer_0.gameObject.GetComponent<VRC.Player>().transform.position.x * 10)) / 10 + "</color></b>  ";
                    positionstr += "<b><color=lime>Y: " + (Mathf.Floor(VRCPlayer.field_Internal_Static_VRCPlayer_0.gameObject.GetComponent<VRC.Player>().transform.position.y * 10)) / 10 + "</color></b>  ";
                    positionstr += "<b><color=cyan>Z: " + (Mathf.Floor(VRCPlayer.field_Internal_Static_VRCPlayer_0.gameObject.GetComponent<VRC.Player>().transform.position.z * 10)) / 10 + "</color></b>  ";
                }
            }
            return positionstr + "\n\n" + worldinfo;
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
        private static Image BackgroundImage;
        private static Text TextText;
        private static bool keyFlag;
        public static bool UIExpanded = false;
        public static bool Initialized = false;
        public static bool enabled = true;
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
            CanvasScaler scaler = CanvasObject.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(Screen.width, Screen.height);
            //          scaler.scaleFactor = 10.0f;
            //          scaler.dynamicPixelsPerUnit = 10.0f;

            //            canvasObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 1.0f);
            //            canvasObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 1.0f);
            BackgroundObject = new GameObject("Background");

            BackgroundObject.AddComponent<CanvasRenderer>();

            BackgroundImage = BackgroundObject.AddComponent<Image>();
            //BackgroundObject.GetComponent<RectTransform>().sizeDelta = new Vector2(256, 768);
            BackgroundObject.GetComponent<RectTransform>().sizeDelta = new Vector2(325, 768); // With Player Framerate counter
            BackgroundObject.GetComponent<RectTransform>().position = new Vector2(130 - (Screen.width / 2), (Screen.height / 6) - 64);
            BackgroundObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(-927f, 111f); // With Player Framerate counter
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
                        TextText.text = "\n                  <color=#FF69B4>emmVRC</color>            fps: " + Mathf.Floor(1.0f / Time.deltaTime) +
                            "\n                  press 'CTRL+E' to close" +
                            "\n" +
                            "\n" +
                            "\nUsers in room"+(RoomManager.field_Internal_Static_ApiWorldInstance_0 != null ? " ("+PlayerManager.field_Private_Static_PlayerManager_0.field_Private_List_1_Player_0.Count+")" : "")+":\n" + userList + "" +
                            "\n" +
                            "\nPosition in world:\n" + CommonHUD.RenderWorldInfo() +
                            "\n" +
                            "\n" +
                            (Configuration.JSONConfig.emmVRCNetworkEnabled ? (NetworkClient.webToken != null ? "<color=lime>Connected to the\nemmVRC Network</color>" : "<color=red>Not connected to the\nemmVRC Network</color>") : "") +
                            "\n";
                        if (APIUser.CurrentUser != null && (Configuration.JSONConfig.InfoSpoofingEnabled || Configuration.JSONConfig.InfoHidingEnabled))
                            TextText.text = TextText.text.Replace(APIUser.CurrentUser.GetName(), (Configuration.JSONConfig.InfoHidingEnabled ? "⛧⛧⛧⛧⛧⛧⛧⛧⛧" : NameSpoofGenerator.spoofedName));
                    }
                    else if (!UIExpanded)
                    {
                        TextText.text = "\n                  <color=#FF69B4>emmVRC</color>            fps: " + Mathf.Floor(1.0f / Time.deltaTime) +
                            "\n                  press 'CTRL+E' to open";
                    }
                }
            }
        }
    }
}

