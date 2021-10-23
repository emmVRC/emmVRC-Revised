using emmVRC.Components;
using emmVRC.Objects;
using emmVRC.Objects.ModuleBases;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace emmVRC.Managers.VRChat
{
    public class NotificationDotManager : MelonLoaderEvents, IWithFixedUpdate
    {
        private static readonly List<GameObject> vanillaIcons = new List<GameObject>();
        private static GameObject emmVRCIcon;
        private static bool _initialized = false;

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            if (buildIndex != -1 || _initialized) return;
            GameObject iconParent = GameObject.Find("UserInterface/UnscaledUI/HudContent/Hud/NotificationDotParent");
            emmVRCIcon = null; 
            foreach (var child in iconParent.transform)
            {
                GameObject icon = child.Cast<Transform>().gameObject;
                vanillaIcons.Add(icon);
                EnableDisableListener listener = icon.AddComponent<EnableDisableListener>();
                listener.OnEnabled += new Action(() => emmVRCIcon.SetActive(false));
                listener.OnDisabled += new Action(() => emmVRCIcon.SetActive(emmVRCNotificationsManager.HasRecentNotifications));
            }

            emmVRCIcon = GameObject.Instantiate(iconParent.transform.GetChild(0).gameObject, iconParent.transform);
            emmVRCIcon.name = "emmVRCIcon";

            Image emmVRCIconImage = emmVRCIcon.GetComponent<Image>();
            emmVRCNotificationsManager.OnNotificationAdded += new Action<Notification>((notification) => { emmVRCIconImage.sprite = notification.icon; emmVRCIconImage.gameObject.SetActive(true); });
            emmVRCNotificationsManager.OnNotificationRemoved += new Action<Notification>((notification) => { if (Managers.emmVRCNotificationsManager.Notifications.Count > 0) emmVRCIconImage.sprite = emmVRCNotificationsManager.Notifications.LastOrDefault().icon; else emmVRCIconImage.gameObject.SetActive(false); });

            _initialized = true;
        }

        public void OnFixedUpdate()
        {
            emmVRCIcon?.SetActive(emmVRCNotificationsManager.HasRecentNotifications);
        }
    }
}