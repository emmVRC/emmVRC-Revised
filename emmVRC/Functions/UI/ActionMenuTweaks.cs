using emmVRC.Libraries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using emmVRC.Objects.ModuleBases;

namespace emmVRC.Functions.UI
{
    public class ActionMenuTweaks : MelonLoaderEvents
    {
        public override void OnUiManagerInit()
        {
            if (!Configuration.JSONConfig.StealthMode && Functions.Other.BuildNumber.buildNumber <= 1134)
            {
                Configuration.onConfigUpdated.Add(new System.Collections.Generic.KeyValuePair<string, Action>("DisableOneHandMovement", Apply));
                Apply();
            }
        }
        public static void Apply()
        {
            QuickMenuUtils.GetVRCUiMInstance().menuContent().transform.parent.Find("ActionMenu/MoveMenuL").gameObject.SetActive(!Configuration.JSONConfig.DisableOneHandMovement);
            QuickMenuUtils.GetVRCUiMInstance().menuContent().transform.parent.Find("ActionMenu/MoveMenuR").gameObject.SetActive(!Configuration.JSONConfig.DisableOneHandMovement);
        }
    }
}
