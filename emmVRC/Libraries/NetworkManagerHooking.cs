using emmVRC.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRC;
using VRC.Core;

namespace emmVRC.Libraries
{
    public class NetworkManagerHooking
    {
        public static void OnPlayerJoined(Player plr)
        {
           
            if (Configuration.JSONConfig.PlayerHistoryEnable && plr.field_Private_APIUser_0 != null && plr.field_Private_APIUser_0.id != APIUser.CurrentUser.id)
            {
                PlayerHistoryMenu.currentPlayersNames.Add(plr.field_Private_APIUser_0.displayName);
                emmVRCLoader.Logger.Log("Player joined: " + plr.field_Private_APIUser_0.displayName);
            }
        }
        public static void OnPlayerLeft(Player plr)
        {
            if (Configuration.JSONConfig.PlayerHistoryEnable)
            {
                emmVRCLoader.Logger.Log("Player left: " + plr.field_Private_APIUser_0.displayName);
            }
        }
    }
}
