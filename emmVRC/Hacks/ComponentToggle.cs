using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VRC;
using VRC.SDKBase;
using VRCSDK2;

namespace emmVRC.Hacks
{
    public class ComponentToggle
    {
        public static VRC_SyncVideoPlayer[] Video_stored_sdk2;
        public static bool videoplayers = true;
        public static VRC.SDKBase.VRC_Pickup[] Pickup_stored;
        public static bool pickupable = true;
        public static bool pickup_object = true;

        public static void OnLevelLoad()
        {
            Store();
        }

        private static void Store()
        {
            Video_stored_sdk2 = UnityEngine.Resources.FindObjectsOfTypeAll<VRC_SyncVideoPlayer>();
            Pickup_stored = UnityEngine.Object.FindObjectsOfType<VRC.SDKBase.VRC_Pickup>();
        }

        public static void Toggle(bool tempOn = false) // tempOn can be used to force settings ON for specific worlds if you'd like
        {
            if (Video_stored_sdk2 == null) Store();
            foreach (var gameObject in Video_stored_sdk2)
            {
                gameObject.GetComponent<VRC_SyncVideoPlayer>().enabled = videoplayers;
                gameObject.gameObject.SetActive(videoplayers);
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
