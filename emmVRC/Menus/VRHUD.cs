﻿using emmVRC.Hacks;
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
        private static GameObject BackgroundObject;
        private static GameObject TextObject;
        private static Transform ShortcutMenu;
        public static bool Initialized = false;
        public static bool enabled = true;
        public static void Initialize() {
            emmVRCLoader.Logger.Log("[emmVRC] Initializing HUD canvas");
            BackgroundObject = new GameObject("Background");

            BackgroundObject.AddComponent<CanvasRenderer>();

            BackgroundObject.AddComponent<RawImage>();
            BackgroundObject.GetComponent<RectTransform>().sizeDelta = new Vector2(256, 768);
            BackgroundObject.GetComponent<RectTransform>().localScale = new Vector3(2.675f, 2.675f, 2.675f);
            BackgroundObject.GetComponent<RectTransform>().anchorMax += new Vector2(.95f, .125f);
            BackgroundObject.GetComponent<RectTransform>().anchorMin += new Vector2(.95f, .125f);
            BackgroundObject.transform.SetParent(QuickMenuUtils.GetQuickMenuInstance().transform.Find("ShortcutMenu"), false);
            BackgroundObject.GetComponent<RawImage>().texture = Resources.uiMinimized;
            if (Configuration.JSONConfig.MoveVRHUDIfSpaceFree && Configuration.JSONConfig.DisableRankToggleButton && Configuration.JSONConfig.DisableReportWorldButton && Configuration.JSONConfig.FunctionsButtonX != 5)
                if (!Configuration.JSONConfig.LogoButtonEnabled || Configuration.JSONConfig.LogoButtonX != 5)
                    BackgroundObject.GetComponent<RectTransform>().position -= new Vector3(0.125f, 0f, 0f);
            TextObject = new GameObject("Text");
            TextObject.AddComponent<CanvasRenderer>();
            TextObject.transform.SetParent(BackgroundObject.transform, false);
            Text text = TextObject.AddComponent<Text>();
            //text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            text.font = UnityEngine.Resources.GetBuiltinResource<Font>("Arial.ttf");
            text.fontSize = 17;
            text.text = "            emmVRClient  fps: 90";
            TextObject.GetComponent<RectTransform>().sizeDelta = new Vector2(250, 768);
            ShortcutMenu = QuickMenuUtils.GetQuickMenuInstance().transform.Find("ShortcutMenu");



            Initialized = true;
            MelonLoader.MelonCoroutines.Start(Loop());
        }

        private static IEnumerator Loop()
        {
            while (true)
            {
                if (Configuration.JSONConfig.HUDEnabled && Resources.uiMinimized != null && enabled)
                {
                    BackgroundObject.SetActive(true);
                }
                else
                {
                    BackgroundObject.SetActive(false);
                }
                yield return new WaitForEndOfFrame();
                if (BackgroundObject != null && TextObject != null && TextObject.GetComponent<Text>() != null)
                {
                    BackgroundObject.GetComponent<RawImage>().texture = Resources.uiMaximized;
                    string userList = CommonHUD.RenderPlayerList();
                    TextObject.GetComponent<Text>().text = "\n            <color=#FF69B4>emmVRC</color> v" + Objects.Attributes.Version +
                        "\n" +
                        "\n" +
                        "\nUsers in room:\n" + userList + "" +
                        "\n" +
                        "\n" +
                        "\nPosition in world:\n" + CommonHUD.RenderWorldInfo() +
                        "\n" +
                        "\n" +
                        "\n" +
                        (Configuration.JSONConfig.emmVRCNetworkEnabled ? (Network.NetworkClient.authToken != null ? "<color=lime>Connected to the\nemmVRC Network</color>" : "<color=red>Not connected to the\nemmVRC Network</color>") : "") +
                        "\n";
                    if (APIUser.CurrentUser != null && (Configuration.JSONConfig.InfoSpoofingEnabled || Configuration.JSONConfig.InfoHidingEnabled))
                        TextObject.GetComponent<Text>().text = TextObject.GetComponent<Text>().text.Replace((VRC.Core.APIUser.CurrentUser.displayName == "" ? VRC.Core.APIUser.CurrentUser.username : VRC.Core.APIUser.CurrentUser.displayName), (Configuration.JSONConfig.InfoHidingEnabled ? "⛧⛧⛧⛧⛧⛧⛧⛧⛧" : NameSpoofGenerator.spoofedName));
                }
            }
        }
        private static string GetPlayerColored(VRC.Player p)
        {
            VRC.Core.APIUser apiuser = p.field_Private_APIUser_0;
            string result;
            if (apiuser == VRC.Core.APIUser.CurrentUser)
            {
                result = "<b><color=aqua>" + apiuser.displayName + "</color></b>";
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
                result = "<b><color=white>" + apiuser.displayName + "</color></b>";
            }
            return result;
        }
    }
}

