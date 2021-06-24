using emmVRC.Libraries;
using emmVRC.Menus;
using emmVRC.Network.Objects;
using Il2CppSystem.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VRC.Core;

namespace emmVRC.Managers
{
    public class UserPermissions
    {
        public string UserId;
        public bool GlobalDynamicBonesEnabled;
        public static UserPermissions GetUserPermissions(string UserId)
        {
            if (File.Exists(Path.Combine(Environment.CurrentDirectory, "UserData/emmVRC/UserPermissions/" + UserId + ".json"))){
                UserPermissions perms = TinyJSON.Decoder.Decode(File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "UserData/emmVRC/UserPermissions/" + UserId + ".json"))).Make<UserPermissions>();
                return perms;
            }
            else
            {
                UserPermissions perms = new UserPermissions { UserId = UserId};
                return perms;
            }
        }
        public void SaveUserPermissions()
        {
            File.WriteAllText(Path.Combine(Environment.CurrentDirectory, "UserData/emmVRC/UserPermissions/" + UserId + ".json"), TinyJSON.Encoder.Encode(this));
        }
    }
    public class UserPermissionManager
    {
        public static PaginatedMenu baseMenu;
        public static PageItem GlobalDynamicBonesEnabledToggle;

        public static UserPermissions selectedUserPermissions;
        public static void Initialize()
        {
            if (!Directory.Exists(Path.Combine(Environment.CurrentDirectory, "UserData/emmVRC/UserPermissions/")))
                Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, "UserData/emmVRC/UserPermissions/"));

            baseMenu = new PaginatedMenu(UserTweaksMenu.UserTweaks, 21304, 142394, "User\nPermissions", "", null);
            baseMenu.menuEntryButton.DestroyMe();
            GlobalDynamicBonesEnabledToggle = new PageItem("Global Dynamic\nBones", () => {
                selectedUserPermissions.GlobalDynamicBonesEnabled = true;
                selectedUserPermissions.SaveUserPermissions();
                ReloadUsers();
            }, "Disabled", () => {
                selectedUserPermissions.GlobalDynamicBonesEnabled = false;
                selectedUserPermissions.SaveUserPermissions();
                ReloadUsers();
            }, "TOGGLE: Enables Global Dynamic Bones for the selected user", true, true);
            baseMenu.pageItems.Add(GlobalDynamicBonesEnabledToggle);
            baseMenu.pageTitles.Add("User Options");
        }
        private static void ReloadUsers()
        {
            VRCPlayer.field_Internal_Static_VRCPlayer_0.ReloadAllAvatars();

            //VRCPlayer.field_Internal_Static_VRCPlayer_0.prop_VRCUserManager_0.Method_Private_Void_Boolean_1(false);
            //VRCPlayer.field_Internal_Static_VRCPlayer_0.prop_VRCUserManager_0.Method_Private_Void_Boolean_2(false);
        }
        public static void OpenMenu(string UserId)
        {
            selectedUserPermissions = UserPermissions.GetUserPermissions(UserId);
            GlobalDynamicBonesEnabledToggle.SetToggleState(selectedUserPermissions.GlobalDynamicBonesEnabled);
            baseMenu.OpenMenu();
        }
    }
}
