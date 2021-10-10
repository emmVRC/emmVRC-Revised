using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using emmVRC.Libraries;
using emmVRC.Objects.ModuleBases;

namespace emmVRC.Menus
{
    public class DisabledButtonMenu : MelonLoaderEvents
    {
        public static QMNestedButton baseMenu;
        private static QMSingleButton emojiButton;
        private static QMSingleButton reportWorld;
        private static QMToggleButton rankToggleButton;

        public override void OnUiManagerInit()
        {
            baseMenu = new QMNestedButton(FunctionsMenu.baseMenu.menuBase, 1920, 1080, "Disabled\nButtons", "Contains buttons from the Quick Menu that were disabled by emmVRC");
            baseMenu.getMainButton().DestroyMe();
            emojiButton = new QMSingleButton(baseMenu, 1, 0, "Emoji", () => {
                QuickMenuUtils.ShowQuickmenuPage("EmojiMenu");
            }, "Express Yourself with Emojis");
            reportWorld = new QMSingleButton(baseMenu, 2, 0, "Report\nWorld", () =>
            {
                QuickMenuUtils.GetQuickMenuInstance().transform.Find("ShortcutMenu/ReportWorldButton").GetComponent<UnityEngine.UI.Button>().onClick.Invoke();
            }, "Report Issues with this World");
            rankToggleButton = new QMToggleButton(baseMenu, 3, 0, "Appearing as\nRank", () =>
            {
                QuickMenuUtils.GetQuickMenuInstance().transform.Find("ShortcutMenu/Toggle_States_ShowTrustRank_Colors").GetComponent<UnityEngine.UI.Button>().onClick.Invoke();
                LoadMenu();
            }, "Appear as\n<color=#2BCE5C>User</color> Rank", () =>
            {
                QuickMenuUtils.GetQuickMenuInstance().transform.Find("ShortcutMenu/Toggle_States_ShowTrustRank_Colors").GetComponent<UnityEngine.UI.Button>().onClick.Invoke();
                LoadMenu();
            }, "TOGGLE: (Known or Higher Trust Rank): Display Your Trust Rank as User", null, null, false, true);
        }
        public static void LoadMenu()
        {
            QuickMenuUtils.ShowQuickmenuPage(baseMenu.getMenuName());
            emojiButton.setActive(Configuration.JSONConfig.DisableEmojiButton);
            reportWorld.setActive(Configuration.JSONConfig.DisableReportWorldButton);
            rankToggleButton.setActive(Configuration.JSONConfig.DisableRankToggleButton);

            if (QuickMenuUtils.GetQuickMenuInstance().transform.Find("ShortcutMenu/Toggle_States_ShowTrustRank_Colors/KNOWN").gameObject.activeSelf)
            {
                if (QuickMenuUtils.GetQuickMenuInstance().transform.Find("ShortcutMenu/Toggle_States_ShowTrustRank_Colors/KNOWN/ON").gameObject.activeSelf)
                {
                    rankToggleButton.setOnText("Appearing as\n<color=#FF7B42>Known User</color>");
                    rankToggleButton.setOffText("Appear as\n<color=#2BCE5C>User</color> Rank");
                    rankToggleButton.setToolTip("TOGGLE: (Known or Higher Trust Rank): Display Your Trust Rank as User");
                }
                else if (QuickMenuUtils.GetQuickMenuInstance().transform.Find("ShortcutMenu/Toggle_States_ShowTrustRank_Colors/KNOWN/OFF").gameObject.activeSelf)
                {
                    rankToggleButton.setOnText("Appearing as\n<color=#FF7B42>Known User</color>");
                    rankToggleButton.setOffText("Appear as\n<color=#2BCE5C>User</color> Rank");
                    rankToggleButton.setToolTip("TOGGLE: (Known or Higher Trust Rank): Display Your Actual Trust Rank");
                }
            }
            else if (QuickMenuUtils.GetQuickMenuInstance().transform.Find("ShortcutMenu/Toggle_States_ShowTrustRank_Colors/TRUSTED").gameObject.activeSelf)
            {
                if (QuickMenuUtils.GetQuickMenuInstance().transform.Find("ShortcutMenu/Toggle_States_ShowTrustRank_Colors/TRUSTED/ON").gameObject.activeSelf)
                {
                    rankToggleButton.setOnText("Appearing as\n<color=#8143E6>Trusted User</color>");
                    rankToggleButton.setOffText("Appear as\n<color=#2BCE5C>User</color> Rank");
                    rankToggleButton.setToolTip("TOGGLE: (Known or Higher Trust Rank): Display Your Trust Rank as User");
                }
                else if (QuickMenuUtils.GetQuickMenuInstance().transform.Find("ShortcutMenu/Toggle_States_ShowTrustRank_Colors/TRUSTED/OFF").gameObject.activeSelf)
                {
                    rankToggleButton.setOnText("Appear as\n<color=#8143E6>Trusted User</color>");
                    rankToggleButton.setOffText("Appearing as\n<color=#2BCE5C>User</color> Rank");
                    rankToggleButton.setToolTip("TOGGLE: (Known or Higher Trust Rank): Display Your Actual Trust Rank");
                }
            }
            else
            {
                rankToggleButton.setActive(false);
            }
        }
    }
}
