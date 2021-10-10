using emmVRC.Libraries;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
namespace emmVRC.Hacks
{
    public class Nameplates
    {
        public static bool colorChanged = true;
        public static void Initialize()
        {
            MelonLoader.MelonCoroutines.Start(Loop());
        }
        private static IEnumerator Loop()
        {
            while (true)
            {
                if (colorChanged && Configuration.JSONConfig.NameplateColorChangingEnabled)
                {
                    VRCPlayer.field_Internal_Static_Color_1 = Libraries.ColorConversion.HexToColor(Configuration.JSONConfig.FriendNamePlateColorHex);
                    VRCPlayer.field_Internal_Static_Color_2 = Libraries.ColorConversion.HexToColor(Configuration.JSONConfig.VisitorNamePlateColorHex);
                    VRCPlayer.field_Internal_Static_Color_3 = Libraries.ColorConversion.HexToColor(Configuration.JSONConfig.NewUserNamePlateColorHex);
                    VRCPlayer.field_Internal_Static_Color_4 = Libraries.ColorConversion.HexToColor(Configuration.JSONConfig.UserNamePlateColorHex);
                    VRCPlayer.field_Internal_Static_Color_5 = Libraries.ColorConversion.HexToColor(Configuration.JSONConfig.KnownUserNamePlateColorHex);
                    VRCPlayer.field_Internal_Static_Color_6 = Libraries.ColorConversion.HexToColor(Configuration.JSONConfig.TrustedUserNamePlateColorHex);
                    if (Functions.Core.ModCompatibility.OGTrustRank)
                    {
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
                    colorChanged = false;
                }
                else if (colorChanged && !Configuration.JSONConfig.NameplateColorChangingEnabled)
                {
                    VRCPlayer.field_Internal_Static_Color_1 = Libraries.ColorConversion.HexToColor("#FFFF00");
                    VRCPlayer.field_Internal_Static_Color_2 = Libraries.ColorConversion.HexToColor("#CCCCCC");
                    VRCPlayer.field_Internal_Static_Color_3 = Libraries.ColorConversion.HexToColor("#1778FF");
                    VRCPlayer.field_Internal_Static_Color_4 = Libraries.ColorConversion.HexToColor("#2BCE5C");
                    VRCPlayer.field_Internal_Static_Color_5 = Libraries.ColorConversion.HexToColor("#FF7B42");
                    VRCPlayer.field_Internal_Static_Color_6 = Libraries.ColorConversion.HexToColor("#8143E6");
                    colorChanged = false;
                }
                yield return new WaitForSeconds(0.5f);
            }
        }
    }
}
