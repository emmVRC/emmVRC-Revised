using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using emmVRC.Libraries;
using emmVRC.Objects.ModuleBases;

namespace emmVRC.Functions.UI
{
    public class LoadingScreen : MelonLoaderEvents
    {
        public static GameObject functionsButton;
        public override void OnUiManagerInit()
        {
            GameObject LoadingPopupBase = UnityEngine.Resources.FindObjectsOfTypeAll<VRCUiPageLoading>().FirstOrDefault().gameObject;

            functionsButton = GameObject.Instantiate(LoadingPopupBase.transform.Find("ButtonMiddle").gameObject, LoadingPopupBase.transform);
            functionsButton.GetComponent<RectTransform>().anchoredPosition += new Vector2(0f, -128f);
            functionsButton.SetActive(Configuration.JSONConfig.ForceRestartButtonEnabled);
            functionsButton.name = "LoadingFunctionsButton";
            functionsButton.GetComponentInChildren<Text>().text = "Force Restart";
            functionsButton.GetComponent<Button>().onClick = new Button.ButtonClickedEvent();
            functionsButton.GetComponent<Button>().onClick.AddListener(UnhollowerRuntimeLib.DelegateSupport.ConvertDelegate<UnityAction>((System.Action)(() =>
            {
                Functions.Other.DestructiveActions.ForceRestart();
            })));
        }
    }
}
