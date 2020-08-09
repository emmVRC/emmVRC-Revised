using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Events;
using emmVRC.Libraries;
using emmVRC.Hacks;
using UnityEngine;
using UnityEngine.UI;

#pragma warning disable 4014

namespace emmVRC.Menus
{
    public class SettingsMenu
    {
        // Base menu for the Settings menu
        public static PaginatedMenu baseMenu;
        public static QMSingleButton ResetSettingsButton;

        // Page 1
        private static PageItem RiskyFunctions;
        private static PageItem VRFlightControls;
        private static PageItem GlobalDynamicBones;
        private static PageItem FriendGlobalDynamicBones;
        private static PageItem EveryoneGlobalDynamicBones;
        private static PageItem emmVRCNetwork;
        private static PageItem GlobalChat;
        private static PageItem ConsoleClean;
        private static PageItem AvatarFavoriteList;

        // Page 2
        private static PageItem InfoBar;
        private static PageItem Clock;
        private static PageItem MasterIcon;
        private static PageItem LogoButton;
        private static PageItem HUD;
        private static PageItem ChooseHUD;
        private static PageItem MoveVRHUD;
        private static PageItem ForceRestart;
        private static PageItem UnlimitedFPS;

        // Page 3
        private static PageItem UIColorChanging;
        private static PageItem UIColorChangePickerButton;
        private static ColorPicker UIColorChangePicker;
        private static PageItem InfoSpoofing;
        private static PageItem InfoHiding;
        private static PageItem InfoSpooferNamePicker;

        // Page 4
        private static PageItem NameplateColorChanging;
        private static PageItem FriendNameplateColorPickerButton;
        private static ColorPicker FriendNameplateColorPicker;
        private static PageItem VisitorNameplateColorPickerButton;
        private static ColorPicker VisitorNameplateColorPicker;
        private static PageItem NewUserNameplateColorPickerButton;
        private static ColorPicker NewUserNameplateColorPicker;
        private static PageItem UserNameplateColorPickerButton;
        private static ColorPicker UserNameplateColorPicker;
        private static PageItem KnownUserNameplateColorPickerButton;
        private static ColorPicker KnownUserNameplateColorPicker;
        private static PageItem TrustedUserNameplateColorPickerButton;
        private static ColorPicker TrustedUserNameplateColorPicker;

        // Page 5
        private static PageItem DisableReportWorld;
        private static PageItem DisableEmoji;
        private static PageItem DisableEmote;
        private static PageItem DisableRankToggle;

        // Page 6
        private static PageItem DisablePlaylists;
        private static PageItem DisableAvatarStats;
        private static PageItem DisableReportUser;
        private static PageItem MinimalWarnKick;

        // Page 7
        private static PageItem DisableAvatarHotWorlds;
        private static PageItem DisableAvatarRandomWorlds;
        private static PageItem DisableAvatarLegacyList;
        private static PageItem DisableAvatarPublicList;

        // Page 8
        private static PageItem FlightKeybind;
        private static PageItem NoclipKeybind;
        private static PageItem SpeedKeybind;
        private static PageItem ThirdPersonKeybind;
        private static PageItem ToggleHUDEnabledKeybind;
        private static PageItem RespawnKeybind;
        private static PageItem GoHomeKeybind;

        private static bool canToggleNetwork = true;


