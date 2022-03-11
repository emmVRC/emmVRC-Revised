using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace emmVRC.Objects
{
    public class Attributes
    {
        public static readonly Version Version = new Version(3, 1, 4, 0); 
        public const int LastTestedBuildNumber = 1169;
        public const string EULAVersion = "1.0.1";
        public const bool Beta = false;
        public const string DateUpdated = "3/10/2022";
        //  "This is a reference for about how long the"
        //  "text can be in the Changelog before it rolls"
        //  "off the page."
        //  "This is an pre-release build! Do not distribute to others."
        public const string Changelog =
            "<b>v3.1.4</b>\n" +
            "• Attempted to make login text more clear about how to make a pin and set up the emmVRC Network\n" +
            "• Fixed issues with Oculus, including network login and Risky Functions\n" +
            "<b>v3.1.3</b>\n" +
            "• Compatibility with VRChat build 1169\n" +
            "<b>v3.1.2</b>\n" +
            "• Removed Emoji Favorites from the action menu temporarily\n" +
            "• Cleaned up and removed unused features\n" +
            "• Fixed a bug where the Programs module would generate an invalid configuration\n" +
            "• Risky Functions are now allowed for world creators, to assist in development. Note that this will <b>not</b> override the existence of the off/on objects\n" +
            "• Desktop reticle now disappears when in third person\n" +
            "\n" +
            "<b>v3.1.1</b>\n" +
            "• Fixed avatar favorites showing error avatars and not allowing use\n" +
            "\n" +
            "<b>v3.1.0</b>\n" +
            "• Implemented Avatar Options! This is where per-avatar settings such as shader and dynamic bone toggling will be from now on, as well as Global Dynamic Bone configuration\n" +
            "• Added light toggles to Avatar Options! This allows you to easily toggle off lights on individual avatars\n" +
            "• Implemented Jump toggling once again! This also allows you to disable jumping after having enabled it in a world\n"+
            "• Keyboard shortcuts for respawning and going home now work without having the Quick Menu open\n" +
            "• Fixed bans from the emmVRC Network not showing up correctly\n" +
            "• Fixed World Volume mute not saving\n" +
            "• Updated for VRChat build 1160, now with more reflection to prevent breaking\n" +
            "• Fixed Desktop and VR flight not working in SDK3 worlds\n" +
            "\n" +
            "<b>v3.0.5</b>\n" +
            "• Fixed support for mods such as ReModCE that would cause the menus to misbehave\n" +
            "• Fixed notification icons being colored slightly blue\n" +
            "\n" +
            "<b>v3.0.4</b>\n" +
            "• Fixed the Social Menu functions button not working on VRChat build 1151\n" +
            "• Fixed Oculus support\n" +
            "\n" +
            "<b>v3.0.3</b>\n" +
            "• Fixed the \"Grey Square\" bug (for real for real this time)\n" +
            "• Added a toggle for UIExpansionKit integration to Settings\n" +
            "• Added a version and emmVRC Network connection status indicator to the emmVRC Functions menu. This behaves identically to the info bar from prior versions\n" +
            "• The new update notification now has a button to view the changelog\n" +
            "• Added emmVRC Actions to the menu displayed when you select a user\n" +
            "• Master Icon settings now actually work\n" +
            "• Changed the behavior of the quit icon in the Quick Menu's Settings page to now offer Instant Restart\n" +
            "• Adjusted the Player Tweaks menu so that speed is available without needing to scroll down a tiny amount\n" +
            "• Fixed the persistent notification icon issue\n" +
            "• The EULA prompt has been moved to the emmVRC tab action, so it will behave better on all platforms\n" +
            "• Fixed the VR flying speeds being switched (left and right would use run speed, and forward/back would use strafe speed)\n" +
            "• Fixed the Nameplate Coloring setting not updating correctly\n" +
            "• Fixed search being accessible despite not being connected to the emmVRC Network\n" +
            "• Global Dynamic Bones should now be functional as it was before the UI update\n" +
            "• Fixed being able to enable NoClip without flight via the action menu and keybinds\n" +
            "\n" +
            "<b>v3.0.2</b>\n" +
            "• Fixed the \"Grey Square\" bug (for real this time)\n" +
            "• Fixed prompts and dialog boxes not showing (resulting in certain functions like Risky Functions being unavailable)\n" +
            "• Removed the Show Author button, as it is now a part of VRChat\n" +
            "\n" +
            "<b>v3.0.1</b>\n" +
            "• Fixed the \"Grey Square\" bug\n" +
            "• Fixed selecting users in the Player History menu\n" +
            "• Fixed all toggles not updating to their proper toggle states\n" +
            "• Fixed being able to enable NoClip and not Flight, resulting in the player becoming stuck\n" +
            "\n" +
            "<b>v3.0.0</b>\n" +
            "• Interaction with pickup triggers (e.g. flashlights) now works like the vanilla game\n" +
            "• emmVRC's menus have been completely remade for the new VRChat UI\n" +
            "• Added more exclusions and inclusions to the color module\n" +
            "• Several modules of emmVRC have been rewritten for better performance and expandability\n" +
            "• Errors in specific modules of emmVRC will no longer cause the rest of the mod to stop loading or working\n" +
            "\n" +
            "<b><color=#FF5555>Notice</color></b>\n" +
            "The following features are <i>known</i> to be broken or unavailable, and will be available in a later update:\n" +
            "- Alarm Clocks\n" +
            "- Emoji Favorites\n" +
            "- Keybind configuration via the UI\n" +
            "\n" +
            "The following features have been removed, and will <i>not</i> be returning to emmVRC:\n" +
            "- Info spoofing\n" +
            "- Stealth mode\n" +
            "";
        public static bool Debug = false;
        public static readonly Version MinimumMelonLoaderVersion = new Version(0, 5, 2);
        public static readonly Version MinimumemmVRCLoaderVersion = new Version(1, 2, 0);
        //public static readonly string[] FlavourTextList = {
        //            "Did you know that Tab is also a drink?",
        //            "Going strong for 2 years!",
        //            "Visual Studio makes me want to scream.",
        //            "\"I'll believe in climate change when Texas\nfreezes over.\"",
        //            "It just works™!",
        //            "Sample Text",
        //            "ǅǅǅǄǄǄǅǅǄǅǅǅ",
        //            "Object reference not set to an instance of\nan object",
        //            "\"You spin my head right round, right round,\nlike a record baby\"",
        //            "<b>BOAT STUCK! <i>BOAT STUCK!</i></b>",
        //            ""
        //};
        public const string VRCPlusMessage = "VRChat, like emmVRC, relies on the support of their users to keep the platform free. Please support VRChat to unlock these features.";
    }
}
