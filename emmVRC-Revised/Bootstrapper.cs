using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace emmVRCLoader
{
    public static class Bootstrapper
    {
        public static ModController mod { get; set; }
        private static bool freshlyLoaded = false;
        public static void Start()
        {
            Logger.Init();
            if (Environment.CurrentDirectory.Contains("oculus") || Environment.CurrentDirectory.Contains("Oculus") || Environment.CommandLine.Contains("--testoculusmode"))
            {
                Logger.LogError("[emmVRCLoader] We suspect you are using emmVRC on the Oculus build of VRChat. This is not supported, and errors reported with the Oculus version can not be fixed. Please install and run VRChat through Steam to use emmVRC and other VRChat mods.");
                System.Windows.Forms.MessageBox.Show("emmVRC has detected that you are using the Oculus build of VRChat. This is not supported, and errors reported with the Oculus version can not be fixed. Please install and run VRChat through Steam to use emmVRC and other VRChat mods.", "emmVRC", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            if ((!Environment.CommandLine.Contains("--noemmvrc") && UpdateManager.ShouldLoadLib()) || Environment.CommandLine.Contains("--emmvrc.devmode"))
            {
                try
                {
                    UpdateManager.Check();
                }
                catch (Exception ex)
                {
                    Logger.Log("[emmVRCLoader] Error occured while trying to update emmVRC: " + ex.ToString());
                }
                if (UpdateManager.downloadedLib == null)
                {
                    Logger.Log("[emmVRCLoader] Could not load emmVRC. Sorry...");
                }
                else
                {
                    try
                    {
                        byte[] data = UpdateManager.downloadedLib;
                        UpdateManager.downloadedLib = null;
                        Assembly modasm = Assembly.Load(data);
                        foreach (Type t in GetLoadableTypes(modasm))
                        {
                            if ("emmVRC".Equals(t.Name))
                            {
                                try
                                {
                                    mod = new ModController();
                                    mod.Create(t);
                                    OnApplicationStart();
                                }
                                catch (Exception e)
                                {
                                    Logger.LogError("[emmVRCLoader] emmVRC failed! " + e);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError("[emmVRCLoader] Something has gone wrong... " + ex.ToString());
                    }
                }
            }
        }
        public static void Update()
        {
            if (freshlyLoaded)
            {
                freshlyLoaded = false;
                mod.OnLevelWasInitialized(0);
            }
            mod.OnUpdate();
        }
        public static void LateUpdate()
        {
            mod.OnLateUpdate();
        }

        public static void FixedUpdate()
        {
            mod.OnFixedUpdate();
        }

        public static void OnGUI()
        {
            mod.OnGUI();
        }

        public static void OnApplicationStart()
        {
            Logger.Log("[emmVRCLoader] OnApplicationStart called");
            mod.OnApplicationStart();
        }

        public static void OnApplicationQuit()
        {
            Logger.Log("[emmVRCLoader] OnApplicationQuit called");
            mod.OnApplicationQuit();
        }

        public static void OnLevelWasLoaded(int level)
        {
            Logger.Log("[emmVRCLoader] OnLevelWasLoaded called (" + level + ")");
            freshlyLoaded = true;
            mod.OnLevelWasLoaded(level);
        }
        public static void OnUIManagerInit()
        {
            Logger.Log("[emmVRCLoader] OnUIManagerInit called");
            mod.OnUIManagerInit();
        }

        public static IEnumerable<Type> GetLoadableTypes(Assembly assembly)
        {
            if (assembly == null) throw new ArgumentNullException(nameof(assembly));
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                Logger.LogError("[emmVRCLoader] An error occured while getting types from assembly " + assembly.GetName().Name + ". Returning types from error.\n" + e);
                return e.Types.Where(t => t != null);
            }
        }
    }

}
