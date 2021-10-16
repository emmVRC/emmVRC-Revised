using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using VRC;
using VRC.UI;
using emmVRC.Objects.ModuleBases;
using emmVRC.Components;


namespace emmVRC.Hacks
{
    public class UserInfoTweaks : MelonLoaderEvents
    {
        public static GameObject lastSeenText;
        public static GameObject platformIcon;
        public static PageUserInfo userInfo;
        private static UiPlatformIcon icon;
        public static string lastCheckedId;


        public override void OnUiManagerInit()
        {
            GameObject inst = GameObject.Find("UserInterface/MenuContent/Screens/UserInfo");
            lastSeenText = GameObject.Instantiate(inst.transform.Find("User Panel/NameText").gameObject, inst.transform);
            lastSeenText.GetComponent<RectTransform>().anchoredPosition = new Vector2(975f, -795f);
            lastSeenText.GetComponent<Text>().alignment = TextAnchor.MiddleRight;
            lastSeenText.GetComponent<Text>().fontSize = 30;
            lastSeenText.GetComponent<Text>().text = "";
            userInfo = inst.transform.GetComponent<PageUserInfo>();
            platformIcon = GameObject.Instantiate(inst.transform.Find("AvatarImage/SupporterIcon").gameObject, inst.transform);
            platformIcon.GetComponent<RectTransform>().anchoredPosition = new Vector2(390f, -170f);
            GameObject.DestroyImmediate(platformIcon.GetComponent<Image>());
            platformIcon.AddComponent<RawImage>();
            platformIcon.SetActive(true);

            icon = GameObject.Find("UserInterface/QuickMenu/QuickMenu_NewElements/_CONTEXT/QM_Context_User_Hover/AvatarImage/PlatformIcon").GetComponent<UiPlatformIcon>();
            EnableDisableListener listener = inst.AddComponent<EnableDisableListener>();
            listener.OnEnabled += () => MelonLoader.MelonCoroutines.Start(WaitForUserReady());
        }

        [UnhollowerBaseLib.Attributes.HideFromIl2Cpp]
        public static IEnumerator WaitForUserReady()
        {
            while (userInfo.field_Public_APIUser_0 == null || userInfo.field_Public_APIUser_0.id == lastCheckedId)
                yield return new WaitForEndOfFrame();
            if (userInfo.gameObject.activeInHierarchy)
            {try
                {
                    lastCheckedId = userInfo.field_Public_APIUser_0.id;
                    if (lastSeenText != null)
                    {
                        try
                        {
                            if (userInfo.field_Public_APIUser_0.statusValue != VRC.Core.APIUser.UserStatus.Offline || !userInfo.field_Public_APIUser_0.isFriend)
                                lastSeenText.GetComponent<Text>().text = "";
                            else
                            {
                                DateTime lastSeen = DateTime.Parse(userInfo.field_Public_APIUser_0.last_login);
                                lastSeenText.GetComponent<Text>().text = "Last login: " + lastSeen.ToString();
                            }
                        }
                        catch (Exception ex)
                        {
                            emmVRCLoader.Logger.LogError("Error parsing last login: " + ex.ToString());
                        }
                    }
                    if (platformIcon != null)
                    {
                        try
                        {
                            if (userInfo.field_Public_APIUser_0.last_platform == "standalonewindows")
                                platformIcon.GetComponent<RawImage>().texture = icon.field_Public_GameObject_1.GetComponent<RawImage>().texture;
                            else
                                platformIcon.GetComponent<RawImage>().texture = icon.field_Public_GameObject_2.GetComponent<RawImage>().texture;
                        }
                        catch (Exception ex)
                        {
                            emmVRCLoader.Logger.LogError("Error with platform icon: " + ex.ToString());
                        }
                    }
                } catch (Exception ex)
                {
                    emmVRCLoader.Logger.LogError("Error while waiting for user: " + ex.ToString());
                }
            }
        }
    }
}
