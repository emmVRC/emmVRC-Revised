using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VRC.Udon;
namespace emmVRC.Objects
{
    public class VRC_UdonTrigger : MonoBehaviour
    {
        public static bool classInjected = false;
        private static VRC.Udon.ProgramSources.SerializedUdonProgramAsset programAsset;
        private static string UdonProgramBundleString = "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";
        private UdonBehaviour beh;
        private VRC.Udon.Common.Interfaces.IUdonHeap heap;
        public string InteractText = "Use";
        public Action OnInteract;
        public VRC_UdonTrigger(IntPtr ptr) : base(ptr) { }
        public static VRC_UdonTrigger Instantiate(GameObject parent, string interactText, Action onInteract)
        {
            if (!classInjected)
            {
                UnhollowerRuntimeLib.ClassInjector.RegisterTypeInIl2Cpp<VRC_UdonTrigger>();
                classInjected = true;
            }
            if (programAsset == null)
            {
                AssetBundle udonBundle = AssetBundle.LoadFromMemory(Convert.FromBase64String(UdonProgramBundleString));
                programAsset = udonBundle.LoadAsset("VRC_UdonTrigger", UnhollowerRuntimeLib.Il2CppType.Of<VRC.Udon.ProgramSources.SerializedUdonProgramAsset>()).Cast<VRC.Udon.ProgramSources.SerializedUdonProgramAsset>();
                programAsset.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            }
            if (parent.GetComponent<VRC.SDK3.Components.VRCInteractable>() != null)
                GameObject.Destroy(parent.GetComponent<VRC.SDK3.Components.VRCInteractable>());
            VRC_UdonTrigger trg = parent.AddComponent<VRC_UdonTrigger>();
            if (interactText != null)
                trg.InteractText = interactText;
            if (onInteract != null)
                trg.OnInteract = onInteract;
            return trg;
        }
        public void Start()
        {
            beh = gameObject.AddComponent<UdonBehaviour>();
            beh.serializedProgramAsset = programAsset;
            beh.InitializeUdonContent();
            beh.Start();
            beh.interactText = InteractText;

        }
        public void FixedUpdate()
        {
            if (beh == null || beh._udonVM == null || beh._udonManager == null) return;
            if (heap == null)
                heap = beh._udonVM.InspectHeap();
            if (heap == null) return;
            if (heap.GetHeapVariable(2).Unbox<Boolean>())
            {
                OnInteract.Invoke();
                heap.CopyHeapVariable(3, 2);
            }
        }

    }
}
