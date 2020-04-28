using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using emmVRC.Libraries;

namespace emmVRC.Menus
{
    public class LoadingScreenMenu
    {
        public static GameObject functionsButton;
        public static void Initialize()
        {
            functionsButton = GameObject.Instantiate(QuickMenuUtils.GetVRCUiMInstance().menuContent.transform.Find("Popups/LoadingPopup/ButtonMiddle"), QuickMenuUtils.GetVRCUiMInstance().menuContent.transform.Find("Popups/LoadingPopup")).gameObject;
            functionsButton.GetComponent<RectTransform>().anchoredPosition += new Vector2(0f, -128f);
            functionsButton.SetActive(Configuration.JSONConfig.ForceRestartButtonEnabled);
            functionsButton.name = "LoadingFunctionsButton";
            functionsButton.GetComponentInChildren<Text>().text = "Force Restart";
            functionsButton.GetComponent<Button>().onClick = new Button.ButtonClickedEvent();
            functionsButton.GetComponent<Button>().onClick.AddListener(UnhollowerRuntimeLib.DelegateSupport.ConvertDelegate<UnityAction>((System.Action)(() =>
            {
                DestructiveActions.ForceRestart();
            })));
        }
    }
}
