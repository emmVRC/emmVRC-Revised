using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Events;
using emmVRC.Libraries;
using emmVRC.Hacks;
using emmVRC.Managers;
using UnityEngine;
using UnityEngine.UI;
using emmVRC.Objects;
using VRC.Core;
//using TMPro;
using VRC;

#pragma warning disable 4014

namespace emmVRC.Menus
{
    public class SettingsMenu
    {
        // Base menu for the Settings menu
        public static PaginatedMenu baseMenu;
        public static QMSingleButton ResetSettingsButton;

        // Page 1 - Core Features

        private static PageItem VRFlightControls;
        private static PageItem GlobalDynamicBones;
        private static PageItem FriendGlobalDynamicBones;
        private static PageItem EveryoneGlobalDynamicBones;
        private static PageItem UIExpansionKitIntegration;
        private static PageItem TrackingSaving;
        private static PageItem ActionMenuIntegration;
        private static PageItem EmojiFavouriteMenu;

        // Page 2 - Visual Features
        private static PageItem RiskyFunctions;
        private static PageItem InfoBar;
        private static PageItem Clock;
        private static PageItem MasterIcon;
        private static PageItem LogoButton;
        private static PageItem HUD;
        private static PageItem ChooseHUD;
        private static PageItem MoveVRHUD;
        private static PageItem ForceRestart;
        private static PageItem UnlimitedFPS;

        // Page 3 - Network Features
        private static PageItem emmVRCNetwork;
        private static PageItem GlobalChat;
        private static PageItem AvatarFavoriteList;
        private static PageItem AvatarFavoriteJumpToStart;

        // Page 4 - Button Positions
        private static PageItem FunctionsMenuPosition;
        private static PageItem LogoPosition;
        private static PageItem NotificationPosition;
        private static ButtonConfigurationMenu FunctionsMenuPositionMenu;
        private static ButtonConfigurationMenu LogoPositionMenu;
        private static ButtonConfigurationMenu NotificationPositionMenu;
        private static PageItem TabMode;
        private static PageItem StealthMode;

        // Page 5 - UI Color Changing
        private static PageItem UIColorChanging;
        private static PageItem UIColorChangePickerButton;
        private static ColorPicker UIColorChangePicker;
        private static PageItem UIActionMenuColorChanging;
        private static PageItem UIMicIconColorChanging;
        private static PageItem UIMicIconPulse;
        private static PageItem UIExpansionKitColorChanging;
        private static PageItem InfoSpoofing;
        private static PageItem InfoSpooferNamePicker;

        // Page 5 - Nameplate Color Changing (pretty much useless)
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
        private static PageItem VeteranUserNameplateColorPickerButton;
        private static ColorPicker VeteranUserNameplateColorPicker;
        private static PageItem LegendaryUserNameplateColorPickerButton;
        private static ColorPicker LegendaryUserNameplateColorPicker;

        // Page 6 - Disable Quick Menu buttons
        private static PageItem DisableReportWorld;
        private static PageItem DisableEmoji;
        private static PageItem DisableEmote;
        private static PageItem DisableRankToggle;
        private static PageItem DisableOldInviteButtons;

        // Page 7 - Disable VRChat buttons
        private static PageItem DisablePlaylists;
        private static PageItem DisableAvatarStats;
        private static PageItem DisableReportUser;
        private static PageItem MinimalWarnKick;
        private static PageItem DisableOneHandMovement;
        private static PageItem DisableMicTooltip;

        // Page 8 - Disable VRC+ Stuff
        private static PageItem DisableVRCPlusAds;
        private static PageItem DisableVRCPlusQMButtons;
        private static PageItem DisableVRCPlusMenuTabs;
        private static PageItem DisableVRCPlusUserInfo;

        // Page 9 - Disable Avatar Lists
        private static PageItem DisableAvatarHotWorlds;
        private static PageItem DisableAvatarRandomWorlds;
        private static PageItem DisableAvatarPersonalList;
        private static PageItem DisableAvatarLegacyList;
        private static PageItem DisableAvatarPublicList;

