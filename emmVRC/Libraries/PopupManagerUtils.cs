using System;
using Il2CppSystem.Reflection;
using Il2CppSystem.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnhollowerBaseLib.Attributes;
using UnityEngine;
using UnhollowerRuntimeLib.XrefScans;

namespace emmVRC.Libraries
{
    public static class PopupManagerUtils
    {
        // Special thanks to Knah for xref scanning, as well as UiExpansionKit
        public static void HideCurrentPopup(this VRCUiPopupManager vrcUiPopupManager)
        {
            VRCUiManager.prop_VRCUiManager_0.HideScreen("POPUP"); // Old code from build 864
        }

        public static void ShowStandardPopup(this VRCUiPopupManager vrcUiPopupManager, string title, string content, Action<VRCUiPopup> onCreated = null) =>
            ShowUiStandardPopup1.Invoke(title, content, onCreated);

        public static void ShowStandardPopup(this VRCUiPopupManager vrcUiPopupManager, string title, string content, string buttonText, Action buttonAction, Action<VRCUiPopup> onCreated = null) =>
            ShowUiStandardPopup2.Invoke(title, content, buttonText, buttonAction, onCreated);

        public static void ShowStandardPopup(this VRCUiPopupManager vrcUiPopupManager, string title, string content, string button1Text, Action button1Action, string button2Text, Action button2Action, Action<VRCUiPopup> onCreated = null) =>
            ShowUiStandardPopup3.Invoke(title, content, button1Text, button1Action, button2Text, button2Action, onCreated);

        public static void ShowStandardPopupV2(this VRCUiPopupManager vrcUiPopupManager, string title, string content, string buttonText, Action buttonAction, Action<VRCUiPopup> onCreated = null) =>
            ShowUiStandardPopupV21.Invoke(title, content, buttonText, buttonAction, onCreated);

        public static void ShowStandardPopupV2(this VRCUiPopupManager vrcUiPopupManager, string title, string content, string button1Text, Action button1Action, string button2Text, Action button2Action, Action<VRCUiPopup> onCreated = null) =>
            ShowUiStandardPopupV22.Invoke(title, content, button1Text, button1Action, button2Text, button2Action, onCreated);

        public static void ShowInputPopup(this VRCUiPopupManager vrcUiPopupManager, string title, string preFilledText, UnityEngine.UI.InputField.InputType inputType, bool keypad, string buttonText, Il2CppSystem.Action<string, List<KeyCode>, UnityEngine.UI.Text> buttonAction, Il2CppSystem.Action cancelAction, string boxText = "Enter text....", bool closeOnAccept = true, Action<VRCUiPopup> onCreated = null, bool startOnLeft = false, int characterLimit = 0) =>
            ShowUiInputPopup.Invoke(title, preFilledText, inputType, keypad, buttonText, buttonAction, cancelAction, boxText, closeOnAccept, onCreated, startOnLeft, characterLimit);
        public static void ShowAlert(this VRCUiPopupManager vrcUiPopupManager, string title, string content, float timeout) =>
            ShowUiAlertPopup.Invoke(title, content, timeout);
        #region Input Popup
        public delegate void ShowUiInputPopupAction(string title, string initialText, UnityEngine.UI.InputField.InputType inputType,
            bool isNumeric, string confirmButtonText, Il2CppSystem.Action<string, List<KeyCode>, UnityEngine.UI.Text> onComplete,
            Il2CppSystem.Action onCancel, string placeholderText = "Enter text...", bool closeAfterInput = true,
            Il2CppSystem.Action<VRCUiPopup> onPopupShown = null, bool startOnLeft = false, int characterLimit = 0);

        private static ShowUiInputPopupAction ourShowUiInputPopupAction;

