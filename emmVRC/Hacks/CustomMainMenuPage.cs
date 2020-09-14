using emmVRC.Libraries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnhollowerRuntimeLib;
using UnityEngine;
using UnityEngine.UI;
using VRC.UI;

namespace emmVRC.Hacks
{
    public class CustomMainMenuPage
    {
        public static VRCUiPage customPage;
        public static GameObject customMenuRoot;
        public static GameObject TitleBar;
        public static Text titleBarText;
        public static List<CustomMainMenuPage> customMenuPages;
        public static Vector3 topCorner;
        public static void Initialize()
        {
            customMenuRoot = new GameObject("CustomMenu", new Il2CppSystem.Type[]{ Il2CppType.Of<VRCUiPage>(), Il2CppType.Of<RectTransform>() });
            customMenuRoot.transform.SetParent(QuickMenuUtils.GetVRCUiMInstance().menuContent.transform.Find("Screens"));
            customPage = customMenuRoot.GetComponent<VRCUiPage>();
            customPage.screenType = "SCREEN";
            customPage.displayName = "Custom Menu";
            customPage.AudioShow = QuickMenuUtils.GetVRCUiMInstance().menuContent.transform.Find("Screens/Avatar").GetComponent<PageAvatar>().AudioShow;
            customPage.AudioHide = QuickMenuUtils.GetVRCUiMInstance().menuContent.transform.Find("Screens/Avatar").GetComponent<PageAvatar>().AudioHide;
            TitleBar = GameObject.Instantiate(QuickMenuUtils.GetVRCUiMInstance().menuContent.transform.Find("Screens/Settings/TitlePanel").gameObject, customMenuRoot.transform, true);
            GameObject.Destroy(TitleBar.transform.Find("VersionText").gameObject);
            titleBarText = TitleBar.GetComponentInChildren<Text>(true);
            customMenuPages = new List<CustomMainMenuPage>();
            topCorner = TitleBar.transform.position;
        }
        public static void ShowCustomMenu(CustomMainMenuPage targetPage)
        {
            try
            {
                QuickMenuUtils.GetQuickMenuInstance().Method_Public_Void_Int32_Boolean_0(3, false);
                QuickMenuUtils.GetVRCUiMInstance().Method_Public_VRCUiPage_VRCUiPage_0(customPage);
                titleBarText.text = targetPage.Title;
                TitleBar.SetActive(true);
                foreach (CustomMainMenuPage page in customMenuPages)
                    page.menuRoot.SetActive(false);
                targetPage.menuRoot.SetActive(true);
            } catch (Exception ex)
            {
                emmVRCLoader.Logger.LogError(ex.ToString());
            }
        }

        public GameObject menuRoot;
        public string Title;

        public CustomMainMenuPage(string Title)
        {
            menuRoot = new GameObject(Title + "_CustomMenu", new Il2CppSystem.Type[] { Il2CppType.Of<RectTransform>() });
            menuRoot.transform.SetParent(customMenuRoot.transform);
            menuRoot.SetActive(false);
            this.Title = Title;
            customMenuPages.Add(this);
        }
        public GameObject CreateCheckbox(string name, Vector2 offset, Action<bool> onCheck)
        {
            GameObject customCheckbox = GameObject.Instantiate(QuickMenuUtils.GetVRCUiMInstance().menuContent.transform.Find("Screens/Settings/ComfortSafetyPanel/HoloportToggle").gameObject, this.menuRoot.transform, true);
            customCheckbox.transform.position = TitleBar.transform.position;
            customCheckbox.GetComponent<RectTransform>().anchoredPosition -= new Vector2(0.6f, 0.2f);
            customCheckbox.GetComponent<RectTransform>().anchoredPosition += offset;
            customCheckbox.name = name + "_CustomCheckbox";
            customCheckbox.GetComponent<Toggle>().enabled = true;
            customCheckbox.GetComponent<Toggle>().onValueChanged = new Toggle.ToggleEvent();
            customCheckbox.GetComponent<Toggle>().isOn = false;
            customCheckbox.GetComponent<Toggle>().onValueChanged.AddListener(onCheck);
            customCheckbox.GetComponentInChildren<Text>(true).text = name;
            return customCheckbox;
        }
        public GameObject CreateButton(string name, Vector2 offset, Action buttonPressed)
        {
            GameObject customButton = GameObject.Instantiate(QuickMenuUtils.GetVRCUiMInstance().menuContent.transform.Find("Screens/Social/Current Status/StatusButton").gameObject, this.menuRoot.transform, true);
            customButton.transform.position = TitleBar.transform.position;
            customButton.GetComponent<RectTransform>().anchoredPosition -= new Vector2(0.55f, 0.2f);
            customButton.GetComponent<RectTransform>().anchoredPosition += offset;
            customButton.name = name + "_CustomButton";
            customButton.GetComponentInChildren<Text>(true).text = name;
            customButton.GetComponent<UnityEngine.UI.Button>().onClick = new UnityEngine.UI.Button.ButtonClickedEvent();
            customButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(buttonPressed);
            return customButton;
        }
    }
}
