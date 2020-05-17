using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VRC.Social;
using VRC;
using VRC.Core;
using VRC.UI;
using System.Collections;
using emmVRC.Managers;
using emmVRC.Libraries;

namespace emmVRC.Hacks
{
    internal class SocialMenuFunctions
    {
        private static GameObject SocialFunctionsButton;
        private static GameObject UserSendMessage;
        private static GameObject UserNotes;
        private static GameObject TeleportButton;
        private static GameObject ToggleBlockButton;

        public static void Initialize()
        {
            SocialFunctionsButton = GameObject.Instantiate(GameObject.Find("MenuContent/Screens/UserInfo/User Panel/Playlists/PlaylistsButton"), GameObject.Find("MenuContent/Screens/UserInfo/User Panel/Playlists").transform);
            SocialFunctionsButton.transform.SetParent(GameObject.Find("MenuContent/Screens/UserInfo/User Panel/").transform);
            GameObject.Destroy(SocialFunctionsButton.transform.Find("Image/Icon_New"));
            SocialFunctionsButton.GetComponentInChildren<Text>().text = "<color=#FF69B4>emmVRC</color> Functions";
            SocialFunctionsButton.GetComponentInChildren<Button>().onClick = new Button.ButtonClickedEvent();
            SocialFunctionsButton.GetComponent<RectTransform>().anchoredPosition += new Vector2(50, 150);
            SocialFunctionsButton.GetComponent<RectTransform>().sizeDelta -= new Vector2(0, 25f);

            UserSendMessage = GameObject.Instantiate(SocialFunctionsButton, SocialFunctionsButton.transform.parent);
            UserSendMessage.GetComponent<RectTransform>().anchoredPosition -= new Vector2(0, 60f);
            UserSendMessage.GetComponentInChildren<Text>().text = "Send Message";
            UserSendMessage.SetActive(false);

            UserNotes = GameObject.Instantiate(UserSendMessage, SocialFunctionsButton.transform.parent);
            UserNotes.GetComponent<RectTransform>().anchoredPosition -= new Vector2(0, 60f);
            UserNotes.GetComponentInChildren<Text>().text = "Notes";
            UserNotes.SetActive(false);

            TeleportButton = GameObject.Instantiate(UserNotes, SocialFunctionsButton.transform.parent);
            TeleportButton.GetComponent<RectTransform>().anchoredPosition -= new Vector2(0, 60f);
            TeleportButton.GetComponentInChildren<Text>().text = "Teleport";
            TeleportButton.SetActive(false);

            ToggleBlockButton = GameObject.Instantiate(TeleportButton, SocialFunctionsButton.transform.parent);
            ToggleBlockButton.GetComponent<RectTransform>().anchoredPosition -= new Vector2(0, 60f);
            ToggleBlockButton.GetComponentInChildren<Text>().text = "<color=#FF69B4>emmVRC</color> Block";
            ToggleBlockButton.SetActive(false);

            SocialFunctionsButton.GetComponentInChildren<Button>().onClick.AddListener(UnhollowerRuntimeLib.DelegateSupport.ConvertDelegate<UnityAction>((System.Action)(() => {
                //UserSendMessage.SetActive(!UserSendMessage.activeSelf);
                UserNotes.SetActive(!UserNotes.activeSelf);
                //ToggleBlockButton.SetActive(!ToggleBlocklButton.activeSelf);
                if (RiskyFunctionsManager.RiskyFunctionsAllowed)
                    TeleportButton.SetActive(!TeleportButton.activeSelf);
                else
                    TeleportButton.SetActive(false);
                GameObject.Find("MenuContent/Screens/UserInfo/User Panel/Playlists").SetActive(!GameObject.Find("MenuContent/Screens/UserInfo/User Panel/Playlists").activeSelf);
                GameObject.Find("MenuContent/Screens/UserInfo/User Panel/Favorite").SetActive(!GameObject.Find("MenuContent/Screens/UserInfo/User Panel/Favorite").activeSelf);
            })));

            UserSendMessage.GetComponentInChildren<Button>().onClick.AddListener(UnhollowerRuntimeLib.DelegateSupport.ConvertDelegate<UnityAction>((System.Action)(() => { 

            })));

            UserNotes.GetComponentInChildren<Button>().onClick.AddListener(UnhollowerRuntimeLib.DelegateSupport.ConvertDelegate<UnityAction>((System.Action)(() => {
                Hacks.PlayerNotes.LoadNote(QuickMenuUtils.GetVRCUiMInstance().menuContent.GetComponentInChildren<PageUserInfo>().user.id, QuickMenuUtils.GetVRCUiMInstance().menuContent.GetComponentInChildren<PageUserInfo>().user.displayName);
            })));

            TeleportButton.GetComponentInChildren<Button>().onClick.AddListener(UnhollowerRuntimeLib.DelegateSupport.ConvertDelegate<UnityAction>((System.Action)(() => {
                if (RiskyFunctionsManager.RiskyFunctionsAllowed)
                {
                    Player plrToTP = null;
                    Libraries.PlayerUtils.GetEachPlayer((Player plr) => {
                        if (plr.field_Private_APIUser_0.id == QuickMenuUtils.GetVRCUiMInstance().menuContent.GetComponentInChildren<PageUserInfo>().user.id)
                            plrToTP = plr;
                    });
                    if (plrToTP != null)
                    {
                        VRCPlayer.field_Internal_Static_VRCPlayer_0.transform.position = plrToTP.field_Internal_VRCPlayer_0.transform.position;
                    }
                    QuickMenuUtils.GetVRCUiMInstance().Method_Public_Void_Boolean_0();
                }
            })));

            MelonLoader.MelonCoroutines.Start(Loop());
        }
        private static IEnumerator Loop()
        {
            while (true)
            {
                yield return new WaitForEndOfFrame();
                if (RoomManager.field_Internal_Static_ApiWorld_0 != null)
                {
                    if (!GameObject.Find("MenuContent/Screens/UserInfo").activeSelf)
                    {
                        UserSendMessage.SetActive(false);
                        UserNotes.SetActive(false);
                        TeleportButton.SetActive(false);
                        ToggleBlockButton.SetActive(false);
                        GameObject.Find("MenuContent/Screens/UserInfo/User Panel/Playlists").SetActive(true);
                        GameObject.Find("MenuContent/Screens/UserInfo/User Panel/Favorite").SetActive(true);
                    }
                }
            }
        }
    }
}
