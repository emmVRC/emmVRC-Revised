using Il2CppSystem;
using System.Linq;
using Il2CppSystem.Collections;
using Il2CppSystem.Collections.Generic;
using Il2CppSystem.Reflection;
using UnityEngine;
using UnhollowerRuntimeLib;
using UnhollowerRuntimeLib.XrefScans;
using VRC.UI;
using System;

namespace emmVRC.Libraries
{
    public class QuickMenuUtils
    {
        // Fetch the VRCUiManager instance
        [System.ObsoleteAttribute("VRCUiManager no longer functions the same. Please use VRC.UI.UiManagerImpl")]
        public static VRCUiManager GetVRCUiMInstance()
        {
            return VRCUiManager.prop_VRCUiManager_0;
        }
        
    }
}
