using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using emmVRC.Libraries;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using emmVRC.Objects;
using emmVRC.Objects.ModuleBases;
using VRC.Core;
using VRC;

namespace emmVRC.Hacks
{
    public class ShortcutMenuButtons : MelonLoaderEvents
    {
        public static GameObject socialNotifications;
        public override void OnUiManagerInit()
        {
            MelonLoader.MelonCoroutines.Start(Process());
            Configuration.onConfigUpdated.Add(new System.Collections.Generic.KeyValuePair<string, Action>("DisableOldInviteButtons", () => MelonLoader.MelonCoroutines.Start(Process())));
        }
        public static IEnumerator Process()
        {
            yield return null;
                socialNotifications = GameObject.Find("UserInterface").transform.Find("Canvas_QuickMenu(Clone)/Container/Window/QMNotificationsArea/Notifications/SocialNotificationsOverlay").gameObject;
                socialNotifications.transform.localScale = (Configuration.JSONConfig.DisableOldInviteButtons ? Vector3.zero : Vector3.one);
        }
    }
}
