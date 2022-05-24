using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using emmVRC.Objects;
using emmVRC.Objects.ModuleBases;
using emmVRC.Utils;

namespace emmVRC.Components.CompatibilityCheckers
{
    public class MelonLoaderVersion : CompatibilityCheck
    {
        public override bool RunCheck()
        {
            string currentVersion = (string)typeof(MelonLoader.BuildInfo).GetField("Version").GetValue(null);
            if (new Version(currentVersion) < Attributes.MinimumMelonLoaderVersion)
            {
                emmVRCLoader.Logger.LogError("You are using an incompatible version of MelonLoader: v" + currentVersion + ". Please install v" + Attributes.MinimumMelonLoaderVersion.ToString(3) + " or newer, via the instructions in our Discord under the #how-to channel. emmVRC will not start.");
                MessageBox.Show(IntPtr.Zero, "You are using an incompatible version of MelonLoader: v" + currentVersion + ". Please install v" + Attributes.MinimumMelonLoaderVersion.ToString(3) + " or newer, via the instructions in our Discord under the #how-to channel. emmVRC will not start.", "emmVRC", 0x00000010u);
                return false;
            }
            return true;
        }
    }
}
