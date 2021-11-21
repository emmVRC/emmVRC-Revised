//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using UnityEngine;

//namespace emmVRC.Utils
//{
//    public static class TextureHackery
//    {
//        public static Texture2D CloneWriteable(this Texture2D oldText)
//        {
//            try
//            {
//                RenderTexture renderTex = RenderTexture.GetTemporary(
//                    oldText.width,
//                    oldText.height,
//                    0, // TODO: Grab depth buffer info from old texture?
//                    RenderTextureFormat.Default,
//                    RenderTextureReadWrite.Linear);
//                Graphics.Blit(oldText, renderTex);
//                RenderTexture previous = RenderTexture.active;
//                RenderTexture.active = renderTex;
//                Texture2D readableTexture = new Texture2D(oldText.width, oldText.height);
//                readableTexture.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
//                readableTexture.Apply();
//                RenderTexture.active = previous;
//                RenderTexture.ReleaseTemporary(renderTex);
//                return readableTexture;
//            } catch (Exception ex)
//            {
//                emmVRCLoader.Logger.LogError("Texture could not be cloned: " + ex.ToString());
//            }
//            return null;
//        }
//        public static Texture2D Colourless(this Texture2D oldText)
//        {
//            Texture2D tempTexture = oldText.CloneWriteable();
//            Texture2D greyTexture = new Texture2D(oldText.width, oldText.height);
//            for (int i=0; i < tempTexture.GetPixels().Count; i++)
//            {
//                tempTexture.SetPixel(i, Color.white);
//            }
//        }
//    }
//}
