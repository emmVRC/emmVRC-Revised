using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
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
        private static LocomotionInputController locomotionController = null;
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
                yield return new WaitForEndOfFrame();
                if (locomotionController == null)
                {
                    if (VRCPlayer.field_Internal_Static_VRCPlayer_0 != null)
                        locomotionController = VRCPlayer.field_Internal_Static_VRCPlayer_0.GetComponent<LocomotionInputController>();
                }
                if (locomotionController != null && SpeedModified && (originalRunSpeed == 0f || originalWalkSpeed == 0f || originalStrafeSpeed == 0f))
                {
                    originalWalkSpeed = locomotionController.walkSpeed;
                    originalRunSpeed = locomotionController.runSpeed;
                    originalStrafeSpeed = locomotionController.strafeSpeed;
                }
                if (locomotionController != null && !SpeedModified && originalRunSpeed != 0f && originalWalkSpeed != 0f && originalStrafeSpeed != 0f)
                {
                    locomotionController.walkSpeed = originalWalkSpeed;
                    locomotionController.runSpeed = originalRunSpeed;
                    locomotionController.strafeSpeed = originalStrafeSpeed;
                    originalRunSpeed = 0f;
                    originalWalkSpeed = 0f;
                    originalStrafeSpeed = 0f;
                    Modifier = 1f;
                }
                if (locomotionController != null && SpeedModified && originalWalkSpeed != 0f && originalRunSpeed != 0f && originalStrafeSpeed != 0f)
                {
                    locomotionController.walkSpeed = (originalWalkSpeed * Modifier);
                    locomotionController.runSpeed = (originalRunSpeed * Modifier);
                    locomotionController.strafeSpeed = (originalStrafeSpeed * Modifier);
                }

            }
        }
    }
}
