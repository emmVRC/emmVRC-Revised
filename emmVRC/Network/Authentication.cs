using System;
using System.IO;
using System.Text;

namespace emmVRC.Network
{
    public class Authentication
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
            return File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "UserData/emmVRC/" + userID + extension));
        }
        public static void DeleteTokenFile(string userID)
        {
            if (File.Exists(Path.Combine(Environment.CurrentDirectory, "UserData/emmVRC/" + userID + extension)))
                File.Delete(Path.Combine(Environment.CurrentDirectory, "UserData/emmVRC/" + userID + extension));
        }

        public static void CreateTokenFile(string userID, string data)
        {
            var fileName = userID + extension;
            File.WriteAllBytes(Path.Combine(path, fileName), Encoding.UTF8.GetBytes(data));
        }
    }
}