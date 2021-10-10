using System;
using UnityEngine;
using emmVRC.Managers;
using emmVRC.Hacks;
using emmVRC.Network;
using emmVRC.Menus;
using emmVRC.Objects;
using System.Linq;
using System.Windows.Forms;
using emmVRC.Libraries;
using System.Collections;
using System.Collections.Generic;
using emmVRC.Objects.ModuleBases;
using System.Reflection;

#pragma warning disable 4014

namespace emmVRC
{
    public static class emmVRC
    {
        public static bool Initialized = false;
        public static readonly AwaitProvider AwaitUpdate = new AwaitProvider();

        // Compatibility checking modules go here, to check for known issues with the installation before proceeding to launch emmVRC.
        private readonly static IEnumerable<CompatibilityCheck> compatCheckers = typeof(emmVRC).Assembly.GetTypes()
            .Where(type => type.IsSubclassOf(typeof(CompatibilityCheck)))
            .Select(type => (CompatibilityCheck)Activator.CreateInstance(type));

        // Doing modules like this is far easier. There are a few limitations tho. All modules have to derive from MelonLoaderEvents, and have to be instance without a constructor
        // To use it just make a new class and inherit from MelonLoaderEvents and then override the methods and you're done. There's no need to add anything here.
        private readonly static IEnumerable<MelonLoaderEvents> eventListeners = typeof(emmVRC).Assembly.GetTypes()
            .Where(type => type.IsSubclassOf(typeof(MelonLoaderEvents)))
            .OrderBy((type) =>
            {
                PriorityAttribute priority = (PriorityAttribute)Attribute.GetCustomAttribute(type, typeof(PriorityAttribute));
                return priority == null ? 0 : priority.priority;
            })
            .Select(type => (MelonLoaderEvents)Activator.CreateInstance(type));

        // There's a special Interface with update because looping through the like 100 modules of emm on update every frame is kinda slow. To use it just impliment the interface
        // as well as all the steps from the regular class.
        private readonly static IEnumerable<IWithUpdate> eventListenersWithUpdate = Assembly.GetExecutingAssembly().GetTypes()
            .Where(type => typeof(IWithUpdate).IsAssignableFrom(type) && type != typeof(IWithUpdate))
            .OrderBy((type) =>
            {
                PriorityAttribute priority = (PriorityAttribute)Attribute.GetCustomAttribute(type, typeof(PriorityAttribute));
                return priority == null ? 0 : priority.priority;
            })
            .Select(type => (IWithUpdate)Activator.CreateInstance(type));

        // Same situations as WithUpdate and these ones
        private readonly static IEnumerable<IWithFixedUpdate> eventListenersWithFixedUpdate = Assembly.GetExecutingAssembly().GetTypes()
            .Where(type => typeof(IWithFixedUpdate).IsAssignableFrom(type) && type != typeof(IWithFixedUpdate))
            .OrderBy((type) =>
            {
                PriorityAttribute priority = (PriorityAttribute)Attribute.GetCustomAttribute(type, typeof(PriorityAttribute));
                return priority == null ? 0 : priority.priority;
            })
            .Select(type => (IWithFixedUpdate)Activator.CreateInstance(type));

        private readonly static IEnumerable<IWithLateUpdate> eventListenersWithLateUpdate = Assembly.GetExecutingAssembly().GetTypes()
            .Where(type => typeof(IWithLateUpdate).IsAssignableFrom(type) && type != typeof(IWithLateUpdate))
            .OrderBy((type) =>
            {
                PriorityAttribute priority = (PriorityAttribute)Attribute.GetCustomAttribute(type, typeof(PriorityAttribute));
                return priority == null ? 0 : priority.priority;
            })
            .Select(type => (IWithLateUpdate)Activator.CreateInstance(type));

        private readonly static IEnumerable<IWithGUI> eventListenersWithGUI = Assembly.GetExecutingAssembly().GetTypes()
            .Where(type => typeof(IWithGUI).IsAssignableFrom(type) && type != typeof(IWithGUI))
            .OrderBy((type) =>
            {
                PriorityAttribute priority = (PriorityAttribute)Attribute.GetCustomAttribute(type, typeof(PriorityAttribute));
                return priority == null ? 0 : priority.priority;
            })
            .Select(type => (IWithGUI)Activator.CreateInstance(type));

        // OnApplicationStart is called when emmVRCLoader passes over control to the emmVRC assembly.
        private static void OnApplicationStart()
        {
            foreach (MelonLoaderEvents listener in eventListeners)
            {
                try
                {
                    listener.OnApplicationStart();
                }
                catch (Exception ex)
                {
                    emmVRCLoader.Logger.LogError($"emmVRC encountered an exception while running OnApplicationStart of \"{listener.GetType().FullName}\":\n" + ex.ToString());
                }
            }

            // Load the config for emmVRC
            Configuration.Initialize();

            MelonLoader.MelonCoroutines.Start(WaitForUiManagerInit());
        }

