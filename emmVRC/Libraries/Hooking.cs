using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using emmVRC.Hacks;
using Harmony;
using UnityEngine;
using VRC;
using VRC.Core;
using emmVRC.Objects.ModuleBases;
using emmVRC.Libraries;

namespace emmVRC.Functions.Core
{
    public class Hooking : MelonLoaderEvents
    {

        private static Regex methodMatchRegex = new Regex("Method_Public_Void_\\d", RegexOptions.Compiled);

        public override void OnUiManagerInit()
        {
            try
            {
                if (!Functions.Core.ModCompatibility.PortalConfirmation)
                {
                    emmVRCLoader.emmVRCLoaderMod.instance.HarmonyInstance.Patch(typeof(PortalInternal).GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance).Single(it => it != null && it.ReturnType == typeof(void) && it.GetParameters().Length == 0 && UnhollowerRuntimeLib.XrefScans.XrefScanner.XrefScan(it).Any(jt => jt.Type == UnhollowerRuntimeLib.XrefScans.XrefType.Global && jt.ReadAsObject()?.ToString() == " was at capacity, cannot enter.")), new HarmonyLib.HarmonyMethod(typeof(Hooking).GetMethod("OnPortalEntered", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)));
                }
            }
            catch (Exception ex)
            {
                emmVRCLoader.Logger.LogError("Portal blocking failed: " + ex.ToString());
            }
            try
            {
                emmVRCLoader.emmVRCLoaderMod.instance.HarmonyInstance.Patch(typeof(VRC_StationInternal).GetMethod("Method_Public_Boolean_Player_Boolean_0"), new HarmonyLib.HarmonyMethod(typeof(Hooking).GetMethod("PlayerCanUseStation", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)));
                //instanceHarmony.Patch(typeof(VRC_StationInternal2).GetMethod("Method_Public_Boolean_Player_Boolean_0"), new Harmony.HarmonyMethod(typeof(Hooking).GetMethod("PlayerCanUseStation", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)));
                //instanceHarmony.Patch(typeof(VRC_StationInternal3).GetMethod("Method_Public_Boolean_Player_Boolean_0"), new Harmony.HarmonyMethod(typeof(Hooking).GetMethod("PlayerCanUseStation", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)));
            }
            catch (Exception ex)
            {
                emmVRCLoader.Logger.LogError("Station patching failed: " + ex.ToString());
            }
            if (!Functions.Core.ModCompatibility.FBTSaver && !Functions.Core.ModCompatibility.IKTweaks && !Environment.CurrentDirectory.Contains("vrchat-vrchat")) // Yet another of yet another crusty Oculus check
            {
                try
                {
                    foreach (System.Reflection.MethodInfo inf in System.Reflection.Assembly.GetAssembly(typeof(QuickMenuContextualDisplay)).GetType("VRCTrackingSteam", true, true).GetMethods())
                    {
                        if (inf.GetParameters().Length == 1 && inf.GetParameters().First().ParameterType == typeof(string) && inf.ReturnType == typeof(bool) && inf.GetRuntimeBaseDefinition() == inf)
                        {
                            emmVRCLoader.emmVRCLoaderMod.instance.HarmonyInstance.Patch(inf, new HarmonyLib.HarmonyMethod(typeof(Hooking).GetMethod("IsCalibratedForAvatar", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)));
                        }
                    }
                    foreach (System.Reflection.MethodInfo inf in System.Reflection.Assembly.GetAssembly(typeof(QuickMenuContextualDisplay)).GetType("VRCTrackingSteam", true, true).GetMethods())
                    {
                        if (inf.GetParameters().Length == 3 && inf.GetParameters().First().ParameterType == typeof(Animator) && inf.ReturnType == typeof(void) && inf.GetRuntimeBaseDefinition() == inf)
                        {
                            emmVRCLoader.emmVRCLoaderMod.instance.HarmonyInstance.Patch(inf, new HarmonyLib.HarmonyMethod(typeof(Hooking).GetMethod("PerformCalibration", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)));
                        }
                    }
                }
                catch (Exception ex)
                {
                    emmVRCLoader.Logger.LogError("VRCTrackingSteam hooking failed: " + ex.ToString());
                }
            }
            try
            {
                foreach (MethodInfo method in typeof(PlayerNameplate).GetMethods(BindingFlags.Public | BindingFlags.Instance).Where(x => methodMatchRegex.IsMatch(x.Name)))
                {
                    emmVRCLoader.Logger.LogDebug($"Found target Rebuild method ({method.Name})");
                    emmVRCLoader.emmVRCLoaderMod.instance.HarmonyInstance.Patch(method, null, new HarmonyLib.HarmonyMethod(typeof(Hooking).GetMethod("OnRebuild", BindingFlags.NonPublic | BindingFlags.Static)));
                }
            }
            catch (Exception ex) { emmVRCLoader.Logger.LogError("Avatar OnRebuild Failed: " + ex.ToString()); }
        }
        
