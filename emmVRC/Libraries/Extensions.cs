using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRC.Core;
using VRC.UI;

namespace emmVRC
{
    public static class Extensions
    {
        public static bool HasCustomName(this APIUser user)
        {
            return !string.IsNullOrWhiteSpace(user.displayName);
        }
        public static string GetName(this APIUser user)
        {
            if (user.HasCustomName())
                return user.displayName;
            else
                return user.username;
        }
    }
}
