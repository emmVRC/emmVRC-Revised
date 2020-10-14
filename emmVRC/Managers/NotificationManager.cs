using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using emmVRC.Objects;
using emmVRC.Libraries;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace emmVRC.Managers
{
    public class NotificationManager
    {
        private static bool Enabled = true;
        private static int notificationActiveTimer = 0;
        public static QMNestedButton NotificationMenu;
        private static QMSingleButton NotificationButton1;
        private static QMSingleButton NotificationButton2;
        private static GameObject NotificationIcon;
        private static List<GameObject> VanillaIcons = new List<GameObject>();
        private static Thread NotificationManagerThread;
        private static List<Notification> Notifications = new List<Notification>();
        private static bool blink = false;
        public static void Initialize()
        {
            // Initialize the emmVRC Notification icon, based on the vanilla Notification icon
            NotificationIcon vanillaIcon = GameObject.FindObjectOfType<NotificationIcon>();
            NotificationManager.NotificationIcon = GameObject.Instantiate(vanillaIcon.notificationIcon, vanillaIcon.notificationIcon.transform.parent);
            NotificationManager.NotificationIcon.name = "emmVRCNotificationIcon";
            NotificationIcon.GetComponent<Image>().sprite = Resources.alertSprite;
            // Populate the list of vanilla icons, for use in checks later
            try
            {
                VanillaIcons.Add(vanillaIcon.notificationIcon);
                VanillaIcons.Add(vanillaIcon.InviteRequestIcon);
                VanillaIcons.Add(vanillaIcon.InviteIcon);
                VanillaIcons.Add(vanillaIcon.FriendRequestIcon);
                VanillaIcons.Add(vanillaIcon.voteKickIcon);
            }
            catch (System.Exception ex)
            {
                emmVRCLoader.Logger.LogError("Failed to fetch all vanilla notification icons: " + ex.ToString());
                emmVRCLoader.Logger.Log("This isn't a major error, it most likely means that the icons were changed around or removed in this build of VRChat, or assemblies need to be regenerated.");
            }
            NotificationMenu = new QMNestedButton("ShortcutMenu", Configuration.JSONConfig.NotificationButtonPositionX, Configuration.JSONConfig.NotificationButtonPositionY, "\nemmVRC\nNotifications", "  new emmVRC Notifications are available!");
            NotificationButton1 = new QMSingleButton(NotificationMenu, 1, 0, "Accept", null, "Accept");
            NotificationButton2 = new QMSingleButton(NotificationMenu, 2, 0, "Decline", null, "Decline");

            // Set up the entering menu action
            NotificationMenu.getMainButton().setAction(() => {
                QuickMenuUtils.ShowQuickmenuPage(NotificationMenu.getMenuName());
                QuickMenuUtils.GetQuickMenuInstance().Method_Public_Void_EnumNPublicSealedvaUnNoToUs7vUsNoUnique_APIUser_String_PDM_0(QuickMenuContextualDisplay.EnumNPublicSealedvaUnNoToUs7vUsNoUnique.Notification, null, Notifications[0].Message);
                if (Notifications[0].Button1Action != null)
                {
                    NotificationButton1.setButtonText(Notifications[0].Button1Text);
                    NotificationButton1.setAction(Notifications[0].Button1Action);
                    NotificationButton1.setActive(true);
                }
                else
                    NotificationButton1.setActive(false);
                if (Notifications[0].Button2Action != null)
                {
                    NotificationButton2.setButtonText(Notifications[0].Button2Text);
                    NotificationButton2.setAction(Notifications[0].Button2Action);
                    NotificationButton2.setActive(true);
                }
                else
                    NotificationButton2.setActive(false);
            });

            // Create the separate thread that will manage the notification system
            NotificationManagerThread = new Thread(Loop)
            {
                Name = "emmVRC Notification Manager Thread",
                IsBackground = true
            };
            NotificationManagerThread.Start();
        }
        private static void Loop()
        {
            while (Enabled)
            {
                Thread.Sleep(1000);
                if (RoomManagerBase.field_Internal_Static_ApiWorld_0 != null)
                {
                    try
                    {
                        // Change icon and button visibility, if there is a notification
                        if (Notifications.Count > 0)
                        {
                            // Checking if a vanilla icon (such as an invite) is already present, to avoid overlapping
                            bool vanillaIconsActive = false;
                            foreach (GameObject icon in VanillaIcons)
                                if (icon.activeSelf)
                                    vanillaIconsActive = true;
                            if (!vanillaIconsActive)
                                NotificationIcon.SetActive(true);
                            else
                                NotificationIcon.SetActive(false);
                            // Set up the notification icon
                            NotificationIcon.GetComponent<Image>().sprite = Notifications[0].Icon;

                            NotificationMenu.getMainButton().setActive(true);
                            NotificationMenu.getMainButton().setButtonText((blink ? "<color=#FF69B4>" + Notifications.Count + "</color>" : "" + Notifications.Count) + "\nemmVRC\nNotifications");
                            NotificationMenu.getMainButton().setToolTip(Notifications.Count + " new emmVRC notifications are available!" + (Notifications[0].Timeout != -1 ? " This notification will expire in " + Notifications[0].Timeout + " seconds." : ""));
                        }
                        else
                        {
                            NotificationIcon.SetActive(false);
                            NotificationMenu.getMainButton().setActive(false);
                            notificationActiveTimer = 0;
                        }

                        if (Notifications.Count > 0 && Notifications[0].Timeout != -1)
                        {
                            // Increment the notification active timer, until the timeout is met
                            if (Notifications[0].Timeout == notificationActiveTimer)
                            {
                                Notifications.RemoveAt(0);
                                notificationActiveTimer = 0;
                            }
                            else
                                notificationActiveTimer++;
                        }
                    }
                    catch (Exception ex)
                    {
                        emmVRCLoader.Logger.LogError("Notification Manager update loop encountered an exception: " + ex.ToString());
                    }
                }
            }
        }

        public static void AddNotification(string text, string button1Text, System.Action button1Action, string button2Text, System.Action button2Action, Sprite notificationIcon = null, int timeout = -1)
        {
            Notification newNotification = new Notification()
            {
                Message = text,
                Button1Text = button1Text,
                Button2Text = button2Text,
                Button1Action = button1Action,
                Button2Action = button2Action,
                Icon = (notificationIcon == null ? Resources.errorSprite : notificationIcon),
                Timeout = timeout
            };
            Notifications.Add(newNotification);
        }
        public static void DismissCurrentNotification()
        {
            if (Notifications.Count > 0)
                Notifications.Remove(Notifications[0]);
            QuickMenuUtils.ShowQuickmenuPage("ShortcutMenu");
            QuickMenuUtils.GetQuickMenuInstance().Method_Public_Void_EnumNPublicSealedvaUnNoToUs7vUsNoUnique_APIUser_String_PDM_0(QuickMenuContextualDisplay.EnumNPublicSealedvaUnNoToUs7vUsNoUnique.NoSelection);
        }
    }
}