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
        private static string WebsiteURL = "https://thetrueyoshifan.com/BakaUpdate.php";
        public static bool LoaderNeedsUpdate = false;
        public static bool HasCheckedLoader = false;
        public static bool HasCheckedUpdate = false;
        public static bool HasCheckedLoadLib = false;
        public static bool LoadLibCheck = true;
        private static WebRequest WebRequest = null;
        private static int ResponseCode = 0;
        internal static Byte[] downloadedLib;

        public static void Check()
        {
            if (!HasCheckedUpdate)
            {
                Logger.Log("[emmVRCLoader] Downloading emmVRC...");
                DownloadLib();
                HasCheckedUpdate = true;
            }
        }

        public static void LoaderCheck()
        {
            HasCheckedLoader = true;
        }

        public static void DownloadLib()
        {
            if (!Environment.CommandLine.Contains("--emmvrc.devmode"))
            {
                try
                {
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
                    }

                } catch (Exception ex)
                {
                    Logger.LogError(ex.ToString());
                }

            }
            else if (Environment.CommandLine.Contains("--emmvrc.devmode"))
            {
                downloadedLib = File.ReadAllBytes(Path.Combine(Environment.CurrentDirectory, "Dependencies/emmVRC.dll"));
            }
        }
        public static bool ShouldLoadLib()
        {
            if (!HasCheckedLoadLib)
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
            }
            return LoadLibCheck;
        }
    }
}
