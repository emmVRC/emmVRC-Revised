
#if (DEBUG == true)
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using emmVRC.Objects.ModuleBases;

namespace emmVRC.Managers
{
    public class DebugAction
    {
        public KeyCode ActionKey;
        public System.Action ActionAction;
        public string Name = "";
        public string Description = "";
    }
    public class DebugManager : MelonLoaderEvents, IWithFixedUpdate
    {
        public static List<DebugAction> DebugActions = new List<DebugAction>();
        public override void OnUiManagerInit()
        {
            emmVRCLoader.Logger.LogDebug("Initializing debug manager");
            if (Environment.CommandLine.Contains("--emmvrc.debug"))
            {
                Objects.Attributes.Debug = true;
            }
        }
        public void OnFixedUpdate()
        {
            if (Objects.Attributes.Debug)
            {
                foreach (DebugAction action in DebugActions)
                {
                    if (Input.GetKeyDown(action.ActionKey))
                        action.ActionAction.Invoke();
                }
            }

        }

    }
}
#endif