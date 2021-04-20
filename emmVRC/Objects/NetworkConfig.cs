using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace emmVRC.Objects
{
    public class NetworkConfig
    {
        public int MessageUpdateRate = 60;
        public bool DisableAuthFile = false;
        public bool DeleteAndDisableAuthFile = false;
        public bool DisableAvatarChecks = false;
        public bool APICallsAllowed = true;
        public bool MessagingAllowed = true;
        public int NetworkAutoRetries = 0;
        public bool VRCPlusRequired = false;
        public static NetworkConfig Instance = new NetworkConfig();
    }
}
