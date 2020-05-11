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
using VRC.Core;

namespace emmVRC.Menus
{
    public class DesktopHUD
    {
        private static GameObject CanvasObject;
        private static GameObject BackgroundObject;
        private static GameObject TextObject;
        private static bool keyFlag;
        public static bool UIExpanded = false;
        public static void Initialize()
        {
            emmVRCLoader.Logger.Log("[emmVRC] Initializing HUD canvas");
            // UI Init
            CanvasObject = new GameObject("emmVRCUICanvas");
            GameObject.DontDestroyOnLoad(CanvasObject);
            Canvas canvas = CanvasObject.AddComponent<Canvas>();

            canvas.renderMode = RenderMode.ScreenSpaceOverlay;

            CanvasObject.transform.position = new Vector3(0, 0, 0);
            CanvasScaler scaler = CanvasObject.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(Screen.width, Screen.height);
            //          scaler.scaleFactor = 10.0f;
            //          scaler.dynamicPixelsPerUnit = 10.0f;

            //            canvasObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 1.0f);
            //            canvasObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 1.0f);
            BackgroundObject = new GameObject("Background");

            BackgroundObject.AddComponent<CanvasRenderer>();

            BackgroundObject.AddComponent<RawImage>();
            BackgroundObject.GetComponent<RectTransform>().sizeDelta = new Vector2(256, 768);
            BackgroundObject.GetComponent<RectTransform>().position = new Vector2(130 - (Screen.width / 2), (Screen.height / 6) - 64);
            BackgroundObject.transform.SetParent(CanvasObject.transform, false);
            BackgroundObject.GetComponent<RawImage>().texture = Resources.uiMinimized;
            TextObject = new GameObject("Text");
            TextObject.AddComponent<CanvasRenderer>();
            TextObject.transform.SetParent(BackgroundObject.transform, false);
            Text text = TextObject.AddComponent<Text>();
            //text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            text.font = UnityEngine.Resources.GetBuiltinResource<Font>("Arial.ttf");
            text.fontSize = 15;
            text.text = "            emmVRClient  fps: 90";
            TextObject.GetComponent<RectTransform>().sizeDelta = new Vector2(250, 768);

            CanvasObject.SetActive(false);

            MelonLoader.MelonCoroutines.Start(Loop());
        }

        private static IEnumerator Loop()
        {
            while (true)
            {
                if (Configuration.JSONConfig.HUDEnabled && Resources.uiMinimized != null)
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
                    keyFlag = true;
                }
                if ((Input.GetKey((KeyCode)Configuration.JSONConfig.ToggleHUDEnabledKeybind[1]) || (KeyCode)Configuration.JSONConfig.ToggleHUDEnabledKeybind[1] == KeyCode.None) && Input.GetKey((KeyCode)Configuration.JSONConfig.ToggleHUDEnabledKeybind[0]) && !keyFlag)
                {
                    Configuration.JSONConfig.HUDEnabled = !Configuration.JSONConfig.HUDEnabled;
                    Configuration.SaveConfig();
                    keyFlag = true;
                }
                    if (!Input.GetKey(KeyCode.E) && !Input.GetKey((KeyCode)Configuration.JSONConfig.ToggleHUDEnabledKeybind[0]) && keyFlag)
                    keyFlag = false;
                if (BackgroundObject != null && TextObject != null && TextObject.GetComponent<Text>() != null)
                {
                    BackgroundObject.GetComponent<RawImage>().texture = UIExpanded ? Resources.uiMaximized : Resources.uiMinimized;
                    if (UIExpanded)
                    {
                        string positionstr = "";
                        string userList = "";
                        string worldinfo = "";
                        //string emmVRCUserList = "";
                        if (RoomManager.field_Internal_Static_ApiWorld_0 != null)
                        {
                            PlayerUtils.GetEachPlayer((VRC.Player plr) => {
                                userList += plr.field_Private_APIUser_0.displayName + "\n";
                            });
                            worldinfo += "\nWorld name:\n" + RoomManager.field_Internal_Static_ApiWorld_0.name;
                            worldinfo += "\n\nWorld creator:\n" + RoomManager.field_Internal_Static_ApiWorld_0.authorName;
                            if (VRCPlayer.field_Internal_Static_VRCPlayer_0 != null)
                            {
                                positionstr += "<b><color=red>X: " + (Mathf.Floor(VRCPlayer.field_Internal_Static_VRCPlayer_0.gameObject.GetComponent<VRC.Player>().transform.position.x * 10)) / 10 + "</color></b>  ";
                                positionstr += "<b><color=lime>Y: " + (Mathf.Floor(VRCPlayer.field_Internal_Static_VRCPlayer_0.gameObject.GetComponent<VRC.Player>().transform.position.y * 10)) / 10 + "</color></b>  ";
                                positionstr += "<b><color=cyan>Z: " + (Mathf.Floor(VRCPlayer.field_Internal_Static_VRCPlayer_0.gameObject.GetComponent<VRC.Player>().transform.position.z * 10)) / 10 + "</color></b>  ";
                            }
                        }
                        TextObject.GetComponent<Text>().text = "\n                  <color=#FF69B4>emmVRC</color>            fps: " + Mathf.Floor(1.0f / Time.deltaTime) +
                            "\n                  press 'CTRL+E' to close" +
                            "\n" +
                            "\n" +
                            "\nUsers in room:\n" + userList + "" +
                            "\n" +
                            "\n" +
                            "\nPosition in world:\n" + positionstr +
                            "\n" +
                            "\n" + worldinfo +
                            "\n" +
                            "\n";
                        if (APIUser.CurrentUser != null && (Configuration.JSONConfig.InfoSpoofingEnabled || Configuration.JSONConfig.InfoHidingEnabled))
                            TextObject.GetComponent<Text>().text = TextObject.GetComponent<Text>().text.Replace((VRC.Core.APIUser.CurrentUser.displayName == "" ? VRC.Core.APIUser.CurrentUser.username : VRC.Core.APIUser.CurrentUser.displayName), (Configuration.JSONConfig.InfoHidingEnabled ? "⛧⛧⛧⛧⛧⛧⛧⛧⛧" : NameSpoofGenerator.spoofedName));
                    }
                    else if (!UIExpanded)
                    {
                        TextObject.GetComponent<Text>().text = "\n                  <color=#FF69B4>emmVRC</color>            fps: " + Mathf.Floor(1.0f / Time.deltaTime) +
                            "\n                  press 'CTRL+E' to open";
                    }
                }
            }
        }
        private static string GetPlayerColored(VRC.Player p)
        {
            VRC.Core.APIUser apiuser = p.field_Private_APIUser_0;
            string result;
            if (apiuser == VRC.Core.APIUser.CurrentUser)
            {
                result = "<b><color=white>" + apiuser.displayName + "</color></b>";
            }
            else if (apiuser.isFriend)
            {
                result = "<b><color=yellow>" + apiuser.displayName + "</color></b>";
            }
            else if (apiuser.hasSuperPowers)
            {
                result = "<b><color=red>" + apiuser.displayName + "</color></b> [Mod]";
            }
            else
            {
                result = "<b><color=cyan>" + apiuser.displayName + "</color></b>";
            }
            return result;
        }
    }
}

