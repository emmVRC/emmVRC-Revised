using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using emmVRC.Network;
using UnityEngine.UI;
using emmVRC.Menus;

namespace emmVRC.Managers
{
    public class RiskyFunctionsManager
    {
        private static bool riskyFuncsAllowed = false;
        public static bool RiskyFunctionsAllowed { get { return riskyFuncsAllowed; } }
        private static bool RiskyFunctionsChecked = false;
        public static Il2CppSystem.Action<string> worldTagsCheck = null;

        private static Button FlightButton;
        private static Button NoclipButton;
        private static Button SpeedButton;
        private static Button ESPButton;

        public static void Initialize()
        {
            MelonLoader.MelonCoroutines.Start(Loop());
        }

        public static IEnumerator Loop()
        {
            while (true)
            {

                // Check if we are in a world; if not, we need to wait until we are.
                if (RoomManager.field_Internal_Static_ApiWorld_0 != null)
                {
                    // Check if we have processed what the status of Risky Functions should be
                    if (!RiskyFunctionsChecked)
                    {
                        if (RiskyFunctionsAllowed)
                        {
                            Menus.PlayerTweaksMenu.SetRiskyFunctions(true);
                            Menus.UserTweaksMenu.SetRiskyFunctions(true);
                        } else
                        {
                            Menus.PlayerTweaksMenu.SetRiskyFunctions(false);
                            Menus.UserTweaksMenu.SetRiskyFunctions(false);
                        }
                        RiskyFunctionsChecked = true;
                    }
                    try
                    {
                        // This does stuff
                        if (FlightButton == null)
                            FlightButton = PlayerTweaksMenu.FlightToggle.getGameObject().GetComponent<Button>();
                        else if (NoclipButton == null)
                            NoclipButton = PlayerTweaksMenu.NoclipToggle.getGameObject().GetComponent<Button>();
                        else if (SpeedButton == null)
                            SpeedButton = PlayerTweaksMenu.SpeedToggle.getGameObject().GetComponent<Button>();
                        else if (ESPButton == null)
                            ESPButton = PlayerTweaksMenu.ESPToggle.getGameObject().GetComponent<Button>();
                        else
                        {
                            if ((FlightButton.enabled || NoclipButton.enabled || SpeedButton.enabled || ESPButton.enabled) && (!RiskyFunctionsAllowed || !Configuration.JSONConfig.RiskyFunctionsEnabled))
                            {
                                RiskyFunctionsChecked = false;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        emmVRCLoader.Logger.LogError(ex.ToString());
                    }
                    
                }
                yield return new WaitForEndOfFrame();
            }
        }
        public static IEnumerator CheckWorld()
        {
            // Wait for the room manager to become available, meaning the player is in the world
            while (RoomManager.field_Internal_Static_ApiWorld_0 == null)
                yield return new WaitForEndOfFrame();

            // Check if we are in a VRCSDK3 world, or if Risky Functions are even on; Risky Functions are (currently) not compatible with VRCSDK3 worlds, so we will not enable risky functions if this is the case
            if (Configuration.JSONConfig.RiskyFunctionsEnabled)
            {
                riskyFuncsAllowed = false;
                RiskyFunctionsChecked = false;
                // Temporary boolean that we will set if the world is whitelisted or blacklisted, to disable our later check.
                bool temp = false;

                var thing = HTTPRequest.get("https://www.thetrueyoshifan.com/RiskyFuncsCheck.php?userid=" + VRC.Core.APIUser.CurrentUser.id + "&worldid=" + RoomManager.field_Internal_Static_ApiWorld_0.id);
                while (!thing.IsCompleted && !thing.IsFaulted)
                    yield return new WaitForEndOfFrame();

                if (!thing.IsFaulted)
                {
                    try
                    {
                        // If the world is whitelisted, "allowed" will be returned. This skips the tag check and enables Risky Functions outright
                        if (thing.Result == "allowed")
                        {
                            temp = true;
                            riskyFuncsAllowed = true;
                        }
                        // If the world is blacklisted, or the user is banned, "denied" is returned. This skips the tag check and disables Risky Functions outright
                        else if (thing.Result == "denied")
                        {
                            temp = true;
                            riskyFuncsAllowed = false;
                        }
                    }
                    catch (System.Exception ex)
                    {
                        emmVRCLoader.Logger.LogError(ex.ToString());
                    }
                }
                else
                {
                    emmVRCLoader.Logger.LogError("Asynchronous net request failed: " + thing.Exception);

                }

                // If the temp flag isn't set, perform the tag check
                if (!temp)
                {
                    List<string> lowerTags = new List<string>();
                    foreach (string str in RoomManager.field_Internal_Static_ApiWorld_0.tags)
                        lowerTags.Add(str.ToLower());
                    if (lowerTags.IndexOf("author_tag_game") != -1 || lowerTags.IndexOf("author_tag_games") != -1 || lowerTags.IndexOf("author_tag_club") != -1 || lowerTags.IndexOf("admin_game") != -1)
                    {
                        riskyFuncsAllowed = false;
                    }
                    else
                    {
                        riskyFuncsAllowed = true;
                    }
                    lowerTags.Clear();
                }
                // Now have Risky Functions reprocess based on this result
                RiskyFunctionsChecked = false;
            }
        }
    }
}
