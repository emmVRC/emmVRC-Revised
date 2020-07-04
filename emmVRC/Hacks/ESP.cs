using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace emmVRC.Hacks
{
    public class ESP
    {
        public static bool ESPEnabled;
        private static bool wasEnabled;
        public static void Initialize()
        {
            MelonLoader.MelonCoroutines.Start(Loop());
        }
        private static IEnumerator Loop()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.1f);

                // Every frame, if ESP is turned on...
                if (ESPEnabled)
                {
                    // Fetch all the Player objects in the world
                    foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Player"))
                    {
                        // Check if the players have the "SelectRegion" mesh. This is the target for ESP
                        if (obj.transform.Find("SelectRegion"))
                        {
                            // Apply the highlight effect using HighlightsFX
                            HighlightsFX.prop_HighlightsFX_0.Method_Public_Void_Renderer_Boolean_0(obj.transform.Find("SelectRegion").GetComponent<Renderer>(), true);
                        }
                    }
                    // Setting a temporary variable, so that we can turn ESP off for everyone if it is toggled off
                    wasEnabled = true;
                }
                
                // If ESP is off, but it was on before...
                else if (!ESPEnabled && wasEnabled)
                {
                    // Fetch all the Player objects in the world
                    foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Player"))
                    {
                        // Check if the players have the "SelectRegion" mesh. THis is the target for disabling ESP
                        if (obj.transform.Find("SelectRegion"))
                        {
                            // Remove the highlight effect using HighlightsFX
                            HighlightsFX.prop_HighlightsFX_0.Method_Public_Void_Renderer_Boolean_0(obj.transform.Find("SelectRegion").GetComponent<Renderer>(), false);
                        }
                    }
                    // Setting the temporary variable, so that this loop will only run once, at least until ESP is turned back on
                    wasEnabled = false;
                }
            }
        }
    }
}
