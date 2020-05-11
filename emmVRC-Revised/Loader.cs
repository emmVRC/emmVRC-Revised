using MelonLoader;
using System.IO;
using NET_SDK;
using NET_SDK.Reflection;

namespace emmVRCLoader
{
    public static class BuildInfo
    {
        public const string Name = "emmVRCLoader";
        public const string Author = "The emmVRC Team";
        public const string Company = "emmVRC";
        public const string Version = "0.0.2";
        public const string DownloadLink = "https://www.thetrueyoshifan.com/downloads/emmVRCLoader.dll";
    }

    public class emmVRCLoader : MelonMod
    {
        public override void OnApplicationStart()
        {
            //MelonModLogger.Log("OnApplicationStart");
            //MelonModLogger.Log(Directory.GetCurrentDirectory());
            Bootstrapper.Start();
        }

        public override void OnLevelWasLoaded(int level)
        {
            if (Bootstrapper.mod != null)
            Bootstrapper.mod.OnLevelWasLoaded(level);
            // Currently only works in MUPOT Mode
            //MelonModLogger.Log("OnLevelWasLoaded");
        }

        public override void OnLevelWasInitialized(int level)
        {
            if (Bootstrapper.mod != null)
                Bootstrapper.mod.OnLevelWasInitialized(level);
            // Currently only works in MUPOT Mode
            //MelonModLogger.Log("OnLevelWasInitialized");
        }

        private bool is_pressed = false;
        public override void OnUpdate()
        {
            if (Bootstrapper.mod != null)
                Bootstrapper.mod.OnUpdate();
            // Currently only works in MUPOT Mode
            //MelonModLogger.Log("OnUpdate");
        }

        public override void OnFixedUpdate()
        {
            if (Bootstrapper.mod != null)
                Bootstrapper.mod.OnFixedUpdate();
            // Currently only works in MUPOT Mode
            //MelonModLogger.Log("OnFixedUpdate");
        }

        public override void OnLateUpdate()
        {
            if (Bootstrapper.mod != null)
                Bootstrapper.mod.OnLateUpdate();
            // Currently only works in MUPOT Mode
            //MelonModLogger.Log("OnLateUpdate");
        }

        public override void OnGUI()
        {
            if (Bootstrapper.mod != null)
                Bootstrapper.mod.OnGUI();
            // Currently only works in MUPOT Mode
            //MelonModLogger.Log("OnGUI");
        }

        public override void OnApplicationQuit()
        {
            if (Bootstrapper.mod != null)
                Bootstrapper.mod.OnApplicationQuit();
            // MelonModLogger.Log("OnApplicationQuit");
        }

        public override void OnModSettingsApplied()
        {
            //MelonModLogger.Log("OnModSettingsApplied");
        }
        public override void VRChat_OnUiManagerInit()
        {
            if (Bootstrapper.mod != null)
                Bootstrapper.mod.OnUIManagerInit();
        }
    }
}
