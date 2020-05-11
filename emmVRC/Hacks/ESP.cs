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
                if (ESPEnabled)
                {
                    foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Player"))
                    {
                        if (obj.transform.Find("SelectRegion"))
                        {
                            HighlightsFX.prop_HighlightsFX_0.Method_Public_Renderer_Boolean_1(obj.transform.Find("SelectRegion").GetComponent<Renderer>(), true);
                        }
                    }
                    wasEnabled = true;
                }
                else if (!ESPEnabled && wasEnabled)
                {
                    foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Player"))
                    {
                        if (obj.transform.Find("SelectRegion"))
                        {
                            HighlightsFX.prop_HighlightsFX_0.Method_Public_Renderer_Boolean_1(obj.transform.Find("SelectRegion").GetComponent<Renderer>(), false);
                        }
                    }
                    wasEnabled = false;
                }
            }
        }
    }
}
