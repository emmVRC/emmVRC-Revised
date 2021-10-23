using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using emmVRC.Objects.ModuleBases;
using emmVRC.Utils;

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
            mainTab = new Utils.Tab(Utils.ButtonAPI.menuTabBase.transform.parent, "emmVRC_MainMenu", "", Functions.Core.Resources.TabIcon);
            notificationsGroup = new ButtonGroup(basePage, "Notifications");
            tweaksGroup = new Utils.ButtonGroup(basePage.menuContents, "Tweaks");
            featuresGroup = new Utils.ButtonGroup(basePage.menuContents, "Features");
            Utils.SingleButton testBtn6 = new Utils.SingleButton(featuresGroup.gameObject.transform, "Alarm\nClocks", () => ButtonAPI.GetQuickMenuInstance().ShowAlert("Not yet implemented"), "Nothing here", Functions.Core.Resources.AlarmClockIcon);
            otherGroup = new Utils.ButtonGroup(basePage.menuContents, "Other");
            Utils.ButtonGroup grp2 = new ButtonGroup(basePage.menuContents, "");
            Utils.SingleButton testBtn11 = new Utils.SingleButton(grp2.gameObject.transform, "Instant\nRestart", () =>
            {
                DestructiveActions.ForceRestart();
            }, "Restarts, instantly.", null);
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
