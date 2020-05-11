using Il2CppSystem;
using Il2CppSystem.Collections.Generic;
using Il2CppSystem.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace emmVRC.Libraries
{
    public class MainMenuUtils
    {
        public static void ShowEmptyMenu()
        {
            QuickMenuUtils.GetQuickMenuInstance().Method_Public_Void_Int32_0(4);
        }
        public static void BuildTestMenu()
        {
            ShowEmptyMenu();
            GameObject menuRoot = new GameObject("NewMenu", new [] { RectTransform.Il2CppType, VRCUiPage.Il2CppType });
            menuRoot.transform.SetParent(Libraries.QuickMenuUtils.GetVRCUiMInstance().menuContent.transform.Find("Screens"));
            menuRoot.GetComponent<VRCUiPage>().screenType = QuickMenuUtils.GetVRCUiMInstance().menuContent.transform.Find("Screens/Avatar").GetComponent<VRCUiPage>().screenType;
            menuRoot.GetComponent<VRCUiPage>().displayName = "Test Menu";
            menuRoot.GetComponent<VRCUiPage>().AudioHide = QuickMenuUtils.GetVRCUiMInstance().menuContent.transform.Find("Screens/Avatar").GetComponent<VRCUiPage>().AudioHide;
            menuRoot.GetComponent<VRCUiPage>().AudioShow = QuickMenuUtils.GetVRCUiMInstance().menuContent.transform.Find("Screens/Avatar").GetComponent<VRCUiPage>().AudioShow;
            CreateButton(menuRoot, "Test", 0, 0, null);
            QuickMenuUtils.GetVRCUiMInstance().ShowScreenButton("UserInterface/MenuContent/Screens/NewMenu");
        }
        public static void CreateButton(GameObject baseObject, string text, float x, float y, UnityAction onClick)
        {
            Transform baseButtonTransform = QuickMenuUtils.GetQuickMenuInstance().transform.Find("ShortcutMenu/SettingsButton");
            if (baseButtonTransform != null)
            {
                Transform modconf = GameObject.Instantiate(baseButtonTransform, baseObject.transform);
                modconf.name = text;
                modconf.GetComponentInChildren<RectTransform>().sizeDelta = new Vector2(300, 100);
                modconf.GetComponentInChildren<Text>().color = Color.white;
                //modconf.GetComponent<Button>().interactable = false;
                modconf.GetComponent<Button>().onClick.RemoveAllListeners();
                modconf.GetComponent<Button>().onClick.AddListener(onClick);
                modconf.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
                modconf.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y);
                modconf.GetComponent<RectTransform>().localRotation = Quaternion.identity;
                modconf.GetComponent<RectTransform>().localScale = Vector3.one;
                modconf.GetComponentInChildren<Text>().fontSize = 30;
            }
            else
            {
                emmVRCLoader.Logger.LogError("QuickMenu/ShortcutMenu/SettingsButton and QuickMenu/ShortcutMenu/SettingsButton are null");
            }
        }
    }
}
