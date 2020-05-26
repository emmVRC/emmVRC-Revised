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

namespace emmVRC.Hacks
{
    public class UIElementsMenu
    {
        private static QMToggleButton ToggleNameplates;
        private static QMToggleButton ToggleHUD;
        public static IEnumerator Initialize()
        {
            ToggleHUD = new QMToggleButton("UIElementsMenu", 1, 0, "HUD On", () =>
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
            ToggleNameplates = new QMToggleButton("UIElementsMenu", 2, 0, "Nameplates\nOn", delegate ()
            {
                Configuration.JSONConfig.NameplatesVisible = true;
                Configuration.SaveConfig();
                QuickMenuUtils.GetQuickMenuInstance().transform.Find("UIElementsMenu/ToggleNameplatesButton").GetComponent<Button>().onClick.Invoke();
            }, "Nameplates\nOff", delegate ()
            {
                Configuration.JSONConfig.NameplatesVisible = false;
                Configuration.SaveConfig();
                QuickMenuUtils.GetQuickMenuInstance().transform.Find("UIElementsMenu/ToggleNameplatesButton").GetComponent<Button>().onClick.Invoke();
            }, "TOGGLE: Select to Turn the Nameplates On/Off");
            ToggleHUD.setToggleState(Configuration.JSONConfig.UIVisible);
            ToggleNameplates.setToggleState(Configuration.JSONConfig.NameplatesVisible);
            while (RoomManager.field_Internal_Static_ApiWorld_0 == null || VRCPlayer.field_Internal_Static_VRCPlayer_0 == null)
                yield return new UnityEngine.WaitForSeconds(0.1f);
            UnhollowerRuntimeLib.DelegateSupport.ConvertDelegate<Il2CppSystem.Action>((System.Action)(() =>
            {
                if (!Configuration.JSONConfig.UIVisible)
                    QuickMenuUtils.GetQuickMenuInstance().transform.Find("UIElementsMenu/ToggleHUDButton").GetComponent<Button>().onClick.Invoke();
                if (!Configuration.JSONConfig.NameplatesVisible)
                    QuickMenuUtils.GetQuickMenuInstance().transform.Find("UIElementsMenu/ToggleNameplatesButton").GetComponent<Button>().onClick.Invoke();
            })).Invoke();

            QuickMenuUtils.GetQuickMenuInstance().transform.Find("UIElementsMenu/ToggleNameplatesButton").gameObject.SetActive(false);
            QuickMenuUtils.GetQuickMenuInstance().transform.Find("UIElementsMenu/ToggleHUDButton").gameObject.SetActive(false);
            yield return null;
        }
        public static IEnumerator OnSceneLoaded()
        {
            while (RoomManager.field_Internal_Static_ApiWorld_0 == null || VRCPlayer.field_Internal_Static_VRCPlayer_0 == null)
                yield return new UnityEngine.WaitForSeconds(0.1f);
            if (!Configuration.JSONConfig.NameplatesVisible && VRCPlayer.Method_Public_Static_Boolean_0())
                UnhollowerRuntimeLib.DelegateSupport.ConvertDelegate<Il2CppSystem.Action>((System.Action)(() =>
                {
                    QuickMenuUtils.GetQuickMenuInstance().transform.Find("UIElementsMenu/ToggleNameplatesButton").GetComponent<Button>().onClick.Invoke();
                })).Invoke();
            else if (Configuration.JSONConfig.NameplatesVisible && !VRCPlayer.Method_Public_Static_Boolean_0())
                UnhollowerRuntimeLib.DelegateSupport.ConvertDelegate<Il2CppSystem.Action>((System.Action)(() =>
                {
                    QuickMenuUtils.GetQuickMenuInstance().transform.Find("UIElementsMenu/ToggleNameplatesButton").GetComponent<Button>().onClick.Invoke();
                })).Invoke();
        }
    }
}
