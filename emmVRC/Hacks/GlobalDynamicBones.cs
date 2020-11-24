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
        // Cache for all the current dynamic bones in the world
        private static List<DynamicBone> currentWorldDynamicBones = new List<DynamicBone>();

        // Cache for all the current dynamic bone colliders in the world
        private static List<DynamicBoneCollider> currentWorldDynamicBoneColliders = new List<DynamicBoneCollider>();

        public static void ProcessDynamicBones(GameObject avatarObject, VRC_AvatarDescriptor avatarDescriptor)
        {
            // Check if we should do anything at all
            if (Configuration.JSONConfig.GlobalDynamicBonesEnabled)
            {
                // Grab avatar permissions for the loaded avatar
                AvatarPermissions aperms = AvatarPermissions.GetAvatarPermissions(avatarDescriptor.GetComponent<PipelineManager>().blueprintId);

                // Grab user permissions for the avatar's user
                UserPermissions uperms = UserPermissions.GetUserPermissions(avatarDescriptor.GetComponentInParent<VRCPlayer>().prop_Player_0.prop_APIUser_0.id);

                // If the user can have global dynamic bones (or is you), or "Everyone Global Dynamic Bones" is on...
                if (uperms.GlobalDynamicBonesEnabled || avatarDescriptor.GetComponentInParent<VRCPlayer>().prop_Player_0.prop_APIUser_0.id == APIUser.CurrentUser.id || Configuration.JSONConfig.EveryoneGlobalDynamicBonesEnabled || ( Configuration.JSONConfig.FriendGlobalDynamicBonesEnabled && APIUser.IsFriendsWith(avatarDescriptor.GetComponentInParent<VRCPlayer>().prop_Player_0.prop_APIUser_0.id)))
                {
                    // If neither Hand nor Feet colliders is turned on, fetch all the colliders from the avatar and add them to cache
                    if (!aperms.HandColliders && !aperms.FeetColliders)
                    {
                        foreach (DynamicBoneCollider coll in avatarObject.GetComponentsInChildren<DynamicBoneCollider>())
                        {
                            currentWorldDynamicBoneColliders.Add(coll);
                        }
                    }
                    else
                    {
                        // If hand colliders specifically is on, fetch all the colliders for each hand and add them to cache
                        if (aperms.HandColliders)
                        {
                            foreach (DynamicBoneCollider coll in avatarObject.GetComponentInChildren<Animator>().GetBoneTransform(HumanBodyBones.LeftHand).GetComponentsInChildren<DynamicBoneCollider>())
                            {
                                if (coll.m_Bound != DynamicBoneCollider.EnumNPublicSealedvaOuIn3vUnique.Inside)
                                    currentWorldDynamicBoneColliders.Add(coll);
                            }
                            foreach (DynamicBoneCollider coll in avatarObject.GetComponentInChildren<Animator>().GetBoneTransform(HumanBodyBones.RightHand).GetComponentsInChildren<DynamicBoneCollider>())
                            {
                                if (coll.m_Bound != DynamicBoneCollider.EnumNPublicSealedvaOuIn3vUnique.Inside)
                                    currentWorldDynamicBoneColliders.Add(coll);
                            }
                        }
                        // If feet folliders specifically is on, fetch all the colliders for each foot and add them to cache
                        if (aperms.FeetColliders)
                        {
                            foreach (DynamicBoneCollider coll in avatarObject.GetComponentInChildren<Animator>().GetBoneTransform(HumanBodyBones.LeftFoot).GetComponentsInChildren<DynamicBoneCollider>())
                            {
                                if (coll.m_Bound != DynamicBoneCollider.EnumNPublicSealedvaOuIn3vUnique.Inside)
                                    currentWorldDynamicBoneColliders.Add(coll);
                            }
                            foreach (DynamicBoneCollider coll in avatarObject.GetComponentInChildren<Animator>().GetBoneTransform(HumanBodyBones.RightFoot).GetComponentsInChildren<DynamicBoneCollider>())
                            {
                                if (coll.m_Bound != DynamicBoneCollider.EnumNPublicSealedvaOuIn3vUnique.Inside)
                                    currentWorldDynamicBoneColliders.Add(coll);
                            }
                        }
                    }

                    // If global dynamics are allowed for all parts of the avatar, grab the dynamic bones for the whole avatar and add them to cache
                    if (!aperms.HeadBones && !aperms.ChestBones && !aperms.HipBones)
                    {
                        foreach (DynamicBone bone in avatarObject.GetComponentsInChildren<DynamicBone>())
                        {
                            currentWorldDynamicBones.Add(bone);
                        }
                    }

                    // Cycle through each dynamic bone in the cache, remove existing colliders, and then add the collider cache to them. 
                    foreach (DynamicBone bone in currentWorldDynamicBones.ToList())
                    {
                        if (bone == null)
                            currentWorldDynamicBones.Remove(bone);
                        else
                            //bone.m_Colliders.Clear();
                            foreach (DynamicBoneCollider coll in currentWorldDynamicBoneColliders.ToList())
                            {
                                if (coll == null)
                                    currentWorldDynamicBoneColliders.Remove(coll);
                                else
                                    if (bone.m_Colliders.IndexOf(coll) == -1)
                                    bone.m_Colliders.Add(coll);
                            }
                    }
                }
            }
        }
    }
}