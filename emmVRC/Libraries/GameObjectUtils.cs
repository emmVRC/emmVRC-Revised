using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace emmVRC.Libraries
{
    public static class GameObjectUtils
    {
        public static string GetPath(this GameObject obj)
        {
            var path = "/" + obj.name;
            while (obj.transform.parent != null)
            {
                obj = obj.transform.parent.gameObject;
                path = "/" + obj.name + path;
            }
            return path;
        }
        public static void GetChildren(this GameObject obj)
        {
            emmVRCLoader.Logger.LogDebug(obj.gameObject.GetPath());
            for (var i = 0; i < obj.transform.childCount; i++)
            {
                var child = obj.transform.GetChild(i);
                child.gameObject.GetChildren();
            }
        }
    }
}
