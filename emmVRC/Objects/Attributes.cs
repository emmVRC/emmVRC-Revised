using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace emmVRC.Objects
{
    public class Attributes
    {
        public static string Version = "2.10.2";
        public static int LastTestedBuildNumber = 1108;
        public static string EULAVersion = "1.0.1";
        public static bool Beta = false;
        public static string DateUpdated = "06/24/2021";
        //  "This is a reference for about how long the"
        //  "text can be in the Changelog before it rolls"
        //  "off the page."
        //  "This is an pre-release build! Do not distribute to others."
        public static string Changelog =
            "<b>v2.10.2</b>\n" +
            "• Updated the in-game changelog format to now show all of the changes since the last major emmVRC version\n" +
            "• More fixes for the double menu glitch related to selecting users\n" +
            "• Avatar permissions are now fully working, and affect both the mirror and shadow clones of avatars now\n" +
            "• Show Avatar Author should now work for Oculus users\n\n" +
            "<b>v2.10.1</b>\n" +
            "• Fixed issues with Full Body Calibration\n" +
            "• Added a failsafe for emmVRC's FBTSaving to not lead to the calibration button doing nothing\n" +
            "• Fixed issues with the emmVRC Tab Menu causing the double menu glitch\n\n" +
            "<b>v2.10.0</b>\n"+
            "• Added a new <b>scrolling text menu</b>! This replaces the older style of text displays in the Quick Menu, and it makes reading and adding text to the emmVRC menus a <i>lot</i> easier\n" +
            "• Added a <b>last login</b> indicator to the User Info page! This will show the last time a user logged into VRChat if they're currently offline\n" +
            "• Added a platform indicator to the profile picture in the User Info page\n" +
            "• Added a <b>View Full</b> button to the World Info page! This allows you to view the entire description of a world, rather than the cut off version that is normally shown\n" +
            "• Added an <b>Avatar Author</b> button to the avatar menu! It is in the upper left of the menu, and will allow you to view the author of any avatar, directly from the menu\n" +
            "• Adjusting a volume slider in the settings while mute is active will now disable the mute for that slider\n" +
            "• Fixed issues with the Quick Menu menus becoming stuck in certain situations (particularly when selecting other users)\n"+
            "• Fixed issues with Select Current User\n"+
            "• Fixed Emoji Favourites for both Oculus and Steam\n"+
            "• Fixed the bug where teleporting from the social menu would put you in a fallback avatar";
        public static bool Debug = false;
        public static string TargetMelonLoaderVersion = "0.4.0";
        public static string TargetemmVRCLoaderVersion = "1.2.0";
        public static string[] IncompatibleMelonLoaderVersions = { "0.1.0", "0.2.0", "0.2.1", "0.2.2", "0.2.3", "0.2.4", "0.2.5", "0.2.6", "0.2.7", "0.2.7.1", "0.2.7.2", "0.2.7.3", "0.2.7.4", "0.3.0" };
        public static string[] IncompatibleemmVRCLoaderVersions = { "0.0.1", "0.0.2", "0.0.3", "0.0.4", "0.0.5", "1.0.0", "1.1.0" };
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
        public static string VRCPlusMessage = "VRChat, like emmVRC, relies on the support of their users to keep the platform free. Please support VRChat to unlock these features.";
    }
}
