using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace emmVRC.Objects
{
    public class HistInstance
    {
        public string WorldNameB64 = "";
        public string WorldID = "";
        public string InstanceID = "";
        public string InstanceOwnerNameB64 = "";
        public string InstanceOwnerID = "";
        public string InstancePrivacy = "invite+";
        public DateTime lastVisited = DateTime.Now;
    }
}
