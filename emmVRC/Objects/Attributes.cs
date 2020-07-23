using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace emmVRC.Objects
{
    public class Attributes
    {
        public static string Version = "1.6.1";
        public static bool Beta = false;
        public static string DateUpdated = "7/23/2020";
        //  "This is a reference for how long the text can be in the Changelog"
        //  "before it rolls off the page."
        public static string Changelog = 
            "• Added a configuration option for changing the maximum \n" +
            "FPS of the \"Unlimited FPS\" setting\n" +
            "• Added a configuration option for changing the maximum \n" +
            "speed of the Risky Functions\n" +
            "• Added lots of optimizations in order to enhance FPS while \n" +
            "using various functions of emmVRC\n";
        public static bool Debug = false;
        public static string TargetMelonLoaderVersion = "0.2.5";
        public static string TargetemmVRCLoaderVersion = "1.0.0";
        public static string[] IncompatibleMelonLoaderVersions = { "0.1.0", "0.2.0", "0.2.1", "0.2.2", "0.2.3" };
        public static string[] IncompatibleemmVRCLoaderVersions = { "0.0.1", "0.0.2", "0.0.3", "0.0.4", "0.0.5" };
        //public static string ValidLoaderHash { get { return "8o1t7DrcDNmUGIMmFhOYWw=="; } }
    }
}
