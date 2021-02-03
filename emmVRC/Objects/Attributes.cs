using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace emmVRC.Objects
{
    public class Attributes
    {
        public static string Version = "2.6.6";
        public static int LastTestedBuildNumber = 1046;
        public static string EULAVersion = "1.0.1";
        public static bool Beta = false;
        public static string DateUpdated = "02/02/2021";
        //  "This is a reference for about how long the"
        //  "text can be in the Changelog before it rolls"
        //  "off the page."
        //  "This is an pre-release build! Do not distribute to others."
        public static string Changelog =
            "• emmVRC is now compatible with VRChat build 1046!\n" +
            "• Color changing now (mostly) supports the new\n" +
            "invite system and menus!\n" +
            "• Fixed a bug where the tabs would still be visible\n" +
            "after using emmVRC buttons\n" +
            "• Fixed the player tweaks menu to actually hide\n" +
            "the previous menu\n" +
            "• Added an option to turn off the old Invite buttons\n" +
            "from pre-build 1046";
        public static bool Debug = false;
        public static string TargetMelonLoaderVersion = "0.2.7.3";
        public static string TargetemmVRCLoaderVersion = "1.0.0";
        public static string[] IncompatibleMelonLoaderVersions = { "0.1.0", "0.2.0", "0.2.1", "0.2.2", "0.2.3", "0.2.4", "0.2.5" };
        public static string[] IncompatibleemmVRCLoaderVersions = { "0.0.1", "0.0.2", "0.0.3", "0.0.4", "0.0.5", "1.0.0" };
        private const string FlavourText = "Love you just the way you are. A cinema... my cinema. ~Emilia";
        //public static string ValidLoaderHash { get { return "8o1t7DrcDNmUGIMmFhOYWw=="; } }
    }
}
