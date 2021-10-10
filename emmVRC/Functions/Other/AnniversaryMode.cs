using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using emmVRC.Objects.ModuleBases;

namespace emmVRC.Functions.Other
{
    public class AnniversaryMode : MelonLoaderEvents
    {
        public override void OnApplicationStart()
        {
            // A little throwback to the first ever mod I worked on, YoshiMod
            if (Environment.CommandLine.Contains("--emmvrc.anniversarymode") || (DateTime.Now.Day == 6 && DateTime.Now.Month == 5))
            {
                emmVRCLoader.Logger.Log("Hello world!");
                emmVRCLoader.Logger.Log("This is the beginning of a new beginning!");
                emmVRCLoader.Logger.Log("Wait... have I said that before?");
            }
        }
    }
}
