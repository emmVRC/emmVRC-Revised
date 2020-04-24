using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace emmVRC.Hacks
{
    public class AvatarMenu
    {
        private static GameObject avatarScreen;
        private static GameObject hotWorldsViewPort;
        private static GameObject hotWorldsButton;
        private static GameObject randomWorldsViewPort;
        private static GameObject randomWorldsButton;
        private static GameObject legacyList;
        private static GameObject publicList;
        public static void Initialize()
        {
            avatarScreen = Libraries.QuickMenuUtils.GetVRCUiMInstance().menuContent.transform.Find("Screens/Avatar").gameObject;
            hotWorldsViewPort = avatarScreen.transform.Find("Vertical Scroll View/Viewport/Content/Avatar Worlds (What's Hot)/ViewPort").gameObject;
            hotWorldsButton = avatarScreen.transform.Find("Vertical Scroll View/Viewport/Content/Avatar Worlds (What's Hot)/Button").gameObject;
            randomWorldsViewPort = avatarScreen.transform.Find("Vertical Scroll View/Viewport/Content/Avatar Worlds (Random)/ViewPort").gameObject;
            randomWorldsButton = avatarScreen.transform.Find("Vertical Scroll View/Viewport/Content/Avatar Worlds (Random)/Button").gameObject;
            legacyList = avatarScreen.transform.Find("Vertical Scroll View/Viewport/Content/Legacy Avatar List").gameObject;
            publicList = avatarScreen.transform.Find("Vertical Scroll View/Viewport/Content/Public Avatar List").gameObject;
            MelonLoader.MelonCoroutines.Start(Loop());
        }
        private static IEnumerator Loop()
        {
            while (true)
            {
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
                if (Configuration.JSONConfig.DisableAvatarLegacy)
                {
                    legacyList.SetActive(false);
                }
                else
                {
                    legacyList.SetActive(true);
                }
                if (Configuration.JSONConfig.DisableAvatarPublic)
                {
                    publicList.SetActive(false);
                }
                else
                {
                    publicList.SetActive(true);
                }
                yield return new WaitForSeconds(1f);
            }
        }
    }
}
