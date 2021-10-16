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
using emmVRC.Objects.ModuleBases;
using VRC.Core;
using VRC;

namespace emmVRC.Hacks
{
    public class ShortcutMenuButtons : MelonLoaderEvents
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
        public override void OnUiManagerInit()
        {
            MelonLoader.MelonCoroutines.Start(Process());
            if (Functions.Other.BuildNumber.buildNumber < 1134)
            {
                Configuration.onConfigUpdated.Add(new System.Collections.Generic.KeyValuePair<string, Action>("LogoButtonX", CalcLogoPosition));
                Configuration.onConfigUpdated.Add(new System.Collections.Generic.KeyValuePair<string, Action>("LogoButtonY", CalcLogoPosition));
                Configuration.onConfigUpdated.Add(new System.Collections.Generic.KeyValuePair<string, Action>("FunctionsButtonX", CalcFunctionsPosition));
                Configuration.onConfigUpdated.Add(new System.Collections.Generic.KeyValuePair<string, Action>("FunctionsButtonY", CalcFunctionsPosition));
                Configuration.onConfigUpdated.Add(new System.Collections.Generic.KeyValuePair<string, Action>("NotificationButtonPositionX", CalcNotificationsPosition));
                Configuration.onConfigUpdated.Add(new System.Collections.Generic.KeyValuePair<string, Action>("NotificationButtonPositionY", CalcNotificationsPosition));

                Configuration.onConfigUpdated.Add(new System.Collections.Generic.KeyValuePair<string, Action>("LogoButtonEnabled", () => MelonLoader.MelonCoroutines.Start(Process())));
                Configuration.onConfigUpdated.Add(new System.Collections.Generic.KeyValuePair<string, Action>("DisableReportWorldButton", () => MelonLoader.MelonCoroutines.Start(Process())));
                Configuration.onConfigUpdated.Add(new System.Collections.Generic.KeyValuePair<string, Action>("DisableEmojiButton", () => MelonLoader.MelonCoroutines.Start(Process())));
                Configuration.onConfigUpdated.Add(new System.Collections.Generic.KeyValuePair<string, Action>("DisableEmoteButton", () => MelonLoader.MelonCoroutines.Start(Process())));
                Configuration.onConfigUpdated.Add(new System.Collections.Generic.KeyValuePair<string, Action>("DisableRankToggleButton", () => MelonLoader.MelonCoroutines.Start(Process())));

                Configuration.onConfigUpdated.Add(new System.Collections.Generic.KeyValuePair<string, Action>("DisableVRCPlusAds", () => MelonLoader.MelonCoroutines.Start(Process())));
                Configuration.onConfigUpdated.Add(new System.Collections.Generic.KeyValuePair<string, Action>("DisableVRCPlusQMButtons", () => MelonLoader.MelonCoroutines.Start(Process())));
            }
            Configuration.onConfigUpdated.Add(new System.Collections.Generic.KeyValuePair<string, Action>("DisableOldInviteButtons", () => MelonLoader.MelonCoroutines.Start(Process())));

        }

        public static void CalcLogoPosition()
        {

            QMSingleButton tempPosition = new QMSingleButton("ShortcutMenu", 1, 0, "", null, "");
            logoButton.getGameObject().GetComponent<RectTransform>().anchoredPosition = tempPosition.getGameObject().GetComponent<RectTransform>().anchoredPosition;
            tempPosition.DestroyMe();
            logoButton.setLocation(Configuration.JSONConfig.LogoButtonX, Configuration.JSONConfig.LogoButtonY);
        }
        public static void CalcFunctionsPosition()
        {
            QMSingleButton tempPosition = new QMSingleButton("ShortcutMenu", 1, 0, "", null, "");
            Menus.FunctionsMenuLegacy.baseMenu.menuEntryButton.getGameObject().GetComponent<RectTransform>().anchoredPosition = tempPosition.getGameObject().GetComponent<RectTransform>().anchoredPosition;
            tempPosition.DestroyMe();
            Menus.FunctionsMenuLegacy.baseMenu.menuEntryButton.setLocation(Configuration.JSONConfig.FunctionsButtonX, Configuration.JSONConfig.FunctionsButtonY);
        }
        public static void CalcNotificationsPosition()
        {
            QMSingleButton tempPosition = new QMSingleButton("ShortcutMenu", 1, 0, "", null, "");
            Managers.NotificationManager.NotificationMenu.getMainButton().getGameObject().GetComponent<RectTransform>().anchoredPosition = tempPosition.getGameObject().GetComponent<RectTransform>().anchoredPosition;
            tempPosition.DestroyMe();

            Managers.NotificationManager.NotificationMenu.getMainButton().setLocation(Configuration.JSONConfig.NotificationButtonPositionX, Configuration.JSONConfig.NotificationButtonPositionY);
            Managers.NotificationManager.AddNotification("Your new button position has been saved.", "Dismiss", Managers.NotificationManager.DismissCurrentNotification, "", null, Functions.Core.Resources.alertSprite, -1);
        }

