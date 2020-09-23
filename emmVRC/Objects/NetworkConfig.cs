﻿using System;
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
        public bool APICallsAllowed = false;
        public static NetworkConfig Instance = new NetworkConfig();
    }
}
