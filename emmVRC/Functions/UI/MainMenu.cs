using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using emmVRC.Objects.ModuleBases;

namespace emmVRC.Functions.UI
{
    public class MainMenu : MelonLoaderEvents
    {
        public static bool menuJustOpened = false;
        public static bool menuOpen = false;
        public static GameObject vrcPlusButton;
        public static GameObject userIconsButton;
        public static GameObject worldButton;
        public static GameObject avatarButton;
        public static GameObject socialButton;
        public static GameObject settingsButton;
        public static GameObject safetyButton;
        public override void OnUiManagerInit()
        {
            if (Functions.Core.ModCompatibility.VRCMinus) return;
            Components.EnableDisableListener listener = GameObject.Find("/UserInterface/MenuContent/Backdrop/Header/Tabs").AddComponent<Components.EnableDisableListener>();
            listener.OnEnabled += () =>
            {
                if (vrcPlusButton == null)
                {
                    vrcPlusButton = GameObject.Find("/UserInterface/MenuContent/Backdrop/Header/Tabs/ViewPort/Content/VRC+PageTab/");
                    userIconsButton = GameObject.Find("/UserInterface/MenuContent/Backdrop/Header/Tabs/ViewPort/Content/GalleryTab/");
                    worldButton = GameObject.Find("/UserInterface/MenuContent/Backdrop/Header/Tabs/ViewPort/Content/WorldsPageTab/");
                    avatarButton = GameObject.Find("/UserInterface/MenuContent/Backdrop/Header/Tabs/ViewPort/Content/AvatarPageTab/");
                    socialButton = GameObject.Find("/UserInterface/MenuContent/Backdrop/Header/Tabs/ViewPort/Content/SocialPageTab/");
                    settingsButton = GameObject.Find("/UserInterface/MenuContent/Backdrop/Header/Tabs/ViewPort/Content/SettingsPageTab/");
                    safetyButton = GameObject.Find("/UserInterface/MenuContent/Backdrop/Header/Tabs/ViewPort/Content/SafetyPageTab/");
                    worldButton.GetComponent<LayoutElement>().preferredWidth = 250f;
                    avatarButton.GetComponent<LayoutElement>().preferredWidth = 250f;
                    socialButton.GetComponent<LayoutElement>().preferredWidth = 250f;
                    settingsButton.GetComponent<LayoutElement>().preferredWidth = 250f;
                    safetyButton.GetComponent<LayoutElement>().preferredWidth = 250f;
                }
                vrcPlusButton.SetActive(!Configuration.JSONConfig.DisableVRCPlusMenuTabs);
                userIconsButton.GetComponent<LayoutElement>().enabled = !Configuration.JSONConfig.DisableVRCPlusMenuTabs;
                userIconsButton.GetComponent<Image>().enabled = !Configuration.JSONConfig.DisableVRCPlusMenuTabs;
                userIconsButton.transform.Find("Image_NEW").gameObject.SetActive(!Configuration.JSONConfig.DisableVRCPlusMenuTabs);
                userIconsButton.SetActive(!Configuration.JSONConfig.DisableVRCPlusMenuTabs);
            };
        }
    }
}
