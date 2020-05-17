using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Events;
using emmVRC.Libraries;
using emmVRC.Hacks;

namespace emmVRC.Menus
{
    public class SettingsMenu
    {
        // Base menu for the Settings menu
        public static PaginatedMenu baseMenu;

        // Page 1
        private static PageItem OpenBeta;
        private static PageItem UnlimitedFPS;
        private static PageItem RiskyFunctions;
        private static PageItem GlobalDynamicBones;
        private static PageItem EveryoneGlobalDynamicBones;
        private static PageItem emmVRCNetwork;
        private static PageItem GlobalChat;
        private static PageItem AutoInviteMessage;
        private static PageItem AvatarFavoriteList;

        // Page 2
        private static PageItem InfoBar;
        private static PageItem Clock;
        private static PageItem MasterIcon;
        private static PageItem LogoButton;
        private static PageItem HUD;
        private static PageItem ForceRestart;
        private static PageItem InfoSpoofing;
        private static PageItem InfoHiding;
        private static PageItem InfoSpooferNamePicker;

        // Page 3
        private static PageItem UIColorChanging;
        private static PageItem UIColorChangePickerButton;
        private static ColorPicker UIColorChangePicker;

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
        private static PageItem DisableRankToggle;

        // Page 6
        private static PageItem DisablePlaylists;
        private static PageItem DisableAvatarStats;
        private static PageItem DisableReportUser;

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


