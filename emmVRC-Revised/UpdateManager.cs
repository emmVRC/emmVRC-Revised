using System;
using System.Text;
using System.IO;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;

namespace emmVRCLoader
{
    public class UpdateManager
    {
        private static string WebsiteURL = "https://dl.emmvrc.com/update.php";
        public static bool LoaderNeedsUpdate = false;
        public static bool HasCheckedLoader = false;
        public static bool HasCheckedUpdate = false;
        public static bool HasCheckedLoadLib = false;
        public static bool LoadLibCheck = true;
        internal static Byte[] downloadedLib;

        public static void Check()
        {
            if (!HasCheckedUpdate)
            {
                if (!IsLibLatest())
                {
                    Logger.Log("[emmVRCLoader] Downloading emmVRC...");
                    DownloadLib();
                }
                else
                {
                    downloadedLib = File.ReadAllBytes(Path.Combine(Environment.CurrentDirectory, "Dependencies/emmVRC.dll"));
                }
                HasCheckedUpdate = true;
            }
        }

        public static void LoaderCheck()
        {
            HasCheckedLoader = true;
        }

        public static void DownloadLib()
        {
            try
            {
                if (!Directory.Exists(Path.Combine(Environment.CurrentDirectory, "Dependencies")))
                    Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, "Dependencies"));
                WebRequest request = WebRequest.Create(WebsiteURL + "?libdownload");
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    Logger.LogError("[emmVRCLoader] Error occured while fetching emmVRC: " + response.StatusDescription);
                }
                else
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    string resultString = reader.ReadToEnd();
                    downloadedLib = Convert.FromBase64String(resultString);
                    File.WriteAllBytes(Path.Combine(Environment.CurrentDirectory, "Dependencies/emmVRC.dll"), downloadedLib);
                }

            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
            }
        }
        public static bool IsLibLatest()
        {
            if (Environment.CommandLine.Contains("--emmvrc.devmode")) return true;
            if (!File.Exists(Path.Combine(Environment.CurrentDirectory, "Dependencies/emmVRC.dll"))) return false;
            try
            {
                MD5 md5 = MD5.Create();
                FileStream libStream = File.OpenRead(Path.Combine(Environment.CurrentDirectory, "Dependencies/emmVRC.dll"));
                string md5hash = BitConverter.ToString(md5.ComputeHash(libStream)).Replace("-", "").ToLower();

                WebRequest request = WebRequest.Create(WebsiteURL + "?lib=" + md5hash);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    string resultString = reader.ReadToEnd();
                    if (resultString == "false")
                        return true;
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return false;
            }
            return false;
        }
        public static bool ShouldLoadLib()
        {
            if (!HasCheckedLoadLib)
            {
                try
                {
                    Logger.Log("[emmVRCLoader] Checking if emmVRC can Load...");

                    WebRequest request = WebRequest.Create(WebsiteURL + ("?shouldload"));
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        StreamReader reader = new StreamReader(response.GetResponseStream());
                        string resultString = reader.ReadToEnd();
                        if (resultString != "true")
                        {
                            Logger.LogError("[emmVRCLoader] emmVRC can't be loaded...");
                            LoadLibCheck = false;
                        }
                        else
                            Logger.Log("[emmVRCLoader] emmVRC can be loaded!");
                    }
                } catch (Exception ex)
                {
                    ex = new Exception();
                    Logger.LogError("emmVRC could not be accessed. Loading from disk, if possible...");
                }
            }
            return LoadLibCheck;
        }
    }
}
