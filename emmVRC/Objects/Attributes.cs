using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace emmVRC.Objects
{
    public class Attributes
    {
        public static string Version = "1.9.2";
        public static bool Beta = false;
        public static string DateUpdated = "8/14/2020";
        //  "This is a reference for a bout how long the"
        //  "text can be in the Changelog before it rolls"
        //  "off the page."
        public static string Changelog =
            "• Removed the emmVRC emote buttons, as VRChat\n" +
            "added them back\n" +
            "• Fixed an error when teleporting to a user\n" +
            "via the Social menu\n" +
            "• Reduced the time UI color changing takes to\n" +
            "finish by about 500ms!\n" +
            "• Portal blocking is fixed for VRChat 2020.3.2p2\n";
        public static bool Debug = false;
        public static string TargetMelonLoaderVersion = "0.2.6";
        public static string TargetemmVRCLoaderVersion = "1.0.0";
        public static string[] IncompatibleMelonLoaderVersions = { "0.1.0", "0.2.0", "0.2.1", "0.2.2", "0.2.3" };
        public static string[] IncompatibleemmVRCLoaderVersions = { "0.0.1", "0.0.2", "0.0.3", "0.0.4", "0.0.5" };
        //public static string ValidLoaderHash { get { return "8o1t7DrcDNmUGIMmFhOYWw=="; } }
    }
}
