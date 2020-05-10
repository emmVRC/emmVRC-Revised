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
        private static string WebsiteURL
        {
            get
            {
                byte[] input = Convert.FromBase64String("aHR0cDovL3RoZXRydWV5b3NoaWZhbi5jb20vZW1tVlJDVXBkYXRlLnBocA==");
                return Encoding.ASCII.GetString(input);
            }
        }
        public static bool LoaderNeedsUpdate = false;
        public static bool HasCheckedLoader = false;
        public static bool HasCheckedUpdate = false;
        public static bool HasCheckedLoadLib = false;
        public static bool LoadLibCheck = true;
        private static WebRequest WebRequest = null;
        private static int ResponseCode = 0;
        private static bool IsBeta = false;
        internal static Byte[] downloadedLib;

        public static void Check()
        {
            if (!HasCheckedUpdate)
            {
                string configpath = null;
                configpath = Path.Combine(Environment.CurrentDirectory, "UserData/emmVRC/config.json");
                /*if (File.Exists(configpath))
                   IsBeta = ((JObject)JsonConvert.DeserializeObject(File.ReadAllText(configpath)))["openBeta"].Value<bool>();*/
                   if (File.Exists(configpath))
                {
                    IsBeta = TinyJSON.Decoder.Decode(File.ReadAllText(configpath))["OpenBetaEnabled"];
                }

                Logger.Log("[emmVRCLoader] Downloading emmVRC...");
                DownloadLib();
                HasCheckedUpdate = true;
            }
        }

        public static void LoaderCheck()
        {
            /*if (!HasCheckedLoader)
            {
                string FileHash = null;
                using (var md5 = MD5.Create())
                {
                    string LoaderPath = null;
                        LoaderPath = Directory.GetCurrentDirectory() + "emmVRCLoader.dll";

                    using (var stream = File.OpenRead(LoaderPath))
                    {
                        var hash = md5.ComputeHash(stream);
                        FileHash = BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                    }
                }
                if (FileHash != null)
                {
                    WebRequest = HttpWebRequest.Create(WebsiteURL + "?loader=" + FileHash);
                    HttpWebResponse response = WebRequest.GetResponse();
                    while (response.ContentLength <= 0) ;
                    if ((response.StatusCode == HttpStatusCode.OK) && response. .Equals("true"))
                    {
                        Logger.Log("[emmVRCLoader] emmVRCLoader needs to update!");
                        LoaderNeedsUpdate = true;
                    }
                    else
                        Logger.Log("[emmVRCLoader] emmVRCLoader is fully up-to-date.");
                }*/
            HasCheckedLoader = true;
            //}
        }

        public static void DownloadLib()
        {
            if (!Environment.CommandLine.Contains("--emmvrc.devmode"))
            {
                try
                {
                    /*WebClient client = new WebClient();
                    client.DownloadData(new Uri(WebsiteURL + (IsBeta ? "?beta&libdownload" : "?libdownload")));
                    client.DownloadDataCompleted += delegate (object sender, DownloadDataCompletedEventArgs e)
                    {
                        if (e.Error != null || e.Cancelled)
                        {
                            Logger.LogError("[emmVRCLoader] Error occured while fetching lib: " + (e.Error != null ? e.Error.ToString() : "download cancelled..."));
                        }
                        else
                        {
                            string resultString = Encoding.Default.GetString(e.Result);
                            downloadedLib = Convert.FromBase64String(resultString);
                        }
                    };*/
                    WebRequest request = WebRequest.Create(WebsiteURL + (IsBeta ? "?beta&libdownload" : "?libdownload"));
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

                WebRequest request = WebRequest.Create(WebsiteURL + (IsBeta ? "?beta&shouldload" : "?shouldload"));
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
                /*WebRequest = new WWW(WebsiteURL + (IsBeta ? "?beta&shouldload" : "?shouldload"));
                while (!WebRequest.isDone) ;
                ResponseCode = Bootstrapper.GetWWWResponseCode(WebRequest);
                if ((ResponseCode == 200) && !WebRequest.text.Equals("true"))
                {
                    Logger.LogError("[emmVRCLoader] emmVRC can't be loaded...");
                    LoadLibCheck = false;
                }
                else
                    Logger.Log("[emmVRCLoader] emmVRC can be loaded!");
                HasCheckedLoadLib = true;*/
            }
            return LoadLibCheck;
        }
    }
}
