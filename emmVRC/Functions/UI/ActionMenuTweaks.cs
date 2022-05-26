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
            Configuration.onConfigUpdated.Add(new System.Collections.Generic.KeyValuePair<string, Action>("DisableOneHandMovement", Apply));
            Apply();
        }
        public static void Apply()
        {
            UnityEngine.Resources.FindObjectsOfTypeAll<ActionMenuDriver>().FirstOrDefault().transform.Find("Container/MoveMenuL").gameObject.SetActive(!Configuration.JSONConfig.DisableOneHandMovement);
            UnityEngine.Resources.FindObjectsOfTypeAll<ActionMenuDriver>().FirstOrDefault().transform.Find("Container/MoveMenuR").gameObject.SetActive(!Configuration.JSONConfig.DisableOneHandMovement);
        }
    }
}
