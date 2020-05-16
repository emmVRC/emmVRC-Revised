using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using emmVRC.Libraries;
using UnityEngine;
using UnityEngine.UI;

namespace emmVRC.Hacks
{
    public class UserInteractMenuButtons
    {
        public static GameObject emojiButton;
        public static GameObject reportWorldButton;
        public static GameObject trustRankButton;
        public static void Initialize()
        {
            if (Configuration.JSONConfig.DisablePlaylistsButton)
                QuickMenuUtils.GetQuickMenuInstance().transform.Find("UserInteractMenu/ViewPlaylistsButton").gameObject.SetActive(false);
            else
                QuickMenuUtils.GetQuickMenuInstance().transform.Find("UserInteractMenu/ViewPlaylistsButton").gameObject.SetActive(true);
            if (Configuration.JSONConfig.DisableAvatarStatsButton)
                QuickMenuUtils.GetQuickMenuInstance().transform.Find("UserInteractMenu/ShowAvatarStatsButton").localScale = new UnityEngine.Vector3(0f, 0f, 0f);
            else
                QuickMenuUtils.GetQuickMenuInstance().transform.Find("UserInteractMenu/ShowAvatarStatsButton").localScale = new UnityEngine.Vector3(1f, 1f, 1f);
            if (Configuration.JSONConfig.DisableReportUserButton)
                QuickMenuUtils.GetQuickMenuInstance().transform.Find("UserInteractMenu/ReportAbuseButton").localScale = new UnityEngine.Vector3(0f, 0f, 0f);
            else
                QuickMenuUtils.GetQuickMenuInstance().transform.Find("UserInteractMenu/ReportAbuseButton").localScale = new UnityEngine.Vector3(1f, 1f, 1f);
        }
    }
}