        // Page 10 - Keybind Configuration
        private static PageItem EnableKeybinds;
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
                }), "No", new System.Action(() =>
                {
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
                    RiskyFunctionsManager.CheckThisWrld().NoAwait(nameof(RiskyFunctionsManager.CheckThisWrld));
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
                    RiskyFunctionsManager.CheckThisWrld().NoAwait(nameof(RiskyFunctionsManager.CheckThisWrld));
                    RefreshMenu();
                }
            }, "Disabled", () =>
            {
                Configuration.JSONConfig.RiskyFunctionsEnabled = false;
                Configuration.SaveConfig();
                RefreshMenu();
                PlayerTweaksMenu.SetRiskyFuncsAllowed(false);
                if (Flight.FlightEnabled || Flight.NoclipEnabled)
                    PlayerTweaksMenu.FlightToggle.setToggleState(false, true);
                if (Speed.SpeedModified)
                    PlayerTweaksMenu.SpeedToggle.setToggleState(false, true);
                if (ESP.ESPEnabled)
                    PlayerTweaksMenu.ESPToggle.setToggleState(false, true);
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
                if (!Libraries.ModCompatibility.MultiplayerDynamicBones)
                {
                    Configuration.JSONConfig.GlobalDynamicBonesEnabled = true;
                    Configuration.SaveConfig();
                    RefreshMenu();
                    VRCPlayer.field_Internal_Static_VRCPlayer_0.ReloadAllAvatars();
                }
                else
                {
                    VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowStandardPopup("emmVRC", "Global Dynamic Bones are not compatible with MultiplayerDynamicBones. You must remove this mod to use Global Dynamic Bones.", "Dismiss", VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup);
                }
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
            }, "Disabled", () =>
            {
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
            UIExpansionKitIntegration = new PageItem("UI Expansion\nKit Integration", () =>
            {
                Configuration.JSONConfig.UIExpansionKitIntegration = true;
                Configuration.SaveConfig();
                if (ModCompatibility.FlightButton != null)
                    ModCompatibility.FlightButton.SetActive(true);
                if (ModCompatibility.NoclipButton != null)
                    ModCompatibility.NoclipButton.SetActive(true);
                if (ModCompatibility.SpeedButton != null)
                    ModCompatibility.SpeedButton.SetActive(true);
                if (ModCompatibility.ESPButton != null)
                    ModCompatibility.ESPButton.SetActive(true);
                RefreshMenu();
            }, "Disabled", () =>
            {
                Configuration.JSONConfig.UIExpansionKitIntegration = false;
                Configuration.SaveConfig();
                if (ModCompatibility.FlightButton != null)
                    ModCompatibility.FlightButton.SetActive(false);
                if (ModCompatibility.NoclipButton != null)
                    ModCompatibility.NoclipButton.SetActive(false);
                if (ModCompatibility.SpeedButton != null)
                    ModCompatibility.SpeedButton.SetActive(false);
                if (ModCompatibility.ESPButton != null)
                    ModCompatibility.ESPButton.SetActive(false);
                RefreshMenu();
            }, "TOGGLE: Shows the Risky Functions buttons in the UI Expansion Kit menu", ModCompatibility.UIExpansionKit);
            TrackingSaving = new PageItem("Calibration\nSaving", () =>
            {
                Configuration.JSONConfig.TrackingSaving = true;
                Configuration.SaveConfig();
            }, "Disabled", () =>
            {
                Configuration.JSONConfig.TrackingSaving = false;
                Configuration.SaveConfig();
            }, "TOGGLE: Saves calibration between avatar switches for Full Body Tracking");
            ActionMenuIntegration = new PageItem("Action Menu\nIntegration", () =>
            {
                Configuration.JSONConfig.ActionMenuIntegration = true;
                Configuration.SaveConfig();
            }, "Disabled", () =>
            {
                Configuration.JSONConfig.ActionMenuIntegration = false;
                Configuration.SaveConfig();
            }, "TOGGLE: Integrates an emmVRC menu into the Action (Radial) Menu, for easy access");
            EmojiFavouriteMenu = new PageItem("Emoji\nFavorites", () =>
            {
                QuickMenuUtils.ShowQuickmenuPage(Hacks.EmojiFavourites.baseMenu.getMenuName());
            }, "Configure your Emoji favorites, accessible from the Action Menu");

            baseMenu.pageItems.Add(RiskyFunctions);
            baseMenu.pageItems.Add(GlobalDynamicBones);
            baseMenu.pageItems.Add(FriendGlobalDynamicBones);
            baseMenu.pageItems.Add(EveryoneGlobalDynamicBones);
            baseMenu.pageItems.Add(VRFlightControls);
            baseMenu.pageItems.Add(ActionMenuIntegration);
            if (!ModCompatibility.FBTSaver)
                baseMenu.pageItems.Add(TrackingSaving);
            else
                baseMenu.pageItems.Add(PageItem.Space);
            if (ModCompatibility.UIExpansionKit)
                baseMenu.pageItems.Add(UIExpansionKitIntegration);
            else
                baseMenu.pageItems.Add(PageItem.Space);
            if (!Environment.CurrentDirectory.Contains("vrchat-vrchat")) // Yet another crusty Oculus check. This one can go bye-bye when EmojiGenerator is in deob
                baseMenu.pageItems.Add(EmojiFavouriteMenu);
            else
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
            HUD = new PageItem("HUD", () =>
            {
                Configuration.JSONConfig.HUDEnabled = true;
                Configuration.SaveConfig();
                RefreshMenu();
            }, "Disabled", () =>
            {
                Configuration.JSONConfig.HUDEnabled = false;
                Configuration.SaveConfig();
                RefreshMenu();
            }, "TOGGLE: Enables the HUD, which shows players in the room and instance information");
            ChooseHUD = new PageItem("Show Quick Menu HUD\n in desktop", () =>
            {
                if (!VRHUD.Initialized)
                {
                    MelonLoader.MelonCoroutines.Start(VRHUD.Initialize());
                }
                else
                {
                    VRHUD.ToggleHUDButton.setActive(true);
                }
                DesktopHUD.enabled = false;
                VRHUD.enabled = true;
                Configuration.JSONConfig.VRHUDInDesktop = true;
                Configuration.SaveConfig();
                RefreshMenu();
            }, "Disabled", () =>
            {
                if (!DesktopHUD.Initialized)
                {
                    MelonLoader.MelonCoroutines.Start(DesktopHUD.Initialize());
                }
                if (VRHUD.Initialized)
                {
                    VRHUD.ToggleHUDButton.setActive(false);
                }
                VRHUD.enabled = false;
                DesktopHUD.enabled = true;
                Configuration.JSONConfig.VRHUDInDesktop = false;
                Configuration.SaveConfig();
                RefreshMenu();
            }, "TOGGLE: Switch between the Quick Menu HUD or normal HUD while in Desktop Mode");
            MoveVRHUD = new PageItem("Closer Quick Menu\nHUD", () =>
            {
                Configuration.JSONConfig.MoveVRHUDIfSpaceFree = true;
                Configuration.SaveConfig();
                VRHUD.RefreshPosition();
                RefreshMenu();
            }, "Disabled", () =>
            {
                Configuration.JSONConfig.MoveVRHUDIfSpaceFree = false;
                Configuration.SaveConfig();
                VRHUD.RefreshPosition();
                RefreshMenu();
            }, "TOGGLE: Allows the Quick Menu HUD to move inwards by one row if no emmVRC or Vanilla Buttons occupy the space (requires restart)");
            LogoButton = new PageItem("Logo Button", () =>
            {
                Configuration.JSONConfig.LogoButtonEnabled = true;
                Configuration.SaveConfig();
                RefreshMenu();
                MelonLoader.MelonCoroutines.Start(Hacks.ShortcutMenuButtons.Process());
            }, "Disabled", () =>
            {
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
            if (!Configuration.JSONConfig.StealthMode)
            {
                baseMenu.pageItems.Add(InfoBar);
                baseMenu.pageItems.Add(Clock);
                baseMenu.pageItems.Add(MasterIcon);
                baseMenu.pageItems.Add(HUD);
                baseMenu.pageItems.Add(ChooseHUD);
                baseMenu.pageItems.Add(MoveVRHUD);
                baseMenu.pageItems.Add(LogoButton);
                baseMenu.pageItems.Add(ForceRestart);
            }
            baseMenu.pageItems.Add(UnlimitedFPS);
            if (Configuration.JSONConfig.StealthMode)
                for (int i = 0; i < 8; i++)
                    baseMenu.pageItems.Add(PageItem.Space);

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
                }
                else
                {
                    VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowStandardPopup("emmVRC", "You must wait 5 seconds before toggling the network on again.", "Okay", new Action(() => { VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup(); }));
                    emmVRCNetwork.SetToggleState(false, false);
                }
            }, "Disabled", () =>
            {
                Configuration.JSONConfig.emmVRCNetworkEnabled = false;
                Configuration.SaveConfig();
                RefreshMenu();
                if (Network.NetworkClient.webToken != null)
                    Network.HTTPRequest.get(Network.NetworkClient.baseURL + "/api/authentication/logout").NoAwait("Logout");
                Network.NetworkClient.webToken = null;
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
            AvatarFavoriteJumpToStart = new PageItem("Jump to\nStart", () =>
            {
                Configuration.JSONConfig.AvatarFavoritesJumpToStart = true;
                Configuration.SaveConfig();
                RefreshMenu();
            }, "Disabled", () =>
            {
                Configuration.JSONConfig.AvatarFavoritesJumpToStart = false;
                Configuration.SaveConfig();
                RefreshMenu();
            }, "TOGGLE: In the emmVRC Favorites, automatically jumps back to the start when switching pages or reloading favorites");

            baseMenu.pageItems.Add(emmVRCNetwork);
            //baseMenu.pageItems.Add(GlobalChat);
            baseMenu.pageItems.Add(AvatarFavoriteList);
            baseMenu.pageItems.Add(AvatarFavoriteJumpToStart);
            for (int i = 0; i < 6; i++)
                baseMenu.pageItems.Add(PageItem.Space);


            FunctionsMenuPosition = new PageItem("Functions\nMenu\nButton", () => { FunctionsMenuPositionMenu.OpenMenu(); }, "Allows changing the position of the Functions button");
            LogoPosition = new PageItem("Logo\nButton", () => { LogoPositionMenu.OpenMenu(); }, "Allows changing the position of the Logo button");
            NotificationPosition = new PageItem("Notification\nButton", () => { NotificationPositionMenu.OpenMenu(); }, "Allows changing the position of the Notification button");

            FunctionsMenuPositionMenu = new ButtonConfigurationMenu(baseMenu.menuBase.getMenuName(), 312312, 654632, "", (ButtonConfigurationMenu menu) =>
            {
                GetUsedMenuSpaces(menu);
            }, (Vector2 vec) =>
            {
                SettingsMenu.LoadMenu();
                QMSingleButton tempPosition = new QMSingleButton("ShortcutMenu", 1, 0, "", null, "");
                FunctionsMenu.baseMenu.menuEntryButton.getGameObject().GetComponent<RectTransform>().anchoredPosition = tempPosition.getGameObject().GetComponent<RectTransform>().anchoredPosition;
                tempPosition.DestroyMe();
                Configuration.JSONConfig.FunctionsButtonX = Mathf.FloorToInt(vec.x);
                Configuration.JSONConfig.FunctionsButtonY = Mathf.FloorToInt(vec.y);
                Configuration.SaveConfig();
                FunctionsMenu.baseMenu.menuEntryButton.setLocation(Mathf.FloorToInt(vec.x), Mathf.FloorToInt(vec.y));
            }, "", "Select the new position for the Functions button", null);

            LogoPositionMenu = new ButtonConfigurationMenu(baseMenu.menuBase.getMenuName(), 520394, 296321, "", (ButtonConfigurationMenu menu) =>
            {
                GetUsedMenuSpaces(menu);
            }, (Vector2 vec) =>
            {
                SettingsMenu.LoadMenu();
                QMSingleButton tempPosition = new QMSingleButton("ShortcutMenu", 1, 0, "", null, "");
                ShortcutMenuButtons.logoButton.getGameObject().GetComponent<RectTransform>().anchoredPosition = tempPosition.getGameObject().GetComponent<RectTransform>().anchoredPosition;
                tempPosition.DestroyMe();
                Configuration.JSONConfig.LogoButtonX = Mathf.FloorToInt(vec.x);
                Configuration.JSONConfig.LogoButtonY = Mathf.FloorToInt(vec.y);
                Configuration.SaveConfig();
                ShortcutMenuButtons.logoButton.setLocation(Mathf.FloorToInt(vec.x), Mathf.FloorToInt(vec.y));
            }, "", "Select the new position for the Logo button", null);

            NotificationPositionMenu = new ButtonConfigurationMenu(baseMenu.menuBase.getMenuName(), 322222, 255423, "", (ButtonConfigurationMenu menu) =>
            {
                GetUsedMenuSpaces(menu);
            }, (Vector2 vec) =>
            {
                SettingsMenu.LoadMenu();
                QMSingleButton tempPosition = new QMSingleButton("ShortcutMenu", 1, 0, "", null, "");
                Managers.NotificationManager.NotificationMenu.getMainButton().getGameObject().GetComponent<RectTransform>().anchoredPosition = tempPosition.getGameObject().GetComponent<RectTransform>().anchoredPosition;
                tempPosition.DestroyMe();
                Configuration.JSONConfig.NotificationButtonPositionX = Mathf.FloorToInt(vec.x);
                Configuration.JSONConfig.NotificationButtonPositionY = Mathf.FloorToInt(vec.y);
                Configuration.SaveConfig();
                Managers.NotificationManager.NotificationMenu.getMainButton().setLocation(Mathf.FloorToInt(vec.x), Mathf.FloorToInt(vec.y));
                Managers.NotificationManager.AddNotification("Your new button position has been saved.", "Dismiss", Managers.NotificationManager.DismissCurrentNotification, "", null, Resources.alertSprite, -1);
            }, "", "Select the new position for the Notification button", null);
            TabMode = new PageItem("Tab Mode", () =>
            {
                Configuration.JSONConfig.TabMode = true;
                Configuration.SaveConfig();
                if (FunctionsMenu.baseMenu.menuEntryButton != null)
                    FunctionsMenu.baseMenu.menuEntryButton.getGameObject().transform.localScale = Vector3.zero;
                if (ShortcutMenuButtons.logoButton != null)
                    ShortcutMenuButtons.logoButton.getGameObject().transform.localScale = Vector3.zero;
                Managers.NotificationManager.NotificationMenu.getMainButton().getGameObject().transform.localScale = Vector3.zero;
                TabMenu.newTab.transform.localScale = Vector3.one;
                RefreshMenu();
            }, "Disabled", () =>
            {
                Configuration.JSONConfig.TabMode = false;
                Configuration.SaveConfig();
                if (FunctionsMenu.baseMenu.menuEntryButton != null)
                    FunctionsMenu.baseMenu.menuEntryButton.getGameObject().transform.localScale = Vector3.one;
                if (ShortcutMenuButtons.logoButton != null)
                    ShortcutMenuButtons.logoButton.getGameObject().transform.localScale = Vector3.one;
                Managers.NotificationManager.NotificationMenu.getMainButton().getGameObject().transform.localScale = Vector3.one;
                TabMenu.newTab.transform.localScale = Vector3.zero;
                RefreshMenu();
            }, "TOGGLE: Enables the emmVRC Tab, which replaces the Shortcut Menu buttons with a menu.");
            StealthMode = new PageItem("Stealth Mode", () =>
            {
                Configuration.JSONConfig.StealthMode = true;
                Configuration.SaveConfig();
                VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowStandardPopup("emmVRC", "You must restart your game for this change to apply.\nRestart now?", "Yes", () => { DestructiveActions.ForceRestart(); }, "No", () => { VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup(); });
            }, "Disabled", () =>
            {
                Configuration.JSONConfig.StealthMode = false;
                Configuration.SaveConfig();
                VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowStandardPopup("emmVRC", "You must restart your game for this change to apply.\nRestart now?", "Yes", () => { DestructiveActions.ForceRestart(); }, "No", () => { VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup(); });
            }, "TOGGLE: Enables emmVRC's stealth mode. This disables most of the visual features, in order to blend in with vanilla VRChat. The Functions button is available in the \"Report World\" menu.");
            if (!Configuration.JSONConfig.StealthMode)
            {
                baseMenu.pageItems.Add(FunctionsMenuPosition);
                baseMenu.pageItems.Add(LogoPosition);
                baseMenu.pageItems.Add(NotificationPosition);
            }
            else
            {
                for (int i = 0; i < 3; i++)
                    baseMenu.pageItems.Add(PageItem.Space);
            }
            baseMenu.pageItems.Add(PageItem.Space);
            baseMenu.pageItems.Add(PageItem.Space);
            baseMenu.pageItems.Add(PageItem.Space);
            baseMenu.pageItems.Add(PageItem.Space);
            if (!Configuration.JSONConfig.StealthMode)
                baseMenu.pageItems.Add(TabMode);
            else
                baseMenu.pageItems.Add(PageItem.Space);
            baseMenu.pageItems.Add(StealthMode);

            UIColorChanging = new PageItem("UI Color\nChange", () =>
            {
                Configuration.JSONConfig.UIColorChangingEnabled = true;
                Configuration.SaveConfig();
                RefreshMenu();
                Hacks.ColorChanger.ApplyIfApplicable();
                if (ModCompatibility.UIExpansionKit)
                    MelonLoader.MelonCoroutines.Start(ModCompatibility.ColorUIExpansionKit());
            }, "Disabled", () =>
            {
                Configuration.JSONConfig.UIColorChangingEnabled = false;
                Configuration.SaveConfig();
                RefreshMenu();
                Hacks.ColorChanger.ApplyIfApplicable();
                if (ModCompatibility.UIExpansionKit)
                    MelonLoader.MelonCoroutines.Start(ModCompatibility.ColorUIExpansionKit());
            }, "TOGGLE: Enables the color changing module, which affects UI, ESP, and loading");
            UIColorChangePicker = new ColorPicker(baseMenu.menuBase.getMenuName(), 1001, 1000, "UI Color", "Select the color for the UI", (UnityEngine.Color newColor) =>
            {
                Configuration.JSONConfig.UIColorHex = ColorConversion.ColorToHex(newColor, true);
                Configuration.SaveConfig();
                LoadMenu();
                Hacks.ColorChanger.ApplyIfApplicable();
                if (ModCompatibility.UIExpansionKit)
                    MelonLoader.MelonCoroutines.Start(ModCompatibility.ColorUIExpansionKit());
            }, () => { LoadMenu(); }, Libraries.ColorConversion.HexToColor(Configuration.JSONConfig.UIColorHex), Libraries.ColorConversion.HexToColor("#0EA6AD"));
            UIColorChangePickerButton = new PageItem("Select UI\nColor", () =>
            {
                QuickMenuUtils.ShowQuickmenuPage(UIColorChangePicker.baseMenu.getMenuName());
            }, "Selects the color splashed across the rest of the UI");
            UIActionMenuColorChanging = new PageItem("Action Menu\nChange", () =>
            {
                Configuration.JSONConfig.UIActionMenuColorChangingEnabled = true;
                Configuration.SaveConfig();
                RefreshMenu();
                Hacks.ColorChanger.ApplyIfApplicable();
            }, "Disabled", () =>
            {
                Configuration.JSONConfig.UIActionMenuColorChangingEnabled = false;
                Configuration.SaveConfig();
                RefreshMenu();
                Hacks.ColorChanger.ApplyIfApplicable();
            }, "TOGGLE: Enables the color changing module for the radial menu");
            UIMicIconColorChanging = new PageItem("Muted Icon\nColor Change", () =>
            {
                Configuration.JSONConfig.UIMicIconColorChangingEnabled = true;
                Configuration.SaveConfig();
                RefreshMenu();
                Hacks.ColorChanger.ApplyIfApplicable();
            }, "Disabled", () =>
            {
                Configuration.JSONConfig.UIMicIconColorChangingEnabled = false;
                Configuration.SaveConfig();
                RefreshMenu();
                Hacks.ColorChanger.ApplyIfApplicable();
            }, "TOGGLE: Enables the color changing module for the Muted Microphone icon on the HUD");
            UIMicIconPulse = new PageItem("Muted Icon\nPulsing", () =>
            {
                Configuration.JSONConfig.UIMicIconPulsingEnabled = true;
                Configuration.SaveConfig();
                RefreshMenu();
                Hacks.ColorChanger.ApplyIfApplicable();
            }, "Disabled", () =>
            {
                Configuration.JSONConfig.UIMicIconPulsingEnabled = false;
                Configuration.SaveConfig();
                RefreshMenu();
                Hacks.ColorChanger.ApplyIfApplicable();
            }, "TOGGLE: Enables or disables VRChat's new pulsating effect on the Microphone icon");
            UIExpansionKitColorChanging = new PageItem("UI Expansion\nKit Coloring", () =>
            {
                Configuration.JSONConfig.UIExpansionKitColorChangingEnabled = true;
                Configuration.SaveConfig();
                RefreshMenu();
                VRCUiManager.prop_VRCUiManager_0.QueueHUDMessage("You must restart VRChat for your changes to apply");
            }, "Disabled", () =>
            {
                Configuration.JSONConfig.UIExpansionKitColorChangingEnabled = false;
                Configuration.SaveConfig();
                RefreshMenu();
                VRCUiManager.prop_VRCUiManager_0.QueueHUDMessage("You must restart VRChat for your changes to apply");
            }, "TOGGLE: Enables changing the color of UIExpansionKit's menu. Requires a restart to apply");
            InfoSpoofing = new PageItem("Local\nInfo Spoofing", () =>
            {
                Configuration.JSONConfig.InfoSpoofingEnabled = true;
                Configuration.SaveConfig();
                RefreshMenu();
            }, "Disabled", () =>
            {
                Configuration.JSONConfig.InfoSpoofingEnabled = false;
                Configuration.SaveConfig();
                RefreshMenu();

                foreach (UnityEngine.UI.Text text in QuickMenuUtils.GetVRCUiMInstance().menuContent().GetComponentsInChildren<UnityEngine.UI.Text>())
                {
                    if (text.text.Contains(Hacks.NameSpoofGenerator.spoofedName))
                    {
                        text.text = text.text.Replace(Hacks.NameSpoofGenerator.spoofedName, APIUser.CurrentUser.GetName());
                    }
                }
                foreach (UnityEngine.UI.Text text in QuickMenuUtils.GetQuickMenuInstance().gameObject.GetComponentsInChildren<UnityEngine.UI.Text>())
                {
                    if (text.text.Contains(Hacks.NameSpoofGenerator.spoofedName))
                    {
                        text.text = text.text.Replace(Hacks.NameSpoofGenerator.spoofedName, APIUser.CurrentUser.GetName());
                    }
                }
                if (VRCPlayer.field_Internal_Static_VRCPlayer_0.GetNameplateText().text.Contains(Hacks.NameSpoofGenerator.spoofedName))
                    VRCPlayer.field_Internal_Static_VRCPlayer_0.GetNameplateText().text = VRCPlayer.field_Internal_Static_VRCPlayer_0.GetNameplateText().text.Replace(Hacks.NameSpoofGenerator.spoofedName, APIUser.CurrentUser.GetName());
            }, "TOGGLE: Enables local info spoofing, which can protect your identity in screenshots, recordings, and streams");
            //InfoSpooferNamePicker = new PageItem("")
            InfoSpooferNamePicker = new PageItem("Set spoofed\nName", () =>
            {
                VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowInputPopup("Enter your spoof name (or none for random)", "", UnityEngine.UI.InputField.InputType.Standard, false, "Accept", new System.Action<string, Il2CppSystem.Collections.Generic.List<UnityEngine.KeyCode>, UnityEngine.UI.Text>((string name, Il2CppSystem.Collections.Generic.List<UnityEngine.KeyCode> keyk, UnityEngine.UI.Text tx) =>
                {
                    if (Configuration.JSONConfig.InfoSpoofingEnabled)
                    {
                        foreach (UnityEngine.UI.Text text in QuickMenuUtils.GetVRCUiMInstance().menuContent().GetComponentsInChildren<UnityEngine.UI.Text>())
                        {
                            if (text.text.Contains(Hacks.NameSpoofGenerator.spoofedName))
                            {
                                text.text = text.text.Replace(Hacks.NameSpoofGenerator.spoofedName, APIUser.CurrentUser.GetName());
                            }
                        }
                        foreach (UnityEngine.UI.Text text in QuickMenuUtils.GetQuickMenuInstance().gameObject.GetComponentsInChildren<UnityEngine.UI.Text>())
                        {
                            if (text.text.Contains(Hacks.NameSpoofGenerator.spoofedName))
                            {
                                text.text = text.text.Replace(Hacks.NameSpoofGenerator.spoofedName, APIUser.CurrentUser.GetName());
                            }
                        }
                        if (VRCPlayer.field_Internal_Static_VRCPlayer_0.GetNameplateText().text.Contains(Hacks.NameSpoofGenerator.spoofedName))
                            VRCPlayer.field_Internal_Static_VRCPlayer_0.GetNameplateText().text = VRCPlayer.field_Internal_Static_VRCPlayer_0.GetNameplateText().text.Replace(Hacks.NameSpoofGenerator.spoofedName, APIUser.CurrentUser.GetName());
                    }
                    Configuration.JSONConfig.InfoSpoofingName = name;
                    Configuration.SaveConfig();
                    RefreshMenu();

                }), null, "Enter spoof name....");
            }, "Allows you to change your spoofed name to one that never changes");
            if (!Configuration.JSONConfig.StealthMode)
            {
                baseMenu.pageItems.Add(UIColorChanging);
                baseMenu.pageItems.Add(UIColorChangePickerButton);
                baseMenu.pageItems.Add(UIActionMenuColorChanging);
                baseMenu.pageItems.Add(UIMicIconColorChanging);
                baseMenu.pageItems.Add(UIMicIconPulse);
                if (ModCompatibility.UIExpansionKit)
                    baseMenu.pageItems.Add(UIExpansionKitColorChanging);
                else
                    baseMenu.pageItems.Add(PageItem.Space);
                baseMenu.pageItems.Add(InfoSpoofing);
                baseMenu.pageItems.Add(InfoSpooferNamePicker);
                baseMenu.pageItems.Add(PageItem.Space);
            }

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
            VeteranUserNameplateColorPicker = new ColorPicker(baseMenu.menuBase.getMenuName(), 1000, 1006, "Veteran User Nameplate Color (OGTrustRanks)", "Select the color for Veteran User Nameplate colors", (UnityEngine.Color newColor) =>
            {
                Configuration.JSONConfig.VeteranUserNamePlateColorHex = ColorConversion.ColorToHex(newColor, true);
                Configuration.SaveConfig();
                LoadMenu();
                Hacks.Nameplates.colorChanged = true;
                VRCPlayer.field_Internal_Static_VRCPlayer_0.ReloadAllAvatars();
            }, () => { LoadMenu(); }, Libraries.ColorConversion.HexToColor(Configuration.JSONConfig.TrustedUserNamePlateColorHex), Libraries.ColorConversion.HexToColor("#ABCDEE"));
            LegendaryUserNameplateColorPicker = new ColorPicker(baseMenu.menuBase.getMenuName(), 1000, 1007, "Legendary User Nameplate Color (OGTrustRanks)", "Select the color for Legendary User Nameplate colors", (UnityEngine.Color newColor) =>
            {
                Configuration.JSONConfig.LegendaryUserNamePlateColorHex = ColorConversion.ColorToHex(newColor, true);
                Configuration.SaveConfig();
                LoadMenu();
                Hacks.Nameplates.colorChanged = true;
                VRCPlayer.field_Internal_Static_VRCPlayer_0.ReloadAllAvatars();
            }, () => { LoadMenu(); }, Libraries.ColorConversion.HexToColor(Configuration.JSONConfig.TrustedUserNamePlateColorHex), Libraries.ColorConversion.HexToColor("#FF69B4"));
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
            VeteranUserNameplateColorPickerButton = new PageItem("Veteran User\nNameplate\nColor", () => { QuickMenuUtils.ShowQuickmenuPage(VeteranUserNameplateColorPicker.baseMenu.getMenuName()); }, "Select the color for Veteran User Nameplate colors (via OGTrustRanks)");
            LegendaryUserNameplateColorPickerButton = new PageItem("Legendary User\nNameplate\nColor", () => { QuickMenuUtils.ShowQuickmenuPage(LegendaryUserNameplateColorPicker.baseMenu.getMenuName()); }, "Select the color for Legendary User Nameplate colors (via OGTrustRanks)");
            if (!Configuration.JSONConfig.StealthMode)
            {
                baseMenu.pageItems.Add(NameplateColorChanging);
                if (!Libraries.ModCompatibility.OGTrustRank)
                {
                    baseMenu.pageItems.Add(PageItem.Space);
                    baseMenu.pageItems.Add(PageItem.Space);
                }
                baseMenu.pageItems.Add(FriendNameplateColorPickerButton);
                baseMenu.pageItems.Add(VisitorNameplateColorPickerButton);
                baseMenu.pageItems.Add(NewUserNameplateColorPickerButton);
                baseMenu.pageItems.Add(UserNameplateColorPickerButton);
                baseMenu.pageItems.Add(KnownUserNameplateColorPickerButton);
                baseMenu.pageItems.Add(TrustedUserNameplateColorPickerButton);
                if (Libraries.ModCompatibility.OGTrustRank)
                {
                    baseMenu.pageItems.Add(VeteranUserNameplateColorPickerButton);
                    baseMenu.pageItems.Add(LegendaryUserNameplateColorPickerButton);
                }
            }
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
            DisableOldInviteButtons = new PageItem("Disable Old\nInvite Buttons", () =>
            {
                Configuration.JSONConfig.DisableOldInviteButtons = true;
                Configuration.SaveConfig();
                MelonLoader.MelonCoroutines.Start(Hacks.ShortcutMenuButtons.Process());
            }, "Enabled", () =>
            {
                Configuration.JSONConfig.DisableOldInviteButtons = false;
                Configuration.SaveConfig();
                MelonLoader.MelonCoroutines.Start(Hacks.ShortcutMenuButtons.Process());
            }, "TOGGLE: Disables the old invite buttons from pre-build 1046 versions of VRChat");

            if (!Configuration.JSONConfig.StealthMode)
            {
                baseMenu.pageItems.Add(DisableReportWorld);
                baseMenu.pageItems.Add(DisableEmoji);
                baseMenu.pageItems.Add(DisableEmote);
                baseMenu.pageItems.Add(DisableRankToggle);
                baseMenu.pageItems.Add(DisableOldInviteButtons);
                for (int i = 0; i < 4; i++)
                    baseMenu.pageItems.Add(PageItem.Space);
            }



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
            DisableOneHandMovement = new PageItem("Disable\nHand Movement", () =>
            {
                Configuration.JSONConfig.DisableOneHandMovement = true;
                Configuration.SaveConfig();
                RefreshMenu();
                Hacks.ActionMenuTweaks.Apply();
            }, "Enabled", () =>
            {
                Configuration.JSONConfig.DisableOneHandMovement = false;
                Configuration.SaveConfig();
                RefreshMenu();
                Hacks.ActionMenuTweaks.Apply();
            }, "TOGGLE: Disables the one-handed movement indicator when one of your VR Controllers has lost tracking");
            DisableMicTooltip = new PageItem("Disable\nMic Tooltip", () =>
            {
                Configuration.JSONConfig.DisableMicTooltip = true;
                Configuration.SaveConfig();
                Hacks.UnscaledUITweaks.Process();
            }, "Enabled", () =>
            {
                Configuration.JSONConfig.DisableMicTooltip = false;
                Configuration.SaveConfig();
                Hacks.UnscaledUITweaks.Process();
            }, "TOGGLE: Disables the \"Hold V\" tooltip above the microphone indicator in the UI");
            if (!Configuration.JSONConfig.StealthMode)
            {
                baseMenu.pageItems.Add(DisablePlaylists);
                baseMenu.pageItems.Add(DisableReportUser);
                baseMenu.pageItems.Add(DisableAvatarStats);
                baseMenu.pageItems.Add(MinimalWarnKick);
                baseMenu.pageItems.Add(DisableOneHandMovement);
                baseMenu.pageItems.Add(DisableMicTooltip);
                for (int i = 0; i < 3; i++)
                    baseMenu.pageItems.Add(PageItem.Space);
            }
            DisableVRCPlusAds = new PageItem("Disable VRC+\nQuick Menu Ads", () =>
            {
                /*
                if (!APIUser.CurrentUser.isSupporter && Objects.NetworkConfig.Instance.VRCPlusRequired)
                    VRCUiPopupManager.prop_VRCUiPopupManager_0.ShowAlert("VRChat Plus Required", "VRChat, like emmVRC, relies on the support of their users to keep the platform free. Please support VRChat to unlock these features.", 0f);
                else*/ // Here is our code for VRChat Plus compliance. Bading!
                {
                    Configuration.JSONConfig.DisableVRCPlusAds = true;
                    Configuration.SaveConfig();
                    RefreshMenu();
                    MelonLoader.MelonCoroutines.Start(Hacks.ShortcutMenuButtons.Process());
                }
            }, "Enabled", () =>
            {
                Configuration.JSONConfig.DisableVRCPlusAds = false;
                Configuration.SaveConfig();
                RefreshMenu();
                MelonLoader.MelonCoroutines.Start(Hacks.ShortcutMenuButtons.Process());
            }, "TOGGLE: Disables the VRChat Plus adverts in the Quick Menu");
            DisableVRCPlusQMButtons = new PageItem("Disable VRC+\nQuick Menu Buttons", () =>
            {
                /*
                if (!APIUser.CurrentUser.isSupporter && Objects.NetworkConfig.Instance.VRCPlusRequired)
                    VRCUiPopupManager.prop_VRCUiPopupManager_0.ShowAlert("VRChat Plus Required", "VRChat, like emmVRC, relies on the support of their users to keep the platform free. Please support VRChat to unlock these features.", 0f);
                else*/ // Here is our code for VRChat Plus compliance. Bading!
                {
                    Configuration.JSONConfig.DisableVRCPlusQMButtons = true;
                    Configuration.SaveConfig();
                    RefreshMenu();
                    MelonLoader.MelonCoroutines.Start(Hacks.ShortcutMenuButtons.Process());
                }
            }, "Enabled", () =>
            {
                Configuration.JSONConfig.DisableVRCPlusQMButtons = false;
                Configuration.SaveConfig();
                RefreshMenu();
                MelonLoader.MelonCoroutines.Start(Hacks.ShortcutMenuButtons.Process());
            }, "TOGGLE: Disables the VRChat Plus buttons in the Quick Menu");
            DisableVRCPlusMenuTabs = new PageItem("Disable VRC+\nMenu Tabs", () =>
            {
                /*
                if (!APIUser.CurrentUser.isSupporter && Objects.NetworkConfig.Instance.VRCPlusRequired)
                    VRCUiPopupManager.prop_VRCUiPopupManager_0.ShowAlert("VRChat Plus Required", "VRChat, like emmVRC, relies on the support of their users to keep the platform free. Please support VRChat to unlock these features.", 0f);
                else*/ // Here is our code for VRChat Plus compliance. Bading!
                {
                    Configuration.JSONConfig.DisableVRCPlusMenuTabs = true;
                    Configuration.SaveConfig();
                    RefreshMenu();
                }

            }, "Enabled", () =>
            {
                Configuration.JSONConfig.DisableVRCPlusMenuTabs = false;
                Configuration.SaveConfig();
                RefreshMenu();

            }, "TOGGLE: Disables the VRChat Plus menu tabs, and adjusts the rest of the tabs to fit again");
            DisableVRCPlusUserInfo = new PageItem("Disable VRC+\nUser Info", () =>
            {
                /*
                if (!APIUser.CurrentUser.isSupporter && Objects.NetworkConfig.Instance.VRCPlusRequired)
                    VRCUiPopupManager.prop_VRCUiPopupManager_0.ShowAlert("VRChat Plus Required", "VRChat, like emmVRC, relies on the support of their users to keep the platform free. Please support VRChat to unlock these features.", 0f);
                else*/ // Here is our code for VRChat Plus compliance. Bading!
                {
                    Configuration.JSONConfig.DisableVRCPlusUserInfo = true;
                    Configuration.SaveConfig();
                    RefreshMenu();
                }

            }, "Enabled", () =>
            {
                Configuration.JSONConfig.DisableVRCPlusUserInfo = false;
                Configuration.SaveConfig();
                RefreshMenu();

            }, "TOGGLE: Disables the VRChat Plus User Info additions");
            if (!Configuration.JSONConfig.StealthMode)
            {
                baseMenu.pageItems.Add(DisableVRCPlusAds);
                baseMenu.pageItems.Add(DisableVRCPlusQMButtons);
                baseMenu.pageItems.Add(DisableVRCPlusMenuTabs);
                baseMenu.pageItems.Add(DisableVRCPlusUserInfo);
                for (int i = 0; i < 5; i++)
                    baseMenu.pageItems.Add(PageItem.Space);
            }

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
            DisableAvatarPersonalList = new PageItem("Disable Personal\nAvatar List", () =>
            {
                Configuration.JSONConfig.DisableAvatarPersonal = true;
                Configuration.SaveConfig();
                RefreshMenu();
            }, "Enabled", () =>
            {
                Configuration.JSONConfig.DisableAvatarPersonal = false;
                Configuration.SaveConfig();
                RefreshMenu();
            }, "TOGGLE: Disables the 'Personal Avatars' list in your Avatars menu");
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
            baseMenu.pageItems.Add(DisableAvatarPersonalList);
            baseMenu.pageItems.Add(DisableAvatarLegacyList);
            baseMenu.pageItems.Add(DisableAvatarPublicList);
            baseMenu.pageItems.Add(PageItem.Space);
            baseMenu.pageItems.Add(PageItem.Space);
            baseMenu.pageItems.Add(PageItem.Space);
            baseMenu.pageItems.Add(PageItem.Space);


            EnableKeybinds = new PageItem("Enable\nKeybinds", () =>
            {
                Configuration.JSONConfig.EnableKeybinds = true;
                Configuration.SaveConfig();
            }, "Disabled", () =>
            {
                Configuration.JSONConfig.EnableKeybinds = false;
                Configuration.SaveConfig();
            }, "TOGGLE: Enables or disables the desktop-mode keybinds for emmVRC");
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
            baseMenu.pageItems.Add(EnableKeybinds);
            baseMenu.pageItems.Add(PageItem.Space);
            baseMenu.pageItems.Add(FlightKeybind);
            baseMenu.pageItems.Add(NoclipKeybind);
            baseMenu.pageItems.Add(SpeedKeybind);
            baseMenu.pageItems.Add(ThirdPersonKeybind);
            baseMenu.pageItems.Add(ToggleHUDEnabledKeybind);
            baseMenu.pageItems.Add(RespawnKeybind);
            baseMenu.pageItems.Add(GoHomeKeybind);


            baseMenu.pageTitles.Add("Core Features" + (Configuration.JSONConfig.StealthMode ? " (Stealth Mode Enabled)" : ""));
            baseMenu.pageTitles.Add("Visual Features" + (Configuration.JSONConfig.StealthMode ? " (Stealth Mode Enabled)" : ""));
            baseMenu.pageTitles.Add("Network Features" + (Configuration.JSONConfig.StealthMode ? " (Stealth Mode Enabled)" : ""));
            baseMenu.pageTitles.Add("Button Positions" + (Configuration.JSONConfig.StealthMode ? " (Stealth Mode Enabled)" : ""));
            if (!Configuration.JSONConfig.StealthMode)
            {

                baseMenu.pageTitles.Add("UI Changing");
                baseMenu.pageTitles.Add("Nameplate Color Changing");
                baseMenu.pageTitles.Add("Disable Quick Menu Buttons");
                baseMenu.pageTitles.Add("Disable Other Features");
                baseMenu.pageTitles.Add("Disable VRChat Plus Buttons");
            }
            baseMenu.pageTitles.Add("Disable Avatar Menu Lists" + (Configuration.JSONConfig.StealthMode ? " (Stealth Mode Enabled)" : ""));
            baseMenu.pageTitles.Add("Keybinds" + (Configuration.JSONConfig.StealthMode ? " (Stealth Mode Enabled)" : ""));
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
                AvatarFavoriteJumpToStart.SetToggleState(Configuration.JSONConfig.AvatarFavoritesJumpToStart);
                UIExpansionKitIntegration.SetToggleState(Configuration.JSONConfig.UIExpansionKitIntegration);
                TrackingSaving.SetToggleState(Configuration.JSONConfig.TrackingSaving);

                InfoBar.SetToggleState(Configuration.JSONConfig.InfoBarDisplayEnabled);
                Clock.SetToggleState(Configuration.JSONConfig.ClockEnabled);
                MasterIcon.SetToggleState(Configuration.JSONConfig.MasterIconEnabled);
                HUD.SetToggleState(Configuration.JSONConfig.HUDEnabled);
                ChooseHUD.SetToggleState(Configuration.JSONConfig.VRHUDInDesktop);
                MoveVRHUD.SetToggleState(Configuration.JSONConfig.MoveVRHUDIfSpaceFree);
                LogoButton.SetToggleState(Configuration.JSONConfig.LogoButtonEnabled);
                ForceRestart.SetToggleState(Configuration.JSONConfig.ForceRestartButtonEnabled);
                UnlimitedFPS.SetToggleState(Configuration.JSONConfig.UnlimitedFPSEnabled);

                if (!Configuration.JSONConfig.StealthMode)
                {
                    LogoPosition.Active = !Configuration.JSONConfig.TabMode;
                    FunctionsMenuPosition.Active = !Configuration.JSONConfig.TabMode;
                    NotificationPosition.Active = !Configuration.JSONConfig.TabMode;
                }
                TabMode.SetToggleState(Configuration.JSONConfig.TabMode);
                StealthMode.SetToggleState(Configuration.JSONConfig.StealthMode);

                UIColorChanging.SetToggleState(Configuration.JSONConfig.UIColorChangingEnabled);
                UIActionMenuColorChanging.SetToggleState(Configuration.JSONConfig.UIActionMenuColorChangingEnabled);
                UIMicIconColorChanging.SetToggleState(Configuration.JSONConfig.UIMicIconColorChangingEnabled);
                UIMicIconPulse.SetToggleState(Configuration.JSONConfig.UIMicIconPulsingEnabled);
                UIExpansionKitColorChanging.SetToggleState(Configuration.JSONConfig.UIExpansionKitColorChangingEnabled);
                InfoSpoofing.SetToggleState(Configuration.JSONConfig.InfoSpoofingEnabled);
                InfoSpooferNamePicker.Name = "Set\nSpoofed\nName";

                NameplateColorChanging.SetToggleState(Configuration.JSONConfig.NameplateColorChangingEnabled);

                DisableReportWorld.SetToggleState(Configuration.JSONConfig.DisableReportWorldButton);
                DisableEmoji.SetToggleState(Configuration.JSONConfig.DisableEmojiButton);
                DisableEmote.SetToggleState(Configuration.JSONConfig.DisableEmoteButton);
                DisableRankToggle.SetToggleState(Configuration.JSONConfig.DisableRankToggleButton);
                DisableOldInviteButtons.SetToggleState(Configuration.JSONConfig.DisableOldInviteButtons);

                DisablePlaylists.SetToggleState(Configuration.JSONConfig.DisablePlaylistsButton);
                DisableAvatarStats.SetToggleState(Configuration.JSONConfig.DisableAvatarStatsButton);
                DisableReportUser.SetToggleState(Configuration.JSONConfig.DisableReportUserButton);
                MinimalWarnKick.SetToggleState(Configuration.JSONConfig.MinimalWarnKickButton);
                DisableOneHandMovement.SetToggleState(Configuration.JSONConfig.DisableOneHandMovement);
                DisableMicTooltip.SetToggleState(Configuration.JSONConfig.DisableMicTooltip);

                DisableVRCPlusAds.SetToggleState(Configuration.JSONConfig.DisableVRCPlusAds);
                DisableVRCPlusQMButtons.SetToggleState(Configuration.JSONConfig.DisableVRCPlusQMButtons);
                DisableVRCPlusMenuTabs.SetToggleState(Configuration.JSONConfig.DisableVRCPlusMenuTabs);
                DisableVRCPlusUserInfo.SetToggleState(Configuration.JSONConfig.DisableVRCPlusUserInfo);

                DisableAvatarHotWorlds.SetToggleState(Configuration.JSONConfig.DisableAvatarHotWorlds);
                DisableAvatarRandomWorlds.SetToggleState(Configuration.JSONConfig.DisableAvatarRandomWorlds);
                DisableAvatarPersonalList.SetToggleState(Configuration.JSONConfig.DisableAvatarPersonal);
                DisableAvatarLegacyList.SetToggleState(Configuration.JSONConfig.DisableAvatarLegacy);
                DisableAvatarPublicList.SetToggleState(Configuration.JSONConfig.DisableAvatarPublic);

                EnableKeybinds.SetToggleState(Configuration.JSONConfig.EnableKeybinds);
                FlightKeybind.Name = "Flight:\n" + (((UnityEngine.KeyCode)Configuration.JSONConfig.FlightKeybind[1] != UnityEngine.KeyCode.None ? KeyCodeConversion.Stringify(((UnityEngine.KeyCode)Configuration.JSONConfig.FlightKeybind[1])) + "+" : "") + (KeyCodeConversion.Stringify((UnityEngine.KeyCode)Configuration.JSONConfig.FlightKeybind[0])));
                NoclipKeybind.Name = "Noclip:\n" + (((UnityEngine.KeyCode)Configuration.JSONConfig.NoclipKeybind[1] != UnityEngine.KeyCode.None ? KeyCodeConversion.Stringify(((UnityEngine.KeyCode)Configuration.JSONConfig.NoclipKeybind[1])) + "+" : "") + (KeyCodeConversion.Stringify((UnityEngine.KeyCode)Configuration.JSONConfig.NoclipKeybind[0])));
                SpeedKeybind.Name = "Speed:\n" + (((UnityEngine.KeyCode)Configuration.JSONConfig.SpeedKeybind[1] != UnityEngine.KeyCode.None ? KeyCodeConversion.Stringify(((UnityEngine.KeyCode)Configuration.JSONConfig.SpeedKeybind[1])) + "+" : "") + (KeyCodeConversion.Stringify((UnityEngine.KeyCode)Configuration.JSONConfig.SpeedKeybind[0])));
                ThirdPersonKeybind.Name = "Third\nPerson:\n" + (((UnityEngine.KeyCode)Configuration.JSONConfig.ThirdPersonKeybind[1] != UnityEngine.KeyCode.None ? KeyCodeConversion.Stringify(((UnityEngine.KeyCode)Configuration.JSONConfig.ThirdPersonKeybind[1])) + "+" : "") + (KeyCodeConversion.Stringify((UnityEngine.KeyCode)Configuration.JSONConfig.ThirdPersonKeybind[0])));
                ToggleHUDEnabledKeybind.Name = "Toggle HUD\nEnabled:\n" + (((UnityEngine.KeyCode)Configuration.JSONConfig.ToggleHUDEnabledKeybind[1] != UnityEngine.KeyCode.None ? KeyCodeConversion.Stringify(((UnityEngine.KeyCode)Configuration.JSONConfig.ToggleHUDEnabledKeybind[1])) + "+" : "") + (KeyCodeConversion.Stringify((UnityEngine.KeyCode)Configuration.JSONConfig.ToggleHUDEnabledKeybind[0])));
                RespawnKeybind.Name = "Respawn:\n" + (((UnityEngine.KeyCode)Configuration.JSONConfig.RespawnKeybind[1] != UnityEngine.KeyCode.None ? KeyCodeConversion.Stringify(((UnityEngine.KeyCode)Configuration.JSONConfig.RespawnKeybind[1])) + "+" : "") + (KeyCodeConversion.Stringify((UnityEngine.KeyCode)Configuration.JSONConfig.RespawnKeybind[0])));
                GoHomeKeybind.Name = "Go Home:\n" + (((UnityEngine.KeyCode)Configuration.JSONConfig.GoHomeKeybind[1] != UnityEngine.KeyCode.None ? KeyCodeConversion.Stringify(((UnityEngine.KeyCode)Configuration.JSONConfig.GoHomeKeybind[1])) + "+" : "") + (KeyCodeConversion.Stringify((UnityEngine.KeyCode)Configuration.JSONConfig.GoHomeKeybind[0])));


            }
            catch (Exception ex)
            {
                emmVRCLoader.Logger.LogError("Error: " + ex.ToString());
            }
        }

        public static void GetUsedMenuSpaces(ButtonConfigurationMenu menu)
        {
            System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<string, Vector2>> unavailableButtons = new System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<string, Vector2>> {
                new System.Collections.Generic.KeyValuePair<string, Vector2>("Worlds", new Vector2(1, 0)),
                new System.Collections.Generic.KeyValuePair<string, Vector2>("Avatar", new Vector2(2, 0)),
                new System.Collections.Generic.KeyValuePair<string, Vector2>("Social", new Vector2(3, 0)),
                new System.Collections.Generic.KeyValuePair<string, Vector2>("Safety", new Vector2(4, 0)),
                new System.Collections.Generic.KeyValuePair<string, Vector2>("Go Home", new Vector2(1, 1)),
                new System.Collections.Generic.KeyValuePair<string, Vector2>("Respawn", new Vector2(2, 1)),
                new System.Collections.Generic.KeyValuePair<string, Vector2>("Seated\nStanding\nPlay", new Vector2(3, 1)),
                new System.Collections.Generic.KeyValuePair<string, Vector2>("Settings", new Vector2(4, 1)),
                new System.Collections.Generic.KeyValuePair<string, Vector2>("UI\nElements", new Vector2(1, 2)),
                new System.Collections.Generic.KeyValuePair<string, Vector2>("Camera", new Vector2(2, 2)),
            };
            if (!Configuration.JSONConfig.DisableEmoteButton)
                unavailableButtons.Add(new System.Collections.Generic.KeyValuePair<string, Vector2>("Emote", new Vector2(3, 2)));
            if (!Configuration.JSONConfig.DisableEmojiButton)
                unavailableButtons.Add(new System.Collections.Generic.KeyValuePair<string, Vector2>("Emoji", new Vector2(4, 2)));
            if (!Configuration.JSONConfig.DisableRankToggleButton)
                unavailableButtons.Add(new System.Collections.Generic.KeyValuePair<string, Vector2>("Toggle\nRank", new Vector2(5, 2)));
            if (!Configuration.JSONConfig.DisableReportWorldButton)
                unavailableButtons.Add(new System.Collections.Generic.KeyValuePair<string, Vector2>("Report\nWorld", new Vector2(5, 1)));
            if (!menu.menuTitle.Contains("Functions"))
                unavailableButtons.Add(new System.Collections.Generic.KeyValuePair<string, Vector2>("<color=#FF69B4>emmVRC</color>\nFunctions", new Vector2(Configuration.JSONConfig.FunctionsButtonX, Configuration.JSONConfig.FunctionsButtonY)));
            if (!menu.menuTitle.Contains("Logo") && Configuration.JSONConfig.LogoButtonEnabled)
                unavailableButtons.Add(new System.Collections.Generic.KeyValuePair<string, Vector2>("<color=#FF69B4>emmVRC</color>\nLogo", new Vector2(Configuration.JSONConfig.LogoButtonX, Configuration.JSONConfig.LogoButtonY)));
            if (!menu.menuTitle.Contains("Notification"))
                unavailableButtons.Add(new System.Collections.Generic.KeyValuePair<string, Vector2>("<color=#FF69B4>emmVRC</color>\nNotification", new Vector2(Configuration.JSONConfig.NotificationButtonPositionX, Configuration.JSONConfig.NotificationButtonPositionY)));
            menu.ChangeDisabledButtons(unavailableButtons);
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
