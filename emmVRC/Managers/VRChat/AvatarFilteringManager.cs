using emmVRC.Objects;
using emmVRC.Objects.ModuleBases;
using emmVRC.Utils;
using System;
using System.Diagnostics;
using UnityEngine;
using VRC.Core;

namespace emmVRC.Managers.VRChat
{
    public class AvatarFilteringManager : MelonLoaderEvents
    {
        public static Action<MonoBehaviour> CheckTransformAction;

        private static readonly Shader diffuse = Shader.Find("VRChat/PC/Toon Lit");

        public static readonly Action<Transform> CheckLights = new Action<Transform>((parent) =>
        {
            foreach (Light light in parent.GetComponentsInChildren<Light>(true))
                if (light != null)
                    GameObject.DestroyImmediate(light);
        });
        public static readonly Action<Transform> CheckAudioSource = new Action<Transform>((parent) =>
        {
            foreach (AudioSource source in parent.GetComponentsInChildren<AudioSource>(true))
                if (source != null)
                    GameObject.DestroyImmediate(source);
        });
        public static readonly Action<Transform> CheckCloth = new Action<Transform>((parent) =>
        {
            foreach (Cloth cloth in parent.GetComponentsInChildren<Cloth>(true))
                if (cloth != null)
                    GameObject.DestroyImmediate(cloth);
        });
        public static readonly Action<Transform> CheckDynamicBone = new Action<Transform>((parent) =>
        {
            foreach (DynamicBone bone in parent.GetComponentsInChildren<DynamicBone>(true))
                if (bone != null)
                    GameObject.DestroyImmediate(bone);

            foreach (DynamicBoneCollider collider in parent.GetComponentsInChildren<DynamicBoneCollider>(true))
                if (collider != null)
                    GameObject.DestroyImmediate(collider);
        });
        public static readonly Action<Transform> CheckParticleSystem = new Action<Transform>((parent) =>
        {
            foreach (ParticleSystem system in parent.GetComponentsInChildren<ParticleSystem>(true))
                if (system != null)
                    system.maxParticles = 0;
        });
        public static readonly Action<Transform> CheckShader = new Action<Transform>((parent) =>
        {
            foreach (Renderer renderer in parent.GetComponentsInChildren<Renderer>(true))
                if (renderer != null)
                    foreach (Material material in renderer.materials)
                        material.shader = diffuse;
            foreach (MeshRenderer renderer in parent.GetComponentsInChildren<MeshRenderer>(true))
                if (renderer != null)
                    foreach (Material material in renderer.materials)
                        material.shader = diffuse;
            foreach (SkinnedMeshRenderer renderer in parent.GetComponentsInChildren<SkinnedMeshRenderer>(true))
                if (renderer != null)
                    foreach (Material material in renderer.materials)
                        material.shader = diffuse;
        });

        public override void OnApplicationStart()
        {
            NetworkEvents.OnAvatarInstantiated += OnAvatarInstantiated;
        }

        public static void OnAvatarInstantiated(VRCPlayer player, ApiAvatar avatar, GameObject gameObject)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            if (!AvatarPermissionsManager.TryGetAvatarPermissions(avatar.id, out AvatarPermissions avatarPerms))
                return;

            Action<Transform> componentCheck = null;
            if (!avatarPerms.LightsEnabled)
                componentCheck += CheckLights;
            if (!avatarPerms.AudioSourcesEnabled)
                componentCheck += CheckAudioSource;
            if (!avatarPerms.ClothEnabled)
                componentCheck += CheckCloth;
            if (!avatarPerms.DynamicBonesEnabled)
                componentCheck += CheckDynamicBone;
            if (!avatarPerms.ParticleSystemsEnabled)
                componentCheck += CheckParticleSystem;
            if (!avatarPerms.ShadersEnabled)
                componentCheck += CheckShader;

            if (componentCheck == null)
                return;

            foreach (var child in gameObject.transform.parent)
            {
                Transform tranform = child.Cast<Transform>();
                // Get shadow and mirror clones
                if (tranform.name.Contains("Avatar"))
                    componentCheck(tranform);
            }
            emmVRCLoader.Logger.LogDebug($"Filtering avatar took {stopwatch.ElapsedTicks / (float)TimeSpan.TicksPerMillisecond:n3}ms");
        }
    }
}