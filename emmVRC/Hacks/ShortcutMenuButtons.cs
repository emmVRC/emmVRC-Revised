﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using emmVRC.Libraries;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using emmVRC.Objects;

namespace emmVRC.Hacks
{
    public class ShortcutMenuButtons
    {
        public static QMSingleButton logoButton;
        public static GameObject emojiButton;
        public static GameObject emoteButton;
        public static GameObject reportWorldButton;
        public static GameObject trustRankButton;
        public static GameObject vrcPlusThankYouButton;
        public static GameObject vrcPlusUserIconButton;
        public static GameObject vrcPlusUserIconCameraButton;
        public static GameObject vrcPlusMiniBanner;
        public static GameObject vrcPlusMainBanner;

        public static IEnumerator Process()
        {
            if (logoButton == null)
            {
                logoButton = new QMSingleButton("ShortcutMenu", Configuration.JSONConfig.LogoButtonX, Configuration.JSONConfig.LogoButtonY, "", () => { System.Diagnostics.Process.Start("https://discord.gg/SpZSH5Z"); }, "emmVRC Version v" + Objects.Attributes.Version + " by the emmVRC Team. Click the logo to join our Discord!", Color.white, Color.white);
                while (Resources.onlineSprite == null) yield return null;
                logoButton.getGameObject().GetComponentInChildren<Image>().sprite = Resources.onlineSprite;
            }
            logoButton.setActive(Configuration.JSONConfig.LogoButtonEnabled);
            emojiButton = QuickMenuUtils.GetQuickMenuInstance().transform.Find("ShortcutMenu/EmojiButton").gameObject;
            emoteButton = QuickMenuUtils.GetQuickMenuInstance().transform.Find("ShortcutMenu/EmoteButton").gameObject;
            reportWorldButton = QuickMenuUtils.GetQuickMenuInstance().transform.Find("ShortcutMenu/ReportWorldButton").gameObject;
            trustRankButton = QuickMenuUtils.GetQuickMenuInstance().transform.Find("ShortcutMenu/Toggle_States_ShowTrustRank_Colors").gameObject;
            vrcPlusThankYouButton = QuickMenuUtils.GetQuickMenuInstance().transform.Find("ShortcutMenu/VRCPlusThankYou").gameObject;
            vrcPlusUserIconButton = QuickMenuUtils.GetQuickMenuInstance().transform.Find("ShortcutMenu/UserIconButton").gameObject;
            vrcPlusUserIconCameraButton = QuickMenuUtils.GetQuickMenuInstance().transform.Find("ShortcutMenu/UserIconCameraButton").gameObject;
            vrcPlusMiniBanner = QuickMenuUtils.GetQuickMenuInstance().transform.Find("ShortcutMenu/VRCPlusMiniBanner").gameObject;
            vrcPlusMainBanner = QuickMenuUtils.GetQuickMenuInstance().transform.Find("ShortcutMenu/HeaderContainer/VRCPlusBanner").gameObject;
            if (Configuration.JSONConfig.DisableEmojiButton)
                emojiButton.SetActive(false);
            else
                emojiButton.SetActive(true);
            if (Configuration.JSONConfig.DisableEmoteButton)
                emoteButton.SetActive(false);
            else
                emoteButton.SetActive(true);
            if (Configuration.JSONConfig.DisableReportWorldButton)
                reportWorldButton.SetActive(false);
            else
                reportWorldButton.SetActive(true);
            if (Configuration.JSONConfig.DisableRankToggleButton)
                trustRankButton.transform.localScale = Vector3.zero;
            else
                trustRankButton.transform.localScale = Vector3.one;
            if (Configuration.JSONConfig.DisableVRCPlusAds)
            {
                vrcPlusMiniBanner.GetComponent<Canvas>().enabled = false;
                vrcPlusMainBanner.GetComponent<Canvas>().enabled = false;
            }
            else
            {
                vrcPlusMiniBanner.GetComponent<Canvas>().enabled = true;
                vrcPlusMainBanner.GetComponent<Canvas>().enabled = true;
            }

            if (Configuration.JSONConfig.DisableVRCPlusQMButtons)
            {
                vrcPlusThankYouButton.transform.localScale = Vector3.zero;
                vrcPlusUserIconButton.transform.localScale = Vector3.zero;
                vrcPlusUserIconCameraButton.transform.localScale = Vector3.zero;
            }
            else
            {
                vrcPlusThankYouButton.transform.localScale = Vector3.one;
                vrcPlusUserIconButton.transform.localScale = Vector3.one;
                vrcPlusUserIconCameraButton.transform.localScale = Vector3.one;
            }
        }
    }
}
