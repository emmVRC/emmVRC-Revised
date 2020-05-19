using Il2CppSystem;
using Il2CppSystem.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace emmVRC.Libraries
{
    public static class PopupManagerUtils
    {
        public static void HideCurrentPopup(this VRCUiPopupManager vrcUiPopupManager)
        {
            VRCUiManager.field_Protected_Static_VRCUiManager_0.HideScreen("POPUP"); // Old code from build 864
        }

        public static void ShowStandardPopup(this VRCUiPopupManager vrcUiPopupManager, string title, string content, Action<VRCUiPopup> onCreated = null) =>
            vrcUiPopupManager.Method_Public_Void_String_String_Action_1_VRCUiPopup_0(title, content, onCreated);

        public static void ShowStandardPopup(this VRCUiPopupManager vrcUiPopupManager, string title, string content, string buttonText, Action buttonAction, Action<VRCUiPopup> onCreated = null) =>
            vrcUiPopupManager.Method_Public_Void_String_String_String_Action_Action_1_VRCUiPopup_3(title, content, buttonText, buttonAction, onCreated);

        public static void ShowStandardPopupV2(this VRCUiPopupManager vrcUiPopupManager, string title, string content, string buttonText, Action buttonAction, Action<VRCUiPopup> onCreated = null) =>
            vrcUiPopupManager.Method_Public_Void_String_String_String_Action_Action_1_VRCUiPopup_0(title, content, buttonText, buttonAction, onCreated);
        
            public static void ShowStandardPopup(this VRCUiPopupManager vrcUiPopupManager, string title, string content, string button1Text, Action button1Action, string button2Text, Action button2Action, Action<VRCUiPopup> onCreated = null) =>
            vrcUiPopupManager.Method_Public_Void_String_String_String_Action_String_Action_Action_1_VRCUiPopup_4(title, content, button1Text, button1Action, button2Text, button2Action, onCreated);
        public static void ShowStandardPopupV2(this VRCUiPopupManager vrcUiPopupManager, string title, string content, string button1Text, Action button1Action, string button2Text, Action button2Action, Action<VRCUiPopup> onCreated = null) =>
            vrcUiPopupManager.Method_Public_Void_String_String_String_Action_String_Action_Action_1_VRCUiPopup_1(title, content, button1Text, button1Action, button2Text, button2Action, onCreated);
        public static void ShowInputPopup(this VRCUiPopupManager vrcUiPopupManager, string title, string preFilledText, UnityEngine.UI.InputField.InputType inputType, bool keypad, string buttonText, Action<string, Il2CppSystem.Collections.Generic.List<KeyCode>, UnityEngine.UI.Text> buttonAction, string boxText = "Enter text....", bool catShrug = true, Action<VRCUiPopup> onCreated = null) =>
            vrcUiPopupManager.Method_Public_Void_String_String_InputType_Boolean_String_Action_3_String_List_1_KeyCode_Text_String_Boolean_Action_1_VRCUiPopup_0(title, preFilledText, inputType, keypad, buttonText, buttonAction, boxText, catShrug, onCreated);
        public static void ShowInputPopup(this VRCUiPopupManager vrcUiPopupManager, string title, string preFilledText, UnityEngine.UI.InputField.InputType inputType, bool keypad, string buttonText, Action<string, Il2CppSystem.Collections.Generic.List<KeyCode>, UnityEngine.UI.Text> buttonAction, Action cancelAction, string boxText = "Enter text....", bool catShrug = true, Action<VRCUiPopup> onCreated = null) =>
            vrcUiPopupManager.Method_Public_Void_String_String_InputType_Boolean_String_Action_3_String_List_1_KeyCode_Text_Action_String_Boolean_Action_1_VRCUiPopup_1(title, preFilledText, inputType, keypad, buttonText, buttonAction, cancelAction, boxText, catShrug, onCreated);
        public static void ShowAlert(this VRCUiPopupManager vrcUiPopupManager, string title, string content, float timeout) =>
            vrcUiPopupManager.Method_Public_Void_String_String_Single_0(title, content, timeout);
    }
}
