using emmVRC.Hacks;
using emmVRC.Libraries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;

namespace emmVRC.Menus
{
    public class UserTweaksMenu
    {
        public static QMNestedButton UserTweaks;
        public static QMSingleButton PlayerNotesButton;
        public static QMSingleButton TeleportButton;
        public static void Initialize()
        {
            UserTweaks = new QMNestedButton("UserInteractMenu", 4, 2, "<color=#FF69B4>emmVRC</color>\nActions", "Provides functions and tweaks with the selected user");
            PlayerNotesButton = new QMSingleButton(UserTweaks, 1, 0, "Player\nNotes", () => { PlayerNotes.LoadNote(QuickMenuUtils.GetQuickMenuInstance().field_Private_APIUser_0.id, QuickMenuUtils.GetQuickMenuInstance().field_Private_APIUser_0.displayName); }, "Allows you to write a note for the specified user");
            TeleportButton = new QMSingleButton(UserTweaks, 2, 0, "Teleport\nto player", () => { if (Configuration.JSONConfig.RiskyFunctionsEnabled) VRCPlayer.field_Internal_Static_VRCPlayer_0.transform.position = QuickMenuUtils.GetQuickMenuInstance().field_Private_VRCPlayer_0.transform.position; }, "Teleports to the selected player. Requires risky functions");
        }
        public static void SetRiskyFunctions(bool status)
        {
            TeleportButton.getGameObject().GetComponent<Button>().enabled = status;
        }
    }
}
