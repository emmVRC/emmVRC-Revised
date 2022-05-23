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
        private static Dictionary<Texture2D, Texture2D> cachedReadableTexture2D = new Dictionary<Texture2D, Texture2D>();
        private static Dictionary<Texture, Texture2D> cachedConvertedTexture2D = new Dictionary<Texture, Texture2D>();
        private static Dictionary<Texture2D, Texture2D> cachedDesaturatedTexture2D = new Dictionary<Texture2D, Texture2D>();
        private static Dictionary<Sprite, Texture2D> cachedUnpackedTexture2D = new Dictionary<Sprite, Texture2D>();

        public static Texture ToTexture(this Texture2D texture)
        {
            return (Texture)texture;
        }
        public static Texture2D ToTexture2D(this Texture texture)
        {
            if (cachedConvertedTexture2D.Any(a => a.Key != null && a.Key == texture)) return cachedConvertedTexture2D.FirstOrDefault(a => a.Key != null && a.Key == texture).Value;
            FilterMode currentFilter = texture.filterMode;
            texture.filterMode = FilterMode.Point;

            RenderTexture tmp = RenderTexture.GetTemporary(
                texture.width,
                texture.height,
                0,
                RenderTextureFormat.ARGB32);
            Graphics.Blit2(texture, tmp);
            RenderTexture.active = tmp;

            Texture2D myTexture2D = new Texture2D(texture.width, texture.height);
            myTexture2D.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            myTexture2D.name = texture.name;
            myTexture2D.ReadPixels(new Rect(0, 0, tmp.width, tmp.height), 0, 0);
            myTexture2D.Apply();
            myTexture2D.wrapMode = texture.wrapMode;
            myTexture2D.wrapModeU = texture.wrapModeU;
            myTexture2D.wrapModeV = texture.wrapModeV;
            myTexture2D.wrapModeW = texture.wrapModeW;

            RenderTexture.active = null;
            RenderTexture.ReleaseTemporary(tmp);
            texture.filterMode = currentFilter;

            cachedConvertedTexture2D.Add(texture, myTexture2D);
            return myTexture2D;
        }
        public static Texture2D CloneReadable(this Texture2D texture)
        {
            if (cachedReadableTexture2D.Any(a => a.Key != null && a.Key == texture)) return cachedReadableTexture2D.FirstOrDefault(a => a.Key != null && a.Key == texture).Value;
            FilterMode currentFilter = texture.filterMode;
            texture.filterMode = FilterMode.Point;
            RenderTexture tmp = RenderTexture.GetTemporary(
                                texture.width,
                                texture.height,
                                0,
                                RenderTextureFormat.ARGB32);

            Graphics.Blit2(texture, tmp);
            RenderTexture.active = tmp;

            Texture2D myTexture2D = new Texture2D(texture.width, texture.height);
            myTexture2D.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            myTexture2D.name = texture.name;
            myTexture2D.ReadPixels(new Rect(0, 0, tmp.width, tmp.height), 0, 0);
            myTexture2D.Apply();
            myTexture2D.wrapMode = texture.wrapMode;
            myTexture2D.wrapModeU = texture.wrapModeU;
            myTexture2D.wrapModeV = texture.wrapModeV;
            myTexture2D.wrapModeW = texture.wrapModeW;

            RenderTexture.active = null;
            RenderTexture.ReleaseTemporary(tmp);
            texture.filterMode = currentFilter;

            cachedReadableTexture2D.Add(texture, myTexture2D);
            return myTexture2D;
        }
        public static Texture2D Desaturate(this Texture2D oldText)
        {
            if (cachedDesaturatedTexture2D.Any(a => a.Key != null && a.Key == oldText)) return cachedDesaturatedTexture2D.FirstOrDefault(a => a.Key != null && a.Key == oldText).Value;
            Texture2D tempTexture = (oldText.isReadable ? oldText : oldText.CloneReadable());
            tempTexture.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            Texture2D greyTexture = new Texture2D(oldText.width, oldText.height);
            greyTexture.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            Color[] c = tempTexture.GetPixels();
            Color[] cDesat = new Color[c.Length];

            for (int i = 0; i < c.Length; i++)
            {
                float desat = (c[i].r + c[i].g + c[i].b) / 3;
                cDesat[i].r = desat;
                cDesat[i].g = desat;
                cDesat[i].b = desat;
                cDesat[i].a = c[i].a;
            }
            greyTexture.SetPixels(cDesat);
            greyTexture.Apply();

            cachedDesaturatedTexture2D.Add(oldText, greyTexture);
            return greyTexture;
        }
        public static Texture2D UnpackTexture(this Sprite sprite)
        {
            if (cachedUnpackedTexture2D.Any(a => a.Key != null && a.Key == sprite)) return cachedUnpackedTexture2D.FirstOrDefault(a => a.Key != null && a.Key == sprite).Value;
            Rect actualRect;
            if (!sprite.packed || sprite.packingMode != SpritePackingMode.Tight)
                actualRect = sprite.textureRect;
            else
            {
                // Deconstructed rect because ooga booga "Arithmetic operation resulted in an overflow"
                float xMin = 1f;
                float yMax = 0f;
                float xMax = 0f;
                float yMin = 1f;

                // Tight packing means that the actual textureRect isn't necessarily accurate, and sprites are built using a UV map instead. This should allow us to extract *any* sprite's texture
                foreach (Vector2 vector in sprite.uv)
                {
                    if (vector.x > xMax)
                        xMax = vector.x;
                    if (vector.x < xMin)
                        xMin = vector.x;
                    if (vector.y > yMax)
                        yMax = vector.y;
                    if (vector.y < yMin)
                        yMin = vector.y;
                }
                actualRect = new Rect
                {
                    m_XMin = xMin * sprite.texture.width,
                    m_YMin = yMin * sprite.texture.height,
                    m_Width = (xMax - xMin) * sprite.texture.width,
                    m_Height = (yMax - yMin) * sprite.texture.height
                };
            }

            Texture2D readableText = (sprite.texture.isReadable ? sprite.texture : sprite.texture.CloneReadable());
            Color[] c = (readableText.GetPixels((int)actualRect.x, (int)actualRect.y, (int)actualRect.width, (int)actualRect.height));
            var slicedText = new Texture2D((int)actualRect.width, (int)actualRect.height);
            slicedText.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            slicedText.SetPixels(c);
            slicedText.Apply();

            cachedUnpackedTexture2D.Add(sprite, slicedText);
            return slicedText;
        }
        public static Sprite ReplaceTexture(this Sprite sprite, Texture2D newTexture)
        {
            if (sprite == null) return sprite;
            var rect = new Rect(0, 0, newTexture.width, newTexture.height);
            var pivot = sprite.pivot / sprite.rect.size;
            var border = sprite.border;
            var newSprite = Sprite.CreateSprite_Injected(newTexture, ref rect, ref pivot, sprite.pixelsPerUnit, 0, SpriteMeshType.FullRect, ref border, false);
            return newSprite;
        }
    }
}
