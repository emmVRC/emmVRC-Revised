using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace emmVRC.Hacks
{
    // This is a really hacky solution to not having custom components
    // Essentially, we are iterating through the objects that **should** have custom components, and updating their stuff manually.
    // This is not optimized at all, and should be scrapped the second we get real custom component support. But for now... here we go

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

    public class CustomWorldObjects
    {
        public static List<GameObject> emmVRCPanels;
        public static void Initialize()
        {

        }
        public static IEnumerator Loop()
        {
            while (true)
            {
                // TODO: Put panel code here. Essentially, we're going to iterate through each panel, and update the contents based on variables from the network.
            }
        }
        public static IEnumerator OnRoomEnter()
        {
            while (RoomManager.field_ApiWorld_0 == null)
                yield return new WaitForEndOfFrame();
            foreach (var gameObj in GameObject.FindObjectsOfType<GameObject>())
            {
                if (gameObj.name == "eVRCDisable")
                {
                    gameObj.SetActive(false);
                }
                else if (gameObj.name == "eVRCEnable")
                {
                    gameObj.SetActive(true);
                }
            }
        }
    }
}
