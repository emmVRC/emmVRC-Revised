using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace emmVRC.Hacks
{
    public class WorldMenuFunctions
    {
        private static GameObject WorldNotesButton;
        public static void Initialize()
        {
            WorldNotesButton = GameObject.Instantiate(GameObject.Find("MenuContent/Screens/WorldInfo/ReportButton"), GameObject.Find("MenuContent/Screens/WorldInfo").transform);
            WorldNotesButton.GetComponentInChildren<Text>().text = "Notes";
            WorldNotesButton.GetComponent<RectTransform>().anchoredPosition -= new Vector2(0, 125f);
            WorldNotesButton.SetActive(true);
            WorldNotesButton.GetComponentInChildren<Button>().onClick = new Button.ButtonClickedEvent();/*
            WorldNotesButton.GetComponentInChildren<Button>().onClick.AddListener(unhollowerbas delegate ()
            {
                try
                {
                    worldNotes.openWorldNoteFuncMainMenu(VRCUiManagerUtils.GetVRCUiManager().menuContent().GetComponentInChildren<PageWorldInfo>().worldInstance.instanceWorld.id);
                }
                catch (Exception ex)
                {
                    emmVRCLoader.Logger.LogError("[emmVRC] " + ex.ToString());
                }
            });*/
        }
    }
}
