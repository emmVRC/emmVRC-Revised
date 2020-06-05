using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using UnhollowerBaseLib;
using UnityEngine;

namespace emmVRC.Libraries.Extensions.UnityEngine
{
    public class Cubemap
    {
        private delegate bool CubeMap_Apply_Delegate(IntPtr @this, bool updateMipmaps, bool makeNoLongerReadable);

        private static IntPtr method_Apply_ptr;
        private static CubeMap_Apply_Delegate CubeMap_Apply;

        public static void Apply(IntPtr instance, bool updateMipmaps = true, bool makeNoLongerReadable = false)
        {
            if (method_Apply_ptr == null)
                method_Apply_ptr = UnhollowerBaseLib.IL2CPP.il2cpp_resolve_icall("UnityEngine.Cubemap::Apply(System.Boolean,System.Boolean)");
            if (method_Apply_ptr != IntPtr.Zero)
                CubeMap_Apply = (CubeMap_Apply_Delegate)Marshal.GetDelegateForFunctionPointer(method_Apply_ptr, typeof(CubeMap_Apply_Delegate));
            if (CubeMap_Apply != null)
                CubeMap_Apply(instance, updateMipmaps, makeNoLongerReadable);
        }
    }
}