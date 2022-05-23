using emmVRC.Network;
using emmVRC.Objects.ModuleBases;
using emmVRC.Utils;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace emmVRC.Menus
{
    [Priority(50)]
    public class PlayerTweaksWingMenu : MelonLoaderEvents
    {
        public static WingPage basePage;
        private static WingButton mainButton;

        private static WingToggle flightToggle;
        private static WingToggle noclipToggle;
        private static WingToggle speedToggle;
        private static WingToggle espToggle;
        private static WingButton waypointsButton;

        public static WingPage waypointsPage;

        private static bool _initialized = false;

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            if (buildIndex != -1 || _initialized) return;
            basePage = new WingPage("emmVRC_PlayerTweaksWing", "Player Tweaks");
            mainButton = new WingButton("Tweaks", () => { OpenMenu(false); }, "Quickly access multiple player tweaks, including Risky Functions", Functions.Core.Resources.PlayerIcon);
            mainButton.SetAction(() => { OpenMenu(true); }, true);

            flightToggle = new WingToggle(basePage, "Flight", (bool val) =>
            {
                if (Managers.RiskyFunctionsManager.AreRiskyFunctionsAllowed && Configuration.JSONConfig.RiskyFunctionsEnabled)
                {
                    if (!val && Functions.PlayerHacks.Flight.IsNoClipEnabled)
                        noclipToggle.SetToggleState(false, true);
                    Functions.PlayerHacks.Flight.SetFlyActive(val);
                }
            }, "Fly around the world using your controllers or mouse + keyboard", "Stop using flight");
            noclipToggle = new WingToggle(basePage, "Noclip", (bool val) =>
            {
                if (Managers.RiskyFunctionsManager.AreRiskyFunctionsAllowed && Configuration.JSONConfig.RiskyFunctionsEnabled)
                {
                    if (val && !Functions.PlayerHacks.Flight.IsFlyEnabled)
                        flightToggle.SetToggleState(true, true);
                    Functions.PlayerHacks.Flight.SetNoClipActive(val);
                }
            }, "Clip through walls to access areas quicker, or find secrets. Requires flight", "Stop using noclip");
            speedToggle = new WingToggle(basePage, "Speed", (bool val) =>
            {
                if (Managers.RiskyFunctionsManager.AreRiskyFunctionsAllowed && Configuration.JSONConfig.RiskyFunctionsEnabled)
                    Functions.PlayerHacks.Speed.SetActive(val);
            }, "Increase or decrease your movement speed", "Go back to the world's default speed");
            espToggle = new WingToggle(basePage, "ESP", (bool val) =>
            {
                if (Managers.RiskyFunctionsManager.AreRiskyFunctionsAllowed && Configuration.JSONConfig.RiskyFunctionsEnabled)
                    Functions.PlayerHacks.ESP.SetActive(val);
            }, "Emit a glow around players in the current instance", "Remove glow around surrounding players");
            flightToggle.SetArrowEnabled(false);
            noclipToggle.SetArrowEnabled(false);
            speedToggle.SetArrowEnabled(false);
            espToggle.SetArrowEnabled(false);
            waypointsButton = new WingButton(basePage, "Waypoints", () => { OpenWaypointsMenu(false); }, "Select from your waypoints to teleport to them", Functions.Core.Resources.WorldHistoryIcon);
            waypointsButton.SetAction(() => { OpenWaypointsMenu(true); }, true);
            waypointsPage = new WingPage("emmVRC_WaypointsWing", "Waypoints");

            Managers.RiskyFunctionsManager.RiskyFuncsProcessed += (bool val) => {
                if (!val)
                {
                    flightToggle.SetToggleState(false, true);
                    noclipToggle.SetToggleState(false, true);
                    espToggle.SetToggleState(false, true);
                }
                flightToggle.SetInteractable(val);
                noclipToggle.SetInteractable(val);
                speedToggle.SetInteractable(val);
                espToggle.SetInteractable(val);
                waypointsButton.SetInteractable(val);
            };
            _initialized = true;
        }
        public static void OpenMenu(bool right)
        {
            flightToggle.SetToggleState(Functions.PlayerHacks.Flight.IsFlyEnabled);
            noclipToggle.SetToggleState(Functions.PlayerHacks.Flight.IsNoClipEnabled);
            speedToggle.SetToggleState(Functions.PlayerHacks.Speed.IsEnabled);
            espToggle.SetToggleState(Functions.PlayerHacks.ESP.IsEnabled);
            flightToggle.SetInteractable(Configuration.JSONConfig.RiskyFunctionsEnabled && Managers.RiskyFunctionsManager.AreRiskyFunctionsAllowed);
            noclipToggle.SetInteractable(Configuration.JSONConfig.RiskyFunctionsEnabled && Managers.RiskyFunctionsManager.AreRiskyFunctionsAllowed);
            speedToggle.SetInteractable(Configuration.JSONConfig.RiskyFunctionsEnabled && Managers.RiskyFunctionsManager.AreRiskyFunctionsAllowed);
            espToggle.SetInteractable(Configuration.JSONConfig.RiskyFunctionsEnabled && Managers.RiskyFunctionsManager.AreRiskyFunctionsAllowed);
            waypointsButton.SetInteractable(Configuration.JSONConfig.RiskyFunctionsEnabled && Managers.RiskyFunctionsManager.AreRiskyFunctionsAllowed);
            if (right)
                basePage.OpenRightMenu();
            else
                basePage.OpenLeftMenu();
        }
        public static void OpenWaypointsMenu(bool right)
        {
            if (!_initialized || !Managers.RiskyFunctionsManager.AreRiskyFunctionsAllowed || !Configuration.JSONConfig.RiskyFunctionsEnabled) return;
            waypointsPage.leftMenuContents.gameObject.transform.DestroyChildren();
            waypointsPage.rightMenuContents.gameObject.transform.DestroyChildren();
            foreach (Objects.Waypoint waypoint in Functions.PlayerHacks.Waypoints.CurrentWaypoints)
            {
                if (waypoint != null)
                {
                    new WingButton(waypointsPage, waypoint.Name == null ? "" : waypoint.Name, () =>
                    {
                        if (!Managers.RiskyFunctionsManager.AreRiskyFunctionsAllowed || !Configuration.JSONConfig.RiskyFunctionsEnabled) return;
                        waypoint.Goto();
                    }, "Teleport to this waypoint");
                }
            }
            if (right)
                waypointsPage.OpenRightMenu();
            else
                waypointsPage.OpenLeftMenu();
        }
    }
}
