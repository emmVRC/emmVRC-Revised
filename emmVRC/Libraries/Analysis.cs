using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnhollowerRuntimeLib.XrefScans;

namespace emmVRC.Libraries
{
    class Analysis
    {
        public static void LogXrefResults(MethodInfo method, List<Type> expectedTypes)
        {
            try
            {
                if (expectedTypes != null)
                    foreach (Type type in expectedTypes)
                        XrefScanMethodDb.RegisterType(type);
                System.Collections.Generic.List<UnhollowerRuntimeLib.XrefScans.XrefInstance> instances = UnhollowerRuntimeLib.XrefScans.XrefScanner.XrefScan(method).ToList();
                emmVRCLoader.Logger.LogDebug("Instance count: " + instances.Count);
                foreach (UnhollowerRuntimeLib.XrefScans.XrefInstance inst in instances)
                {
                    emmVRCLoader.Logger.LogDebug("XrefType: " + inst.Type.ToString());
                    if (inst.Type == XrefType.Global)
                    {
                        emmVRCLoader.Logger.LogDebug("Actual type: " + inst.ReadAsObject()?.GetType().ToString());
                        emmVRCLoader.Logger.LogDebug("Value: " + inst.ReadAsObject()?.ToString());
                    }
                    else if (inst.Type == XrefType.Method)
                    {
                        if (inst.TryResolve() != null) { 
                            emmVRCLoader.Logger.LogDebug("Method name: " + inst.TryResolve().Name);
                            emmVRCLoader.Logger.LogDebug("Parent type: " + inst.TryResolve().ReflectedType.Name);
                        }
                        else
                            emmVRCLoader.Logger.LogDebug("Method is null. Make sure you have defined the expected type for this method");
                    }
                }
            } catch (Exception ex)
            {
                emmVRCLoader.Logger.LogDebug("Xref scan failed: " + ex.ToString());
            }
        }
    }
}
