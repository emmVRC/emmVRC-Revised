using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace emmVRC.Libraries
{
    public class KeybindChanger
    {
        private static GameObject textObject;
        private static QMNestedButton baseMenu;
        private static QMSingleButton acceptButton;
        private static QMSingleButton cancelButton;
        private static string title;
        private static KeyCode mainKey = KeyCode.None;
        private static KeyCode modifierKey1;
        private static Action<KeyCode, KeyCode> acceptAction;
        private static Action cancelAction;
        private static bool fetchingKeys = false;
        private static List<KeyCode> allowedKeyCodes = new List<KeyCode> { KeyCode.Q, KeyCode.E, KeyCode.R, KeyCode.T, KeyCode.Y, KeyCode.U, KeyCode.I, KeyCode.O, KeyCode.P, KeyCode.F, KeyCode.G, KeyCode.H, KeyCode.J, KeyCode.K, KeyCode.L, KeyCode.Z, KeyCode.X, KeyCode.B, KeyCode.N, KeyCode.M, KeyCode.Alpha0, KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9, KeyCode.Tilde, KeyCode.Minus, KeyCode.Plus, KeyCode.Backslash, KeyCode.Slash, KeyCode.Period, KeyCode.Comma, KeyCode.F1, KeyCode.F2, KeyCode.F3, KeyCode.F4, KeyCode.F5, KeyCode.F6, KeyCode.F7, KeyCode.F8, KeyCode.F9, KeyCode.F10, KeyCode.F11, KeyCode.F12 };
        private static List<KeyCode> allowedKeyModifiers = new List<KeyCode> { KeyCode.LeftControl, KeyCode.LeftCommand, KeyCode.LeftAlt, KeyCode.LeftShift, KeyCode.RightControl, KeyCode.RightCommand, KeyCode.RightAlt, KeyCode.RightShift };
        public static bool InMenu = false;
        public static void Initialize()
        {
            baseMenu = new QMNestedButton(Menus.SettingsMenu.baseMenu.menuBase, 10293, 10239, "a02k3212", "");
            baseMenu.getMainButton().DestroyMe();

            acceptButton = new QMSingleButton(baseMenu, 1, 2, "Accept", () => { acceptAction.Invoke(mainKey, modifierKey1); fetchingKeys = false; modifierKey1 = KeyCode.None; mainKey = KeyCode.None; }, "Accept this keybind");
            cancelButton = new QMSingleButton(baseMenu, 4, 2, "Cancel", () => { cancelAction.Invoke(); fetchingKeys = false; modifierKey1 = KeyCode.None; mainKey = KeyCode.None; }, "Cancel keybind setup");
            textObject = GameObject.Instantiate(QuickMenuUtils.GetQuickMenuInstance().transform.Find("QuickMenu_NewElements/_InfoBar/EarlyAccessText").gameObject, baseMenu.getBackButton().getGameObject().transform.parent);
            baseMenu.getBackButton().DestroyMe();
            textObject.GetComponent<Text>().fontStyle = FontStyle.Normal;
            textObject.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
            textObject.GetComponent<Text>().text = "Press a key...";
            textObject.GetComponent<RectTransform>().anchoredPosition += new Vector2(580f, -920f);
            MelonLoader.MelonCoroutines.Start(Loop());
        }
        public static void Show(string title, Action<KeyCode, KeyCode> acceptAction, Action cancelAction)
        {
            KeybindChanger.title = title;
            KeybindChanger.acceptAction = acceptAction;
            KeybindChanger.cancelAction = cancelAction;
            QuickMenuUtils.ShowQuickmenuPage(baseMenu.getMenuName());
            fetchingKeys = true;
        }
        public static IEnumerator Loop()
        {
            while (true)
            {
                if (fetchingKeys)
                {
                    bool modifier = false;
                    foreach (KeyCode mKey in allowedKeyModifiers)
                    {
                        if (Input.GetKey(mKey))
                        {
                            modifierKey1 = mKey;
                            modifier = true;
                        }
                    }
                    foreach (KeyCode vKey in allowedKeyCodes)
                    {
                        if (Input.GetKey(vKey))
                        {
                            mainKey = vKey;
                            if (!modifier)
                                modifierKey1 = KeyCode.None;
                        }
                    }

                    if (textObject != null)
                        textObject.GetComponent<Text>().text = (title+"\n\n\n"+(modifierKey1 != KeyCode.None ? modifierKey1.ToString() + " + " : "") + mainKey);
                }
                yield return new WaitForEndOfFrame();
            }
        }

    }
}
