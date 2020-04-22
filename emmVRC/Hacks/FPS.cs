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
            if (Configuration.JSONConfig.UnlimitedFPSEnabled)
                UnityEngine.Application.targetFrameRate = 200;
        }
    }
}
