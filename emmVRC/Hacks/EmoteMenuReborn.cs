using emmVRC.Libraries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace emmVRC.Hacks
{
    public class EmoteMenuReborn
    {
        private static UnityEngine.UI.Button.ButtonClickedEvent oldButtonEvent;
        private static TextDisplayMenu emoteDisplayMenu;
        public static QMNestedButton newEmoteMenu;
        private static QMSingleButton Emote1Button;
        private static QMSingleButton Emote2Button;
        private static QMSingleButton Emote3Button;
        private static QMSingleButton Emote4Button;
        private static QMSingleButton Emote5Button;
        private static QMSingleButton Emote6Button;
        private static QMSingleButton Emote7Button;
        private static QMSingleButton Emote8Button;
        public static void Initialize()
        {
            emoteDisplayMenu = new TextDisplayMenu("ShortcutMenu", 3231, 1232, "EmoteMenuMessage", "", null, "", "This avatar does not have emotes.\nTo access expressions, exit the\nQuick Menu and use...\n\n\nIndex & Rift : [Joystick Press]\nVive Wand: [Right Pad Down Press or Hold QM Button]\nDesktop: [R Button]");
            emoteDisplayMenu.menuEntryButton.DestroyMe();
            //oldButtonEvent = QuickMenuUtils.GetQuickMenuInstance().transform.Find("ShortcutMenu/EmoteButton").GetComponent<UnityEngine.UI.Button>().onClick;
            QuickMenuUtils.GetQuickMenuInstance().transform.Find("ShortcutMenu/EmoteButton").GetComponent<UnityEngine.UI.Button>().onClick = new UnityEngine.UI.Button.ButtonClickedEvent();
            QuickMenuUtils.GetQuickMenuInstance().transform.Find("ShortcutMenu/EmoteButton").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(new System.Action(() => {
                if (VRCPlayer.field_Internal_Static_VRCPlayer_0 != null)
                    if (VRCPlayer.field_Internal_Static_VRCPlayer_0.prop_VRCAvatarManager_0 != null)
                        if (VRCPlayer.field_Internal_Static_VRCPlayer_0.prop_VRCAvatarManager_0.gameObject.GetComponentInChildren<VRCSDK2.VRC_AvatarDescriptor>() != null)
                        {
                            QuickMenuUtils.ShowQuickmenuPage(newEmoteMenu.getMenuName());

                            Emote1Button.setButtonText(GetEmote(1));
                            Emote1Button.setToolTip("Plays the emote " + GetEmote(1));
                            Emote2Button.setButtonText(GetEmote(2));
                            Emote2Button.setToolTip("Plays the emote " + GetEmote(2));
                            Emote3Button.setButtonText(GetEmote(3));
                            Emote3Button.setToolTip("Plays the emote " + GetEmote(3));
                            Emote4Button.setButtonText(GetEmote(4));
                            Emote4Button.setToolTip("Plays the emote " + GetEmote(4));
                            Emote5Button.setButtonText(GetEmote(5));
                            Emote5Button.setToolTip("Plays the emote " + GetEmote(5));
                            Emote6Button.setButtonText(GetEmote(6));
                            Emote6Button.setToolTip("Plays the emote " + GetEmote(6));
                            Emote7Button.setButtonText(GetEmote(7));
                            Emote7Button.setToolTip("Plays the emote " + GetEmote(7));
                            Emote8Button.setButtonText(GetEmote(8));
                            Emote8Button.setToolTip("Plays the emote " + GetEmote(8));
                            return;
                        }
                emoteDisplayMenu.OpenMenu();
            }));
            newEmoteMenu = new QMNestedButton("ShortcutMenu", 3964, 1852, "Emote", "Make Your Avatar Wave, Dance, and More");
            newEmoteMenu.getMainButton().DestroyMe();
            Emote1Button = new QMSingleButton(newEmoteMenu, 1, 0, "Emote 1", () => { Emote(1); }, "");
            Emote2Button = new QMSingleButton(newEmoteMenu, 2, 0, "Emote 2", () => { Emote(2); }, "");
            Emote3Button = new QMSingleButton(newEmoteMenu, 3, 0, "Emote 3", () => { Emote(3); }, "");
            Emote4Button = new QMSingleButton(newEmoteMenu, 4, 0, "Emote 4", () => { Emote(4); }, "");
            Emote5Button = new QMSingleButton(newEmoteMenu, 1, 1, "Emote 5", () => { Emote(5); }, "");
            Emote6Button = new QMSingleButton(newEmoteMenu, 2, 1, "Emote 6", () => { Emote(6); }, "");
            Emote7Button = new QMSingleButton(newEmoteMenu, 3, 1, "Emote 7", () => { Emote(7); }, "");
            Emote8Button = new QMSingleButton(newEmoteMenu, 4, 1, "Emote 8", () => { Emote(8); }, "");
        }
        public static void Emote(int emoteNumber)
        {
            if (VRCPlayer.field_Internal_Static_VRCPlayer_0 != null)
            {
                VRCPlayer.field_Internal_Static_VRCPlayer_0.TriggerEmote(emoteNumber);
            }
        }
        public static string GetEmote(int val)
        {
            if (VRCPlayer.field_Internal_Static_VRCPlayer_0.prop_VRCAvatarManager_0 != null && VRCPlayer.field_Internal_Static_VRCPlayer_0.prop_VRCAvatarManager_0.field_Private_Animator_0 != null)
            {
                AnimatorOverrideController animatorOverrideController = VRCPlayer.field_Internal_Static_VRCPlayer_0.prop_VRCAvatarManager_0.field_Private_Animator_0.runtimeAnimatorController.Cast<AnimatorOverrideController>();
                return animatorOverrideController[string.Format("EMOTE{0}", val)].name;
            }
            else
            {
                return QuickMenu._standEmotes[val-1];
            }
        }
    }
}
