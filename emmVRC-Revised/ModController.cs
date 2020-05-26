using System;
using System.Reflection;

namespace emmVRCLoader
{
    [Obfuscation(Exclude = true)]
    public class ModController
    {
        private MethodInfo onApplicationStartMethod;
        private MethodInfo onApplicationQuitMethod;
        private MethodInfo onLevelWasLoadedMethod;
        private MethodInfo onLevelWasInitializedMethod;
        private MethodInfo onUpdateMethod;
        private MethodInfo onFixedUpdateMethod;
        private MethodInfo onLateUpdateMethod;
        private MethodInfo onGUIMethod;
        private MethodInfo onModSettingsApplied;
        private MethodInfo onUIManagerInit;

        public void Create(Type mod)
        {
            MethodInfo[] methods = mod.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (MethodInfo method in methods)
            {
                if (method.Name.Equals("OnApplicationStart") && method.GetParameters().Length == 0)
                    onApplicationStartMethod = method;
                if (method.Name.Equals("OnApplicationQuit") && method.GetParameters().Length == 0)
                    onApplicationQuitMethod = method;
                if (method.Name.Equals("OnLevelWasLoaded") && method.GetParameters().Length == 1 && method.GetParameters()[0].ParameterType == typeof(int))
                    onLevelWasLoadedMethod = method;
                if (method.Name.Equals("OnLevelWasInitialized") && method.GetParameters().Length == 1 && method.GetParameters()[0].ParameterType == typeof(int))
                    onLevelWasInitializedMethod = method;
                if (method.Name.Equals("OnUpdate") && method.GetParameters().Length == 0)
                    onUpdateMethod = method;
                if (method.Name.Equals("OnFixedUpdate") && method.GetParameters().Length == 0)
                    onFixedUpdateMethod = method;
                if (method.Name.Equals("OnLateUpdate") && method.GetParameters().Length == 0)
                    onLateUpdateMethod = method;
                if (method.Name.Equals("OnGUI") && method.GetParameters().Length == 0)
                    onGUIMethod = method;
                if (method.Name.Equals("OnModSettingsApplied") && method.GetParameters().Length == 0)
                    onModSettingsApplied = method;
                if (method.Name.Equals("OnUIManagerInit") && method.GetParameters().Length == 0)
                    onUIManagerInit = method;
            }
        }

        public virtual void OnApplicationStart() => onApplicationStartMethod?.Invoke(null, new object[] { });
        public virtual void OnApplicationQuit() => onApplicationQuitMethod?.Invoke(null, new object[] { });
        public virtual void OnLevelWasLoaded(int level) => onLevelWasLoadedMethod?.Invoke(null, new object[] { level });
        public virtual void OnLevelWasInitialized(int level) => onLevelWasInitializedMethod?.Invoke(null, new object[] { level });
        public virtual void OnUpdate() => onUpdateMethod?.Invoke(null, new object[] { });
        public virtual void OnFixedUpdate() => onFixedUpdateMethod?.Invoke(null, new object[] { });
        public virtual void OnLateUpdate() => onLateUpdateMethod?.Invoke(null, new object[] { });
        public virtual void OnGUI() => onGUIMethod?.Invoke(null, new object[] { });
        public virtual void OnModSettingsApplied() => onModSettingsApplied?.Invoke(null, new object[] { });
        public virtual void OnUIManagerInit() => onUIManagerInit?.Invoke(null, new object[] { });
    }
}