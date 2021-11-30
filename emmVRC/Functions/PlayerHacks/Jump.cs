
using emmVRC.Objects.ModuleBases;
using VRC.SDKBase;

namespace emmVRC.Functions.PlayerHacks
{
    public class Jump : MelonLoaderEvents
    {
        public static bool IsEnabled { get; private set; }

        public static float OriginalJumpImpulse { get; private set; }

        public static void SetActive(bool active)
        {
            VRCPlayerApi localPlayerApi = VRCPlayer.field_Internal_Static_VRCPlayer_0?.field_Private_VRCPlayerApi_0;
            if (localPlayerApi == null)
                return;

            if (active)
            {
                if (!IsEnabled)
                {
                    OriginalJumpImpulse = localPlayerApi.GetJumpImpulse();
                    localPlayerApi.SetJumpImpulse(2.8f);
                    IsEnabled = true;
                }
            }
            else
            {
                localPlayerApi.SetJumpImpulse(OriginalJumpImpulse);
                IsEnabled = false;
            }
        }
    }
}