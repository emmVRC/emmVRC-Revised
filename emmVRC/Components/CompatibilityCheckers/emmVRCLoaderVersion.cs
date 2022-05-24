using System;
using System.IO;
using System.Net;
using emmVRC.Objects.ModuleBases;
using emmVRC.Objects;
using emmVRC.Utils;

namespace emmVRC.Components.CompatibilityCheckers
{
    public class emmVRCLoaderVersion : CompatibilityCheck
    {
        public override bool RunCheck()
        {
            string currentEmmVRCLoaderVersion = (string)typeof(emmVRCLoader.BuildInfo).GetField("Version").GetValue(null);

            if (new Version(currentEmmVRCLoaderVersion) < Attributes.MinimumemmVRCLoaderVersion)
            {
                try
                {
                    WebClient cli = new WebClient();
                    string dest = "";
                    foreach (string modFile in System.IO.Directory.GetFiles(Path.Combine(Environment.CurrentDirectory, "Mods")))
                    {
                        if (modFile.Contains("emmVRC"))
                        {
                            dest = modFile;
                            System.IO.File.Delete(modFile);
                        }
                    }
                    cli.DownloadFile("https://dl.emmvrc.com/downloads/emmVRCLoader.dll", dest != "" ? dest : Path.Combine(Environment.CurrentDirectory, "Mods/emmVRCLoader.dll"));
                    MessageBox.Show(IntPtr.Zero, "The newest emmVRCLoader has been downloaded to your Mods folder. To use emmVRC, restart your game. If the problem persists, remove any current emmVRCLoader files, and download the latest from #loader-updates in the emmVRC Discord.", "emmVRC", 0x0u);
                    return false;
                }
                catch (Exception ex)
                {
                    emmVRCLoader.Logger.LogError("Attempt to download the new loader failed. You must download the latest from https://dl.emmvrc.com/downloads/emmVRCLoader.dll manually.");
                    emmVRCLoader.Logger.LogError("Error: " + ex);
                    MessageBox.Show(IntPtr.Zero, "You are using an incompatible version of emmVRCLoader: v" + currentEmmVRCLoaderVersion + ". Please install v" + Attributes.MinimumemmVRCLoaderVersion.ToString(3) + " or greater, from the #loader-updates channel in the emmVRC Discord.", "emmVRC", 0x00000010u);
                    return false;
                }
            }
            return true;
        }
    }
}
