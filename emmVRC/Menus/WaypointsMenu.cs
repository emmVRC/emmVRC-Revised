using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using emmVRC.Libraries;
using UnityEngine;
using emmVRC.Objects.ModuleBases;

namespace emmVRC.Menus
{
    [Priority(70)]
    public class WaypointsMenu : MelonLoaderEvents
    {
        public static PaginatedMenu baseMenu;
        private static QMSingleButton waypoint1Button;
        private static PageItem NewWaypointButton;

        public static QMNestedButton selectedWaypointMenu;
        private static QMSingleButton teleportButton;
        private static QMSingleButton renameButton;
        private static QMSingleButton setLocationButton;
        private static QMSingleButton removeButton;

        private static int selectedWaypoint = 0;
        private static string currentWorldID = "";

        public override void OnUiManagerInit()
        {
            baseMenu = new PaginatedMenu(PlayerTweaksMenu.baseMenu, 19293, 10234, "Waypoints", "", null);
            baseMenu.menuEntryButton.DestroyMe();
            

            selectedWaypointMenu = new QMNestedButton(baseMenu.menuBase, 1024, 768, "Selected Waypoint", "");
            selectedWaypointMenu.getMainButton().DestroyMe();
            teleportButton = new QMSingleButton(selectedWaypointMenu, 1, 0, "Teleport", () => { Functions.PlayerHacks.Waypoints.CurrentWaypoints[selectedWaypoint].Goto(); }, "Teleport to this waypoint");
            renameButton = new QMSingleButton(selectedWaypointMenu, 2, 0, "Rename", () =>
            {
                VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowInputPopup("Type a name (or none for default)", "", UnityEngine.UI.InputField.InputType.Standard, false, "Accept", new System.Action<string, Il2CppSystem.Collections.Generic.List<UnityEngine.KeyCode>, UnityEngine.UI.Text>((string name, Il2CppSystem.Collections.Generic.List<UnityEngine.KeyCode> keyk, UnityEngine.UI.Text tx) =>
                {
                    Functions.PlayerHacks.Waypoints.CurrentWaypoints[selectedWaypoint].Name = name; Functions.PlayerHacks.Waypoints.SaveWaypoints(); LoadWaypointList();
                }), null, "Enter name...");
            }, "Rename this waypoint");
            setLocationButton = new QMSingleButton(selectedWaypointMenu, 3, 0, "Set\nLocation", () =>
            {
                Functions.PlayerHacks.Waypoints.CurrentWaypoints[selectedWaypoint].x = VRCPlayer.field_Internal_Static_VRCPlayer_0.transform.position.x;
                Functions.PlayerHacks.Waypoints.CurrentWaypoints[selectedWaypoint].y = VRCPlayer.field_Internal_Static_VRCPlayer_0.transform.position.y;
                Functions.PlayerHacks.Waypoints.CurrentWaypoints[selectedWaypoint].z = VRCPlayer.field_Internal_Static_VRCPlayer_0.transform.position.z;
                Functions.PlayerHacks.Waypoints.CurrentWaypoints[selectedWaypoint].rx = VRCPlayer.field_Internal_Static_VRCPlayer_0.transform.rotation.x;
                Functions.PlayerHacks.Waypoints.CurrentWaypoints[selectedWaypoint].ry = VRCPlayer.field_Internal_Static_VRCPlayer_0.transform.rotation.y;
                Functions.PlayerHacks.Waypoints.CurrentWaypoints[selectedWaypoint].rz = VRCPlayer.field_Internal_Static_VRCPlayer_0.transform.rotation.z;
                Functions.PlayerHacks.Waypoints.CurrentWaypoints[selectedWaypoint].rw = VRCPlayer.field_Internal_Static_VRCPlayer_0.transform.rotation.w;
                Functions.PlayerHacks.Waypoints.SaveWaypoints();
                LoadWaypointList();
            }, "Set the waypoint location");
            removeButton = new QMSingleButton(selectedWaypointMenu, 4, 0, "Remove", () =>
            {
                Functions.PlayerHacks.Waypoints.CurrentWaypoints.RemoveAt(selectedWaypoint);
                Functions.PlayerHacks.Waypoints.SaveWaypoints();
                LoadWaypointList();
            }, "Remove this waypoint");
        }


        public static void LoadWaypointList()
        {
            emmVRCLoader.Logger.LogDebug("BaseMenu is " + (baseMenu == null ? "null" : "not null"));
            emmVRCLoader.Logger.LogDebug("PageItems is " + (baseMenu.pageItems == null ? "null" : "not null"));
            baseMenu.pageItems.Clear();
            foreach (Objects.Waypoint waypoint in Functions.PlayerHacks.Waypoints.CurrentWaypoints)
            {
                if (waypoint != null)
                {
                    baseMenu.pageItems.Add(new PageItem(waypoint.Name == null ? "" : waypoint.Name, () =>
                    {
                        selectedWaypoint = Functions.PlayerHacks.Waypoints.CurrentWaypoints.IndexOf(waypoint);
                        QuickMenuUtils.ShowQuickmenuPage(selectedWaypointMenu.getMenuName());
                    }, "See options for this waypoint"));
                }
            }
            baseMenu.pageItems.Add(NewWaypointButton = new PageItem("+\nNew\nWaypoint", () => {
                Functions.PlayerHacks.Waypoints.CurrentWaypoints.Add(new Objects.Waypoint
                {
                    Name = "Waypoint " + (Functions.PlayerHacks.Waypoints.CurrentWaypoints.Count + 1),
                    x = VRCPlayer.field_Internal_Static_VRCPlayer_0.transform.position.x,
                    y = VRCPlayer.field_Internal_Static_VRCPlayer_0.transform.position.y,
                    z = VRCPlayer.field_Internal_Static_VRCPlayer_0.transform.position.z,
                    rx = VRCPlayer.field_Internal_Static_VRCPlayer_0.transform.rotation.x,
                    ry = VRCPlayer.field_Internal_Static_VRCPlayer_0.transform.rotation.y,
                    rz = VRCPlayer.field_Internal_Static_VRCPlayer_0.transform.rotation.z,
                    rw = VRCPlayer.field_Internal_Static_VRCPlayer_0.transform.rotation.w
                });
                Functions.PlayerHacks.Waypoints.SaveWaypoints();
                LoadWaypointList();
            }, "Create a new waypoint at your current position"));
            baseMenu.OpenMenu();
        }
    }
}
