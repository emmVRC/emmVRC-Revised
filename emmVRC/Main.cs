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
            emmVRCLoader.Logger.Log("UI manager initialized");
            // Load resources for emmVRC, including downloading them if needed
            MelonLoader.MelonCoroutines.Start(Resources.LoadResources());

            // Initialize the "Functions" menu
            emmVRCLoader.Logger.LogDebug("Initializing functions menu...");
            Menus.FunctionsMenu.Initialize();

            // Initialize the "Disabled Buttons" menu
            emmVRCLoader.Logger.LogDebug("Initializing Disabled Buttons menu...");
            Menus.DisabledButtonMenu.Initialize();

            // Initialize the "Settings" menu
            emmVRCLoader.Logger.LogDebug("Initializing Settings menu...");
            Menus.SettingsMenu.Initialize();

            // Start the Notification manager
            emmVRCLoader.Logger.LogDebug("Initializing Notification manager...");
            Managers.NotificationManager.Initialize();

            // Process the "Hacks" for the Shortcut Menu
            Hacks.ShortcutMenuButtons.Process();

            // Initialize the Quick Menu clock
            Hacks.InfoBarClock.Initialize();

            // Initialize the Quick Menu status
            Hacks.InfoBarStatus.Initialize();

            // Initialize the Info Spoof/Hider
            // Hacks.InfoSpoofing.Initialize();

            // Initialize the Nameplate color changer
            Hacks.Nameplates.Initialize();

            // Change the FOV, if we want to
            Hacks.FOV.Initialize();

            // Change the target FPS to 200
            Hacks.FPS.Initialize();

            // Start the Master Icon Crown
            Hacks.MasterCrown.Initialize();

            // Start the Avatar Favorite system
            Hacks.CustomAvatarFavorites.Initialize();

            //Patches.AvatarLoading.Apply();

            // At this point, if no errors have occured, emmVRC is done initializing
            emmVRCLoader.Logger.Log("Initialization is successful. Welcome to emmVRC!");
            emmVRCLoader.Logger.Log("You are running version " + Objects.Attributes.Version);
            
        }
        public static void OnLevelWasLoaded(int level)
        {
            emmVRCLoader.Logger.Log("Level was loaded: " + level);
            // Ensure that everything through here is after the game has loaded
            if (level == -1)
            {
                // Reset the instance clock when you switch instances
                if (Configuration.JSONConfig.ClockEnabled && Hacks.InfoBarClock.clockText != null)
                    Hacks.InfoBarClock.instanceTime = 0;


            }
        }
        public static void OnUpdate()
        {
            // Check if resources have finished initializing
            if (Resources.onlineSprite != null)
            {
                // If the user is new to emmVRC, present the emmVRC Welcome message
                if (!Configuration.JSONConfig.WelcomeMessageShown)
                {
                    Managers.NotificationManager.AddNotification("Welcome to the new emmVRC! For updates regarding the client, teasers for new features, and bug reports and support, join the Discord!", "Open\nDiscord", () => { System.Diagnostics.Process.Start("https://discord.gg/SpZSH5Z"); Managers.NotificationManager.DismissCurrentNotification(); }, "Dismiss",() => { Managers.NotificationManager.DismissCurrentNotification(); }, Resources.alertSprite);
                    Configuration.JSONConfig.WelcomeMessageShown = true;
                    Configuration.SaveConfig();
                }
            }
            Hacks.CustomAvatarFavorites.OnUpdate();            
        }
        

        // OnApplicationQuit is called when the game is trying to quit.
    }
}
