using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using emmVRC.Network;

#pragma warning disable 4014

namespace emmVRC
{
    public class DestructiveActions
    {
        public static void ForceQuit()
        {
            if (NetworkClient.authToken != null)
                HTTPRequest.get(NetworkClient.baseURL + "/api/authentication/logout");
            Process.GetCurrentProcess().Kill();
        }
        public static void ForceRestart()
        {
            if (NetworkClient.authToken != null)
                HTTPRequest.get(NetworkClient.baseURL + "/api/authentication/logout");
                try { Process.Start(@Environment.CurrentDirectory + "\\VRChat.exe", Environment.CommandLine.ToString()); } catch (Exception ex) { ex = new Exception(); }
            Process.GetCurrentProcess().Kill();
        }
    }
}
