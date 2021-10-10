using System;
using System.Reflection;
using UnhollowerRuntimeLib;
using UnityEngine;
using emmVRC.Objects.ModuleBases;

namespace emmVRC.Components
{
    public class ComponentManager : MelonLoaderEvents
    {
        public override void OnApplicationStart()
        {
            emmVRCLoader.Logger.LogDebug("Registering types in Il2Cpp");
            foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
                RegisterTypeRecursive(type);
        }

        private static void RegisterTypeRecursive(Type t)
        {
            if (t == null || !t.IsSubclassOf(typeof(MonoBehaviour)))
                return;

            emmVRCLoader.Logger.LogDebug(t.FullName);
            ClassInjector.RegisterTypeInIl2Cpp(t, false);
        }
    }
}