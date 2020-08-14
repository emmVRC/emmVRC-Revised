using System;
using System.Net;
using System.IO;
using System.Reflection;
using Il2CppSystem.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using UnhollowerBaseLib;
using MelonLoader;
using UnityEngine;
using UnhollowerRuntimeLib;
using Transmtn.DTO;
using Transmtn.Exceptions;
using VRC.SDKBase;
using VRCSDK2;
using VRC;
using System.Linq.Expressions;

namespace emmVRC.Libraries
{
    public class Hooking
    {
        private delegate void AvatarInstantiatedDelegate(IntPtr @this, IntPtr avatarPtr, IntPtr avatarDescriptorPtr, bool loaded);
        private static AvatarInstantiatedDelegate onAvatarInstantiatedDelegate;
        private delegate void PortalEnteredDelegate(IntPtr instance);
        private static PortalEnteredDelegate onPortalEnteredDelegate;
        private static Harmony.HarmonyInstance instanceHarmony;


        /*public static void Hook(IntPtr target, IntPtr detour)
        {
            typeof(Imports).GetMethod("Hook", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic).Invoke(null, new object[] { target, detour });
        }*/


        public unsafe static void Initialize()
        {
            instanceHarmony = Harmony.HarmonyInstance.Create("emmVRCHarmony");
            try
            {
                instanceHarmony.Patch(typeof(Il2CppSystem.Console).GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static).First(
                    (System.Reflection.MethodInfo a) =>
                    {
                        if (a.Name != "WriteLine")
                        {
                            return false;
                        }
                        if (a.GetParameters().Length == 1)
                        {
                            return a.GetParameters()[0].ParameterType == typeof(string);
                        }
                        return false;
                    }), new Harmony.HarmonyMethod(typeof(Hooking).GetMethod("IL2CPPConsoleWriteLine", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)), null, null);
            } catch (Exception ex)
            {
                emmVRCLoader.Logger.LogError("Console cleanup failed: " + ex.ToString());
            }
            try
            {
                IntPtr funcToHookAvtr = (IntPtr)typeof(VRCAvatarManager.MulticastDelegateNPublicSealedVoGaVRBoUnique).GetField("NativeMethodInfoPtr_Invoke_Public_Virtual_New_Void_GameObject_VRC_AvatarDescriptor_Boolean_0", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static).GetValue(null);
                Imports.Hook(funcToHookAvtr, new System.Action<IntPtr, IntPtr, IntPtr, bool>(OnAvatarInstantiated).Method.MethodHandle.GetFunctionPointer());
                onAvatarInstantiatedDelegate = Marshal.GetDelegateForFunctionPointer<AvatarInstantiatedDelegate>(*(IntPtr*)funcToHookAvtr);
            } catch (Exception ex)
            {
                emmVRCLoader.Logger.LogError("Avatar patching failed: " + ex.ToString());
            }
            try
            {
                IntPtr funcToEnterPortal = (IntPtr)typeof(PortalInternal).GetField("NativeMethodInfoPtr_Method_Public_Void_2", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static).GetValue(null);
                var original = *(IntPtr*)funcToEnterPortal;
                Imports.Hook((IntPtr)(&original), Marshal.GetFunctionPointerForDelegate(new Action<IntPtr>(OnPortalEntered)));
                onPortalEnteredDelegate = Marshal.GetDelegateForFunctionPointer<PortalEnteredDelegate>(original);
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
        
        private static bool IL2CPPConsoleWriteLine(string __0)
        {
            if (__0.Contains("authcookie") || Configuration.JSONConfig.ConsoleClean)
                return false;
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
        /*private static bool TransmtnGet(Transmtn.HttpConnection __instance, string __0, ref string __result)
        {
            string result;
            try
            {
                WebClient webClient = new WebClient();
                webClient.Headers.Add(HttpRequestHeader.Cookie, string.Format("apiKey={0}; auth={1}", __instance.Config.apiKey, __instance.Token.Token));
                webClient.Headers.Add(HttpRequestHeader.UserAgent, "Transmtn-Pipeline");
                webClient.Headers.Add("X-Client-Version", __instance._clientVersion);
                webClient.Headers.Add("X-Platform", __instance._clientVersion);
                Stream stream = webClient.OpenRead(__instance.Endpoint + __0);
                StreamReader streamReader = new StreamReader(stream);
                string text = streamReader.ReadToEnd();
                stream.Close();
                streamReader.Close();
                result = text;
            }
            catch (System.Net.WebException ex)
            {
                throw ex;
            }
            __result = result;
            return false;
        }*/

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
        private static void OnPortalEntered(IntPtr @this)
        {
            if (@this != IntPtr.Zero)
            {
                if (!Configuration.JSONConfig.PortalBlockingEnable)
                    onPortalEnteredDelegate(@this);
            }
        }
    }
}
