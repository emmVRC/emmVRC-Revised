using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using emmVRC.Libraries;
using emmVRC.Menus;
using emmVRC.Objects;
using emmVRC.Objects.ModuleBases;

namespace emmVRC.Functions.UI
{
    public class TabMenu : MelonLoaderEvents
    {
        public static GameObject newTab;
        public static QMNestedButton tabMenu;
        public static QMSingleButton logoButton;
        public static QMSingleButton functionsButton;
        public static QMSingleButton notificationsButton;

        public static GameObject menuTitle;
        public static GameObject menuSubTitle;
        public static GameObject badge;
        public override void OnUiManagerInit()
        {
            if (Configuration.JSONConfig.StealthMode) return;
            MelonLoader.MelonCoroutines.Start(Initialize());
        }
        public static IEnumerator Initialize()
        {
            while (Functions.Core.Resources.onlineSprite == null || Functions.Core.Resources.TabIcon == null || MonoBehaviourPublicObCoGaCoObCoObCoUnique.prop_MonoBehaviourPublicObCoGaCoObCoObCoUnique_0 == null || MonoBehaviourPublicObCoGaCoObCoObCoUnique.prop_MonoBehaviourPublicObCoGaCoObCoObCoUnique_0.field_Public_ArrayOf_GameObject_0 == null) yield return null;
            newTab = UnityEngine.Object.Instantiate(GameObject.Find("UserInterface/QuickMenu/QuickModeTabs/NotificationsTab"), GameObject.Find("UserInterface/QuickMenu/QuickModeTabs/NotificationsTab").transform.parent);
            newTab.name = "emmVRCTab";
            newTab.GetComponent<UiTooltip>().field_Public_String_0 = "emmVRC Menu";
            newTab.GetComponent<UiTooltip>().field_Public_String_1 = "emmVRC Menu";
            newTab.transform.Find("Icon").GetComponent<UnityEngine.UI.Image>().sprite = Functions.Core.Resources.TabIcon;


            tabMenu = new QMNestedButton("ShortcutMenu", 1923, 10394, "TabMenu", "dasdasdas");
            GameObject tabMenuObj = QuickMenu.prop_QuickMenu_0.gameObject.transform.Find(tabMenu.getMenuName()).gameObject;

            logoButton = new QMSingleButton(tabMenu, 1, 0, "", () => { if (Configuration.JSONConfig.LogoButtonEnabled) System.Diagnostics.Process.Start("https://discord.gg/SpZSH5Z"); }, "emmVRC Version v" + Objects.Attributes.Version + " by the emmVRC Team. Click the logo to join our Discord!", Color.white, Color.white);
            functionsButton = new QMSingleButton(tabMenu, 1, 1, "Functions", () => {
                QuickMenu.prop_QuickMenu_0.transform.Find("QuickModeTabs/HomeTab").GetComponent<UnityEngine.UI.Button>().onClick.Invoke();
                FunctionsMenu.baseMenu.menuEntryButton.getGameObject().GetComponent<UnityEngine.UI.Button>().onClick.Invoke();
            }, "Extra functions that can enhance the user experience or provide practical features");
            notificationsButton = new QMSingleButton(tabMenu, 2, 1, "0\n<color=#FF69B4>emmVRC</color>\nNotifications", () =>
            {
                QuickMenu.prop_QuickMenu_0.transform.Find("QuickModeTabs/HomeTab").GetComponent<UnityEngine.UI.Button>().onClick.Invoke();
                if (Managers.NotificationManager.Notifications.Count > 0)
                    Managers.NotificationManager.NotificationMenu.getMainButton().getGameObject().GetComponent<UnityEngine.UI.Button>().onClick.Invoke();
            }, "View your current emmVRC notifications");

            logoButton.getGameObject().GetComponentInChildren<UnityEngine.UI.Image>().sprite = Functions.Core.Resources.onlineSprite;
            logoButton.getGameObject().GetComponent<RectTransform>().sizeDelta.Scale(new Vector2(0.25f, 0.25f));

            GameObject menuTitle = GameObject.Instantiate(QuickMenuUtils.GetQuickMenuInstance().transform.Find("QuickMenu_NewElements/_InfoBar/EarlyAccessText").gameObject, tabMenuObj.transform);
            menuTitle.GetComponent<UnityEngine.UI.Text>().fontStyle = FontStyle.Normal;
            menuTitle.GetComponent<UnityEngine.UI.Text>().alignment = TextAnchor.MiddleCenter;
            menuTitle.GetComponent<UnityEngine.UI.Text>().color = Color.white;
            menuTitle.GetComponent<UnityEngine.UI.Text>().text = "      <color=#FF69B4>emmVRC </color>v" + Attributes.Version;
            menuTitle.GetComponent<UnityEngine.UI.Text>().fontSize *= 2;
            menuTitle.GetComponent<RectTransform>().anchoredPosition += new Vector2(580f, -495f);

            GameObject menuSubTitle = GameObject.Instantiate(QuickMenuUtils.GetQuickMenuInstance().transform.Find("QuickMenu_NewElements/_InfoBar/EarlyAccessText").gameObject, tabMenuObj.transform);
            menuSubTitle.GetComponent<UnityEngine.UI.Text>().fontStyle = FontStyle.Normal;
            menuSubTitle.GetComponent<UnityEngine.UI.Text>().alignment = TextAnchor.MiddleCenter;
            menuSubTitle.GetComponent<UnityEngine.UI.Text>().color = Color.white;
            menuSubTitle.GetComponent<UnityEngine.UI.Text>().fontSize = (int)(menuSubTitle.GetComponent<UnityEngine.UI.Text>().fontSize * 0.85);
            int randomIndex = new System.Random().Next(Attributes.FlavourTextList.Length);
            menuSubTitle.GetComponent<UnityEngine.UI.Text>().text = Attributes.FlavourTextList[randomIndex];
            menuSubTitle.GetComponent<RectTransform>().anchoredPosition += new Vector2(725f, -620f);

            tabMenu.getMainButton().DestroyMe();
            tabMenu.getBackButton().DestroyMe();
            MonoBehaviour tabDescriptor = newTab.GetComponents<MonoBehaviour>().First(c => c.GetIl2CppType().GetMethod("ShowTabContent") != null);

            System.Collections.Generic.List<GameObject> existingTabs = MonoBehaviourPublicObCoGaCoObCoObCoUnique.prop_MonoBehaviourPublicObCoGaCoObCoObCoUnique_0.field_Public_ArrayOf_GameObject_0.ToList();
            existingTabs.Add(newTab);
            MonoBehaviourPublicObCoGaCoObCoObCoUnique.prop_MonoBehaviourPublicObCoGaCoObCoObCoUnique_0.field_Public_ArrayOf_GameObject_0 = existingTabs.ToArray();

            tabDescriptor.GetIl2CppType().GetFields().First(f => f.FieldType.IsEnum).SetValue(tabDescriptor, new Il2CppSystem.Int32 { m_value = existingTabs.Count }.BoxIl2CppObject());

            //newTab.GetComponent<UnityEngine.UI.Button>().onClick = new UnityEngine.UI.Button.ButtonClickedEvent();
            newTab.GetComponent<UnityEngine.UI.Button>().onClick.AddListener((Action)(() =>
            {
                QuickMenuUtils.ShowQuickmenuPage(tabMenuObj.name);
                notificationsButton.setButtonText(Managers.NotificationManager.Notifications.Count + "\nNotifications");
                if (Managers.NotificationManager.Notifications.Count <= 0)
                    notificationsButton.getGameObject().GetComponent<UnityEngine.UI.Button>().interactable = false;
                else
                    notificationsButton.getGameObject().GetComponent<UnityEngine.UI.Button>().interactable = true;
            }));

            if (!Configuration.JSONConfig.TabMode)
                newTab.transform.localScale = Vector3.zero;
            badge = newTab.transform.Find("Badge").gameObject;
            while (newTab.GetComponent<MonoBehaviourPublicGaTeSiSiUnique>() == null)
                yield return new WaitForSeconds(1f);
            GameObject.Destroy(newTab.GetComponent<MonoBehaviourPublicGaTeSiSiUnique>());
        }
    }
}
