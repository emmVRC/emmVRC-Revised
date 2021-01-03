using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace emmVRC.Objects
{
    public class Attributes
    {
        public static string Version = "2.5.0";
        public static int LastTestedBuildNumber = 1030;
        public static string EULAVersion = "1.0.0";
        public static bool Beta = false;
        public static string DateUpdated = "12/22/2020";
        //  "This is a reference for about how long the"
        //  "text can be in the Changelog before it rolls"
        //  "off the page."
        //  "This is an pre-release build! Do not distribute to others."
        public static string Changelog =
            "• emmVRC now supports having multiple custom menu tracks\n" +
            "at once! There is a new \"CustomMenuMusic\" folder that\n" +
            "you can drop files into, to be picked at random on startup!\n" +
            "• The master icon feature is back in emmVRC, and fully\n" +
            "functional\n" +
            "•The emmVRC HUD now shows the ping of other users in the\n" +
            "world\n" +
            "\nPlus countless bug fixes. Please see the Discord for the full\n" +
            "changelog!";
        public static bool Debug = false;
        public static string TargetMelonLoaderVersion = "0.2.7.3";
        public static string TargetemmVRCLoaderVersion = "1.0.0";
        public static string[] IncompatibleMelonLoaderVersions = { "0.1.0", "0.2.0", "0.2.1", "0.2.2", "0.2.3", "0.2.4", "0.2.5" };
        public static string[] IncompatibleemmVRCLoaderVersions = { "0.0.1", "0.0.2", "0.0.3", "0.0.4", "0.0.5", "1.0.0" };
        private const string FlavourText = "Down with the sickness! ~Emilia";
        //public static string ValidLoaderHash { get { return "8o1t7DrcDNmUGIMmFhOYWw=="; } }
    }
}
