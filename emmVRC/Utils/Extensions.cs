using System;
using UnityEngine;
using UnityEngine.UI;

namespace emmVRC.Utils
{
    public static class Extensions
    {
        public static string GetPath(this GameObject gameObject)
        {
            string path = "/" + gameObject.name;
            while (gameObject.transform.parent != null)
            {
                gameObject = gameObject.transform.parent.gameObject;
                path = "/" + gameObject.name + path;
            }
            return path;
        }

        public static string GetQuickMenuRelativePath(this GameObject gameObject)
        {
            string path = gameObject.GetPath();
            string quickMenuPath = Singletons.quickMenu.gameObject.GetPath();

            string removedPath = path.Remove(0, quickMenuPath.Length).Substring(1);

            if (removedPath.Length == path.Length)
                throw new ArgumentException("Given GameObject was not relative to the QuickMenu");
            return removedPath;
        }

        public static void DestroyChildren(this Transform transform, Func<Transform, bool> exclude)
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
                if (exclude == null || exclude(transform.GetChild(i)))
                    GameObject.DestroyImmediate(transform.GetChild(i).gameObject);
        }

        public static void DestroyChildren(this Transform transform) => DestroyChildren(transform, null);

        public static Vector3 SetX(this Vector3 vector, float x)
        {
            return new Vector3(x, vector.y, vector.z);
        }
        public static Vector3 SetY(this Vector3 vector, float y)
        {
            return new Vector3(vector.x, y, vector.z);
        }
        public static Vector3 SetZ(this Vector3 vector, float z)
        {
            return new Vector3(vector.x, vector.y, z);
        }

        public static float RoundAmount(this float i, float nearestFactor)
        {
            return (float)Math.Round(i / nearestFactor) * nearestFactor;
        }

        public static Vector3 RoundAmount(this Vector3 i, float nearestFactor)
        {
            return new Vector3(i.x.RoundAmount(nearestFactor), i.y.RoundAmount(nearestFactor), i.z.RoundAmount(nearestFactor));
        }

        public static Vector2 RoundAmount(this Vector2 i, float nearestFactor)
        {
            return new Vector2(i.x.RoundAmount(nearestFactor), i.y.RoundAmount(nearestFactor));
        }

        public static Sprite ToSprite(this Texture2D tex)
        {
            Rect rect = new Rect(0, 0, tex.width, tex.height);
            Vector2 pivot = new Vector2(0.5f, 0.5f);
            Vector4 border = Vector4.zero;
            Sprite sprite = Sprite.CreateSprite_Injected(tex, ref rect, ref pivot, 50, 0, SpriteMeshType.FullRect, ref border, false);
            sprite.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            return sprite;
        }

        // https://stackoverflow.com/questions/8809354/replace-first-occurrence-of-pattern-in-a-string
        public static string ReplaceFirst(this string text, string search, string replace)
        {
            int pos = text.IndexOf(search);
            if (pos < 0)
            {
                return text;
            }
            return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
        }

        public static ColorBlock SetColor(this ColorBlock block, Color color)
        {
            return new ColorBlock()
            {
                colorMultiplier = block.colorMultiplier,
                disabledColor = Color.grey,
                highlightedColor = color,
                normalColor = color / 1.5f,
                pressedColor = Color.white,
                selectedColor = color / 1.5f,
            };
        }

        public static void DelegateSafeInvoke(this Delegate @delegate, params object[] args)
        {
            Delegate[] delegates = @delegate.GetInvocationList();
            for (int i = 0; i < delegates.Length; i++)
            {
                try
                {
                    delegates[i].DynamicInvoke(args);
                }
                catch (Exception ex)
                {
                    emmVRCLoader.Logger.LogError("Error while executing delegate:\n" + ex.ToString());
                }
            }
        }

        public static string ToEasyString(this TimeSpan timeSpan)
        {
            if (Mathf.FloorToInt(timeSpan.Ticks / TimeSpan.TicksPerDay) > 0)
                return $"{timeSpan:%d} days";
            else if (Mathf.FloorToInt(timeSpan.Ticks / TimeSpan.TicksPerHour) > 0)
                return $"{timeSpan:%h} hours";
            else if (Mathf.FloorToInt(timeSpan.Ticks / TimeSpan.TicksPerMinute) > 0)
                return $"{timeSpan:%m} minutes";
            else
                return $"{timeSpan:%s} seconds";
        }
    }
}