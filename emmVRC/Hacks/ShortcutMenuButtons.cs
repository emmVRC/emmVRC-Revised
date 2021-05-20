using System;
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
using VRC.Core;
using VRC;

namespace emmVRC.Hacks
{
    public class ShortcutMenuButtons
    {
        public static QMSingleButton logoButton;
        public static GameObject emojiButton;
        public static GameObject emoteButton;
        public static GameObject reportWorldButton;
        public static GameObject trustRankButton;
        public static GameObject socialNotifications;
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
            logoButton.getGameObject().transform.localScale = (Configuration.JSONConfig.TabMode ? Vector3.zero : Vector3.one);
            emojiButton = QuickMenuUtils.GetQuickMenuInstance().transform.Find("ShortcutMenu/EmojiButton").gameObject;
            emoteButton = QuickMenuUtils.GetQuickMenuInstance().transform.Find("ShortcutMenu/EmoteButton").gameObject;
            reportWorldButton = QuickMenuUtils.GetQuickMenuInstance().transform.Find("ShortcutMenu/ReportWorldButton").gameObject;
            trustRankButton = QuickMenuUtils.GetQuickMenuInstance().transform.Find("ShortcutMenu/Toggle_States_ShowTrustRank_Colors").gameObject;
            socialNotifications = QuickMenuUtils.GetQuickMenuInstance().transform.Find("ShortcutMenu/HeaderContainer/SocialNotifications").gameObject;
            vrcPlusThankYouButton = QuickMenuUtils.GetQuickMenuInstance().transform.Find("ShortcutMenu/VRCPlusThankYou").gameObject;
            vrcPlusUserIconButton = QuickMenuUtils.GetQuickMenuInstance().transform.Find("ShortcutMenu/UserIconButton").gameObject;
            vrcPlusUserIconCameraButton = QuickMenuUtils.GetQuickMenuInstance().transform.Find("ShortcutMenu/UserIconCameraButton").gameObject;
            vrcPlusMiniBanner = QuickMenuUtils.GetQuickMenuInstance().transform.Find("ShortcutMenu/VRCPlusMiniBanner").gameObject;
            vrcPlusMainBanner = QuickMenuUtils.GetQuickMenuInstance().transform.Find("ShortcutMenu/HeaderContainer/VRCPlusBanner").gameObject;
            
            emojiButton.SetActive(!Configuration.JSONConfig.DisableEmojiButton);
            emoteButton.SetActive(!Configuration.JSONConfig.DisableEmoteButton);
            reportWorldButton.SetActive(!Configuration.JSONConfig.DisableReportWorldButton);
            trustRankButton.transform.localScale = (Configuration.JSONConfig.DisableRankToggleButton ? Vector3.zero : Vector3.one);
            socialNotifications.transform.localScale = (Configuration.JSONConfig.DisableOldInviteButtons ? Vector3.zero : Vector3.one);
            while (APIUser.CurrentUser == null || Objects.NetworkConfig.Instance == null)
                yield return new WaitForEndOfFrame();
            if (CustomAvatarFavorites.DoesUserHaveVRCPlus())
            {
                vrcPlusMiniBanner.GetComponent<Canvas>().enabled = !Configuration.JSONConfig.DisableVRCPlusAds;
                vrcPlusMainBanner.GetComponent<Canvas>().enabled = !Configuration.JSONConfig.DisableVRCPlusAds;
                vrcPlusThankYouButton.transform.localScale = (Configuration.JSONConfig.DisableVRCPlusQMButtons ? Vector3.zero : Vector3.one);
                vrcPlusUserIconButton.transform.localScale = (Configuration.JSONConfig.DisableVRCPlusQMButtons ? Vector3.zero : Vector3.one);
                vrcPlusUserIconCameraButton.transform.localScale = (Configuration.JSONConfig.DisableVRCPlusQMButtons ? Vector3.zero : Vector3.one);
            }
        }
    }
}
