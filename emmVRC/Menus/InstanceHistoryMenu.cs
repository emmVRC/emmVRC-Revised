using emmVRC.Libraries;
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
        private static List<SerializedWorld> previousInstances;

        public static void Initialize()
        {
            baseMenu = new PaginatedMenu(FunctionsMenu.baseMenu.menuBase, 201945, 104894, "Instance\nHistory", "", null);
            baseMenu.menuEntryButton.DestroyMe();
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
                } catch (Exception ex)
                {
                    emmVRCLoader.Logger.LogError("Your instance history file is invalid. It will be wiped.");
                    File.Delete(Path.Combine(Environment.CurrentDirectory, "UserData/emmVRC/instancehistory.emm"));
                    previousInstances = new List<SerializedWorld>();
                }
            }
            foreach (SerializedWorld pastInstance in previousInstances)
            {
                if ((pastInstance.loggedDateTime - DateTime.Now).TotalDays > 1)
                {
                    previousInstances.Remove(pastInstance);
                }
                baseMenu.pageItems.Add(new PageItem(pastInstance.WorldName+"\n"+pastInstance.WorldOwner, () =>
                {
                    new PortalInternal().Method_Private_String_String_0(pastInstance.WorldID, pastInstance.WorldTags);
                }, pastInstance.WorldName+", hosted by "+pastInstance.WorldOwner+", last joined "+pastInstance.loggedDateTime.ToShortDateString()+" "+pastInstance.loggedDateTime.ToShortTimeString()+"\nSelect to join"));
            }
            if (previousInstances.Count == 0)
                baseMenu.pageItems.Add(PageItem.Space());
        }
        public static void SaveInstances()
        {
            File.WriteAllBytes(Path.Combine(Environment.CurrentDirectory, "UserData/emmVRC/instancehistory.emm"), Encoding.UTF8.GetBytes(TinyJSON.Encoder.Encode(previousInstances, TinyJSON.EncodeOptions.PrettyPrint)));
        }
        public static IEnumerator EnteredWorld()
        {
            while (RoomManager.field_ApiWorld_0 == null || RoomManager.field_ApiWorldInstance_0 == null)
                yield return new WaitForEndOfFrame();
            SerializedWorld world = new SerializedWorld
            {
                WorldID = RoomManager.field_ApiWorld_0.id,
                WorldTags = RoomManager.field_ApiWorldInstance_0.idWithTags,
                WorldOwner = RoomManager.field_ApiWorldInstance_0.GetInstanceCreator(),
                WorldType = RoomManager.field_ApiWorldInstance_0.InstanceType.ToString(),
                WorldName = RoomManager.field_ApiWorld_0.name,
                WorldImageURL = RoomManager.field_ApiWorld_0.thumbnailImageUrl,
                loggedDateTime = DateTime.Now
            };
            /*for (int i=0; i < previousInstances.Count; i++)
            {
                if (previousInstances[i].WorldID == world.WorldID && previousInstances[i].WorldTags == world.WorldTags)
                    previousInstances.RemoveAt(i);
            }*/
            previousInstances.Add(world);
            SaveInstances();
            baseMenu.pageItems.Add(new PageItem(world.WorldName+"\n"+world.WorldTags.Substring(0, world.WorldTags.IndexOf('~')), () =>
            {
                new PortalInternal().Method_Private_String_String_0(world.WorldID, world.WorldTags);
            }, world.WorldName + ", last joined " + world.loggedDateTime.ToShortDateString() + " " + world.loggedDateTime.ToShortTimeString() + "\nSelect to join"));
        }
    }
}
