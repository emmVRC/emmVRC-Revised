using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Security.Cryptography;

namespace emmVRCLoader
{
    public static class UpdateManager
    {
        private const string WebsiteURL = "https://dl.emmvrc.com/update.php";

        private static string CachedVersionPath => Path.Combine(Environment.CurrentDirectory, "Dependencies/emmVRC.dll");
        private static string CachedParanoidVersionPath => Path.Combine(Environment.CurrentDirectory, "Dependencies/emmVRC.new.dll");

        public static byte[] Check()
        {
            if (!IsLibLatest())
            {
                Logger.Log("Downloading emmVRC...");
                return DownloadLib();
            }
            else
            {
                return File.ReadAllBytes(CachedVersionPath);
            }
        }

        public static byte[] DownloadLib()
        {
            try
            {
                if (!Directory.Exists(Path.Combine(Environment.CurrentDirectory, "Dependencies")))
                    Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, "Dependencies"));
                WebRequest request = WebRequest.Create(WebsiteURL + "?libdownload");
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    Logger.LogError("Error occured while fetching emmVRC: " + response.StatusDescription);
                    return null;
                }
                else
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    string resultString = reader.ReadToEnd();
                    byte[] assembly = Convert.FromBase64String(resultString);

                    if (!emmVRCLoaderMod.isParanoidMode)
                    {
                        File.WriteAllBytes(CachedVersionPath, assembly);
                    }
                    else
                    {
                        File.WriteAllBytes(CachedParanoidVersionPath, assembly);

                        ParanoidMode();

                        File.Delete(CachedVersionPath);
                        File.Move(CachedParanoidVersionPath, CachedVersionPath);
                    }
                    return assembly;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return null;
            }
        }

        public static bool IsLibLatest()
        {
            if (Environment.CommandLine.Contains("--emmvrc.devmode"))
            {
                Logger.LogWarning("You have emmVRC's dev mode on, which is well, only meant for devs!");
                Logger.LogWarning("Unless you understand what is does,");
                Logger.LogWarning("I would recommend you turn it off, as it prevents updates.");
                Logger.LogWarning("To turn it off, simply remove the \"--emmvrc.devmode\" launch option.");
                return true;
            }

            if (emmVRCLoaderMod.isParanoidMode)
            {
                try
                {
                    // If the current emmVRC.dll is alr newest, then delete the emmVRC.new.dll and continue
                    if (CheckHashOfFile(CachedVersionPath))
                    {
                        File.Delete(CachedParanoidVersionPath);
                        return true;
                    }

                    // If not, then replace the emmVRC.dll with emmVRC.new.dll and check the hash
                    if (File.Exists(CachedParanoidVersionPath))
                    {
                        File.Delete(CachedVersionPath);
                        File.Move(CachedParanoidVersionPath, CachedVersionPath);
                    }

                    return CheckHashOfFile(CachedVersionPath);
                }
                catch (Exception ex)
                {
                    ParanoidMode();
                    throw ex;
                }
            }
            else
            {
                if (File.Exists(CachedParanoidVersionPath))
                    File.Delete(CachedParanoidVersionPath);

                return CheckHashOfFile(CachedVersionPath);
            }
        }

        private static bool CheckHashOfFile(string filePath)
        {
            if (!File.Exists(filePath))
                return false;

            try
            {
                MD5 md5 = MD5.Create();
                string md5hash;
                using (FileStream libStream = File.OpenRead(filePath))
                    md5hash = BitConverter.ToString(md5.ComputeHash(libStream)).Replace("-", "").ToLower();
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
            catch
            {
                return true;
            }
            return false;
        }

        public static bool ShouldLoadLib()
        {
            bool loadLibCheck = true;
            try
            {
                Logger.Log("Checking if emmVRC can Load...");

                WebRequest request = WebRequest.Create(WebsiteURL + ("?shouldload"));
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    string resultString = reader.ReadToEnd();
                    if (resultString != "true")
                    {
                        Logger.LogError("emmVRC can't be loaded...");
                        loadLibCheck = false;
                    }
                    else
                        Logger.Log("emmVRC can be loaded!");
                }
            }
            catch
            {
                Logger.LogError("emmVRC could not be accessed. Loading from disk, if possible...");
            }

            return loadLibCheck;
        }

        private static void ParanoidMode()
        {
            Logger.LogWarning("-----------------------------------------------------------------------------");
            Logger.LogWarning("Paranoid mode is on!");
            Logger.LogWarning("emmVRC has auto-updated. If you would like to inspect the new file,");
            Logger.LogWarning("it is located in the \"Dependencies\" folder with the name \"emmVRC.new.dll\".");
            Logger.LogWarning("Please enter \"continue\" to continue with the new update");
            Logger.LogWarning("or you may enter anything else to close the game.");
            Logger.LogWarning("To disable paranoid mode, please remove the \"--emmvrc.paranoid\" launch option.");
            Logger.LogWarning("-----------------------------------------------------------------------------");

            if (Console.ReadLine() != "continue")
                Process.GetCurrentProcess().Kill();
        }
    }
}