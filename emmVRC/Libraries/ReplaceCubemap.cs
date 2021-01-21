using System.Collections;
using UnityEngine;

namespace emmVRC.Libraries
{
    public class ReplaceCubemap
    {
        public static int CubemapResolution = 256;

        private static Texture2D source;

        private static Vector3[][] faces =
        {
        new Vector3[] {
            new Vector3(1.0f, 1.0f, -1.0f),
            new Vector3(1.0f, 1.0f, 1.0f),
            new Vector3(1.0f, -1.0f, -1.0f),
            new Vector3(1.0f, -1.0f, 1.0f)
        },
        new Vector3[] {
            new Vector3(-1.0f, 1.0f, 1.0f),
            new Vector3(-1.0f, 1.0f, -1.0f),
            new Vector3(-1.0f, -1.0f, 1.0f),
            new Vector3(-1.0f, -1.0f, -1.0f)
        },
        new Vector3[] {
            new Vector3(-1.0f, 1.0f, 1.0f),
            new Vector3(1.0f, 1.0f, 1.0f),
            new Vector3(-1.0f, 1.0f, -1.0f),
            new Vector3(1.0f, 1.0f, -1.0f)
        },
        new Vector3[] {
            new Vector3(-1.0f, -1.0f, -1.0f),
            new Vector3(1.0f, -1.0f, -1.0f),
            new Vector3(-1.0f, -1.0f, 1.0f),
            new Vector3(1.0f, -1.0f, 1.0f)
        },
        new Vector3[] {
            new Vector3(-1.0f, 1.0f, -1.0f),
            new Vector3(1.0f, 1.0f, -1.0f),
            new Vector3(-1.0f, -1.0f, -1.0f),
            new Vector3(1.0f, -1.0f, -1.0f)
        },
        new Vector3[] {
            new Vector3(1.0f, 1.0f, 1.0f),
            new Vector3(-1.0f, 1.0f, 1.0f),
            new Vector3(1.0f, -1.0f, 1.0f),
            new Vector3(-1.0f, -1.0f, 1.0f)
        }
    };
        internal static Cubemap BuildCubemap(Texture2D sourceTex)
        {
            // new cubemap 
            CubemapResolution = sourceTex.width;
            Cubemap c = new Cubemap(CubemapResolution, TextureFormat.RGBA32, false);
            source = sourceTex;
            Color[] CubeMapColors;

            for (int i = 0; i < 6; i++)
            {
                CubeMapColors = CreateCubemapTexture(CubemapResolution, (CubemapFace)i);
                c.SetPixels(CubeMapColors, (CubemapFace)i);
            }
            // we set the cubemap from the texture pixel by pixel
            c.Apply();
            return c;
        }
        private static Color[] CreateCubemapTexture(int resolution, CubemapFace face)
        {
            Texture2D texture = new Texture2D(resolution, resolution, TextureFormat.RGB24, false);

            Vector3 texelX_Step = (faces[(int)face][1] - faces[(int)face][0]) / resolution;
            Vector3 texelY_Step = (faces[(int)face][3] - faces[(int)face][2]) / resolution;

            float texelSize = 1.0f / resolution;
            float texelIndex = 0.0f;

            //Create textured face
            Color[] cols = new Color[resolution];
            for (int y = 0; y < resolution; y++)
            {
                Vector3 texelX = faces[(int)face][0];
                Vector3 texelY = faces[(int)face][2];
                for (int x = 0; x < resolution; x++)
                {
                    cols[x] = Project(Vector3.Lerp(texelX, texelY, texelIndex).normalized);
                    texelX += texelX_Step;
                    texelY += texelY_Step;
                }
                texture.SetPixels(0, y, resolution, 1, cols, 0);
                texelIndex += texelSize;
            }
            texture.wrapMode = TextureWrapMode.Clamp;
            texture.Apply();

            Color[] colors = texture.GetPixels();

            return colors;
        }


        private static Color Project(Vector3 direction)
        {
            float theta = Mathf.Atan2(direction.z, direction.x) + 3.1415926f / 180.0f;
            float phi = Mathf.Acos(direction.y);

            int texelX = (int)(((theta / 3.1415926f) * 0.5f + 0.5f) * source.width);
            if (texelX < 0) texelX = 0;
            if (texelX >= source.width) texelX = source.width - 1;
            int texelY = (int)((phi / 3.1415926f) * source.height);
            if (texelY < 0) texelY = 0;
            if (texelY >= source.height) texelY = source.height - 1;

            return source.GetPixel(texelX, source.height - texelY - 1);
        }
    }
}