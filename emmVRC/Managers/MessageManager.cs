using emmVRC.Libraries;
using emmVRC.Network;
using emmVRC.Network.Objects;
using System;
using System.Collections;
using System.Collections.Generic;
using emmVRC.Objects.ModuleBases;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VRC;
using VRC.Core;
using Transmtn;
using Transmtn.DTO;

#pragma warning disable 4014

// This class is moreso here as a novelty. Messaging isn't in emmVRC, and hasn't been for a fair bit.
// Maybe with the network rewrites, we can do it again...

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
    public class MessageManager : MelonLoaderEvents
    {
        private static List<PendingMessage> pendingMessages = new List<PendingMessage>();
        public static bool messageRead = true;
        /*
        public static IEnumerator SendMessage(string message, string targetId)
        {
            if (NetworkClient.webToken != null && Configuration.JSONConfig.emmVRCNetworkEnabled)
            {
                SerializableMessage msg = new SerializableMessage { body = message, recipient = targetId, icon = "None" };
                var request = HTTPRequest.post(NetworkClient.baseURL + "/api/message", msg);
                while (!request.IsCompleted && !request.IsFaulted)
                    yield return new WaitForEndOfFrame();

                if (request.IsFaulted)
                {
                    emmVRCLoader.Logger.LogError("Asynchronous net post failed: " + request.Exception);
                    NotificationManager.AddNotification("We could not send your message. Check to make sure you are connected to the emmVRC Network, or try again later.", "Dismiss", NotificationManager.DismissCurrentNotification, "", null, Resources.errorSprite, -1);
                }
            } else
            {
                NotificationManager.AddNotification("You must be connected to the emmVRC Network in order to send messages. We are sorry for the inconvenience.", "Dismiss", NotificationManager.DismissCurrentNotification, "", null, Resources.errorSprite, -1);
            }
        }*/
        public static IEnumerator CheckLoop()
        {
            yield return null;
            /*
            while (true)
            {
                if (NetworkClient.webToken != null)
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
                        try
                        {
                            NotificationManager.AddNotification("Message from " + Encoding.UTF8.GetString(Convert.FromBase64String(msg.message.rest_message_sender_name)) + ", sent " + new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc).AddSeconds(Double.Parse(msg.message.rest_message_created)).ToLocalTime().ToShortDateString() + " " + new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc).AddSeconds(Double.Parse(msg.message.rest_message_created)).ToLocalTime().ToShortTimeString() + "\n" + Encoding.UTF8.GetString(Convert.From
            String(msg.message.rest_message_body)), "Go to\nMessages", () =>
                            {
                                if (NetworkClient.webToken != null)
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
                                    VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowInputPopup("Send a message to " + Encoding.UTF8.GetString(Convert.FromBase64String(msg.message.rest_message_sender_name)) + ":", "", UnityEngine.UI.InputField.InputType.Standard, false, "Send", new System.Action<string, Il2CppSystem.Collections.Generic.List<UnityEngine.KeyCode>, UnityEngine.UI.Text>((string msg2, Il2CppSystem.Collections.Generic.List<UnityEngine.KeyCode> keyk, UnityEngine.UI.Text tx) =>
                                    {
                                        MelonLoader.MelonCoroutines.Start(SendMessage(msg2, msg.message.rest_message_sender_id));
                                    }), null, "Enter message....");
                                }
                            }, "Mark as\nRead", () =>
                            {
                                if (NetworkClient.webToken != null)
                                    try
                                    {
                                        HTTPRequest.patch(NetworkClient.baseURL + "/api/message/" + msg.message.rest_message_id, null);
                                    }
                                    catch (Exception ex)
                                    {
                                        emmVRCLoader.Logger.LogError(ex.ToString());
                                    }
                                NotificationManager.DismissCurrentNotification();
                            }, "Block\nUser", () => {
                                if (NetworkClient.webToken != null)
                                    try
                                    {
                                        HTTPRequest.patch(NetworkClient.baseURL + "/api/message/" + msg.message.rest_message_id, null);
                                        HTTPRequest.post(NetworkClient.baseURL + "/api/blocked/" + msg.message.rest_message_sender_id, null);
                                        VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowStandardPopup("emmVRC", "The block state for this user has been toggled.", "Okay", new System.Action(() => { VRCUiPopupManager.prop_VRCUiPopupManager_0.HideCurrentPopup(); }));
                                    }
                                    catch (Exception ex)
                                    {
                                        emmVRCLoader.Logger.LogError(ex.ToString());
                                    }
                                NotificationManager.DismissCurrentNotification();
                            }, Resources.messageSprite, -1);
                            msg.read = true;
                        }
                        catch (Exception ex)
                        {
                            ex = new Exception();
                        }
                }
                yield return new WaitForSeconds(Objects.NetworkConfig.Instance.MessageUpdateRate);
            }*/
        }
    }
}
