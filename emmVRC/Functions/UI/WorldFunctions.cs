using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using emmVRC.Libraries;
using VRC.UI;
using emmVRC.Objects.ModuleBases;

namespace emmVRC.Functions.UI
{
    public class WorldFunctions : MelonLoaderEvents
    {
        private static GameObject WorldNotesButton;
        private static GameObject ViewFullDescButton;
        public override void OnUiManagerInit()
        {
            if (Configuration.JSONConfig.StealthMode) return;
            WorldNotesButton = GameObject.Instantiate(GameObject.Find("MenuContent/Screens/WorldInfo/ReportButton"), GameObject.Find("MenuContent/Screens/WorldInfo").transform);
            WorldNotesButton.GetComponentInChildren<Text>().text = "Notes";
            WorldNotesButton.GetComponent<RectTransform>().anchoredPosition -= new Vector2(0, 130f);
            WorldNotesButton.SetActive(true);
            WorldNotesButton.GetComponentInChildren<Button>().onClick = new Button.ButtonClickedEvent();
            WorldNotesButton.GetComponentInChildren<Button>().onClick.AddListener(new System.Action(() => {
                Functions.UI.WorldNotes.LoadNote(QuickMenuUtils.GetVRCUiMInstance().menuContent().GetComponentInChildren<PageWorldInfo>().field_Private_ApiWorld_0.id, QuickMenuUtils.GetVRCUiMInstance().menuContent().GetComponentInChildren<PageWorldInfo>().field_Private_ApiWorld_0.name);
            }));
            ViewFullDescButton = GameObject.Instantiate(GameObject.Find("MenuContent/Screens/WorldInfo/ReportButton"), GameObject.Find("MenuContent/Screens/WorldInfo").transform);
            ViewFullDescButton.transform.Find("Text").GetComponent<RectTransform>().offsetMax = new Vector2(-70.45f, 10f);
            ViewFullDescButton.transform.Find("Text").GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 0f);
            ViewFullDescButton.GetComponentInChildren<Text>().text = "View Full";
            ViewFullDescButton.GetComponentInChildren<Text>().resizeTextForBestFit = true;
            ViewFullDescButton.GetComponent<RectTransform>().offsetMax = new Vector2(-400f, 100f);
            ViewFullDescButton.GetComponent<RectTransform>().offsetMin = new Vector2(-650f, 35f);

            //ViewFullDescButton.GetComponent<RectTransform>().anchorMax -= new Vector2(0f, 25f);
            ViewFullDescButton.SetActive(true);
            ViewFullDescButton.GetComponentInChildren<Button>().onClick = new Button.ButtonClickedEvent();
            ViewFullDescButton.GetComponentInChildren<Button>().onClick.AddListener(new System.Action(() =>
            {
                VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowStandardPopupV2("Description for " + QuickMenuUtils.GetVRCUiMInstance().menuContent().GetComponentInChildren<PageWorldInfo>().field_Private_ApiWorld_0.name, QuickMenuUtils.GetVRCUiMInstance().menuContent().GetComponentInChildren<PageWorldInfo>().field_Private_ApiWorld_0.description, "Close", () => { VRCUiPopupManager.prop_VRCUiPopupManager_0.HideCurrentPopup(); });
            }));
        }

        internal static void ToggleItemESP(bool toggle)
        {
            foreach (var gameObject in Hacks.ComponentToggle.Pickup_stored)
            {
                Renderer bubbleRenderer = gameObject.GetComponent<Renderer>();
                if (bubbleRenderer != null)
                {
                    HighlightsFX.prop_HighlightsFX_0.Method_Public_Void_Renderer_Boolean_0(bubbleRenderer, toggle);
                }
            }
        }

        internal static void ToggleTriggerESP(bool toggle)
        {
            foreach (var gameObject in Hacks.ComponentToggle.Trigger_stored)
            {
                Renderer bubbleRenderer = gameObject.GetComponent<Renderer>();
                if (bubbleRenderer != null)
                {
                    HighlightsFX.prop_HighlightsFX_0.Method_Public_Void_Renderer_Boolean_0(bubbleRenderer, toggle);
                }
            }
        }
    }
}
