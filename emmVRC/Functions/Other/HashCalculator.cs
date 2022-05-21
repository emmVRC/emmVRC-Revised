using System;
using System.IO;
using System.Security.Cryptography;
using emmVRC.Objects.ModuleBases;
using emmVRCLoader;

namespace emmVRC.Functions.Other
{
    [Priority(0)]
    public class HashCalculator : MelonLoaderEvents
    {
        public override void OnApplicationStart()
        {
            emmVRCLoader.Logger.Log("emmVRC module: " + HashUtil.GetSHA256("Dependencies/emmVRC.dll"));
        }
    }
}
