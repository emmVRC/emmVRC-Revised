namespace emmVRC.Objects.ModuleBases
{
    public class MelonLoaderEvents
    {
        public virtual void OnApplicationStart() { }
        public virtual void OnUiManagerInit() { }
        public virtual void OnSceneWasLoaded(int buildIndex, string sceneName) { }
        public virtual void OnSceneWasInitialized(int buildIndex, string sceneName) { }
        public virtual void OnSceneWasUnloaded(int buildIndex, string sceneName) { }
        public virtual void OnApplicationQuit() { }

        public MelonLoaderEvents() { }
    }
}