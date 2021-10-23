using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using emmVRC.Objects.ModuleBases;

namespace emmVRC.Functions.Other
{
    public class LaunchOptions : MelonLoaderEvents
    {
        public override void OnApplicationStart()
        {
            emmVRCLoader.Logger.LogDebug("[NOTICE] emmVRC Debug Mode is intended for development or troubleshooting purposes. Using it in regular play can result in unexpected lag or other issues. If you are seeing this and are not sure what to do, check your launch options for `--emmvrc.debug`.");
        }
    }
}
