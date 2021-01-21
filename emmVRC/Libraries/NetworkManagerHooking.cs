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
        public static void OnPlayerJoined(VRC.Player plr)
        {
           
            if (Configuration.JSONConfig.PlayerHistoryEnable && plr.field_Private_APIUser_0 != null && plr.field_Private_APIUser_0.id != APIUser.CurrentUser.id)
            {
                PlayerHistoryMenu.currentPlayers.Add(new InstancePlayer { Name = plr.field_Private_APIUser_0.GetName(), UserID = plr.field_Private_APIUser_0.id, TimeJoinedStamp = DateTime.Now.ToShortTimeString() });
                
            }
            if (Configuration.JSONConfig.LogPlayerJoin)
                emmVRCLoader.Logger.Log("Player joined: " + plr.field_Private_APIUser_0.GetName());
        }
        public static void OnPlayerLeft(VRC.Player plr)
        {
            if (Configuration.JSONConfig.PlayerHistoryEnable)
            {
                
            }
            if (Configuration.JSONConfig.LogPlayerJoin)
                emmVRCLoader.Logger.Log("Player left: " + plr.field_Private_APIUser_0.GetName());
        }
    }
}