        public static ShowUiInputPopupAction ShowUiInputPopup
        {
            get
            {
                if (ourShowUiInputPopupAction != null) return ourShowUiInputPopupAction;

                var targetMethod = typeof(VRCUiPopupManager).GetMethods(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public)
                    .FirstOrDefault(it => it.GetParameters().Length == 12 && XrefScanner.XrefScan(it).Any(jt => jt.Type == XrefType.Global && jt.ReadAsObject()?.ToString() == "UserInterface/MenuContent/Popups/InputPopup"));

                ourShowUiInputPopupAction = (ShowUiInputPopupAction)Delegate.CreateDelegate(typeof(ShowUiInputPopupAction), VRCUiPopupManager.prop_VRCUiPopupManager_0, targetMethod);

                return ourShowUiInputPopupAction;
            }
        }
        #endregion
        #region Standard Popup with no buttons
        public delegate void ShowUiStandardPopup1Action(string title, string body, Il2CppSystem.Action<VRCUiPopup> onPopupShown = null);

        private static ShowUiStandardPopup1Action ourShowUiStandardPopup1Action;

        public static ShowUiStandardPopup1Action ShowUiStandardPopup1
        {
            get
            {
                if (ourShowUiStandardPopup1Action != null) return ourShowUiStandardPopup1Action;

                var targetMethod = typeof(VRCUiPopupManager).GetMethods(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public)
                    .FirstOrDefault(it => it.GetParameters().Length == 3 && !it.Name.Contains("PDM") && XrefScanner.XrefScan(it).Any(jt => jt.Type == XrefType.Global && jt.ReadAsObject()?.ToString() == "UserInterface/MenuContent/Popups/StandardPopup"));

                ourShowUiStandardPopup1Action = (ShowUiStandardPopup1Action)Delegate.CreateDelegate(typeof(ShowUiStandardPopup1Action), VRCUiPopupManager.prop_VRCUiPopupManager_0, targetMethod);

                return ourShowUiStandardPopup1Action;
            }
        }
        #endregion
        #region Standard Popup with middle button
        public delegate void ShowUiStandardPopup2Action(string title, string body, string middleButtonText, Il2CppSystem.Action middleButtonAction, Il2CppSystem.Action<VRCUiPopup> onPopupShown = null);

        private static ShowUiStandardPopup2Action ourShowUiStandardPopup2Action;

        public static ShowUiStandardPopup2Action ShowUiStandardPopup2
        {
            get
            {
                if (ourShowUiStandardPopup2Action != null) return ourShowUiStandardPopup2Action;

                var targetMethod = typeof(VRCUiPopupManager).GetMethods(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public)
                    .FirstOrDefault(it => it.GetParameters().Length == 5 && !it.Name.Contains("PDM") && XrefScanner.XrefScan(it).Any(jt => jt.Type == XrefType.Global && jt.ReadAsObject()?.ToString() == "UserInterface/MenuContent/Popups/StandardPopup"));

                ourShowUiStandardPopup2Action = (ShowUiStandardPopup2Action)Delegate.CreateDelegate(typeof(ShowUiStandardPopup2Action), VRCUiPopupManager.prop_VRCUiPopupManager_0, targetMethod);

                return ourShowUiStandardPopup2Action;
            }
        }
        #endregion
        #region Standrd Popup with two buttons
        public delegate void ShowUiStandardPopup3Action(string title, string body, string leftButtonText, Il2CppSystem.Action leftButtonAction, string rightButtonText, Il2CppSystem.Action rightButtonAction, Il2CppSystem.Action<VRCUiPopup> onPopupShown = null);

        private static ShowUiStandardPopup3Action ourShowUiStandardPopup3Action;

        public static ShowUiStandardPopup3Action ShowUiStandardPopup3
        {
            get
            {
                if (ourShowUiStandardPopup3Action != null) return ourShowUiStandardPopup3Action;

                var targetMethod = typeof(VRCUiPopupManager).GetMethods(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public)
                    .FirstOrDefault(it => it.GetParameters().Length == 7 && !it.Name.Contains("PDM") && XrefScanner.XrefScan(it).Any(jt => jt.Type == XrefType.Global && jt.ReadAsObject()?.ToString() == "UserInterface/MenuContent/Popups/StandardPopup"));

                ourShowUiStandardPopup3Action = (ShowUiStandardPopup3Action)Delegate.CreateDelegate(typeof(ShowUiStandardPopup3Action), VRCUiPopupManager.prop_VRCUiPopupManager_0, targetMethod);

                return ourShowUiStandardPopup3Action;
            }
        }
        #endregion
        #region Standard Popup v2 with middle button
        public delegate void ShowUiStandardPopupV21Action(string title, string body, string middleButtonText, Il2CppSystem.Action middleButtonAction, Il2CppSystem.Action<VRCUiPopup> onPopupShown = null);

