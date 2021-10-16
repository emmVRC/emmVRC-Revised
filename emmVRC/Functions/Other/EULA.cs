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
using emmVRC.Objects.ModuleBases;

namespace emmVRC.Libraries
{
    public class EULA : MelonLoaderEvents
    {
        public static bool EULADownloaded = false;
        public static string EULAText = "Please read this End User License Agreement carefully before clicking the Agree button, or using emmVRC. By clicking the Agree button or using emmVRC, you are agreeing to be bound by the terms and conditions of this Agreement.\r\n\r\nLicense:\r\n\r\nIf you do not agree to the terms of this Agreement, do not click Agree and discontinue use of emmVRC.\r\n\r\nThe emmVRC Team grants you a revocable, non-exclusive, non-transferable, limited license to download, install and use emmVRC solely for your personal, non-commercial purposes strictly in accordance with this Agreement.\r\n\r\nRestrictions:\r\n\r\nYou agree not to, and you will not permit others to:\r\n\r\na) Sell or redistribute emmVRC as part of a pack, for monetary gain, or otherwise commercially exploit emmVRC.\r\n\r\nb) Modify or otherwise override emmVRC, or its limitations, internally or externally.\r\n\r\nc) Utilize emmVRC for malicious purposes, to include hacking, stealing, exploiting, or otherwise causing harm to other user(s).\r\n\r\nd) Abuse the emmVRC Network, either through emmVRC or through external programs.\r\n\r\ne) Claim emmVRC or the network as your own.\r\n\r\nModifications to emmVRC:\r\n\r\nThe emmVRC Team reserves the right to modify, suspend or discontinue, temporarily or permanently, emmVRC or the emmVRC Network, with or without notice and without liability to you.\r\n\r\nTerm and Termination:\r\n\r\nThis agreement shall remain in effect until terminated by you or The emmVRC Team.\r\n\r\nThe emmVRC Team may, in its sole discretion, at any time and for any or no reason, suspend or terminate this Agreement with or without prior notice.\r\n\r\nThis Agreement will terminate immediately, without prior notice from The emmVRC Team, in the event that you fail to comply with any provision of this Agreement. You may also terminate this Agreement by deleting emmVRC and all copies thereof from your computer.\r\n\r\nUpon termination of this Agreement, you shall cease all use of emmVRC and delete all copies of emmVRC from your computer.\r\n\r\nSeverability:\r\n\r\nIf any provision of this Agreement is held to be unenforceable or invalid, such provision will be changed and interpreted to accomplish the objectives of such provision to the greatest extent possible under applicable law, and the remaining provisions will continue in full force and effect.\r\n\r\nAmendments:\r\n\r\nThe emmVRC Team reserves the right to, at its sole discretion, modify or replace this Agreement at any time. If a revision is material, we will provide at least 7 days' notice prior to any new terms taking effect. What constitutes a material change will be determined at our sole discretion.\r\n\r\nContact Information:\r\n\r\nIf you have any questions about this Agreement, please contact us:\r\nlegal@emmvrc.com\r\n\r\nUpdated 01/19/2021";
        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            if (buildIndex != -1) return;

            MelonLoader.MelonCoroutines.Start(EULA.Initialize());
        }

        public static IEnumerator Initialize()
        {
            while (!EULADownloaded)
            {
                if (Configuration.JSONConfig.AcceptedEULAVersion != Attributes.EULAVersion)
                {
                    File.WriteAllLines(Path.Combine(Environment.CurrentDirectory, "UserData/emmVRC/eula.txt"), new string[] { EULAText });
                    yield return new WaitForSeconds(!Environment.CurrentDirectory.Contains("vrchat-vrchat") ? 10f : 20f); // Yet another of yee old crusty Oculus checks.
                    VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowStandardPopup("emmVRC", Configuration.JSONConfig.AcceptedEULAVersion == "0.0.0" ? "Please read the EULA for emmVRC.\nThis will open on your desktop." : "Please read the updated EULA\nfor emmVRC.",
                        "Open EULA", () => { 
                            System.Diagnostics.Process.Start(Path.Combine(Environment.CurrentDirectory, "UserData/emmVRC/eula.txt"));
                        },
                        "Agree", () => {
                            Configuration.WriteConfigOption("AcceptedEULAVersion", Attributes.EULAVersion);
                            VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup(); });
                    EULADownloaded = true;
                }
                else EULADownloaded = true;
            }
        }
    }
}
