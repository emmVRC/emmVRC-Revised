using NET_SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRC;

namespace emmVRC.Libraries
{
    public class PlayerUtils
    {
        private static Il2CppSystem.Action<Player> getAllPlayersCache;
        private static System.Action<Player> requestedAction;
        public static void GetEachPlayer(System.Action<Player> act)
        {
            if (getAllPlayersCache == null)
                getAllPlayersCache = UnhollowerRuntimeLib.DelegateSupport.ConvertDelegate<Il2CppSystem.Action<Player>>((System.Action<Player>)((Player plr) => {
                    requestedAction.Invoke(plr);
                }));
            requestedAction = act;
            PlayerManager.field_PlayerManager_0.field_List_1_Player_0.ForEach(getAllPlayersCache);
        }
    }
}
