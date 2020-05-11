/*using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using emmVRC.Libraries;
using emmVRC.Objects;

namespace emmVRC.Menus
{
    public class InstanceHistory
    {
        private static List<HistInstance> PreviousInstances;
        private static PaginatedMenu menuBase;
        private static bool InInstance = false;
        public static void Initialize()
        {
            if (File.Exists(Path.Combine(System.Environment.CurrentDirectory, "UserData/emmVRC/instanceHistory.json")))
            {
                PreviousInstances = TinyJSON.Decoder.Decode(File.ReadAllText(Path.Combine(System.Environment.CurrentDirectory, "UserData/emmVRC/instanceHistory.json"))).Make<List<HistInstance>>();
                if (PreviousInstances == null)
                {
                    PreviousInstances = new List<HistInstance>();
                    emmVRCLoader.Logger.LogError("The instance history was null. Resetting...");
                }
            }
            else
            {
                PreviousInstances = new List<HistInstance>();
            }
            foreach(HistInstance instance in PreviousInstances)
            {
                if ((instance.lastVisited - DateTime.Now).TotalDays > 3)
                    PreviousInstances.Remove(instance);
            }
            menuBase = new PaginatedMenu(FunctionsMenu.baseMenu.menuBase, 20394, 10294, "Instance\nHistory", "Allows you to revisit previous instances after leaving or crashing", null);
            menuBase.menuEntryButton.DestroyMe();


        }
        public static void LoadMenu()
        {
            menuBase.OpenMenu();
            menuBase.pageItems.Clear();
            foreach(HistInstance inst in PreviousInstances)
            {
                menuBase.pageItems.Add(new PageItem(Encoding.UTF8.GetString(Convert.FromBase64String(inst.WorldNameB64)), () => {
                    VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowStandardPopup("emmVRC", "Go to the world \"" + Encoding.UTF8.GetString(Convert.FromBase64String(inst.WorldNameB64)) + "\"?", "Yes", UnhollowerRuntimeLib.DelegateSupport.ConvertDelegate<Il2CppSystem.Action>((System.Action)(() => {
                        VRCFlowManager.field_VRCFlowManager_0.Method_Public_String_String_Type13749128_Action_1_String_Boolean_0(inst.WorldID, Encoding.UTF8.GetString(Convert.FromBase64String(inst.InstanceID)));
                        emmVRCLoader.Logger.LogDebug(inst.InstanceID);
                        VRCFlowManager.field_VRCFlowManager_0.Method_Public_String_String_Type13749128_Action_1_String_Boolean_0(inst.WorldID, inst.InstanceID);
                        VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup();
                    })), "No", UnhollowerRuntimeLib.DelegateSupport.ConvertDelegate<Il2CppSystem.Action>((System.Action)(() => {
                        VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup();                   
                    })));
                }, "Instance Owner: "+Encoding.UTF8.GetString(Convert.FromBase64String(inst.InstanceOwnerNameB64))+"\nLast visited: "+inst.lastVisited.ToShortDateString()));
            }
        }
        public static void OnUpdate()
        {
            if (!InInstance && RoomManager.field_Internal_Static_ApiWorld_0 != null)
            {
                PreviousInstances.Add(new HistInstance
                {
                    WorldNameB64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(RoomManager.field_Internal_Static_ApiWorld_0.name)),
                    WorldID = RoomManager.field_Internal_Static_ApiWorld_0.id,
                    InstanceID = VRC.Core.APIUser.CurrentUser.location,
                    //InstanceID = Convert.ToBase64String(Encoding.UTF8.GetBytes(RoomManager.field_Internal_Static_ApiWorld_0.instanceId)),
                    //InstanceOwnerNameB64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(RoomManager.field_Internal_Static_ApiWorldInstance_0.instanceCreator)),
                    InstancePrivacy = (RoomManager.field_Internal_Static_ApiWorldInstance_0.InstanceType == VRC.Core.ApiWorldInstance.AccessType.InviteOnly ? "invite" : (RoomManager.field_Internal_Static_ApiWorldInstance_0.InstanceType == VRC.Core.ApiWorldInstance.AccessType.InvitePlus ? "invite+" : (RoomManager.field_Internal_Static_ApiWorldInstance_0.InstanceType == VRC.Core.ApiWorldInstance.AccessType.FriendsOnly ? "friends" : RoomManager.field_Internal_Static_ApiWorldInstance_0.InstanceType == VRC.Core.ApiWorldInstance.AccessType.FriendsOfGuests ? "friends+" : "public")))
                });
                InInstance = true;

            }
            else if (InInstance && RoomManager.field_Internal_Static_ApiWorld_0 == null)
                InInstance = false;
        }
    }
}
*/