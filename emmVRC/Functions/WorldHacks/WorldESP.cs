using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace emmVRC.Functions.WorldHacks
{
    public class WorldESP
    {
        internal static void ToggleItemESP(bool toggle)
        {
            foreach (var gameObject in Hacks.ComponentToggle.Pickup_stored)
            {
                Renderer bubbleRenderer = gameObject.GetComponent<Renderer>();
                if (bubbleRenderer != null)
                {
                    HighlightsFX.prop_HighlightsFX_0.Method_Public_Void_Renderer_Boolean_0(bubbleRenderer, toggle);
                }
            }
        }

        internal static void ToggleTriggerESP(bool toggle)
        {
            foreach (var gameObject in Hacks.ComponentToggle.Trigger_stored)
            {
                Renderer bubbleRenderer = gameObject.GetComponent<Renderer>();
                if (bubbleRenderer != null)
                {
                    HighlightsFX.prop_HighlightsFX_0.Method_Public_Void_Renderer_Boolean_0(bubbleRenderer, toggle);
                }
            }
        }
    }
}
