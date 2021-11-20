using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using emmVRC.Utils;
using emmVRC.Objects.ModuleBases;
using UnityEngine;
using UnityEngine.UI;


namespace emmVRC.Functions.UI
{
    [Priority(50)]
    public class ShortcutMenuCloseOptions : MelonLoaderEvents
    {
        private static bool _initialized;
        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            if (buildIndex != -1 || _initialized) return;
            ButtonAPI.GetQuickMenuInstance().transform.Find("Container/Window/QMParent/Menu_Settings/QMHeader_H1/RightItemContainer/Button_QM_Exit/").GetComponent<Button>().onClick = new Button.ButtonClickedEvent();
            ButtonAPI.GetQuickMenuInstance().transform.Find("Container/Window/QMParent/Menu_Settings/QMHeader_H1/RightItemContainer/Button_QM_Exit/").GetComponent<Button>().onClick.AddListener(new System.Action(() => {
                ButtonAPI.GetQuickMenuInstance().ShowCustomDialog("Exit", "Really exit VRChat?", "Quit", "Restart", "Cancel", Functions.Other.DestructiveActions.ForceQuit, Functions.Other.DestructiveActions.ForceRestart, null);
            }));
            _initialized = true;
        }
    }
}
