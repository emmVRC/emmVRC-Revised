using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using emmVRC.Utils;
using emmVRC.Components;
using emmVRC.Objects.ModuleBases;
using UnityEngine;
using UnityEngine.UI;
using VRC.DataModel;
using VRC.DataModel.Core;
using VRC.Core;
using VRC;

namespace emmVRC.Menus
{
    [Priority(55)]
    public class SelectedUserLocalMenu : MelonLoaderEvents
    {
        private static ButtonGroup selectedUserGroup;
        private static SimpleSingleButton avatarOptionsButton;
        private static SimpleSingleButton playerNotesButton;
        private static SimpleSingleButton teleportButton;
        private static SimpleSingleButton favouriteAvatarButton;

        private static bool _initialized = false;
        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            if (buildIndex != -1 || _initialized) return;
            Transform baseMenuTransform = ButtonAPI.menuPageBase.transform.parent.Find("Menu_SelectedUser_Local/ScrollRect/Viewport/VerticalLayoutGroup");
            selectedUserGroup = new ButtonGroup(baseMenuTransform, "emmVRC Actions");
            avatarOptionsButton = new SimpleSingleButton(selectedUserGroup, "Avatar\nOptions", () => {
                APIUser selectedAPIUser = UserSelectionManager.field_Private_Static_UserSelectionManager_0.field_Private_APIUser_1;
                if (selectedAPIUser == null)
                    selectedAPIUser = UserSelectionManager.field_Private_Static_UserSelectionManager_0.field_Private_APIUser_0;
                if (selectedAPIUser == null)
                    selectedAPIUser = APIUser.CurrentUser;
                if (selectedAPIUser == null) return;
                VRCPlayer selectedVRCPlayer = PlayerManager.field_Private_Static_PlayerManager_0.field_Private_List_1_Player_0.ToArray().FirstOrDefault(a => a.field_Private_APIUser_0 != null && a.field_Private_APIUser_0.id == selectedAPIUser.id)?._vrcplayer;
                AvatarOptionsMenu.OpenMenu(selectedVRCPlayer != null ? selectedVRCPlayer : VRCPlayer.field_Internal_Static_VRCPlayer_0);
            }, "Shows various options for the selected avatar, including toggling components and global dynamic bones");
            playerNotesButton = new SimpleSingleButton(selectedUserGroup, "Player\nNotes", () => {
                APIUser selectedUser = (UserSelectionManager.field_Private_Static_UserSelectionManager_0.field_Private_APIUser_1 == null ? APIUser.CurrentUser : UserSelectionManager.field_Private_Static_UserSelectionManager_0.field_Private_APIUser_1);
                Functions.UI.PlayerNotes.LoadNoteQM(selectedUser.id, selectedUser.GetName());
                }, "View the notes for the selected player");
            teleportButton = new SimpleSingleButton(selectedUserGroup, "Teleport", () => {
                VRC.Player selectedPlayer = PlayerManager.field_Private_Static_PlayerManager_0.field_Private_List_1_Player_0.ToArray().FirstOrDefault(a => a.field_Private_APIUser_0 != null && a.field_Private_APIUser_0.id == UserSelectionManager.field_Private_Static_UserSelectionManager_0.field_Private_APIUser_1.id);
                if (selectedPlayer == null) return;
                if (Configuration.JSONConfig.RiskyFunctionsEnabled && Managers.RiskyFunctionsManager.AreRiskyFunctionsAllowed)
                    VRCPlayer.field_Internal_Static_VRCPlayer_0.field_Private_VRCPlayerApi_0.TeleportTo(selectedPlayer.transform.position, selectedPlayer.transform.rotation);
            }, "Teleport to the selected player (requires Risky Functions to be enabled and allowed)");
            Components.EnableDisableListener listener = teleportButton.gameObject.AddComponent<EnableDisableListener>();
            listener.OnEnabled += () =>
            {
                teleportButton.SetInteractable(Managers.RiskyFunctionsManager.AreRiskyFunctionsAllowed && Configuration.JSONConfig.RiskyFunctionsEnabled);
            };
            favouriteAvatarButton = new SimpleSingleButton(selectedUserGroup, "Favorite\nAvatar", () => {
                if (!Utils.PlayerUtils.DoesUserHaveVRCPlus())
                    ButtonAPI.GetQuickMenuInstance().ShowOKDialog("VRChat Plus Required", Objects.Attributes.VRCPlusMessage);
                else
                {
                    bool flag = false;
                    APIUser selectedAPIUser = UserSelectionManager.field_Private_Static_UserSelectionManager_0.field_Private_APIUser_1;
                    if (selectedAPIUser == null)
                        selectedAPIUser = UserSelectionManager.field_Private_Static_UserSelectionManager_0.field_Private_APIUser_0;
                    if (selectedAPIUser == null) return;
                    VRCPlayer selectedVRCPlayer = PlayerManager.field_Private_Static_PlayerManager_0.field_Private_List_1_Player_0.ToArray().FirstOrDefault(a => a.field_Private_APIUser_0 != null && a.field_Private_APIUser_0.id == selectedAPIUser.id)?._vrcplayer;
                    if (selectedVRCPlayer == null) return;

                    if (selectedAPIUser.allowAvatarCopying && selectedVRCPlayer._player.prop_ApiAvatar_0.releaseStatus == "public")
                    {
                        foreach (ApiAvatar avtr in Functions.UI.CustomAvatarFavorites.LoadedAvatars)
                        {
                            if (avtr.id == selectedVRCPlayer._player.prop_ApiAvatar_0.id)
                                flag = true;
                        }
                        if (flag)
                            ButtonAPI.GetQuickMenuInstance().ShowOKDialog("emmVRC", "You already have this avatar favorited");
                        else
                            Functions.UI.CustomAvatarFavorites.FavoriteAvatar(selectedVRCPlayer._player.prop_ApiAvatar_0).NoAwait(nameof(Functions.UI.CustomAvatarFavorites.FavoriteAvatar));
                    }
                    else
                        ButtonAPI.GetQuickMenuInstance().ShowOKDialog("emmVRC", "This avatar is not public, or the user does not have cloning turned on.");
                }
            }, "Favorite this user's avatar to your emmVRC Favorite list (Requires VRC+ and cloning to be on)");
            _initialized = true;
        }
    }
}
