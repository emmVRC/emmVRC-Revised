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
                        if (Functions.PlayerHacks.Flight.IsNoClipEnabled && Functions.PlayerHacks.Flight.IsFlyEnabled)
                            Functions.PlayerHacks.Flight.SetNoClipActive(false);
                        Functions.PlayerHacks.Flight.SetFlyActive(!Functions.PlayerHacks.Flight.IsFlyEnabled);
                        keyFlag = true;
                    }
                    // If the noclip keybind is pressed...
                    if ((Input.GetKey((UnityEngine.KeyCode)Configuration.JSONConfig.NoclipKeybind[1]) || (KeyCode)Configuration.JSONConfig.NoclipKeybind[1] == KeyCode.None) && Input.GetKey((UnityEngine.KeyCode)Configuration.JSONConfig.NoclipKeybind[0]) && !keyFlag)
                    {
                        if (!Functions.PlayerHacks.Flight.IsFlyEnabled && !Functions.PlayerHacks.Flight.IsNoClipEnabled)
                            Functions.PlayerHacks.Flight.SetFlyActive(true);
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
                    VRCUiPopupManager.prop_VRCUiPopupManager_0.ShowStandardPopup("Go Home", "Are you sure you want to return to your home world?", "Yes", () => { VRCFlowManager.prop_VRCFlowManager_0.Method_Public_Void_1(); }, "No", VRCUiPopupManager.prop_VRCUiPopupManager_0.HideCurrentPopup);
                    //Utils.ButtonAPI.menuPageBase.transform.Find("ScrollRect/Viewport/VerticalLayoutGroup/Buttons_QuickActions").Find("Button_GoHome").GetComponentInChildren<Button>().Press();
                    keyFlag = true;
                }
                if ((Input.GetKey((KeyCode)Configuration.JSONConfig.RespawnKeybind[1]) || (UnityEngine.KeyCode)Configuration.JSONConfig.RespawnKeybind[1] == KeyCode.None) && Input.GetKey((KeyCode)Configuration.JSONConfig.RespawnKeybind[0]) && !keyFlag)
                {
                    SpawnManager.field_Private_Static_SpawnManager_0.Method_Public_Void_VRCPlayer_0(VRCPlayer.field_Internal_Static_VRCPlayer_0);
                    //Utils.ButtonAPI.menuPageBase.transform.Find("ScrollRect/Viewport/VerticalLayoutGroup/Buttons_QuickActions").Find("Button_Respawn").GetComponentInChildren<Button>().Press();
                    keyFlag = true;
                }
                if (!Input.GetKey((UnityEngine.KeyCode)Configuration.JSONConfig.FlightKeybind[0]) && !Input.GetKey((UnityEngine.KeyCode)Configuration.JSONConfig.NoclipKeybind[0]) && !Input.GetKey((UnityEngine.KeyCode)Configuration.JSONConfig.SpeedKeybind[0]) && !Input.GetKey((KeyCode)Configuration.JSONConfig.ThirdPersonKeybind[0]) && !Input.GetKey((KeyCode)Configuration.JSONConfig.GoHomeKeybind[0]) && !Input.GetKey((KeyCode)Configuration.JSONConfig.RespawnKeybind[0]) && keyFlag)
                    keyFlag = false;
            }
        }
    }
}
