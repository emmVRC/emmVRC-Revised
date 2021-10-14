using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using emmVRC.Utils;
using emmVRC.Objects.ModuleBases;

namespace emmVRC.Menus
{
    [Priority(50)]
    public class SettingsMenu : MelonLoaderEvents
    {
        private static MenuPage settingsPage;
        private static SingleButton settingsButton;
        private static ButtonGroup featuresGroup;
        private static ToggleButton riskyFunctionsToggle;
        private static ToggleButton globalDynamicBonesToggle;
        private static ToggleButton friendGlobalDynamicBonesToggle;
        private static ToggleButton everyoneGlobalDynamicBonesToggle;
        private static ToggleButton vrFlightControlsToggle;
        private static ToggleButton trackingSavingToggle;
        private static ToggleButton actionMenuIntegrationToggle;
        private static ToggleButton emojiFavoriteMenuToggle;
        private static ToggleButton clockToggle;
        private static ToggleButton masterIconToggle;
        private static ToggleButton hudToggle;
        private static ToggleButton forceRestartToggle;
        private static ToggleButton unlimitedFPSToggle;
        private static ToggleButton uiExpansionKitToggle;

        private static ButtonGroup networkGroup;
        private static ToggleButton emmVRCNetworkToggle;
        private static ToggleButton avatarFavoriteListToggle;
        private static ToggleButton avatarFavoriteJumpToggle;
        private static ToggleButton submitAvatarPedestalsToggle;

        private static ButtonGroup colorGroup;
        private static ToggleButton uiColorChangingToggle;
        private static SingleButton uiColorButton;
        private static ToggleButton uiActionMenuColorChangingToggle;
        private static ToggleButton uiMicIconColorChangingToggle;
        private static ToggleButton uiMicIconPulseToggle;
        private static ToggleButton uiExpansionKitColorChangingToggle;
        private static ToggleButton nameplateColorChangingToggle;

        private static MenuPage colorChangingPage;
        private static SimpleSingleButton colorChoiceButton;
        private static SingleButton friendNameplateColorButton;
        private static SingleButton visitorNameplateColorButton;
        private static SingleButton newUserNameplateColorButton;
        private static SingleButton userNameplateColorButton;
        private static SingleButton knownUserNameplateColorButton;
        private static SingleButton trustedUserNameplateColorButton;
        private static SingleButton veteranUserNameplateColorButton;
        private static SingleButton legendaryUserNameplateColorButton;

        private static ButtonGroup vrchatUiGroup;
        private static ToggleButton upperInviteButtonsToggle;
        private static ToggleButton oneHandMovementToggle;
        private static ToggleButton micTooltipToggle;
        private static ToggleButton vrcPlusMenuTabsToggle;
        private static ToggleButton vrcPlusUserInfoToggle;

        private static ButtonGroup avatarListsGroup;
        private static ToggleButton avatarHotWorldsToggle;
        private static ToggleButton avatarRandomWorldsToggle;
        private static ToggleButton avatarPersonalListToggle;
        private static ToggleButton avatarLegacyListToggle;
        private static ToggleButton avatarPublicListToggle;
        private static ToggleButton avatarOtherListToggle;

        private static ButtonGroup keybindsGroup;
        private static ToggleButton keybindsToggle;
        private static TextButton flightKeybindButton;
        private static TextButton noclipKeybindButton;
        private static TextButton speedKeybindButton;
        private static TextButton thirdPersonKeybindButton;
        private static TextButton hudEnabledKeybindButton;
        private static TextButton respawnKeybindButton;
        private static TextButton goHomeKeybindButton;

        private static bool _initialized = false;
        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            if (buildIndex != -1 || _initialized) return;
            settingsPage = new MenuPage("emmVRC_Settings", "Settings", false, true);
            settingsButton = new SingleButton(FunctionsMenu.otherGroup, "Settings", OpenMenu, "", Functions.Core.Resources.SettingsIcon);

