using MelonLoader;
using System;
using System.Reflection;

#pragma warning disable IDE1006 // Naming Styles

namespace emmVRCLoader
{
    [Obfuscation(Exclude = true)]
    public static class BuildInfo
    {
        public const string Name = "emmVRCLoader";
        public const string Author = "The emmVRC Team";
        public const string Company = "emmVRC";
        public const string Version = "1.5.0";
        public const string DownloadLink = "https://dl.emmvrc.com/downloads/emmVRCLoader.dll";
    }

    // Never have your class the same name as the namespace it's just annoying
    [Obfuscation(Exclude = true)]
    public class emmVRCLoaderMod : MelonMod
    {
        public static emmVRCLoaderMod instance;
        public static bool isDebug;
        public static bool isParanoidMode;
        public override void OnApplicationStart()
        {
            isDebug = Environment.CommandLine.Contains("--emmvrc.debug");
            isParanoidMode = Environment.CommandLine.Contains("--emmvrc.paranoid");
            instance = this;
            Bootstrapper.Start();
        }
        public override void OnSceneWasLoaded(int buildIndex, string sceneName) => Bootstrapper.mod?.OnSceneWasLoaded(buildIndex, sceneName);
        public override void OnSceneWasInitialized(int buildIndex, string sceneName) => Bootstrapper.mod?.OnSceneWasInitialized(buildIndex, sceneName);
        public override void OnSceneWasUnloaded(int buildIndex, string sceneName) => Bootstrapper.mod?.OnSceneWasUnloaded(buildIndex, sceneName);
        public override void OnUpdate() => Bootstrapper.mod?.OnUpdate();
        public override void OnFixedUpdate() => Bootstrapper.mod?.OnFixedUpdate();
        public override void OnLateUpdate() => Bootstrapper.mod?.OnLateUpdate();
        public override void OnGUI() => Bootstrapper.mod?.OnGUI();
        public override void OnApplicationQuit() => Bootstrapper.mod?.OnApplicationQuit();
    }
}