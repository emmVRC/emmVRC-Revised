using System;
using UnityEngine;
using emmVRC.Managers;
using emmVRC.Hacks;
using emmVRC.Network;
using emmVRC.Menus;
using emmVRC.Objects;
using Il2CppSystem.IO;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using emmVRC.Libraries;
using VRC.Core;
using Il2CppSystem.Reflection;


#pragma warning disable 4014

namespace emmVRC
{
    public static class emmVRC
    {
        public static bool Initialized = false;
        // OnApplicationStart is called when emmVRCLoader passes over control to the emmVRC assembly.
        private static void OnApplicationStart()
        {
            // A little throwback to the first ever mod I worked on, YoshiMod
            if (Environment.CommandLine.Contains("--emmvrc.anniversarymode"))
            {
                emmVRCLoader.Logger.Log("Hello world!");
                emmVRCLoader.Logger.Log("This is the beginning of a new beginning!");
                emmVRCLoader.Logger.Log("Wait... have I said that before?");
            }

            // Load the config for emmVRC
            Configuration.Initialize();

            // Enable stealth mode if the launch option is present
            if (Environment.CommandLine.Contains("--emmvrc.stealthmode"))
                Configuration.JSONConfig.StealthMode = true;

        }

        // OnUIManagerInit is the equivelent of the VRCUiManagerUtils.WaitForUIManagerInit, but better
        public static void OnUIManagerInit()
        {
            int VRCBuildNumber = UnityEngine.Resources.FindObjectsOfTypeAll<VRCApplicationSetup>().First().buildNumber;
            emmVRCLoader.Logger.Log("VRChat build is: " + VRCBuildNumber);
            if (VRCBuildNumber >= 1019)
                Attributes.VRCPlusVersion = true;
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            /*bool HarmonyPresent = false;
            string[] ModsFolderFiles = Directory.GetFiles(Path.Combine(Environment.CurrentDirectory, "Mods"));
            foreach (string str in ModsFolderFiles)
            {
                if (str.Contains("harmony") || str.Contains("Harmony"))
                    HarmonyPresent = true;
            }
            if (HarmonyPresent)
            {
                System.Windows.Forms.MessageBox.Show("You have an incompatible copy of Harmony in your Mods folder. Please remove it for emmVRC to function correctly.", "emmVRC", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                emmVRCLoader.Logger.LogError("Harmony detected in the Mods folder. Please remove it for emmVRC to function correctly.");
            }*/
            string currentVersion = (string)typeof(MelonLoader.BuildInfo).GetField("Version").GetValue(null);
            string currentEmmVRCLoaderVersion = (string)typeof(emmVRCLoader.BuildInfo).GetField("Version").GetValue(null);
            if (Attributes.IncompatibleMelonLoaderVersions.Contains(currentVersion))
            {
                emmVRCLoader.Logger.LogError("You are using an incompatible version of MelonLoader: v" + currentVersion + ". Please install v" + Attributes.TargetMelonLoaderVersion + ", via the instructions in our Discord under the #how-to channel. emmVRC will not start.");
                System.Windows.Forms.MessageBox.Show("You are using an incompatible version of MelonLoader: v" + currentVersion + ". Please install v" + Attributes.TargetMelonLoaderVersion + ", via the instructions in our Discord under the #how-to channel. emmVRC will not start.", "emmVRC", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                watch.Stop();
                return;
            }
            else if (Attributes.IncompatibleemmVRCLoaderVersions.Contains(currentEmmVRCLoaderVersion))
            {
                try
                {
                    WebClient cli = new WebClient();
                    string dest = "";
                    foreach (string modFile in System.IO.Directory.GetFiles(Path.Combine(Environment.CurrentDirectory, "Mods")))
                    {
                        if (modFile.Contains("emmVRC"))
                        {
                            dest = modFile;
                            System.IO.File.Delete(modFile);
                        }
                    }
                    if (dest != "")
                        cli.DownloadFile("https://thetrueyoshifan.com/downloads/emmVRCLoader.dll", dest);
                    else
                        cli.DownloadFile("https://thetrueyoshifan.com/downloads/emmVRCLoader.dll", Path.Combine(Environment.CurrentDirectory, "Mods/emmVRCLoader.dll"));
                    MessageBox.Show("The newest emmVRCLoader has been downloaded to your Mods folder. To use emmVRC, restart your game. If the problem persists, remove any current emmVRCLoader files, and download the latest from #loader-updates in the emmVRC Discord.", "emmVRC", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    watch.Stop();
                }
                catch (Exception ex)
                {
                    emmVRCLoader.Logger.LogError("Attempt to download the new loader failed. You must download the latest from https://thetrueyoshifan.com/downloads/emmVRCLoader.dll manually.");
                    emmVRCLoader.Logger.LogError("Error: " + ex.ToString());
                    System.Windows.Forms.MessageBox.Show("You are using an incompatible version of emmVRCLoader: v" + currentEmmVRCLoaderVersion + ". Please install v" + Attributes.TargetemmVRCLoaderVersion + " or greater, from the #loader-updates channel in the emmVRC Discord. emmVRC cannot start.", "emmVRC", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    watch.Stop();
                }
                return;
            }
            else if (VRCBuildNumber < Attributes.LastTestedBuildNumber)
            {
                emmVRCLoader.Logger.LogError("You are using an older version of VRChat than supported by emmVRC: " + VRCBuildNumber + ". Please update VRChat through Steam or Oculus to build " + Attributes.LastTestedBuildNumber + ". emmVRC will not start.");
                System.Windows.Forms.MessageBox.Show("You are using an older version of VRChat than supported by emmVRC: " + VRCBuildNumber + ". Please update VRChat through Steam or Oculus to build " + Attributes.LastTestedBuildNumber + ". emmVRC will not start.", "emmVRC", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                watch.Stop();
                return;
            }

            else
            {
                if (Configuration.JSONConfig.emmVRCNetworkEnabled)
                {
                    // Network connection
                    emmVRCLoader.Logger.Log("Initializing network...");
                    try
                    {
                        NetworkClient.InitializeClient();
                    }
                    catch (Exception ex)
                    {
                        emmVRCLoader.Logger.LogError("Error occured while initializing network: " + ex.ToString());
                    }
                }
                // Initialize the Debug manager
                Managers.DebugManager.Initialize();

                emmVRCLoader.Logger.LogDebug("UI manager initialized");

                // Start trying to set up the menu music
                if (!Configuration.JSONConfig.StealthMode)
                    MelonLoader.MelonCoroutines.Start(Hacks.CustomMenuMusic.Initialize());

                // Load resources for emmVRC, including downloading them if needed
                //Resources.LoadResources();
                MelonLoader.MelonCoroutines.Start(Resources.LoadResources());

                // Initialize the "UI Elements" replacement buttons
                if (!Attributes.VRCPlusVersion)
                    MelonLoader.MelonCoroutines.Start(Hacks.UIElementsMenu.Initialize());

                // Initialize the Mod Compatibility system
                Libraries.ModCompatibility.Initialize();

                // If stealth mode is on, initialize the "Report World" menu
                if (Configuration.JSONConfig.StealthMode)
                    Menus.ReportWorldMenu.Initialize();

                // Initialize the "Functions" menu
                emmVRCLoader.Logger.LogDebug("Initializing functions menu...");
                Menus.FunctionsMenu.Initialize();

                // Initialize the "User Interact Actions" menu
                emmVRCLoader.Logger.LogDebug("Initializing user interact menu...");
                Menus.UserTweaksMenu.Initialize();

                // Initialize the "World Tweaks" menu
                Menus.WorldTweaksMenu.Initialize();

                // Initialize the "Player Tweaks" menu
                Menus.PlayerTweaksMenu.Initialize();

                // Initialize the "Disabled Buttons" menu
                emmVRCLoader.Logger.LogDebug("Initializing Disabled Buttons menu...");
                Menus.DisabledButtonMenu.Initialize();

                // Initialize the "Programs" menu
                emmVRCLoader.Logger.LogDebug("Initializing Programs menu...");
                Menus.ProgramMenu.Initialize();

                // Initialize the "Credits" menu
                Menus.CreditsMenu.Initialize();

                // Initialize the World Instance History menu
                Menus.InstanceHistoryMenu.Initialize();

                // Initialize the Player History menu
                Menus.PlayerHistoryMenu.Initialize();

                // Initialize the "Settings" menu
                emmVRCLoader.Logger.LogDebug("Initializing Settings menu...");
                Menus.SettingsMenu.Initialize();

                // Initialize the "Supporters" menu
                Menus.SupporterMenu.Initialize();

                // Initialize the "Changelog" menu
                Menus.ChangelogMenu.Initialize();

                // Initialize the "Debug" menu
                Menus.DebugMenu.Initialize();

                // Initialize the "Social Menu Functions" menu
                if (!Configuration.JSONConfig.StealthMode)
                    Hacks.SocialMenuFunctions.Initialize();

                // Initialize the User List Refresh button on the Social Menu
                if (!Configuration.JSONConfig.StealthMode)
                    Hacks.UserListRefresh.Initialize();

                // Initialize the "World Functions" menu
                if (!Configuration.JSONConfig.StealthMode)
                    Hacks.WorldFunctions.Initialize();

                // Initialize Player Notes
                if (!Configuration.JSONConfig.StealthMode)
                    Hacks.PlayerNotes.Initialize();

                // Initialize World Notes
                if (!Configuration.JSONConfig.StealthMode)
                    Hacks.WorldNotes.Initialize();

                // Initialize Avatar Property Saving
                Hacks.AvatarPropertySaving.Initialize();

                // Initialize the "Loading Screen Functions" menu
                if (!Configuration.JSONConfig.StealthMode)
                    Menus.LoadingScreenMenu.Initialize();

                // Initialize the Keybind Changing UI
                Libraries.KeybindChanger.Initialize();

                // Initialize the Keybind system
                Managers.KeybindManager.Initialize();

                // Start the Notification manager
                emmVRCLoader.Logger.LogDebug("Initializing Notification manager...");
                Managers.NotificationManager.Initialize();

                // Process the "Hacks" for the Shortcut Menu
                if (!Configuration.JSONConfig.StealthMode)
                    MelonLoader.MelonCoroutines.Start(Hacks.ShortcutMenuButtons.Process());

                // Process the "Hacks" for the User Interact Menu
                UserInteractMenuButtons.Initialize();

                // Process the "Hacks" for the Settings Menu (basically just logout)
                Hacks.LogoutPatch.Initialize();

                // Initialize the Quick Menu clock
                if (!Configuration.JSONConfig.StealthMode)
                    Hacks.InfoBarClock.Initialize();

                // Initialize the Quick Menu status
                if (!Configuration.JSONConfig.StealthMode)
                    Hacks.InfoBarStatus.Initialize();

                // Initialize the FBT saving system
                Hacks.FBTSaving.Initialize();

                // Initialize the Flight and Noclip module for Risky Functions
                Hacks.Flight.Initialize();

                // Initialize the Speed module for Risky Functions
                Hacks.Speed.Initialize();

                // Initialize the ESP module for Risky Functions
                Hacks.ESP.Initialize();

                // Initialize the Risky Functions manager
                Managers.RiskyFunctionsManager.Initialize();

                // Initialize the Waypoint menu for Risky Functions
                Menus.WaypointsMenu.Initialize();

                // Initialize the Avatar Menu hacks
                Hacks.AvatarMenu.Initialize();

                // Initialize the Info Spoof/Hider
                if (!Configuration.JSONConfig.StealthMode)
                    Hacks.InfoSpoofing.Initialize();

                // Initialize the Nameplate color changer
                Hacks.Nameplates.Initialize();

                // Initialize the Flashlight system
                Menus.FlashlightMenu.Initialize();

                // Change the FOV, if we want to
                Hacks.FOV.Initialize();

                // Change the target FPS to 200
                Hacks.FPS.Initialize();

                // Initialize the Action Menu hacks
                if (!Configuration.JSONConfig.StealthMode)
                    Hacks.ActionMenuTweaks.Apply();

                // Initialize the Volume hooker
                if (!Configuration.JSONConfig.StealthMode)
                    Hacks.Volume.Initialize();

                // Initialize the message system
                Managers.MessageManager.Initialize();

                // Initialize the custom Main Menu page system
                //Hacks.CustomMainMenuPage.Initialize();

                // Initialize the Main Menu Tweaks
                if (Attributes.VRCPlusVersion)
                    Hacks.MainMenuTweaks.Initialize();

                // Initialize the emmVRC HUD
                if (!Configuration.JSONConfig.StealthMode)
                {
                    if (Configuration.JSONConfig.VRHUDInDesktop || UnityEngine.XR.XRDevice.isPresent)
                        MelonLoader.MelonCoroutines.Start(Menus.VRHUD.Initialize());
                    else
                        MelonLoader.MelonCoroutines.Start(Menus.DesktopHUD.Initialize());
                }

                // Initialize the Third Person mode
                try
                {
                    Hacks.ThirdPerson.Initialize();
                }
                catch (Exception ex)
                {
                    emmVRCLoader.Logger.LogError(ex.ToString());
                }

                // Start the Master Icon Crown
                if (!Configuration.JSONConfig.StealthMode && !Attributes.VRCPlusVersion)
                    Hacks.MasterCrown.Initialize();
                // Start the Avatar Favorite system
                Hacks.CustomAvatarFavorites.Initialize();

                if (Configuration.JSONConfig.emmVRCNetworkEnabled)
                    MelonLoader.MelonCoroutines.Start(loadNetworked());

                // Applying some quick commands on OnSceneLoaded
                UnityEngine.SceneManagement.SceneManager.add_sceneLoaded(new System.Action<UnityEngine.SceneManagement.Scene, UnityEngine.SceneManagement.LoadSceneMode>((asa, asd) =>
                {
                    Hacks.MirrorTweaks.FetchMirrors();
                    Hacks.PedestalTweaks.FetchPedestals();
                    Menus.PlayerTweaksMenu.SpeedToggle.setToggleState(false, true);
                    Menus.PlayerTweaksMenu.FlightToggle.setToggleState(false, true);
                    Menus.PlayerTweaksMenu.NoclipToggle.setToggleState(false, true);
                    Menus.PlayerTweaksMenu.ESPToggle.setToggleState(false, true);
                    Menus.FlashlightMenu.toggleFlashlight.setToggleState(false);

                    MelonLoader.MelonCoroutines.Start(Menus.WaypointsMenu.LoadWorld());
                    MelonLoader.MelonCoroutines.Start(EULA.Initialize());
                    // Ensure that everything through here is after the game has loaded
                    // Reset the instance clock when you switch instances
                    MelonLoader.MelonCoroutines.Start(InstanceHistoryMenu.EnteredWorld());
                    if (Configuration.JSONConfig.ClockEnabled && Hacks.InfoBarClock.clockText != null && !Configuration.JSONConfig.StealthMode)
                        Hacks.InfoBarClock.instanceTime = 0;
                    MelonLoader.MelonCoroutines.Start(Managers.RiskyFunctionsManager.CheckThisWrld());
                    if (!Configuration.JSONConfig.StealthMode)
                        MelonLoader.MelonCoroutines.Start(Hacks.CustomWorldObjects.OnRoomEnter());
                    MelonLoader.MelonCoroutines.Start(Hacks.UIElementsMenu.OnSceneLoaded());
                    if (Configuration.JSONConfig.LastVersion != Attributes.Version)
                    {
                        Configuration.JSONConfig.LastVersion = Attributes.Version;
                        Configuration.SaveConfig();
                        Managers.NotificationManager.AddNotification("emmVRC has updated to version " + Attributes.Version + "!", "View\nChangelog", () => { Managers.NotificationManager.DismissCurrentNotification(); Menus.ChangelogMenu.baseMenu.OpenMenu(); }, "Dismiss", Managers.NotificationManager.DismissCurrentNotification, Resources.alertSprite, -1);
                    }
                    if (Libraries.ModCompatibility.MultiplayerDynamicBones && Configuration.JSONConfig.GlobalDynamicBonesEnabled)
                    {
                        Configuration.JSONConfig.GlobalDynamicBonesEnabled = false;
                        Configuration.SaveConfig();
                        Managers.NotificationManager.AddNotification("You are currently using MultiplayerDynamicBones. emmVRC's Global Dynamic Bones have been disabled, as only one can be used at a time.", "Dismiss", Managers.NotificationManager.DismissCurrentNotification, "", null, Resources.alertSprite, -1);
                    }
                    PlayerHistoryMenu.currentPlayers = new System.Collections.Generic.List<InstancePlayer>();

                }));

                // Initialize Avatar Permissions
                AvatarPermissionManager.Initialize();

                // Initialize User Permissions
                UserPermissionManager.Initialize();

                // Apply color changing
                if (!Configuration.JSONConfig.StealthMode)
                    Hacks.ColorChanger.ApplyIfApplicable();

                // Initialize hooking, for things such as Global Dynamic Bones
                Libraries.Hooking.Initialize();

                // At this point, if no errors have occured, emmVRC is done initializing
                watch.Stop();
                emmVRCLoader.Logger.Log("Initialization is successful in " + watch.Elapsed.ToString(@"ss\.f", null) + "s. Welcome to emmVRC!");
                emmVRCLoader.Logger.Log("You are running version " + Objects.Attributes.Version);
                if (Configuration.JSONConfig.StealthMode)
                {
                    emmVRCLoader.Logger.Log("You have emmVRC's Stealth Mode enabled. To access the functions menu, press the \"Report World\" button. Most visual functions of emmVRC have been disabled.");
                    if (Environment.CommandLine.Contains("--emmvrc.stealthmode"))
                        emmVRCLoader.Logger.Log("To disable stealth mode, remove '--emmvrc.stealthmode' from your launch options. Then, disable \"Stealth Mode\" in the emmVRC Settings, and restart the game.");
                    else
                        emmVRCLoader.Logger.Log("To disable stealth mode, disable \"Stealth Mode\" in the emmVRC Settings, and restart the game.");
                }
                Initialized = true;

                // Debug actions need to go before this
                DebugMenu.PopulateDebugMenu();
            }
        }

        public static System.Collections.IEnumerator loadNetworked()
        {
            while (NetworkClient.authToken == null)
                yield return new WaitForSeconds(1.5f);
            MelonLoader.MelonCoroutines.Start(CustomAvatarFavorites.PopulateList());
        }

        public static void OnUpdate()
        {
            if (!Initialized)
                return;
            // Check if resources have finished initializing
            if (Resources.onlineSprite != null)
            {
                // If the user is new to emmVRC, present the emmVRC Welcome message
                if (!Configuration.JSONConfig.WelcomeMessageShown)
                {
                    Managers.NotificationManager.AddNotification("Welcome to the new emmVRC! For updates regarding the client, teasers for new features, and bug reports and support, join the Discord!", "Open\nDiscord", () => { System.Diagnostics.Process.Start("https://discord.gg/SpZSH5Z"); Managers.NotificationManager.DismissCurrentNotification(); }, "Dismiss", () => { Managers.NotificationManager.DismissCurrentNotification(); }, Resources.alertSprite);
                    Configuration.JSONConfig.WelcomeMessageShown = true;
                    Configuration.SaveConfig();
                }
            }
            Hacks.CustomAvatarFavorites.OnUpdate();
        }
        public static void OnApplicationQuit()
        {
            if (!Initialized)
                return;
            if (NetworkClient.authToken != null)
                HTTPRequest.get(NetworkClient.baseURL + "/api/authentication/logout");
        }
    }
}
