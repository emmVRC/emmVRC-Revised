using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using emmVRC.Libraries;
using emmVRC.Utils;
using emmVRC.Objects;
using emmVRC.Objects.ModuleBases;

namespace emmVRC.Menus
{
    [Priority(55)]
    public class WaypointsMenu : MelonLoaderEvents
    {
        private static bool _initialized = false;
        private static MenuPage waypointsPage;

        private static ButtonGroup waypointsGroup;
        private static List<SimpleSingleButton> waypointButtons = new List<SimpleSingleButton>();

        private static MenuPage selectedWaypointPage;
        private static ButtonGroup optionsGroup;
        private static SimpleSingleButton teleportButton;
        private static SimpleSingleButton renameButton;
        private static SimpleSingleButton setLocationButton;
        private static SimpleSingleButton removeButton;

        private static int selectedWaypoint = 0;
        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            if (buildIndex != -1 || _initialized) return;

            waypointsPage = new MenuPage("emmVRC_Waypoints", "Waypoints", false, true, true, () => {
                Functions.PlayerHacks.Waypoints.CurrentWaypoints.Add(new Objects.Waypoint
                {
                    Name = "Waypoint " + (Functions.PlayerHacks.Waypoints.CurrentWaypoints.Count(a => a != null) + 1),
                    x = VRCPlayer.field_Internal_Static_VRCPlayer_0.transform.position.x,
                    y = VRCPlayer.field_Internal_Static_VRCPlayer_0.transform.position.y,
                    z = VRCPlayer.field_Internal_Static_VRCPlayer_0.transform.position.z,
                    rx = VRCPlayer.field_Internal_Static_VRCPlayer_0.transform.rotation.x,
                    ry = VRCPlayer.field_Internal_Static_VRCPlayer_0.transform.rotation.y,
                    rz = VRCPlayer.field_Internal_Static_VRCPlayer_0.transform.rotation.z,
                    rw = VRCPlayer.field_Internal_Static_VRCPlayer_0.transform.rotation.w
                });
                Functions.PlayerHacks.Waypoints.SaveWaypoints();
                LoadMenu();
            }, "Add a new waypoint for your current location", ButtonAPI.plusIconSprite);
            waypointsPage.menuContents.GetComponent<UnityEngine.UI.VerticalLayoutGroup>().childControlHeight = true;
            waypointsPage.AddExtButton(() => {
                ButtonAPI.GetQuickMenuInstance().ShowConfirmDialog("Waypoints", "Are you sure you want to remove all your waypoints for this world? You cannot undo this!", new System.Action(() =>
                {
                    Functions.PlayerHacks.Waypoints.CurrentWaypoints.Clear();
                    Functions.PlayerHacks.Waypoints.SaveWaypoints();
                    LoadMenu();
                }), null);
            }, "Removes all of the waypoints for the current world", ButtonAPI.trashIconSprite);

            waypointsGroup = new ButtonGroup(waypointsPage, "");

            selectedWaypointPage = new MenuPage("emmVRC_SelectedWaypoint", "Selected waypoint", false, true);
            optionsGroup = new ButtonGroup(selectedWaypointPage, "Waypoint Options");
            teleportButton = new SimpleSingleButton(optionsGroup, "Teleport", () => Functions.PlayerHacks.Waypoints.CurrentWaypoints[selectedWaypoint].Goto(), "Teleport to this waypoint");
            renameButton = new SimpleSingleButton(optionsGroup, "Rename", () =>
            {
                selectedWaypointPage.CloseMenu();
                VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowInputPopup("Type a name (or none for default)", "", UnityEngine.UI.InputField.InputType.Standard, false, "Accept", new System.Action<string, Il2CppSystem.Collections.Generic.List<UnityEngine.KeyCode>, UnityEngine.UI.Text>((string name, Il2CppSystem.Collections.Generic.List<UnityEngine.KeyCode> keyk, UnityEngine.UI.Text tx) =>
                {
                    Functions.PlayerHacks.Waypoints.CurrentWaypoints[selectedWaypoint].Name = name; Functions.PlayerHacks.Waypoints.SaveWaypoints(); selectedWaypointPage.CloseMenu(); OpenMenu();
                }), null, "Enter name...");
            }, "Rename this waypoint");
            setLocationButton = new SimpleSingleButton(optionsGroup, "Set Location", () =>
            {
                
                Functions.PlayerHacks.Waypoints.CurrentWaypoints[selectedWaypoint].x = VRCPlayer.field_Internal_Static_VRCPlayer_0.transform.position.x;
                Functions.PlayerHacks.Waypoints.CurrentWaypoints[selectedWaypoint].y = VRCPlayer.field_Internal_Static_VRCPlayer_0.transform.position.y;
                Functions.PlayerHacks.Waypoints.CurrentWaypoints[selectedWaypoint].z = VRCPlayer.field_Internal_Static_VRCPlayer_0.transform.position.z;
                Functions.PlayerHacks.Waypoints.CurrentWaypoints[selectedWaypoint].rx = VRCPlayer.field_Internal_Static_VRCPlayer_0.transform.rotation.x;
                Functions.PlayerHacks.Waypoints.CurrentWaypoints[selectedWaypoint].ry = VRCPlayer.field_Internal_Static_VRCPlayer_0.transform.rotation.y;
                Functions.PlayerHacks.Waypoints.CurrentWaypoints[selectedWaypoint].rz = VRCPlayer.field_Internal_Static_VRCPlayer_0.transform.rotation.z;
                Functions.PlayerHacks.Waypoints.CurrentWaypoints[selectedWaypoint].rw = VRCPlayer.field_Internal_Static_VRCPlayer_0.transform.rotation.w;
                Functions.PlayerHacks.Waypoints.SaveWaypoints();
                selectedWaypointPage.CloseMenu();
                LoadMenu();
            }, "Set the waypoint's location to where you are standing");
            removeButton = new SimpleSingleButton(optionsGroup, "Remove", () =>
            {
                ButtonAPI.GetQuickMenuInstance().ShowConfirmDialog("Waypoint", $"Are you sure you want to remove '{Functions.PlayerHacks.Waypoints.CurrentWaypoints[selectedWaypoint].Name}'?", new System.Action(() =>
                {
                    Functions.PlayerHacks.Waypoints.CurrentWaypoints.RemoveAt(selectedWaypoint);
                    Functions.PlayerHacks.Waypoints.SaveWaypoints();
                    selectedWaypointPage.CloseMenu();
                    selectedWaypointPage.CloseMenu();
                    LoadMenu();
                }), null);
            }, "Remove this waypoint");

            _initialized = true;
        }
        public static void OpenMenu()
        {
            waypointsPage.OpenMenu();
            LoadMenu();
        }
        public static void LoadMenu()
        {
            if (!_initialized || !Managers.RiskyFunctionsManager.AreRiskyFunctionsAllowed || !Configuration.JSONConfig.RiskyFunctionsEnabled) return;
            waypointsGroup.gameObject.transform.DestroyChildren();
            foreach (Objects.Waypoint waypoint in Functions.PlayerHacks.Waypoints.CurrentWaypoints)
            {
                if (waypoint != null)
                {
                    new SimpleSingleButton(waypointsGroup, waypoint.Name == null ? "" : waypoint.Name, () =>
                    {
                        selectedWaypoint = Functions.PlayerHacks.Waypoints.CurrentWaypoints.IndexOf(waypoint);
                        selectedWaypointPage.OpenMenu();
                    }, "See options for this waypoint");
                }
            }
        }
    }
}
