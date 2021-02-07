using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRC.Core;
using VRC.UI;
using Logger = emmVRCLoader.Logger;

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
        
        public static void NoAwait(this Task task, string taskDescription = null)
        {
            task.ContinueWith(tsk =>
            {
                if (tsk.IsFaulted)
                    Logger.LogError($"Free-floating Task {(taskDescription == null ? "" : $"({taskDescription})")}}} failed with exception: {tsk.Exception}");
            });
        }
    }
}
