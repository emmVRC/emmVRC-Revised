using MelonLoader;

namespace emmVRCLoader
{
    public static class Logger
    {
        public static void Log(object obj) => MelonLogger.Msg(obj);

        public static void LogWarning(object obj) => MelonLogger.Warning(obj);

        // MelonLoader already has an error limit of 100(?) built in. its configurable as well I'm pretty sure
        public static void LogError(object obj) => MelonLogger.Error(obj);

        public static void LogDebug(object obj)
        {
            if (emmVRCLoaderMod.isDebug)
                MelonLogger.Msg("[Debug] " + obj?.ToString());
        }
    }
}
