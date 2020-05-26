using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace emmVRCLoader
{
    public class LoaderCheck
    {
        public static string Check()
        {
            var hasher = MD5.Create();
            return Convert.ToBase64String(hasher.ComputeHash(File.ReadAllBytes(Path.Combine(Environment.CurrentDirectory, "Mods/", System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName))));
        }
    }
}
