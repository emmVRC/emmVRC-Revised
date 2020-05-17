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
        public static void SendMessage(string message, string targetId)
        {
            if (NetworkClient.authToken != null)
            {
                SerializableMessage msg = new SerializableMessage { body = message, recipient = targetId, icon = ""};
                HTTPRequest.post_sync(NetworkClient.baseURL + "/api/message", TinyJSON.Encoder.Encode(msg, TinyJSON.EncodeOptions.NoTypeHints));
            }
        }
        public static IEnumerator CheckLoop()
        {
            while (true)
            {
                if (NetworkClient.authToken != null)
                {
                    Message[] messageArray = null;
                    try
                    {
                        messageArray = TinyJSON.Decoder.Decode(HTTPRequest.get_sync(NetworkClient.baseURL + "/api/message")).Make<Message[]>();
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
                }
                foreach (PendingMessage msg in pendingMessages)
                {
                    if (!msg.read)
                    NotificationManager.AddNotification("Message from " + Encoding.UTF8.GetString(Convert.FromBase64String(msg.message.rest_message_sender_name)) + "\nSent " + msg.message.rest_message_created + "\n" + Encoding.UTF8.GetString(Convert.FromBase64String(msg.message.rest_message_body)), "Go to\nMessages", () => { }, "Mark as\nRead", () => {
                        if (NetworkClient.authToken != null)
                            HTTPRequest.patch_sync(NetworkClient.baseURL + "/api/message/" + msg.message.rest_message_id, null);
                        NotificationManager.DismissCurrentNotification();
                    }, Resources.messageSprite, -1);
                    msg.read = true;
                }
                yield return new WaitForSeconds(15f);
            }
        }
    }
}
