using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Security.Cryptography;

namespace emmVRCLoader
{
    public static class UpdateManager
    {
        private const string _webUrl = "https://prod-dl.emmvrc.com";

        private static string CachedVersionPath => Path.Combine(Environment.CurrentDirectory, "Dependencies/emmVRC.dll");
        private static string CachedParanoidVersionPath => Path.Combine(Environment.CurrentDirectory, "Dependencies/emmVRC.new.dll");

        public static byte[] Check()
        {
            if (!IsLibLatest())
            {
                Logger.Log("Downloading emmVRC...");
                return DownloadLib();
            }
            
            return File.ReadAllBytes(CachedVersionPath);
        }

        public static byte[] DownloadLib()
        {
            try
            {
                if (!Directory.Exists("Dependencies"))
                    Directory.CreateDirectory("Dependencies");

                var fileHash = HashUtil.GetSHA256(CachedVersionPath);
                var webRequest = (HttpWebRequest)WebRequest.Create(_webUrl + $"/mod/{fileHash}");
                var webResponse = (HttpWebResponse)webRequest.GetResponse();

                if (webResponse.StatusCode == HttpStatusCode.OK)
                {
                    using (var stream = webResponse.GetResponseStream())
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            var bufferSize = 4096;
                            int readBytes;
                            
                            var readBuffer = new byte[bufferSize];
                            
                            while ((readBytes = stream.Read(readBuffer, 0, bufferSize)) > 0)
                            {
                                memoryStream.Write(readBuffer, 0, readBytes);
                            }
                            
                            var data = memoryStream.ToArray();
                            
                            if (!emmVRCLoaderMod.isParanoidMode)
                            {
                                File.WriteAllBytes(CachedVersionPath, data);
                            }
                            else
                            {
                                File.WriteAllBytes(CachedParanoidVersionPath, data);

                                ParanoidMode();

                                File.Delete(CachedVersionPath);
                                File.Move(CachedParanoidVersionPath, CachedVersionPath);
                            }

                            return data;
                        }
                    }
                }

                if (webResponse.StatusCode == HttpStatusCode.NoContent)
                {
                    Logger.Log("emmVRC is up to date.");
                    return File.ReadAllBytes(CachedVersionPath);
                }
                
                Logger.LogError($"An unknown error occured while fetching emmVRC: {webResponse.StatusDescription}");
                return null;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return null;
            }
        }

        public static bool IsLibLatest()
        {
            #if DEBUG
            if (Environment.CommandLine.Contains("--emmvrc.devmode"))
                return true;
            #endif

            if (emmVRCLoaderMod.isParanoidMode)
            {
                if (!File.Exists(CachedVersionPath))
                    return false;
                
                if (!File.Exists(CachedParanoidVersionPath))
                    return false;
                
                var paranoidDllHash = HashUtil.GetSHA256(CachedParanoidVersionPath);
                var currentDllHash = HashUtil.GetSHA256(CachedVersionPath);

                if (paranoidDllHash == currentDllHash)
                {
                    File.Delete(CachedParanoidVersionPath);
                    return true;
                }
                
                if (File.Exists(CachedParanoidVersionPath))
                {
                    File.Delete(CachedVersionPath);
                    File.Move(CachedParanoidVersionPath, CachedVersionPath);
                }
            }
            
            return false;
        }

        public static bool ShouldLoadLib()
        {
            try
            {
                Logger.Log("Checking if emmVRC can Load...");

                var webRequest = (HttpWebRequest)WebRequest.Create(_webUrl + "/should_load");
                var webResponse = (HttpWebResponse)webRequest.GetResponse();

                if (webResponse.StatusCode != HttpStatusCode.OK)
                {
                    Logger.LogError("emmVRC can't be loaded...");
                    return false;
                }

                Logger.Log("emmVRC can be loaded!");
            }
            catch (Exception)
            {
                Logger.LogError("emmVRC could not be accessed. Loading from disk, if possible...");
            }

            return true;
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