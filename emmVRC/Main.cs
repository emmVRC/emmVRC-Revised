using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using VRC;
using VRC.Core;
using emmVRC.Libraries;
using NET_SDK.Harmony;
using emmVRC.Managers;
using Il2CppSystem.IO;
using emmVRC.Objects;
using emmVRC.Hacks;
using emmVRC.Network;
using MS.Internal.Xml.XPath;

namespace emmVRC
{
    public static class emmVRC
    {
        // OnApplicationStart is called when emmVRCLoader passes over control to the emmVRC assembly.
        private static void OnApplicationStart()
        {
            // A little throwback to the first ever mod I worked on, YoshiMod
            emmVRCLoader.Logger.Log("Hello world!");
            emmVRCLoader.Logger.Log("This is the beginning of a new beginning!");
            emmVRCLoader.Logger.Log("Wait... have I said that before?");

            // Load the config for emmVRC
            Configuration.Initialize();

        }

        // OnUIManagerInit is the equivelent of the VRCUiManagerUtils.WaitForUIManagerInit, but better
        public static void OnUIManagerInit()
        {

            // Network connection
            emmVRCLoader.Logger.Log("Initializing network...");
            NetworkClient.InitializeClient(null);
                
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
            //Menus.InstanceHistoryMenu.Initialize();

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

            // Start the Notification manager
            emmVRCLoader.Logger.LogDebug("Initializing Notification manager...");
            Managers.NotificationManager.Initialize();

            // Process the "Hacks" for the Shortcut Menu
            MelonLoader.MelonCoroutines.Start(Hacks.ShortcutMenuButtons.Process());

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

            // Initialize the emmVRC HUD
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

            // Start the emmVRC Network client
            MelonLoader.MelonCoroutines.Start(loadNetworked());

            // Applying some quick commands on OnSceneLoaded
            UnityEngine.SceneManagement.SceneManager.add_sceneLoaded(UnhollowerRuntimeLib.DelegateSupport.ConvertDelegate<UnityAction<UnityEngine.SceneManagement.Scene, UnityEngine.SceneManagement.LoadSceneMode>>((System.Action<UnityEngine.SceneManagement.Scene, UnityEngine.SceneManagement.LoadSceneMode>)((asa, asd) =>
            {
                Hacks.MirrorTweaks.FetchMirrors();
                Menus.PlayerTweaksMenu.SpeedToggle.setToggleState(false, true);
                Menus.PlayerTweaksMenu.FlightToggle.setToggleState(false, true);
                Menus.PlayerTweaksMenu.NoclipToggle.setToggleState(false, true);
                Menus.PlayerTweaksMenu.ESPToggle.setToggleState(false, true);

                MelonLoader.MelonCoroutines.Start(Menus.WaypointsMenu.LoadWorld());
                // Ensure that everything through here is after the game has loaded
                // Reset the instance clock when you switch instances
                if (Configuration.JSONConfig.ClockEnabled && Hacks.InfoBarClock.clockText != null)
                    Hacks.InfoBarClock.instanceTime = 0;
                MelonLoader.MelonCoroutines.Start(Managers.RiskyFunctionsManager.CheckWorld());
                MelonLoader.MelonCoroutines.Start(Hacks.CustomWorldObjects.OnRoomEnter());
            })));

            Patches.AvatarLoading.Apply();

            // At this point, if no errors have occured, emmVRC is done initializing
            emmVRCLoader.Logger.Log("Initialization is successful. Welcome to emmVRC!");
            emmVRCLoader.Logger.Log("You are running version " + Objects.Attributes.Version);

            // As a last-ditch effort, try changing colors
            Hacks.ColorChanger.ApplyIfApplicable();
        }

        public static System.Collections.IEnumerator loadNetworked()
        {
            while (NetworkClient.authToken == null)
                yield return new WaitForSeconds(1.5f);
            CustomAvatarFavorites.PopulateList();
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
            HTTPRequest.get_sync(NetworkClient.baseURL + "api/authentication/logout");
        }
    }
}
