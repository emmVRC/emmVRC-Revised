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

namespace emmVRC.Managers
{
    public class KeybindManager
    {
        private static bool keyFlag;
        public static void Initialize()
        {
            MelonLoader.MelonCoroutines.Start(Loop());
        }
        private static IEnumerator Loop()
        {
            while (true)
            {
                if (Configuration.JSONConfig.EnableKeybinds)
                {
                    if (RiskyFunctionsManager.RiskyFuncsAreAllowed && Configuration.JSONConfig.RiskyFunctionsEnabled)
                    {
                        // If the flight keybind is pressed...
                        if ((Input.GetKey((UnityEngine.KeyCode)Configuration.JSONConfig.FlightKeybind[1]) || (KeyCode)Configuration.JSONConfig.FlightKeybind[1] == KeyCode.None) && Input.GetKey((UnityEngine.KeyCode)Configuration.JSONConfig.FlightKeybind[0]) && !keyFlag)
                        {
                            Menus.PlayerTweaksMenu.FlightToggle.setToggleState(!Hacks.Flight.FlightEnabled, true);
                            keyFlag = true;
                        }
                        // If the noclip keybind is pressed...
                        if ((Input.GetKey((UnityEngine.KeyCode)Configuration.JSONConfig.NoclipKeybind[1]) || (KeyCode)Configuration.JSONConfig.NoclipKeybind[1] == KeyCode.None) && Input.GetKey((UnityEngine.KeyCode)Configuration.JSONConfig.NoclipKeybind[0]) && !keyFlag)
                        {
                            Menus.PlayerTweaksMenu.NoclipToggle.setToggleState(!Hacks.Flight.NoclipEnabled, true);
                            keyFlag = true;
                        }
                        // If the Speed keybind is pressed...
                        if ((Input.GetKey((UnityEngine.KeyCode)Configuration.JSONConfig.SpeedKeybind[1]) || (KeyCode)Configuration.JSONConfig.SpeedKeybind[1] == KeyCode.None) && Input.GetKey((UnityEngine.KeyCode)Configuration.JSONConfig.SpeedKeybind[0]) && !keyFlag)
                        {
                            Menus.PlayerTweaksMenu.SpeedToggle.setToggleState(!Hacks.Speed.SpeedModified, true);
                            keyFlag = true;
                        }
                    }
                    if (Input.GetKey((KeyCode)Configuration.JSONConfig.ThirdPersonKeybind[1]) && Input.GetKey((KeyCode)Configuration.JSONConfig.ThirdPersonKeybind[0]) && !keyFlag)
                    {
                        if (Hacks.ThirdPerson.CameraSetup != 2)
                        {
                            Hacks.ThirdPerson.CameraSetup++;
                            Hacks.ThirdPerson.TPCameraBack.transform.position -= Hacks.ThirdPerson.TPCameraBack.transform.forward * Hacks.ThirdPerson.zoomOffset;
                            Hacks.ThirdPerson.TPCameraFront.transform.position += Hacks.ThirdPerson.TPCameraBack.transform.forward * Hacks.ThirdPerson.zoomOffset;
                            Hacks.ThirdPerson.zoomOffset = 0f;
                            keyFlag = true;
                        }
                        else
                        {
                            Hacks.ThirdPerson.CameraSetup = 0;
                            Hacks.ThirdPerson.TPCameraBack.transform.position -= Hacks.ThirdPerson.TPCameraBack.transform.forward * Hacks.ThirdPerson.zoomOffset;
                            Hacks.ThirdPerson.TPCameraFront.transform.position += Hacks.ThirdPerson.TPCameraBack.transform.forward * Hacks.ThirdPerson.zoomOffset;
                            Hacks.ThirdPerson.zoomOffset = 0f;
                            keyFlag = true;
                        }
                    }
                    if (Input.GetKey((KeyCode)Configuration.JSONConfig.GoHomeKeybind[1]) && Input.GetKey((KeyCode)Configuration.JSONConfig.GoHomeKeybind[0]) && !keyFlag)
                    {
                        QuickMenuUtils.GetQuickMenuInstance().transform.Find("ShortcutMenu/GoHomeButton").GetComponent<Button>().onClick.Invoke();
                        keyFlag = true;
                    }
                    if (Input.GetKey((KeyCode)Configuration.JSONConfig.RespawnKeybind[1]) && Input.GetKey((KeyCode)Configuration.JSONConfig.RespawnKeybind[0]) && !keyFlag)
                    {
                        QuickMenuUtils.GetQuickMenuInstance().transform.Find("ShortcutMenu/RespawnButton").GetComponent<Button>().onClick.Invoke();
                        keyFlag = true;
                    }
                    if (!Input.GetKey((UnityEngine.KeyCode)Configuration.JSONConfig.FlightKeybind[0]) && !Input.GetKey((UnityEngine.KeyCode)Configuration.JSONConfig.NoclipKeybind[0]) && !Input.GetKey((UnityEngine.KeyCode)Configuration.JSONConfig.SpeedKeybind[0]) && !Input.GetKey((KeyCode)Configuration.JSONConfig.ThirdPersonKeybind[0]) && !Input.GetKey((KeyCode)Configuration.JSONConfig.GoHomeKeybind[0]) && !Input.GetKey((KeyCode)Configuration.JSONConfig.RespawnKeybind[0]) && keyFlag)
                        keyFlag = false;
                }
                yield return new WaitForEndOfFrame();
            }
        }
    }
}
