using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using emmVRC.Components;
using emmVRC.Libraries;
using emmVRC.Utils;
using emmVRC.Objects;
using emmVRC.Objects.ModuleBases;
using UnityEngine;
using UnityEngine.UI;
using VRC;
using VRC.DataModel;
using VRC.Core;

namespace emmVRC.Menus
{
    [Priority(55)]
    public class AvatarOptionsMenu : MelonLoaderEvents
    {
        private static MenuPage avatarOptionsPage;
        
        private static ButtonGroup avatarOptionsGroup;
        private static ToggleButton dynamicBonesToggle;
        private static ToggleButton particleSystemsToggle;
        private static ToggleButton clothToggle;
        private static ToggleButton shadersToggle;
        private static ToggleButton audioSourcesToggle;
        private static ToggleButton lightsToggle;

        private static ButtonGroup globalDynamicsGroup;
        private static ToggleButton handCollidersToggle;
        private static ToggleButton footCollidersToggle;

        public static VRCPlayer selectedPlayer;

        private static AvatarPermissions currPermissions;
        private static bool _initialized = false;
        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            if (buildIndex != -1 || _initialized) return;
            //Transform baseMenuTransform = ButtonAPI.menuPageBase.transform.parent.Find("Menu_SelectedUser_Local/ScrollRect/Viewport/VerticalLayoutGroup");
            avatarOptionsPage = new MenuPage("emmVRC_AvatarOptions", "Avatar Options", false);
            avatarOptionsGroup = new ButtonGroup(avatarOptionsPage, "Components");
            
            dynamicBonesToggle = new ToggleButton(avatarOptionsGroup, "Dynamic Bones", (bool val) =>
            {
                currPermissions.DynamicBonesEnabled = val;
                Managers.AvatarPermissionsManager.SaveAvatarPermissions(currPermissions);
                selectedPlayer?.ReloadAvatar();
            }, "Turn on Dynamic Bones for this avatar", "Turn off Dynamic Bones for this avatar");
            particleSystemsToggle = new ToggleButton(avatarOptionsGroup, "Particle Systems", (bool val) =>
            {
                currPermissions.ParticleSystemsEnabled = val;
                Managers.AvatarPermissionsManager.SaveAvatarPermissions(currPermissions);
                selectedPlayer?.ReloadAvatar();
            }, "Turn on Particle Systems for this avatar", "Turn off Particle Systems for this avatar");
            clothToggle = new ToggleButton(avatarOptionsGroup, "Cloth", (bool val) =>
            {
                currPermissions.ClothEnabled = val;
                Managers.AvatarPermissionsManager.SaveAvatarPermissions(currPermissions);
                selectedPlayer?.ReloadAvatar();
            }, "Turn on Cloth for this avatar", "Turn off Cloth for this avatar");
            shadersToggle = new ToggleButton(avatarOptionsGroup, "Shaders", (bool val) =>
            {
                currPermissions.ShadersEnabled = val;
                Managers.AvatarPermissionsManager.SaveAvatarPermissions(currPermissions);
                selectedPlayer?.ReloadAvatar();
            }, "Turn on Shaders for this avatar", "Turn off Shaders for this avatar");
            new ButtonGroup(avatarOptionsPage, "");
            audioSourcesToggle = new ToggleButton(avatarOptionsGroup, "Audio Sources", (bool val) =>
            {
                currPermissions.AudioSourcesEnabled = val;
                Managers.AvatarPermissionsManager.SaveAvatarPermissions(currPermissions);
                selectedPlayer?.ReloadAvatar();
            }, "Turn on Audio Sources for this avatar", "Turn off Audio Sources for this avatar");
            _initialized = true;
            lightsToggle = new ToggleButton(avatarOptionsGroup, "Lights", (bool val) =>
            {
                currPermissions.LightsEnabled = val;
                Managers.AvatarPermissionsManager.SaveAvatarPermissions(currPermissions);
                selectedPlayer?.ReloadAvatar();
            }, "Turn on Lights for this avatar", "Turn off Lights for this avatar");

            globalDynamicsGroup = new ButtonGroup(avatarOptionsPage, "Global Dynamics");
            handCollidersToggle = new ToggleButton(globalDynamicsGroup, "Hand Colliders", (bool val) =>
            {
                currPermissions.HandColliders = val;
                Managers.AvatarPermissionsManager.SaveAvatarPermissions(currPermissions);
                selectedPlayer?.ReloadAvatar();
            }, "Turn on Hand Colliders for Global Dynamic Bones", "Turn off Hand Colliders for Global Dynamic Bones");
            footCollidersToggle = new ToggleButton(globalDynamicsGroup, "Feet Colliders", (bool val) =>
            {
                currPermissions.FeetColliders = val;
                Managers.AvatarPermissionsManager.SaveAvatarPermissions(currPermissions);
                selectedPlayer?.ReloadAvatar();
            }, "Turn on Feet Colliders for Global Dynamic Bones", "Turn off Feet Colliders for Global Dynamic Bones");
        }
        public static void OpenMenu(VRCPlayer targetPlayer)
        {
            avatarOptionsPage.OpenMenu();
            emmVRCLoader.Logger.LogDebug("OpenMenu called on Avatar Options");
            selectedPlayer = targetPlayer;
            ApiAvatar targetAvatar = selectedPlayer?.prop_VRCAvatarManager_0.field_Private_ApiAvatar_0;
            if (targetAvatar == null)
                targetAvatar = selectedPlayer?.prop_VRCAvatarManager_0.field_Private_ApiAvatar_1;
            emmVRCLoader.Logger.LogDebug("Target avatar is " + (targetAvatar == null ? "null" : "not null"));
            Managers.AvatarPermissionsManager.TryGetAvatarPermissions(targetAvatar.id, out currPermissions);
            emmVRCLoader.Logger.LogDebug("Tried getting avatar permissions, currPermissions is " + (currPermissions == null ? "null" : "not null"));
            if (currPermissions == null)
                currPermissions = new AvatarPermissions() { AvatarId = targetAvatar.id };
            dynamicBonesToggle.SetToggleState(currPermissions.DynamicBonesEnabled);
            particleSystemsToggle.SetToggleState(currPermissions.ParticleSystemsEnabled);
            clothToggle.SetToggleState(currPermissions.ClothEnabled);
            shadersToggle.SetToggleState(currPermissions.ShadersEnabled);
            audioSourcesToggle.SetToggleState(currPermissions.AudioSourcesEnabled);
            lightsToggle.SetToggleState(currPermissions.LightsEnabled);

            handCollidersToggle.SetToggleState(currPermissions.HandColliders);
            footCollidersToggle.SetToggleState(currPermissions.FeetColliders);

        }
    }
}
