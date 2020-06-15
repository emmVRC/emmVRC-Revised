﻿using emmVRC.Libraries;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using emmVRC.Objects;
using emmVRC.Managers;
using UnityEngine;

namespace emmVRC.Menus
{
    public class InstanceHistoryMenu
    {
        public static PaginatedMenu baseMenu;
        private static QMSingleButton clearInstances;
        private static List<SerializedWorld> previousInstances;

        public static void Initialize()
        {
            baseMenu = new PaginatedMenu(FunctionsMenu.baseMenu.menuBase, 201945, 104894, "Instance\nHistory", "", null);
            baseMenu.menuEntryButton.DestroyMe();
            clearInstances = new QMSingleButton(baseMenu.menuBase, 5, 1, "<color=#FFCCBB>Clear\nInstances</color>", () => {
                VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowStandardPopup("Instance History", "Are you sure you want to clear the instance history? All previously joined instances will be lost!", "Yes", UnhollowerRuntimeLib.DelegateSupport.ConvertDelegate<Il2CppSystem.Action>((System.Action)(() => {
                    previousInstances.Clear();
                    SaveInstances();
                    LoadMenu();
                    VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup();
                })), "No", UnhollowerRuntimeLib.DelegateSupport.ConvertDelegate<Il2CppSystem.Action>((System.Action)(() => {
                    VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup();
                })));
            }, "Clears the instance history of all previous instances");
            previousInstances = new List<SerializedWorld>();
            if (!File.Exists(Path.Combine(Environment.CurrentDirectory, "UserData/emmVRC/instancehistory.emm")))
            {
                previousInstances = new List<SerializedWorld>();
                File.WriteAllBytes(Path.Combine(Environment.CurrentDirectory, "UserData/emmVRC/instancehistory.emm"), Encoding.UTF8.GetBytes(TinyJSON.Encoder.Encode(previousInstances, TinyJSON.EncodeOptions.PrettyPrint)));
            }
            else
            {
                string input = Encoding.UTF8.GetString(File.ReadAllBytes(Path.Combine(Environment.CurrentDirectory, "UserData/emmVRC/instancehistory.emm")));
                try
                {
                    previousInstances = TinyJSON.Decoder.Decode(input).Make<List<SerializedWorld>>();
                    SerializedWorld removeable = null;
                    foreach (SerializedWorld pastInstance in previousInstances)
                    {
                        if (UnixTime.ToDateTime(pastInstance.loggedDateTime) < DateTime.Now.AddDays(-1))
                        {
                            removeable = pastInstance;
                        }
                    }

                    if (removeable != null)
                    {
                        previousInstances.Remove(removeable);
                        SaveInstances();
                    }

                }
                catch (Exception ex)
                {
                    ex = new Exception();
                    emmVRCLoader.Logger.LogError("Your instance history file is invalid. It will be wiped.");
                    File.Delete(Path.Combine(Environment.CurrentDirectory, "UserData/emmVRC/instancehistory.emm"));
                    previousInstances = new List<SerializedWorld>();
                }
            }

        }
        public static void LoadMenu()
        {
            baseMenu.pageItems.Clear();
            foreach (SerializedWorld pastInstance in previousInstances) { 
                baseMenu.pageItems.Insert(0, new PageItem(pastInstance.WorldName + "\n" + InstanceIDUtilities.GetInstanceID(pastInstance.WorldTags), () =>
                {
                    new PortalInternal().Method_Private_Void_String_String_0(pastInstance.WorldID, pastInstance.WorldTags);
                }, pastInstance.WorldName + ", last joined " + UnixTime.ToDateTime(pastInstance.loggedDateTime).ToShortDateString() + " " + UnixTime.ToDateTime(pastInstance.loggedDateTime).ToShortTimeString() + "\nSelect to join"));
            }
        }
        public static void SaveInstances()
        {
            File.WriteAllBytes(Path.Combine(Environment.CurrentDirectory, "UserData/emmVRC/instancehistory.emm"), Encoding.UTF8.GetBytes(TinyJSON.Encoder.Encode(previousInstances, TinyJSON.EncodeOptions.PrettyPrint)));
        }
        public static IEnumerator EnteredWorld()
        {
            while (RoomManager.field_Internal_Static_ApiWorld_0 == null || RoomManager.field_Internal_Static_ApiWorldInstance_0 == null)
                yield return new WaitForEndOfFrame();
            try
            {
                SerializedWorld world = new SerializedWorld
                {
                    WorldID = RoomManager.field_Internal_Static_ApiWorld_0.id,
                    WorldTags = RoomManager.field_Internal_Static_ApiWorldInstance_0.idWithTags,
                    WorldOwner = RoomManager.field_Internal_Static_ApiWorldInstance_0.GetInstanceCreator(),
                    WorldType = RoomManager.field_Internal_Static_ApiWorldInstance_0.InstanceType.ToString(),
                    WorldName = RoomManager.field_Internal_Static_ApiWorld_0.name,
                    WorldImageURL = RoomManager.field_Internal_Static_ApiWorld_0.thumbnailImageUrl
                };
                SerializedWorld removeable = null;
                foreach (SerializedWorld previousInstance in previousInstances)
                {
                    if (previousInstance.WorldID == world.WorldID && InstanceIDUtilities.GetInstanceID(previousInstance.WorldTags) == InstanceIDUtilities.GetInstanceID(world.WorldTags))
                    {
                        removeable = previousInstance;
                    }
                }
                if (removeable != null)
                    previousInstances.Remove(removeable);
                previousInstances.Add(world);
                SaveInstances();
                LoadMenu();
            } catch (Exception ex)
            {
                emmVRCLoader.Logger.LogError(ex.ToString());
            }
        }
    }
}
