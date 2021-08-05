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


namespace emmVRC.Hacks
{
    public class UserInfoTweaks : MonoBehaviour
    {
        public static bool classInjected = false;
        public GameObject lastSeenText;
        public GameObject platformIcon;
        public PageUserInfo userInfo;
        public string lastCheckedId;

        public UserInfoTweaks(IntPtr ptr) : base(ptr) { }
        public static UserInfoTweaks Instantiate(GameObject parent)
        {
            if (!classInjected)
            {
                UnhollowerRuntimeLib.ClassInjector.RegisterTypeInIl2Cpp<UserInfoTweaks>();
                classInjected = true;
            }
            return parent.AddComponent<UserInfoTweaks>();
        }
        public static void Initialize()
        {
            UserInfoTweaks inst = UserInfoTweaks.Instantiate(GameObject.Find("UserInterface/MenuContent/Screens/UserInfo"));
            inst.lastSeenText = GameObject.Instantiate(inst.transform.Find("User Panel/NameText").gameObject, inst.transform);
            inst.lastSeenText.GetComponent<RectTransform>().anchoredPosition = new Vector2(975f, -795f);
            inst.lastSeenText.GetComponent<Text>().alignment = TextAnchor.MiddleRight;
            inst.lastSeenText.GetComponent<Text>().fontSize = 30;
            inst.lastSeenText.GetComponent<Text>().text = "";
            inst.userInfo = inst.transform.GetComponent<PageUserInfo>();
            inst.platformIcon = GameObject.Instantiate(inst.transform.Find("AvatarImage/SupporterIcon").gameObject, inst.transform);
            inst.platformIcon.GetComponent<RectTransform>().anchoredPosition = new Vector2(390f, -170f);
            GameObject.DestroyImmediate(inst.platformIcon.GetComponent<Image>());
            inst.platformIcon.AddComponent<RawImage>();
            inst.platformIcon.SetActive(true);

        }
        public void OnEnable()
        {
            MelonLoader.MelonCoroutines.Start(WaitForUserReady());
        }
        [UnhollowerBaseLib.Attributes.HideFromIl2Cpp]
        public IEnumerator WaitForUserReady()
        {
            while (userInfo.field_Public_APIUser_0 == null || userInfo.field_Public_APIUser_0.id == lastCheckedId)
                yield return new WaitForEndOfFrame();
            if (gameObject.activeInHierarchy)
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
                                platformIcon.GetComponent<RawImage>().texture = UnityEngine.Resources.FindObjectsOfTypeAll<UiPlatformIcon>().First().field_Public_GameObject_1.GetComponent<RawImage>().texture;
                            else
                                platformIcon.GetComponent<RawImage>().texture = UnityEngine.Resources.FindObjectsOfTypeAll<UiPlatformIcon>().First().field_Public_GameObject_2.GetComponent<RawImage>().texture;
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
