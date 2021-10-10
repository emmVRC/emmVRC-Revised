using System;
using System.Linq;
using System.Reflection;
using UnhollowerRuntimeLib.XrefScans;

namespace emmVRC.Utils
{
    public static class XrefUtils
    {
        public static bool CheckMethod(MethodInfo method, string match)
        {
            try
            {
                return XrefScanner.XrefScan(method)
                    .Any(instance => instance.Type == XrefType.Global && instance.ReadAsObject().ToString().Contains(match));
            }
            catch { }
            return false;
        }

        public static bool CheckUsedBy(MethodInfo method, string methodName, Type type = null)
        {
            foreach (XrefInstance instance in XrefScanner.UsedBy(method))
            {
                if (instance.Type == XrefType.Method)
                {
                    try
                    {
                        if ((type == null || instance.TryResolve().DeclaringType == type) && instance.TryResolve().Name.Contains(methodName))
                            return true;
                    }
                    catch
                    {

                    }
                }
            }
            return false;
        }

        public static bool CheckUsing(MethodInfo method, string methodName, Type type = null)
        {
            foreach (XrefInstance instance in XrefScanner.XrefScan(method))
            {
                if (instance.Type == XrefType.Method)
                {
                    try
                    {
                        if ((type == null || instance.TryResolve().DeclaringType == type) && instance.TryResolve().Name.Contains(methodName))
                            return true;
                    }
                    catch
                    {

                    }
                }
            }
            return false;
        }
    }
}