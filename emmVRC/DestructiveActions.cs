using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace emmVRC
{
    public class DestructiveActions
    {
        public static void ForceQuit()
        {
            Process.GetCurrentProcess().Kill();
        }
        public static void ForceRestart()
        {
            try { Process.Start(@Environment.CurrentDirectory + "\\VRChat.exe", Environment.CommandLine.ToString()); } catch (Exception ex) { ex = new Exception(); }
            Process.GetCurrentProcess().Kill();
        }
    }
}
