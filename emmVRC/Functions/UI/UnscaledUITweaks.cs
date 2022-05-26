using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using emmVRC.Objects.ModuleBases;
namespace emmVRC.Functions.UI
{
    public class UnscaledUI : MelonLoaderEvents
    {
        private static GameObject micTooltipDesktop;
        private static GameObject micTooltipXbox;
        public override void OnUiManagerInit()
        {
            Configuration.onConfigUpdated.Add(new System.Collections.Generic.KeyValuePair<string, Action>("DisableMicTooltip", Process));
            Process();
        }
        public static void Process()
        {
            if (micTooltipDesktop == null)
                micTooltipDesktop = GameObject.Find("UserInterface/UnscaledUI/HudContent_Old/Hud/VoiceDotParent/PushToTalkKeybd");
            if (micTooltipXbox == null)
                micTooltipXbox = GameObject.Find("UserInterface/UnscaledUI/HudContent_Old/Hud/VoiceDotParent/PushToTalkXbox");
            micTooltipDesktop.transform.localScale = (Configuration.JSONConfig.DisableMicTooltip ? Vector3.zero : Vector3.one);
            micTooltipXbox.transform.localScale = (Configuration.JSONConfig.DisableMicTooltip ? Vector3.zero : Vector3.one);
        }
    }
}
