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
            VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.Method_Public_Void_String_String_InputType_Boolean_String_Action_3_String_List_1_KeyCode_Text_Action_String_Boolean_Action_1_VRCUiPopup_1(title, "", UnityEngine.UI.InputField.InputType.Standard, false, buttonText, UnhollowerRuntimeLib.DelegateSupport.ConvertDelegate<Il2CppSystem.Action<string, Il2CppSystem.Collections.Generic.List<UnityEngine.KeyCode>, UnityEngine.UI.Text>>((System.Action<string, Il2CppSystem.Collections.Generic.List<UnityEngine.KeyCode>, UnityEngine.UI.Text>)((string s, Il2CppSystem.Collections.Generic.List<UnityEngine.KeyCode> k, UnityEngine.UI.Text t) => { acceptAction.Invoke(s); })), null);
        }
    }
}
