using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace emmVRC.Hacks
{
    public class FPS
    {
        public static void Initialize()
        {
            // "Unlimited FPS" only works in Desktop mode, as VRChat controls the framerate separately for VR mode.
            if (Configuration.JSONConfig.UnlimitedFPSEnabled)
                UnityEngine.Application.targetFrameRate = Configuration.JSONConfig.FPSLimit;
        }
    }
}
