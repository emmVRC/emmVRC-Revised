using System;
using System.IO;
using System.Security.Cryptography;
using emmVRC.Objects.ModuleBases;

namespace emmVRC.Functions.Other
{
    [Priority(0)]
    public class HashCalculator : MelonLoaderEvents
    {
        public override void OnApplicationStart()
        {
            MD5 md5 = MD5.Create();
            System.IO.FileStream libStream = System.IO.File.OpenRead(Path.Combine(Environment.CurrentDirectory, "Dependencies/emmVRC.dll"));
            string md5hash = BitConverter.ToString(md5.ComputeHash(libStream)).Replace("-", "").ToLower();
            emmVRCLoader.Logger.Log("emmVRC module: " + md5hash);
        }
    }
}
