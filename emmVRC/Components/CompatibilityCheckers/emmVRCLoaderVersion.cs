using System;
using System.IO;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;
using emmVRC.Objects.ModuleBases;
using emmVRC.Objects;

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
                    MessageBox.Show("The newest emmVRCLoader has been downloaded to your Mods folder. To use emmVRC, restart your game. If the problem persists, remove any current emmVRCLoader files, and download the latest from #loader-updates in the emmVRC Discord.", "emmVRC", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
                catch (Exception ex)
                {
                    emmVRCLoader.Logger.LogError("Attempt to download the new loader failed. You must download the latest from https://dl.emmvrc.com/downloads/emmVRCLoader.dll manually.");
                    emmVRCLoader.Logger.LogError("Error: " + ex.ToString());
                    System.Windows.Forms.MessageBox.Show("You are using an incompatible version of emmVRCLoader: v" + currentEmmVRCLoaderVersion + ". Please install v" + Attributes.MinimumemmVRCLoaderVersion.ToString(3) + " or greater, from the #loader-updates channel in the emmVRC Discord.", "emmVRC", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return false;
                }
            }
            return true;
        }
    }
}
