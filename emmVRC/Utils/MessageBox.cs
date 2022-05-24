using System;
using System.Runtime.InteropServices;

namespace emmVRC.Utils;

public class MessageBox
{
    [DllImport("user32.dll", EntryPoint = "MessageBox", CharSet = CharSet.Auto)]
    public static extern int Show(IntPtr hWnd, string lpText, string lpCaption, uint uType);
}