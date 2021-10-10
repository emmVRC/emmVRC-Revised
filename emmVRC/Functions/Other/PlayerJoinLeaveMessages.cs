using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using emmVRC.Objects.ModuleBases;

namespace emmVRC.Functions.Other
{
    public class PlayerJoinLeaveMessages : MelonLoaderEvents
    {
        public override void OnUiManagerInit()
        {
            Utils.NetworkEvents.OnPlayerJoined += (VRC.Player plr) =>
            {
                if (Configuration.JSONConfig.LogPlayerJoin)
                    emmVRCLoader.Logger.Log("Player joined: " + plr.prop_APIUser_0.GetName());
            };
            Utils.NetworkEvents.OnPlayerLeft += (VRC.Player plr) =>
            {
                if (Configuration.JSONConfig.LogPlayerJoin)
                    emmVRCLoader.Logger.Log("Player left: " + plr.prop_APIUser_0.GetName());
            };
        }
    }
}
