﻿using emmVRC.Hacks;
using emmVRC.Libraries;
using emmVRC.Managers;
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
        public static QMSingleButton AvatarPermissions;
        public static QMSingleButton UserPermissions;
        public static QMSingleButton PlayerNotesButton;
        public static QMSingleButton TeleportButton;
        public static QMSingleButton SendMessageButton;
        public static void Initialize()
        {
            UserTweaks = new QMNestedButton("UserInteractMenu", Configuration.JSONConfig.PlayerActionsButtonX, Configuration.JSONConfig.PlayerActionsButtonY, "<color=#FF69B4>emmVRC</color>\nActions", "Provides functions and tweaks with the selected user");
            AvatarPermissions = new QMSingleButton(UserTweaks, 1, 0, "Avatar\nOptions", () => { AvatarPermissionManager.OpenMenu(QuickMenuUtils.GetQuickMenuInstance().field_Private_VRCPlayer_0.prop_ApiAvatar_0.id, true); }, "Allows you to configure permissions for this user's avatar, which includes dynamic bone settings");
            UserPermissions = new QMSingleButton(UserTweaks, 2, 0, "User\nOptions", () => { UserPermissionManager.OpenMenu(QuickMenuUtils.GetQuickMenuInstance().field_Private_APIUser_0.id); }, "Allows you to configure permissions for this user, which includes enabling Global Dynamic Bones");
            PlayerNotesButton = new QMSingleButton(UserTweaks, 3, 0, "Player\nNotes", () => { PlayerNotes.LoadNote(QuickMenuUtils.GetQuickMenuInstance().field_Private_APIUser_0.id, QuickMenuUtils.GetQuickMenuInstance().field_Private_APIUser_0.displayName); }, "Allows you to write a note for the specified user");
            TeleportButton = new QMSingleButton(UserTweaks, 4, 0, "Teleport\nto player", () => { if (Configuration.JSONConfig.RiskyFunctionsEnabled) VRCPlayer.field_Internal_Static_VRCPlayer_0.transform.position = QuickMenuUtils.GetQuickMenuInstance().field_Private_VRCPlayer_0.transform.position; }, "Teleports to the selected player. Requires risky functions");
            SendMessageButton = new QMSingleButton(UserTweaks, 1, 1, "Send\nMessage", () =>
            {
                string targetId = QuickMenuUtils.GetQuickMenuInstance().field_Private_APIUser_0.id;
                VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowInputPopup("Send a message to " + QuickMenuUtils.GetQuickMenuInstance().field_Private_APIUser_0.displayName + ":", "", UnityEngine.UI.InputField.InputType.Standard, false, "Send", new System.Action<string, Il2CppSystem.Collections.Generic.List<UnityEngine.KeyCode>, UnityEngine.UI.Text>((string msg, Il2CppSystem.Collections.Generic.List<UnityEngine.KeyCode> keyk, UnityEngine.UI.Text tx) => {
                    MelonLoader.MelonCoroutines.Start(MessageManager.SendMessage(msg, targetId));
                }), null, "Enter message....");
            }, "Send a message to this player, either through emmVRC Network, or invites");
        }
        public static void SetRiskyFunctions(bool status)
        {
            TeleportButton.getGameObject().GetComponent<Button>().enabled = status;
        }
    }
}
