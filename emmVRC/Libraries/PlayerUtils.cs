using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRC;
using VRC.SDKBase;
using TMPro;

namespace emmVRC.Libraries
{
    public class PlayerUtils
    {
        //private static Il2CppSystem.Action<Player> getAllPlayersCache;
        private static System.Action<Player> requestedAction;
        public static void GetEachPlayer(System.Action<Player> act)
        {

            requestedAction = act;
            foreach (Player plr in PlayerManager.field_Private_Static_PlayerManager_0.field_Private_List_1_Player_0)
            {
                requestedAction.Invoke(plr);
            }
            //PlayerManager.field_Private_Static_PlayerManager_0.field_Private_List_1_Player_0.ForEach(getAllPlayersCache);
        }
    }

	#region Korty's Addons
	public static class PlayerReflect
	{
		public static VRCPlayerApi GetVRCPlayerApi(this Player player) { return player.prop_VRCPlayerApi_0; }

		public static VRCPlayer GetLocalVRCPlayer() { return VRCPlayer.field_Internal_Static_VRCPlayer_0; }

		public static VRCPlayer GetVRCPlayer(this Player player) { return player.field_Internal_VRCPlayer_0; }

		public static bool GetIsInVR(this Player Instance) { return Instance.GetVRCPlayerApi().IsUserInVR(); }

		public static bool GetIsInVR(this VRCPlayer Instance) { return Instance.field_Private_VRCPlayerApi_0.IsUserInVR(); }

		public static bool GetIsInVR(this PlayerNet Instance) { return Instance.field_Internal_VRCPlayer_0.field_Private_VRCPlayerApi_0.IsUserInVR(); }

		public static int GetFrames(this VRCPlayer Instance)
		{
			if (Instance.field_Private_PlayerNet_0.field_Private_Byte_0 == 0)
			{
				return 0;
			}
			return (int)(1000f / (float)Instance.field_Private_PlayerNet_0.field_Private_Byte_0);
		}

		public static string GetFramesColored(this VRCPlayer Instance)
		{
			string arg;
			if (Instance.GetFrames() >= 80)
			{
				arg = "<color=#33ff47>";
			}
			else if (Instance.GetFrames() <= 80 && Instance.GetFrames() >= 30)
			{
				arg = "<color=#ff8936>";
			}
			else
			{
				arg = "<color=red>";
			}
			return string.Format("{0}{1}</color>", arg, Instance.GetFrames());
		}

		public static TextMeshProUGUI GetSelfNameplateText() { return GetLocalVRCPlayer().field_Public_PlayerNameplate_0.gameObject.transform.Find("Contents/Main/Text Container/Name").GetComponent<TextMeshProUGUI>(); }
	}
	#endregion
}
