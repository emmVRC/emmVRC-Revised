using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using emmVRC.Objects;
using emmVRC.Objects.ModuleBases;
using VRC.Core;

namespace emmVRC.Functions.PlayerHacks
{
    public class PlayerHistory : MelonLoaderEvents
    {
        public static List<InstancePlayer> currentPlayers;

        public override void OnUiManagerInit()
        {
            currentPlayers = new List<InstancePlayer>();

            Utils.NetworkEvents.OnPlayerJoined += (VRC.Player plr) =>
            {
                if (Configuration.JSONConfig.PlayerHistoryEnable && plr.prop_APIUser_0 != null && plr.prop_APIUser_0.id != APIUser.CurrentUser.id)
                    currentPlayers.Add(new InstancePlayer { Name = plr.prop_APIUser_0.GetName(), UserID = plr.prop_APIUser_0.id, TimeJoinedStamp = DateTime.Now.ToShortTimeString() });
            };
            Utils.NetworkEvents.OnLocalPlayerJoined += (VRC.Player plr) => {
                currentPlayers.Clear();
            };
        }
    }
}
