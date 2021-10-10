using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using emmVRC.Libraries;
using emmVRC.Objects.ModuleBases;


namespace emmVRC.Menus
{
    public class ReportWorldMenu : MelonLoaderEvents
    {
        public static QMNestedButton baseMenu;
        public static QMSingleButton ReportWorldButton;
        public static GameObject RealReportWorldButton;
        public static UnityEngine.UI.Button.ButtonClickedEvent baseReportWorldEvent;
        public override void OnUiManagerInit()
        {
            if (!Configuration.JSONConfig.StealthMode) return;
            baseMenu = new QMNestedButton("ShortcutMenu", 5, 1, "Report\nWorld", "Report Issues with this World");
            ReportWorldButton = new QMSingleButton(baseMenu, 5, 1, "Report\nWorld", () => { RealReportWorldButton.GetComponentInChildren<Button>(true).onClick.Invoke(); }, "Report Issues with this World (this is the real button)");
            RealReportWorldButton = QuickMenuUtils.GetQuickMenuInstance().transform.Find("ShortcutMenu/ReportWorldButton").gameObject;
            RealReportWorldButton.SetActive(false);
            if (Configuration.JSONConfig.StealthMode)
            {
                emmVRCLoader.Logger.Log("You have emmVRC's Stealth Mode enabled. To access the functions menu, press the \"Report World\" button. Most visual functions of emmVRC have been disabled.");
                if (Environment.CommandLine.Contains("--emmvrc.stealthmode"))
                    emmVRCLoader.Logger.Log("To disable stealth mode, remove '--emmvrc.stealthmode' from your launch options. Then, disable \"Stealth Mode\" in the emmVRC Settings, and restart the game.");
                else
                    emmVRCLoader.Logger.Log("To disable stealth mode, disable \"Stealth Mode\" in the emmVRC Settings, and restart the game.");
            }
        }
    }
}
