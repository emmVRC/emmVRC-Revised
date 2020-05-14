using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace emmVRC.Network.Objects
{
    public class Blocked : SerializedObject
    {
        string blocked_target_user_id = "";
        string blocked_expire_date = "";
        string blocked_created_date = "";
    }
}
