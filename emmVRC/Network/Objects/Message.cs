using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace emmVRC.Network.Objects
{
    public class Message : SerializedObject
    {
        public string rest_message_id = "";
        public string rest_message_sender_name = "";
        public string rest_message_sender_id = "";
        public string rest_message_body = "";
        public string rest_message_icon = "";
        public string rest_message_created = "";
    }
}
