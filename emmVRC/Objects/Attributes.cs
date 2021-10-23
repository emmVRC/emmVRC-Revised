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
        public const int LastTestedBuildNumber = 1121;
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
            "• UIExpansionKit coloring will no longer affect checkboxes\n" +
            "• Added more exclusions and inclusions to the color module\n" +
            "• Mods can now opt out of emmVRC coloring for buttons by using a \"emmVRCDoNotColor\" GameObject within the button\n" +
            "• The original Functions menu button has been removed in preparation for newer UI designs of VRChat\n" +
            "• Info spoofing has been removed, as it was too hard to maintain and its' purpose is largely provided by Streamer mode\n" +
            "• Several modules of emmVRC have been rewritten for better performance and expandability\n" +
            "• Errors in specific modules of emmVRC will no longer cause the rest of the mod to stop loading or working";
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
