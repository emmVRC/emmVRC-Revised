using Il2CppSystem;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using VRC;
using VRC.Core;
using UnityEngine;
using UnityEngine.UI;
using emmVRC.Libraries;

namespace emmVRC.Hacks
{
    public class MasterCrown
    {
        private static GameObject masterIconObj;
        public static Sprite crownSprite;
        public static void Initialize()
        {
            MelonLoader.MelonCoroutines.Start(Loop());
        }
        private static IEnumerator Loop()
        {
            while (true)
            {
                while (Resources.crownSprite == null) yield return null;
                try
                {
                    if (RoomManager.field_Internal_Static_ApiWorld_0 != null && Configuration.JSONConfig.MasterIconEnabled && masterIconObj == null)
                    {
                        if (PlayerManager.field_Private_Static_PlayerManager_0.field_Private_List_1_Player_0.Count > 1)
                            Libraries.PlayerUtils.GetEachPlayer((Player player) =>
                            {
                                if (masterIconObj == null && player.prop_VRCPlayerApi_0.isMaster && player.field_Private_APIUser_0.id != APIUser.CurrentUser.id)
                                {
                                    emmVRCLoader.Logger.LogDebug("Initializing master icon...");
                                    GameObject templateObject = player._vrcplayer.field_Private_Transform_0.parent.transform.Find("Player Nameplate/Canvas/Nameplate/Contents/Friend Marker").gameObject;
                                    emmVRCLoader.Logger.LogDebug("1");
                                    masterIconObj = GameObject.Instantiate(templateObject, templateObject.transform.parent);
                                    emmVRCLoader.Logger.LogDebug("2");
                                    masterIconObj.GetComponent<RectTransform>().anchoredPosition += new Vector2(256f, 24f);
                                    emmVRCLoader.Logger.LogDebug("3");
                                    masterIconObj.GetComponent<Image>().sprite = Resources.crownSprite;
                                    emmVRCLoader.Logger.LogDebug("4");
                                    masterIconObj.SetActive(true);
                                    emmVRCLoader.Logger.LogDebug("Master icon initialized");

                                }
                            });
                        else
                        {
                            GameObject.Destroy(masterIconObj);
                            masterIconObj = null;
                        }
                    }
                    else if (masterIconObj != null && !Configuration.JSONConfig.MasterIconEnabled)
                        GameObject.Destroy(masterIconObj);
                }
                catch (System.Exception ex)
                {
                    ex = new System.Exception();
                }
                yield return new WaitForSeconds(0.25f);
            }
        }
    }
}
