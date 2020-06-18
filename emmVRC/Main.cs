using System;
using UnityEngine;
using UnityEngine.Events;
using emmVRC.Managers;
using emmVRC.Hacks;
using emmVRC.Network;
using emmVRC.Menus;
using emmVRC.Libraries;
using VRC;
using VRC.Core;
using VRC.SDKBase;
using VRCSDK2;
using emmVRC.Objects;

namespace emmVRC
{
    public static class emmVRC
    {
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

        }

        // OnUIManagerInit is the equivelent of the VRCUiManagerUtils.WaitForUIManagerInit, but better
        public static void OnUIManagerInit()
        {
            if (Configuration.JSONConfig.emmVRCNetworkEnabled)
            {
                // Network connection
                emmVRCLoader.Logger.Log("Initializing network...");
                try
                {
                    NetworkClient.InitializeClient();
                } catch (Exception ex)
                {
                    emmVRCLoader.Logger.LogError("Error occured while initializing network: " + ex.ToString());
                }
            }
            // Initialize the Debug manager
            Managers.DebugManager.Initialize();

            emmVRCLoader.Logger.LogDebug("UI manager initialized");

            // Start trying to set up the menu music
            MelonLoader.MelonCoroutines.Start(Hacks.CustomMenuMusic.Initialize());

            // Load resources for emmVRC, including downloading them if needed
            //Resources.LoadResources();
            MelonLoader.MelonCoroutines.Start(Resources.LoadResources());

            // Initialize the "UI Elements" replacement buttons
            MelonLoader.MelonCoroutines.Start(Hacks.UIElementsMenu.Initialize());

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

            // Initialize the "Settings" menu
            emmVRCLoader.Logger.LogDebug("Initializing Settings menu...");
            Menus.SettingsMenu.Initialize();

            // Initialize the "Supporters" menu
            Menus.SupporterMenu.Initialize();

            // Initialize the "Social Menu Functions" menu
            Hacks.SocialMenuFunctions.Initialize();

            // Initialize the "World Functions" menu
            Hacks.WorldFunctions.Initialize();

            // Initialize Player Notes
            Hacks.PlayerNotes.Initialize();

            // Initialize World Notes
            Hacks.WorldNotes.Initialize();

            // Initialize the "Loading Screen Functions" menu
            Menus.LoadingScreenMenu.Initialize();

            // Initialize the Keybind Changing UI
            Libraries.KeybindChanger.Initialize();

            // Initialize the Keybind system
            Managers.KeybindManager.Initialize();
            
            // Start the Notification manager
            emmVRCLoader.Logger.LogDebug("Initializing Notification manager...");
            Managers.NotificationManager.Initialize();

            // Process the "Hacks" for the Shortcut Menu
            MelonLoader.MelonCoroutines.Start(Hacks.ShortcutMenuButtons.Process());

            // Process the "Hacks" for the User Interact Menu
            UserInteractMenuButtons.Initialize();

            // Initialize the Quick Menu clock
            Hacks.InfoBarClock.Initialize();

            // Initialize the Quick Menu status
            Hacks.InfoBarStatus.Initialize();

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
            Hacks.InfoSpoofing.Initialize();

            // Initialize the Nameplate color changer
            Hacks.Nameplates.Initialize();

            // Change the FOV, if we want to
            Hacks.FOV.Initialize();

            // Change the target FPS to 200
            Hacks.FPS.Initialize();

            // Initialize the Volume hooker
            Hacks.Volume.Initialize();

            // Initialize the message system
            Managers.MessageManager.Initialize();

            // Initialize the emmVRC HUD
            if (Configuration.JSONConfig.VRHUDInDesktop || VRCTrackingManager.Method_Public_Static_Boolean_9())
                Menus.VRHUD.Initialize();
            else
                Menus.DesktopHUD.Initialize();

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

                MelonLoader.MelonCoroutines.Start(Menus.WaypointsMenu.LoadWorld());
                // Ensure that everything through here is after the game has loaded
                // Reset the instance clock when you switch instances
                MelonLoader.MelonCoroutines.Start(InstanceHistoryMenu.EnteredWorld());
                if (Configuration.JSONConfig.ClockEnabled && Hacks.InfoBarClock.clockText != null)
                    Hacks.InfoBarClock.instanceTime = 0;
                MelonLoader.MelonCoroutines.Start(Managers.RiskyFunctionsManager.CheckWorld());
                MelonLoader.MelonCoroutines.Start(Hacks.CustomWorldObjects.OnRoomEnter());
                MelonLoader.MelonCoroutines.Start(Hacks.UIElementsMenu.OnSceneLoaded());

            }));
            
            // Initialize Avatar Permissions
            AvatarPermissionManager.Initialize();

            // Initialize User Permissions
            UserPermissionManager.Initialize();

            // Apply color changing
            Hacks.ColorChanger.ApplyIfApplicable();

            // Initialize hooking, for things such as Global Dynamic Bones
            Libraries.Hooking.Initialize();

            // At this point, if no errors have occured, emmVRC is done initializing
            emmVRCLoader.Logger.Log("Initialization is successful. Welcome to emmVRC!");
            emmVRCLoader.Logger.Log("You are running version " + Objects.Attributes.Version);
            
        }

        public static System.Collections.IEnumerator loadNetworked()
        {
            while (NetworkClient.authToken == null)
                yield return new WaitForSeconds(1.5f);
            MelonLoader.MelonCoroutines.Start(CustomAvatarFavorites.PopulateList());
        }

        public static void OnUpdate()
        {
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
            if (NetworkClient.authToken != null)
                HTTPRequest.get(NetworkClient.baseURL + "/api/authentication/logout");
        }
    }
}
