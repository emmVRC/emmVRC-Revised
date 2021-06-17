using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using emmVRC.Hacks;
using Harmony;
using UnityEngine;
using VRC;
using VRC.Core;

namespace emmVRC.Libraries
{
    public class Hooking
    {
        private static Harmony.HarmonyInstance instanceHarmony;
        private static Action<Player> event1Action;
        private static Action<Player> event2Action;

        private static Regex methodMatchRegex = new Regex("Method_Public_Void_\\d", RegexOptions.Compiled);

        public unsafe static void Initialize()
        {
            instanceHarmony = Harmony.HarmonyInstance.Create("emmVRCHarmony");
            try
            {
                foreach (MethodInfo inf in typeof(VRCPlayer).GetMethods())
                {
                    if (inf.Name.Contains("Method_Private_Void_GameObject_VRC_AvatarDescriptor_Boolean_PDM_") && !UnhollowerRuntimeLib.XrefScans.XrefScanner.XrefScan(inf).Any(jt => jt.Type == UnhollowerRuntimeLib.XrefScans.XrefType.Global && jt.ReadAsObject()?.ToString() == "Avatar is Ready, Initializing"))
                    {
                        instanceHarmony.Patch(inf, new HarmonyMethod(typeof(Hooking).GetMethod("OnAvatarInstantiated", BindingFlags.NonPublic | BindingFlags.Static)));
                    }
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
                //instanceHarmony.Patch(typeof(VRC_StationInternal2).GetMethod("Method_Public_Boolean_Player_Boolean_0"), new Harmony.HarmonyMethod(typeof(Hooking).GetMethod("PlayerCanUseStation", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)));
                //instanceHarmony.Patch(typeof(VRC_StationInternal3).GetMethod("Method_Public_Boolean_Player_Boolean_0"), new Harmony.HarmonyMethod(typeof(Hooking).GetMethod("PlayerCanUseStation", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)));
            }
            catch (Exception ex)
            {
                emmVRCLoader.Logger.LogError("Station patching failed: " + ex.ToString());
            }
            try
            {
                NetworkManager.field_Internal_Static_NetworkManager_0.field_Internal_VRCEventDelegate_1_Player_0.Method_Public_Void_UnityAction_1_T_0(new System.Action<Player>((Player plr) =>
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
                NetworkManager.field_Internal_Static_NetworkManager_0.field_Internal_VRCEventDelegate_1_Player_1.Method_Public_Void_UnityAction_1_T_1(new System.Action<Player>((Player plr) =>
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
            if (!ModCompatibility.FBTSaver && !ModCompatibility.IKTweaks && !Environment.CurrentDirectory.Contains("vrchat-vrchat")) // Yet another of yet another crusty Oculus check
            {
                try
                {
                    foreach (System.Reflection.MethodInfo inf in System.Reflection.Assembly.GetAssembly(typeof(QuickMenuContextualDisplay)).GetType("VRCTrackingSteam", true, true).GetMethods())
                    {
                        if (inf.GetParameters().Length == 1 && inf.GetParameters().First().ParameterType == typeof(string) && inf.ReturnType == typeof(bool) && inf.GetRuntimeBaseDefinition() == inf)
                        {
                            instanceHarmony.Patch(inf, new Harmony.HarmonyMethod(typeof(Hooking).GetMethod("IsCalibratedForAvatar", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)));
                        }
                    }
                    foreach (System.Reflection.MethodInfo inf in System.Reflection.Assembly.GetAssembly(typeof(QuickMenuContextualDisplay)).GetType("VRCTrackingSteam", true, true).GetMethods())
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
            }
            try
            {
                foreach (MethodInfo method in typeof(PlayerNameplate).GetMethods(BindingFlags.Public | BindingFlags.Instance).Where(x => methodMatchRegex.IsMatch(x.Name)))
                {
                    emmVRCLoader.Logger.LogDebug($"Found target Rebuild method ({method.Name})", true);
                    instanceHarmony.Patch(method, null, new HarmonyMethod(typeof(Hooking).GetMethod("OnRebuild", BindingFlags.NonPublic | BindingFlags.Static)));
                }
            }
            catch (Exception ex) { emmVRCLoader.Logger.LogError("Avatar OnRebuild Failed: " + ex.ToString()); }
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
            if (__0 == null || __instance == null) return true;
            if (Configuration.JSONConfig.TrackingSaving)
            {
                emmVRCLoader.Logger.LogDebug("Saving calibration info...");
                if (VRCPlayer.field_Internal_Static_VRCPlayer_0.prop_VRCAvatarManager_0.field_Private_ApiAvatar_0 != null)
                    MelonLoader.MelonCoroutines.Start(FBTSaving.SaveCalibrationInfo(__instance, VRCPlayer.field_Internal_Static_VRCPlayer_0.prop_VRCAvatarManager_0.field_Private_ApiAvatar_0.id));
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

        private static bool PlayerCanUseStation(ref bool __result, VRC.Player __0, bool __1)
        {
            if (__0 != null && __0 == VRCPlayer.field_Internal_Static_VRCPlayer_0._player && Configuration.JSONConfig.ChairBlockingEnable)
            {
                __result = false;
                return false;
            }
            return true;
        }
        private static void OnAvatarInstantiated(GameObject __0, VRC.SDKBase.VRC_AvatarDescriptor __1, bool __2)
        {
            if (__2)
            {
                emmVRCLoader.Logger.LogDebug("Avatar loaded");
                Managers.AvatarPermissionManager.ProcessAvatar(__0, __1);
                if (!Libraries.ModCompatibility.MultiplayerDynamicBones)
                {
                    Hacks.GlobalDynamicBones.ProcessDynamicBones(__0, __1);
                }
                //MelonLoader.MelonCoroutines.Start(AvatarPropertySaving.OnLoadAvatar(__1));
            }
        }

        private static bool OnPortalEntered(PortalInternal __instance)
        {
            if (!Configuration.JSONConfig.PortalBlockingEnable)
                return true;
            return false;
        }

        private static void OnRebuild(PlayerNameplate __instance)
        {
            if (Configuration.JSONConfig.StealthMode || __instance.field_Private_VRCPlayer_0 == null) return;
            if (__instance.field_Private_VRCPlayer_0._player != null && __instance.field_Private_VRCPlayer_0._player.prop_APIUser_0 != null)
                if (Configuration.JSONConfig.InfoSpoofingEnabled)
                    VRCPlayer.field_Internal_Static_VRCPlayer_0.GetNameplateText().text = Configuration.JSONConfig.InfoSpoofingName;
                else if (!Configuration.JSONConfig.InfoSpoofingEnabled && VRCPlayer.field_Internal_Static_VRCPlayer_0.GetNameplateText().text.Contains(Hacks.NameSpoofGenerator.spoofedName))
                    VRCPlayer.field_Internal_Static_VRCPlayer_0.GetNameplateText().text = VRCPlayer.field_Internal_Static_VRCPlayer_0.GetNameplateText().text.Replace(Hacks.NameSpoofGenerator.spoofedName, APIUser.CurrentUser.GetName());
        }
    }
}
