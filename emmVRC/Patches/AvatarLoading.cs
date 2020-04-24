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
using NET_SDK;
using NET_SDK.Reflection;
using NET_SDK.Harmony;

namespace emmVRC.Patches
{
    public class AvatarLoading
    {
        public static NET_SDK.Harmony.Patch avatarLoadingPatch;
        public static void Apply()
        {
            
            //var HarmonyInstance = NET_SDK.Harmony.Manager.CreateInstance("emmVRC Patching");
            //var AvatarManager = SDK.GetClass("VRCAvatarManager");
            //HarmonyInstance.Patch(AvatarManager.GetMethods().FirstOrDefault(x => (x.GetReturnType().Name == "System.Void") && x.GetParameters().FirstOrDefault().Ptr == NET_SDK.IL2CPP.il2cpp_class_get_type(SDK.GetClass("UnityEngine.GameObject").Ptr)), AccessTools.Method(typeof(Avatarload), "AvatarLoadingPatch"));
            //var VRCAvatarManager = NET_SDK.SDK.GetClass("VRCAvatarManager");
            //IL2CPP_Method[] methods = VRCAvatarManager.GetMethods(x => (x.HasFlag(NET_SDK.Reflection.IL2CPP_BindingFlags.METHOD_PUBLIC) && (x.GetParameterCount() == 1) && x.GetParameters()[0].));
            //for (int i = 0; i < methods.Length; i++)
            //{
            //    HarmonyInstance.Patch(methods[i], AccessTools.Method(typeof(AvatarLoading), "AvatarLoadingPatch"));
            //}
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
        public static bool AvatarLoadingPatch(UnityEngine.GameObject __0, VRCAvatarManager __instance)
        {
            
            emmVRCLoader.Logger.Log("I did it!");
            return true;
        }
        public void ApplyAvatarFeaturePermissions(System.IntPtr @this)
        {
            
            avatarLoadingPatch.InvokeOriginal();
        }
    }
}