        private static bool PlayerCanUseStation(ref bool __result, VRC.Player __0, bool __1)
        {
            if (__0 != null && __0 == VRCPlayer.field_Internal_Static_VRCPlayer_0._player && Configuration.JSONConfig.ChairBlockingEnable)
            {
                __result = false;
                return false;
            }
            return true;
        }

        /*private static void OnAvatarInstantiated(GameObject __0, VRC.SDKBase.VRC_AvatarDescriptor __1, bool __2)
        {
            if (__2)
            {
               
            }
        }*/

        private static bool OnPortalEntered(PortalInternal __instance)
        {
            if (!Configuration.JSONConfig.PortalBlockingEnable)
                return true;
            return false;
        }

        private static void OnRebuild(PlayerNameplate __instance)
        {
            if (__instance.field_Private_VRCPlayer_0 == null) return;
            if (__instance.field_Private_VRCPlayer_0._player != null && __instance.field_Private_VRCPlayer_0._player.prop_APIUser_0 != null)
            {
                if (Configuration.JSONConfig.NameplateColorChangingEnabled && !Functions.Core.ModCompatibility.OGTrustRank)
                {
                    APIUser user = __instance.field_Private_VRCPlayer_0._player.prop_APIUser_0;
                    if (user.isFriend)
                        __instance.field_Private_VRCPlayer_0.GetNameplateBackground().color = (ColorConversion.HexToColor(Configuration.JSONConfig.FriendNamePlateColorHex));
                    else if (user.hasVeteranTrustLevel)
                        __instance.field_Private_VRCPlayer_0.GetNameplateBackground().color = (ColorConversion.HexToColor(Configuration.JSONConfig.TrustedUserNamePlateColorHex));
                    else if (user.hasTrustedTrustLevel)
                        __instance.field_Private_VRCPlayer_0.GetNameplateBackground().color = (ColorConversion.HexToColor(Configuration.JSONConfig.KnownUserNamePlateColorHex));
                    else if (user.hasKnownTrustLevel)
                        __instance.field_Private_VRCPlayer_0.GetNameplateBackground().color = (ColorConversion.HexToColor(Configuration.JSONConfig.UserNamePlateColorHex));
                    else if (user.hasBasicTrustLevel)
                        __instance.field_Private_VRCPlayer_0.GetNameplateBackground().color = (ColorConversion.HexToColor(Configuration.JSONConfig.NewUserNamePlateColorHex));
                }
            }
        }
        private static bool IsCalibratedForAvatar(ref VRCTrackingSteam __instance, ref bool __result, string __0)
        {
            if (__0 != null && Functions.PlayerHacks.FBTSaving.IsPreviouslyCalibrated(__0) && RoomManager.field_Internal_Static_ApiWorld_0 != null && Configuration.JSONConfig.TrackingSaving)
            {
                emmVRCLoader.Logger.LogDebug("Avatar was previously calibrated, loading calibration data");
                __result = true;
                Functions.PlayerHacks.FBTSaving.LoadCalibrationInfo(__instance, __0);
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
            if (__0 == null || __instance == null) return true;
            if (Configuration.JSONConfig.TrackingSaving)
            {
                emmVRCLoader.Logger.LogDebug("Saving calibration info...");
                if (VRCPlayer.field_Internal_Static_VRCPlayer_0.prop_VRCAvatarManager_0.field_Private_ApiAvatar_0 != null)
                    MelonLoader.MelonCoroutines.Start(Functions.PlayerHacks.FBTSaving.SaveCalibrationInfo(__instance, VRCPlayer.field_Internal_Static_VRCPlayer_0.prop_VRCAvatarManager_0.field_Private_ApiAvatar_0.id));
                else if (VRCPlayer.field_Internal_Static_VRCPlayer_0.field_Private_VRCAvatarManager_0.field_Private_ApiAvatar_1 != null)
                    MelonLoader.MelonCoroutines.Start(Functions.PlayerHacks.FBTSaving.SaveCalibrationInfo(__instance, VRCPlayer.field_Internal_Static_VRCPlayer_0.prop_VRCAvatarManager_0.field_Private_ApiAvatar_1.id));
                else
                    emmVRCLoader.Logger.LogError("Could not fetch avatar information for this avatar");
            }
            return true;
        }

        private static bool SetControllerVisibility(VRCTrackingManager __instance, bool __0)
        {
            //if (UnityEngine.Resources.FindObjectsOfTypeAll<VRCTrackingSteam>().Count != 0 && UnityEngine.Resources.FindObjectsOfTypeAll<VRCTrackingSteam>()[0].field_Private_String_0 == null && !__0)
            //    return false;
            return true;
        }

       
    }
}
