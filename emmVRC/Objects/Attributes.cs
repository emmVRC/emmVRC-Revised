using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace emmVRC.Objects
{
    public class Attributes
    {
        public static string Version = "2.8.0";
        public static int LastTestedBuildNumber = 1067;
        public static string EULAVersion = "1.0.1";
        public static bool Beta = false;
        public static string DateUpdated = "03/30/2021";
        //  "This is a reference for about how long the"
        //  "text can be in the Changelog before it rolls"
        //  "off the page."
        //  "This is an pre-release build! Do not distribute to others."
        public static string Changelog =
            "• Added sorting to the emmVRC Favorite list, as well as the\n" +
            "avatar search list!\n"+
            "• Added Pickup, Pickup object, and Video Player (SDK2) toggles\n" +
            "• Added Item and Trigger ESP\n" +
            "• Added Player FPS counter to Player List HUD\n" +
            "• Changed Avatar Pedestal buttons into a single toggle button\n" +
            "• Fixed Name Spoofing not being applied to your nameplate\n" +
            "• Fixed the avatar favorites system not properly unloading\n" +
            "when the menu is closed\n";
        public static bool Debug = false;
        public static string TargetMelonLoaderVersion = "0.3.0";
        public static string TargetemmVRCLoaderVersion = "1.1.0";
        public static string[] IncompatibleMelonLoaderVersions = { "0.1.0", "0.2.0", "0.2.1", "0.2.2", "0.2.3", "0.2.4", "0.2.5", "0.2.6", "0.2.7", "0.2.7.1", "0.2.7.2", "0.2.7.3", "0.2.7.4" };
        public static string[] IncompatibleemmVRCLoaderVersions = { "0.0.1", "0.0.2", "0.0.3", "0.0.4", "0.0.5", "1.0.0" };
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
                    "<b>BOAT STUCK! <i>BOAT STUCK!</i></b>"
        };

        private const string FlavourText = "Love you just the way you are. A cinema... my cinema. ~Emilia";
        //public static string ValidLoaderHash { get { return "8o1t7DrcDNmUGIMmFhOYWw=="; } }
    }
}
