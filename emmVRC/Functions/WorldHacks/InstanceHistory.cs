using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using emmVRC.Libraries;
using emmVRC.Objects;
using emmVRC.Objects.ModuleBases;
using UnityEngine;

namespace emmVRC.Functions.WorldHacks
{
    public class InstanceHistory : MelonLoaderEvents
    {
        internal static List<SerializedWorld> previousInstances;
        public override void OnUiManagerInit()
        {
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
                    previousInstances.RemoveAll(a => UnixTime.ToDateTime(a.loggedDateTime) < DateTime.Now.AddDays(-3));
                    SaveInstances();
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
        public static void SaveInstances()
        {
            File.WriteAllBytes(Path.Combine(Environment.CurrentDirectory, "UserData/emmVRC/instancehistory.emm"), Encoding.UTF8.GetBytes(TinyJSON.Encoder.Encode(previousInstances, TinyJSON.EncodeOptions.PrettyPrint)));
        }
        public static void ClearInstances()
        {
            previousInstances.Clear();
            File.WriteAllBytes(Path.Combine(Environment.CurrentDirectory, "UserData/emmVRC/instancehistory.emm"), Encoding.UTF8.GetBytes(TinyJSON.Encoder.Encode(previousInstances, TinyJSON.EncodeOptions.PrettyPrint)));
        }
        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            if (buildIndex == -1)
                MelonLoader.MelonCoroutines.Start(EnteredWorld());
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
                    WorldTags = RoomManager.field_Internal_Static_ApiWorldInstance_0.instanceId,
                    WorldOwner = (RoomManager.field_Internal_Static_ApiWorldInstance_0.ownerId == null ? "" : RoomManager.field_Internal_Static_ApiWorldInstance_0.ownerId),
                    WorldType = RoomManager.field_Internal_Static_ApiWorldInstance_0.type.ToString(),
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
                Functions.WorldHacks.InstanceHistory.SaveInstances();
            }
            catch (Exception ex)
            {
                emmVRCLoader.Logger.LogError(ex.ToString());
            }
        }
        public static string PrettifyInstanceType(string original)
        {
            switch (original)
            {
                default:
                    return original;
                case "FriendsOfGuests":
                    return "Friends+";
                case "FriendsOnly":
                    return "Friends";
                case "InvitePlus":
                    return "Invite+";
                case "InviteOnly":
                    return "Invite";
            }
        }
    }
}
