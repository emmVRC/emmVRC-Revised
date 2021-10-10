using HarmonyLib;
using System;
using System.Reflection;

namespace emmVRCLoader
{
    public class ModController
    {
        private readonly MethodInfo onApplicationStartMethod;
        private readonly MethodInfo onApplicationQuitMethod;
        private readonly MethodInfo onSceneWasLoadedMethod;
        private readonly MethodInfo onSceneWasInitializedMethod;
        private readonly MethodInfo onSceneWasUnloadedMethod;
        private readonly MethodInfo onUpdateMethod;
        private readonly MethodInfo onFixedUpdateMethod;
        private readonly MethodInfo onLateUpdateMethod;
        private readonly MethodInfo onGUIMethod;

        public ModController(Type mod)
        {
            onApplicationStartMethod = mod.GetMethod(nameof(OnApplicationStart), AccessTools.all);
            onApplicationQuitMethod = mod.GetMethod(nameof(OnApplicationQuit), AccessTools.all);
            onSceneWasLoadedMethod = mod.GetMethod(nameof(OnSceneWasLoaded), AccessTools.all);
            onSceneWasInitializedMethod = mod.GetMethod(nameof(OnSceneWasInitialized), AccessTools.all);
            onSceneWasUnloadedMethod = mod.GetMethod(nameof(OnSceneWasUnloaded), AccessTools.all);
            onUpdateMethod = mod.GetMethod(nameof(OnUpdate), AccessTools.all);
            onFixedUpdateMethod = mod.GetMethod(nameof(OnFixedUpdate), AccessTools.all);
            onLateUpdateMethod = mod.GetMethod(nameof(OnLateUpdate), AccessTools.all);
            onGUIMethod = mod.GetMethod(nameof(OnGUI), AccessTools.all);
        }

        public void OnApplicationStart() => onApplicationStartMethod?.Invoke(null, null);
        public void OnApplicationQuit() => onApplicationQuitMethod?.Invoke(null, null);
        public void OnSceneWasLoaded(int buildIndex, string sceneName) => onSceneWasLoadedMethod?.Invoke(null, new object[2] { buildIndex, sceneName });
        public void OnSceneWasInitialized(int buildIndex, string sceneName) => onSceneWasInitializedMethod?.Invoke(null, new object[2] { buildIndex, sceneName });
        public void OnSceneWasUnloaded(int buildIndex, string sceneName) => onSceneWasUnloadedMethod?.Invoke(null, new object[2] { buildIndex, sceneName });
        public void OnUpdate() => onUpdateMethod?.Invoke(null, null);
        public void OnFixedUpdate() => onFixedUpdateMethod?.Invoke(null, null);
        public void OnLateUpdate() => onLateUpdateMethod?.Invoke(null, null);
        public void OnGUI() => onGUIMethod?.Invoke(null, null);
    }
}