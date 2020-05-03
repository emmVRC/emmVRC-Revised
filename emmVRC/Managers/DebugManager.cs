using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace emmVRC.Managers
{
    public class DebugAction
    {
        public KeyCode ActionKey;
        public System.Action ActionAction;
    }
    public class DebugManager
    {
        public static List<DebugAction> DebugActions = new List<DebugAction>();
        public static void Initialize()
        {
            if (Environment.CommandLine.Contains("--emmvrc.debug"))
            {
                Objects.Attributes.Debug = true;
            }
            MelonLoader.MelonCoroutines.Start(Loop());
        }
        public static IEnumerator Loop()
        {
            while (true)
            {
                if (Objects.Attributes.Debug)
                {
                    foreach (DebugAction action in DebugActions)
                    {
                        if (Input.GetKeyDown(action.ActionKey))
                            action.ActionAction.Invoke();
                    }
                }
                yield return new WaitForFixedUpdate();
            }
        }

    }
}
