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

namespace emmVRC.Hacks
{
    public class WorldFunctions
    {
        private static GameObject WorldNotesButton;
        public static void Initialize()
        {
            WorldNotesButton = GameObject.Instantiate(GameObject.Find("MenuContent/Screens/WorldInfo/ReportButton"), GameObject.Find("MenuContent/Screens/WorldInfo").transform);
            WorldNotesButton.GetComponentInChildren<Text>().text = "Notes";
            WorldNotesButton.GetComponent<RectTransform>().anchoredPosition -= new Vector2(0, 125f);
            WorldNotesButton.SetActive(true);
            WorldNotesButton.GetComponentInChildren<Button>().onClick = new Button.ButtonClickedEvent();
            WorldNotesButton.GetComponentInChildren<Button>().onClick.AddListener(new System.Action(() => {
                WorldNotes.LoadNote(QuickMenuUtils.GetVRCUiMInstance().menuContent().GetComponentInChildren<PageWorldInfo>().field_Private_ApiWorld_0.id, QuickMenuUtils.GetVRCUiMInstance().menuContent().GetComponentInChildren<PageWorldInfo>().field_Private_ApiWorld_0.name);
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
