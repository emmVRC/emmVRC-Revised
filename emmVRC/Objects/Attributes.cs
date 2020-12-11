using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace emmVRC.Objects
{
    public class Attributes
    {
        public static string Version = "2.3.2";
        public static int LastTestedBuildNumber = 1026;
        public static string EULAVersion = "1.0.0";
        public static bool Beta = false;
        public static string DateUpdated = "12/09/2020";
        //  "This is a reference for about how long the"
        //  "text can be in the Changelog before it rolls"
        //  "off the page."
        //  "This is an pre-release build! Do not distribute to others."
        public static string Changelog =
            "• Added compatibility with VRC-Minus\n" +
            "• Increased delay in the instant restart, so that players\n" +
            "shouldn't get locked out of instances anymore.";
        public static bool Debug = false;
        public static string TargetMelonLoaderVersion = "0.2.7.3";
        public static string TargetemmVRCLoaderVersion = "1.0.0";
        public static string[] IncompatibleMelonLoaderVersions = { "0.1.0", "0.2.0", "0.2.1", "0.2.2", "0.2.3", "0.2.4", "0.2.5" };
        public static string[] IncompatibleemmVRCLoaderVersions = { "0.0.1", "0.0.2", "0.0.3", "0.0.4", "0.0.5", "1.0.0" };
        public static bool VRCPlusVersion = false;
        private const string FlavourText = "Dead? Me? Nah, never. You can ban me as much as you want, but my job will never be done here. ~Emilia";
        //public static string ValidLoaderHash { get { return "8o1t7DrcDNmUGIMmFhOYWw=="; } }
    }
}
