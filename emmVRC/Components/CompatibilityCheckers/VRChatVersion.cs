﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using emmVRC.Objects.ModuleBases;
using emmVRC.Utils;
using UnityEngine;
using VRC.Core;

namespace emmVRC.Components.CompatibilityCheckers
{
    public class VRChatVersion : CompatibilityCheck
    {
        public static int buildNumber;
        public override bool RunCheck()
        {
            int VRCBuildNumber = 0;
            UnhollowerBaseLib.Il2CppArrayBase<MonoBehaviour> components = GameObject.Find("_Application/ApplicationSetup").GetComponents<MonoBehaviour>();
            foreach (MonoBehaviour component in components)
            {
                if (component.TryCast<Transform>() != null || component.TryCast<ApiCache>() != null)
                    continue;

                emmVRCLoader.Logger.LogDebug(component.GetType().Name);
                bool flag = false;
                foreach (Il2CppSystem.Reflection.FieldInfo field in component.GetIl2CppType().GetFields())
                {
                    if (flag == false)
                        if (field.FieldType == UnhollowerRuntimeLib.Il2CppType.Of<int>())
                        {
                            VRCBuildNumber = field.GetValue(component).Unbox<int>();
                            flag = true;
                        }
                }
            }
            emmVRCLoader.Logger.Log("VRChat build is: " + VRCBuildNumber);
            buildNumber = VRCBuildNumber;
            if (VRCBuildNumber < Objects.Attributes.LastTestedBuildNumber)
            {
                emmVRCLoader.Logger.LogError("You are using an older version of VRChat than supported by emmVRC: " + VRCBuildNumber + ". Please update VRChat through Steam or Oculus to build " + Objects.Attributes.LastTestedBuildNumber + ".");
                MessageBox.Show(IntPtr.Zero, "You are using an older version of VRChat than supported by emmVRC: " + VRCBuildNumber + ". Please update VRChat through Steam or Oculus to build " + Objects.Attributes.LastTestedBuildNumber + ".", "emmVRC", 0x00000010u);
                return false;
            }
            return true;
        }
        
    }
}
