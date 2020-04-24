using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace emmVRC.Libraries
{
    public enum KeyCodeStringStyle {
        Unity,
        Clean
    }
    public class KeyCodeConversion
    {
        public static string Stringify(UnityEngine.KeyCode keyCode, KeyCodeStringStyle style = KeyCodeStringStyle.Clean)
        {
            if (style == KeyCodeStringStyle.Unity)
                return keyCode.ToString();
            else
            {
                if (keyCode == UnityEngine.KeyCode.LeftControl)
                    return "LeftCTRL";
                else if (keyCode == UnityEngine.KeyCode.RightControl)
                    return "RightCTRL";
                else if (keyCode == UnityEngine.KeyCode.LeftCommand)
                    return "LeftWin";
                else if (keyCode == UnityEngine.KeyCode.RightCommand)
                    return "RightWin";
                else if (keyCode == UnityEngine.KeyCode.Alpha0)
                    return "0";
                else if (keyCode == UnityEngine.KeyCode.Alpha1)
                    return "1";
                else if (keyCode == UnityEngine.KeyCode.Alpha2)
                    return "2";
                else if (keyCode == UnityEngine.KeyCode.Alpha3)
                    return "3";
                else if (keyCode == UnityEngine.KeyCode.Alpha4)
                    return "4";
                else if (keyCode == UnityEngine.KeyCode.Alpha5)
                    return "5";
                else if (keyCode == UnityEngine.KeyCode.Alpha6)
                    return "6";
                else if (keyCode == UnityEngine.KeyCode.Alpha7)
                    return "7";
                else if (keyCode == UnityEngine.KeyCode.Alpha8)
                    return "8";
                else if (keyCode == UnityEngine.KeyCode.Alpha9)
                    return "9";
                else
                    return keyCode.ToString();
            }
        }
    }
}
