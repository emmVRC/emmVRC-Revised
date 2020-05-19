using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace emmVRC.Libraries
{
    public class InstanceIDUtilities
    {
        public static string GetInstanceID(string baseID)
        {
            if (baseID.Contains('~'))
                return baseID.Substring(0, baseID.IndexOf('~'));
            else
                return baseID;

        }
    }
}
