using emmVRC.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VRC.Core;
using VRC.SDKBase;
using emmVRC.Objects.ModuleBases;

namespace emmVRC.Functions.PlayerHacks
{
    public class GlobalDynamicBones : MelonLoaderEvents
    {
        // Cache for all the current dynamic bones in the world
        private static List<DynamicBone> currentWorldDynamicBones = new List<DynamicBone>();

        // Cache for all the current dynamic bone colliders in the world
        private static List<DynamicBoneCollider> currentWorldDynamicBoneColliders = new List<DynamicBoneCollider>();

        public override void OnUiManagerInit()
        {
            if (Functions.Core.ModCompatibility.MultiplayerDynamicBones) return;
            Utils.NetworkEvents.OnAvatarInstantiated += (plr, avtr, gobj) => ProcessDynamicBones(gobj, avtr, plr);
            Configuration.onConfigUpdated.Add(new System.Collections.Generic.KeyValuePair<string, Action>("GlobalDynamicBonesEnabled", Utils.PlayerUtils.ReloadAllAvatars));
            Configuration.onConfigUpdated.Add(new System.Collections.Generic.KeyValuePair<string, Action>("FriendGlobalDynamicBonesEnabled", Utils.PlayerUtils.ReloadAllAvatars));
            Configuration.onConfigUpdated.Add(new System.Collections.Generic.KeyValuePair<string, Action>("EveryoneGlobalDynamicBonesEnabled", Utils.PlayerUtils.ReloadAllAvatars));
        }

        public static void ProcessDynamicBones(GameObject avatarObject, ApiAvatar avtr, VRCPlayer player)
        {
            // Check if we should do anything at all
            if (Configuration.JSONConfig.GlobalDynamicBonesEnabled)
            {
                if (avatarObject == null)
                    emmVRCLoader.Logger.LogDebug("Avatar object is null");
                if (avtr == null)
                    emmVRCLoader.Logger.LogDebug("APIAvatar is invalid");
                if (player == null)
                    emmVRCLoader.Logger.LogDebug("Player is invalid");
                if (avatarObject == null || avtr == null || player == null) return;

                // Grab avatar permissions for the loaded avatar
                AvatarPermissions aperms = AvatarPermissions.GetAvatarPermissions(avtr.id);

                // Grab user permissions for the avatar's user
                UserPermissions uperms = UserPermissions.GetUserPermissions(player._player.prop_APIUser_0.id);

                // If the user can have global dynamic bones (or is you), or "Everyone Global Dynamic Bones" is on...
                if (uperms.GlobalDynamicBonesEnabled || player == VRCPlayer.field_Internal_Static_VRCPlayer_0 || Configuration.JSONConfig.EveryoneGlobalDynamicBonesEnabled || ( Configuration.JSONConfig.FriendGlobalDynamicBonesEnabled && APIUser.IsFriendsWith(player._player.prop_APIUser_0.id)))
                {
                    emmVRCLoader.Logger.LogDebug("Global Dynamic Bones is allowed for this user, processing...");
                    // If neither Hand nor Feet colliders is turned on, fetch all the colliders from the avatar and add them to cache
                    if (!aperms.HandColliders && !aperms.FeetColliders)
                    {
                        emmVRCLoader.Logger.LogDebug("Hand and Feet colliders are disabled, so we're fetching all colliders.");
                        foreach (DynamicBoneCollider coll in avatarObject.GetComponentsInChildren<DynamicBoneCollider>())
                        {
                            currentWorldDynamicBoneColliders.Add(coll);
                        }
                        emmVRCLoader.Logger.LogDebug("There are " + currentWorldDynamicBoneColliders.Count + " processed colliders in this instance.");
                    }
                    else
                    {
                        emmVRCLoader.Logger.LogDebug("Hand colliders are " + (aperms.HandColliders ? "enabled" : "disabled") + ", foot colliders are " + (aperms.FeetColliders ? "enabled" : "disabled"));
                        // If hand colliders specifically is on, fetch all the colliders for each hand and add them to cache
                        if (aperms.HandColliders && avatarObject.GetComponentInChildren<Animator>() != null)
                        {
                            if (avatarObject.GetComponentInChildren<Animator>().GetBoneTransform(HumanBodyBones.LeftHand) != null && avatarObject.GetComponentInChildren<Animator>().GetBoneTransform(HumanBodyBones.RightHand) != null)
                            {
                                emmVRCLoader.Logger.LogDebug("This avatar has valid hands, fetching colliders...");
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
                                emmVRCLoader.Logger.LogDebug("There are " + currentWorldDynamicBoneColliders.Count + " processed colliders in this instance.");
                            }
                        }
                        // If feet folliders specifically is on, fetch all the colliders for each foot and add them to cache
                        if (aperms.FeetColliders && avatarObject.GetComponentInChildren<Animator>() != null)
                        {
                            if (avatarObject.GetComponentInChildren<Animator>().GetBoneTransform(HumanBodyBones.LeftFoot) != null && avatarObject.GetComponentInChildren<Animator>().GetBoneTransform(HumanBodyBones.RightFoot) != null)
                            {
                                emmVRCLoader.Logger.LogDebug("This avatar has valid feet, fetching colliders...");
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
                                emmVRCLoader.Logger.LogDebug("There are " + currentWorldDynamicBoneColliders.Count + " processed colliders in this instance.");
                            }
                        }
                    }

                    // If global dynamics are allowed for all parts of the avatar, grab the dynamic bones for the whole avatar and add them to cache
                    if (!aperms.HeadBones && !aperms.ChestBones && !aperms.HipBones)
                    {
                        emmVRCLoader.Logger.LogDebug("Global Dynamic Bones are allowed for all parts of the avatar. Processing bones...");
                        foreach (DynamicBone bone in avatarObject.GetComponentsInChildren<DynamicBone>())
                        {
                            currentWorldDynamicBones.Add(bone);
                        }
                        emmVRCLoader.Logger.LogDebug("There are " + currentWorldDynamicBones.Count + " processed bones in this instance.");
                    }

                    // Cycle through each dynamic bone in the cache, remove existing colliders, and then add the collider cache to them. 
                    emmVRCLoader.Logger.LogDebug("Now processing available dynamic bones, and adding colliders...");
                    foreach (DynamicBone bone in currentWorldDynamicBones.ToList())
                    {
                        if (bone == null)
                        {
                            currentWorldDynamicBones.Remove(bone);
                        }
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
            emmVRCLoader.Logger.LogDebug("Done processing dynamic bones for avatar.");
        }
    }
}