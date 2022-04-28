using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace emmVRC.Objects
{
    public class Attributes
    {
        public static readonly Version Version = new Version(3, 1, 6, 0); 
        public const int LastTestedBuildNumber = 1189;
        public const string EULAVersion = "1.0.1";
        public const bool Beta = false;
        public const string DateUpdated = "4/21/2022";
        //  "This is an pre-release build! Do not distribute to others."
        //  When writing the changelog, please only include changes for the current minor version, unless a new minor version just came out
        public const string Changelog =
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
            "<b>v3.1.3</b>\n" +
            "• Compatibility with VRChat build 1169\n\n" +
            "<b>v3.1.2</b>\n" +
            "• Removed Emoji Favorites from the action menu temporarily\n" +
            "• Cleaned up and removed unused features\n" +
            "• Fixed a bug where the Programs module would generate an invalid configuration\n" +
            "• Risky Functions are now allowed for world creators, to assist in development. Note that this will <b>not</b> override the existence of the off/on objects\n" +
            "• Desktop reticle now disappears when in third person\n\n" +
            "<b>v3.1.1</b>\n" +
            "• Fixed avatar favorites showing error avatars and not allowing use\n\n" +
            "<b>v3.1.0</b>\n" +
            "• Implemented Avatar Options! This is where per-avatar settings such as shader and dynamic bone toggling will be from now on, as well as Global Dynamic Bone configuration\n" +
            "• Added light toggles to Avatar Options! This allows you to easily toggle off lights on individual avatars\n" +
            "• Implemented Jump toggling once again! This also allows you to disable jumping after having enabled it in a world\n"+
            "• Keyboard shortcuts for respawning and going home now work without having the Quick Menu open\n" +
            "• Fixed bans from the emmVRC Network not showing up correctly\n" +
            "• Fixed World Volume mute not saving\n" +
            "• Updated for VRChat build 1160, now with more reflection to prevent breaking\n" +
            "• Fixed Desktop and VR flight not working in SDK3 worlds\n\n" +
            "<b><color=#FF5555>Notice</color></b>\n" +
            "The following features are <i>known</i> to be broken or unavailable, and will be available in a later update:\n" +
            "- Alarm Clocks\n" +
            "- Emoji Favorites\n" +
            "- Keybind configuration via the UI\n" +
            "\n" +
            "The following features have been removed, and will <i>not</i> be returning to emmVRC:\n" +
            "- Info spoofing\n" +
            "- Stealth mode\n" +
            "";
        public static bool Debug = false;
        public static readonly Version MinimumMelonLoaderVersion = new Version(0, 5, 2);
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
