using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace emmVRC.Network.Objects
{
    public class Message : SerializedObject
    {
        string rest_message_id = "";
        string rest_message_sender_name = "";
        string rest_message_sender_id = "";
        string rest_message_body = "";
        string rest_message_icon = "";
        string rest_message_created = "";
    }
}
