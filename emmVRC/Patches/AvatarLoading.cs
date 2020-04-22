using System;
using MelonLoader;
using Harmony;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Il2CppSystem.Reflection;
using UnhollowerBaseLib;
using UnhollowerRuntimeLib;
using Il2CppSystem;

namespace emmVRC.Patches
{
    public class AvatarLoading
    {
        public static NET_SDK.Harmony.Patch avatarLoadingPatch;
        public static void Apply()
        {
            
            /*NET_SDK.Harmony.Instance instance = NET_SDK.Harmony.Manager.CreateInstance("AvatarLoadingPatcher");
            NET_SDK.Reflection.IL2CPP_Method methIsBadMkay = null;
            foreach (NET_SDK.Reflection.IL2CPP_Method method in NET_SDK.SDK.GetClass("VRCAvatarManager").GetMethods(NET_SDK.Reflection.IL2CPP_BindingFlags.METHOD_PRIVATE))
            {
                emmVRCLoader.Logger.Log("[emmVRC] Boop!");
                if (method.GetParameterCount() != 1) return;
                if (method.GetParameters().First().Ptr != IL2CPP.il2cpp_class_get_type(NET_SDK.SDK.GetClass("UnityEngine.GameObject").Ptr))
                {
                    emmVRCLoader.Logger.Log("[emmVRC] Found the method!");
                    methIsBadMkay = method;
                }
            }
            if (methIsBadMkay != null)
                avatarLoadingPatch = instance.Patch(methIsBadMkay, typeof(AvatarLoading).GetMethod("ApplyAvatarFeaturePermissions"));*/
            
        }
        public void ApplyAvatarFeaturePermissions(System.IntPtr @this)
        {
            
            avatarLoadingPatch.InvokeOriginal();
        }
    }
}
