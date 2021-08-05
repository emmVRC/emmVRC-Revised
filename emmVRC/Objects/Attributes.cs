using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace emmVRC.Objects
{
    public class Attributes
    {
        public static string Version = "2.11.3";
        public static int LastTestedBuildNumber = 1121;
        public static string EULAVersion = "1.0.1";
        public static bool Beta = false;
        public static string DateUpdated = "08/05/2021";
        //  "This is a reference for about how long the"
        //  "text can be in the Changelog before it rolls"
        //  "off the page."
        //  "This is an pre-release build! Do not distribute to others."
        public static string Changelog =
            "<b>v2.11.3</b>\n" +
            "• Fixed Custom Loading Music and custom Alarm tones\n" +
            "• Fixed buttons going invisible when selected while using emmVRC\n" +
            "• Fixed the Favorite button favoriting a user's fallback avatar when you have them selected\n" +
            "<b>v2.11.2</b>\n" +
            "• Updated for compatibility with the VRChat 2019 update\n" +
            "<b>v2.11.1</b>\n" +
            "• Global Dynamic Bones should now be working on Oculus\n" +
            "• Added the instance type to Instance History's tooltip\n" +
            "• Fixed the CameraPlus tooltip in emmVRC Settings\n\n" +
            "<b>v2.11.0</b>\n" +
            "• Added <b>nameplate coloring!</b> This uses the same color system used prior to the nameplate update\n" +
            "• Added <b>CameraPlus!</b> This implementation will be released as a standalone mod later as well, but it allows you finer control over the VRChat Camera. Credits for the original mod and code goes to Slaynash\n" +
            "• Added an <b>Alarm Clock</b>, available for both system time and instance time\n" +
            "• The flashlight now has a \"Use\" action to toggle the light on and off\n" +
            "• Fixed the VRHUD issue that has been present for a while now. Special thanks to Loukylor for the fix, along with many more recently!\n";
        public static bool Debug = false;
        public static string TargetMelonLoaderVersion = "0.4.3";
        public static string TargetemmVRCLoaderVersion = "1.2.0";
        public static string[] IncompatibleMelonLoaderVersions = { "0.1.0", "0.2.0", "0.2.1", "0.2.2", "0.2.3", "0.2.4", "0.2.5", "0.2.6", "0.2.7", "0.2.7.1", "0.2.7.2", "0.2.7.3", "0.2.7.4", "0.3.0", "0.4.0" };
        public static string[] IncompatibleemmVRCLoaderVersions = { "0.0.1", "0.0.2", "0.0.3", "0.0.4", "0.0.5", "1.0.0", "1.1.0" };
        public static string[] FlavourTextList = {
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
        public static string VRCPlusMessage = "VRChat, like emmVRC, relies on the support of their users to keep the platform free. Please support VRChat to unlock these features.";
    }
}
