using System;
using System.Reflection;

namespace emmVRCLoader
{
    public static class Bootstrapper
    {
        public static ModController mod;

        public static void Start()
        {
            if ((Environment.CommandLine.Contains("--noemmvrc") || !UpdateManager.ShouldLoadLib()) && !Environment.CommandLine.Contains("--emmvrc.devmode"))
                return;

            byte[] buffer = null;
            try
            {
                buffer = UpdateManager.Check();
            }
            catch (Exception ex)
            {
                Logger.LogError("Error occured while trying to update emmVRC: " + ex.ToString());
                Logger.Log("Continuing to try to load emmVRC...");
            }

            if (buffer == null)
            {
                Logger.Log("Could not load emmVRC. Sorry...");
                return;
            }

            try
            {
                Assembly assembly = Assembly.Load(buffer);
                try
                {
                    Type emmMain = assembly.GetType("emmVRC.emmVRCMain");
                    if (emmMain == null)
                        emmMain = assembly.GetType("emmVRC.emmVRC");
                    mod = new ModController(emmMain);
                    mod.OnApplicationStart();
                }
                catch (Exception ex)
                {
                    Logger.LogError("emmVRC failed! " + ex.ToString());
                }
            }
            catch (Exception ex)
            {
                Logger.LogError("Something has gone wrong... " + ex.ToString());
            }
        }
    }

}