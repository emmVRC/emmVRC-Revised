using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace emmVRC.Objects
{
    public class Attributes
    {
        public static readonly Version Version = new Version(3, 2, 0, 3); 
        public const int LastTestedBuildNumber = 1194;
        public const string EULAVersion = "1.0.1";
        public const bool Beta = true;
        public const string DateUpdated = "5/21/2022";
        //  "This is an pre-release build! Do not distribute to others."
        //  When writing the changelog, please only include changes for the current minor version, unless a new minor version just came out
        public const string Changelog =
            
            "<b>v3.2.0</b>\n" +
            "• Completely new network backend\n" +
            "• Implemented in game menu to reset pin code\n" +
            "• Avatar favorites menu will no longer silently discard errors\n" +
            "• Added back the Alarm Clock, complete with an infinite number of alarms, and a total rework for UI 2.0\n" +
            "• Added back the Clock, now available in the bottom of the Launch Pad of the Quick Menu\n" +
            "• Added back Emoji Favorites, and enhanced it with categories built into the settings menu\n" +
            "• Minor reworks to the versioning and changelog system\n" +
            "• Removed Unlimited FPS, as the majority of its' function is now in VRChat itself\n" +
            "• Removed pointless code regarding ranks that are no longer in the game\n\n" +
            "<b>v3.1.6</b>\n" +
            "• Fixed third person causing local avatars to experience Z-fighting in certain situations\n" +
            "• Fixed microphone icon coloring not functioning\n" +
            "• Fixed harmless menu errors\n" +
            "• Removed extraneous code to improve performance and reliability\n\n" +
            "<b>v3.1.5</b>\n" +
            "• Favorites & search will no longer always show an error bot.\n" +
            "• Menu recoloring should no longer throw an error on startup. (Some things may look wrong! let us know!)\n\n" +
            "<b>v3.1.4</b>\n" +
            "• Attempted to make login text more clear about how to make a pin and set up the emmVRC Network\n" +
            "• Fixed issues with Oculus, including network login and Risky Functions\n\n" +
            "<b><color=#FF5555>Notice</color></b>\n" +
            "The following features are <i>known</i> to be broken or unavailable, and will be available in a later update:\n" +
            "- Keybind configuration via the UI\n";
        public static bool Debug = false;
        public static readonly Version MinimumMelonLoaderVersion = new Version(0, 5, 4);
        public static readonly Version MinimumemmVRCLoaderVersion = new Version(1, 2, 0);
        //public static readonly string[] FlavourTextList = {
        //            "Did you know that Tab is also a drink?",
        //            "Going strong for 2 years!",
        //            "Visual Studio makes me want to scream.",
        //            "\"I'll believe in climate change when Texas\nfreezes over.\"",
        //            "It just works™!",
        //            "Sample Text",
        //            "ǅǅǅǄǄǄǅǅǄǅǅǅ",
        //            "Object reference not set to an instance of\nan object",
        //            "\"You spin my head right round, right round,\nlike a record baby\"",
        //            "<b>BOAT STUCK! <i>BOAT STUCK!</i></b>",
        //            ""
        //};
        public const string VRCPlusMessage = "VRChat, like emmVRC, relies on the support of their users to keep the platform free. Please support VRChat to unlock these features.";
    }
}