        public static IEnumerator Process()
        {

            if (Functions.Other.BuildNumber.buildNumber < 1134)
            {
                if (logoButton == null)
                {
                    logoButton = new QMSingleButton("ShortcutMenu", Configuration.JSONConfig.LogoButtonX, Configuration.JSONConfig.LogoButtonY, "", () => { System.Diagnostics.Process.Start("https://discord.gg/SpZSH5Z"); }, "emmVRC Version v" + Objects.Attributes.Version + " by the emmVRC Team. Click the logo to join our Discord!", Color.white, Color.white);
                    while (Functions.Core.Resources.onlineSprite == null) yield return null;
                    logoButton.getGameObject().GetComponentInChildren<Image>().sprite = Functions.Core.Resources.onlineSprite;
                }
                logoButton.setActive(Configuration.JSONConfig.LogoButtonEnabled);
                logoButton.getGameObject().transform.localScale = (Configuration.JSONConfig.TabMode ? Vector3.zero : Vector3.one);
                emojiButton = QuickMenuUtils.GetQuickMenuInstance().transform.Find("ShortcutMenu/EmojiButton").gameObject;
                emoteButton = QuickMenuUtils.GetQuickMenuInstance().transform.Find("ShortcutMenu/EmoteButton").gameObject;
                reportWorldButton = QuickMenuUtils.GetQuickMenuInstance().transform.Find("ShortcutMenu/ReportWorldButton").gameObject;
                trustRankButton = QuickMenuUtils.GetQuickMenuInstance().transform.Find("ShortcutMenu/Toggle_States_ShowTrustRank_Colors").gameObject;
                vrcPlusThankYouButton = QuickMenuUtils.GetQuickMenuInstance().transform.Find("ShortcutMenu/VRCPlusThankYou").gameObject;
                vrcPlusUserIconButton = QuickMenuUtils.GetQuickMenuInstance().transform.Find("ShortcutMenu/GalleryButton").gameObject;
                vrcPlusUserIconCameraButton = QuickMenuUtils.GetQuickMenuInstance().transform.Find("ShortcutMenu/UserIconCameraButton").gameObject;
                vrcPlusMiniBanner = QuickMenuUtils.GetQuickMenuInstance().transform.Find("ShortcutMenu/VRCPlusMiniBanner").gameObject;
                vrcPlusMainBanner = QuickMenuUtils.GetQuickMenuInstance().transform.Find("ShortcutMenu/HeaderContainer/VRCPlusBanner").gameObject;
                socialNotifications = QuickMenuUtils.GetQuickMenuInstance().transform.Find("ShortcutMenu/HeaderContainer/SocialNotifications").gameObject;
                emojiButton.SetActive(!Configuration.JSONConfig.DisableEmojiButton);
                emoteButton.SetActive(!Configuration.JSONConfig.DisableEmoteButton);
                reportWorldButton.SetActive(!Configuration.JSONConfig.DisableReportWorldButton);
                trustRankButton.transform.localScale = (Configuration.JSONConfig.DisableRankToggleButton ? Vector3.zero : Vector3.one);
                socialNotifications.transform.localScale = (Configuration.JSONConfig.DisableOldInviteButtons ? Vector3.zero : Vector3.one);
                while (APIUser.CurrentUser == null || Objects.NetworkConfig.Instance == null)
                    yield return new WaitForEndOfFrame();
                vrcPlusMiniBanner.GetComponent<Canvas>().enabled = !Configuration.JSONConfig.DisableVRCPlusAds;
                vrcPlusMainBanner.GetComponent<Canvas>().enabled = !Configuration.JSONConfig.DisableVRCPlusAds;
                vrcPlusThankYouButton.transform.localScale = (Configuration.JSONConfig.DisableVRCPlusQMButtons ? Vector3.zero : Vector3.one);
                vrcPlusUserIconButton.transform.localScale = (Configuration.JSONConfig.DisableVRCPlusQMButtons ? Vector3.zero : Vector3.one);
                vrcPlusUserIconCameraButton.transform.localScale = (Configuration.JSONConfig.DisableVRCPlusQMButtons ? Vector3.zero : Vector3.one);
            }
            else
            {
                socialNotifications = GameObject.Find("UserInterface").transform.Find("Canvas_QuickMenu(Clone)/Container/Window/QMNotificationsArea/NotificationsAndThankYou/SocialNotificationsOverlay").gameObject;
                socialNotifications.transform.localScale = (Configuration.JSONConfig.DisableOldInviteButtons ? Vector3.zero : Vector3.one);
            }


            
        }
    }
}
