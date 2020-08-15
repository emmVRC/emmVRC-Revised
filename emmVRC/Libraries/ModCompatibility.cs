using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace emmVRC.Libraries
{
    public class ModCompatibility
    {

        public static bool MultiplayerDynamicBones = false;
        public static bool PortalConfirmation = false;
        public static bool MControl = false;
        public static bool CleanConsole = false;
        public static bool OGTrustRank = false;

        public static void Initialize()
        {
            if (MelonLoader.MelonHandler.Mods.FindIndex(i => i.Info.Name == "MultiplayerDynamicBones") != -1)
                MultiplayerDynamicBones = true;
            if (MelonLoader.MelonHandler.Mods.FindIndex(i => i.Info.Name == "Portal Confirmation") != -1)
                PortalConfirmation = true;
            if (MelonLoader.MelonHandler.Mods.FindIndex(i => i.Info.Name == "MControl") != -1)
                MControl = true;
            if (MelonLoader.MelonHandler.Mods.FindIndex(i => i.Info.Name == "CleanConsole") != -1)
                CleanConsole = true;
            if (MelonLoader.MelonHandler.Mods.FindIndex(i => i.Info.Name == "OGTrustRanks") != -1)
                OGTrustRank = true;

            if (MultiplayerDynamicBones)
                emmVRCLoader.Logger.LogDebug("Detected MultiplayerDynamicBones");
            if (PortalConfirmation)
                emmVRCLoader.Logger.LogDebug("Detected PortalConfirmation");
            if (MControl)
                emmVRCLoader.Logger.LogDebug("Detected MControl");
            if (CleanConsole)
                emmVRCLoader.Logger.LogDebug("Detected CleanConsole");
            if (OGTrustRank)
                emmVRCLoader.Logger.LogDebug("Detected OGTrustRank");
        }
    }
}