        public static void Initialize()
        {
            // Initialize the Paginated Menu for the Settings menu.
            baseMenu = new PaginatedMenu(FunctionsMenu.baseMenu.menuBase, 1024, 768, "Settings", "The base menu for the settings menu", null);
            baseMenu.menuEntryButton.DestroyMe();

            ResetSettingsButton = new QMSingleButton(baseMenu.menuBase, 5, 1, "<color=#FFCCBB>Revert to\ndefaults</color>", () =>
            {
                VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowStandardPopup("emmVRC", "Are you sure you want to revert settings? You will lose your custom colors and custom button positions!", "Yes", new System.Action(() =>
                {
                    Configuration.JSONConfig = new Objects.Config { WelcomeMessageShown = true };
                    Configuration.SaveConfig();
                    ColorChanger.ApplyIfApplicable();
                    Nameplates.colorChanged = true;
                    ShortcutMenuButtons.Process();
                    emmVRCLoader.Logger.Log("emmVRC settings have been reverted.");
                    VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup();
                }), "No", new System.Action(() => {
                    VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup();
                }));
            }, "Resets all emmVRC Settings to their default values. This requires a restart to fully take effect");

            RiskyFunctions = new PageItem("Risky Functions", () =>
            {
                if (!Configuration.JSONConfig.RiskyFunctionsWarningShown)
                    VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowStandardPopup("Risky Functions", "By agreeing, you accept that the use of these functions could be reported to VRChat, and that you will not use them for malicious purposes.\n", "Agree", new System.Action(() =>
                {
                    VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup();
                    Configuration.JSONConfig.RiskyFunctionsEnabled = true;
                    Configuration.JSONConfig.RiskyFunctionsWarningShown = true;
                    Configuration.SaveConfig();
                    MelonLoader.MelonCoroutines.Start(Managers.RiskyFunctionsManager.CheckWorld());
                    RefreshMenu();
                }), "Decline", new System.Action(() =>
                {
                    VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup();
                    RiskyFunctions.SetToggleState(false);
                    RefreshMenu();
                }));
                else
                {
                    Configuration.JSONConfig.RiskyFunctionsEnabled = true;
                    Configuration.SaveConfig();
                    MelonLoader.MelonCoroutines.Start(Managers.RiskyFunctionsManager.CheckWorld());
                    RefreshMenu();
                }
            }, "Disabled", () =>
            {
                Configuration.JSONConfig.RiskyFunctionsEnabled = false;
                Configuration.SaveConfig();
                RefreshMenu();
                PlayerTweaksMenu.SetRiskyFunctions(false);
            }, "TOGGLE: Enables the Risky Functions, which contains functions that shouldn't be used in public worlds. This includes flight, noclip, speed, and teleporting/waypoints");
            VRFlightControls = new PageItem("VR Flight\nControls", () =>
            {
                Configuration.JSONConfig.VRFlightControls = true;
                Configuration.SaveConfig();
            }, "Disabled", () =>
            {
                Configuration.JSONConfig.VRFlightControls = false;
                Configuration.SaveConfig();
            }, "TOGGLE: Enables the enhanced VR flight controls. May not work on Windows Mixed Reality");
            GlobalDynamicBones = new PageItem("Global\nDynamic Bones", () =>
            {
                Configuration.JSONConfig.GlobalDynamicBonesEnabled = true;
                Configuration.SaveConfig();
                RefreshMenu();
                VRCPlayer.field_Internal_Static_VRCPlayer_0.ReloadAllAvatars();
            }, "Disabled", () =>
            {
                Configuration.JSONConfig.GlobalDynamicBonesEnabled = false;
                Configuration.JSONConfig.FriendGlobalDynamicBonesEnabled = false;
                Configuration.JSONConfig.EveryoneGlobalDynamicBonesEnabled = false;
                Configuration.SaveConfig();
                RefreshMenu();
                LoadMenu();
                VRCPlayer.field_Internal_Static_VRCPlayer_0.ReloadAllAvatars();
            }, "TOGGLE: Enables the Global Dynamic Bones system");
            FriendGlobalDynamicBones = new PageItem("Friend Global\nDynamic Bones", () =>
            {
                Configuration.JSONConfig.FriendGlobalDynamicBonesEnabled = true;
                Configuration.JSONConfig.EveryoneGlobalDynamicBonesEnabled = false;
                Configuration.SaveConfig();
                RefreshMenu();
                LoadMenu();
                VRCPlayer.field_Internal_Static_VRCPlayer_0.ReloadAllAvatars();
            }, "Disabled", () => {
                Configuration.JSONConfig.FriendGlobalDynamicBonesEnabled = false;
                Configuration.SaveConfig();
                RefreshMenu();
                LoadMenu();
                VRCPlayer.field_Internal_Static_VRCPlayer_0.ReloadAllAvatars();
            }, "TOGGLE: Enables Global Dynamic Bones for friends. Note that this might cause lag with lots of friends in a room");
            EveryoneGlobalDynamicBones = new PageItem("Everybody Global\nDynamic Bones", () =>
            {
                Configuration.JSONConfig.EveryoneGlobalDynamicBonesEnabled = true;
                Configuration.JSONConfig.FriendGlobalDynamicBonesEnabled = false;
                Configuration.SaveConfig();
                RefreshMenu();
                LoadMenu();
                VRCPlayer.field_Internal_Static_VRCPlayer_0.ReloadAllAvatars();
            }, "Disabled", () =>
            {
                Configuration.JSONConfig.EveryoneGlobalDynamicBonesEnabled = false;
                Configuration.SaveConfig();
                RefreshMenu();
                LoadMenu();
                VRCPlayer.field_Internal_Static_VRCPlayer_0.ReloadAllAvatars();
            }, "TOGGLE: Enables Global Dynamic Bones for everyone. Note that this might cause lag in large instances");
            emmVRCNetwork = new PageItem("emmVRC Network\nEnabled", () =>
            {
                if (canToggleNetwork)
                {
                    Configuration.JSONConfig.emmVRCNetworkEnabled = true;
                    Configuration.SaveConfig();
                    RefreshMenu();
                    Network.NetworkClient.InitializeClient();
                    //Network.NetworkClient.PromptLogin();
                    MelonLoader.MelonCoroutines.Start(emmVRC.loadNetworked());
                }else
                {
                    VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowStandardPopup("emmVRC", "You must wait 5 seconds before toggling the network on again.", "Okay", new Action(() => { VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup();}));
                    emmVRCNetwork.SetToggleState(false, false);
                }
            }, "Disabled", () =>
            {
                Configuration.JSONConfig.emmVRCNetworkEnabled = false;
                Configuration.SaveConfig();
                RefreshMenu();
                if (Network.NetworkClient.authToken != null)
                    Network.HTTPRequest.get(Network.NetworkClient.baseURL + "/api/authentication/logout");
                Network.NetworkClient.authToken = null;
                canToggleNetwork = false;
                MelonLoader.MelonCoroutines.Start(WaitForDelayNetwork());
            }, "TOGGLE: Enables the emmVRC Network, which provides more functionality, like Global Chat and Messaging");
            GlobalChat = new PageItem("Global Chat", () =>
            {
                Configuration.JSONConfig.GlobalChatEnabled = true;
                Configuration.SaveConfig();
                RefreshMenu();
            }, "Disabled", () =>
            {
                Configuration.JSONConfig.GlobalChatEnabled = false;
                Configuration.SaveConfig();
                RefreshMenu();
            }, "TOGGLE: Enables the fetching and use of the Global Chat using the emmVRC Network", false); // TODO: Remove false at the end when emmVRC Network is ready
            ConsoleClean = new PageItem("Console\nCleaning", () =>
            {
                Configuration.JSONConfig.ConsoleClean = true;
                Configuration.SaveConfig();
                RefreshMenu();
            }, "Disabled", () =>
            {
                Configuration.JSONConfig.ConsoleClean = false;
                Configuration.SaveConfig();
                RefreshMenu();
            }, "TOGGLE: Hides some of VRChat's console messages, including PostOffice and \"This is happening\"");
            AvatarFavoriteList = new PageItem("emmVRC\nFavorite List", () =>
            {
                Configuration.JSONConfig.AvatarFavoritesEnabled = true;
                Configuration.SaveConfig();
                RefreshMenu();
            }, "Disabled", () =>
            {
                Configuration.JSONConfig.AvatarFavoritesEnabled = false;
                Configuration.SaveConfig();
                RefreshMenu();
            }, "TOGGLE: Enables the emmVRC Custom Avatar Favorite list, using the emmVRC Network"); 
            baseMenu.pageItems.Add(RiskyFunctions);
            baseMenu.pageItems.Add(emmVRCNetwork);
            baseMenu.pageItems.Add(AvatarFavoriteList);
            
            baseMenu.pageItems.Add(GlobalDynamicBones);
            baseMenu.pageItems.Add(FriendGlobalDynamicBones);
            baseMenu.pageItems.Add(EveryoneGlobalDynamicBones);
            baseMenu.pageItems.Add(VRFlightControls);
            //baseMenu.pageItems.Add(GlobalChat);
            baseMenu.pageItems.Add(ConsoleClean);
            baseMenu.pageItems.Add(PageItem.Space);

            InfoBar = new PageItem("Info Bar", () =>
            {
                Configuration.JSONConfig.InfoBarDisplayEnabled = true;
                Configuration.SaveConfig();
                RefreshMenu();
            }, "Disabled", () =>
            {
                Configuration.JSONConfig.InfoBarDisplayEnabled = false;
                Configuration.SaveConfig();
                RefreshMenu();
            }, "TOGGLE: Enable the info bar in the Quick Menu, which shows the current emmVRC version and network compatibility");
            Clock = new PageItem("Clock", () =>
            {
                Configuration.JSONConfig.ClockEnabled = true;
                Configuration.SaveConfig();
                RefreshMenu();
            }, "Disabled", () =>
            {
                Configuration.JSONConfig.ClockEnabled = false;
                Configuration.SaveConfig();
                RefreshMenu();
            }, "TOGGLE: Enables the clock in the Quick Menu, which shows your computer time, and instance time");
            MasterIcon = new PageItem("Master Icon", () =>
            {
                Configuration.JSONConfig.MasterIconEnabled = true;
                Configuration.SaveConfig();
                RefreshMenu();
            }, "Disabled", () =>
            {
                Configuration.JSONConfig.MasterIconEnabled = false;
                Configuration.SaveConfig();
                RefreshMenu();
            }, "TOGGLE: Enables the crown icon above the instance master");
            HUD = new PageItem("HUD", () => {
                Configuration.JSONConfig.HUDEnabled = true;
                Configuration.SaveConfig();
                RefreshMenu();
            }, "Disabled", () => {
                Configuration.JSONConfig.HUDEnabled = false;
                Configuration.SaveConfig();
                RefreshMenu();
            }, "TOGGLE: Enables the HUD, which shows players in the room and instance information");
            ChooseHUD = new PageItem("Show Quick Menu HUD\n in desktop", () => {
                if (!VRHUD.Initialized) {
                    VRHUD.Initialize();
                }
                DesktopHUD.enabled = false;
                VRHUD.enabled = true;
                Configuration.JSONConfig.VRHUDInDesktop = true;
                Configuration.SaveConfig();
                RefreshMenu();
            }, "Disabled", () => {
                if (!DesktopHUD.Initialized) {
                    DesktopHUD.Initialize();
                }
                VRHUD.enabled = false;
                DesktopHUD.enabled = true;
                Configuration.JSONConfig.VRHUDInDesktop = false;
                Configuration.SaveConfig();
                RefreshMenu();
            }, "TOGGLE: Switch between the Quick Menu HUD or normal HUD while in Desktop Mode");
            MoveVRHUD = new PageItem("Closer Quick Menu\nHUD", () => {
                Configuration.JSONConfig.MoveVRHUDIfSpaceFree = true;
                Configuration.SaveConfig();
                RefreshMenu();
            }, "Disabled", () => {
                Configuration.JSONConfig.MoveVRHUDIfSpaceFree = false;
                Configuration.SaveConfig();
                RefreshMenu();
            }, "TOGGLE: Allows the Quick Menu HUD to move inwards by one row if no emmVRC Buttons occupy the space (requires restart)");
            LogoButton = new PageItem("Logo Button", () => {
                Configuration.JSONConfig.LogoButtonEnabled = true;
                Configuration.SaveConfig();
                RefreshMenu();
                MelonLoader.MelonCoroutines.Start(Hacks.ShortcutMenuButtons.Process());
            }, "Disabled", () => {
                Configuration.JSONConfig.LogoButtonEnabled = false;
                Configuration.SaveConfig();
                RefreshMenu();
                MelonLoader.MelonCoroutines.Start(Hacks.ShortcutMenuButtons.Process());
            }, "TOGGLE: Enables the emmVRC Logo on your Quick Menu, that takes you to the Discord.");
            ForceRestart = new PageItem("Force Restart\non Loading Screen", () =>
            {
                Configuration.JSONConfig.ForceRestartButtonEnabled = true;
                Configuration.SaveConfig();
                RefreshMenu();
            }, "Disabled", () =>
            {
                Configuration.JSONConfig.ForceRestartButtonEnabled = false;
                Configuration.SaveConfig();
                RefreshMenu();
            }, "TOGGLE: Enables the Force Restart button on the loading screen, to help free you from softlocks");
            UnlimitedFPS = new PageItem("Unlimited FPS", () =>
            {
                Configuration.JSONConfig.UnlimitedFPSEnabled = true;
                Configuration.SaveConfig();
                RefreshMenu();
            }, "Disabled", () =>
            {
                Configuration.JSONConfig.UnlimitedFPSEnabled = false;
                Configuration.SaveConfig();
                RefreshMenu();
            }, "TOGGLE: Changes the VRChat FPS limit to 144, in desktop only.");
           
            baseMenu.pageItems.Add(InfoBar);
            baseMenu.pageItems.Add(Clock);
            baseMenu.pageItems.Add(MasterIcon);
            baseMenu.pageItems.Add(HUD);
            baseMenu.pageItems.Add(ChooseHUD);
            baseMenu.pageItems.Add(MoveVRHUD);
            baseMenu.pageItems.Add(LogoButton);
            baseMenu.pageItems.Add(ForceRestart);
            baseMenu.pageItems.Add(UnlimitedFPS);
            //baseMenu.pageItems.Add(PageItem.Space);

            UIColorChanging = new PageItem("UI Color\nChange", () =>
            {
                Configuration.JSONConfig.UIColorChangingEnabled = true;
                Configuration.SaveConfig();
                RefreshMenu();
                Hacks.ColorChanger.ApplyIfApplicable();
            }, "Disabled", () =>
            {
                Configuration.JSONConfig.UIColorChangingEnabled = false;
                Configuration.SaveConfig();
                RefreshMenu();
                Hacks.ColorChanger.ApplyIfApplicable();
            }, "TOGGLE: Enables the color changing module, which affects UI, ESP, and loading");
            UIColorChangePicker = new ColorPicker(baseMenu.menuBase.getMenuName(), 1001, 1000, "UI Color", "Select the color for the UI", (UnityEngine.Color newColor) =>
            {
                Configuration.JSONConfig.UIColorHex = ColorConversion.ColorToHex(newColor, true);
                Configuration.SaveConfig();
                LoadMenu();
                Hacks.ColorChanger.ApplyIfApplicable();
            }, () => { LoadMenu(); }, Libraries.ColorConversion.HexToColor(Configuration.JSONConfig.UIColorHex), Libraries.ColorConversion.HexToColor("#0EA6AD"));
            UIColorChangePickerButton = new PageItem("Select UI\nColor", () =>
            {
                QuickMenuUtils.ShowQuickmenuPage(UIColorChangePicker.baseMenu.getMenuName());
            }, "Selects the color splashed across the rest of the UI");
            InfoSpoofing = new PageItem("Local\nInfo Spoofing", () =>
            {
                if (Configuration.JSONConfig.InfoHidingEnabled)
                {
                    InfoHiding.SetToggleState(false, true);
                    RefreshMenu();
                }
                Configuration.JSONConfig.InfoSpoofingEnabled = true;
                Configuration.SaveConfig();
                RefreshMenu();
            }, "Disabled", () =>
            {
                Configuration.JSONConfig.InfoSpoofingEnabled = false;
                Configuration.SaveConfig();
                RefreshMenu();

                foreach (UnityEngine.UI.Text text in QuickMenuUtils.GetVRCUiMInstance().menuContent.GetComponentsInChildren<UnityEngine.UI.Text>())
                {
                    if (text.text.Contains("⛧⛧⛧⛧⛧⛧⛧⛧⛧") || text.text.Contains(Hacks.NameSpoofGenerator.spoofedName))
                    {
                        text.text = text.text.Replace("⛧⛧⛧⛧⛧⛧⛧⛧⛧", (VRC.Core.APIUser.CurrentUser.displayName == "" ? VRC.Core.APIUser.CurrentUser.username : VRC.Core.APIUser.CurrentUser.displayName));
                        text.text = text.text.Replace(Hacks.NameSpoofGenerator.spoofedName, (VRC.Core.APIUser.CurrentUser.displayName == "" ? VRC.Core.APIUser.CurrentUser.username : VRC.Core.APIUser.CurrentUser.displayName));
                    }
                }
                foreach (UnityEngine.UI.Text text in QuickMenuUtils.GetQuickMenuInstance().gameObject.GetComponentsInChildren<UnityEngine.UI.Text>())
                {
                    if (text.text.Contains("⛧⛧⛧⛧⛧⛧⛧⛧⛧") || text.text.Contains(Hacks.NameSpoofGenerator.spoofedName))
                    {
                        text.text = text.text.Replace("⛧⛧⛧⛧⛧⛧⛧⛧⛧", (VRC.Core.APIUser.CurrentUser.displayName == "" ? VRC.Core.APIUser.CurrentUser.username : VRC.Core.APIUser.CurrentUser.displayName));
                        text.text = text.text.Replace(Hacks.NameSpoofGenerator.spoofedName, (VRC.Core.APIUser.CurrentUser.displayName == "" ? VRC.Core.APIUser.CurrentUser.username : VRC.Core.APIUser.CurrentUser.displayName));
                    }
                }
            }, "TOGGLE: Enables local info spoofing, which can protect your identity in screenshots, recordings, and streams");
            //InfoSpooferNamePicker = new PageItem("")
            InfoHiding = new PageItem("Local\nInfo Hiding", () =>
            {
                if (Configuration.JSONConfig.InfoSpoofingEnabled)
                {
                    InfoSpoofing.SetToggleState(false, true);
                    RefreshMenu();
                }
                Configuration.JSONConfig.InfoHidingEnabled = true;
                Configuration.SaveConfig();
                RefreshMenu();
            }, "Disabled", () =>
            {
                Configuration.JSONConfig.InfoHidingEnabled = false;
                Configuration.SaveConfig();
                RefreshMenu();

                foreach (UnityEngine.UI.Text text in QuickMenuUtils.GetVRCUiMInstance().menuContent.GetComponentsInChildren<UnityEngine.UI.Text>())
                {
                    if (text.text.Contains("⛧⛧⛧⛧⛧⛧⛧⛧⛧") || text.text.Contains(Hacks.NameSpoofGenerator.spoofedName))
                    {
                        text.text = text.text.Replace("⛧⛧⛧⛧⛧⛧⛧⛧⛧", (VRC.Core.APIUser.CurrentUser.displayName == "" ? VRC.Core.APIUser.CurrentUser.username : VRC.Core.APIUser.CurrentUser.displayName));
                        text.text = text.text.Replace(Hacks.NameSpoofGenerator.spoofedName, (VRC.Core.APIUser.CurrentUser.displayName == "" ? VRC.Core.APIUser.CurrentUser.username : VRC.Core.APIUser.CurrentUser.displayName));
                    }
                }
                foreach (UnityEngine.UI.Text text in QuickMenuUtils.GetQuickMenuInstance().gameObject.GetComponentsInChildren<UnityEngine.UI.Text>())
                {
                    if (text.text.Contains("⛧⛧⛧⛧⛧⛧⛧⛧⛧") || text.text.Contains(Hacks.NameSpoofGenerator.spoofedName))
                    {
                        text.text = text.text.Replace("⛧⛧⛧⛧⛧⛧⛧⛧⛧", (VRC.Core.APIUser.CurrentUser.displayName == "" ? VRC.Core.APIUser.CurrentUser.username : VRC.Core.APIUser.CurrentUser.displayName));
                        text.text = text.text.Replace(Hacks.NameSpoofGenerator.spoofedName, (VRC.Core.APIUser.CurrentUser.displayName == "" ? VRC.Core.APIUser.CurrentUser.username : VRC.Core.APIUser.CurrentUser.displayName));
                    }
                }
            }, "TOGGLE: Enables local info hiding, which can protect your identity in screenshots, recordings, and streams");
            InfoSpooferNamePicker = new PageItem("F", () =>
            {
                VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowInputPopup("Enter your spoof name (or none for random)", "", UnityEngine.UI.InputField.InputType.Standard, false, "Accept", new System.Action<string, Il2CppSystem.Collections.Generic.List<UnityEngine.KeyCode>, UnityEngine.UI.Text>((string name, Il2CppSystem.Collections.Generic.List<UnityEngine.KeyCode> keyk, UnityEngine.UI.Text tx) =>
                {
                    if (Configuration.JSONConfig.InfoSpoofingEnabled || Configuration.JSONConfig.InfoHidingEnabled)
                    {
                        foreach (UnityEngine.UI.Text text in QuickMenuUtils.GetVRCUiMInstance().menuContent.GetComponentsInChildren<UnityEngine.UI.Text>())
                        {
                            if (text.text.Contains("⛧⛧⛧⛧⛧⛧⛧⛧⛧") || text.text.Contains(Hacks.NameSpoofGenerator.spoofedName))
                            {
                                text.text = text.text.Replace("⛧⛧⛧⛧⛧⛧⛧⛧⛧", (VRC.Core.APIUser.CurrentUser.displayName == "" ? VRC.Core.APIUser.CurrentUser.username : VRC.Core.APIUser.CurrentUser.displayName));
                                text.text = text.text.Replace(Hacks.NameSpoofGenerator.spoofedName, (VRC.Core.APIUser.CurrentUser.displayName == "" ? VRC.Core.APIUser.CurrentUser.username : VRC.Core.APIUser.CurrentUser.displayName));
                            }
                        }
                        foreach (UnityEngine.UI.Text text in QuickMenuUtils.GetQuickMenuInstance().gameObject.GetComponentsInChildren<UnityEngine.UI.Text>())
                        {
                            if (text.text.Contains("⛧⛧⛧⛧⛧⛧⛧⛧⛧") || text.text.Contains(Hacks.NameSpoofGenerator.spoofedName))
                            {
                                text.text = text.text.Replace("⛧⛧⛧⛧⛧⛧⛧⛧⛧", (VRC.Core.APIUser.CurrentUser.displayName == "" ? VRC.Core.APIUser.CurrentUser.username : VRC.Core.APIUser.CurrentUser.displayName));
                                text.text = text.text.Replace(Hacks.NameSpoofGenerator.spoofedName, (VRC.Core.APIUser.CurrentUser.displayName == "" ? VRC.Core.APIUser.CurrentUser.username : VRC.Core.APIUser.CurrentUser.displayName));
                            }
                        }
                    }
                    Configuration.JSONConfig.InfoSpoofingName = name;
                    Configuration.SaveConfig();
                    RefreshMenu();

                }), null, "Enter spoof name....");
            }, "Allows you to change your spoofed name to one that never changes");
            baseMenu.pageItems.Add(UIColorChanging);
            baseMenu.pageItems.Add(UIColorChangePickerButton);
            baseMenu.pageItems.Add(PageItem.Space);
            baseMenu.pageItems.Add(PageItem.Space);
            baseMenu.pageItems.Add(PageItem.Space);
            baseMenu.pageItems.Add(PageItem.Space);
            baseMenu.pageItems.Add(InfoSpoofing);
            baseMenu.pageItems.Add(InfoHiding);
            baseMenu.pageItems.Add(InfoSpooferNamePicker);

            FriendNameplateColorPicker = new ColorPicker(baseMenu.menuBase.getMenuName(), 1000, 1000, "Friend Nameplate Color", "Select the color for Friend Nameplate colors", (UnityEngine.Color newColor) =>
            {
                Configuration.JSONConfig.FriendNamePlateColorHex = ColorConversion.ColorToHex(newColor, true);
                Configuration.SaveConfig();
                LoadMenu();
                Hacks.Nameplates.colorChanged = true;
                VRCPlayer.field_Internal_Static_VRCPlayer_0.ReloadAllAvatars();
            }, () => { LoadMenu(); }, Libraries.ColorConversion.HexToColor(Configuration.JSONConfig.FriendNamePlateColorHex), Libraries.ColorConversion.HexToColor("#FFFF00"));
            VisitorNameplateColorPicker = new ColorPicker(baseMenu.menuBase.getMenuName(), 1000, 1001, "Visitor Nameplate Color", "Select the color for Visitor Nameplate colors", (UnityEngine.Color newColor) =>
            {
                Configuration.JSONConfig.VisitorNamePlateColorHex = ColorConversion.ColorToHex(newColor, true);
                Configuration.SaveConfig();
                LoadMenu();
                Hacks.Nameplates.colorChanged = true;
                VRCPlayer.field_Internal_Static_VRCPlayer_0.ReloadAllAvatars();
            }, () => { LoadMenu(); }, Libraries.ColorConversion.HexToColor(Configuration.JSONConfig.VisitorNamePlateColorHex), Libraries.ColorConversion.HexToColor("#CCCCCC"));
            NewUserNameplateColorPicker = new ColorPicker(baseMenu.menuBase.getMenuName(), 1000, 1002, "New User Nameplate Color", "Select the color for New User Nameplate colors", (UnityEngine.Color newColor) =>
            {
                Configuration.JSONConfig.NewUserNamePlateColorHex = ColorConversion.ColorToHex(newColor, true);
                Configuration.SaveConfig();
                LoadMenu();
                Hacks.Nameplates.colorChanged = true;
                VRCPlayer.field_Internal_Static_VRCPlayer_0.ReloadAllAvatars();
            }, () => { LoadMenu(); }, Libraries.ColorConversion.HexToColor(Configuration.JSONConfig.NewUserNamePlateColorHex), Libraries.ColorConversion.HexToColor("#1778FF"));
            UserNameplateColorPicker = new ColorPicker(baseMenu.menuBase.getMenuName(), 1000, 1003, "User Nameplate Color", "Select the color for User Nameplate colors", (UnityEngine.Color newColor) =>
            {
                Configuration.JSONConfig.UserNamePlateColorHex = ColorConversion.ColorToHex(newColor, true);
                Configuration.SaveConfig();
                LoadMenu();
                Hacks.Nameplates.colorChanged = true;
                VRCPlayer.field_Internal_Static_VRCPlayer_0.ReloadAllAvatars();
            }, () => { LoadMenu(); }, Libraries.ColorConversion.HexToColor(Configuration.JSONConfig.UserNamePlateColorHex), Libraries.ColorConversion.HexToColor("#2BCE5C"));
            KnownUserNameplateColorPicker = new ColorPicker(baseMenu.menuBase.getMenuName(), 1000, 1004, "Known User Nameplate Color", "Select the color for Known User Nameplate colors", (UnityEngine.Color newColor) =>
            {
                Configuration.JSONConfig.KnownUserNamePlateColorHex = ColorConversion.ColorToHex(newColor, true);
                Configuration.SaveConfig();
                LoadMenu();
                Hacks.Nameplates.colorChanged = true;
                VRCPlayer.field_Internal_Static_VRCPlayer_0.ReloadAllAvatars();
            }, () => { LoadMenu(); }, Libraries.ColorConversion.HexToColor(Configuration.JSONConfig.KnownUserNamePlateColorHex), Libraries.ColorConversion.HexToColor("#FF7B42"));
            TrustedUserNameplateColorPicker = new ColorPicker(baseMenu.menuBase.getMenuName(), 1000, 1005, "Trusted User Nameplate Color", "Select the color for Trusted User Nameplate colors", (UnityEngine.Color newColor) =>
            {
                Configuration.JSONConfig.TrustedUserNamePlateColorHex = ColorConversion.ColorToHex(newColor, true);
                Configuration.SaveConfig();
                LoadMenu();
                Hacks.Nameplates.colorChanged = true;
                VRCPlayer.field_Internal_Static_VRCPlayer_0.ReloadAllAvatars();
            }, () => { LoadMenu(); }, Libraries.ColorConversion.HexToColor(Configuration.JSONConfig.TrustedUserNamePlateColorHex), Libraries.ColorConversion.HexToColor("#8143E6"));
            NameplateColorChanging = new PageItem("Nameplate\nColor Changing", () =>
            {
                Configuration.JSONConfig.NameplateColorChangingEnabled = true;
                Configuration.SaveConfig();
                RefreshMenu();
                Hacks.Nameplates.colorChanged = true;
                VRCPlayer.field_Internal_Static_VRCPlayer_0.ReloadAllAvatars();
            }, "Disabled", () =>
            {
                Configuration.JSONConfig.NameplateColorChangingEnabled = false;
                Configuration.SaveConfig();
                RefreshMenu();
                Hacks.Nameplates.colorChanged = true;
                VRCPlayer.field_Internal_Static_VRCPlayer_0.ReloadAllAvatars();
            }, "TOGGLE: Enables the nameplate color changing module, which changes the colors of the various trust ranks in nameplates and the UI");
            FriendNameplateColorPickerButton = new PageItem("Friend\nNameplate\nColor", () => { QuickMenuUtils.ShowQuickmenuPage(FriendNameplateColorPicker.baseMenu.getMenuName()); }, "Select the color for Friend Nameplate colors");
            VisitorNameplateColorPickerButton = new PageItem("Visitor\nNameplate\nColor", () => { QuickMenuUtils.ShowQuickmenuPage(VisitorNameplateColorPicker.baseMenu.getMenuName()); }, "Select the color for Visitor Nameplate colors");
            NewUserNameplateColorPickerButton = new PageItem("New User\nNameplate\nColor", () => { QuickMenuUtils.ShowQuickmenuPage(NewUserNameplateColorPicker.baseMenu.getMenuName()); }, "Select the color for New User Nameplate colors");
            UserNameplateColorPickerButton = new PageItem("User\nNameplate\nColor", () => { QuickMenuUtils.ShowQuickmenuPage(UserNameplateColorPicker.baseMenu.getMenuName()); }, "Select the color for User Nameplate colors");
            KnownUserNameplateColorPickerButton = new PageItem("Known User\nNameplate\nColor", () => { QuickMenuUtils.ShowQuickmenuPage(KnownUserNameplateColorPicker.baseMenu.getMenuName()); }, "Select the color for Known User Nameplate colors");
            TrustedUserNameplateColorPickerButton = new PageItem("Trusted User\nNameplate\nColor", () => { QuickMenuUtils.ShowQuickmenuPage(TrustedUserNameplateColorPicker.baseMenu.getMenuName()); }, "Select the color for Trusted User Nameplate colors");
            baseMenu.pageItems.Add(NameplateColorChanging);
            baseMenu.pageItems.Add(PageItem.Space);
            baseMenu.pageItems.Add(PageItem.Space);
            baseMenu.pageItems.Add(FriendNameplateColorPickerButton);
            baseMenu.pageItems.Add(VisitorNameplateColorPickerButton);
            baseMenu.pageItems.Add(NewUserNameplateColorPickerButton);
            baseMenu.pageItems.Add(UserNameplateColorPickerButton);
            baseMenu.pageItems.Add(KnownUserNameplateColorPickerButton);
            baseMenu.pageItems.Add(TrustedUserNameplateColorPickerButton);

            DisableReportWorld = new PageItem("Disable\nReport World", () =>
            {
                Configuration.JSONConfig.DisableReportWorldButton = true;
                Configuration.SaveConfig();
                RefreshMenu();
                MelonLoader.MelonCoroutines.Start(Hacks.ShortcutMenuButtons.Process());
            }, "Enabled", () =>
            {
                Configuration.JSONConfig.DisableReportWorldButton = false;
                Configuration.SaveConfig();
                RefreshMenu();
                MelonLoader.MelonCoroutines.Start(Hacks.ShortcutMenuButtons.Process());
            }, "TOGGLE: Disables the 'Report World' button in the Quick Menu. Its functionality can be found in the Worlds menu, and the Disabled Buttons menu");
            DisableEmoji = new PageItem("Disable\nEmoji", () =>
            {
                Configuration.JSONConfig.DisableEmojiButton = true;
                Configuration.SaveConfig();
                RefreshMenu();
                MelonLoader.MelonCoroutines.Start(Hacks.ShortcutMenuButtons.Process());
            }, "Enabled", () =>
            {
                Configuration.JSONConfig.DisableEmojiButton = false;
                Configuration.SaveConfig();
                RefreshMenu();
                MelonLoader.MelonCoroutines.Start(Hacks.ShortcutMenuButtons.Process());
            }, "TOGGLE: Disables the 'Emoji' button in the Quick Menu. Its functionality can be found in the Disabled Buttons menu, as well as the new Radial menu");
            DisableEmote = new PageItem("Disable\nEmote", () =>
            {
                Configuration.JSONConfig.DisableEmoteButton = true;
                Configuration.SaveConfig();
                RefreshMenu();
                MelonLoader.MelonCoroutines.Start(Hacks.ShortcutMenuButtons.Process());
            }, "Enabled", () =>
            {
                Configuration.JSONConfig.DisableEmoteButton = false;
                Configuration.SaveConfig();
                RefreshMenu();
                MelonLoader.MelonCoroutines.Start(Hacks.ShortcutMenuButtons.Process());
            }, "TOGGLE: Disables the 'Emote' button in the Quick Menu. Its functionality can be found in the new Radial menu");
            DisableRankToggle = new PageItem("Disable\nRank Toggle", () =>
            {
                Configuration.JSONConfig.DisableRankToggleButton = true;
                Configuration.SaveConfig();
                RefreshMenu();
                MelonLoader.MelonCoroutines.Start(Hacks.ShortcutMenuButtons.Process());
            }, "Enabled", () =>
            {
                Configuration.JSONConfig.DisableRankToggleButton = false;
                Configuration.SaveConfig();
                RefreshMenu();
                MelonLoader.MelonCoroutines.Start(Hacks.ShortcutMenuButtons.Process());
            }, "TOGGLE: Disables the 'Rank Toggle' switch in the Quick Menu. Its functionality can be found in the Disabled Buttons menu");
            DisableReportUser = new PageItem("Disable\nReport User", () =>
            {
                Configuration.JSONConfig.DisableReportUserButton = true;
                Configuration.SaveConfig();
                RefreshMenu();
                UserInteractMenuButtons.Initialize();
            }, "Enabled", () =>
            {
                Configuration.JSONConfig.DisableReportUserButton = false;
                Configuration.SaveConfig();
                RefreshMenu();
                UserInteractMenuButtons.Initialize();
            }, "TOGGLE: Disables the 'Report User' button in the User Interact Menu. Its functionality can be found in the Social menu"); 
            DisablePlaylists = new PageItem("Disable\nPlaylists", () =>
            {
                Configuration.JSONConfig.DisablePlaylistsButton = true;
                Configuration.SaveConfig();
                RefreshMenu();
                UserInteractMenuButtons.Initialize();
            }, "Enabled", () =>
            {
                Configuration.JSONConfig.DisablePlaylistsButton = false;
                Configuration.SaveConfig();
                RefreshMenu();
                UserInteractMenuButtons.Initialize();
            }, "TOGGLE: Disables the 'Playlists' button in the User Interact Menu. Its functionality can be found in the Social menu"); 
            DisableAvatarStats = new PageItem("Disable\nAvatar Stats", () =>
            {
                Configuration.JSONConfig.DisableAvatarStatsButton = true;
                Configuration.SaveConfig();
                RefreshMenu();
                UserInteractMenuButtons.Initialize();
            }, "Enabled", () =>
            {
                Configuration.JSONConfig.DisableAvatarStatsButton = false;
                Configuration.SaveConfig();
                RefreshMenu();
                UserInteractMenuButtons.Initialize();
            }, "TOGGLE: Disables the 'Avatar Stats' button in the User Interact Menu.");
            MinimalWarnKick = new PageItem("Minimal Warn\nKick Button", () =>
            {
                Configuration.JSONConfig.MinimalWarnKickButton = true;
                Configuration.SaveConfig();
                RefreshMenu();
                UserInteractMenuButtons.Initialize();
            }, "Disabled", () =>
            {
                Configuration.JSONConfig.MinimalWarnKickButton = false;
                Configuration.SaveConfig();
                RefreshMenu();
                UserInteractMenuButtons.Initialize();
            }, "TOGGLE: Combines the Warn and Kick buttons into one space, to make room for more buttons");
            baseMenu.pageItems.Add(DisableReportWorld);
            baseMenu.pageItems.Add(DisableEmoji);
            baseMenu.pageItems.Add(DisableEmote);
            baseMenu.pageItems.Add(DisableRankToggle);
            baseMenu.pageItems.Add(DisablePlaylists);
            baseMenu.pageItems.Add(DisableReportUser);
            baseMenu.pageItems.Add(DisableAvatarStats);
            baseMenu.pageItems.Add(MinimalWarnKick);
            baseMenu.pageItems.Add(PageItem.Space);

            DisableAvatarHotWorlds = new PageItem("Disable Hot Avatar\nWorld List", () =>
            {
                Configuration.JSONConfig.DisableAvatarHotWorlds = true;
                Configuration.SaveConfig();
                RefreshMenu();
            }, "Enabled", () =>
            {
                Configuration.JSONConfig.DisableAvatarHotWorlds = false;
                Configuration.SaveConfig();
                RefreshMenu();
            }, "TOGGLE: Disables the 'Hot Avatar Worlds' list in your Avatars menu");
            DisableAvatarRandomWorlds = new PageItem("Disable Random\nAvatar World List", () =>
            {
                Configuration.JSONConfig.DisableAvatarRandomWorlds = true;
                Configuration.SaveConfig();
                RefreshMenu();
            }, "Enabled", () =>
            {
                Configuration.JSONConfig.DisableAvatarRandomWorlds = false;
                Configuration.SaveConfig();
                RefreshMenu();
            }, "TOGGLE: Disables the 'Random Avatar Worlds' list in your Avatars menu");
            DisableAvatarLegacyList = new PageItem("Disable Legacy\nAvatar List", () =>
            {
                Configuration.JSONConfig.DisableAvatarLegacy = true;
                Configuration.SaveConfig();
                RefreshMenu();
            }, "Enabled", () =>
            {
                Configuration.JSONConfig.DisableAvatarLegacy = false;
                Configuration.SaveConfig();
                RefreshMenu();
            }, "TOGGLE: Disables the 'Legacy Avatars' list in your Avatars menu");
            DisableAvatarPublicList = new PageItem("Disable Public\nAvatar List", () =>
            {
                Configuration.JSONConfig.DisableAvatarPublic = true;
                Configuration.SaveConfig();
                RefreshMenu();
            }, "Enabled", () =>
            {
                Configuration.JSONConfig.DisableAvatarPublic = false;
                Configuration.SaveConfig();
                RefreshMenu();
            }, "TOGGLE: Disables the 'Public Avatars' list in your Avatars menu");
            baseMenu.pageItems.Add(DisableAvatarHotWorlds);
            baseMenu.pageItems.Add(DisableAvatarRandomWorlds);
            baseMenu.pageItems.Add(DisableAvatarLegacyList);
            baseMenu.pageItems.Add(DisableAvatarPublicList);
            baseMenu.pageItems.Add(PageItem.Space);
            baseMenu.pageItems.Add(PageItem.Space);
            baseMenu.pageItems.Add(PageItem.Space);
            baseMenu.pageItems.Add(PageItem.Space);
            baseMenu.pageItems.Add(PageItem.Space);


            FlightKeybind = new PageItem("Flight\nKeybind:\nLeftCTRL + F", () =>
            {
                KeybindChanger.Show("Please press a keybind for Flight:", (UnityEngine.KeyCode mainKey, UnityEngine.KeyCode modifier) =>
                {
                    Configuration.JSONConfig.FlightKeybind[0] = (int)mainKey;
                    Configuration.JSONConfig.FlightKeybind[1] = (int)modifier;
                    Configuration.SaveConfig();
                    LoadMenu();
                }, () =>
                {
                    LoadMenu();
                });
            }, "Change the keybind for flying (Default is LeftCTRL + F");
            NoclipKeybind = new PageItem("Noclip\nKeybind:\nLeftCTRL + M", () =>
            {
                KeybindChanger.Show("Please press a keybind for Noclip:", (UnityEngine.KeyCode mainKey, UnityEngine.KeyCode modifier) =>
                {
                    Configuration.JSONConfig.NoclipKeybind[0] = (int)mainKey;
                    Configuration.JSONConfig.NoclipKeybind[1] = (int)modifier;
                    Configuration.SaveConfig();
                    LoadMenu();
                }, () =>
                {
                    LoadMenu();
                });
            }, "Change the keybind for noclip (Default is LeftCTRL + M");
            SpeedKeybind = new PageItem("Speed\nKeybind:\nLeftCTRL + G", () =>
            {
                KeybindChanger.Show("Please press a keybind for Speed:", (UnityEngine.KeyCode mainKey, UnityEngine.KeyCode modifier) =>
                {
                    Configuration.JSONConfig.SpeedKeybind[0] = (int)mainKey;
                    Configuration.JSONConfig.SpeedKeybind[1] = (int)modifier;
                    Configuration.SaveConfig();
                    LoadMenu();
                }, () =>
                {
                    LoadMenu();
                });
            }, "Change the keybind for speed (Default is LeftCTRL + G");
            ThirdPersonKeybind = new PageItem("Third\nPerson\nKeybind:\nLeftCTRL + T", () =>
            {
                KeybindChanger.Show("Please press a keybind for Third Person:", (UnityEngine.KeyCode mainKey, UnityEngine.KeyCode modifier) =>
                {
                    Configuration.JSONConfig.ThirdPersonKeybind[0] = (int)mainKey;
                    Configuration.JSONConfig.ThirdPersonKeybind[1] = (int)modifier;
                    Configuration.SaveConfig();
                    LoadMenu();
                }, () =>
                {
                    LoadMenu();
                });
            }, "Change the keybind for third person (Default is LeftCTRL + T");
            ToggleHUDEnabledKeybind = new PageItem("Toggle HUD\nEnabled\nKeybind:\nLeftCTRL + J", () =>
            {
                KeybindChanger.Show("Please press a keybind for toggling the HUD:", (UnityEngine.KeyCode mainKey, UnityEngine.KeyCode modifier) =>
                {
                    Configuration.JSONConfig.ToggleHUDEnabledKeybind[0] = (int)mainKey;
                    Configuration.JSONConfig.ToggleHUDEnabledKeybind[1] = (int)modifier;
                    Configuration.SaveConfig();
                    LoadMenu();
                }, () =>
                {
                    LoadMenu();
                });
            }, "Change the keybind for toggling the HUD on and off (Default is LeftCTRL + J");
            RespawnKeybind = new PageItem("Respawn\nKeybind:\nLeftCTRL + Y", () =>
            {
                KeybindChanger.Show("Please press a keybind for respawning:", (UnityEngine.KeyCode mainKey, UnityEngine.KeyCode modifier) =>
                {
                    Configuration.JSONConfig.RespawnKeybind[0] = (int)mainKey;
                    Configuration.JSONConfig.RespawnKeybind[1] = (int)modifier;
                    Configuration.SaveConfig();
                    LoadMenu();
                }, () =>
                {
                    LoadMenu();
                });
            }, "Change the keybind for respawning (Default is LeftCTRL + Y");
            GoHomeKeybind = new PageItem("Go Home\nKeybind:\nLeftCTRL + U", () =>
            {
                KeybindChanger.Show("Please press a keybind for going home:", (UnityEngine.KeyCode mainKey, UnityEngine.KeyCode modifier) =>
                {
                    Configuration.JSONConfig.GoHomeKeybind[0] = (int)mainKey;
                    Configuration.JSONConfig.GoHomeKeybind[1] = (int)modifier;
                    Configuration.SaveConfig();
                    LoadMenu();
                }, () =>
                {
                    LoadMenu();
                });
            }, "Change the keybind for going to your home world (Default is LeftCTRL + U");
            baseMenu.pageItems.Add(FlightKeybind);
            baseMenu.pageItems.Add(NoclipKeybind);
            baseMenu.pageItems.Add(SpeedKeybind);
            baseMenu.pageItems.Add(ThirdPersonKeybind);
            baseMenu.pageItems.Add(ToggleHUDEnabledKeybind);
            baseMenu.pageItems.Add(RespawnKeybind);
            baseMenu.pageItems.Add(GoHomeKeybind);


            baseMenu.pageTitles.Add("Core Features");
            baseMenu.pageTitles.Add("Visual Features");
            baseMenu.pageTitles.Add("UI Changing");
            baseMenu.pageTitles.Add("Nameplate Color Changing");
            baseMenu.pageTitles.Add("Disable VRChat Buttons");
            baseMenu.pageTitles.Add("Disable Avatar Menu Lists");
            baseMenu.pageTitles.Add("Keybinds");
        }
        public static void RefreshMenu()
        {
            try
            {
                RiskyFunctions.SetToggleState(Configuration.JSONConfig.RiskyFunctionsEnabled);
                VRFlightControls.SetToggleState(Configuration.JSONConfig.VRFlightControls);
                GlobalDynamicBones.SetToggleState(Configuration.JSONConfig.GlobalDynamicBonesEnabled);
                FriendGlobalDynamicBones.SetToggleState(Configuration.JSONConfig.FriendGlobalDynamicBonesEnabled);
                EveryoneGlobalDynamicBones.SetToggleState(Configuration.JSONConfig.EveryoneGlobalDynamicBonesEnabled);
                emmVRCNetwork.SetToggleState(Configuration.JSONConfig.emmVRCNetworkEnabled);
                GlobalChat.SetToggleState(Configuration.JSONConfig.GlobalChatEnabled);
                AvatarFavoriteList.SetToggleState(Configuration.JSONConfig.AvatarFavoritesEnabled);
                ConsoleClean.SetToggleState(Configuration.JSONConfig.ConsoleClean);
                
                InfoBar.SetToggleState(Configuration.JSONConfig.InfoBarDisplayEnabled);
                Clock.SetToggleState(Configuration.JSONConfig.ClockEnabled);
                MasterIcon.SetToggleState(Configuration.JSONConfig.MasterIconEnabled);
                HUD.SetToggleState(Configuration.JSONConfig.HUDEnabled);
                ChooseHUD.SetToggleState(Configuration.JSONConfig.VRHUDInDesktop);
                MoveVRHUD.SetToggleState(Configuration.JSONConfig.MoveVRHUDIfSpaceFree);
                LogoButton.SetToggleState(Configuration.JSONConfig.LogoButtonEnabled);
                ForceRestart.SetToggleState(Configuration.JSONConfig.ForceRestartButtonEnabled);
                UnlimitedFPS.SetToggleState(Configuration.JSONConfig.UnlimitedFPSEnabled);

                UIColorChanging.SetToggleState(Configuration.JSONConfig.UIColorChangingEnabled);
                InfoSpoofing.SetToggleState(Configuration.JSONConfig.InfoSpoofingEnabled);
                InfoHiding.SetToggleState(Configuration.JSONConfig.InfoHidingEnabled);
                InfoSpooferNamePicker.Name = "Set\nSpoofed\nName";

                NameplateColorChanging.SetToggleState(Configuration.JSONConfig.NameplateColorChangingEnabled);

                DisableReportWorld.SetToggleState(Configuration.JSONConfig.DisableReportWorldButton);
                DisableEmoji.SetToggleState(Configuration.JSONConfig.DisableEmojiButton);
                DisableEmote.SetToggleState(Configuration.JSONConfig.DisableEmoteButton);
                DisableRankToggle.SetToggleState(Configuration.JSONConfig.DisableRankToggleButton);
                DisablePlaylists.SetToggleState(Configuration.JSONConfig.DisablePlaylistsButton);
                DisableAvatarStats.SetToggleState(Configuration.JSONConfig.DisableAvatarStatsButton);
                DisableReportUser.SetToggleState(Configuration.JSONConfig.DisableReportUserButton);
                MinimalWarnKick.SetToggleState(Configuration.JSONConfig.MinimalWarnKickButton);

                DisableAvatarHotWorlds.SetToggleState(Configuration.JSONConfig.DisableAvatarHotWorlds);
                DisableAvatarRandomWorlds.SetToggleState(Configuration.JSONConfig.DisableAvatarRandomWorlds);
                DisableAvatarLegacyList.SetToggleState(Configuration.JSONConfig.DisableAvatarLegacy);
                DisableAvatarPublicList.SetToggleState(Configuration.JSONConfig.DisableAvatarPublic);

                FlightKeybind.Name = "Flight:\n" + (((UnityEngine.KeyCode)Configuration.JSONConfig.FlightKeybind[1] != UnityEngine.KeyCode.None ? KeyCodeConversion.Stringify(((UnityEngine.KeyCode)Configuration.JSONConfig.FlightKeybind[1])) + "+" : "") + (KeyCodeConversion.Stringify((UnityEngine.KeyCode)Configuration.JSONConfig.FlightKeybind[0])));
                NoclipKeybind.Name = "Noclip:\n" + (((UnityEngine.KeyCode)Configuration.JSONConfig.NoclipKeybind[1] != UnityEngine.KeyCode.None ? KeyCodeConversion.Stringify(((UnityEngine.KeyCode)Configuration.JSONConfig.NoclipKeybind[1])) + "+" : "") + (KeyCodeConversion.Stringify((UnityEngine.KeyCode)Configuration.JSONConfig.NoclipKeybind[0])));
                SpeedKeybind.Name = "Speed:\n" + (((UnityEngine.KeyCode)Configuration.JSONConfig.SpeedKeybind[1] != UnityEngine.KeyCode.None ? KeyCodeConversion.Stringify(((UnityEngine.KeyCode)Configuration.JSONConfig.SpeedKeybind[1])) + "+" : "") + (KeyCodeConversion.Stringify((UnityEngine.KeyCode)Configuration.JSONConfig.SpeedKeybind[0])));
                ThirdPersonKeybind.Name = "Third\nPerson:\n" + (((UnityEngine.KeyCode)Configuration.JSONConfig.ThirdPersonKeybind[1] != UnityEngine.KeyCode.None ? KeyCodeConversion.Stringify(((UnityEngine.KeyCode)Configuration.JSONConfig.ThirdPersonKeybind[1])) + "+" : "") + (KeyCodeConversion.Stringify((UnityEngine.KeyCode)Configuration.JSONConfig.ThirdPersonKeybind[0])));
                ToggleHUDEnabledKeybind.Name = "Toggle HUD\nEnabled:\n" + (((UnityEngine.KeyCode)Configuration.JSONConfig.ToggleHUDEnabledKeybind[1] != UnityEngine.KeyCode.None ? KeyCodeConversion.Stringify(((UnityEngine.KeyCode)Configuration.JSONConfig.ToggleHUDEnabledKeybind[1])) + "+" : "") + (KeyCodeConversion.Stringify((UnityEngine.KeyCode)Configuration.JSONConfig.ToggleHUDEnabledKeybind[0])));
                RespawnKeybind.Name = "Respawn:\n"+ (((UnityEngine.KeyCode)Configuration.JSONConfig.RespawnKeybind[1] != UnityEngine.KeyCode.None ? KeyCodeConversion.Stringify(((UnityEngine.KeyCode)Configuration.JSONConfig.RespawnKeybind[1])) + "+" : "") + (KeyCodeConversion.Stringify((UnityEngine.KeyCode)Configuration.JSONConfig.RespawnKeybind[0])));
                GoHomeKeybind.Name = "Go Home:\n" + (((UnityEngine.KeyCode)Configuration.JSONConfig.GoHomeKeybind[1] != UnityEngine.KeyCode.None ? KeyCodeConversion.Stringify(((UnityEngine.KeyCode)Configuration.JSONConfig.GoHomeKeybind[1])) + "+" : "") + (KeyCodeConversion.Stringify((UnityEngine.KeyCode)Configuration.JSONConfig.GoHomeKeybind[0])));


            }
            catch (Exception ex)
            {
                emmVRCLoader.Logger.LogError("Error: " + ex.ToString());
            }
        }
        public static void LoadMenu()
        {
            RefreshMenu();
            baseMenu.OpenMenu();
        }
        public static IEnumerator WaitForDelayNetwork()
        {
            yield return new WaitForSeconds(5f);
            canToggleNetwork = true;
        }
    }
}
