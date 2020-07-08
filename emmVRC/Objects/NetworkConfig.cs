using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace emmVRC.Objects
{
    public class NetworkConfig
    {
        public int MessageUpdateRate = 15;
        public bool DisableAuthFile = false;
        public bool DeleteAndDisableAuthFile = false;
        public bool DisableAvatarChecks = false;
        public static NetworkConfig Instance = new NetworkConfig();
    }
}
