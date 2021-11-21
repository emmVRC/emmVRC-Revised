using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using emmVRC.Utils;
using emmVRC.Objects.ModuleBases;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace emmVRC.Menus
{
    [Priority(51)]
    public class NotificationMenu : MelonLoaderEvents
    {
        private static MenuPage notificationViewPage;
        private static SimpleSingleButton textView;
        private static TextMeshProUGUI textMeshPro;
        private static ButtonGroup notificationActionGroup;
        private static SimpleSingleButton acceptButton;
        private static SimpleSingleButton ignoreButton;

        private static SingleButton[] notificationButtons;
        private static Objects.Notification currentNotification;

        private static bool _initialized = false;
        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            if (buildIndex != -1 || _initialized) return;
            notificationButtons = new SingleButton[] { new SingleButton(FunctionsMenu.notificationsGroup, "", null, ""), new SingleButton(FunctionsMenu.notificationsGroup, "", null, ""), new SingleButton(FunctionsMenu.notificationsGroup, "", null, ""), new SingleButton(FunctionsMenu.notificationsGroup, "", null, "") };
            notificationViewPage = new MenuPage("emmVRC_NotificationView", "emmVRC Notification", false);
            textView = new SimpleSingleButton(notificationViewPage, "This is a long string of text that can easily go on to the next line and beyond!", null, "");
            GameObject.DestroyImmediate(textView.gameObject.GetComponent<Button>());
            GameObject.DestroyImmediate(textView.gameObject.GetComponent<VRC.UI.Elements.Tooltips.UiTooltip>());
            textView.gameObject.transform.DestroyChildren(a => !a.name.Contains("Text"));
            textMeshPro = textView.gameObject.transform.GetComponentInChildren<TextMeshProUGUI>();
            textMeshPro.GetComponent<RectTransform>().offsetMin = new Vector2(-500f, -150f);
            textMeshPro.GetComponent<RectTransform>().offsetMax = new Vector2(500f, 150f);
            textMeshPro.color = Configuration.menuColor();
            new ButtonGroup(notificationViewPage, "");
            notificationActionGroup = new ButtonGroup(notificationViewPage, "");
            acceptButton = new SimpleSingleButton(notificationActionGroup, "", null, "");
            ignoreButton = new SimpleSingleButton(notificationActionGroup, "", null, "");

            Managers.emmVRCNotificationsManager.OnNotificationAdded += ((Objects.Notification notif) => {
                UpdateNotificationGroup();
            });
            Managers.emmVRCNotificationsManager.OnNotificationRemoved += ((Objects.Notification notif) => {
                UpdateNotificationGroup();
                if (notificationViewPage.menuContents.gameObject.activeInHierarchy)
                    notificationViewPage.CloseMenu();
            });

            _initialized = true;

            UpdateNotificationGroup();
        }
        private static void UpdateNotificationGroup()
        {
                foreach (SingleButton btn in notificationButtons)
                    btn.gameObject.SetActive(false);
            FunctionsMenu.notificationsGroup.SetActive(false);
            if (Managers.emmVRCNotificationsManager.Notifications.Count < 4 && Managers.emmVRCNotificationsManager.Notifications.Count > 0)
            {
                FunctionsMenu.notificationsGroup.SetActive(true);
                for (int i=0; i < Managers.emmVRCNotificationsManager.Notifications.Count; i++)
                {
                    Objects.Notification notif = Managers.emmVRCNotificationsManager.Notifications[i];
                    notificationButtons[i].gameObject.SetActive(true);
                    notificationButtons[i].SetText(Managers.emmVRCNotificationsManager.Notifications[i].name);
                    notificationButtons[i].SetIconColor(Color.white);
                    notificationButtons[i].SetIcon(Managers.emmVRCNotificationsManager.Notifications[i].icon);
                    notificationButtons[i].SetAction(() =>
                   {
                        currentNotification = notif;
                        notificationViewPage.OpenMenu();
                        textView.SetText(notif.content);
                        acceptButton.gameObject.SetActive(notif.showAcceptButton);
                        ignoreButton.gameObject.SetActive(notif.showIgnoreButton);
                        acceptButton.SetText(notif.acceptButtonText);
                        ignoreButton.SetText(notif.ignoreButtonText);
                        acceptButton.SetAction(() => { notificationViewPage.CloseMenu(); if (notif.acceptButton != null) notif.acceptButton.Invoke(); Managers.emmVRCNotificationsManager.RemoveNotification(notif); });
                        ignoreButton.SetAction(() => { notificationViewPage.CloseMenu(); if (notif.ignoreButton != null) notif.ignoreButton.Invoke(); Managers.emmVRCNotificationsManager.RemoveNotification(notif); });
                        acceptButton.SetTooltip(notif.acceptButtonTooltip);
                        ignoreButton.SetTooltip(notif.ignoreButtonTooltip);
                    });
                }
            }
        }
    }
}
