/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRCSDK2;
using UnityEngine;
using UnhollowerRuntimeLib;

namespace emmVRC.Libraries
{
    class emmVRC_Trigger : VRC_Interactable
    {
        public Action onInteract;
        public static bool injectedCustomType = false;
        public emmVRC_Trigger(IntPtr ptr) : base(ptr) { }
        public static emmVRC_Trigger Instantiate(GameObject parent, Action onInteract)
        {
            if (!injectedCustomType)
            {
                ClassInjector.RegisterTypeInIl2Cpp<emmVRC_Trigger>();
            }
            emmVRC_Trigger t = parent.AddComponent<emmVRC_Trigger>();
            t.onInteract += onInteract;
            return t;
        }
    }
}
*/