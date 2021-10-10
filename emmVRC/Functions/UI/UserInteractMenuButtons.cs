using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using emmVRC.Libraries;
using UnityEngine;
using UnityEngine.UI;
using emmVRC.Objects.ModuleBases;

namespace emmVRC.Functions.UI
{
    public class UserInteractMenuButtons : MelonLoaderEvents
    {
        public static GameObject playlistButton;
        public static GameObject avatarStatsButton;
        public static GameObject reportUserButton;
        public static GameObject warnButton;
        public static GameObject kickButton;
        public static QMSingleButton HalfWarnButton;
        public static QMSingleButton HalfKickButton;
        private static bool LoopRunning = false;
        public override void OnUiManagerInit()
        {
            if (Configuration.JSONConfig.StealthMode) return;
                playlistButton = QuickMenuUtils.GetQuickMenuInstance().transform.Find("UserInteractMenu/ViewPlaylistsButton").gameObject;
                avatarStatsButton = QuickMenuUtils.GetQuickMenuInstance().transform.Find("UserInteractMenu/ShowAvatarStatsButton").gameObject;
                reportUserButton = QuickMenuUtils.GetQuickMenuInstance().transform.Find("UserInteractMenu/ReportAbuseButton").gameObject;
                warnButton = QuickMenuUtils.GetQuickMenuInstance().transform.Find("UserInteractMenu/WarnButton").gameObject;
                kickButton = QuickMenuUtils.GetQuickMenuInstance().transform.Find("UserInteractMenu/KickButton").gameObject;
                HalfWarnButton = new QMSingleButton("UserInteractMenu", 2, 3, "Warn", () => { QuickMenuUtils.GetQuickMenuInstance().transform.Find("UserInteractMenu/WarnButton").GetComponent<Button>().onClick.Invoke(); }, "World Owner Only: Warn this User of Bad Behavior");
                HalfWarnButton.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1f, 2.0175f);
                HalfWarnButton.getGameObject().GetComponent<RectTransform>().anchoredPosition += new Vector2(0f, 96f);
                HalfKickButton = new QMSingleButton("UserInteractMenu", 2, 3, "Kick", () => { QuickMenuUtils.GetQuickMenuInstance().transform.Find("UserInteractMenu/KickButton").GetComponent<Button>().onClick.Invoke(); }, "World Owner Only: Kick this User from The World");
                HalfKickButton.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1f, 2.0175f);
                HalfKickButton.getGameObject().GetComponent<RectTransform>().anchoredPosition += new Vector2(0f, -96f);

            Components.EnableDisableListener listener = QuickMenuUtils.GetQuickMenuInstance().transform.Find("UserInteractMenu").gameObject.AddComponent<Components.EnableDisableListener>();
            listener.OnEnabled += () =>
            {
                if (Configuration.JSONConfig.MinimalWarnKickButton)
                {
                    HalfWarnButton.setActive(warnButton.activeSelf);
                    HalfKickButton.setActive(kickButton.activeSelf);
                }
                else
                {
                    HalfWarnButton.setActive(false);
                    HalfKickButton.setActive(false);
                }
                if (Configuration.JSONConfig.DisablePlaylistsButton)
                    playlistButton.gameObject.SetActive(false);
                else
                    playlistButton.gameObject.SetActive(true);
                if (Configuration.JSONConfig.DisableAvatarStatsButton)
                    avatarStatsButton.transform.localScale = new UnityEngine.Vector3(0f, 0f, 0f);
                else
                    avatarStatsButton.transform.localScale = new UnityEngine.Vector3(1f, 1f, 1f);
                if (Configuration.JSONConfig.DisableReportUserButton)
                    reportUserButton.transform.localScale = new UnityEngine.Vector3(0f, 0f, 0f);
                else
                    reportUserButton.transform.localScale = new UnityEngine.Vector3(1f, 1f, 1f);
                if (Configuration.JSONConfig.MinimalWarnKickButton)
                {
                    warnButton.transform.localScale = new UnityEngine.Vector3(0f, 0f, 0f);
                    kickButton.transform.localScale = new UnityEngine.Vector3(0f, 0f, 0f);
                }
                else
                {
                    warnButton.transform.localScale = new UnityEngine.Vector3(1f, 1f, 1f);
                    kickButton.transform.localScale = new UnityEngine.Vector3(1f, 1f, 1f);
                }
            };
        }
    }
}
