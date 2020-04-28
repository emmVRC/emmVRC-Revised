using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
namespace emmVRC.Managers
{
    public class RiskyFunctionsManager
    {
        public static bool RiskyFunctionsAllowed { get; private set; } = false;
        public static bool RiskyFunctionsChecked = false;
        public static void Initialize()
        {
            MelonLoader.MelonCoroutines.Start(Loop());
        }

        public static IEnumerator Loop()
        {
            while (true)
            {
                // Check if we are in a world; if not, we need to wait until we are.
                if (RoomManager.field_ApiWorld_0 != null)
                {
                    // Check if we have processed what the status of Risky Functions should be
                    if (!RiskyFunctionsChecked)
                    {
                        if (RiskyFunctionsAllowed)
                        {
                            Menus.PlayerTweaksMenu.SetRiskyFunctions(true);
                        } else
                        {
                            Menus.PlayerTweaksMenu.SetRiskyFunctions(false);
                        }
                        RiskyFunctionsChecked = true;

                    }

                    // Now check for keyshortcuts, if Risky Functions is allowed
                    if (RiskyFunctionsAllowed)
                    {
                        // If the flight keybind is pressed...
                        if (Input.GetKey((UnityEngine.KeyCode)Configuration.JSONConfig.FlightKeybind[1]) && Input.GetKeyDown((UnityEngine.KeyCode)Configuration.JSONConfig.FlightKeybind[0])){
                            Menus.PlayerTweaksMenu.FlightToggle.setToggleState(!Hacks.Flight.FlightEnabled, true);
                        }
                        // If the noclip keybind is pressed...
                        if (Input.GetKey((UnityEngine.KeyCode)Configuration.JSONConfig.NoclipKeybind[1]) && Input.GetKeyDown((UnityEngine.KeyCode)Configuration.JSONConfig.NoclipKeybind[0]))
                        {
                            Menus.PlayerTweaksMenu.NoclipToggle.setToggleState(!Hacks.Flight.NoclipEnabled, true);
                        }
                        // If the Speed keybind is pressed...
                        if (Input.GetKey((UnityEngine.KeyCode)Configuration.JSONConfig.SpeedKeybind[1]) && Input.GetKeyDown((UnityEngine.KeyCode)Configuration.JSONConfig.SpeedKeybind[0]))
                        {
                            Menus.PlayerTweaksMenu.SpeedToggle.setToggleState(!Hacks.Speed.SpeedModified, true);
                        }
                    }

                }
                yield return new WaitForEndOfFrame();
            }
        }
        public static IEnumerator CheckWorld()
        {
            // Wait for the room manager to become available, meaning the player is in the world
            while (RoomManager.field_ApiWorld_0 == null)
                yield return new WaitForEndOfFrame();

            // Check if we are in a VRCSDK3 world, or if Risky Functions are even on; Risky Functions are (currently) not compatible with VRCSDK3 worlds, so we will not enable risky functions if this is the case
            if (Transform.FindObjectOfType<VRC.SDK3.Components.VRCSceneDescriptor>() == null && Configuration.JSONConfig.RiskyFunctionsEnabled)
            {
                // Temporary boolean that we will set if the world is whitelisted or blacklisted, to disable our later check.
                bool temp = false;

                // Sets up the UnityWebRequest to the remote server. TODO: Make this HTTPS://
                UnityWebRequest req = UnityWebRequest.Get("http://www.thetrueyoshifan.com/RiskyFuncsCheck.php?userid=" + VRC.Core.APIUser.CurrentUser.id + "&worldid=" + RoomManager.field_ApiWorld_0.id);
                
                // Sends the web request async
                req.SendWebRequest();
                
                // Allow the rest of the game to run while we're awaiting our response
                while (!req.isDone)
                    yield return new WaitForEndOfFrame();

                // If there is an error, risky functions should be disabled; this could mean the server is down, or the server response is being tampered with or blocked
                if (req.responseCode != 200)
                {
                    emmVRCLoader.Logger.LogError("Network error occured while checking Risky Functions status: " + req.error);
                    emmVRCLoader.Logger.LogError("Please check your internet connection and firewall settings.");
                    Configuration.JSONConfig.RiskyFunctionsEnabled = false;
                    Configuration.SaveConfig();
                    NotificationManager.AddNotification("A network error occured while checking this world. Risky Functions have been disabled. Please check your firewall or antivirus, and restart your game.", "Dismiss", () => { NotificationManager.DismissCurrentNotification(); }, "", null, Resources.errorSprite, -1);
                    temp = true;
                }
                else
                {
                    // If the world is whitelisted, "allowed" will be returned. This skips the tag check and enables Risky Functions outright
                    if (Il2CppSystem.Text.Encoding.UTF8.GetString(req.downloadHandler.data) == "allowed")
                    {
                        temp = true;
                        RiskyFunctionsAllowed = true;
                    }
                    // If the world is blacklisted, or the user is banned, "denied" is returned. This skips the tag check and disables Risky Functions outright
                    else if (Il2CppSystem.Text.Encoding.UTF8.GetString(req.downloadHandler.data) == "denied")
                    {
                        temp = true;
                        RiskyFunctionsAllowed = false;
                    }
                }
                // If the temp flag isn't set, perform the tag check
                if (!temp)
                    foreach (string tag in RoomManager.field_ApiWorld_0.tags)
                    {
                        if (tag == "author_tag_game" || tag == "admin_game")
                            RiskyFunctionsAllowed = false;
                    }

                // Now have Risky Functions reprocess based on this result
                RiskyFunctionsChecked = false;
            }
        }
    }
}
