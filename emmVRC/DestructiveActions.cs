using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using emmVRC.Network;
using emmVRC.Managers;
using emmVRC.Objects;
using UnityEngine;
using System.Collections;
using System.Threading;

#pragma warning disable 4014

namespace emmVRC
{
    public class DestructiveActions
    {
        public static void ForceQuit()
        {
            if (NetworkClient.authToken != null)
                HTTPRequest.get(NetworkClient.baseURL + "/api/authentication/logout");
            if (Attributes.Debug)
            {
                Thread quitThread = new Thread(QuitAfterQuit)
                {
                    IsBackground = true,
                    Name = "emmVRC Quit Thread"
                };
                quitThread.Start();
            }
        }
        public static void ForceRestart()
        {
            if (NetworkClient.authToken != null)
                HTTPRequest.get(NetworkClient.baseURL + "/api/authentication/logout");
            if (Attributes.Debug)
            {
                Thread restartThread = new Thread(RestartAfterQuit)
                {
                    IsBackground = true,
                    Name = "emmVRC Restart Thread"
                };
                restartThread.Start();
            }
        }
        public static void RestartAfterQuit()
        {
            Application.Quit();
            Thread.Sleep(1000);
            try { Process.Start(@Environment.CurrentDirectory + "\\VRChat.exe", Environment.CommandLine.ToString()); } catch (Exception ex) { ex = new Exception(); }
            Process.GetCurrentProcess().Kill();
        }
        public static void QuitAfterQuit()
        {
            Application.Quit();
            Thread.Sleep(1000);
            Process.GetCurrentProcess().Kill();
        }
    }
}
