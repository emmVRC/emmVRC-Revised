using emmVRC.Objects.ModuleBases;
using System.Linq;
using System.Reflection;
using VRC.UI;
using VRC.Core;

namespace emmVRC.Utils
{
    public class WorldUtils : MelonLoaderEvents
    {
        private static MethodInfo joinWorldFromWorldPage;

        public override void OnApplicationStart()
        {
            // This is the method called by the world page to join a world, it sends all the proper info and things
            joinWorldFromWorldPage = typeof(PageWorldInfo).GetMethods()
                .Where(mb => mb.Name.StartsWith("Method_Private_Void_"))// && !mb.Name.Contains("_PDM_"))
                .First(mb => XrefUtils.CheckMethod(mb, "WorldInfo_Go"));
        }

        public static void JoinWorld(ApiWorld world, ApiWorldInstance instance)
        {
            Singletons.pageWorldInfo.field_Private_ApiWorld_0 = world;
            Singletons.pageWorldInfo.field_Public_ApiWorldInstance_0 = instance;
            joinWorldFromWorldPage.Invoke(Singletons.pageWorldInfo, null);
        }
    }
}