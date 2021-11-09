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
using emmVRC.Utils;

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
            // Load the config for emmVRC
            Configuration.Initialize();
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
            }
            else
            {
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

                // At this point, if no errors have occured, emmVRC is done initializing
                watch.Stop();
                emmVRCLoader.Logger.Log("Initialization is successful in " + watch.Elapsed.ToString(@"ss\.f", null) + "s. Welcome to emmVRC!");
                emmVRCLoader.Logger.Log("You are running version " + Objects.Attributes.Version);

                Initialized = true;
                //DebugManager.DebugActions.Add(new DebugAction
                //{
                //    ActionKey = KeyCode.Alpha0,
                //    ActionAction = () =>
                //    {
                //        VRC.DataModel.IUser user = RoomManager.field_Private_Static_List_1_IUser_0.ToArray().First(a => a.ID == VRC.Core.APIUser.CurrentUser.id);
                //        Utils.ButtonAPI.GetQuickMenuInstance().ShowSelectedUserPage(true, user);
                //    }
                //});

            }
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
                    Managers.emmVRCNotificationsManager.AddNotification(new Notification("Welcome to emmVRC!", null, "Welcome to emmVRC! For updates regarding the client, teasers for new features, and bug reports and support, join the Discord!", true, true, () => { Utils.ButtonAPI.GetQuickMenuInstance().AskConfirmOpenURL("https://discord.gg/emmVRC"); }, "Open Discord", "Open the invite to our Discord", true, null, "Dismiss", "Dismisses the notification"));
                    Configuration.WriteConfigOption("WelcomeMessageShown", true);
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
            NetworkClient.Logout();
        }
    }
}
