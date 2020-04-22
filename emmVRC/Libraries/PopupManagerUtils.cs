using Il2CppSystem;
using Il2CppSystem.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace emmVRC.Libraries
{
    public static class PopupManagerUtils
    {
        public static void HideCurrentPopup(this VRCUiPopupManager vrcUiPopupManager)
        {
            VRCUiManager.field_VRCUiManager_0.HideScreen("POPUP"); // Old code from build 864
        }

        public static void ShowStandardPopup(this VRCUiPopupManager vrcUiPopupManager, string title, string content, Action<VRCUiPopup> onCreated = null) =>
            vrcUiPopupManager.Method_Public_String_String_Action_1_VRCUiPopup_0(title, content, onCreated);

        public static void ShowStandardPopup(this VRCUiPopupManager vrcUiPopupManager, string title, string content, string buttonText, Action buttonAction, Action<VRCUiPopup> onCreated = null) =>
            vrcUiPopupManager.Method_Public_String_String_String_Action_Action_1_VRCUiPopup_0(title, content, buttonText, buttonAction, onCreated);

        public static void ShowStandardPopup(this VRCUiPopupManager vrcUiPopupManager, string title, string content, string button1Text, Action button1Action, string button2Text, Action button2Action, Action<VRCUiPopup> onCreated = null) =>
            vrcUiPopupManager.Method_Public_String_String_String_Action_String_Action_Action_1_VRCUiPopup_0(title, content, button1Text, button1Action, button2Text, button2Action, onCreated);
    }
}
