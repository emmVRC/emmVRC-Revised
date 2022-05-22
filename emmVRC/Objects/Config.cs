
using UnityEngine;

namespace emmVRC.Objects
{
    public class Config
    {
        // The main emmVRC configuration structure. Please don't remove anything; it seems to break configs!

        // emmVRC Version log
        public string LastVersion = "0.0.0";

        // emmVRC Language
        public int LanguageOverride = -1;

        // Feature options
        public bool RiskyFunctionsEnabled = false;
        public bool GlobalDynamicBonesEnabled = true;
        public bool FriendGlobalDynamicBonesEnabled = false;
        public bool EveryoneGlobalDynamicBonesEnabled = false;
        public bool AutomaticHandCollidersEnabled = false;
        public bool emmVRCNetworkEnabled = true;
        public bool GlobalChatEnabled = true;
        public bool VRFlightControls = true;
        public bool UIExpansionKitIntegration = false;
        public bool InfoBarDisplayEnabled = true;
        public bool ClockEnabled = true;
        public bool AvatarFavoritesEnabled = true;
        public bool AvatarFavoritesJumpToStart = true;
        public bool SubmitAvatarPedestals = true;
        public bool MasterIconEnabled = true;
        public bool LogoButtonEnabled = true;
        public bool HUDEnabled = true;
        public bool VRHUDInDesktop = false;
        public bool MoveVRHUDIfSpaceFree = false;
        public bool ForceRestartButtonEnabled = true;
        public bool PortalBlockingEnable = false;
        public bool ChairBlockingEnable = false;
        public bool PlayerHistoryEnable = false;
        public bool LogPlayerJoin = false;
        public bool TrackingSaving = true;
        public bool ActionMenuIntegration = true;
        public bool MigrationButtonVisible = true;
        public bool DisableMicTooltip = false;
        public bool ActionMenuAPIIntegration = false;

        public bool CameraPlus = true;

        public bool IgnoreWorldCreatorTips = false;

        // Avatar List render limits
        public int FavoriteRenderLimit = 50;
        public int SearchRenderLimit = 100;

        // FOV value
        public int CustomFOV = 60;

        // FPS limit
        public int FPSLimit = 144;

        // Speed modifier
        public float SpeedModifier = 2f;

        // Last seen startup message
        public int LastSeenStartupMessage = -1;

        // Flashlight Config
        public string FlashlightColorHex = "#FFFFFF";
        public float FlashlightRange = 5f;
        public float FlashlightPower = 1f;
        public float FlashlightAngle = 50f;

        public System.Collections.Generic.List<int> FavouritedEmojis = new System.Collections.Generic.List<int>();
        public bool AlarmsEnabled = true;

        // Hack options
        public bool DisableAvatarPedestals = false;

        // Sub section: Shortcut Menu
        public bool DisableOldInviteButtons = false;
        // Sub section: Action Menu
        public bool DisableOneHandMovement = false;
        // Sub section: VRChat Minus
        public bool DisableVRCPlusMenuTabs = false;
        public bool DisableVRCPlusUserInfo = false;

        // Sub section: Avatar Menu
        public bool DisableAvatarHotWorlds = false;
        public bool DisableAvatarRandomWorlds = false;
        public bool DisableAvatarPersonal = false;
        public bool DisableAvatarLegacy = false;
        public bool DisableAvatarPublic = false;
        public bool DisableAvatarOther = false;

        // UI Color setup
        public bool UIColorChangingEnabled = false;
        public string UIColorHex = "#0EA6AD";
        public bool UIActionMenuColorChangingEnabled = true;
        public bool UIExpansionKitColorChangingEnabled = true;
        public bool UIMicIconColorChangingEnabled = false;
        public bool UIMicIconPulsingEnabled = true;

        // Nameplate Color Setup
        public bool NameplateColorChangingEnabled = false;
        public string FriendNamePlateColorHex = "#FFFF00";
        public string VisitorNamePlateColorHex = "#CCCCCC";
        public string NewUserNamePlateColorHex = "#1778FF";
        public string UserNamePlateColorHex = "#2BCE5C";
        public string KnownUserNamePlateColorHex = "#FF7B42";
        public string TrustedUserNamePlateColorHex = "#8143E6";
        public string VeteranUserNamePlateColorHex = "#ABCDEE";
        public string LegendaryUserNamePlateColorHex = "#FF69B4";

        // Avatar list sorting options
        public int SortingMode = 0;
        public bool SortingInverse = false;

        // One-timers
        public string AcceptedEULAVersion = "0.0.0";
        public bool WelcomeMessageShown = false;
        public bool RiskyFunctionsWarningShown = false;

        // Hook-ins for the VRChat nameplate and UI hiding
        public bool NameplatesVisible = true;
        public bool UIVisible = true;

        // Hook-ins for the VRChat volume controls
        public float UIVolume = 0f;
        public bool UIVolumeMute = false;
        public float WorldVolume = 0f;
        public bool WorldVolumeMute = false;
        public float VoiceVolume = 0f;
        public bool VoiceVolumeMute = false;
        public float AvatarVolume = 0f;
        public bool AvatarVolumeMute = false;

        // Network

        // Extra options to configure, in case people want to mess with them
        public float MaxSpeedIncrease = 5f;

        public bool EnableKeybinds = true;
        public bool FlightKeybindEnabled = true;
        public int[] FlightKeybind = new int[2] { 102, 306 };
        public bool NoclipKeybindEnabled = true;
        public int[] NoclipKeybind = new int[2] { 109, 306 };
        public bool SpeedKeybindEnabled = true;
        public int[] SpeedKeybind = new int[2] { 103, 306 };
        public bool ThirdPersonKeybindEnabled = true;
        public int[] ThirdPersonKeybind = new int[2] { 116, 306 };
        public bool ToggleHUDKeybindEnabled = true;
        public int[] ToggleHUDEnabledKeybind = new int[2] { 106, 306 };
        public bool RespawnKeybindEnabled = true;
        public int[] RespawnKeybind = new int[2] { 121, 306 };
        public bool GoHomeKeybindEnabled = true;
        public int[] GoHomeKeybind = new int[2] { 117, 306 };
    }
}
