using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using NET_SDK.Harmony;
using NET_SDK.Reflection;
using NET_SDK;

namespace emmVRC.Hacks
{
    public class Hook
    {
        private static NET_SDK.Harmony.Patch OnUpdatePatch;
        public static void Start()
        {
            NET_SDK.Harmony.Instance harmonyInstance = NET_SDK.Harmony.Manager.CreateInstance("VRCApplicationUpdate");
            IL2CPP_Class VRCApplicationClass = SDK.GetClass("VRCApplication");
            OnUpdatePatch = harmonyInstance.Patch(VRCApplicationClass.GetMethod("Update"), typeof(Hook).GetMethod("OnUpdate", BindingFlags.Instance | BindingFlags.Public));
        }
        public void OnUpdate()
        {
            OnUpdatePatch.InvokeOriginal(IL2CPP.ObjectToIntPtr(this));
            emmVRCLoader.Logger.Log("Hook is working!");
        }

    }
}
