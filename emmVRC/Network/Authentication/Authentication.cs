using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using emmVRC.Network.Objects;

namespace emmVRC.Network.Authentication
{
    public static class Authentication
    {
        private static string path = "/";
        private static string[] files = System.IO.Directory.GetFiles(path, "*.ema");
        private static string extension = ".";

        public static bool Exists(string userID)
        {
            if (File.Exists(userID + extension))
                return true;
            return false;
        }

        public static string ReadTokenFile(string userID)
        {
            if (!File.Exists(userID + extension))
                return null;
            File.Decrypt(userID + extension);
            return System.IO.File.ReadAllText(userID + extension);
        }

        public static void RemoveTokenFile()
        {
            foreach (string file in files)
            {
                if (File.Exists(file))
                {
                    File.Delete(file);
                }
            }
        }

        public static void CreateTokenFile(string userID, string data)
        {
            string fileName = userID + extension;
            do
            {
                RemoveTokenFile();
            } while (File.Exists(fileName));

            using (StreamWriter streamWriter = File.CreateText(fileName))
            {
                streamWriter.Write(data);
            }

            File.Encrypt(fileName);
            files.Append(fileName);
        }
    }
}
