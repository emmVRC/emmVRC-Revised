using emmVRC.Objects.ModuleBases;
using emmVRC.Utils;
using System;
using UnityEngine;
using VRC.Core;
using Logger = emmVRCLoader.Logger;

namespace emmVRC.Managers
{
    public class RiskyFunctionsManager : MelonLoaderEvents
    {
        public static event Action<bool> RiskyFuncsProcessed; 
        public static bool AreRiskyFunctionsAllowed
        {
            get => _areRiskyFunctionsAllowed;
            private set
            {
                _areRiskyFunctionsAllowed = value;
                OnRiskyFunctionCheckCompleted?.Invoke(value);
            }
        }
        private static bool _areRiskyFunctionsAllowed;
        public static event Action<bool> OnRiskyFunctionCheckCompleted;

        public override void OnApplicationStart()
        {
            NetworkEvents.OnInstanceChanged += OnInstanceChange;
            NetworkEvents.OnLocalPlayerJoined += (VRC.Player player) => { 
                if (player.field_Private_APIUser_0 != null && player.field_Private_APIUser_0.id == APIUser.CurrentUser.id)
                    UnityWebRequestUtils.Get($"https://prod-dl.emmvrc.com/risky_func/{RoomManager.field_Internal_Static_ApiWorld_0.id}", new Action<string>((result) => OnFinish(result, RoomManager.field_Internal_Static_ApiWorld_0)));
            };
        }

        private static void OnInstanceChange(ApiWorld world, ApiWorldInstance instance)
        {
            AreRiskyFunctionsAllowed = false;
        }

        private static void OnFinish(string result, ApiWorld world)
        {
            if (!string.IsNullOrWhiteSpace(result))
            {
                switch (result)
                {
                    case "allowed":
                        AreRiskyFunctionsAllowed = true;
                        RiskyFuncsProcessed?.DelegateSafeInvoke(AreRiskyFunctionsAllowed);
                        return;

                    case "denied":
                        AreRiskyFunctionsAllowed = false;
                        RiskyFuncsProcessed?.DelegateSafeInvoke(AreRiskyFunctionsAllowed);
                        return;
                }
            }

            var allowObject = GameObject.Find("eVRCRiskFuncEnable") ?? GameObject.Find("UniversalRiskyFuncEnable");

            if (allowObject != null
                && allowObject.scene.name != null
                && allowObject.scene.name.Equals("DontDestroyOnLoad"))
            {
                AreRiskyFunctionsAllowed = false;
                RiskyFuncsProcessed?.DelegateSafeInvoke(AreRiskyFunctionsAllowed);
                return;
            }

            if (allowObject != null)
            {
                AreRiskyFunctionsAllowed = true;
                RiskyFuncsProcessed?.DelegateSafeInvoke(AreRiskyFunctionsAllowed);
                return;
            }

            var denyObject = GameObject.Find("eVRCRiskFuncDisable") ??
                             GameObject.Find("UniversalRiskyFuncDisable");

            if (denyObject != null)
            {
                AreRiskyFunctionsAllowed = false;
                RiskyFuncsProcessed?.DelegateSafeInvoke(AreRiskyFunctionsAllowed);
                return;
            }
            
            if ((GameObject.Find("eVRCRiskFuncEnable") != null || GameObject.Find("eVRCRiskFuncDisable") != null) && RoomManager.field_Internal_Static_ApiWorld_0.authorId == APIUser.CurrentUser.id && !Configuration.JSONConfig.IgnoreWorldCreatorTips)
            {
                emmVRCLoader.Logger.Log("[NOTICE] The eVRCRiskFuncDisable/Enable objects are soon to be deprecated. Instead, please use \"UniversalRiskyFuncDisable\" and \"UniversalRiskyFuncEnable\"");
                //NotificationManager.AddNotification("The eVRCRiskFuncDisable/Enable objects are soon to be deprecated. Instead, please use \"UniversalRiskyFuncDisable\" and \"UniversalRiskyFuncEnable\"", "Dismiss", NotificationManager.DismissCurrentNotification, "Never show\nagain", () =>
                //{
                //    Configuration.WriteConfigOption("IgnoreWorldCreatorTips", true);
                //}, Functions.Core.Resources.alertSprite);
            }

            foreach (string worldTag in world.tags)
            {
                if (worldTag.ToLower().Contains("game") || worldTag.ToLower().Contains("club"))
                {
                    AreRiskyFunctionsAllowed = false;
                    RiskyFuncsProcessed?.DelegateSafeInvoke(AreRiskyFunctionsAllowed);
                    return;
                }
            }

            AreRiskyFunctionsAllowed = true;
            RiskyFuncsProcessed?.DelegateSafeInvoke(AreRiskyFunctionsAllowed);
        }
    }
}
