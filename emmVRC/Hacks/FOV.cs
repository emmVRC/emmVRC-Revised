using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace emmVRC.Hacks
{
    public class FOV
    {
        public static void Initialize()
        {
            // 60 is the default FOV for VRChat, so if the config has a different value, we should apply it
            if (Configuration.JSONConfig.CustomFOV != 60)
            {
                // Fetch the camera object. This only works on Desktop, but setting FOV for VR could be catastrophic
                GameObject cameraobj = GameObject.Find("Camera (eye)");

                // Check if the camera object exists. If not, we're not going to set anything
                if (cameraobj != null)
                {
                    Camera cameracomp = cameraobj.GetComponent<Camera>();
                    if (cameracomp != null)
                        // Apply the custom FOV
                        cameracomp.fieldOfView = Configuration.JSONConfig.CustomFOV;
                }
            }
        }
    }
}
