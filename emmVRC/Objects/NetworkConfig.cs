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
        public static NetworkConfig Instance = new NetworkConfig();
    }
}
