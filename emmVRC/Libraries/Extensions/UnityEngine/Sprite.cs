using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace emmVRC.Libraries.Extensions.UnityEngine
{
    public static class Sprite
    {
        private delegate IntPtr Sprite_CreateSprite_Injected_Delegate(IntPtr texturePtr, Rect rect, Vector2 pivot, float pixelsPerUnit, uint extrude, /*SpriteMeshType*/ int meshType, Vector4 border, bool generateFallbackPhysicsShape);

        private static IntPtr method_Sprite_CreateSprite_Injected_ptr;
        private static Sprite_CreateSprite_Injected_Delegate Sprite_CreateSprite_Injected;

        public static global::UnityEngine.Sprite Create(Texture2D texture, Rect rect, Vector2 pivot)
            => Create(texture, rect, pivot, 100.0f);
        public static global::UnityEngine.Sprite Create(Texture2D texture, Rect rect, Vector2 pivot, float pixelsPerUnit)
            => Create(texture, rect, pivot, pixelsPerUnit, 0);
        public static global::UnityEngine.Sprite Create(Texture2D texture, Rect rect, Vector2 pivot, float pixelsPerUnit, uint extrude)
            => Create(texture, rect, pivot, pixelsPerUnit, extrude, 1);
        public static global::UnityEngine.Sprite Create(Texture2D texture, Rect rect, Vector2 pivot, float pixelsPerUnit, uint extrude, /*SpriteMeshType*/ int meshType)
            => Create(texture, rect, pivot, pixelsPerUnit, extrude, meshType, Vector4.zero);
        public static global::UnityEngine.Sprite Create(Texture2D texture, Rect rect, Vector2 pivot, float pixelsPerUnit, uint extrude, /*SpriteMeshType*/ int meshType, Vector4 border)
            => Create(texture, rect, pivot, pixelsPerUnit, extrude, meshType, border, false);

        public static global::UnityEngine.Sprite Create(Texture2D texture, Rect rect, Vector2 pivot, float pixelsPerUnit, uint extrude, /*SpriteMeshType*/ int meshType, Vector4 border, bool generateFallbackPhysicsShape)
        {
            if (texture == null)
                return null;
            if (rect.xMax > texture.width)
            {
                // IL_003a ArgumentException
                throw new ArgumentException("rect width is too small");
            }

            if (rect.yMax > texture.height)
            {
                // IL_003a ArgumentException
                throw new ArgumentException("rect height is too small");
            }

            if (pixelsPerUnit <= 0)
            {
                // IL_00a8 ArgumentException
                throw new ArgumentException("pixelsPerUnit should be over 0");
            }

            return CreateSprite(texture, rect, pivot, pixelsPerUnit, extrude, meshType, border, generateFallbackPhysicsShape);
        }

        private static global::UnityEngine.Sprite CreateSprite(Texture2D texture, Rect rect, Vector2 pivot, float pixelsPerUnit, uint extrude, /*SpriteMeshType*/ int meshType, Vector4 border, bool generateFallbackPhysicsShape)
        {
            return CreateSprite_Injected(texture, rect, pivot, pixelsPerUnit, extrude, meshType, border, generateFallbackPhysicsShape);
        }

        private static global::UnityEngine.Sprite CreateSprite_Injected(Texture2D texture, Rect rect, Vector2 pivot, float pixelsPerUnit, uint extrude, /*SpriteMeshType*/ int meshType, Vector4 border, bool generateFallbackPhysicsShape)
        {
            if (method_Sprite_CreateSprite_Injected_ptr == IntPtr.Zero)
                method_Sprite_CreateSprite_Injected_ptr = NET_SDK.IL2CPP.il2cpp_resolve_icall(NET_SDK.IL2CPP.StringToIntPtr("UnityEngine.Sprite::CreateSprite_Injected(UnityEngine.Texture2D,UnityEngine.Rect&,UnityEngine.Vector2&,System.Single,System.UInt32,UnityEngine.SpriteMeshType,UnityEngine.Vector4&,System.Boolean)"));
            if (method_Sprite_CreateSprite_Injected_ptr != IntPtr.Zero)
                Sprite_CreateSprite_Injected = (Sprite_CreateSprite_Injected_Delegate)Marshal.GetDelegateForFunctionPointer(method_Sprite_CreateSprite_Injected_ptr, typeof(Sprite_CreateSprite_Injected_Delegate));
            if (Sprite_CreateSprite_Injected != null)
            {
                IntPtr ptr = Sprite_CreateSprite_Injected(texture.Pointer, rect, pivot, pixelsPerUnit, extrude, meshType, border, generateFallbackPhysicsShape);
                if (ptr != IntPtr.Zero)
                    return new global::UnityEngine.Sprite(ptr);
            }
            return null;
        }
    }
}
