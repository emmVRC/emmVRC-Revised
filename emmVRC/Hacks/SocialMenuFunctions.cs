using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VRC;
using VRC.Core;
using VRC.UI;
using System.Collections;
using emmVRC.Managers;
using emmVRC.Libraries;
using BestHTTP.ServerSentEvents;

namespace emmVRC.Hacks
{
    internal class SocialMenuFunctions
    {
        private static GameObject SocialFunctionsButton;
        private static GameObject UserSendMessage;
        private static GameObject UserNotes;
        private static GameObject TeleportButton;
        private static GameObject ToggleBlockButton;
        private static GameObject PortalToUserButton;

        private static int PortalCooldownTimer = 0;

        public static void Initialize()
        {
            SocialFunctionsButton = GameObject.Instantiate(GameObject.Find("MenuContent/Screens/UserInfo/User Panel/Playlists/PlaylistsButton"), GameObject.Find("MenuContent/Screens/UserInfo/User Panel/Playlists").transform);
            SocialFunctionsButton.transform.SetParent(GameObject.Find("MenuContent/Screens/UserInfo/User Panel/").transform);
            GameObject.Destroy(SocialFunctionsButton.transform.Find("Image/Icon_New").gameObject);
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

            PortalToUserButton = GameObject.Instantiate(TeleportButton, SocialFunctionsButton.transform.parent);
            PortalToUserButton.GetComponent<RectTransform>().anchoredPosition -= new Vector2(0, 60f);
            PortalToUserButton.GetComponentInChildren<Text>().text = "Drop Portal";
            PortalToUserButton.SetActive(false);

            SocialFunctionsButton.GetComponentInChildren<Button>().onClick.AddListener(new System.Action(() => {
                UserSendMessage.SetActive(!UserSendMessage.activeSelf);
                UserNotes.SetActive(!UserNotes.activeSelf);
                //ToggleBlockButton.SetActive(!ToggleBlocklButton.activeSelf);
                if (RiskyFunctionsManager.RiskyFunctionsAllowed)
                    TeleportButton.SetActive(!TeleportButton.activeSelf);
                else
                    TeleportButton.SetActive(false);
                try
                {
                    if (QuickMenuUtils.GetVRCUiMInstance().menuContent.GetComponentInChildren<PageUserInfo>() != null && QuickMenuUtils.GetVRCUiMInstance().menuContent.GetComponentInChildren<PageUserInfo>().user != null && QuickMenuUtils.GetVRCUiMInstance().menuContent.GetComponentInChildren<PageUserInfo>().user.location != "private" && QuickMenuUtils.GetVRCUiMInstance().menuContent.GetComponentInChildren<PageUserInfo>().user.location != "" && !QuickMenuUtils.GetVRCUiMInstance().menuContent.GetComponentInChildren<PageUserInfo>().user.location.Contains("friends"))
                        PortalToUserButton.SetActive(!PortalToUserButton.activeSelf);
                    else
                        PortalToUserButton.SetActive(false);
                } catch (Exception ex)
                {
                    ex = new Exception();
                }
                try
                {
                    GameObject.Find("MenuContent/Screens/UserInfo/User Panel/Playlists").SetActive(!GameObject.Find("MenuContent/Screens/UserInfo/User Panel/Playlists").activeSelf);
                } catch (Exception ex)
                {
                    ex = new Exception();
                }
                try
                {
                    GameObject.Find("MenuContent/Screens/UserInfo/User Panel/Favorite").SetActive(!GameObject.Find("MenuContent/Screens/UserInfo/User Panel/Favorite").activeSelf);
                } catch (Exception ex)
                {
                    ex = new Exception();
                }
            }));

            UserSendMessage.GetComponentInChildren<Button>().onClick.AddListener(new System.Action(() => {
                VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowInputPopup("Send a message to " + QuickMenuUtils.GetVRCUiMInstance().menuContent.GetComponentInChildren<PageUserInfo>().user.displayName + ":", "", UnityEngine.UI.InputField.InputType.Standard, false, "Send", new System.Action<string, Il2CppSystem.Collections.Generic.List<UnityEngine.KeyCode>, UnityEngine.UI.Text>((string msg, Il2CppSystem.Collections.Generic.List<UnityEngine.KeyCode> keyk, UnityEngine.UI.Text tx) => {
                    
                        MelonLoader.MelonCoroutines.Start(MessageManager.SendMessage(msg, QuickMenuUtils.GetVRCUiMInstance().menuContent.GetComponentInChildren<PageUserInfo>().user.id));
                }), null, "Enter message...");
            }));

            UserNotes.GetComponentInChildren<Button>().onClick.AddListener(new System.Action(() => {
                Hacks.PlayerNotes.LoadNote(QuickMenuUtils.GetVRCUiMInstance().menuContent.GetComponentInChildren<PageUserInfo>().user.id, QuickMenuUtils.GetVRCUiMInstance().menuContent.GetComponentInChildren<PageUserInfo>().user.displayName);
            }));

            TeleportButton.GetComponentInChildren<Button>().onClick.AddListener(new System.Action(() => {
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
                    QuickMenuUtils.GetVRCUiMInstance().Method_Public_Void_Boolean_4();
                }
            }));
            PortalToUserButton.GetComponentInChildren<Button>().onClick.AddListener(new System.Action(() => {
                if (QuickMenuUtils.GetVRCUiMInstance().menuContent.GetComponentInChildren<PageUserInfo>().user.location != "private" && QuickMenuUtils.GetVRCUiMInstance().menuContent.GetComponentInChildren<PageUserInfo>().user.location != "" && !QuickMenuUtils.GetVRCUiMInstance().menuContent.GetComponentInChildren<PageUserInfo>().user.location.Contains("friends"))
                {try
                    {
                        if (PortalCooldownTimer == 0)
                        {
                            string[] instanceInfo = QuickMenuUtils.GetVRCUiMInstance().menuContent.GetComponentInChildren<PageUserInfo>().user.location.Split(':');
                            emmVRCLoader.Logger.Log(instanceInfo[1]);
                            GameObject portal = VRC.SDKBase.Networking.Instantiate(VRC.SDKBase.VRC_EventHandler.VrcBroadcastType.Always, "Portals/PortalInternalDynamic", VRCPlayer.field_Internal_Static_VRCPlayer_0.transform.position + VRCPlayer.field_Internal_Static_VRCPlayer_0.transform.forward * 2, VRCPlayer.field_Internal_Static_VRCPlayer_0.transform.rotation);
                            VRC.SDKBase.Networking.RPC(VRC.SDKBase.RPC.Destination.AllBufferOne, portal, "ConfigurePortal", new Il2CppSystem.Object[]
                            {
                        (Il2CppSystem.String)instanceInfo[0],
                        (Il2CppSystem.String)instanceInfo[1],
                        new Il2CppSystem.Int32
                        {
                            m_value = 0
                        }.BoxIl2CppObject()
                            });
                            PortalCooldownTimer = 5;
                            MelonLoader.MelonCoroutines.Start(PortalCooldown());
                        }
                        else
                        {
                            VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowStandardPopup("emmVRC", "You must wait "+PortalCooldownTimer+" seconds before dropping another portal.", "Dismiss", new System.Action(() => { VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup(); }));
                        }
                    } catch (Exception ex)
                    {
                        emmVRCLoader.Logger.LogError(ex.ToString());
                    }
                }
                else
                {
                    VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowStandardPopup("emmVRC", "You cannot drop a portal to this user.", "Dismiss", new System.Action(() => { VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup(); }));
                }
            }));

            MelonLoader.MelonCoroutines.Start(Loop());
        }
        public static IEnumerator PortalCooldown() { 
        while (PortalCooldownTimer > 0)
            {
                yield return new WaitForSeconds(1f);
                PortalCooldownTimer--;
            }
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
                        PortalToUserButton.SetActive(false);
                        try
                        {
                            GameObject.Find("MenuContent/Screens/UserInfo/User Panel/Playlists").SetActive(true);
                            
                        } catch (Exception ex)
                        {
                            ex = new Exception();
                        }
                        try
                        {
                            GameObject.Find("MenuContent/Screens/UserInfo/User Panel/Favorite").SetActive(true);
                        }
                        catch (Exception ex)
                        {
                            ex = new Exception();
                        }
                        
                    }
                }
            }
        }
    }
}