            featuresGroup = new ButtonGroup(settingsPage, "Features");
            riskyFunctionsToggle = new ToggleButton(featuresGroup, "Risky Functions", (bool val) =>
            {
                if (!Configuration.JSONConfig.RiskyFunctionsWarningShown && !Configuration.JSONConfig.RiskyFunctionsEnabled)
                {
                    ButtonAPI.GetQuickMenuInstance().ShowConfirmDialog("Risky Functions", "By selecting 'yes', you accept that the use of these functions could be reported to VRChat, and that you will not use them for malicious purposes.", new System.Action(() => {
                        Configuration.WriteConfigOption("RiskyFunctionsEnabled", val);
                        Configuration.WriteConfigOption("RiskyFunctionsWarningShown", true);
                    }), new System.Action(() =>
                    {
                        riskyFunctionsToggle.SetToggleState(false);
                    }));

                }
                else
                {
                    Configuration.WriteConfigOption("RiskyFunctionsEnabled", val);
                }
            }, "Enables Risky Functions, which allows use of modules such as Flight and Speed in approved worlds", "Disables Risky Functions");
            globalDynamicBonesToggle = new ToggleButton(featuresGroup, "Global\nDynamic Bones", (bool val) =>
            {
                Configuration.WriteConfigOption("GlobalDynamicBonesEnabled", val);
            }, "Enables Global Dynamic Bones, allowing you to interact with other users' dynamic bones", "Disables Global Dynamic Bones");
            friendGlobalDynamicBonesToggle = new ToggleButton(featuresGroup, "Friend Global\nDynamics", (bool val) =>
            {
                Configuration.WriteConfigOption("FriendGlobalDynamicBonesEnabled", val);
            }, "Turns on Global Dynamic Bones automatically for friends", "Do not turn on Global Dynamic Bones automatically for friends");
            everyoneGlobalDynamicBonesToggle = new ToggleButton(featuresGroup, "Everyone Global\nDynamics", (bool val) =>
            {
                Configuration.WriteConfigOption("EveryoneGlobalDynamicBonesEnabled", val);
            }, "Turns on Global Dynamic Bones for everyone. This can cause lag!", "Do not enable Global Dynamic Bones for everyone");
            new ButtonGroup(settingsPage, "");
            vrFlightControlsToggle = new ToggleButton(featuresGroup, "VR Flight\nControls", (bool val) =>
            {
                Configuration.WriteConfigOption("VRFlightControls", val);
            }, "Use VR-friendly flight controls", "Use desktop-mode flight controls");
            trackingSavingToggle = new ToggleButton(featuresGroup, "Calibration\nSaving", (bool val) => {
                Configuration.WriteConfigOption("TrackingSaving", val);
            }, "Enables saving of FBT calibration", "Disables saving of FBT calibration");
            actionMenuIntegrationToggle = new ToggleButton(featuresGroup, "Action Menu\nIntegration", (bool val) =>
            {
                Configuration.WriteConfigOption("ActionMenuIntegration", val);
            }, "Enables emmVRC Action Menu Integration", "Disables emmVRC Action Menu Integration");
            clockToggle = new ToggleButton(featuresGroup, "Clock", (bool val) =>
            {
                Configuration.WriteConfigOption("ClockEnabled", val);
            }, "Enables the Quick Menu clock, which shows both the local time and the current time spent in this instance", "Disables the Quick Menu clock");
            new ButtonGroup(settingsPage, "");
            masterIconToggle = new ToggleButton(featuresGroup, "Master Icon", (bool val) =>
            {
                Configuration.WriteConfigOption("MasterIconEnabled", val);
            }, "Enables the Master Icon, which shows above the current master of the instance", "Disables the Master Icon");
            hudToggle = new ToggleButton(featuresGroup, "HUD", (bool val) =>
            {
                Configuration.WriteConfigOption("HUDEnabled", val);
            }, "Enables the desktop HUD, which shows information such as the current position and players in the instance", "Disable the desktop HUD");
            forceRestartToggle = new ToggleButton(featuresGroup, "Force Restart", (bool val) =>
            {
                Configuration.WriteConfigOption("ForceRestartButtonEnabled", val);
            }, "Enables the Force Restart button in loading screens", "Disables the Force Restart button in loading screens");
            unlimitedFPSToggle = new ToggleButton(featuresGroup, "Unlimited\nFPS", (bool val) =>
            {
                Configuration.WriteConfigOption("UnlimitedFPSEnabled", val);
            }, "Removes the FPS limiter, which by default limits your FPS to 90 (Desktop only!)", "Enables the FPS limiter (Desktop only!)");
            networkGroup = new ButtonGroup(settingsPage, "Network");
            emmVRCNetworkToggle = new ToggleButton(networkGroup, "emmVRC\nNetwork", (bool val) =>
            {
                Configuration.WriteConfigOption("emmVRCNetworkEnabled", val);
            }, "Enables the emmVRC Network, which provides features like Avatar Favorites", "Disables the emmVRC Network");
            avatarFavoriteListToggle = new ToggleButton(networkGroup, "Avatar\nFavorites", (bool val) =>
            {
                Configuration.WriteConfigOption("AvatarFavoritesEnabled", val);
            }, "Enables emmVRC Avatar Favorites, which allows to search our database of avatars, and if you have VRChat Plus, favorite an unlimited number of avatars", "Disables emmVRC Avatar Favorites");
            avatarFavoriteJumpToggle = new ToggleButton(networkGroup, "Jump to\nStart", (bool val) =>
            {
                Configuration.WriteConfigOption("AvatarFavoritesJumpToStart", val);
            }, "In the emmVRC Favorites, automatically jumps back to the start when switching pages or reloading favorites", "In the emmVRC Favorites, automatically jumps back to the start when switching pages or reloading favorites");
            submitAvatarPedestalsToggle = new ToggleButton(networkGroup, "Submit Public\nAvatar Pedestals", (bool val) =>
            {
                Configuration.WriteConfigOption("SubmitAvatarPedestals", val);
            }, "Automatically submit public pedestals in the world to be included in emmVRC Search", "Do not automatically submit public pedestals in the world to be included in emmVRC Search");

