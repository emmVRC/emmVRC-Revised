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
            // It also doesn't need to go up from 144, as not many people have monitors that can display more than 144hz
            if (Configuration.JSONConfig.UnlimitedFPSEnabled)
                UnityEngine.Application.targetFrameRate = 144;
        }
    }
}
