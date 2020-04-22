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
        public static Thread MasterCrownThread;
        private static GameObject masterIconObj;
        public static void Initialize()
        {
            MelonLoader.MelonCoroutines.Start(Loop());
        }
        private static IEnumerator Loop()
        {
            while (true)
            {
                try
                {
                    if (RoomManager.field_ApiWorld_0 != null && Configuration.JSONConfig.MasterIconEnabled && Resources.crownSprite != null)
                    {
                        if (PlayerManager.Method_Public_10().Count > 1)
                            foreach (Player player in PlayerManager.Method_Public_10().ToArray())
                            {
                                if (masterIconObj == null && player.prop_VRCPlayerApi_0.isMaster && player.field_APIUser_0.id != APIUser.CurrentUser.id)
                                {
                                    masterIconObj = GameObject.Instantiate(player.field_VRCPlayer_0.friendSprite.gameObject, player.field_VRCPlayer_0.friendSprite.transform.parent);
                                    masterIconObj.GetComponent<RectTransform>().anchoredPosition += new Vector2(768f, 0f);
                                    masterIconObj.GetComponent<Image>().sprite = Resources.crownSprite;
                                    if (Configuration.JSONConfig.NameplatesVisible)
                                        masterIconObj.SetActive(true);
                                    else
                                        masterIconObj.SetActive(false);
                                }
                            }
                    }
                }
                catch (System.Exception ex)
                {
                    emmVRCLoader.Logger.LogError("[emmVRC] " + ex.ToString());
                }
                yield return new WaitForSeconds(0.25f);
            }
        }
    }
}
