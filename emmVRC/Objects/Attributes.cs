using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace emmVRC.Objects
{
    public class Attributes
    {
        public static string Version = "1.7.1";
        public static bool Beta = false;
        public static string DateUpdated = "7/31/2020";
        //  "This is a reference for a bout how long the text can be in"
        //  "the Changelog before it rolls off the page."
        public static string Changelog = 
            "• Changed Global Dynamic Behavior with existing\n" +
            "colliders. From now on, existing colliders on bones\n" +
            "will be preserved when using Global Dynamic Bones\n" +
            "• Corrected the behavior of flight to be correct.\n" +
            "It should be more correct now!";
        public static bool Debug = false;
        public static string TargetMelonLoaderVersion = "0.2.5";
        public static string TargetemmVRCLoaderVersion = "1.0.0";
        public static string[] IncompatibleMelonLoaderVersions = { "0.1.0", "0.2.0", "0.2.1", "0.2.2", "0.2.3" };
        public static string[] IncompatibleemmVRCLoaderVersions = { "0.0.1", "0.0.2", "0.0.3", "0.0.4", "0.0.5" };
        //public static string ValidLoaderHash { get { return "8o1t7DrcDNmUGIMmFhOYWw=="; } }
    }
}
