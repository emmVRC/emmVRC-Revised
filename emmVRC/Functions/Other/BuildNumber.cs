using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using emmVRC.Objects.ModuleBases;
using UnityEngine;
using VRC.Core;

namespace emmVRC.Functions.Other
{
    [Priority(1)]
    public class BuildNumber : MelonLoaderEvents
    {
        public static int buildNumber;
        public override void OnUiManagerInit()
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
                emmVRCLoader.Logger.LogError("You are using an older version of VRChat than supported by emmVRC: " + VRCBuildNumber + ". This *will* cause issues. Please update VRChat through Steam or Oculus to build "+Objects.Attributes.LastTestedBuildNumber+".");
                System.Windows.Forms.MessageBox.Show("You are using an older version of VRChat than supported by emmVRC: " + VRCBuildNumber + ". This *will* cause issues. Please update VRChat through Steam or Oculus to build " + Objects.Attributes.LastTestedBuildNumber + ".", "emmVRC", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return;
            }
        }
    }
}