            colorGroup = new ButtonGroup(settingsPage, "Coloring");
            uiColorChangingToggle = new ToggleButton(colorGroup, "UI Coloring", (bool val) =>
            {
                Configuration.WriteConfigOption("UIColorChangingEnabled", val);
            }, "Enable coloring the VRChat UI", "Disable coloring the VRChat UI");
            uiColorButton = new SingleButton(colorGroup, "UI\nColor", () =>
            {
                ButtonAPI.GetQuickMenuInstance().ShowAlert("Not yet implemented"); // TODO: Add color configuration
            }, "Choose the color for UI coloring", null); // TODO: Add circle for color representation
            uiActionMenuColorChangingToggle = new ToggleButton(colorGroup, "Action Menu\nColoring", (bool val) =>
            {
                Configuration.WriteConfigOption("UIActionMenuColorChangingEnabled", val);
            }, "Enable coloring of the Action Menu", "Disable coloring of the Action Menu");
            uiMicIconColorChangingToggle = new ToggleButton(colorGroup, "Microphone\nIcon Coloring", (bool val) =>
            {
                Configuration.WriteConfigOption("UIMicIconColorChangingEnabled", val);
            }, "Color the microphone icon", "Do not color the microphone icon");
            new ButtonGroup(settingsPage, "");
            uiMicIconPulseToggle = new ToggleButton(colorGroup, "Microphone\nIcon Pulsing", (bool val) =>
            {
                Configuration.WriteConfigOption("UIMicIconPulsingEnabled", val);
            }, "Disable the pulsing of the microphone icon", "Enable the pulsing of the microphone icon");
            
            nameplateColorChangingToggle = new ToggleButton(colorGroup, "Nameplate\nColoring", (bool val) =>
            {
                Configuration.WriteConfigOption("NameplateColorChangingEnabled ", val);
            }, "Color the nameplates of other users", "Do not color nameplates");
            uiExpansionKitColorChangingToggle = new ToggleButton(colorGroup, "UIExpansionKit\nColoring", (bool val) =>
            {
                Configuration.WriteConfigOption("UIExpansionKitColorChangingEnabled", val);
            }, "Enable coloring of the UI Expansion Kit menus", "Do not color UI Expansion Kit menus");

            colorChangingPage = new MenuPage("emmVRC_Settings_NameplateColors", "Nameplate Colors", false, true);
            colorChoiceButton = new SimpleSingleButton(colorGroup, "Nameplate\nColors", colorChangingPage.OpenMenu, "Configure the colors of the VRChat nameplates");
            ButtonGroup colorChangingGroup = new ButtonGroup(colorChangingPage, "");
            friendNameplateColorButton = new SingleButton(colorChangingGroup, "Friends", () =>
            {
                ButtonAPI.GetQuickMenuInstance().ShowAlert("Not yet implemented"); // TODO: Add color configuration
            }, "Configure the nameplate color for friends");
            visitorNameplateColorButton = new SingleButton(colorChangingGroup, "Visitor", () =>
            {
                ButtonAPI.GetQuickMenuInstance().ShowAlert("Not yet implemented"); // TODO: Add color configuration
            }, "Configure the nameplate color for visitorss");
            newUserNameplateColorButton = new SingleButton(colorChangingGroup, "New User", () =>
            {
                ButtonAPI.GetQuickMenuInstance().ShowAlert("Not yet implemented"); // TODO: Add color configuration
            }, "Configure the nameplate color for new users");
            userNameplateColorButton = new SingleButton(colorChangingGroup, "User", () =>
            {
                ButtonAPI.GetQuickMenuInstance().ShowAlert("Not yet implemented"); // TODO: Add color configuration
            }, "Configure the nameplate color for users");
            knownUserNameplateColorButton = new SingleButton(colorChangingGroup, "Known User", () =>
            {
                ButtonAPI.GetQuickMenuInstance().ShowAlert("Not yet implemented"); // TODO: Add color configuration
            }, "Configure the nameplate color for known users");
            trustedUserNameplateColorButton = new SingleButton(colorChangingGroup, "Trusted User", () =>
            {
                ButtonAPI.GetQuickMenuInstance().ShowAlert("Not yet implemented"); // TODO: Add color configuration
            }, "Configure the nameplate color for trusted users");