        private static ShowUiStandardPopupV21Action ourShowUiStandardPopupV21Action;

        public static ShowUiStandardPopupV21Action ShowUiStandardPopupV21
        {
            get
            {
                if (ourShowUiStandardPopupV21Action != null) return ourShowUiStandardPopupV21Action;

                var targetMethod = typeof(VRCUiPopupManager).GetMethods(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public)
                    .FirstOrDefault(it => it.GetParameters().Length == 5 && !it.Name.Contains("PDM") && XrefScanner.XrefScan(it).Any(jt => jt.Type == XrefType.Global && jt.ReadAsObject()?.ToString() == "UserInterface/MenuContent/Popups/StandardPopupV2"));

                ourShowUiStandardPopupV21Action = (ShowUiStandardPopupV21Action)Delegate.CreateDelegate(typeof(ShowUiStandardPopupV21Action), VRCUiPopupManager.prop_VRCUiPopupManager_0, targetMethod);

                return ourShowUiStandardPopupV21Action;
            }
        }
        #endregion
        #region Standard Popup v2 with two buttons
        public delegate void ShowUiStandardPopupV22Action(string title, string body, string leftButtonText, Il2CppSystem.Action leftButtonAction, string rightButtonText, Il2CppSystem.Action rightButtonAction, Il2CppSystem.Action<VRCUiPopup> onPopupShown = null);

        private static ShowUiStandardPopupV22Action ourShowUiStandardPopupV22Action;

        public static ShowUiStandardPopupV22Action ShowUiStandardPopupV22
        {
            get
            {
                if (ourShowUiStandardPopupV22Action != null) return ourShowUiStandardPopupV22Action;

                var targetMethod = typeof(VRCUiPopupManager).GetMethods(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public)
                    .FirstOrDefault(it => it.GetParameters().Length == 7 && !it.Name.Contains("PDM") && XrefScanner.XrefScan(it).Any(jt => jt.Type == XrefType.Global && jt.ReadAsObject()?.ToString() == "UserInterface/MenuContent/Popups/StandardPopupV2"));

                ourShowUiStandardPopupV22Action = (ShowUiStandardPopupV22Action)Delegate.CreateDelegate(typeof(ShowUiStandardPopupV22Action), VRCUiPopupManager.prop_VRCUiPopupManager_0, targetMethod);

                return ourShowUiStandardPopupV22Action;
            }
        }
        #endregion
        #region Alert Popup
        public delegate void ShowUiAlertPopupAction(string title, string body, float timeout);

        private static ShowUiAlertPopupAction ourShowUiAlertPopupAction;

        public static ShowUiAlertPopupAction ShowUiAlertPopup
        {
            get
            {
                if (ourShowUiAlertPopupAction != null) return ourShowUiAlertPopupAction;

                var targetMethod = typeof(VRCUiPopupManager).GetMethods(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public)
                    .FirstOrDefault(it => it.GetParameters().Length == 3 && XrefScanner.XrefScan(it).Any(jt => jt.Type == XrefType.Global && jt.ReadAsObject()?.ToString() == "UserInterface/MenuContent/Popups/AlertPopup"));

                ourShowUiAlertPopupAction = (ShowUiAlertPopupAction)Delegate.CreateDelegate(typeof(ShowUiAlertPopupAction), VRCUiPopupManager.prop_VRCUiPopupManager_0, targetMethod);

                return ourShowUiAlertPopupAction;
            }
        }
        #endregion
    }


}
