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
            if (Configuration.JSONConfig.CustomFOV != 60)
            {
                GameObject cameraobj = GameObject.Find("Camera (eye)");
                if (cameraobj != null)
                {
                    Camera cameracomp = cameraobj.GetComponent<Camera>();
                    if (cameracomp != null)
                        cameracomp.fieldOfView = Configuration.JSONConfig.CustomFOV;
                }
            }
        }
    }
}