        private static IEnumerator WaitForUiManagerInit()
        {
            while (VRCUiManager.field_Private_Static_VRCUiManager_0 == null)
                yield return null;

            OnUIManagerInit();
        }

        // OnUIManagerInit is the equivelent of the VRCUiManagerUtils.WaitForUIManagerInit, but better
        public static void OnUIManagerInit()
        {
            emmVRCLoader.Logger.LogDebug("UI manager initialized");

            if (compatCheckers.Any(a => a.RunCheck() == false))
            {
                emmVRCLoader.Logger.LogError("One or more compatibility issues were detected. See above for details. emmVRC cannot start.");
                MessageBox.Show("One or more compatibility issues were detected. See the console for more details. emmVRC cannot start.", "emmVRC", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } else { 
                var watch = new System.Diagnostics.Stopwatch();
                watch.Start();

                foreach (MelonLoaderEvents listener in eventListeners)
                {
                    try
                    {
                        listener.OnUiManagerInit();
                    }
                    catch (Exception ex)
                    {
                        emmVRCLoader.Logger.LogError($"emmVRC encountered an exception while running UiManagerInit of \"{listener.GetType().FullName}\":\n" + ex.ToString());
                    }
                }

                // Initialize the Nameplate color changer
                emmVRCLoader.Logger.LogDebug("Initializing Nameplate Color Changing module...");
                Hacks.Nameplates.Initialize();

                // Initialize the custom Main Menu page system
                //Hacks.CustomMainMenuPage.Initialize();

                if (!Configuration.JSONConfig.StealthMode)
                {
                    emmVRCLoader.Logger.LogDebug("Initializing Action Menu module...");
                    MelonLoader.MelonCoroutines.Start(Menus.ActionMenuFunctions.Initialize());
                }

                // Initialize the emmVRC HUD
                if (!Configuration.JSONConfig.StealthMode)
                {
                    if (Configuration.JSONConfig.VRHUDInDesktop || UnityEngine.XR.XRDevice.isPresent)
                        MelonLoader.MelonCoroutines.Start(Menus.VRHUD.Initialize());
                    else
                        MelonLoader.MelonCoroutines.Start(Menus.DesktopHUD.Initialize());
                }

                // Applying some quick commands on OnSceneLoaded
                emmVRCLoader.Logger.LogDebug("Processing OnSceneLoaded events...");
                UnityEngine.SceneManagement.SceneManager.add_sceneLoaded(new System.Action<UnityEngine.SceneManagement.Scene, UnityEngine.SceneManagement.LoadSceneMode>((asa, asd) =>
                {
                    OnSceneLoaded(asa, asd);
                }));

                // Initialize Avatar Permissions
                emmVRCLoader.Logger.LogDebug("Initializing Avatar Permissions module...");
                AvatarPermissionManager.Initialize();

                // Initialize User Permissions
                emmVRCLoader.Logger.LogDebug("Initializing User Permissions module...");
                UserPermissionManager.Initialize();

                if (!Configuration.JSONConfig.StealthMode)
                {
                    emmVRCLoader.Logger.LogDebug("Initializing User Info tweaks...");
                    Hacks.UserInfoTweaks.Initialize();
                }

                emmVRCLoader.Logger.LogDebug("Initializing Emoji Favourites system...");
                MelonLoader.MelonCoroutines.Start(Hacks.EmojiFavourites.Initialize());

                // Initialize hooking, for things such as Global Dynamic Bones
                emmVRCLoader.Logger.LogDebug("Initializing hooks...");
                Libraries.Hooking.Initialize();

                // At this point, if no errors have occured, emmVRC is done initializing
                watch.Stop();
                emmVRCLoader.Logger.Log("Initialization is successful in " + watch.Elapsed.ToString(@"ss\.f", null) + "s. Welcome to emmVRC!");
                emmVRCLoader.Logger.Log("You are running version " + Objects.Attributes.Version);


                Initialized = true;

                // Debug actions need to go before this
                DebugMenu.PopulateDebugMenu();
            }
        }

        public static void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode loadSceneMode)
        {
            Hacks.MirrorTweaks.FetchMirrors();
            Hacks.PedestalTweaks.FetchPedestals();
            MelonLoader.MelonCoroutines.Start(EULA.Initialize());

            //MelonLoader.MelonCoroutines.Start(Hacks.UIElementsMenu.OnSceneLoaded());
            if (Configuration.JSONConfig.LastVersion != Attributes.Version)
            {
                Configuration.JSONConfig.LastVersion = Attributes.Version;
                Configuration.SaveConfig();
                Managers.NotificationManager.AddNotification("emmVRC has updated to version " + Attributes.Version + "!", "View\nChangelog", () =>
                { Managers.NotificationManager.DismissCurrentNotification(); Menus.ChangelogMenu.baseMenu.OpenMenu(); }, "Dismiss", Managers.NotificationManager.DismissCurrentNotification, Functions.Core.Resources.alertSprite, -1);
            }
            if (Functions.Core.ModCompatibility.MultiplayerDynamicBones && Configuration.JSONConfig.GlobalDynamicBonesEnabled)
            {
                Configuration.JSONConfig.GlobalDynamicBonesEnabled = false;
                Configuration.SaveConfig();
                Managers.NotificationManager.AddNotification("You are currently using MultiplayerDynamicBones. emmVRC's Global Dynamic Bones have been disabled, as only one can be used at a time.", "Dismiss", Managers.NotificationManager.DismissCurrentNotification, "", null, Functions.Core.Resources.alertSprite, -1);
            }
            PlayerHistoryMenu.currentPlayers = new System.Collections.Generic.List<InstancePlayer>();

            #region Korty's Addons
            Hacks.ComponentToggle.OnLevelLoad();
            Hacks.ComponentToggle.Toggle();
            #endregion
        }
        public static void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            foreach (MelonLoaderEvents listener in eventListeners)
            {
                try
                {
                    listener.OnSceneWasLoaded(buildIndex, sceneName);
                }
                catch (Exception ex)
                {
                    emmVRCLoader.Logger.LogError($"emmVRC encountered an exception while running OnSceneLoad of \"{listener.GetType().FullName}\":\n" + ex.ToString());
                }
            }
        }

