//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using UnityEngine;
//namespace emmVRC.Libraries
//{
//    public class TextureUtilities
//    {
//        public static Texture2D FlipTextureVertically(Texture2D original)
//        {
//            Texture2D working = original;
//            var originalPixels = working.GetPixels();

//            Color[] newPixels = new Color[originalPixels.Length];

//            int width = working.width;
//            int rows = working.height;

//            for (int x = 0; x < width; x++)
//            {
//                for (int y = 0; y < rows; y++)
//                {
//                    newPixels[x + y * width] = originalPixels[x + (rows - y - 1) * width];
//                }
//            }

//            working.SetPixels(newPixels);
//            working.Apply();
//            return working;
//        }
//        public static Texture2D FlipTextureHorizontally(Texture2D original)
//        {
//            Texture2D working = original;
//            var originalPixels = working.GetPixels();

//            Color[] newPixels = new Color[originalPixels.Length];

//            int width = working.width;
//            int rows = working.height;

//            for (int x = 0; x < width; x++)
//            {
//                for (int y = 0; y < rows; y++)
//                {
//                    newPixels[x * rows + y] = originalPixels[(width - x - 1) + y * rows];
//                }
//            }

//            working.SetPixels(newPixels);
//            working.Apply();
//            return working;
//        }
//    }
    
//}
