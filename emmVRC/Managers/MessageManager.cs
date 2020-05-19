using emmVRC.Libraries;
using emmVRC.Network;
using emmVRC.Network.Objects;
using Il2CppSystem.Xml.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VRC;
using VRC.Core;
using Transmtn;
using Transmtn.DTO;



namespace emmVRC.Managers
{
    public class PendingMessage
    {
        public Message message;
        public bool read = false;
    }
    public class SerializableMessage
    {
        public string recipient;
        public string body;
        public string icon;
    }
    public class MessageManager
    {
        private static List<PendingMessage> pendingMessages = new List<PendingMessage>();
        public static bool messageRead = true;
        public static void Initialize()
        {
            MelonLoader.MelonCoroutines.Start(CheckLoop());
        }
        public static IEnumerator SendMessage(string message, string targetId)
        {
            if (NetworkClient.authToken != null && !Configuration.JSONConfig.AutoInviteMessage && Configuration.JSONConfig.emmVRCNetworkEnabled)
            {
                SerializableMessage msg = new SerializableMessage { body = message, recipient = targetId, icon = "None" };
                var request = HTTPRequest.post(NetworkClient.baseURL + "/api/message", msg);
                while (!request.IsCompleted && !request.IsFaulted)
                    yield return new WaitForEndOfFrame();

                if (request.IsFaulted)
                {
                    emmVRCLoader.Logger.LogError("Asynchronous net post failed: " + request.Exception);
                    VRCWebSocketsManager.field_Private_Static_VRCWebSocketsManager_0.field_Private_Api_0.PostOffice.Send(Transmtn.DTO.Notifications.Invite.Create(targetId, "", new Location("", new Transmtn.DTO.Instance("", "", "", "", "", false)), "message from " + APIUser.CurrentUser.displayName + ", sent " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + ":\n" + message));
                }
            } else
            {
                VRCWebSocketsManager.field_Private_Static_VRCWebSocketsManager_0.field_Private_Api_0.PostOffice.Send(Transmtn.DTO.Notifications.Invite.Create(targetId, "", new Location("", new Transmtn.DTO.Instance("", "", "", "", "", false)), "message from "+APIUser.CurrentUser.displayName+", sent "+DateTime.Now.ToShortDateString() + " " +  DateTime.Now.ToShortTimeString()+":\n"+message));
            }
        }
        public static IEnumerator CheckLoop()
        {
            while (true)
            {
                if (NetworkClient.authToken != null)
                {
                    Message[] messageArray = null;

                    var thing = HTTPRequest.get(NetworkClient.baseURL + "/api/message");
                    while (!thing.IsCompleted && !thing.IsFaulted)
                        yield return new WaitForEndOfFrame();

                    if (!thing.IsFaulted)
                    {
                        try
                        {
                            messageArray = TinyJSON.Decoder.Decode(thing.Result).Make<Message[]>();
                        }
                        catch (System.Exception ex)
                        {
                            emmVRCLoader.Logger.LogError(ex.ToString());
                        }
                        if (messageArray != null)
                        {
                            foreach (Message msg in messageArray)
                            {
                                if (pendingMessages.FindIndex(a => a.message.rest_message_id == msg.rest_message_id) == -1)
                                    pendingMessages.Add(new PendingMessage { message = msg, read = false });
                            }
                        }
                    } else
                    {
                        emmVRCLoader.Logger.LogError("Asynchronous net request failed: " + thing.Exception);
                    }
                }
                foreach (PendingMessage msg in pendingMessages)
                {
                    if (!msg.read)
                    NotificationManager.AddNotification("Message from " + Encoding.UTF8.GetString(Convert.FromBase64String(msg.message.rest_message_sender_name)) + ", sent " + new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc).AddSeconds(Double.Parse(msg.message.rest_message_created)).ToLocalTime().ToShortDateString() + " " + new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc).AddSeconds(Double.Parse(msg.message.rest_message_created)).ToLocalTime().ToShortTimeString() + "\n" + Encoding.UTF8.GetString(Convert.FromBase64String(msg.message.rest_message_body)), "Go to\nMessages", () => {
                        if (NetworkClient.authToken != null)
                        {
                            try
                            {
                                HTTPRequest.patch(NetworkClient.baseURL + "/api/message/" + msg.message.rest_message_id, null);
                                //HTTPRequest.patch_sync(NetworkClient.baseURL + "/api/message/" + msg.message.rest_message_id, null);
                            }
                            catch (Exception ex)
                            {
                                emmVRCLoader.Logger.LogError(ex.ToString());
                            }
                            NotificationManager.DismissCurrentNotification();
                            InputUtilities.OpenInputBox("Send a message to " + Encoding.UTF8.GetString(Convert.FromBase64String(msg.message.rest_message_sender_name)), "Send", (string msg2) => {
                                SendMessage(msg2, msg.message.rest_message_sender_id);
                            });
                        }
                    }, "Mark as\nRead", () => {
                        if (NetworkClient.authToken != null)
                            try
                            {
                                HTTPRequest.patch(NetworkClient.baseURL + "/api/message/" + msg.message.rest_message_id, null);
                            } catch (Exception ex)
                            {
                                emmVRCLoader.Logger.LogError(ex.ToString());
                            }
                        NotificationManager.DismissCurrentNotification();
                    }, Resources.messageSprite, -1);
                    msg.read = true;
                }
                yield return new WaitForSeconds(15f);
            }
        }
    }
}
