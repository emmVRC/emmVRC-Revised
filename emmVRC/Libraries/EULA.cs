using emmVRC.Network;
using emmVRC.Objects;
using System.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace emmVRC.Libraries
{
    public class EULA
    {
        public static bool EULADownloaded = false;
        public static IEnumerator Initialize()
        {
            while (!EULADownloaded)
            {
                if (Configuration.JSONConfig.AcceptedEULAVersion != Attributes.EULAVersion)
                {
                    emmVRCLoader.Logger.Log("Downloading EULA...");
                    while (RoomManager.field_Internal_Static_ApiWorld_0 == null || RoomManager.field_Internal_Static_ApiWorldInstance_0 == null || VRCPlayer.field_Internal_Static_VRCPlayer_0 == null)
                        yield return new WaitForSeconds(1f);
                    var eulaDownload = HTTPRequest.get("https://www.thetrueyoshifan.com/downloads/emmvrcresources/eula.txt");
                    while (!eulaDownload.IsCompleted && !eulaDownload.IsFaulted)
                        yield return new WaitForSeconds(1f);
                    if (eulaDownload.IsFaulted || (eulaDownload.IsCompleted && eulaDownload.Result.ToLower().Contains("nullreferenceexception")))
                    {
                        emmVRCLoader.Logger.LogError("EULA could not be downloaded.");
                        yield return new WaitForSeconds(2.5f);
                    }
                    else if (eulaDownload.IsCompleted)
                    {
                        File.WriteAllLines(Path.Combine(Environment.CurrentDirectory, "UserData/emmVRC/eula.txt"), new string[] { eulaDownload.Result });
                        yield return new WaitForSeconds(2.5f);
                        VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowStandardPopup("emmVRC", "Please read the EULA for emmVRC.\nThis will open on your desktop.", "Open EULA", () => { System.Diagnostics.Process.Start(Path.Combine(Environment.CurrentDirectory, "UserData/emmVRC/eula.txt")); }, "Agree", () => { Configuration.JSONConfig.AcceptedEULAVersion = Attributes.EULAVersion; Configuration.SaveConfig(); VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup(); });
                        EULADownloaded = true;
                    }
                }
                else EULADownloaded = true;
            }
        }
    }
}
