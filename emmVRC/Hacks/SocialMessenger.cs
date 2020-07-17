using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using emmVRC.Libraries;
using emmVRC.Managers;
using emmVRC.Network.Objects;

namespace emmVRC.Hacks
{
    public class SocialMessage
    {
        public string UserID;
        public string MessageText;
    }

    public class SocialMessenger
    {
        public static void Initialize() { }

        public static void OpenText(string userID, string displayName)
        {
            try
            {
                VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowInputPopup("Send message to " + displayName + ":", "", UnityEngine.UI.InputField.InputType.Standard, false, "Send", new System.Action<string, Il2CppSystem.Collections.Generic.List<UnityEngine.KeyCode>, UnityEngine.UI.Text>((string newMessageText, Il2CppSystem.Collections.Generic.List<UnityEngine.KeyCode> keyk, UnityEngine.UI.Text tx) =>
                {
                    SocialMessage socialMessage = new SocialMessage { UserID = userID, MessageText = newMessageText };
                    SendMessage(socialMessage);

                }), null, "Enter message....");
            }
            catch (Exception ex)
            {
                emmVRCLoader.Logger.LogError("Failed to send message: " + ex.ToString());
            }
        }

        public static void OpenConversation(string userID, string displayName, Message[] messageConvo)
        {
            try
            {
                //TODO change to use arrows to go up and down
                VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowStandardPopup("Conversation with " + Encoding.UTF8.GetString(Convert.FromBase64String(displayName)), messageConvo.Length == 0 ? "No messages with this user." : loadContext(messageConvo), "Send Message", () =>
                {
                    OpenText(userID, Encoding.UTF8.GetString(Convert.FromBase64String(displayName)));
                });

            }
            catch (Exception ex)
            {
                emmVRCLoader.Logger.LogError("Failed to send message: " + ex.ToString());
            }
        }

        private static string loadContext(Message[] convo)
        {
            string spacer = "    ";
            string newLine = "\n";
            string context = "";

            //TODO showing in wrong order UGH
            foreach (Message message in convo)
            {
                context += "[ " + message.rest_message_created + " ]" + spacer + Encoding.UTF8.GetString(Convert.FromBase64String(message.rest_message_sender_name)) + spacer + newLine + spacer + Encoding.UTF8.GetString(Convert.FromBase64String(message.rest_message_body)) + newLine;
            }
            //for (int i = convo.Length - 1; i < convo.Length - 1; i++)
            //{
            //    context += "[ "+ convo[i].rest_message_created + " ]" + spacer + Encoding.UTF8.GetString(Convert.FromBase64String(convo[i].rest_message_sender_name)) + spacer + newLine + spacer + Encoding.UTF8.GetString(Convert.FromBase64String(convo[i].rest_message_body)) + newLine;
            //}
            return context;
        }

        public static void SendMessage(SocialMessage message)
        {
            MessageManager.SendMessage(message.MessageText, message.UserID);
        }

    }
}