            vrchatUiGroup = new ButtonGroup(settingsPage, "VRChat UI");
            upperInviteButtonsToggle = new ToggleButton(vrchatUiGroup, "Upper Invite\nButtons", (bool val) =>
            {
                Configuration.WriteConfigOption("DisableOldInviteButtons", !val);
            }, "Disable the invite buttons along the top of the Quick Menu", "Enable the invite buttons along the top of the Quick Menu");
            oneHandMovementToggle = new ToggleButton(vrchatUiGroup, "One handed\nMovement", (bool val) =>
            {
                Configuration.WriteConfigOption("DisableOneHandMovement", !val);
            }, "Disable the one handed movement guide that appears when the action menu is open", "Enable the one handed movement guide that appears when the action menu is open");
            micTooltipToggle = new ToggleButton(vrchatUiGroup, "Microphone\nTooltip", (bool val) =>
            {
                Configuration.WriteConfigOption("DisableMicTooltip", !val);
            }, "Disable the microphone keybind tooltip above the microphone icon", "Enable the microphone keybind tooltip above the microphone icon");
            vrcPlusMenuTabsToggle = new ToggleButton(vrchatUiGroup, "VRChat+\nMenu Tabs", (bool val) =>
            {
                if (Utils.PlayerUtils.DoesUserHaveVRCPlus())
                    Configuration.WriteConfigOption("DisableVRCPlusMenuTabs", !val);
                else
                    ButtonAPI.GetQuickMenuInstance().ShowOKDialog("VRChat+ required", Objects.Attributes.VRCPlusMessage);
            }, "Disable the Gallery and VRC+ tabs in the Main Menu", "Enable the Gallery and VRC+ tabs in the Main Menu");
            new ButtonGroup(settingsPage, "");
            vrcPlusUserInfoToggle = new ToggleButton(vrchatUiGroup, "VRChat+\nUser Info", (bool val) =>
            {
                if (Utils.PlayerUtils.DoesUserHaveVRCPlus())
                    Configuration.WriteConfigOption("DisableVRCPlusUserInfo", !val);
                else
                    ButtonAPI.GetQuickMenuInstance().ShowOKDialog("VRChat+ required", Objects.Attributes.VRCPlusMessage);
            }, "Disable the VRChat Plus information in the User Info page of the Main Menu", "Enable the VRChat Plus information in the User Info page of the Main Menu");

            avatarListsGroup = new ButtonGroup(settingsPage, "Avatar Lists");
            avatarHotWorldsToggle = new ToggleButton(avatarListsGroup, "Hot Worlds\nCategory", (bool val) =>
            {
                Configuration.WriteConfigOption("DisableAvatarHotWorlds", !val);
            }, "Disable the \"Hot Worlds\" category in the Avatar Menu", "Enable the \"Hot Worlds\" category in the Avatar Menu");
            avatarRandomWorldsToggle = new ToggleButton(avatarListsGroup, "Random\nCategory", (bool val) =>
            {
                Configuration.WriteConfigOption("DisableAvatarRandomWorlds", !val);
            }, "Disable the \"Random\" category in the Avatar Menu", "Enable the \"Random Worlds\" category in the Avatar Menu");
            avatarPersonalListToggle = new ToggleButton(avatarListsGroup, "Personal\nList", (bool val) =>
            {
                Configuration.WriteConfigOption("DisableAvatarPersonal", !val);
            }, "Disable the \"Personal\" list in the Avatar Menu", "Enable the \"Personal\" list in the Avatar Menu");
            avatarLegacyListToggle = new ToggleButton(avatarListsGroup, "Legacy\nList", (bool val) =>
            {
                Configuration.WriteConfigOption("DisableAvatarLegacy", !val);
            }, "Disable the \"Legacy\" list in the Avatar Menu", "Enable the \"Legacy\" list in the Avatar Menu");
            new ButtonGroup(settingsPage, "");
            avatarPublicListToggle = new ToggleButton(avatarListsGroup, "Public\nList", (bool val) =>
            {
                Configuration.WriteConfigOption("DisableAvatarPublic", !val);
            }, "Disable the \"Public\" list in the Avatar Menu", "Enable the \"Public\" list in the Avatar Menu");
            avatarOtherListToggle = new ToggleButton(avatarListsGroup, "Other\nList", (bool val) =>
            {
                Configuration.WriteConfigOption("DisableAvatarOther", !val);
            }, "Disable the \"Other\" list in the Avatar Menu", "Enable the \"Other\" list in the Avatar Menu");


