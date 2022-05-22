using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using emmVRC.Utils;
using emmVRC.Objects.ModuleBases;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace emmVRC.Menus
{
    [Priority(55)]
    public class KeybindAdjustmentMenu : MelonLoaderEvents, IWithUpdate
    {
        private static bool _initialized = false;
        private static MenuPage keybindAdjustmentPage;
        private static TextMeshProUGUI textText;
        private static ButtonGroup displayGroup;
        private static SimpleSingleButton modifierKeyDisplay;
        private static SimpleSingleButton mainKeyDisplay;
        private static Action<int[]> onKeybindAccepted;
        private static KeyCode currentMainKey;
        private static KeyCode currentModifier;
        private static bool modifier;
        private static List<KeyCode> allowedKeyCodes = new List<KeyCode> { KeyCode.Q, KeyCode.E, KeyCode.R, KeyCode.T, KeyCode.Y, KeyCode.U, KeyCode.I, KeyCode.O, KeyCode.P, KeyCode.F, KeyCode.G, KeyCode.H, KeyCode.J, KeyCode.K, KeyCode.L, KeyCode.Z, KeyCode.X, KeyCode.B, KeyCode.N, KeyCode.M, KeyCode.Alpha0, KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9, KeyCode.Tilde, KeyCode.Minus, KeyCode.Plus, KeyCode.Backslash, KeyCode.Slash, KeyCode.Period, KeyCode.Comma, KeyCode.F1, KeyCode.F2, KeyCode.F3, KeyCode.F4, KeyCode.F5, KeyCode.F6, KeyCode.F7, KeyCode.F8, KeyCode.F9, KeyCode.F10, KeyCode.F11, KeyCode.F12 };
        private static List<KeyCode> allowedKeyModifiers = new List<KeyCode> { KeyCode.LeftControl, KeyCode.LeftCommand, KeyCode.LeftAlt, KeyCode.LeftShift, KeyCode.RightControl, KeyCode.RightCommand, KeyCode.RightAlt, KeyCode.RightShift };
        private static bool menuOpen = false;

        public override void OnSceneWasInitialized(int buildIndex, string sceneName)
        {
            if (buildIndex != -1 || _initialized) return;

            keybindAdjustmentPage = new MenuPage("emmVRC_KeybindAdjustment", "Keybind", false, true, true, () =>
            {
                onKeybindAccepted?.Invoke(new int[] { (int)currentMainKey, (int)currentModifier });
                keybindAdjustmentPage.CloseMenu();
            }, "Save the keybind", ButtonAPI.onIconSprite);

            GameObject textBase = new GameObject("ChangelogText");
            textBase.transform.SetParent(keybindAdjustmentPage.menuContents);
            textBase.transform.localPosition = Vector3.zero;
            textBase.transform.localRotation = new Quaternion(0, 0, 0, 0);
            textBase.transform.localScale = Vector3.one;
            TextMeshProUGUI textText = textBase.AddComponent<TextMeshProUGUI>();
            textText.margin = new Vector4(25, 0, 50, 0);
            textText.text = "<size=25>Please press the new keys for this keybind</size>";

            displayGroup = new ButtonGroup(keybindAdjustmentPage, "");

            modifierKeyDisplay = new SimpleSingleButton(displayGroup, "CTRL", null, "");
            mainKeyDisplay = new SimpleSingleButton(displayGroup, "A", null, "");
            modifierKeyDisplay.SetInteractable(false);
            mainKeyDisplay.SetInteractable(false);

            Components.EnableDisableListener listener = keybindAdjustmentPage.menuContents.gameObject.AddComponent<Components.EnableDisableListener>();
            listener.OnEnabled += () =>
            {
                menuOpen = true;
            };
            listener.OnDisabled += () =>
            {
                menuOpen = false;
            };

            _initialized = true;
        }
        public static void ShowMenu(int[] currentKeybind, Action<int[]> onAccept)
        {
            onKeybindAccepted = onAccept;
            currentMainKey = (KeyCode)currentKeybind[0];
            currentModifier = (KeyCode)currentKeybind[1];

            keybindAdjustmentPage.OpenMenu();
        }
        public void OnUpdate()
        {
            if (!menuOpen) return;
            if (Input.anyKey)
            {
                bool modifier = false;
                foreach (KeyCode mKey in allowedKeyModifiers)
                {
                    if (Input.GetKey(mKey))
                    {
                        currentModifier = mKey;
                        modifier = true;
                    }
                }
                foreach (KeyCode vKey in allowedKeyCodes)
                {
                    if (Input.GetKey(vKey))
                    {
                        currentMainKey = vKey;
                        if (!modifier)
                            currentModifier = KeyCode.None;
                    }
                }
                modifierKeyDisplay.SetText(Libraries.KeyCodeConversion.Stringify(currentModifier));
                mainKeyDisplay.SetText(currentMainKey.ToString());

            }
        }
    }
}
