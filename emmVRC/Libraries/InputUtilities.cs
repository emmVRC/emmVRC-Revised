using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace emmVRC.Libraries
{
    public class InputUtilities
    {
        public static void OpenInputBox(string title, string buttonText, Action<string> acceptAction) 
        {
            VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowInputPopup(title, "", UnityEngine.UI.InputField.InputType.Standard, false, buttonText, UnhollowerRuntimeLib.DelegateSupport.ConvertDelegate<Il2CppSystem.Action<string, Il2CppSystem.Collections.Generic.List<UnityEngine.KeyCode>, UnityEngine.UI.Text>>((System.Action<string, Il2CppSystem.Collections.Generic.List<UnityEngine.KeyCode>, UnityEngine.UI.Text>)((string s, Il2CppSystem.Collections.Generic.List<UnityEngine.KeyCode> k, UnityEngine.UI.Text t) => { acceptAction.Invoke(s); })), null, "Enter text....");
        }

        public static void OpenHiddenBox(string title, string buttonText, Action<string> acceptAction)
        {
            VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowInputPopup(title, "", UnityEngine.UI.InputField.InputType.Password, false, buttonText, UnhollowerRuntimeLib.DelegateSupport.ConvertDelegate<Il2CppSystem.Action<string, Il2CppSystem.Collections.Generic.List<UnityEngine.KeyCode>, UnityEngine.UI.Text>>((System.Action<string, Il2CppSystem.Collections.Generic.List<UnityEngine.KeyCode>, UnityEngine.UI.Text>)((string s, Il2CppSystem.Collections.Generic.List<UnityEngine.KeyCode> k, UnityEngine.UI.Text t) => { acceptAction.Invoke(s); })), null, "Enter text....");
        }

        public static void OpenNumberPad(string title, string buttonText, Action<string> acceptAction)
        {
            VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowInputPopup(title, "", UnityEngine.UI.InputField.InputType.Standard, true, buttonText, UnhollowerRuntimeLib.DelegateSupport.ConvertDelegate<Il2CppSystem.Action<string, Il2CppSystem.Collections.Generic.List<UnityEngine.KeyCode>, UnityEngine.UI.Text>>((System.Action<string, Il2CppSystem.Collections.Generic.List<UnityEngine.KeyCode>, UnityEngine.UI.Text>)((string s, Il2CppSystem.Collections.Generic.List<UnityEngine.KeyCode> k, UnityEngine.UI.Text t) => { acceptAction.Invoke(s); })), null, "Enter text....");
        }
    }
}
