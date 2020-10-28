using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using emmVRC.Libraries;


namespace emmVRC.Menus
{
    public class ReportWorldMenu
    {
        public static QMNestedButton baseMenu;
        public static QMSingleButton ReportWorldButton;
        public static GameObject RealReportWorldButton;
        public static UnityEngine.UI.Button.ButtonClickedEvent baseReportWorldEvent;
        public static void Initialize()
        {
            baseMenu = new QMNestedButton("ShortcutMenu", 5, 1, "Report\nWorld", "Report Issues with this World");
            ReportWorldButton = new QMSingleButton(baseMenu, 5, 1, "Report\nWorld", () => { RealReportWorldButton.GetComponentInChildren<Button>(true).onClick.Invoke(); }, "Report Issues with this World (this is the real button)");
            RealReportWorldButton = QuickMenuUtils.GetQuickMenuInstance().transform.Find("ShortcutMenu/ReportWorldButton").gameObject;
            RealReportWorldButton.SetActive(false);
            
        }
    }
}
