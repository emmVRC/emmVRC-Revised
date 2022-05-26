using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace emmVRC.Objects
{
    public class Attributes
    {
        public static readonly Version Version = new Version(3, 2, 0, 0);
        public const int LastTestedBuildNumber = 1201;
        public const string EULAVersion = "1.0.1";
        public const bool Beta = false;
        public const string DateUpdated = "5/26/2022";
        //  "This is an pre-release build! Do not distribute to others."
        //  When writing the changelog, please only include changes for the current minor version, unless a new minor version just came out
        public const string Changelog =

            "<b>v3.2.0</b>\n" +
            "• Added back the Alarm Clock, complete with an infinite number of alarms, and a total rework for UI 2.0\n" +
            "• Added back the Clock, now available in the bottom of the Launch Pad of the Quick Menu\n" +
            "• Added back Emoji Favorites, and enhanced it with categories built into the settings menu\n" +
            "• Added back keybind reconfiguration in-game\n" +
            "• Added desktop zoom that functions similar to Source games. Use middle mouse to zoom in and out\n" +
            "• Added a Wing menu for quick access to Player Tweaks\n" +
            "• Started work on localization for various strings of emmVRC, to aid in easier understanding of important messages\n" +
            "  -  German localization is present, thanks to Bluscream\n" +
            "  -  French localization is present, thanks to Lupsoris\n" +
            "• Minor reworks to the versioning and changelog system\n" +
            "• Minor reworks to UI color changing\n" +
            "• Removed Unlimited FPS, as the majority of its' function is now in VRChat itself\n" +
            "• Removed pointless code regarding ranks that are no longer in the game\n" +
            "• Removed FBT Calibration Saving, as this is now a part of VRChat\n" +
            "• Removed social list refresh, as it is no longer necessary\n" +
            "• Updated for compatibility with VRChat build 1201\n\n" +
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
            "• Fixed issues with Oculus, including network login and Risky Functions\n\n";
        public static bool Debug = false;
        public static readonly Version MinimumMelonLoaderVersion = new Version(0, 5, 4);
        public static readonly Version MinimumemmVRCLoaderVersion = new Version(1, 5, 0);
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
    }
}