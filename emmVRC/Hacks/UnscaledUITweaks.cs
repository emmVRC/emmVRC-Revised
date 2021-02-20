using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace emmVRC.Hacks
{
    public class UnscaledUITweaks
    {
        private static GameObject micTooltipDesktop;
        private static GameObject micTooltipXbox;
        public static void Process()
        {
            if (micTooltipDesktop == null)
                micTooltipDesktop = GameObject.Find("UserInterface/UnscaledUI/HudContent/Hud/VoiceDotParent/PushToTalkKeybd");
            if (micTooltipXbox == null)
                micTooltipXbox = GameObject.Find("UserInterface/UnscaledUI/HudContent/Hud/VoiceDotParent/PushToTalkXbox");
            micTooltipDesktop.transform.localScale = (Configuration.JSONConfig.DisableMicTooltip ? Vector3.zero : Vector3.one);
            micTooltipXbox.transform.localScale = (Configuration.JSONConfig.DisableMicTooltip ? Vector3.zero : Vector3.one);
        }
    }
}
