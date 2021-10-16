using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using emmVRC.Objects.ModuleBases;

namespace emmVRC.Hacks
{
    public class OriginalMirror
    {
        public VRC.SDKBase.VRC_MirrorReflection MirrorParent;
        public LayerMask OriginalLayers;
    }
    public class MirrorTweaks : MelonLoaderEvents
    {
        public static List<OriginalMirror> originalMirrors = new List<OriginalMirror>();
        private static LayerMask optimizeMask = new LayerMask { value = 263680 };
        private static LayerMask beautifyMask = new LayerMask { value = -1025 };
        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            originalMirrors = new List<OriginalMirror>();
            foreach (VRC.SDKBase.VRC_MirrorReflection refl in UnityEngine.Resources.FindObjectsOfTypeAll<VRC.SDKBase.VRC_MirrorReflection>())
            {
                originalMirrors.Add(new OriginalMirror { MirrorParent = refl, OriginalLayers = refl.m_ReflectLayers });
            }
        }
        public static void Optimize()
        {
            if (originalMirrors.Count != 0)
                foreach (OriginalMirror mirr in originalMirrors)
                    mirr.MirrorParent.m_ReflectLayers = optimizeMask;
        }
        public static void Beautify()
        {
            if (originalMirrors.Count != 0)
                foreach (OriginalMirror mirr in originalMirrors)
                    mirr.MirrorParent.m_ReflectLayers = beautifyMask;
        }
        public static void Revert()
        {
            if (originalMirrors.Count != 0)
                foreach (OriginalMirror mirr in originalMirrors)
                    mirr.MirrorParent.m_ReflectLayers = mirr.OriginalLayers;
        }
    }
}
