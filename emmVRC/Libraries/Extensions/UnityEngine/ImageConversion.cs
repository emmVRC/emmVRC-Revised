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
    public static class ImageConversion
    {
        private delegate bool ImageConversion_LoadImage_Delegate(IntPtr tex, IntPtr data, bool markNonReadable);

        private static IntPtr method_LoadImage_ptr;
        private static ImageConversion_LoadImage_Delegate ImageConversion_LoadImage;

        public static bool LoadImage(Texture2D tex, byte[] data, bool markNonReadable)
        {
            if (method_LoadImage_ptr == null)
                method_LoadImage_ptr = NET_SDK.IL2CPP.il2cpp_resolve_icall(NET_SDK.IL2CPP.StringToIntPtr("UnityEngine.ImageConversion::LoadImage(UnityEngine.Texture2D,System.Byte[],System.Boolean)"));
            if (method_LoadImage_ptr != IntPtr.Zero)
                ImageConversion_LoadImage = (ImageConversion_LoadImage_Delegate)Marshal.GetDelegateForFunctionPointer(method_LoadImage_ptr, typeof(ImageConversion_LoadImage_Delegate));
            if (ImageConversion_LoadImage != null)
                return ImageConversion_LoadImage(tex.Pointer, ((Il2CppStructArray<byte>)data).Pointer, markNonReadable);
            return false;
        }
    }
}
