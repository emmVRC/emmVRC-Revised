using System;
using System.Linq;
using System.Reflection;
using UnhollowerRuntimeLib.XrefScans;
using UnhollowerBaseLib.Attributes;
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

        public static Type GetTypeFromObfuscatedName(string obfuscatedName) => AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .First(type => type.GetCustomAttribute<ObfuscatedNameAttribute>() != null && type.GetCustomAttribute<ObfuscatedNameAttribute>().ObfuscatedName.Equals(obfuscatedName, StringComparison.InvariantCulture));
    }
}
