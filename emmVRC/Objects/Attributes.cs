using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace emmVRC.Objects
{
    public class Attributes
    {
        public const string Version = "3.0.0";
        public const int LastTestedBuildNumber = 1149;
        public const string EULAVersion = "1.0.1";
        public const bool Beta = false;
        public const string DateUpdated = "10/08/2021";
        //  "This is a reference for about how long the"
        //  "text can be in the Changelog before it rolls"
        //  "off the page."
        //  "This is an pre-release build! Do not distribute to others."
        public const string Changelog =
            "<b>v3.0.0</b>\n" +
            "• Interaction with pickup triggers (e.g. flashlights) now works like the vanilla game\n" +
            "• emmVRC's menus have been completely remade for the new VRChat UI\n" +
            "• Added more exclusions and inclusions to the color module\n" +
            "• Several modules of emmVRC have been rewritten for better performance and expandability\n" +
            "• Errors in specific modules of emmVRC will no longer cause the rest of the mod to stop loading or working\n" +
            "\n" +
            "<b><color=#FF5555>Notice</color></b>\n" +
            "The following features are <i>known</i> to be broken or unavailable, and will be available in a later update:\n" +
            "- Alarm Clocks\n" +
            "- Keybind configuration via the UI\n" +
            "- UIExpansionKit coloring\n" +
            "\n" +
            "The following features have been removed, and will <i>not</i> be returning to emmVRC:\n" +
            "- Info spoofing\n" +
            "- Stealth mode\n" +
            "";
        public static bool Debug = false;
        public const string TargetMelonLoaderVersion = "0.4.3";
        public const string TargetemmVRCLoaderVersion = "1.2.0";
        public static readonly string[] IncompatibleMelonLoaderVersions = { "0.1.0", "0.2.0", "0.2.1", "0.2.2", "0.2.3", "0.2.4", "0.2.5", "0.2.6", "0.2.7", "0.2.7.1", "0.2.7.2", "0.2.7.3", "0.2.7.4", "0.3.0", "0.4.0" };
        public static readonly string[] IncompatibleemmVRCLoaderVersions = { "0.0.1", "0.0.2", "0.0.3", "0.0.4", "0.0.5", "1.0.0", "1.1.0" };
        public static readonly string[] FlavourTextList = {
                    "Did you know that Tab is also a drink?",
                    "Going strong for almost 2 years!",
                    "Visual Studio makes me want to scream.",
                    "\"I'll believe in climate change when Texas\nfreezes over.\"",
                    "It just works™!",
                    "Sample Text",
                    "ǅǅǅǄǄǄǅǅǄǅǅǅ",
                    "Object reference not set to an instance of\nan object",
                    "\"You spin my head right round, right round,\nlike a record baby\"",
                    "<b>BOAT STUCK! <i>BOAT STUCK!</i></b>",
                    ""
        };
        public const string VRCPlusMessage = "VRChat, like emmVRC, relies on the support of their users to keep the platform free. Please support VRChat to unlock these features.";
    }
}
