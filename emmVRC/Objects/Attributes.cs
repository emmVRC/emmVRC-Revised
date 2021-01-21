using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace emmVRC.Objects
{
    public class Attributes
    {
        public static string Version = "2.6.1";
        public static int LastTestedBuildNumber = 1030;
        public static string EULAVersion = "1.0.1";
        public static bool Beta = false;
        public static string DateUpdated = "01/20/2021";
        //  "This is a reference for about how long the"
        //  "text can be in the Changelog before it rolls"
        //  "off the page."
        //  "This is an pre-release build! Do not distribute to others."
        public static string Changelog =
            "• The emmVRC Custom Avatar Favorites now has pages!\n" +
            "• To take advantage of the new optimizations, the\n" +
            "avatar favorite cap has been removed!\n" +
            "• Added an option to disable the \"My Creations\"\n" +
            "avatar category\n" +
            "• The emmVRC EULA has been updated\n" +
            "\nPlus several bug fixes. Please see the Discord for the full\n" +
            "changelog!";
        public static bool Debug = false;
        public static string TargetMelonLoaderVersion = "0.2.7.3";
        public static string TargetemmVRCLoaderVersion = "1.0.0";
        public static string[] IncompatibleMelonLoaderVersions = { "0.1.0", "0.2.0", "0.2.1", "0.2.2", "0.2.3", "0.2.4", "0.2.5" };
        public static string[] IncompatibleemmVRCLoaderVersions = { "0.0.1", "0.0.2", "0.0.3", "0.0.4", "0.0.5", "1.0.0" };
        //private const string FlavourText = "Can't think of anything to ad ~Emilia";
        //public static string ValidLoaderHash { get { return "8o1t7DrcDNmUGIMmFhOYWw=="; } }
    }
}
