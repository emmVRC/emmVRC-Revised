
namespace emmVRC.Objects
{
    public class Config
    {
        // The main emmVRC configuration structure. Please don't remove anything; it seems to break configs!

        // Feature options
        public bool OpenBetaEnabled = false;
        public bool UnlimitedFPSEnabled = false;
        public bool RiskyFunctionsEnabled = false;
        public bool GlobalDynamicBonesEnabled = false;
        public bool EveryoneGlobalDynamicBonesEnabled = false;
        public bool emmVRCNetworkEnabled = true;
        public bool GlobalChatEnabled = true;
        public bool AutoInviteMessage = false;
        public bool InfoBarDisplayEnabled = true;
        public bool ClockEnabled = true;
        public bool AvatarFavoritesEnabled = true;
        public bool MasterIconEnabled = true;
        public bool LogoButtonEnabled = true;
        public bool HUDEnabled = true;
        public bool ForceRestartButtonEnabled = true;

        // FOV value
        public int CustomFOV = 60;

        // Button positions
        public int FunctionsButtonX = 5;
        public int FunctionsButtonY = 2;
        public int LogoButtonX = 5;
        public int LogoButtonY = -1;
        public int UserInteractButtonX = 4;
        public int UserInteractButtonY = 2;
        public int NotificationButtonPositionX = 0;
        public int NotificationButtonPositionY = -1;
        public int PlayerActionsButtonX = 4;
        public int PlayerActionsButtonY = 2;

        // Hack options
        // Sub section: Shortcut Menu
        public bool DisableReportWorldButton = false;
        public bool DisableEmojiButton = false;
        public bool DisableRankToggleButton = false;
        // Sub section: User Interact Menu
        public bool DisablePlaylistsButton = false;
        public bool DisableAvatarStatsButton = false;
        public bool DisableReportUserButton = false;
        // Sub section: Avatar Menu
        public bool DisableAvatarHotWorlds = false;
        public bool DisableAvatarRandomWorlds = false;
        public bool DisableAvatarLegacy = false;
        public bool DisableAvatarPublic = false;

        // UI Color setup
        public bool UIColorChangingEnabled = false;
        public string UIColorHex = "#0EA6AD";

        // Nameplate Color Setup
        public bool NameplateColorChangingEnabled = false;
        public string FriendNamePlateColorHex = "#FFFF00";
        public string VisitorNamePlateColorHex = "#CCCCCC";
        public string NewUserNamePlateColorHex = "#1778FF";
        public string UserNamePlateColorHex = "#2BCE5C";
        public string KnownUserNamePlateColorHex = "#FF7B42";
        public string TrustedUserNamePlateColorHex = "#8143E6";

        // Info spoofing and hiding
        public bool InfoSpoofingEnabled = false;
        public string InfoSpoofingName = "";
        public bool InfoHidingEnabled = false;

        // One-timers
        public bool WelcomeMessageShown = false;
        public bool RiskyFunctionsWarningShown = false;

        // Hook-ins for the VRChat nameplate and UI hiding
        public bool NameplatesVisible = true;
        public bool UIVisible = true;

        public int[] FlightKeybind = new int[2] {  102, 306 };
        public int[] NoclipKeybind = new int[2] {  109, 306 };
        public int[] SpeedKeybind = new int[2] {  103, 306 };
        public int[] ThirdPersonKeybind = new int[2] {  116, 306 };
        public int[] ToggleHUDEnabledKeybind = new int[2] {  106, 306 };
    }
}
