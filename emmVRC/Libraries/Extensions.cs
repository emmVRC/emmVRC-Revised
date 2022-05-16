using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRC.Core;
using VRC.UI;
using System.Reflection;
using UnityEngine;
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
        public static T GetCopyOf<T>(this Component comp, T other) where T : Component
        {
            Type type = comp.GetType();
            if (type != other.GetType()) return null; // type mis-match
            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default | BindingFlags.DeclaredOnly;
            PropertyInfo[] pinfos = type.GetProperties(flags);
            foreach (var pinfo in pinfos)
            {
                if (pinfo.CanWrite)
                {
                    try
                    {
                        pinfo.SetValue(comp, pinfo.GetValue(other, null), null);
                    }
                    catch { }
                }
            }
            FieldInfo[] finfos = type.GetFields(flags);
            foreach (var finfo in finfos)
            {
                finfo.SetValue(comp, finfo.GetValue(other));
            }
            return comp as T;
        }
        public static T AddComponent<T>(this GameObject go, T toAdd) where T : Component
        {
            return go.AddComponent<T>().GetCopyOf(toAdd) as T;
        }

    }
}
