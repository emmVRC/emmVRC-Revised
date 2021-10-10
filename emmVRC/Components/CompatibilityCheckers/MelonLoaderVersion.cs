using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using emmVRC.Objects;
using emmVRC.Objects.ModuleBases;

namespace emmVRC.Components.CompatibilityCheckers
{
    public class MelonLoaderVersion : CompatibilityCheck
    {
        public override bool RunCheck()
        {
            string currentVersion = (string)typeof(MelonLoader.BuildInfo).GetField("Version").GetValue(null);
            if (Attributes.IncompatibleMelonLoaderVersions.Contains(currentVersion))
            {
                emmVRCLoader.Logger.LogError("You are using an incompatible version of MelonLoader: v" + currentVersion + ". Please install v" + Attributes.TargetMelonLoaderVersion + " or newer, via the instructions in our Discord under the #how-to channel. emmVRC will not start.");
                System.Windows.Forms.MessageBox.Show("You are using an incompatible version of MelonLoader: v" + currentVersion + ". Please install v" + Attributes.TargetMelonLoaderVersion + " or newer, via the instructions in our Discord under the #how-to channel. emmVRC will not start.", "emmVRC", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return false;
            }
            return true;
        }
    }
}
