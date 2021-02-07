using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using VRC;
using VRC.SDK;
using VRC.SDKBase;

namespace emmVRC.Hacks
{
    public class Speed
    {
        private static float originalWalkSpeed = 0f;
        private static float originalRunSpeed = 0f;
        private static float originalStrafeSpeed = 0f;
        //private static LocomotionInputController locomotionController = null;
        public static float Modifier = 1f;
        public static bool SpeedModified = false;
        public static void Initialize()
        {
            MelonLoader.MelonCoroutines.Start(Loop());
        }
        private static IEnumerator Loop()
        {
            while (true)
            {
                if (RoomManager.field_Internal_Static_ApiWorld_0 == null)
                {
                    originalRunSpeed = 0f;
                    originalWalkSpeed = 0f;
                    originalStrafeSpeed = 0f;
                }
                while (RoomManager.field_Internal_Static_ApiWorld_0 == null)
                    yield return new WaitForSeconds(1f);
                yield return new WaitForEndOfFrame();
                if (VRCPlayer.field_Internal_Static_VRCPlayer_0 != null && VRCPlayer.field_Internal_Static_VRCPlayer_0.prop_VRCPlayerApi_0 != null && RoomManager.field_Internal_Static_ApiWorld_0 != null)
                {
                    
                    if (SpeedModified && (originalRunSpeed == 0f || originalWalkSpeed == 0f || originalStrafeSpeed == 0f))
                    {
                        originalWalkSpeed = VRCPlayer.field_Internal_Static_VRCPlayer_0.prop_VRCPlayerApi_0.GetWalkSpeed();
                        originalRunSpeed = VRCPlayer.field_Internal_Static_VRCPlayer_0.prop_VRCPlayerApi_0.GetRunSpeed();
                        originalStrafeSpeed = VRCPlayer.field_Internal_Static_VRCPlayer_0.prop_VRCPlayerApi_0.GetStrafeSpeed();
                    }
                    if (!SpeedModified && originalRunSpeed != 0f && originalWalkSpeed != 0f && originalStrafeSpeed != 0f)
                    {
                        VRCPlayer.field_Internal_Static_VRCPlayer_0.prop_VRCPlayerApi_0.SetWalkSpeed(originalWalkSpeed);
                        VRCPlayer.field_Internal_Static_VRCPlayer_0.prop_VRCPlayerApi_0.SetRunSpeed(originalRunSpeed);
                        VRCPlayer.field_Internal_Static_VRCPlayer_0.prop_VRCPlayerApi_0.SetStrafeSpeed(originalStrafeSpeed);
                        originalRunSpeed = 0f;
                        originalWalkSpeed = 0f;
                        originalStrafeSpeed = 0f;
                        Modifier = 1f;
                    }
                    if (SpeedModified && originalWalkSpeed != 0f && originalRunSpeed != 0f && originalStrafeSpeed != 0f)
                    {
                        VRCPlayer.field_Internal_Static_VRCPlayer_0.prop_VRCPlayerApi_0.SetWalkSpeed(originalWalkSpeed * Modifier);
                        VRCPlayer.field_Internal_Static_VRCPlayer_0.prop_VRCPlayerApi_0.SetRunSpeed(originalRunSpeed * Modifier);
                        VRCPlayer.field_Internal_Static_VRCPlayer_0.prop_VRCPlayerApi_0.SetStrafeSpeed(originalStrafeSpeed * Modifier);
                    }
                    
                }
                

            }
        }
    }
}
