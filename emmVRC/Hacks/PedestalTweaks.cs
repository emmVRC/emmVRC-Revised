using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VRC;
using VRCSDK2;
using emmVRC.Objects.ModuleBases;

namespace emmVRC.Hacks
{
    public class OriginalPedestal
    {
        public GameObject PedestalParent;
        public bool originalActiveStatus;
    }
    public class PedestalTweaks : MelonLoaderEvents
    {
        public static List<OriginalPedestal> originalPedestals;
        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            originalPedestals = new List<OriginalPedestal>();
            foreach (VRC.SDKBase.VRC_AvatarPedestal pdst in UnityEngine.Resources.FindObjectsOfTypeAll<VRC.SDKBase.VRC_AvatarPedestal>())
            {
                originalPedestals.Add(new OriginalPedestal { PedestalParent = pdst.gameObject, originalActiveStatus = pdst.gameObject.activeSelf });
            }
        }
        public static void Disable()
        {
            if (originalPedestals.Count != 0)
                foreach (OriginalPedestal pdst in originalPedestals)
                    pdst.PedestalParent.SetActive(false);
        }
        public static void Enable()
        {
            if (originalPedestals.Count != 0)
                foreach (OriginalPedestal pdst in originalPedestals)
                    pdst.PedestalParent.SetActive(true);
        }
        public static void Revert()
        {
            if (originalPedestals.Count != 0)
                foreach (OriginalPedestal pdst in originalPedestals)
                    pdst.PedestalParent.SetActive(pdst.originalActiveStatus);
        }
    }
}
