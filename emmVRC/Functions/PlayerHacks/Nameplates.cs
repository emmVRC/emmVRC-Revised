using emmVRC.Libraries;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using emmVRC.Objects.ModuleBases;

namespace emmVRC.Functions.PlayerHacks
{
    public class Nameplates : MelonLoaderEvents
    {

        public override void OnUiManagerInit()
        {
            Configuration.onConfigUpdated.Add(new System.Collections.Generic.KeyValuePair<string, Action>("NameplateColorChangingEnabled", Utils.PlayerUtils.ReloadAllAvatars));
            Configuration.onConfigUpdated.Add(new System.Collections.Generic.KeyValuePair<string, Action>("FriendNamePlateColorHex", Utils.PlayerUtils.ReloadAllAvatars));
            Configuration.onConfigUpdated.Add(new System.Collections.Generic.KeyValuePair<string, Action>("VisitorNamePlateColorHex", Utils.PlayerUtils.ReloadAllAvatars));
            Configuration.onConfigUpdated.Add(new System.Collections.Generic.KeyValuePair<string, Action>("NewUserNamePlateColorHex", Utils.PlayerUtils.ReloadAllAvatars));
            Configuration.onConfigUpdated.Add(new System.Collections.Generic.KeyValuePair<string, Action>("UserNamePlateColorHex", Utils.PlayerUtils.ReloadAllAvatars));
            Configuration.onConfigUpdated.Add(new System.Collections.Generic.KeyValuePair<string, Action>("KnownUserNamePlateColorHex", Utils.PlayerUtils.ReloadAllAvatars));
            Configuration.onConfigUpdated.Add(new System.Collections.Generic.KeyValuePair<string, Action>("TrustedUserNamePlateColorHex", Utils.PlayerUtils.ReloadAllAvatars));
            Configuration.onConfigUpdated.Add(new System.Collections.Generic.KeyValuePair<string, Action>("VeteranUserNamePlateColorHex", Utils.PlayerUtils.ReloadAllAvatars));
            Configuration.onConfigUpdated.Add(new System.Collections.Generic.KeyValuePair<string, Action>("LegendaryUserNamePlateColorHex", Utils.PlayerUtils.ReloadAllAvatars));
            Utils.NetworkEvents.OnPlayerJoined += (plr) =>
            {
                if (Functions.Core.ModCompatibility.OGTrustRank)
                {
                    if (Configuration.JSONConfig.NameplateColorChangingEnabled)
                    {

                        VRCPlayer.field_Internal_Static_Color_1 = Libraries.ColorConversion.HexToColor(Configuration.JSONConfig.FriendNamePlateColorHex);
                        VRCPlayer.field_Internal_Static_Color_2 = Libraries.ColorConversion.HexToColor(Configuration.JSONConfig.VisitorNamePlateColorHex);
                        VRCPlayer.field_Internal_Static_Color_3 = Libraries.ColorConversion.HexToColor(Configuration.JSONConfig.NewUserNamePlateColorHex);
                        VRCPlayer.field_Internal_Static_Color_4 = Libraries.ColorConversion.HexToColor(Configuration.JSONConfig.UserNamePlateColorHex);
                        VRCPlayer.field_Internal_Static_Color_5 = Libraries.ColorConversion.HexToColor(Configuration.JSONConfig.KnownUserNamePlateColorHex);
                        VRCPlayer.field_Internal_Static_Color_6 = Libraries.ColorConversion.HexToColor(Configuration.JSONConfig.TrustedUserNamePlateColorHex);

                        try
                        {
                            MelonLoader.MelonHandler.Mods.First(i => i.Info.Name == "OGTrustRanks").Assembly.GetType("OGTrustRanks.OGTrustRanks").GetField("TrustedUserColor", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static).SetValue(null, Libraries.ColorConversion.HexToColor(Configuration.JSONConfig.TrustedUserNamePlateColorHex));
                            MelonLoader.MelonHandler.Mods.First(i => i.Info.Name == "OGTrustRanks").Assembly.GetType("OGTrustRanks.OGTrustRanks").GetField("VeteranUserColor", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static).SetValue(null, Libraries.ColorConversion.HexToColor(Configuration.JSONConfig.VeteranUserNamePlateColorHex));
                            MelonLoader.MelonHandler.Mods.First(i => i.Info.Name == "OGTrustRanks").Assembly.GetType("OGTrustRanks.OGTrustRanks").GetField("LegendaryUserColor", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static).SetValue(null, Libraries.ColorConversion.HexToColor(Configuration.JSONConfig.LegendaryUserNamePlateColorHex));
                        }
                        catch (Exception ex)
                        {
                            emmVRCLoader.Logger.LogError(ex.ToString());
                        }
                    }
                }
                else if (!Configuration.JSONConfig.NameplateColorChangingEnabled)
                {
                    VRCPlayer.field_Internal_Static_Color_1 = Libraries.ColorConversion.HexToColor("#FFFF00");
                    VRCPlayer.field_Internal_Static_Color_2 = Libraries.ColorConversion.HexToColor("#CCCCCC");
                    VRCPlayer.field_Internal_Static_Color_3 = Libraries.ColorConversion.HexToColor("#1778FF");
                    VRCPlayer.field_Internal_Static_Color_4 = Libraries.ColorConversion.HexToColor("#2BCE5C");
                    VRCPlayer.field_Internal_Static_Color_5 = Libraries.ColorConversion.HexToColor("#FF7B42");
                    VRCPlayer.field_Internal_Static_Color_6 = Libraries.ColorConversion.HexToColor("#8143E6");
                }
            };

        }
    }
}

