using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using emmVRC.Utils;
using emmVRC.Objects.ModuleBases;
using TMPro;

namespace emmVRC.Functions.UI
{
    [Priority(55)]
    public class DashboardBar : MelonLoaderEvents, IWithFixedUpdate
    {
        private static bool _initialized = false;
        private static TMPro.TextMeshProUGUI textBase;
        private static bool amPm;

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            if (buildIndex != -1 || _initialized) return;
            Transform menuBase = Resources.FindObjectsOfTypeAll<VRC.UI.Elements.Menus.LaunchPadQMMenu>().FirstOrDefault().gameObject.transform.Find("ScrollRect/Viewport/VerticalLayoutGroup");
            GameObject infoBarBase = GameObject.Instantiate(menuBase.transform.Find("Header_QuickActions").gameObject, menuBase);
            infoBarBase.name = "emmVRC_InfoBar";
            textBase = infoBarBase.GetComponentInChildren<TMPro.TextMeshProUGUI>(true);
            textBase.GetComponent<RectTransform>().sizeDelta = new Vector2(1600f, 50f);
            textBase.fontSize = 30;

            // Determine if time should be AM or PM, based on the system registry. There is no better way to do this.
            amPm = Microsoft.Win32.Registry.GetValue(@"HKEY_CURRENT_USER\Control Panel\International", "sShortTime", "x").ToString().Contains("h");



            _initialized = true;
        }
        public void OnFixedUpdate()
        {
            if (!_initialized) return;
            if (!Configuration.JSONConfig.InfoBarDisplayEnabled && !Configuration.JSONConfig.ClockEnabled) return;

            // Build a time string based on the current time
            var timeString = DateTime.Now.ToString(amPm ? "hh:mm tt" : "HH:mm");

            // Set up the instance timer string to be updated later
            string instanceTimeString = TimeSpan.FromSeconds(RoomManager.prop_Single_0).ToString("hh':'mm':'ss");

            textBase.text = "<color=white>" + (Configuration.JSONConfig.ClockEnabled ? timeString + " ("+instanceTimeString+")" : "")+"</color>";
        }
    }
}
