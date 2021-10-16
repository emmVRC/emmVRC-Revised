using emmVRC.Libraries;
using emmVRC.Menus;
using emmVRC.Objects.ModuleBases;
using emmVRC.Network.Objects;
using Il2CppSystem.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VRC.Core;

namespace emmVRC.Managers
{
    public class AvatarPermissions
    {
        public string AvatarId;
        public bool HandColliders = true;
        public bool FeetColliders;
        public bool HeadBones;
        public bool ChestBones;
        public bool HipBones;
        public bool DynamicBonesEnabled = true;
        public bool ParticleSystemsEnabled = true;
        public bool ClothEnabled = true;
        public bool ShadersEnabled = true;
        public bool AudioSourcesEnabled = true;
        public static AvatarPermissions GetAvatarPermissions(string avatarId)
        {
            if (File.Exists(Path.Combine(Environment.CurrentDirectory, "UserData/emmVRC/AvatarPermissions/" + avatarId + ".json")))
            {
                AvatarPermissions perms = TinyJSON.Decoder.Decode(File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "UserData/emmVRC/AvatarPermissions/" + avatarId + ".json"))).Make<AvatarPermissions>();
                return perms;
            }
            else
            {
                AvatarPermissions perms = new AvatarPermissions { AvatarId = avatarId };
                return perms;
            }
        }
        public void SaveAvatarPermissions()
        {
            File.WriteAllText(Path.Combine(Environment.CurrentDirectory, "UserData/emmVRC/AvatarPermissions/" + AvatarId + ".json"), TinyJSON.Encoder.Encode(this));
        }
    }
    [Priority(50)]
    public class AvatarPermissionManager : MelonLoaderEvents
    {
        public static PaginatedMenu baseMenu;
        public static PageItem DynamicBonesEnabledToggle;
        public static PageItem ParticleSystemsEnabledToggle;
        public static PageItem ClothEnabledToggle;
        public static PageItem ShadersEnabledToggle;
        public static PageItem AudioSourcesEnabledToggle;

        public static PageItem HandCollidersToggle;
        public static PageItem FeetCollidersToggle;

        public static PageItem HeadBonesToggle;
        public static PageItem ChestBonesToggle;
        public static PageItem HipBonesToggle;

        public static bool UserInteractMenu = false;

        public static UnityAction originalBackAction;

        public static AvatarPermissions selectedAvatarPermissions;
        public override void OnUiManagerInit()
        {
            if (!Directory.Exists(Path.Combine(Environment.CurrentDirectory, "UserData/emmVRC/AvatarPermissions/")))
                Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, "UserData/emmVRC/AvatarPermissions/"));

            baseMenu = new PaginatedMenu(UserTweaksMenu.UserTweaks, 29304, 102394, "Avatar\nPermissions", "", null);
            baseMenu.menuEntryButton.DestroyMe();
            DynamicBonesEnabledToggle = new PageItem("Dynamic Bones", () =>
            {
                selectedAvatarPermissions.DynamicBonesEnabled = true;
                selectedAvatarPermissions.SaveAvatarPermissions();
                ReloadAvatars();
            }, "Disabled", () =>
            {
                selectedAvatarPermissions.DynamicBonesEnabled = false;
                selectedAvatarPermissions.SaveAvatarPermissions();
                ReloadAvatars();
            }, "TOGGLE: Enables Dynamic Bones for the selected avatar", true, true);
            ParticleSystemsEnabledToggle = new PageItem("Particle\nSystems", () =>
            {
                selectedAvatarPermissions.ParticleSystemsEnabled = true;
                selectedAvatarPermissions.SaveAvatarPermissions();
                ReloadAvatars();
            }, "Disabled", () =>
            {
                selectedAvatarPermissions.ParticleSystemsEnabled = false;
                selectedAvatarPermissions.SaveAvatarPermissions();
                ReloadAvatars();
            }, "TOGGLE: Enables Particle Systems for the selected avatar", true, true);
            ClothEnabledToggle = new PageItem("Cloth", () =>
            {
                selectedAvatarPermissions.ClothEnabled = true;
                selectedAvatarPermissions.SaveAvatarPermissions();
                ReloadAvatars();
            }, "Disabled", () =>
            {
                selectedAvatarPermissions.ClothEnabled = false;
                selectedAvatarPermissions.SaveAvatarPermissions();
                ReloadAvatars();
            }, "TOGGLE: Enables Cloth for the selected avatar", true, true);
            ShadersEnabledToggle = new PageItem("Shaders", () =>
            {
                selectedAvatarPermissions.ShadersEnabled = true;
                selectedAvatarPermissions.SaveAvatarPermissions();
                ReloadAvatars();
            }, "Disabled", () =>
            {
                selectedAvatarPermissions.ShadersEnabled = false;
                selectedAvatarPermissions.SaveAvatarPermissions();
                ReloadAvatars();
            }, "TOGGLE: Enables Shaders for the selected avatar", true, true);
            AudioSourcesEnabledToggle = new PageItem("Audio\nSources", () =>
            {
                selectedAvatarPermissions.AudioSourcesEnabled = true;
                selectedAvatarPermissions.SaveAvatarPermissions();
                ReloadAvatars();
            }, "Disabled", () =>
            {
                selectedAvatarPermissions.AudioSourcesEnabled = false;
                selectedAvatarPermissions.SaveAvatarPermissions();
                ReloadAvatars();
            }, "TOGGLE: Enables Audio Sources for the selected avatar", true, true);
            baseMenu.pageItems.Add(DynamicBonesEnabledToggle);
            baseMenu.pageItems.Add(ParticleSystemsEnabledToggle);
            baseMenu.pageItems.Add(ClothEnabledToggle);
            baseMenu.pageItems.Add(ShadersEnabledToggle);
            baseMenu.pageItems.Add(AudioSourcesEnabledToggle);
            baseMenu.pageItems.Add(PageItem.Space);
            baseMenu.pageItems.Add(PageItem.Space);
            baseMenu.pageItems.Add(PageItem.Space);
            baseMenu.pageItems.Add(PageItem.Space);

            HandCollidersToggle = new PageItem("Hand\nColliders", () =>
            {
                selectedAvatarPermissions.HandColliders = true;
                selectedAvatarPermissions.SaveAvatarPermissions();
                ReloadAvatars();
            }, "Disabled", () =>
            {
                selectedAvatarPermissions.HandColliders = false;
                selectedAvatarPermissions.SaveAvatarPermissions();
                ReloadAvatars();
            }, "TOGGLE: Select to only use these colliders for Global Dynamic Bone interactions");
            FeetCollidersToggle = new PageItem("Feet\nColliders", () =>
            {
                selectedAvatarPermissions.FeetColliders = true;
                selectedAvatarPermissions.SaveAvatarPermissions();
                ReloadAvatars();
            }, "Disabled", () =>
            {
                selectedAvatarPermissions.FeetColliders = false;
                selectedAvatarPermissions.SaveAvatarPermissions();
                ReloadAvatars();
            }, "TOGGLE: Select to only use these colliders for Global Dynamic Bone interactions");
            baseMenu.pageItems.Add(HandCollidersToggle);
            baseMenu.pageItems.Add(FeetCollidersToggle);

            baseMenu.pageTitles.Add("Avatar Features");
            baseMenu.pageTitles.Add("Exclusive Global Dynamic Bone Colliders");
            baseMenu.menuBase.getBackButton().getGameObject().GetComponent<Button>().onClick = new Button.ButtonClickedEvent();
            baseMenu.menuBase.getBackButton().getGameObject().GetComponent<Button>().onClick.AddListener(new System.Action(() =>
            {
                if (UserInteractMenu)
                    QuickMenuUtils.ShowQuickmenuPage("UserInteractMenu");
                else
                    QuickMenuUtils.ShowQuickmenuPage(PlayerTweaksMenuLegacy.baseMenu.getMenuName());
            }));

        }
        public static void ReloadAvatars()
        {
            VRCPlayer.field_Internal_Static_VRCPlayer_0.ReloadAllAvatars();
            //VRCPlayer.field_Internal_Static_VRCPlayer_0.prop_VRCAvatarManager_0.Method_Private_Void_Boolean_1(false);
            //VRCPlayer.field_Internal_Static_VRCPlayer_0.prop_VRCAvatarManager_0.Method_Private_Void_Boolean_2(false);
        }
        public static void OpenMenu(string avatarId, bool inUserInteractMenu = false)
        {
            UserInteractMenu = inUserInteractMenu;
            selectedAvatarPermissions = AvatarPermissions.GetAvatarPermissions(avatarId);
            DynamicBonesEnabledToggle.SetToggleState(selectedAvatarPermissions.DynamicBonesEnabled);
            ParticleSystemsEnabledToggle.SetToggleState(selectedAvatarPermissions.ParticleSystemsEnabled);
            ClothEnabledToggle.SetToggleState(selectedAvatarPermissions.ClothEnabled);
            ShadersEnabledToggle.SetToggleState(selectedAvatarPermissions.ShadersEnabled);
            AudioSourcesEnabledToggle.SetToggleState(selectedAvatarPermissions.AudioSourcesEnabled);

            HandCollidersToggle.SetToggleState(selectedAvatarPermissions.HandColliders);
            FeetCollidersToggle.SetToggleState(selectedAvatarPermissions.FeetColliders);
            baseMenu.OpenMenu();
        }
        public static void ProcessAvatar(GameObject avatarObject, VRC.SDKBase.VRC_AvatarDescriptor avatarDescriptor)
        {
            if (avatarObject == null || avatarDescriptor == null || avatarDescriptor.GetComponent<PipelineManager>() == null) return;
            foreach (Transform trsf in new Transform[] { avatarObject.transform, avatarObject.transform.parent.Find("_AvatarMirrorClone"), avatarObject.transform.parent.Find("_AvatarShadowClone") })
            {
                if (trsf == null) return;
                GameObject obj = trsf.gameObject;
                try
                {
                    AvatarPermissions perms = AvatarPermissions.GetAvatarPermissions(avatarDescriptor.GetComponent<PipelineManager>().blueprintId);
                    if (!perms.AudioSourcesEnabled)
                        foreach (AudioSource src in obj.GetComponentsInChildren<AudioSource>(true))
                            if (src != null)
                                GameObject.Destroy(src);
                    if (!perms.ClothEnabled)
                        foreach (Cloth clth in obj.GetComponentsInChildren<Cloth>(true))
                            if (clth != null)
                                GameObject.Destroy(clth);
                    if (!perms.DynamicBonesEnabled)
                    {
                        foreach (DynamicBone bone in obj.GetComponentsInChildren<DynamicBone>(true))
                            if (bone != null)
                                GameObject.Destroy(bone);
                        foreach (DynamicBoneCollider coll in obj.GetComponentsInChildren<DynamicBoneCollider>(true))
                            if (coll != null)
                                GameObject.Destroy(coll);
                    }
                    if (!perms.ParticleSystemsEnabled)
                        foreach (ParticleSystem part in obj.GetComponentsInChildren<ParticleSystem>(true))
                            if (part != null)
                                part.maxParticles = 0;
                    if (!perms.ShadersEnabled)
                    {
                        foreach (Renderer rend in obj.GetComponentsInChildren<Renderer>(true))
                        {
                            if (rend != null)
                            {
                                emmVRCLoader.Logger.LogDebug("Found renderer on " + rend.gameObject.name);
                                foreach (Material mat in rend.materials)
                                {
                                    if (mat != null)
                                    {
                                        emmVRCLoader.Logger.LogDebug("Found material " + mat.name + " using shader " + mat.shader.name + ", overriding with Diffuse...");
                                        mat.shader = Shader.Find("Diffuse");
                                    }
                                }
                            }
                        }
                    }
                    emmVRCLoader.Logger.LogDebug("Finished processing avatar permissions");
                }
                catch (Exception ex)
                {
                    emmVRCLoader.Logger.LogError("Error processing avatar: " + ex.ToString());
                }
            }
        }
    }
}
