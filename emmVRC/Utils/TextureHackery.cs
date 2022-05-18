using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace emmVRC.Utils
{
    public static class TextureHackery
    {
        private static List<Texture2D> cachedReadableTextures = new List<Texture2D>();
        public static Texture2D CloneReadable(this Texture2D texture)
        {
            if (cachedReadableTextures.Any(a => a.name == texture.name)) return cachedReadableTextures.FirstOrDefault(a => a.name == texture.name);

            RenderTexture tmp = RenderTexture.GetTemporary(
                                texture.width,
                                texture.height,
                                0,
                                RenderTextureFormat.Default,
                                RenderTextureReadWrite.Linear);
            
            Graphics.Blit(texture, tmp);            
            RenderTexture previous = RenderTexture.active;            
            RenderTexture.active = tmp;
            
            Texture2D myTexture2D = new Texture2D(texture.width, texture.height);
            myTexture2D.name = texture.name;
            myTexture2D.ReadPixels(new Rect(0, 0, tmp.width, tmp.height), 0, 0);
            myTexture2D.Apply();
            
            RenderTexture.active = previous;            
            RenderTexture.ReleaseTemporary(tmp);
            cachedReadableTextures.Add(myTexture2D);
            return myTexture2D;
        }
        public static Texture2D Desaturate(this Texture2D oldText)
        {
            Texture2D tempTexture = (oldText.isReadable ? oldText : oldText.CloneReadable());
            tempTexture.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            Texture2D greyTexture = new Texture2D(oldText.width, oldText.height);
            greyTexture.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            Color[] c = tempTexture.GetPixels();
            Color[] cDesat = new Color[c.Length];

            for (int i=0; i < c.Length; i++)
            {
                float desat = (c[i].r + c[i].g + c[i].b) / 3;
                cDesat[i].r = desat;
                cDesat[i].g = desat;
                cDesat[i].b = desat;
                cDesat[i].a = c[i].a;
            }
            greyTexture.SetPixels(cDesat);
            greyTexture.Apply();
            return greyTexture;
        }
        public static Texture2D UnpackTexture(this Sprite sprite)
        {
            Color[] c = (sprite.texture.isReadable ? sprite.texture : sprite.texture.CloneReadable()).GetPixels((int)sprite.textureRect.x, (int)sprite.textureRect.y, (int)sprite.textureRect.width, (int)sprite.textureRect.height);
            var slicedText = new Texture2D((int)sprite.textureRect.width, (int)sprite.textureRect.height);
            slicedText.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            slicedText.SetPixels(c);
            slicedText.Apply();
            return slicedText;
        }
        public static Sprite ReplaceTexture(this Sprite sprite, Texture2D newTexture)
        {
            var rect = new Rect(0, 0, sprite.rect.width, sprite.rect.height);//sprite.rect;
            var pivot = sprite.pivot / sprite.rect.size;
            var border = sprite.border;
            Sprite sprite2 = Sprite.CreateSpriteWithoutTextureScripting_Injected(ref rect, ref pivot, sprite.pixelsPerUnit, sprite.texture.Desaturate());
            //for (int i = 0; i < sprite.triangles.Length; i++)
            //    sprite2.triangles[i] = sprite.triangles[i];
            //for (int i = 0; i < sprite.uv.Length; i++)
            //    sprite2.uv[i] = sprite.uv[i];
            //for (int i = 0; i < sprite.vertices.Length; i++)
            //    sprite2.vertices[i] = sprite.vertices[i];
            //sprite2.pivot.Set(sprite.pivot.x, sprite.pivot.y);
            return sprite2;
        }
    }
}
