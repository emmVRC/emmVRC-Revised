using emmVRC.Objects.ModuleBases;
using emmVRC.Utils;
using System;
using UnityEngine;
using VRC.Core;

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
        }

        private static void OnInstanceChange(ApiWorld world, ApiWorldInstance instance)
        {
            AreRiskyFunctionsAllowed = false;
            UnityWebRequestUtils.Get($"https://dl.emmvrc.com/riskyfuncs.php?worldid={world.id}", new Action<string>((result) => OnFinish(result, world)));
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

            if (GameObject.Find("eVRCRiskFuncEnable") != null || GameObject.Find("UniversalRiskyFuncEnable") != null)
            {
                AreRiskyFunctionsAllowed = true;
                RiskyFuncsProcessed?.DelegateSafeInvoke(AreRiskyFunctionsAllowed);
                return;
            }
            else if (GameObject.Find("eVRCRiskFuncDisable") != null || GameObject.Find("UniversalRiskyFuncDisable") != null)
            {
                AreRiskyFunctionsAllowed = false;
                RiskyFuncsProcessed?.DelegateSafeInvoke(AreRiskyFunctionsAllowed);
                return;
            }
            if ((GameObject.Find("eVRCRiskFuncEnable") != null || GameObject.Find("eVRCRiskFuncDisable") != null) && RoomManager.field_Internal_Static_ApiWorld_0.authorId == APIUser.CurrentUser.id && !Configuration.JSONConfig.IgnoreWorldCreatorTips)
            {
                emmVRCLoader.Logger.Log("[NOTICE] The eVRCRiskFuncDisable/Enable objects are soon to be deprecated. Instead, please use \"UniversalRiskyFuncDisable\" and \"UniversalRiskyFuncEnable\"");
                NotificationManager.AddNotification("The eVRCRiskFuncDisable/Enable objects are soon to be deprecated. Instead, please use \"UniversalRiskyFuncDisable\" and \"UniversalRiskyFuncEnable\"", "Dismiss", NotificationManager.DismissCurrentNotification, "Never show\nagain", () =>
                {
                    Configuration.WriteConfigOption("IgnoreWorldCreatorTips", true);
                }, Functions.Core.Resources.alertSprite);
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