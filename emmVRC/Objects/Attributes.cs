﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace emmVRC.Objects
{
    public class Attributes
    {
        public static string Version = "2.4.1";
        public static int LastTestedBuildNumber = 1030;
        public static string EULAVersion = "1.0.0";
        public static bool Beta = false;
        public static string DateUpdated = "12/22/2020";
        //  "This is a reference for about how long the"
        //  "text can be in the Changelog before it rolls"
        //  "off the page."
        //  "This is an pre-release build! Do not distribute to others."
        public static string Changelog =
            "• Added an Action (Radial) Menu for emmVRC! You can now\n" +
            "quickly access risky functions and parameter saving within\n" +
            "the radial menu!\n" +
            "• Reorganized the Settings menu, giving Network Settings its\n" +
            "own section\n" +
            "• Fixed parameter saving, so that it should save the correct\n" +
            "values. Also added more internal parameters to the blacklist.\n" +
            "• Implemented optimizations for the avatar favorites. This\n" +
            "should improve performance even more when the menu isn't\n" +
            "active.";
        public static bool Debug = false;
        public static string TargetMelonLoaderVersion = "0.2.7.3";
        public static string TargetemmVRCLoaderVersion = "1.0.0";
        public static string[] IncompatibleMelonLoaderVersions = { "0.1.0", "0.2.0", "0.2.1", "0.2.2", "0.2.3", "0.2.4", "0.2.5" };
        public static string[] IncompatibleemmVRCLoaderVersions = { "0.0.1", "0.0.2", "0.0.3", "0.0.4", "0.0.5", "1.0.0" };
        private const string FlavourText = "Push me over. Kick me and beat me while I'm down. I'm gonna get up and keep moving forward, no matter what. ~Emilia";
        //public static string ValidLoaderHash { get { return "8o1t7DrcDNmUGIMmFhOYWw=="; } }
    }
}
