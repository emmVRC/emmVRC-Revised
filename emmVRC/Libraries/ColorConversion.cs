using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace emmVRC.Libraries
{
    public class ColorConversion
    {
        public static Color HexToColor(string hexColor)
        {
            if (hexColor.IndexOf('#') != -1)
                hexColor = hexColor.Replace("#", "");
            float r, g, b = 0;
            r = int.Parse(hexColor.Substring(0, 2), System.Globalization.NumberStyles.AllowHexSpecifier) / 255.0F;
            g = int.Parse(hexColor.Substring(2, 2), System.Globalization.NumberStyles.AllowHexSpecifier) / 255.0F;
            b = int.Parse(hexColor.Substring(4, 2), System.Globalization.NumberStyles.AllowHexSpecifier) / 255.0F;
            return new Color(r, g, b);
        }
        public static string ColorToHex(Color baseColor, bool hash = false)
        {
            int r = Convert.ToInt32(baseColor.r * 255.0f);
            int g = Convert.ToInt32(baseColor.g * 255.0f);
            int b = Convert.ToInt32(baseColor.b * 255.0f);
            string hexValue = r.ToString("X2") + g.ToString("X2") + b.ToString("X2");
            if (hash)
                hexValue = "#" + hexValue;
            return hexValue;
        }
    }
}