using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using emmVRC.Libraries;
using UnityEngine.UI;
using UnityEngine.Events;
using VRC;
using emmVRC.Objects.ModuleBases;

namespace emmVRC.Functions.UI
{
    public class UIElementsMenu : MelonLoaderEvents
    {
        public static QMToggleButton ToggleHUD;
        private bool initialized = false;
        public override void OnUiManagerInit()
        {
            ToggleHUD = new QMToggleButton("UIElementsMenu", 1, 2, "HUD On", () =>
            {
                Configuration.JSONConfig.UIVisible = true;
                Configuration.SaveConfig();
                QuickMenuUtils.GetQuickMenuInstance().transform.Find("UIElementsMenu/ToggleHUDButton").GetComponent<Button>().onClick.Invoke();
            }, "HUD Off", () =>
            {
                Configuration.JSONConfig.UIVisible = false;
                Configuration.SaveConfig();
                QuickMenuUtils.GetQuickMenuInstance().transform.Find("UIElementsMenu/ToggleHUDButton").GetComponent<Button>().onClick.Invoke();
            }, "TOGGLE: Select to Turn the HUD On/Off");

            ToggleHUD.setToggleState(Configuration.JSONConfig.UIVisible);

            QuickMenuUtils.GetQuickMenuInstance().transform.Find("UIElementsMenu/ToggleHUDButton").gameObject.SetActive(false);
        }
        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            if (buildIndex == -1 && !initialized)
            {
                if (!Configuration.JSONConfig.UIVisible)
                    try
                    {
                        QuickMenuUtils.GetQuickMenuInstance().transform.Find("UIElementsMenu/ToggleHUDButton").GetComponent<Button>().onClick.Invoke();
                    }
                    catch {};
                initialized = true;
            }
        }
            
        
    }
}