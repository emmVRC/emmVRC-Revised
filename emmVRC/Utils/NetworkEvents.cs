using emmVRC.Objects.ModuleBases;
using MelonLoader;
using System;
using System.Reflection;
using UnityEngine;
using VRC;
using VRC.Core;
using HarmonyLib;

namespace emmVRC.Utils
{
    public class NetworkEvents : MelonLoaderEvents
    {
        public static event Action<Player> OnPlayerJoined;
        public static event Action<Player> OnPlayerLeft;

        public static event Action<Player> OnLocalPlayerJoined;
        public static event Action<Player> OnLocalPlayerLeft;

        public static event Action<VRCPlayer> OnVRCPlayerAwoke;
        public static event Action<VRCPlayer> OnVRCPlayerDestroyed;

        public static event Action<VRCPlayer, ApiAvatar, GameObject> OnAvatarInstantiated;

        public static event Action<ApiWorld, ApiWorldInstance> OnInstanceChanged;

        private static void OnPlayerJoin(Player player)
        {
            if (player == null)
                return;

            emmVRCLoader.Logger.LogDebug($"OnPlayerJoin: {player.ToString()}");
            try
            {
                if (player.field_Private_APIUser_0 == null || player.field_Private_APIUser_0.IsSelf)
                    OnLocalPlayerJoined?.DelegateSafeInvoke(player);
                else
                    OnPlayerJoined?.DelegateSafeInvoke(player);
            }
            catch (Exception ex)
            {
                emmVRCLoader.Logger.LogError("Error while executing OnPlayerJoin:\n" + ex.ToString());
            }
        }

        private static void OnPlayerLeave(Player player)
        {
            if (player == null)
                return;

            emmVRCLoader.Logger.LogDebug($"OnPlayerLeave: {player.ToString()}");
            try
            {
                if (player.field_Private_APIUser_0 == null || player.field_Private_APIUser_0.IsSelf)
                    OnLocalPlayerLeft?.DelegateSafeInvoke(player);
                else
                    OnPlayerLeft?.DelegateSafeInvoke(player);
            }
            catch (Exception ex)
            {
                emmVRCLoader.Logger.LogError("Error while executing OnPlayerLeave:\n" + ex.ToString());
            }
        }

        private static void OnVRCPlayerAwake(VRCPlayer __instance)
        {
            if (__instance == null)
                return;

            try
            {
                __instance.Method_Public_add_Void_OnAvatarIsReady_0(new Action(()
                    => OnAvatarInstantiate(__instance, __instance.field_Private_ApiAvatar_0, __instance.field_Internal_GameObject_0)));
            }
            catch (Exception ex)
            {
                emmVRCLoader.Logger.LogError("Error while adding to OnAvatarInstantiate:\n" + ex.ToString());
            }

            emmVRCLoader.Logger.LogDebug($"OnPlayerAwake: {__instance.ToString()}");
            try
            {
                OnVRCPlayerAwoke?.DelegateSafeInvoke(__instance);
            }
            catch (Exception ex)
            {
                emmVRCLoader.Logger.LogError("Error while executing OnPlayerAwake:\n" + ex.ToString());
            }
        }

        private static void OnVRCPlayerDestroy(VRCPlayer __instance)
        {
            if (__instance == null)
                return;

            emmVRCLoader.Logger.LogDebug($"OnPlayerDestroy: {__instance.ToString()}");
            try
            {
                OnVRCPlayerDestroyed?.DelegateSafeInvoke(__instance);
            }
            catch (Exception ex)
            {
                emmVRCLoader.Logger.LogError("Error while executing OnPlayerDestroy:\n" + ex.ToString());
            }
        }

        private static void OnAvatarInstantiate(VRCPlayer player, ApiAvatar avatar, GameObject gameObject)
        {
            if (player == null || avatar == null || gameObject == null)
                return;

            emmVRCLoader.Logger.LogDebug($"OnAvatarInstantiate: {player.ToString()}, {avatar.ToString()}, {gameObject.ToString()}");
            try
            {
                OnAvatarInstantiated?.DelegateSafeInvoke(player, avatar, gameObject);
            }
            catch (Exception ex)
            {
                emmVRCLoader.Logger.LogError("Error while executing OnAvatarInstantiate:\n" + ex.ToString());
            }
        }

        private static void OnInstanceChange(ApiWorld __0, ApiWorldInstance __1)
        {
            if (__0 == null || __1 == null)
                return;

            emmVRCLoader.Logger.LogDebug($"OnInstanceChange: {__0.ToString()}, {__1.ToString()}");
            try
            {
                OnInstanceChanged?.DelegateSafeInvoke(__0, __1);
            }
            catch (Exception ex)
            {
                emmVRCLoader.Logger.LogError("Error while executing OnInstanceChange:\n" + ex.ToString());
            }
        }

        public override void OnUiManagerInit()
        {
            var field0 = NetworkManager.field_Internal_Static_NetworkManager_0.field_Internal_VRCEventDelegate_1_Player_0;
            var field1 = NetworkManager.field_Internal_Static_NetworkManager_0.field_Internal_VRCEventDelegate_1_Player_1;

            field0.field_Private_HashSet_1_UnityAction_1_T_0.Add(new Action<Player>(OnPlayerJoin));
            field1.field_Private_HashSet_1_UnityAction_1_T_0.Add(new Action<Player>(OnPlayerLeave));

            emmVRCLoader.emmVRCLoaderMod.instance.HarmonyInstance.Patch(typeof(VRCPlayer).GetMethod("Awake"), null, typeof(NetworkEvents).GetMethod(nameof(OnVRCPlayerAwake), BindingFlags.Static | BindingFlags.NonPublic).ToNewHarmonyMethod());
            emmVRCLoader.emmVRCLoaderMod.instance.HarmonyInstance.Patch(typeof(VRCPlayer).GetMethod("OnDestroy"), typeof(NetworkEvents).GetMethod(nameof(OnVRCPlayerDestroy), BindingFlags.Static | BindingFlags.NonPublic).ToNewHarmonyMethod());
            emmVRCLoader.emmVRCLoaderMod.instance.HarmonyInstance.Patch(typeof(RoomManager).GetMethod("Method_Public_Static_Boolean_ApiWorld_ApiWorldInstance_String_Int32_0"), null, typeof(NetworkEvents).GetMethod(nameof(OnInstanceChange), BindingFlags.NonPublic | BindingFlags.Static).ToNewHarmonyMethod());
        }
    }
}