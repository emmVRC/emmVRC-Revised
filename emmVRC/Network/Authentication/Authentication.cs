﻿using System;
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
        private static string path = Path.Combine(Environment.CurrentDirectory, "UserData/emmVRC/");
        private static string[] files = System.IO.Directory.GetFiles(path, "*.ema");
        private static string extension = ".ema";

        public static bool Exists(string userID)
        {
            if (File.Exists(path+userID + extension))
                return true;
            return false;
        }

        public static string ReadTokenFile(string userID)
        {
            if (!File.Exists(path+userID + extension))
                return "";
            return System.IO.File.ReadAllText(path+userID + extension);
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
                File.Delete(path+fileName);
            } while (File.Exists(path+fileName));

            File.WriteAllBytes(path+fileName, Encoding.UTF8.GetBytes(data));

            files.Append(fileName);
        }
    }
}
