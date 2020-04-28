using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using emmVRC.Libraries;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace emmVRC.Hacks
{
    public class ShortcutMenuButtons
    {
        public static QMSingleButton logoButton;
        public static GameObject emojiButton;
        public static GameObject reportWorldButton;
        public static GameObject trustRankButton;
        public static void Process()
        {
            if (logoButton == null)
            {
                logoButton = new QMSingleButton("ShortcutMenu", Configuration.JSONConfig.LogoButtonX, Configuration.JSONConfig.LogoButtonY, "", () => { System.Diagnostics.Process.Start("https://discord.gg/SpZSH5Z"); }, "emmVRC Version v" + Objects.Attributes.Version + " by the emmVRC Team. Click the logo to join our Discord!", Color.white, Color.white);

            }
            logoButton.setActive(Configuration.JSONConfig.LogoButtonEnabled);
            emojiButton = QuickMenuUtils.GetQuickMenuInstance().transform.Find("ShortcutMenu/EmojiButton").gameObject;
            reportWorldButton = QuickMenuUtils.GetQuickMenuInstance().transform.Find("ShortcutMenu/ReportWorldButton").gameObject;
            trustRankButton = QuickMenuUtils.GetQuickMenuInstance().transform.Find("ShortcutMenu/Toggle_States_ShowTrustRank_Colors").gameObject;
            if (Configuration.JSONConfig.DisableEmojiButton)
                emojiButton.SetActive(false);
            else
                emojiButton.SetActive(true);
            if (Configuration.JSONConfig.DisableReportWorldButton)
                reportWorldButton.SetActive(false);
            else
                reportWorldButton.SetActive(true);
            if (Configuration.JSONConfig.DisableRankToggleButton)
                trustRankButton.transform.localScale = new Vector3(0f, 0f, 0f);
            else
                trustRankButton.transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }
}
