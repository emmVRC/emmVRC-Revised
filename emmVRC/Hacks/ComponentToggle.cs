using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VRC;
using VRC.SDKBase;
using VRCSDK2;
using RenderHeads.Media.AVProVideo;
using UnityEngine.Video;
using emmVRC.Objects.ModuleBases;

namespace emmVRC.Hacks
{
    public class ComponentToggle : MelonLoaderEvents
    {
        internal static VRC_SyncVideoPlayer[] Video_stored_sdk2;
        internal static SyncVideoPlayer[] Video_stored_sdk3; // type 1
        internal static MediaPlayer[] Video_stored_sdk3_2; // type 2
        internal static VideoPlayer[] Video_stored_sdk3_3; // type 3
        public static bool videoplayers = true;

        internal static VRC.SDKBase.VRC_Pickup[] Pickup_stored;
        public static bool pickupable = true;
        public static bool pickup_object = true;

        internal static VRC.SDKBase.VRC_Trigger[] Trigger_stored; // for TriggerESP

        public override void OnSceneWasLoaded(int buildIndex, string sceneName) { Store(); Toggle(); }

        private static void Store()
        {
            if (RoomManager.field_Internal_Static_ApiWorld_0 != null && UnityEngine.Resources.FindObjectsOfTypeAll<VRC.SDK3.Components.VRCSceneDescriptor>().Count > 0)
            {
                Video_stored_sdk3 = UnityEngine.Resources.FindObjectsOfTypeAll<SyncVideoPlayer>();
                Video_stored_sdk3_2 = UnityEngine.Resources.FindObjectsOfTypeAll<MediaPlayer>();
                Video_stored_sdk3_3 = UnityEngine.Resources.FindObjectsOfTypeAll<VideoPlayer>();
            }
            else
                Video_stored_sdk2 = UnityEngine.Resources.FindObjectsOfTypeAll<VRC_SyncVideoPlayer>();
            Pickup_stored = UnityEngine.Object.FindObjectsOfType<VRC.SDKBase.VRC_Pickup>();
            Trigger_stored = UnityEngine.Object.FindObjectsOfType<VRC.SDKBase.VRC_Trigger>();
        }

        internal static void Toggle(bool tempOn = false) // tempOn can be used to force settings ON for specific worlds if you'd like
        {
            if (RoomManager.field_Internal_Static_ApiWorld_0 != null && UnityEngine.Resources.FindObjectsOfTypeAll<VRC.SDK3.Components.VRCSceneDescriptor>().Count > 0)
            {
                if (Video_stored_sdk3 == null || Video_stored_sdk3_2 == null || Video_stored_sdk3_3 == null) Store();
                foreach (var gameObject in Video_stored_sdk3)
                {
                    if (tempOn)
                    {
                        gameObject.GetComponent<SyncVideoPlayer>().enabled = true;
                        gameObject.gameObject.SetActive(true);
                    }
                    else
                    {
                        gameObject.GetComponent<SyncVideoPlayer>().enabled = videoplayers;
                        gameObject.gameObject.SetActive(videoplayers);
                    }
                }

                foreach (var gameObject in Video_stored_sdk3_2)
                {
                    if (tempOn)
                    {
                        gameObject.GetComponent<MediaPlayer>().enabled = true;
                        gameObject.gameObject.SetActive(true);
                    }
                    else
                    {
                        gameObject.GetComponent<MediaPlayer>().enabled = videoplayers;
                        gameObject.gameObject.SetActive(videoplayers);
                    }
                }

                foreach (var gameObject in Video_stored_sdk3_3)
                {
                    if (tempOn)
                    {
                        gameObject.GetComponent<VideoPlayer>().enabled = true;
                        gameObject.gameObject.SetActive(true);
                    }
                    else
                    {
                        gameObject.GetComponent<VideoPlayer>().enabled = videoplayers;
                        gameObject.gameObject.SetActive(videoplayers);
                    }
                }
            }
            else
            {
                if (Video_stored_sdk2 == null) Store();
                foreach (var gameObject in Video_stored_sdk2)
                {
                    gameObject.GetComponent<VRC_SyncVideoPlayer>().enabled = videoplayers;
                    gameObject.gameObject.SetActive(videoplayers);
                }
            }

            if (Pickup_stored == null) Store();
            foreach (var gameObject in Pickup_stored)
            {
                if (tempOn)
                {
                    gameObject.GetComponent<VRC.SDKBase.VRC_Pickup>().pickupable = true;
                    gameObject.gameObject.SetActive(true);
                }
                else
                {
                    gameObject.GetComponent<VRC.SDKBase.VRC_Pickup>().pickupable = pickupable;
                    gameObject.gameObject.SetActive(pickup_object);
                }
            }
        }
    }
}