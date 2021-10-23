using emmVRC.Libraries;
using emmVRC.Menus;
using emmVRC.Objects.ModuleBases;
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
    [Priority(50)]
    public class UserPermissionManager : MelonLoaderEvents
    {

        public static UserPermissions selectedUserPermissions;
        public override void OnUiManagerInit()
        {
            if (!Directory.Exists(Path.Combine(Environment.CurrentDirectory, "UserData/emmVRC/UserPermissions/")))
                Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, "UserData/emmVRC/UserPermissions/"));

        }
        private static void ReloadUsers()
        {
            VRCPlayer.field_Internal_Static_VRCPlayer_0.ReloadAllAvatars();

            //VRCPlayer.field_Internal_Static_VRCPlayer_0.prop_VRCUserManager_0.Method_Private_Void_Boolean_1(false);
            //VRCPlayer.field_Internal_Static_VRCPlayer_0.prop_VRCUserManager_0.Method_Private_Void_Boolean_2(false);
        }
        
    }
}
