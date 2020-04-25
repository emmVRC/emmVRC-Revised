using NET_SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

namespace emmVRC.Libraries
{
    public class AssetBundle
    {
        private delegate IntPtr LoadFromFile_Internal_Delegate(IntPtr path, uint crc, ulong offset);
        private delegate IntPtr LoadAsset_Internal_Delegate(IntPtr assetbundle, IntPtr name, IntPtr type);

        private static bool init;

        private static LoadFromFile_Internal_Delegate method_LoadFromFile_internal;
        private static LoadAsset_Internal_Delegate method_LoadAsset_Internal;
        private IntPtr ptr;

        public AssetBundle(IntPtr ptr)
        {
            this.ptr = ptr;
        }

        private static void Init()
        {
            init = true;
            IntPtr methodpointer_LoadFromFile_Internal = IL2CPP.il2cpp_resolve_icall(IL2CPP.StringToIntPtr("UnityEngine.AssetBundle::LoadFromFile_Internal(System.String,System.UInt32,System.UInt64)"));
            IntPtr methodpointer_LoadAsset_Internal = IL2CPP.il2cpp_resolve_icall(IL2CPP.StringToIntPtr("UnityEngine.AssetBundle::LoadAsset_Internal(System.String,System.Type)"));

            if (methodpointer_LoadFromFile_Internal != IntPtr.Zero)
                method_LoadFromFile_internal = (LoadFromFile_Internal_Delegate)Marshal.GetDelegateForFunctionPointer(methodpointer_LoadFromFile_Internal, typeof(LoadFromFile_Internal_Delegate));
            if (methodpointer_LoadAsset_Internal != IntPtr.Zero)
                method_LoadAsset_Internal = (LoadAsset_Internal_Delegate)Marshal.GetDelegateForFunctionPointer(methodpointer_LoadAsset_Internal, typeof(LoadAsset_Internal_Delegate));

            emmVRCLoader.Logger.Log("UnityEngine.AssetBundle initialized. " +
                "LoadFromFile_Internal is *" + methodpointer_LoadFromFile_Internal +
                ", LoadAsset_Internal is *" + methodpointer_LoadAsset_Internal);
        }


        private static AssetBundle LoadFromFile_Internal(string path, uint crc, ulong offset)
        {
            if (!init) Init();

            IntPtr r = method_LoadFromFile_internal(IL2CPP.StringToIntPtr(path), crc, offset);
            return r == IntPtr.Zero ? null : new AssetBundle(r);
        }

        public static AssetBundle LoadFromFile(string path)
        {
            return LoadFromFile_Internal(path, 0U, 0UL);
        }

        public UnityEngine.Object LoadAsset(string name)
        {
            return LoadAsset(name, GameObject.Il2CppType.Pointer);
        }

        public UnityEngine.Sprite LoadAssetSprite(string name)
        {
            return LoadAssetSprite(name, Sprite.Il2CppType.Pointer);
        }

        private UnityEngine.Object LoadAsset(string name, IntPtr type)
        {
            if (name == null)
            {
                throw new NullReferenceException("The input asset name cannot be null.");
            }
            if (name.Length == 0)
            {
                throw new ArgumentException("The input asset name cannot be empty.");
            }
            if (type == null)
            {
                throw new NullReferenceException("The input type cannot be null.");
            }
            return LoadAsset_Internal(name, type);
        }

        private UnityEngine.Sprite LoadAssetSprite(string name, IntPtr type)
        {
            if (name == null)
            {
                throw new NullReferenceException("The input asset name cannot be null.");
            }
            if (name.Length == 0)
            {
                throw new ArgumentException("The input asset name cannot be empty.");
            }
            if (type == null)
            {
                throw new NullReferenceException("The input type cannot be null.");
            }
            return LoadAssetSprite_Internal(name, type);
        }

        private UnityEngine.Object LoadAsset_Internal(string name, IntPtr type)
        {
            if (!init) Init();

            IntPtr r = method_LoadAsset_Internal(ptr, IL2CPP.StringToIntPtr(name), type);
            return r == IntPtr.Zero ? null : new UnityEngine.Object(r);
        }
        private UnityEngine.Sprite LoadAssetSprite_Internal(string name, IntPtr type)
        {
            if (!init) Init();

            IntPtr r = method_LoadAsset_Internal(ptr, IL2CPP.StringToIntPtr(name), type);
            return r == IntPtr.Zero ? null : new UnityEngine.Sprite(r);
        }
    }
}
