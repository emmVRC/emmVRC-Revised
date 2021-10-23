using emmVRC.Objects;
using System;
using System.Collections.Generic;

#pragma warning disable IDE1006 // Naming Styles

namespace emmVRC.Managers
{
    public static class emmVRCNotificationsManager
    {
        public static IReadOnlyList<Notification> Notifications => _notifications;
        private static readonly List<Notification> _notifications = new List<Notification>();

        public static event Action<Notification> OnNotificationAdded;
        public static event Action<Notification> OnNotificationRemoved;

        public static int RecentNotificationCount { get; set; }
        public static bool HasRecentNotifications => RecentNotificationCount > 0;

        public static void AddNotification(Notification notification)
        {
            _notifications.Insert(0, notification);
            RecentNotificationCount++;
            OnNotificationAdded?.Invoke(notification);
        }

        public static void RemoveNotification(Notification notification)
        {
            _notifications.Remove(notification);
            // The notification will always be recent if it cant be ignored, so we have to decrement the counter on remove
            // Other wise the notification is always seen to be able to remove it
            if (!notification.canIgnore)
                RecentNotificationCount--;
            OnNotificationRemoved?.Invoke(notification);
        }

        public static void RemoveNotificationAt(int index)
        {
            Notification notification = _notifications[index];
            _notifications.RemoveAt(index);
            OnNotificationRemoved?.Invoke(notification);
        }
    }
}