using emmVRC.Libraries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace emmVRC.Hacks
{
    public class ActionMenuTweaks
    {
        public static void Apply()
        {
            QuickMenuUtils.GetVRCUiMInstance().menuContent().transform.parent.Find("ActionMenu/MoveMenuL").gameObject.SetActive(!Configuration.JSONConfig.DisableOneHandMovement);
            QuickMenuUtils.GetVRCUiMInstance().menuContent().transform.parent.Find("ActionMenu/MoveMenuR").gameObject.SetActive(!Configuration.JSONConfig.DisableOneHandMovement);
        }
    }
}
