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
using VRC;
using VRC.Core;

namespace emmVRC.Managers
{
    internal class RiskyFunctionsManager
    {
        private static bool riskyFuncsAllowed = false;
        internal static bool RiskyFuncsAreAllowed { get { return riskyFuncsAllowed; } }

        private static bool RiskyFuncsAreChecked = false;
        internal static Il2CppSystem.Action<string> worldTagsCheck = null;

        private static Button FlightButton;
        private static Button NoclipButton;
        private static Button SpeedButton;
        private static Button ESPButton;
        private static Button ItemESPButton;
        private static Button TriggerESPButton;

        internal static void Initialize()
        {
            MelonLoader.MelonCoroutines.Start(Loop());
        }

        internal static IEnumerator Loop()
        {
            // This giant block of crap was an attempt to break mods that try to modify the Risky Functions checks.
            // It's unused because... well... I don't particularly care anymore. We have HarmonyShield, and if people are using those kinds of mods, we just simply don't have to support 'em.
            /*if (riskyFuncsDisable || riskyFunctionsDisable || disableAllowed || disableChecked)
            {
                Menus.PlayerTweaksMenu.FlightToggle.DestroyMe();
                Menus.PlayerTweaksMenu.NoclipToggle.DestroyMe();
                Menus.PlayerTweaksMenu.SpeedMinusButton.DestroyMe();
                Menus.PlayerTweaksMenu.SpeedPlusButton.DestroyMe();
                Menus.PlayerTweaksMenu.SpeedReset.DestroyMe();
                Menus.PlayerTweaksMenu.WaypointMenu.DestroyMe();
                Menus.PlayerTweaksMenu.ESPToggle.DestroyMe();
                Menus.PlayerTweaksMenu.EnableJumpButton.DestroyMe();
                Menus.PlayerTweaksMenu.SpeedToggle.DestroyMe();
                GameObject.Destroy(Menus.PlayerTweaksMenu.SpeedText);
                GameObject.Destroy(Menus.PlayerTweaksMenu.SpeedSlider.slider);
            } else if (!riskyFuncsDisable && !riskyFunctionsDisable && !disableAllowed && !disableChecked)*/
            while (true)
            {

                // Check if we are in a world; if not, we need to wait until we are.
                if (RoomManager.field_Internal_Static_ApiWorld_0 != null)
                {
                    // Check if we have processed what the status of Risky Functions should be
                    if (!RiskyFuncsAreChecked)
                    {
                        if (RiskyFuncsAreAllowed && Configuration.JSONConfig.RiskyFunctionsEnabled)
                        {
                            Menus.PlayerTweaksMenu.SetRiskyFuncsAllowed(true);
                            Menus.UserTweaksMenu.SetRiskyFuncsAllowed(true);
                            Menus.WorldTweaksMenu.SetRiskyFuncsAllowed(true);
                        }
                        else {
                            Menus.PlayerTweaksMenu.SetRiskyFuncsAllowed(false);
                            Menus.UserTweaksMenu.SetRiskyFuncsAllowed(false);
                            Menus.WorldTweaksMenu.SetRiskyFuncsAllowed(false);
                        }
                        RiskyFuncsAreChecked = true;
                    }
                    try
                    {
                        // Grabs the Risky Functions buttons and caches them for future use. "This does stuff" is what the previous comment was. Come on Emmy.
                        if (FlightButton == null)
                            FlightButton = PlayerTweaksMenu.FlightToggle.getGameObject().GetComponent<Button>();
                        else if (NoclipButton == null)
                            NoclipButton = PlayerTweaksMenu.NoclipToggle.getGameObject().GetComponent<Button>();
                        else if (SpeedButton == null)
                            SpeedButton = PlayerTweaksMenu.SpeedToggle.getGameObject().GetComponent<Button>();
                        else if (ESPButton == null)
                            ESPButton = PlayerTweaksMenu.ESPToggle.getGameObject().GetComponent<Button>();
                        else if (ItemESPButton == null)
                            ItemESPButton = WorldTweaksMenu.ItemESPToggle.getGameObject().GetComponent<Button>();
                        else if (TriggerESPButton == null)
                            TriggerESPButton = WorldTweaksMenu.TriggerESPToggle.getGameObject().GetComponent<Button>();
                        else
                        {
                            // Ensures that Risky Functions buttons are not enabled when they shouldn't be
                            if ((FlightButton.enabled || NoclipButton.enabled || SpeedButton.enabled || ESPButton.enabled || ItemESPButton.enabled || TriggerESPButton.enabled) && (!RiskyFuncsAreAllowed || !Configuration.JSONConfig.RiskyFunctionsEnabled))
                            {
                                RiskyFuncsAreChecked = false;
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
        internal static async Task CheckThisWrld()
        {
            // Wait for the room manager to become available, meaning the player is in the world
            while (RoomManager.field_Internal_Static_ApiWorld_0 == null)
                await emmVRC.AwaitUpdate.Yield();

            // Check if we are in a VRCSDK3 world, or if Risky Functions are even on; Risky Functions are (currently) not compatible with VRCSDK3 worlds, so we will not enable risky functions if this is the case
            if (Configuration.JSONConfig.RiskyFunctionsEnabled)
            {
                riskyFuncsAllowed = false;
                RiskyFuncsAreChecked = false;
                // Temporary boolean that we will set if the world is whitelisted or blacklisted, to disable our later check.
                bool temp = false;

                try
                {
                    var result = await HTTPRequest.get("https://dl.emmvrc.com/riskyfuncs.php?userid=" +
                                                       VRC.Core.APIUser.CurrentUser.id + "&worldid=" +
                                                       RoomManager.field_Internal_Static_ApiWorld_0.id);
                    
                    // If the world is whitelisted, "allowed" will be returned. This skips the tag check and enables Risky Functions outright
                    if (result == "allowed")
                    {
                        temp = true;
                        riskyFuncsAllowed = true;
                    }
                    // If the world is blacklisted, or the user is banned, "denied" is returned. This skips the tag check and disables Risky Functions outright
                    else if (result == "denied")
                    {
                        temp = true;
                        riskyFuncsAllowed = false;
                    }
                }
                finally
                {
                    await emmVRC.AwaitUpdate.Yield();
                    
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

                    // If you are the creator of the world, you get to use Risky Functions in it, unless the root objects below exist.
                    if (RoomManager.field_Internal_Static_ApiWorld_0.authorId == APIUser.CurrentUser.id)
                        riskyFuncsAllowed = true;


                    // As a final check, and to allow world creators more choice over Risky Functions without relying on our whitelist, we are looking for "eVRCRiskFuncDisable" or "eVRCRiskFuncEnable"
                    // If these are present, they will completely override our choice from tags and the online list, and manually disable or enable Risky Functions
                    GameObject[] rootObjects = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
                    if (rootObjects.Any(a => a.name == "eVRCRiskFuncDisable"))
                        riskyFuncsAllowed = false;
                    else if (rootObjects.Any(a => a.name == "eVRCRiskFuncEnable"))
                        riskyFuncsAllowed = true;

                    // Now have Risky Functions reprocess based on this result
                    RiskyFuncsAreChecked = false;
                }
            }
        }
    }
}
