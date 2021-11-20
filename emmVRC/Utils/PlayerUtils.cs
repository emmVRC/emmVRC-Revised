using emmVRC.Objects.ModuleBases;
using System.Linq;
using System.Reflection;
using VRC;
using VRC.Core;

namespace emmVRC.Utils
{
    public class PlayerUtils : MelonLoaderEvents
    {
        private static MethodInfo _reloadAvatarMethod;
        private static MethodInfo _reloadAllAvatarsMethod;
        public override void OnUiManagerInit()
        {
            _reloadAvatarMethod = typeof(VRCPlayer).GetMethods().First(mi => mi.Name.StartsWith("Method_Private_Void_Boolean_") && mi.Name.Length < 31 && mi.GetParameters().Any(pi => pi.IsOptional));
            _reloadAllAvatarsMethod = typeof(VRCPlayer).GetMethods().First(mi => mi.Name.StartsWith("Method_Public_Void_Boolean_") && mi.Name.Length < 30 && mi.GetParameters().All(pi => pi.IsOptional) && XrefUtils.CheckUsedBy(mi, "Method_Public_Void_", typeof(FeaturePermissionManager)));// Both methods seem to do the same thing;
            emmVRCLoader.Logger.LogDebug(_reloadAllAvatarsMethod.Name);
        }

        

        public static void ReloadAllAvatars()
        {
            _reloadAllAvatarsMethod.Invoke(VRCPlayer.field_Internal_Static_VRCPlayer_0, new object[] { true });
            _reloadAvatarMethod.Invoke(VRCPlayer.field_Internal_Static_VRCPlayer_0, new object[] { true });
        }

        public static void ReloadAvatar(VRCPlayer player)
        {
            _reloadAvatarMethod.Invoke(player, new object[] { true });
        }
        public static bool DoesUserHaveVRCPlus()
        {
            return VRC.Core.APIUser.CurrentUser.isSupporter;
        }
    }
}