            _initialized = true;
        }
        private static void OpenMenu()
        {
            settingsPage.OpenMenu();
            riskyFunctionsToggle.SetToggleState(Configuration.JSONConfig.RiskyFunctionsEnabled);
            globalDynamicBonesToggle.SetToggleState(Configuration.JSONConfig.GlobalDynamicBonesEnabled);
            friendGlobalDynamicBonesToggle.SetToggleState(Configuration.JSONConfig.FriendGlobalDynamicBonesEnabled);
            everyoneGlobalDynamicBonesToggle.SetToggleState(Configuration.JSONConfig.EveryoneGlobalDynamicBonesEnabled);
            vrFlightControlsToggle.SetToggleState(Configuration.JSONConfig.VRFlightControls);
            trackingSavingToggle.SetToggleState(Configuration.JSONConfig.TrackingSaving);
            actionMenuIntegrationToggle.SetToggleState(Configuration.JSONConfig.ActionMenuIntegration);
            clockToggle.SetToggleState(Configuration.JSONConfig.ClockEnabled);
            masterIconToggle.SetToggleState(Configuration.JSONConfig.MasterIconEnabled);
            hudToggle.SetToggleState(Configuration.JSONConfig.HUDEnabled);
            forceRestartToggle.SetToggleState(Configuration.JSONConfig.ForceRestartButtonEnabled);
            unlimitedFPSToggle.SetToggleState(Configuration.JSONConfig.UnlimitedFPSEnabled);

            emmVRCNetworkToggle.SetToggleState(Configuration.JSONConfig.emmVRCNetworkEnabled);
            avatarFavoriteListToggle.SetToggleState(Configuration.JSONConfig.AvatarFavoritesEnabled);
            avatarFavoriteJumpToggle.SetToggleState(Configuration.JSONConfig.AvatarFavoritesJumpToStart);
            submitAvatarPedestalsToggle.SetToggleState(Configuration.JSONConfig.SubmitAvatarPedestals);

            uiColorChangingToggle.SetToggleState(Configuration.JSONConfig.UIColorChangingEnabled);
            uiActionMenuColorChangingToggle.SetToggleState(Configuration.JSONConfig.UIActionMenuColorChangingEnabled);
            uiMicIconColorChangingToggle.SetToggleState(Configuration.JSONConfig.UIMicIconColorChangingEnabled);
            uiMicIconPulseToggle.SetToggleState(Configuration.JSONConfig.UIMicIconPulsingEnabled);
            if (Functions.Core.ModCompatibility.UIExpansionKit)
            {
                uiExpansionKitColorChangingToggle.SetToggleState(Configuration.JSONConfig.UIExpansionKitColorChangingEnabled);
                uiExpansionKitColorChangingToggle.gameObject.SetActive(true);
            }
            else
                uiExpansionKitColorChangingToggle.gameObject.SetActive(false);
            nameplateColorChangingToggle.SetToggleState(Configuration.JSONConfig.NameplateColorChangingEnabled);
            
            upperInviteButtonsToggle.SetToggleState(!Configuration.JSONConfig.DisableOldInviteButtons);
            oneHandMovementToggle.SetToggleState(!Configuration.JSONConfig.DisableOneHandMovement);
            micTooltipToggle.SetToggleState(!Configuration.JSONConfig.DisableMicTooltip);
            vrcPlusMenuTabsToggle.SetToggleState(!Configuration.JSONConfig.DisableVRCPlusMenuTabs);
            vrcPlusUserInfoToggle.SetToggleState(!Configuration.JSONConfig.DisableVRCPlusUserInfo);

            avatarHotWorldsToggle.SetToggleState(!Configuration.JSONConfig.DisableAvatarHotWorlds);
            avatarRandomWorldsToggle.SetToggleState(!Configuration.JSONConfig.DisableAvatarRandomWorlds);
            avatarPersonalListToggle.SetToggleState(!Configuration.JSONConfig.DisableAvatarPersonal);
            avatarLegacyListToggle.SetToggleState(!Configuration.JSONConfig.DisableAvatarLegacy);
            avatarPublicListToggle.SetToggleState(!Configuration.JSONConfig.DisableAvatarPublic);
            avatarOtherListToggle.SetToggleState(!Configuration.JSONConfig.DisableAvatarOther);
        }
    }
}
