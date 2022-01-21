﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using emmVRC.Objects.ModuleBases;

namespace emmVRC.Functions.WorldHacks
{
    // Here are the component types we have to work with:

    // --eVRCDisable--
    // When on an object, emmVRC will just disable the entire object on scene load. Simple enough.

    // --eVRCEnable--
    // When on an object, emmVRC will just enable the entire object on scene load. Also pretty simple.

    // --eVRCPanel--
    // When on an object, this will display all kinds of stats from the emmVRC Network, including:
    // * Currently online users
    // * Past online users (over 30 day period)
    // * Snippet of global chat

    // I'm not too worried about getting this in, until we get the network going. It's not going to matter much up until that point.

    public class CustomWorldObjects : MelonLoaderEvents
    {
        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            if (buildIndex != -1) return;
            return;
            GameObject[] allObjects = UnityEngine.Resources.FindObjectsOfTypeAll<GameObject>();
            foreach (GameObject obj in allObjects)
            {
                if (obj.name == "eVRCDisable")
                {
                    obj.SetActive(false);
                }
                else if (obj.name == "eVRCEnable")
                {
                    obj.SetActive(true);
                }
                else if (obj.name == "eVRCPanel")
                {
                    obj.AddComponent<Components.emmVRCPanel>();
                }
            }
        }
    }
}