        public static void OnSceneWasInitialized(int buildIndex, string sceneName)
        {
            DateTime startTime = DateTime.Now;

            foreach (MelonLoaderEvents listener in eventListeners)
            {
                try
                {
                    listener.OnSceneWasInitialized(buildIndex, sceneName);
                }
                catch (Exception ex)
                {
                    emmVRCLoader.Logger.LogError($"emmVRC encountered an exception while running OnSceneInit of \"{listener.GetType().FullName}\":\n" + ex.ToString());
                }
            }
        }

        public static void OnSceneWasUnloaded(int buildIndex, string sceneName)
        {
            foreach (MelonLoaderEvents listener in eventListeners)
            {
                try
                {
                    listener.OnSceneWasUnloaded(buildIndex, sceneName);
                }
                catch (Exception ex)
                {
                    emmVRCLoader.Logger.LogError($"emmVRC encountered an exception while running OnSceneUnload of \"{listener.GetType().FullName}\":\n" + ex.ToString());
                }
            }
        }
        public static void OnUpdate()
        {
            AwaitUpdate.Flush();

            foreach (IWithUpdate listener in eventListenersWithUpdate)
            {
                try
                {
                    listener.OnUpdate();
                }
                catch (Exception ex)
                {
                    emmVRCLoader.Logger.LogError($"emmVRC encountered an exception while running OnUpdate of \"{listener.GetType().FullName}\":\n" + ex.ToString());
                }
            }

            if (!Initialized)
                return;
            // Check if resources have finished initializing
            if (Functions.Core.Resources.onlineSprite != null)
            {
                // If the user is new to emmVRC, present the emmVRC Welcome message
                if (!Configuration.JSONConfig.WelcomeMessageShown)
                {
                    Managers.NotificationManager.AddNotification("Welcome to the new emmVRC! For updates regarding the client, teasers for new features, and bug reports and support, join the Discord!", "Open\nDiscord", () =>
                    { System.Diagnostics.Process.Start("https://discord.gg/SpZSH5Z"); Managers.NotificationManager.DismissCurrentNotification(); }, "Dismiss", () => { Managers.NotificationManager.DismissCurrentNotification(); }, Functions.Core.Resources.alertSprite);
                    Configuration.JSONConfig.WelcomeMessageShown = true;
                    Configuration.SaveConfig();
                }
            }

        }
        public static void OnFixedUpdate()
        {
            foreach (IWithFixedUpdate listener in eventListenersWithFixedUpdate)
            {
                try
                {
                    listener.OnFixedUpdate();
                }
                catch (Exception ex)
                {
                    emmVRCLoader.Logger.LogError($"emmVRC encountered an exception while running OnFixedUpdate of \"{listener.GetType().FullName}\":\n" + ex.ToString());
                }
            }
        }
        public static void OnLateUpdate()
        {
            foreach (IWithLateUpdate listener in eventListenersWithLateUpdate)
            {
                try
                {
                    listener.LateUpdate();
                }
                catch (Exception ex)
                {
                    emmVRCLoader.Logger.LogError($"emmVRC encountered an exception while running OnLateUpdate of \"{listener.GetType().FullName}\":\n" + ex.ToString());
                }
            }
        }

        public static void OnGUI()
        {
            foreach (IWithGUI listener in eventListenersWithGUI)
            {
                try
                {
                    listener.OnGUI();
                }
                catch (Exception ex)
                {
                    emmVRCLoader.Logger.LogError($"emmVRC encountered an exception while running OnGUI of \"{listener.GetType().FullName}\":\n" + ex.ToString());
                }
            }
        }
        public static void OnApplicationQuit()
        {
            if (!Initialized)
                return;
            if (NetworkClient.webToken != null)
                HTTPRequest.get(NetworkClient.baseURL + "/api/authentication/logout").NoAwait("Logout");
        }
    }
}
