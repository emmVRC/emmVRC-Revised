using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace emmVRCLoader
{
    public class HashUtil
    {
        public static string GetSHA256(string filePath)
        {
            if (!File.Exists(filePath))
                return Enumerable.Repeat("0", 64).Aggregate((a, b) => a + b);

            using (var sha256 = SHA256.Create())
            {
                using (var stream = File.OpenRead(filePath))
                {
                    var hash = sha256.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLower();
                }
            }
        }
    }
}