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

namespace emmVRC.Functions.Other
{
    public class DestructiveActions
    {
        public static void ForceQuit()
        {
            NetworkClient.Logout();
            Thread quitThread = new Thread(QuitAfterQuit)
            {
                IsBackground = true,
                Name = "emmVRC Quit Thread"
            };
            quitThread.Start();
        }
        public static void ForceRestart()
        {
            if (NetworkClient.webToken != null)
                HTTPRequest.get(NetworkClient.baseURL + "/api/authentication/logout").NoAwait("Logout");
            Thread restartThread = new Thread(RestartAfterQuit)
            {
                IsBackground = true,
                Name = "emmVRC Restart Thread"
            };
            restartThread.Start();
        }
        public static void RestartAfterQuit()
        {
            Application.Quit();
            Thread.Sleep(2500);
            try { Process.Start(@Environment.CurrentDirectory + "\\VRChat.exe", Environment.CommandLine.ToString()); } catch (Exception ex) { ex = new Exception(); }
            Process.GetCurrentProcess().Kill();
        }
        public static void QuitAfterQuit()
        {
            Application.Quit();
            Thread.Sleep(2500);
            Process.GetCurrentProcess().Kill();
        }
    }
}
