using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using emmVRC.Libraries;
using emmVRC.Objects.ModuleBases;

namespace emmVRC.Functions.UI
{
    public class AvatarMenu : MelonLoaderEvents
    {
        private static GameObject avatarScreen;
        private static GameObject hotWorldsViewPort;
        private static GameObject hotWorldsButton;
        private static GameObject randomWorldsViewPort;
        private static GameObject randomWorldsButton;
        private static GameObject personalList;
        private static GameObject legacyList;
        private static GameObject publicList;
        private static GameObject otherList;
        private static UiAvatarList otherListList;

        public override void OnUiManagerInit()
        {
            // Gather all the GameObjects for the VRChat default avatar lists (that we can about anyway)
            avatarScreen = UnityEngine.Resources.FindObjectsOfTypeAll<VRC.UI.PageAvatar>().FirstOrDefault().gameObject;
            hotWorldsViewPort = avatarScreen.transform.Find("Vertical Scroll View/Viewport/Content/Avatar Worlds (What's Hot)/ViewPort").gameObject;
            hotWorldsButton = avatarScreen.transform.Find("Vertical Scroll View/Viewport/Content/Avatar Worlds (What's Hot)/Button").gameObject;
            randomWorldsViewPort = avatarScreen.transform.Find("Vertical Scroll View/Viewport/Content/Avatar Worlds (Random)/ViewPort").gameObject;
            randomWorldsButton = avatarScreen.transform.Find("Vertical Scroll View/Viewport/Content/Avatar Worlds (Random)/Button").gameObject;
            personalList = avatarScreen.transform.Find("Vertical Scroll View/Viewport/Content/Personal Avatar List").gameObject;
            legacyList = avatarScreen.transform.Find("Vertical Scroll View/Viewport/Content/Legacy Avatar List").gameObject;
            publicList = avatarScreen.transform.Find("Vertical Scroll View/Viewport/Content/Public Avatar List").gameObject;
            otherList = avatarScreen.transform.Find("Vertical Scroll View/Viewport/Content/Licensed Avatar List (1)").gameObject;
            if (otherList == null)
                otherList = avatarScreen.transform.Find("Vertical Scroll View/Viewport/Content/Licensed Avatar List").gameObject;
            if (otherList != null)
                otherListList = otherList.GetComponent<UiAvatarList>();

            Components.EnableDisableListener listener = avatarScreen.AddComponent<Components.EnableDisableListener>();
            listener.OnEnabled += () =>
            {
                // Disable or enable the "Hot Avatar Worlds" category
                if (Configuration.JSONConfig.DisableAvatarHotWorlds)
                {
                    hotWorldsViewPort.SetActive(false);
                    hotWorldsButton.SetActive(false);
                }
                else
                {
                    hotWorldsViewPort.SetActive(true);
                    hotWorldsButton.SetActive(true);
                }

                // Disable or enable the "Random Avatar Worlds" category
                if (Configuration.JSONConfig.DisableAvatarRandomWorlds)
                {
                    randomWorldsViewPort.SetActive(false);
                    randomWorldsButton.SetActive(false);
                }
                else
                {
                    randomWorldsViewPort.SetActive(true);
                    randomWorldsButton.SetActive(true);
                }

                // Disable or enable the "Personal Avatars" category
                if (Configuration.JSONConfig.DisableAvatarPersonal)
                {
                    personalList.SetActive(false);
                }
                else
                {
                    personalList.SetActive(true);
                }

                // Disable or enable the "Legacy Avatars" category
                if (Configuration.JSONConfig.DisableAvatarLegacy)
                {
                    legacyList.SetActive(false);
                }
                else
                {
                    legacyList.SetActive(true);
                }

                // Disable or enable the "Public Avatars" category
                if (Configuration.JSONConfig.DisableAvatarPublic)
                {
                    publicList.SetActive(false);
                }
                else
                {
                    publicList.SetActive(true);
                }

                // Disable or enable the "Other" category
                if (Configuration.JSONConfig.DisableAvatarOther && otherListList != null)
                {
                    otherList.SetActive(false);
                }
                else if (!Configuration.JSONConfig.DisableAvatarOther && otherListList != null)
                {
                    if (otherListList.pickers.ToArray().Any(a => a.isActiveAndEnabled))
                        otherList.SetActive(true);
                }
            };
        }
    }
}
