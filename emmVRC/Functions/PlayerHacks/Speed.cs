using emmVRC.Managers;
using emmVRC.Objects.ModuleBases;
using VRC.SDKBase;

namespace emmVRC.Functions.PlayerHacks
{
    public class Speed : MelonLoaderEvents
    {
        public static bool IsEnabled { get; private set; }

        public static float OriginalRunSpeed { get; private set; }
        public static float OriginalStrafeSpeed { get; private set; }
        public static float OriginalWalkSpeed { get; private set; }

        public override void OnUiManagerInit()
        {
            Configuration.onConfigUpdated.Add(new System.Collections.Generic.KeyValuePair<string, System.Action>("RiskyFunctionsEnabled", () => {
                if (!Configuration.JSONConfig.RiskyFunctionsEnabled && IsEnabled)
                {
                    SetActive(false);
                }
            }));
        }

        public static void SetActive(bool active)
        {
            if (active)
            {
                VRCPlayerApi playerApi = VRCPlayer.field_Internal_Static_VRCPlayer_0?.field_Private_VRCPlayerApi_0;
                if (!RiskyFunctionsManager.AreRiskyFunctionsAllowed || playerApi == null)
                    return;

                if (!IsEnabled)
                {
                    OriginalRunSpeed = playerApi.GetRunSpeed();
                    OriginalStrafeSpeed = playerApi.GetStrafeSpeed();
                    OriginalWalkSpeed = playerApi.GetWalkSpeed();
                }

                playerApi.SetRunSpeed(OriginalRunSpeed * Configuration.JSONConfig.SpeedModifier);
                playerApi.SetStrafeSpeed(OriginalStrafeSpeed * Configuration.JSONConfig.SpeedModifier);
                playerApi.SetWalkSpeed(OriginalWalkSpeed * Configuration.JSONConfig.SpeedModifier);

                IsEnabled = true;
            }
            else
            {
                VRCPlayerApi playerApi = VRCPlayer.field_Internal_Static_VRCPlayer_0?.field_Private_VRCPlayerApi_0;
                if (playerApi == null)
                    return;

                playerApi.SetRunSpeed(OriginalRunSpeed);
                playerApi.SetStrafeSpeed(OriginalStrafeSpeed);
                playerApi.SetWalkSpeed(OriginalWalkSpeed);

                IsEnabled = false;
            }
        }
    }
}