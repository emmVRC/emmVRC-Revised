/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnhollowerRuntimeLib;
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
        public static void GetProperties(object comp)
        {
            System.Reflection.PropertyInfo[] properties = comp.GetType().GetProperties();
            //Get all the properties of your class.
            foreach (System.Reflection.PropertyInfo pI in properties)
            {
                emmVRCLoader.Logger.LogDebug("Property name: " + pI.Name);
                emmVRCLoader.Logger.LogDebug("Property type: " + pI.PropertyType.Name);
                if (pI.PropertyType == typeof(GameObject))
                {
                    emmVRCLoader.Logger.LogDebug("Property path: " + ((GameObject)pI.GetValue(comp)).GetPath());
                }
                else if (pI.PropertyType == typeof(Transform))
                {
                    emmVRCLoader.Logger.LogDebug("Property path: " + ((Transform)pI.GetValue(comp)).gameObject.GetPath());
                }
                else
                {
                    emmVRCLoader.Logger.LogDebug("Property value: " + pI.GetValue(comp));
                }
            }
        }
        
        public static void CompareProperties(object comp1, object comp2)
        {
            if (comp1.GetType() != comp2.GetType())
            {
                emmVRCLoader.Logger.LogError("Error during compare operation: Component types do not match.");
                return;
            }
            System.Reflection.PropertyInfo[] properties1 = comp1.GetType().GetProperties();
            System.Reflection.PropertyInfo[] properties2 = comp2.GetType().GetProperties();
            List<object> comp1PropertyValues = new List<object>();
            List<object> comp2PropertyValues = new List<object>();
            foreach (System.Reflection.PropertyInfo pI in properties1)
            {
                comp1PropertyValues.Add(pI.GetValue(comp1));
            }
            foreach (System.Reflection.PropertyInfo pI in properties2)
            {
                comp2PropertyValues.Add(pI.GetValue(comp2));
            }
            for (int i=0; i < comp1PropertyValues.Count; i++)
            {
                if (comp1PropertyValues[i] != null && comp2PropertyValues[i] != null && comp1PropertyValues[i].ToString() != comp2PropertyValues[i].ToString())
                {
                    emmVRCLoader.Logger.LogDebug("Difference detected:");
                    emmVRCLoader.Logger.LogDebug("Name: " + properties1[i].Name);
                    //emmVRCLoader.Logger.LogDebug("Type: " + properties1[i].GetType().Name);
                    emmVRCLoader.Logger.LogDebug("Value 1: " + comp1PropertyValues[i]);
                    emmVRCLoader.Logger.LogDebug("Value 2: " + comp2PropertyValues[i]);
                }
            }

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
*/