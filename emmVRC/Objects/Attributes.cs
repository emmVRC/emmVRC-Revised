using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace emmVRC.Objects
{
    public class Attributes
    {
        public static string Version = "2.1.0";
        public static string EULAVersion = "1.0.0";
        public static bool Beta = false;
        public static string DateUpdated = "10/27/2020";
        //  "This is a reference for about how long the"
        //  "text can be in the Changelog before it rolls"
        //  "off the page."
        //  "This is an pre-release build! Do not distribute to others."
        public static string Changelog =
            //"This is an pre-release build! Do not distribute to others\n" +
            "• Added stealth mode! This disables most of emmVRC's visual\n" +
            "functions, while allowing you to still use the mod in secret!\n" +
            "• Player History now shows the time a player joined your\n" +
            "instance";
        /*public static string Changelog =
            "• Fixed the Social Menu teleport button. No\n" +
            "longer will it make an insanely loud sound!\n" +
            "• Fixed Portal Blocking for VRChat 2020.3.2p3\n"+
            "• Added compatibility layer for the Immersive\n"+
            "HUD mod by DDAkebono";*/
        public static bool Debug = false;
        public static string TargetMelonLoaderVersion = "0.2.6";
        public static string TargetemmVRCLoaderVersion = "1.0.0";
        public static string[] IncompatibleMelonLoaderVersions = { "0.1.0", "0.2.0", "0.2.1", "0.2.2", "0.2.3", "0.2.4", "0.2.5" };
        public static string[] IncompatibleemmVRCLoaderVersions = { "0.0.1", "0.0.2", "0.0.3", "0.0.4", "0.0.5" };
        //public static string ValidLoaderHash { get { return "8o1t7DrcDNmUGIMmFhOYWw=="; } }
    }
}