        public static void Initialize()
        {
            // Initialize the Paginated Menu for the Settings menu.
            baseMenu = new PaginatedMenu(FunctionsMenu.baseMenu.menuBase, 1024, 768, "Settings", "The base menu for the settings menu", null);
            baseMenu.menuEntryButton.DestroyMe();

            OpenBeta = new PageItem("Open Beta", () =>
            {
                Configuration.JSONConfig.OpenBetaEnabled = true;
                Configuration.SaveConfig();
                RefreshMenu();
            }, "Disabled", () =>
            {
                Configuration.JSONConfig.OpenBetaEnabled = false;
                Configuration.SaveConfig();
                RefreshMenu();
            }, "TOGGLE: Enables the emmVRC Open Beta. Requires a restart to take affect");
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
            }, "TOGGLE: Enables the FPS unlimiter, in desktop only.");
            RiskyFunctions = new PageItem("Risky Functions", () =>
            {
                if (!Configuration.JSONConfig.RiskyFunctionsWarningShown)
                    VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowStandardPopup("Risky Functions", "By enabling these functions, you accept the risk that these functions could be detected by VRChat, and you agree to not use them for malicious or harassment purposes.", "Agree", UnhollowerRuntimeLib.DelegateSupport.ConvertDelegate<Il2CppSystem.Action>((System.Action)(() =>
                {
                    VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup();
                    Configuration.JSONConfig.RiskyFunctionsEnabled = true;
                    Configuration.JSONConfig.RiskyFunctionsWarningShown = true;
                    Configuration.SaveConfig();
                    MelonLoader.MelonCoroutines.Start(Managers.RiskyFunctionsManager.CheckWorld());
                    RefreshMenu();
                })), "Decline", UnhollowerRuntimeLib.DelegateSupport.ConvertDelegate<Il2CppSystem.Action>((System.Action)(() =>
                {
                    VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup();
                    RiskyFunctions.SetToggleState(false);
                    RefreshMenu();
                })));
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
            GlobalDynamicBones = new PageItem("Global\nDynamic Bones", () =>
            {
                Configuration.JSONConfig.GlobalDynamicBonesEnabled = true;
                Configuration.SaveConfig();
                RefreshMenu();
                VRCPlayer.field_Internal_Static_VRCPlayer_0.Method_Public_Void_Boolean_0(false);
                VRCPlayer.field_Internal_Static_VRCPlayer_0.Method_Public_Void_10();
            }, "Disabled", () =>
            {
                Configuration.JSONConfig.GlobalDynamicBonesEnabled = false;
                Configuration.SaveConfig();
                RefreshMenu();
                VRCPlayer.field_Internal_Static_VRCPlayer_0.Method_Public_Void_Boolean_0(false);
                VRCPlayer.field_Internal_Static_VRCPlayer_0.Method_Public_Void_10();
            }, "TOGGLE: Enables the Global Dynamic Bones system");
            EveryoneGlobalDynamicBones = new PageItem("Everybody Global\nDynamic Bones", () =>
            {
                Configuration.JSONConfig.EveryoneGlobalDynamicBonesEnabled = true;
                Configuration.SaveConfig();
                RefreshMenu();
                VRCPlayer.field_Internal_Static_VRCPlayer_0.Method_Public_Void_Boolean_0(false);
                VRCPlayer.field_Internal_Static_VRCPlayer_0.Method_Public_Void_10();
            }, "Disabled", () =>
            {
                Configuration.JSONConfig.EveryoneGlobalDynamicBonesEnabled = false;
                Configuration.SaveConfig();
                RefreshMenu();
                VRCPlayer.field_Internal_Static_VRCPlayer_0.Method_Public_Void_Boolean_0(false);
                VRCPlayer.field_Internal_Static_VRCPlayer_0.Method_Public_Void_10();
            }, "TOGGLE: Enables Global Dynamic Bones for everyone. Note that this might cause lag in large instances");
            emmVRCNetwork = new PageItem("emmVRC Network\nEnabled", () =>
            {
                Configuration.JSONConfig.emmVRCNetworkEnabled = true;
                Configuration.SaveConfig();
                RefreshMenu();
                Network.NetworkClient.InitializeClient();
                MelonLoader.MelonCoroutines.Start(emmVRC.loadNetworked());
            }, "Disabled", () =>
            {
                Configuration.JSONConfig.emmVRCNetworkEnabled = false;
                Configuration.SaveConfig();
                RefreshMenu();
                if (Network.NetworkClient.authToken != null)
                    Network.HTTPRequest.get_sync(Network.NetworkClient.baseURL + "/api/authentication/logout");
                Network.NetworkClient.authToken = null;
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
            AutoInviteMessage = new PageItem("Automatic\nInvite Messages", () =>
            {
                Configuration.JSONConfig.AutoInviteMessage = true;
                Configuration.SaveConfig();
                RefreshMenu();
            }, "Disabled", () =>
            {
                Configuration.JSONConfig.AutoInviteMessage = false;
                Configuration.SaveConfig();
                RefreshMenu();
            }, "TOGGLE: Sends messages through invites, instead of the emmVRC Network");
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
            baseMenu.pageItems.Add(OpenBeta);
            baseMenu.pageItems.Add(UnlimitedFPS);
            baseMenu.pageItems.Add(RiskyFunctions);
            baseMenu.pageItems.Add(GlobalDynamicBones);
            baseMenu.pageItems.Add(EveryoneGlobalDynamicBones);
            baseMenu.pageItems.Add(emmVRCNetwork);
            baseMenu.pageItems.Add(GlobalChat);
            baseMenu.pageItems.Add(AutoInviteMessage);
            baseMenu.pageItems.Add(AvatarFavoriteList);

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
            }, "TOGGLE: Enables the HUD, which shows players in the room and instance information in Desktop");
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
                Libraries.InputUtilities.OpenInputBox("Enter your spoof name (or none for random)", "Accept", (name) =>
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
                });
            }, "Allows you to change your spoofed name to one that never changes");
            baseMenu.pageItems.Add(InfoBar);
            baseMenu.pageItems.Add(Clock);
            baseMenu.pageItems.Add(MasterIcon);
            baseMenu.pageItems.Add(LogoButton);
            baseMenu.pageItems.Add(HUD);
            baseMenu.pageItems.Add(ForceRestart);
            baseMenu.pageItems.Add(InfoSpoofing);
            baseMenu.pageItems.Add(InfoHiding);
            baseMenu.pageItems.Add(InfoSpooferNamePicker);

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
            }, () => { LoadMenu(); });
            UIColorChangePickerButton = new PageItem("Select UI\nColor", () =>
            {
                QuickMenuUtils.ShowQuickmenuPage(UIColorChangePicker.baseMenu.getMenuName());
            }, "Selects the color splashed across the rest of the UI");
            baseMenu.pageItems.Add(UIColorChanging);
            baseMenu.pageItems.Add(UIColorChangePickerButton);
            baseMenu.pageItems.Add(PageItem.Space());
            baseMenu.pageItems.Add(PageItem.Space());
            baseMenu.pageItems.Add(PageItem.Space());
            baseMenu.pageItems.Add(PageItem.Space());
            baseMenu.pageItems.Add(PageItem.Space());
            baseMenu.pageItems.Add(PageItem.Space());
            baseMenu.pageItems.Add(PageItem.Space());

            FriendNameplateColorPicker = new ColorPicker(baseMenu.menuBase.getMenuName(), 1000, 1000, "Friend Nameplate Color", "Select the color for Friend Nameplate colors", (UnityEngine.Color newColor) =>
            {
                Configuration.JSONConfig.FriendNamePlateColorHex = ColorConversion.ColorToHex(newColor, true);
                Configuration.SaveConfig();
                LoadMenu();
                Hacks.Nameplates.colorChanged = true;
            }, () => { LoadMenu(); });
            VisitorNameplateColorPicker = new ColorPicker(baseMenu.menuBase.getMenuName(), 1000, 1001, "Visitor Nameplate Color", "Select the color for Visitor Nameplate colors", (UnityEngine.Color newColor) =>
            {
                Configuration.JSONConfig.VisitorNamePlateColorHex = ColorConversion.ColorToHex(newColor, true);
                Configuration.SaveConfig();
                LoadMenu();
                Hacks.Nameplates.colorChanged = true;
            }, () => { LoadMenu(); });
            NewUserNameplateColorPicker = new ColorPicker(baseMenu.menuBase.getMenuName(), 1000, 1002, "New User Nameplate Color", "Select the color for New User Nameplate colors", (UnityEngine.Color newColor) =>
            {
                Configuration.JSONConfig.NewUserNamePlateColorHex = ColorConversion.ColorToHex(newColor, true);
                Configuration.SaveConfig();
                LoadMenu();
                Hacks.Nameplates.colorChanged = true;
            }, () => { LoadMenu(); });
            UserNameplateColorPicker = new ColorPicker(baseMenu.menuBase.getMenuName(), 1000, 1003, "User Nameplate Color", "Select the color for User Nameplate colors", (UnityEngine.Color newColor) =>
            {
                Configuration.JSONConfig.UserNamePlateColorHex = ColorConversion.ColorToHex(newColor, true);
                Configuration.SaveConfig();
                LoadMenu();
                Hacks.Nameplates.colorChanged = true;
                VRCPlayer.field_Internal_Static_VRCPlayer_0.Method_Public_Void_Boolean_1(false);
            }, () => { LoadMenu(); });
            KnownUserNameplateColorPicker = new ColorPicker(baseMenu.menuBase.getMenuName(), 1000, 1004, "Known User Nameplate Color", "Select the color for Known User Nameplate colors", (UnityEngine.Color newColor) =>
            {
                Configuration.JSONConfig.KnownUserNamePlateColorHex = ColorConversion.ColorToHex(newColor, true);
                Configuration.SaveConfig();
                LoadMenu();
                Hacks.Nameplates.colorChanged = true;
            }, () => { LoadMenu(); });
            TrustedUserNameplateColorPicker = new ColorPicker(baseMenu.menuBase.getMenuName(), 1000, 1005, "Trusted User Nameplate Color", "Select the color for Trusted User Nameplate colors", (UnityEngine.Color newColor) =>
            {
                Configuration.JSONConfig.TrustedUserNamePlateColorHex = ColorConversion.ColorToHex(newColor, true);
                Configuration.SaveConfig();
                LoadMenu();
                Hacks.Nameplates.colorChanged = true;
            }, () => { LoadMenu(); });
            NameplateColorChanging = new PageItem("Nameplate\nColor Changing", () =>
            {
                Configuration.JSONConfig.NameplateColorChangingEnabled = true;
                Configuration.SaveConfig();
                RefreshMenu();
                Hacks.Nameplates.colorChanged = true;
            }, "Disabled", () =>
            {
                Configuration.JSONConfig.NameplateColorChangingEnabled = false;
                Configuration.SaveConfig();
                RefreshMenu();
                Hacks.Nameplates.colorChanged = true;
            }, "TOGGLE: Enables the nameplate color changing module, which changes the colors of the various trust ranks in nameplates and the UI");
            FriendNameplateColorPickerButton = new PageItem("Friend\nNameplate\nColor", () => { QuickMenuUtils.ShowQuickmenuPage(FriendNameplateColorPicker.baseMenu.getMenuName()); }, "Select the color for Friend Nameplate colors");
            VisitorNameplateColorPickerButton = new PageItem("Visitor\nNameplate\nColor", () => { QuickMenuUtils.ShowQuickmenuPage(VisitorNameplateColorPicker.baseMenu.getMenuName()); }, "Select the color for Visitor Nameplate colors");
            NewUserNameplateColorPickerButton = new PageItem("New User\nNameplate\nColor", () => { QuickMenuUtils.ShowQuickmenuPage(NewUserNameplateColorPicker.baseMenu.getMenuName()); }, "Select the color for New User Nameplate colors");
            UserNameplateColorPickerButton = new PageItem("User\nNameplate\nColor", () => { QuickMenuUtils.ShowQuickmenuPage(UserNameplateColorPicker.baseMenu.getMenuName()); }, "Select the color for User Nameplate colors");
            KnownUserNameplateColorPickerButton = new PageItem("Known User\nNameplate\nColor", () => { QuickMenuUtils.ShowQuickmenuPage(KnownUserNameplateColorPicker.baseMenu.getMenuName()); }, "Select the color for Known User Nameplate colors");
            TrustedUserNameplateColorPickerButton = new PageItem("Trusted User\nNameplate\nColor", () => { QuickMenuUtils.ShowQuickmenuPage(TrustedUserNameplateColorPicker.baseMenu.getMenuName()); }, "Select the color for Trusted User Nameplate colors");
            baseMenu.pageItems.Add(NameplateColorChanging);
            baseMenu.pageItems.Add(PageItem.Space());
            baseMenu.pageItems.Add(PageItem.Space());
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
            }, "TOGGLE: Disables the 'Emoji' button in the Quick Menu. Its functionality can be found in the Disabled Buttons menu");
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
            baseMenu.pageItems.Add(DisableReportWorld);
            baseMenu.pageItems.Add(DisableEmoji);
            baseMenu.pageItems.Add(DisableRankToggle);
            baseMenu.pageItems.Add(DisablePlaylists);
            baseMenu.pageItems.Add(DisableReportUser);
            baseMenu.pageItems.Add(DisableAvatarStats);
            baseMenu.pageItems.Add(PageItem.Space());
            baseMenu.pageItems.Add(PageItem.Space());
            baseMenu.pageItems.Add(PageItem.Space());

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
            baseMenu.pageItems.Add(PageItem.Space());
            baseMenu.pageItems.Add(PageItem.Space());
            baseMenu.pageItems.Add(PageItem.Space());
            baseMenu.pageItems.Add(PageItem.Space());
            baseMenu.pageItems.Add(PageItem.Space());


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
            baseMenu.pageItems.Add(FlightKeybind);
            baseMenu.pageItems.Add(NoclipKeybind);
            baseMenu.pageItems.Add(SpeedKeybind);
            baseMenu.pageItems.Add(ThirdPersonKeybind);
            baseMenu.pageItems.Add(ToggleHUDEnabledKeybind);


            baseMenu.pageTitles.Add("Core Features");
            baseMenu.pageTitles.Add("Visual Features");
            baseMenu.pageTitles.Add("UI Color Changing");
            baseMenu.pageTitles.Add("Nameplate Color Changing");
            baseMenu.pageTitles.Add("Disable VRChat Buttons");
            baseMenu.pageTitles.Add("Disable Avatar Menu Lists");
            baseMenu.pageTitles.Add("Keybinds");
        }
        public static void RefreshMenu()
        {
            try
            {
                OpenBeta.SetToggleState(Configuration.JSONConfig.OpenBetaEnabled);
                UnlimitedFPS.SetToggleState(Configuration.JSONConfig.UnlimitedFPSEnabled);
                RiskyFunctions.SetToggleState(Configuration.JSONConfig.RiskyFunctionsEnabled);
                GlobalDynamicBones.SetToggleState(Configuration.JSONConfig.GlobalDynamicBonesEnabled);
                EveryoneGlobalDynamicBones.SetToggleState(Configuration.JSONConfig.EveryoneGlobalDynamicBonesEnabled);
                emmVRCNetwork.SetToggleState(Configuration.JSONConfig.emmVRCNetworkEnabled);
                GlobalChat.SetToggleState(Configuration.JSONConfig.GlobalChatEnabled);
                AutoInviteMessage.SetToggleState(Configuration.JSONConfig.AutoInviteMessage);
                AvatarFavoriteList.SetToggleState(Configuration.JSONConfig.AvatarFavoritesEnabled);

                InfoBar.SetToggleState(Configuration.JSONConfig.InfoBarDisplayEnabled);
                Clock.SetToggleState(Configuration.JSONConfig.ClockEnabled);
                MasterIcon.SetToggleState(Configuration.JSONConfig.MasterIconEnabled);
                LogoButton.SetToggleState(Configuration.JSONConfig.LogoButtonEnabled);
                HUD.SetToggleState(Configuration.JSONConfig.HUDEnabled);
                ForceRestart.SetToggleState(Configuration.JSONConfig.ForceRestartButtonEnabled);
                InfoSpoofing.SetToggleState(Configuration.JSONConfig.InfoSpoofingEnabled);
                InfoHiding.SetToggleState(Configuration.JSONConfig.InfoHidingEnabled);
                InfoSpooferNamePicker.Name = "Set\nSpoofed\nName";

                UIColorChanging.SetToggleState(Configuration.JSONConfig.UIColorChangingEnabled);

                NameplateColorChanging.SetToggleState(Configuration.JSONConfig.NameplateColorChangingEnabled);

                DisableReportWorld.SetToggleState(Configuration.JSONConfig.DisableReportWorldButton);
                DisableEmoji.SetToggleState(Configuration.JSONConfig.DisableEmojiButton);
                DisableRankToggle.SetToggleState(Configuration.JSONConfig.DisableRankToggleButton);
                DisablePlaylists.SetToggleState(Configuration.JSONConfig.DisablePlaylistsButton);
                DisableAvatarStats.SetToggleState(Configuration.JSONConfig.DisableAvatarStatsButton);
                DisableReportUser.SetToggleState(Configuration.JSONConfig.DisableReportUserButton);

                DisableAvatarHotWorlds.SetToggleState(Configuration.JSONConfig.DisableAvatarHotWorlds);
                DisableAvatarRandomWorlds.SetToggleState(Configuration.JSONConfig.DisableAvatarRandomWorlds);
                DisableAvatarLegacyList.SetToggleState(Configuration.JSONConfig.DisableAvatarLegacy);
                DisableAvatarPublicList.SetToggleState(Configuration.JSONConfig.DisableAvatarPublic);

                FlightKeybind.Name = "Flight:\n" + (((UnityEngine.KeyCode)Configuration.JSONConfig.FlightKeybind[1] != UnityEngine.KeyCode.None ? KeyCodeConversion.Stringify(((UnityEngine.KeyCode)Configuration.JSONConfig.FlightKeybind[1])) + "+" : "") + (KeyCodeConversion.Stringify((UnityEngine.KeyCode)Configuration.JSONConfig.FlightKeybind[0])));
                NoclipKeybind.Name = "Noclip:\n" + (((UnityEngine.KeyCode)Configuration.JSONConfig.NoclipKeybind[1] != UnityEngine.KeyCode.None ? KeyCodeConversion.Stringify(((UnityEngine.KeyCode)Configuration.JSONConfig.NoclipKeybind[1])) + "+" : "") + (KeyCodeConversion.Stringify((UnityEngine.KeyCode)Configuration.JSONConfig.NoclipKeybind[0])));
                SpeedKeybind.Name = "Speed:\n" + (((UnityEngine.KeyCode)Configuration.JSONConfig.SpeedKeybind[1] != UnityEngine.KeyCode.None ? KeyCodeConversion.Stringify(((UnityEngine.KeyCode)Configuration.JSONConfig.SpeedKeybind[1])) + "+" : "") + (KeyCodeConversion.Stringify((UnityEngine.KeyCode)Configuration.JSONConfig.SpeedKeybind[0])));
                ThirdPersonKeybind.Name = "Third\nPerson:\n" + (((UnityEngine.KeyCode)Configuration.JSONConfig.ThirdPersonKeybind[1] != UnityEngine.KeyCode.None ? KeyCodeConversion.Stringify(((UnityEngine.KeyCode)Configuration.JSONConfig.ThirdPersonKeybind[1])) + "+" : "") + (KeyCodeConversion.Stringify((UnityEngine.KeyCode)Configuration.JSONConfig.ThirdPersonKeybind[0])));
                ToggleHUDEnabledKeybind.Name = "Toggle HUD\nEnabled:\n" + (((UnityEngine.KeyCode)Configuration.JSONConfig.ToggleHUDEnabledKeybind[1] != UnityEngine.KeyCode.None ? KeyCodeConversion.Stringify(((UnityEngine.KeyCode)Configuration.JSONConfig.ToggleHUDEnabledKeybind[1])) + "+" : "") + (KeyCodeConversion.Stringify((UnityEngine.KeyCode)Configuration.JSONConfig.ToggleHUDEnabledKeybind[0])));



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
    }
}
