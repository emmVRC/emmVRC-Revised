using emmVRC.Libraries;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using VRC.Core;
using VRC.Udon.Wrapper.Modules;
using emmVRC.Objects.ModuleBases;

namespace emmVRC.Managers
{
    public class KeybindManager : MelonLoaderEvents, IWithUpdate
    {
        private static bool keyFlag;

        public void OnUpdate()
        {
            //if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.H))
            //{
            //    Configuration.WriteConfigOption("UIVisible", !Configuration.JSONConfig.UIVisible);
            //    Functions.UI.UIElementsMenuLegacy.ToggleHUD.setToggleState(Configuration.JSONConfig.HUDEnabled);
            //}
            if (Configuration.JSONConfig.EnableKeybinds)
            {
                if (RiskyFunctionsManager.AreRiskyFunctionsAllowed && Configuration.JSONConfig.RiskyFunctionsEnabled)
                {
                    // If the flight keybind is pressed...
                    if ((Input.GetKey((UnityEngine.KeyCode)Configuration.JSONConfig.FlightKeybind[1]) || (KeyCode)Configuration.JSONConfig.FlightKeybind[1] == KeyCode.None) && Input.GetKey((UnityEngine.KeyCode)Configuration.JSONConfig.FlightKeybind[0]) && !keyFlag)
                    {
                        Functions.PlayerHacks.Flight.SetFlyActive(!Functions.PlayerHacks.Flight.IsFlyEnabled);
                        keyFlag = true;
                    }
                    // If the noclip keybind is pressed...
                    if ((Input.GetKey((UnityEngine.KeyCode)Configuration.JSONConfig.NoclipKeybind[1]) || (KeyCode)Configuration.JSONConfig.NoclipKeybind[1] == KeyCode.None) && Input.GetKey((UnityEngine.KeyCode)Configuration.JSONConfig.NoclipKeybind[0]) && !keyFlag)
                    {
                        Functions.PlayerHacks.Flight.SetNoClipActive(!Functions.PlayerHacks.Flight.IsNoClipEnabled);
                        keyFlag = true;
                    }
                    // If the Speed keybind is pressed...
                    if ((Input.GetKey((UnityEngine.KeyCode)Configuration.JSONConfig.SpeedKeybind[1]) || (KeyCode)Configuration.JSONConfig.SpeedKeybind[1] == KeyCode.None) && Input.GetKey((UnityEngine.KeyCode)Configuration.JSONConfig.SpeedKeybind[0]) && !keyFlag)
                    {
                        Functions.PlayerHacks.Speed.SetActive(!Functions.PlayerHacks.Speed.IsEnabled);
                        keyFlag = true;
                    }
                }
                if ((Input.GetKey((KeyCode)Configuration.JSONConfig.ThirdPersonKeybind[1]) || (UnityEngine.KeyCode)Configuration.JSONConfig.ThirdPersonKeybind[1] == KeyCode.None) && Input.GetKey((KeyCode)Configuration.JSONConfig.ThirdPersonKeybind[0]) && !keyFlag)
                {
                    if (Functions.View.ThirdPerson.CameraSetup != 2)
                    {
                        Functions.View.ThirdPerson.CameraSetup++;
                        Functions.View.ThirdPerson.TPCameraBack.transform.position -= Functions.View.ThirdPerson.TPCameraBack.transform.forward * Functions.View.ThirdPerson.zoomOffset;
                        Functions.View.ThirdPerson.TPCameraFront.transform.position += Functions.View.ThirdPerson.TPCameraBack.transform.forward * Functions.View.ThirdPerson.zoomOffset;
                        Functions.View.ThirdPerson.zoomOffset = 0f;
                        Functions.View.ThirdPerson.ChangeCameraView();
                        keyFlag = true;
                    }
                    else
                    {
                        Functions.View.ThirdPerson.CameraSetup = 0;
                        Functions.View.ThirdPerson.TPCameraBack.transform.position -= Functions.View.ThirdPerson.TPCameraBack.transform.forward * Functions.View.ThirdPerson.zoomOffset;
                        Functions.View.ThirdPerson.TPCameraFront.transform.position += Functions.View.ThirdPerson.TPCameraBack.transform.forward * Functions.View.ThirdPerson.zoomOffset;
                        Functions.View.ThirdPerson.zoomOffset = 0f;
                        Functions.View.ThirdPerson.ChangeCameraView();
                        keyFlag = true;
                    }
                }
                if ((Input.GetKey((KeyCode)Configuration.JSONConfig.GoHomeKeybind[1]) || (UnityEngine.KeyCode)Configuration.JSONConfig.GoHomeKeybind[1] == KeyCode.None) && Input.GetKey((KeyCode)Configuration.JSONConfig.GoHomeKeybind[0]) && !keyFlag)
                {
                    QuickMenuUtils.GetQuickMenuInstance().transform.Find("ShortcutMenu/GoHomeButton").GetComponent<Button>().onClick.Invoke();
                    keyFlag = true;
                }
                if ((Input.GetKey((KeyCode)Configuration.JSONConfig.RespawnKeybind[1]) || (UnityEngine.KeyCode)Configuration.JSONConfig.RespawnKeybind[1] == KeyCode.None) && Input.GetKey((KeyCode)Configuration.JSONConfig.RespawnKeybind[0]) && !keyFlag)
                {
                    QuickMenuUtils.GetQuickMenuInstance().transform.Find("ShortcutMenu/RespawnButton").GetComponent<Button>().onClick.Invoke();
                    keyFlag = true;
                }
                if (!Input.GetKey((UnityEngine.KeyCode)Configuration.JSONConfig.FlightKeybind[0]) && !Input.GetKey((UnityEngine.KeyCode)Configuration.JSONConfig.NoclipKeybind[0]) && !Input.GetKey((UnityEngine.KeyCode)Configuration.JSONConfig.SpeedKeybind[0]) && !Input.GetKey((KeyCode)Configuration.JSONConfig.ThirdPersonKeybind[0]) && !Input.GetKey((KeyCode)Configuration.JSONConfig.GoHomeKeybind[0]) && !Input.GetKey((KeyCode)Configuration.JSONConfig.RespawnKeybind[0]) && keyFlag)
                    keyFlag = false;
            }
        }
    }
}
