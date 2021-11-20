using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using emmVRC.Objects.ModuleBases;
using emmVRC.Utils;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace emmVRC.Menus
{
    [Priority(50)]
    public class FunctionsMenu : MelonLoaderEvents
    {
        public static MenuPage basePage;
        private static Tab mainTab;
        internal static ButtonGroup notificationsGroup;
        internal static ButtonGroup tweaksGroup;
        internal static ButtonGroup featuresGroup;
        internal static ButtonGroup otherGroup;

        private static bool _initialized = false;

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            if (buildIndex != -1 || _initialized) return;
            basePage = new Utils.MenuPage("emmVRC_MainMenu", "emmVRC", true, false, true, () => Utils.ButtonAPI.GetQuickMenuInstance().AskConfirmOpenURL("https://discord.gg/emmVRC"), "<color=#FF69B4>emmVRC</color> version " + Objects.Attributes.Version+".\nClick to join our Discord!", Functions.Core.Resources.onlineSprite, true);
            mainTab = new Utils.Tab(Utils.ButtonAPI.menuTabBase.transform.parent, "emmVRC_MainMenu", "emmVRC", Functions.Core.Resources.TabIcon, () => {
                if (Configuration.JSONConfig.AcceptedEULAVersion != Objects.Attributes.EULAVersion)
                {
                    Utils.ButtonAPI.menuTabBase.transform.parent.Find("Page_Dashboard").GetComponent<Button>().onClick.Invoke();
                    ButtonAPI.GetQuickMenuInstance().ShowCustomDialog("Welcome to emmVRC!", (Configuration.JSONConfig.AcceptedEULAVersion == "0.0.0" ? "To use emmVRC, you must first read and agree to our End User License Agreement.\n\nThis will open in your web browser." : "We have updated our End User License Agreement. To continue, you must read and agree to it.\n\nThis will open in your web browser."), "Open EULA", "Agree", "Decline", () =>
                    {
                        Application.OpenURL("https://ttyf.me/emmvrceula");
                    }, () =>
                    {
                        Configuration.WriteConfigOption("AcceptedEULAVersion", Objects.Attributes.EULAVersion);
                    });
                }
            });

            GameObject textBase = new GameObject("ChangelogText");
            textBase.transform.SetParent(basePage.menuContents);
            textBase.transform.localPosition = Vector3.zero;
            textBase.transform.localRotation = new Quaternion(0, 0, 0, 0);
            textBase.transform.localScale = Vector3.one;
            TextMeshProUGUI textText = textBase.AddComponent<TextMeshProUGUI>();
            textText.margin = new Vector4(25, 0, 50, 0);
            textText.text = "Version " + Objects.Attributes.Version.ToString(3);
            textText.alignment = TextAlignmentOptions.Left;

            Network.NetworkClient.onLogin += () => {
                textText.text = "<align=\"left\">  Version " + Objects.Attributes.Version.ToString(3) + "<line-height=0>\n" + (Configuration.JSONConfig.emmVRCNetworkEnabled ? (!string.IsNullOrEmpty(Network.NetworkClient.webToken) ? "<align=\"right\"><color=#00FF00>Connected to the emmVRC Network  </color>" : "<align=\"right\"><color=#FF0000>Not connected to the emmVRC Network  </color>") : "");
            };
            Network.NetworkClient.onLogout += () =>
            {
                textText.text = "<align=\"left\">  Version " + Objects.Attributes.Version.ToString(3) + "<line-height=0>\n" + (Configuration.JSONConfig.emmVRCNetworkEnabled ? (!string.IsNullOrEmpty(Network.NetworkClient.webToken) ? "<align=\"right\"><color=#00FF00>Connected to the emmVRC Network  </color>" : "<align=\"right\"><color=#FF0000>Not connected to the emmVRC Network  </color>") : "");
            };
            Components.EnableDisableListener textListener = textBase.AddComponent<Components.EnableDisableListener>();
            textListener.OnEnabled += () =>
            {
                textText.text = "<align=\"left\">  Version " + Objects.Attributes.Version.ToString(3) + "<line-height=0>\n" + (Configuration.JSONConfig.emmVRCNetworkEnabled ? (!string.IsNullOrEmpty(Network.NetworkClient.webToken) ? "<align=\"right\"><color=#00FF00>Connected to the emmVRC Network  </color>" : "<align=\"right\"><color=#FF0000>Not connected to the emmVRC Network  </color>") : "");
            };

            notificationsGroup = new ButtonGroup(basePage, "Notifications");
            tweaksGroup = new Utils.ButtonGroup(basePage.menuContents, "Tweaks");
            featuresGroup = new Utils.ButtonGroup(basePage.menuContents, "Features");
            Utils.SingleButton testBtn6 = new Utils.SingleButton(featuresGroup.gameObject.transform, "Alarm\nClocks", () => ButtonAPI.GetQuickMenuInstance().ShowAlert("Not yet implemented"), "Nothing here", Functions.Core.Resources.AlarmClockIcon);
            otherGroup = new Utils.ButtonGroup(basePage.menuContents, "Other");
            basePage.menuContents.parent.parent.parent.GetChild(0).Find("RightItemContainer/Button_QM_Expand/Icon").GetComponent<UnityEngine.RectTransform>().sizeDelta = new UnityEngine.Vector2(72, 72);
            basePage.menuContents.parent.parent.parent.GetChild(0).Find("RightItemContainer/Button_QM_Expand/Icon").GetComponent<UnityEngine.RectTransform>().localPosition = new UnityEngine.Vector3(0f, 8f, 0f);
            _initialized = true;

            Managers.emmVRCNotificationsManager.OnNotificationAdded += (Objects.Notification notif) =>
            {
                mainTab.SetBadge(Managers.emmVRCNotificationsManager.Notifications.Count == 0 ? false : true, Managers.emmVRCNotificationsManager.Notifications.Count + " NEW");
            };
            Managers.emmVRCNotificationsManager.OnNotificationRemoved += (Objects.Notification notif) =>
            {
                mainTab.SetBadge(Managers.emmVRCNotificationsManager.Notifications.Count == 0 ? false : true, Managers.emmVRCNotificationsManager.Notifications.Count + " NEW");
            };
        }
    }
}
