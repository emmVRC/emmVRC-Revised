using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using emmVRC.Network.Objects;

namespace emmVRC.Network
{
    public static class Authentication
    {
        private static string path = Path.Combine(Environment.CurrentDirectory, "UserData/emmVRC/");
        private static string extension = ".ema";

        public static bool Exists(string userID)
        {
            return File.Exists(Path.Combine(Environment.CurrentDirectory, "UserData/emmVRC/" + userID + extension));
        }

        public static string ReadTokenFile(string userID)
        {
            if (!File.Exists(Path.Combine(Environment.CurrentDirectory, "UserData/emmVRC/" + userID + extension)))
                return "";
            return System.IO.File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "UserData/emmVRC/" + userID + extension));
        }
        public static void DeleteTokenFile(string userID)
        {
            if (File.Exists(Path.Combine(Environment.CurrentDirectory, "UserData/emmVRC/" + userID + extension)))
                File.Delete(Path.Combine(Environment.CurrentDirectory, "UserData/emmVRC/" + userID + extension));
        }

        public static void CreateTokenFile(string userID, string data)
        {
            string fileName = userID + extension;
            File.WriteAllBytes(Path.Combine(path, fileName), Encoding.UTF8.GetBytes(data));
        }
    }
}