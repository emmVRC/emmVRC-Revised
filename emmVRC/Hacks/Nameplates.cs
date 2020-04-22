using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace emmVRC.Hacks
{
    public class Nameplates
    {
        public static bool colorChanged = true;
        private static Thread NameplateColorThread;
        public static void Initialize()
        {
            NameplateColorThread = new Thread(Loop)
            {
                Name = "emmVRC Nameplate Coloring Thread",
                IsBackground = true
            };
            NameplateColorThread.Start();
        }
        private static void Loop()
        {
            while (true)
            {
                if (colorChanged && Configuration.JSONConfig.NameplateColorChangingEnabled)
                {
                    VRCPlayer.field_Color_1 = Libraries.ColorConversion.HexToColor(Configuration.JSONConfig.FriendNamePlateColorHex);
                    VRCPlayer.field_Color_2 = Libraries.ColorConversion.HexToColor(Configuration.JSONConfig.VisitorNamePlateColorHex);
                    VRCPlayer.field_Color_3 = Libraries.ColorConversion.HexToColor(Configuration.JSONConfig.NewUserNamePlateColorHex);
                    VRCPlayer.field_Color_4 = Libraries.ColorConversion.HexToColor(Configuration.JSONConfig.UserNamePlateColorHex);
                    VRCPlayer.field_Color_5 = Libraries.ColorConversion.HexToColor(Configuration.JSONConfig.KnownUserNamePlateColorHex);
                    VRCPlayer.field_Color_6 = Libraries.ColorConversion.HexToColor(Configuration.JSONConfig.TrustedUserNamePlateColorHex);
                    colorChanged = false;
                }
                else if (colorChanged && !Configuration.JSONConfig.NameplateColorChangingEnabled)
                {
                    VRCPlayer.field_Color_1 = Libraries.ColorConversion.HexToColor("#FFFF00");
                    VRCPlayer.field_Color_2 = Libraries.ColorConversion.HexToColor("#CCCCCC");
                    VRCPlayer.field_Color_3 = Libraries.ColorConversion.HexToColor("#1778FF");
                    VRCPlayer.field_Color_4 = Libraries.ColorConversion.HexToColor("#2BCE5C");
                    VRCPlayer.field_Color_5 = Libraries.ColorConversion.HexToColor("#FF7B42");
                    VRCPlayer.field_Color_6 = Libraries.ColorConversion.HexToColor("#8143E6");
                    colorChanged = false;
                }
            }
        }
    }
}
