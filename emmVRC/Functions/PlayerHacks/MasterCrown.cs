using VRC;
using VRC.Core;
using UnityEngine;
using UnityEngine.UI;
using emmVRC.Objects.ModuleBases;

namespace emmVRC.Hacks
{
    public class MasterCrown : MelonLoaderEvents
    {
        private static GameObject masterIconObj;
        public static Sprite crownSprite;
        public override void OnUiManagerInit()
        {
            Utils.NetworkEvents.OnPlayerJoined += (Player plr) => {
                if (plr.prop_APIUser_0 != null && plr.prop_APIUser_0.id != APIUser.CurrentUser.id && plr.prop_VRCPlayerApi_0 != null && plr.prop_VRCPlayerApi_0.isMaster && Configuration.JSONConfig.MasterIconEnabled)
                    InstantiateIcon(plr);
            };
            Utils.NetworkEvents.OnPlayerLeft += (Player plr) =>
            {
                Libraries.PlayerUtils.GetEachPlayer((Player player) =>
                {
                    if (player.prop_VRCPlayerApi_0.isMaster && player.prop_APIUser_0.id != APIUser.CurrentUser.id && Configuration.JSONConfig.MasterIconEnabled)
                    {
                        InstantiateIcon(player);
                    }
                });
            };
            Configuration.onConfigUpdated.Add(new System.Collections.Generic.KeyValuePair<string, System.Action>("MasterIconEnabled", () => {
                Libraries.PlayerUtils.GetEachPlayer((Player player) =>
                {
                    if (player.prop_VRCPlayerApi_0.isMaster && player.prop_APIUser_0.id != APIUser.CurrentUser.id && Configuration.JSONConfig.MasterIconEnabled)
                    {
                        InstantiateIcon(player);
                    }
                });
                if (!Configuration.JSONConfig.MasterIconEnabled && masterIconObj != null)
                    GameObject.DestroyImmediate(masterIconObj);
            }));
        }
        public static void InstantiateIcon(Player plr)
        {
            if (masterIconObj != null)
                GameObject.DestroyImmediate(masterIconObj);
            GameObject templateObject = plr._vrcplayer.field_Private_Transform_0.parent.transform.Find("Player Nameplate/Canvas/Nameplate/Contents/Friend Marker").gameObject;
            masterIconObj = GameObject.Instantiate(templateObject, templateObject.transform.parent);
            masterIconObj.GetComponent<RectTransform>().anchoredPosition += new Vector2(256f, 24f);
            masterIconObj.GetComponent<Image>().sprite = Functions.Core.Resources.crownSprite;
            masterIconObj.SetActive(true);
        }
    }
}
