using Il2CppSystem;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using VRC;
using VRC.Core;
using UnityEngine;
using UnityEngine.UI;

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
                if (RoomManager.field_ApiWorld_0 != null && Configuration.JSONConfig.MasterIconEnabled && masterIconObj == null)
                {
                    if (PlayerManager.field_PlayerManager_0.field_List_1_Player_0.Count > 1)
                        Libraries.PlayerUtils.GetEachPlayer((Player player) =>
                        {
                            if (masterIconObj == null && player.prop_VRCPlayerApi_0.isMaster && player.field_APIUser_0.id != APIUser.CurrentUser.id)
                            {
                                masterIconObj = GameObject.Instantiate(player.field_VRCPlayer_0.friendSprite.gameObject, player.field_VRCPlayer_0.friendSprite.transform.parent);
                                masterIconObj.GetComponent<RectTransform>().anchoredPosition += new Vector2(768f, 0f);
                                masterIconObj.GetComponent<Image>().sprite = Resources.crownSprite;

                            }
                            else
                            {
                                if (Configuration.JSONConfig.NameplatesVisible)
                                    masterIconObj.SetActive(true);
                                else
                                    masterIconObj.SetActive(false);
                            }
                        });
                    else
                    {
                        GameObject.Destroy(masterIconObj);
                        masterIconObj = null;
                    }
                }
                yield return new WaitForSeconds(0.25f);
            }
        }
    }
}
