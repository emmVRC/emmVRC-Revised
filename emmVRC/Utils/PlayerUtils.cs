using emmVRC.Objects.ModuleBases;
using System.Linq;
using System.Reflection;

namespace emmVRC.Utils
{
    public class PlayerUtils : MelonLoaderEvents
    {
        private static MethodInfo _reloadAvatarMethod;
        private static MethodInfo _reloadAllAvatarsMethod;
        public override void OnApplicationStart()
        {
            _reloadAvatarMethod = typeof(VRCPlayer).GetMethods().First(mi => mi.Name.StartsWith("Method_Private_Void_Boolean_") && mi.Name.Length < 31 && mi.GetParameters().Any(pi => pi.IsOptional));
            _reloadAllAvatarsMethod = typeof(VRCPlayer).GetMethods().Last(mi => mi.Name.StartsWith("Method_Public_Void_Boolean_") && mi.Name.Length < 30 && mi.GetParameters().Any(pi => pi.IsOptional));
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