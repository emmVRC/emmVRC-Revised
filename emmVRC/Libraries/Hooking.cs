using System;
using System.Linq;
using System.Runtime.InteropServices;
using MelonLoader;
using UnityEngine;


namespace emmVRC.Libraries
{
    public class Hooking
    {
        private delegate void AvatarInstantiatedDelegate(IntPtr @this, IntPtr avatarPtr, IntPtr avatarDescriptorPtr, bool loaded);
        private static AvatarInstantiatedDelegate onAvatarInstantiatedDelegate;
        private static Harmony.HarmonyInstance instanceHarmony;

        public unsafe static void Initialize()
        {
            instanceHarmony = Harmony.HarmonyInstance.Create("emmVRCHarmony");
            try
            {
                if (!Libraries.ModCompatibility.MultiplayerDynamicBones)
                {
                    IntPtr funcToHookAvtr = (IntPtr)typeof(VRCAvatarManager.MulticastDelegateNPublicSealedVoGaVRBoUnique).GetField("NativeMethodInfoPtr_Invoke_Public_Virtual_New_Void_GameObject_VRC_AvatarDescriptor_Boolean_0", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static).GetValue(null);
                    Imports.Hook(funcToHookAvtr, new System.Action<IntPtr, IntPtr, IntPtr, bool>(OnAvatarInstantiated).Method.MethodHandle.GetFunctionPointer());
                    onAvatarInstantiatedDelegate = Marshal.GetDelegateForFunctionPointer<AvatarInstantiatedDelegate>(*(IntPtr*)funcToHookAvtr);
                }
            } catch (Exception ex)
            {
                emmVRCLoader.Logger.LogError("Avatar patching failed: " + ex.ToString());
            }
            try
            {
                if (!Libraries.ModCompatibility.PortalConfirmation)
                {
                    //new PortalInternal().Method_Public_Void_4();
                    instanceHarmony.Patch(typeof(PortalInternal).GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance).Single(it => it != null && it.ReturnType == typeof(void) && it.GetParameters().Length == 0 && UnhollowerRuntimeLib.XrefScans.XrefScanner.XrefScan(it).Any(jt => jt.Type == UnhollowerRuntimeLib.XrefScans.XrefType.Global && jt.ReadAsObject()?.ToString() == " was at capacity, cannot enter.")), new Harmony.HarmonyMethod(typeof(Hooking).GetMethod("OnPortalEntered", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)));
                }
            } catch (Exception ex)
            {
                emmVRCLoader.Logger.LogError("Portal blocking failed: " + ex.ToString());
            }
            try
            {
                instanceHarmony.Patch(typeof(VRC_StationInternal).GetMethod("Method_Public_Boolean_Player_Boolean_0"), new Harmony.HarmonyMethod(typeof(Hooking).GetMethod("PlayerCanUseStation", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)));
                instanceHarmony.Patch(typeof(VRC_StationInternal2).GetMethod("Method_Public_Boolean_Player_Boolean_0"), new Harmony.HarmonyMethod(typeof(Hooking).GetMethod("PlayerCanUseStation", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)));
                instanceHarmony.Patch(typeof(VRC_StationInternal3).GetMethod("Method_Public_Boolean_Player_Boolean_0"), new Harmony.HarmonyMethod(typeof(Hooking).GetMethod("PlayerCanUseStation", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)));
            } catch (Exception ex)
            {
                emmVRCLoader.Logger.LogError("Station patching failed: " + ex.ToString());
            }

        }
        
        private static bool PlayerCanUseStation(ref bool __result, VRC.Player __0, bool __1)
        {
            if (__0 != null && __0 == VRCPlayer.field_Internal_Static_VRCPlayer_0.field_Private_Player_0 && Configuration.JSONConfig.ChairBlockingEnable)
            {
                __result = false;
                return false;
            }
            return true;
        }

        private static void OnAvatarInstantiated(IntPtr @this, IntPtr avatarPtr, IntPtr avatarDescriptorPtr, bool loaded)
        {
            onAvatarInstantiatedDelegate(@this, avatarPtr, avatarDescriptorPtr, loaded);

            try
            {
                if (loaded)
                {
                    Managers.AvatarPermissionManager.ProcessAvatar(new GameObject(avatarPtr), new VRC.SDKBase.VRC_AvatarDescriptor(avatarDescriptorPtr));
                    Hacks.GlobalDynamicBones.ProcessDynamicBones(new GameObject(avatarPtr), new VRC.SDKBase.VRC_AvatarDescriptor(avatarDescriptorPtr), new VRCAvatarManager(@this));
                }
            }
            catch (System.Exception ex)
            {
                emmVRCLoader.Logger.LogError(ex.ToString());
            }
        }
        private static bool OnPortalEntered(PortalInternal __instance)
        {
            if (!Configuration.JSONConfig.PortalBlockingEnable)
                return true;
            return false;
        }
    }
}
