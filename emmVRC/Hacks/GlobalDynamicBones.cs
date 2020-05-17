using emmVRC.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VRC.Core;
using VRC.SDKBase;

namespace emmVRC.Hacks
{
    public class GlobalDynamicBones
    {
        private static List<DynamicBone> currentWorldDynamicBones = new List<DynamicBone>();
        private static List<DynamicBoneCollider> currentWorldDynamicBoneColliders = new List<DynamicBoneCollider>();
        public static void ProcessDynamicBones(GameObject avatarObject, VRC_AvatarDescriptor avatarDescriptor, VRCAvatarManager avatarManager)
        {
            if (Configuration.JSONConfig.GlobalDynamicBonesEnabled)
            {
                AvatarPermissions aperms = AvatarPermissions.GetAvatarPermissions(avatarDescriptor.GetComponent<PipelineManager>().blueprintId);
                UserPermissions uperms = UserPermissions.GetUserPermissions(avatarDescriptor.GetComponentInParent<VRCPlayer>().prop_Player_0.prop_APIUser_0.id);
                if (uperms.GlobalDynamicBonesEnabled || avatarDescriptor.GetComponentInParent<VRCPlayer>().prop_Player_0.prop_APIUser_0.id == APIUser.CurrentUser.id || Configuration.JSONConfig.EveryoneGlobalDynamicBonesEnabled)
                {
                    if (!aperms.HandColliders && !aperms.FeetColliders)
                    {
                        foreach (DynamicBoneCollider coll in avatarObject.GetComponentsInChildren<DynamicBoneCollider>())
                        {
                            currentWorldDynamicBoneColliders.Add(coll);
                        }
                    }
                    else
                    {
                        if (aperms.HandColliders)
                        {
                            foreach (DynamicBoneCollider coll in avatarObject.GetComponentInChildren<Animator>().GetBoneTransform(HumanBodyBones.LeftHand).GetComponentsInChildren<DynamicBoneCollider>())
                                if (coll.m_Bound != DynamicBoneCollider.EnumNPublicSealedvaOuIn3vUnique.Inside)
                                    currentWorldDynamicBoneColliders.Add(coll);
                            foreach (DynamicBoneCollider coll in avatarObject.GetComponentInChildren<Animator>().GetBoneTransform(HumanBodyBones.RightHand).GetComponentsInChildren<DynamicBoneCollider>())
                                if (coll.m_Bound != DynamicBoneCollider.EnumNPublicSealedvaOuIn3vUnique.Inside)
                                    currentWorldDynamicBoneColliders.Add(coll);
                        }
                        if (aperms.FeetColliders)
                        {
                            foreach (DynamicBoneCollider coll in avatarObject.GetComponentInChildren<Animator>().GetBoneTransform(HumanBodyBones.LeftFoot).GetComponentsInChildren<DynamicBoneCollider>())
                                if (coll.m_Bound != DynamicBoneCollider.EnumNPublicSealedvaOuIn3vUnique.Inside)
                                    currentWorldDynamicBoneColliders.Add(coll);
                            foreach (DynamicBoneCollider coll in avatarObject.GetComponentInChildren<Animator>().GetBoneTransform(HumanBodyBones.RightFoot).GetComponentsInChildren<DynamicBoneCollider>())
                                if (coll.m_Bound != DynamicBoneCollider.EnumNPublicSealedvaOuIn3vUnique.Inside)
                                    currentWorldDynamicBoneColliders.Add(coll);
                        }
                    }
                    if (!aperms.HeadBones && !aperms.ChestBones && !aperms.HipBones)
                    {
                        foreach (DynamicBone bone in avatarObject.GetComponentsInChildren<DynamicBone>())
                        {
                            currentWorldDynamicBones.Add(bone);
                        }
                    }
                    foreach (DynamicBone bone in currentWorldDynamicBones)
                    {
                        bone.m_Colliders.Clear();
                        foreach (DynamicBoneCollider coll in currentWorldDynamicBoneColliders)
                            bone.m_Colliders.Add(coll);
                    }
                }
            }
        }
    }
}