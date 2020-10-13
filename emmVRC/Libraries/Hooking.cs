using System;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using emmVRC.Hacks;
using Harmony;
using MelonLoader;
using Transmtn.DTO;
using UnityEngine;
using VRC;

namespace emmVRC.Libraries
{
    public class Hooking
    {
        private delegate void AvatarInstantiatedDelegate(IntPtr @this, IntPtr avatarPtr, IntPtr avatarDescriptorPtr, bool loaded);
        private static AvatarInstantiatedDelegate onAvatarInstantiatedDelegate;
        private static Harmony.HarmonyInstance instanceHarmony;
        private static Action<Player> event1Action;
        private static Action<Player> event2Action;

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
            }
            catch (Exception ex)
            {
                emmVRCLoader.Logger.LogError("Avatar patching failed: " + ex.ToString());
            }
            try
            {
                if (!Libraries.ModCompatibility.PortalConfirmation)
                {
                    instanceHarmony.Patch(typeof(PortalInternal).GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance).Single(it => it != null && it.ReturnType == typeof(void) && it.GetParameters().Length == 0 && UnhollowerRuntimeLib.XrefScans.XrefScanner.XrefScan(it).Any(jt => jt.Type == UnhollowerRuntimeLib.XrefScans.XrefType.Global && jt.ReadAsObject()?.ToString() == " was at capacity, cannot enter.")), new Harmony.HarmonyMethod(typeof(Hooking).GetMethod("OnPortalEntered", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)));
                }
            }
            catch (Exception ex)
            {
                emmVRCLoader.Logger.LogError("Portal blocking failed: " + ex.ToString());
            }
            try
            {
                instanceHarmony.Patch(typeof(VRC_StationInternal).GetMethod("Method_Public_Boolean_Player_Boolean_0"), new Harmony.HarmonyMethod(typeof(Hooking).GetMethod("PlayerCanUseStation", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)));
                instanceHarmony.Patch(typeof(VRC_StationInternal2).GetMethod("Method_Public_Boolean_Player_Boolean_0"), new Harmony.HarmonyMethod(typeof(Hooking).GetMethod("PlayerCanUseStation", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)));
                instanceHarmony.Patch(typeof(VRC_StationInternal3).GetMethod("Method_Public_Boolean_Player_Boolean_0"), new Harmony.HarmonyMethod(typeof(Hooking).GetMethod("PlayerCanUseStation", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)));
            }
            catch (Exception ex)
            {
                emmVRCLoader.Logger.LogError("Station patching failed: " + ex.ToString());
            }
            try
            {
                NetworkManager.field_Internal_Static_NetworkManager_0.field_Internal_ObjectPublicHa1UnT1Unique_1_Player_0.Method_Public_Void_UnityAction_1_T_0(new System.Action<Player>((Player plr) =>
                {
                    if (event1Action == null && event2Action == null)
                    {
                        event1Action = (Player plr2) => { NetworkManagerHooking.OnPlayerJoined(plr2); };
                        event2Action = (Player plr3) => { NetworkManagerHooking.OnPlayerLeft(plr3); };
                        event1Action.Invoke(plr);
                    }
                    else
                    {
                        event1Action.Invoke(plr);
                    }
                }));
                NetworkManager.field_Internal_Static_NetworkManager_0.field_Internal_ObjectPublicHa1UnT1Unique_1_Player_0.Method_Public_Void_UnityAction_1_T_1(new System.Action<Player>((Player plr) =>
                {
                    if (event1Action == null && event2Action == null)
                    {
                        event2Action = (Player plr2) => { NetworkManagerHooking.OnPlayerJoined(plr2); };
                        event1Action = (Player plr3) => { NetworkManagerHooking.OnPlayerLeft(plr3); };
                        event2Action.Invoke(plr);
                    }
                    else
                    {
                        event2Action.Invoke(plr);
                    }
                }));
            }
            catch (Exception ex)
            {
                emmVRCLoader.Logger.LogError("Network Manager hooking failed: " + ex.ToString());
            }
            if (!ModCompatibility.FBTSaver)
            {
                try
                {
                    foreach (System.Reflection.MethodInfo inf in typeof(VRCTrackingSteam).GetMethods())
                    {
                        if (inf.GetParameters().Length == 1 && inf.GetParameters().First().ParameterType == typeof(string) && inf.ReturnType == typeof(bool) && inf.GetRuntimeBaseDefinition() == inf)
                        {
                            instanceHarmony.Patch(inf, new Harmony.HarmonyMethod(typeof(Hooking).GetMethod("IsCalibratedForAvatar", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)));
                        }
                    }
                    foreach (System.Reflection.MethodInfo inf in typeof(VRCTrackingSteam).GetMethods())
                    {
                        if (inf.GetParameters().Length == 3 && inf.GetParameters().First().ParameterType == typeof(Animator) && inf.ReturnType == typeof(void) && inf.GetRuntimeBaseDefinition() == inf)
                        {
                            instanceHarmony.Patch(inf, new Harmony.HarmonyMethod(typeof(Hooking).GetMethod("PerformCalibration", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)));
                        }
                    }
                }
                catch (Exception ex)
                {
                    emmVRCLoader.Logger.LogError("VRCTrackingSteam hooking failed: " + ex.ToString());
                }
                try
                {

                    instanceHarmony.Patch(typeof(VRCTrackingManager).GetMethods()
                        .Single(it => it != null && it.ReturnType == typeof(void) && it.GetParameters().Length == 1 && it.GetParameters().First().ParameterType == typeof(bool) && UnhollowerRuntimeLib.XrefScans.XrefScanner.XrefScan(it).Any(jt => jt.Type == UnhollowerRuntimeLib.XrefScans.XrefType.Method && jt.TryResolve() != null && jt.TryResolve().ReflectedType != null && jt.TryResolve().ReflectedType.Name == "Whiteboard")), new HarmonyMethod(typeof(Hooking).GetMethod("SetControllerVisibility", BindingFlags.NonPublic | BindingFlags.Static)));
                }
                catch (Exception ex)
                {
                    emmVRCLoader.Logger.LogError("SetControllerVisibility hooking failed: " + ex.ToString());
                }
            }
        }

        private static bool IsCalibratedForAvatar(ref VRCTrackingSteam __instance, ref bool __result, string __0)
        {
            if (__0 != null && FBTSaving.IsPreviouslyCalibrated(__0) && RoomManager.field_Internal_Static_ApiWorld_0 != null && Configuration.JSONConfig.TrackingSaving)
            {
                emmVRCLoader.Logger.LogDebug("Avatar was previously calibrated, loading calibration data");
                __result = true;
                FBTSaving.LoadCalibrationInfo(__instance, __0);
                return false;
            }
            else
            {
                emmVRCLoader.Logger.LogDebug("Avatar was not previously calibrated, or tracking saving is off");
                __result = false;
                return true;
            }
        }
        private static bool PerformCalibration(ref VRCTrackingSteam __instance, Animator __0, bool __1, bool __2)
        {
            if (Configuration.JSONConfig.TrackingSaving)
            {
                emmVRCLoader.Logger.LogDebug("Saving calibration info...");
                MelonLoader.MelonCoroutines.Start(FBTSaving.SaveCalibrationInfo(__instance, VRCPlayer.field_Internal_Static_VRCPlayer_0.prop_VRCAvatarManager_0.field_Private_ApiAvatar_0.id));
            }
            return true;
        }
        
        private static bool SetControllerVisibility(VRCTrackingManager __instance, bool __0)
        {
            //if (UnityEngine.Resources.FindObjectsOfTypeAll<VRCTrackingSteam>().Count != 0 && UnityEngine.Resources.FindObjectsOfTypeAll<VRCTrackingSteam>()[0].field_Private_String_0 == null && !__0)
            //    return false;
            return true;
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
                    VRC.SDKBase.VRC_AvatarDescriptor descriptor = new VRC.SDKBase.VRC_AvatarDescriptor(avatarDescriptorPtr);
                    GameObject avatarObj = new GameObject(avatarPtr);
                    VRCAvatarManager avatarMgr = new VRCAvatarManager(@this);
                    Managers.AvatarPermissionManager.ProcessAvatar(avatarObj, descriptor);
                    Hacks.GlobalDynamicBones.ProcessDynamicBones(avatarObj, descriptor, avatarMgr);
                    MelonLoader.MelonCoroutines.Start(AvatarPropertySaving.OnLoadAvatar(descriptor));
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
