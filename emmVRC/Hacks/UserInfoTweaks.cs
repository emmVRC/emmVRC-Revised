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
        public static PageUserInfo userInfo;
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
            EnableDisableListener listener = inst.AddComponent<EnableDisableListener>();
            listener.OnEnabled += () => MelonLoader.MelonCoroutines.Start(WaitForUserReady());
        }

        [UnhollowerBaseLib.Attributes.HideFromIl2Cpp]
        public static IEnumerator WaitForUserReady()
        {
            while (userInfo.field_Private_APIUser_0 == null || userInfo.field_Private_APIUser_0.id == lastCheckedId)
                yield return new WaitForEndOfFrame();
            if (userInfo.gameObject.activeInHierarchy)
            {try
                {
                    lastCheckedId = userInfo.field_Private_APIUser_0.id;
                    if (lastSeenText != null)
                    {
                        try
                        {
                            if (userInfo.field_Private_APIUser_0.statusValue != VRC.Core.APIUser.UserStatus.Offline || !userInfo.field_Private_APIUser_0.isFriend)
                                lastSeenText.GetComponent<Text>().text = "";
                            else
                            {
                                DateTime lastSeen = DateTime.Parse(userInfo.field_Private_APIUser_0.last_login);
                                lastSeenText.GetComponent<Text>().text = "Last login: " + lastSeen.ToString();
                            }
                        }
                        catch (Exception ex)
                        {
                            emmVRCLoader.Logger.LogError("Error parsing last login: " + ex.ToString());
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
