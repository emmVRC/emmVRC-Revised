using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using emmVRC.Libraries;
using UnityEngine;
using TinyJSON;

namespace emmVRC.Menus
{
    public class Waypoint
    {
        public string Name = "";
        public float x = 0f;
        public float y = 0f;
        public float z = 0f;
        public float rx = 0f;
        public float ry = 0f;
        public float rz = 0f;
        public float rw = 0f;
    }
    public class WaypointsMenu
    {
        public static QMNestedButton baseMenu;
        private static QMSingleButton waypoint1Button;
        private static QMSingleButton waypoint2Button;
        private static QMSingleButton waypoint3Button;
        private static QMSingleButton waypoint4Button;
        private static QMSingleButton waypoint5Button;
        private static QMSingleButton waypoint6Button;
        private static QMSingleButton waypoint7Button;
        private static QMSingleButton waypoint8Button;

        public static QMNestedButton selectedWaypointMenu;
        private static QMSingleButton teleportButton;
        private static QMSingleButton renameButton;
        private static QMSingleButton setLocationButton;

        private static int selectedWaypoint = 0;
        private static string currentWorldID = "";
        private static List<Waypoint> worldWaypoints = new List<Waypoint>();

        public static void Initialize()
        {
            baseMenu = new QMNestedButton(PlayerTweaksMenu.baseMenu, 19293, 10234, "Waypoints", "");
            baseMenu.getMainButton().DestroyMe();
            waypoint1Button = new QMSingleButton(baseMenu, 1, 0, "Waypoint\n1", () => { selectedWaypoint = 0; LoadWaypointOptions(); }, "View options for this waypoint");
            waypoint2Button = new QMSingleButton(baseMenu, 2, 0, "Waypoint\n2", () => { selectedWaypoint = 1; LoadWaypointOptions(); }, "View options for this waypoint");
            waypoint3Button = new QMSingleButton(baseMenu, 3, 0, "Waypoint\n3", () => { selectedWaypoint = 2; LoadWaypointOptions(); }, "View options for this waypoint");
            waypoint4Button = new QMSingleButton(baseMenu, 4, 0, "Waypoint\n4", () => { selectedWaypoint = 3; LoadWaypointOptions(); }, "View options for this waypoint");
            waypoint5Button = new QMSingleButton(baseMenu, 1, 1, "Waypoint\n5", () => { selectedWaypoint = 4; LoadWaypointOptions(); }, "View options for this waypoint");
            waypoint6Button = new QMSingleButton(baseMenu, 2, 1, "Waypoint\n6", () => { selectedWaypoint = 5; LoadWaypointOptions(); }, "View options for this waypoint");
            waypoint7Button = new QMSingleButton(baseMenu, 3, 1, "Waypoint\n7", () => { selectedWaypoint = 6; LoadWaypointOptions(); }, "View options for this waypoint");
            waypoint8Button = new QMSingleButton(baseMenu, 4, 1, "Waypoint\n8", () => { selectedWaypoint = 7; LoadWaypointOptions(); }, "View options for this waypoint");

            selectedWaypointMenu = new QMNestedButton(baseMenu, 1024, 768, "Selected Waypoint", "");
            selectedWaypointMenu.getMainButton().DestroyMe();
            teleportButton = new QMSingleButton(selectedWaypointMenu, 1, 0, "Teleport", () => { VRCPlayer.field_VRCPlayer_0.transform.position = new Vector3(worldWaypoints[selectedWaypoint].x, worldWaypoints[selectedWaypoint].y, worldWaypoints[selectedWaypoint].z); VRCPlayer.field_VRCPlayer_0.transform.rotation = new Quaternion(worldWaypoints[selectedWaypoint].rx, worldWaypoints[selectedWaypoint].ry, worldWaypoints[selectedWaypoint].rz, worldWaypoints[selectedWaypoint].rw); }, "Teleport to this waypoint");
            renameButton = new QMSingleButton(selectedWaypointMenu, 2, 0, "Rename", () => { InputUtilities.OpenInputBox("Type a name (or none for default)", "Accept", (string name) => { worldWaypoints[selectedWaypoint].Name = name; SaveWaypoints(); }); }, "Rename this waypoint (currently unnamed)");
            setLocationButton = new QMSingleButton(selectedWaypointMenu, 3, 0, "Set\nLocation", () => { 
                worldWaypoints[selectedWaypoint].x = VRCPlayer.field_VRCPlayer_0.transform.position.x; 
                worldWaypoints[selectedWaypoint].y = VRCPlayer.field_VRCPlayer_0.transform.position.y; 
                worldWaypoints[selectedWaypoint].z = VRCPlayer.field_VRCPlayer_0.transform.position.z;
                worldWaypoints[selectedWaypoint].rx = VRCPlayer.field_VRCPlayer_0.transform.rotation.x;
                worldWaypoints[selectedWaypoint].ry = VRCPlayer.field_VRCPlayer_0.transform.rotation.y;
                worldWaypoints[selectedWaypoint].rz = VRCPlayer.field_VRCPlayer_0.transform.rotation.z;
                worldWaypoints[selectedWaypoint].rw = VRCPlayer.field_VRCPlayer_0.transform.rotation.w;
                SaveWaypoints();
                teleportButton.getGameObject().GetComponent<UnityEngine.UI.Button>().enabled = true;
                renameButton.getGameObject().GetComponent<UnityEngine.UI.Button>().enabled = true;
            }, "Set the waypoint location");
        
        }

        public static IEnumerator LoadWorld()
        {
            while (RoomManager.field_ApiWorld_0 == null)
                yield return new WaitForEndOfFrame();
            currentWorldID = RoomManager.field_ApiWorld_0.id;
            if (!Directory.Exists(Path.Combine(Environment.CurrentDirectory, "UserData/emmVRC/Waypoints")))
            {
                Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, "UserData/emmVRC/Waypoints"));
            }
            if (File.Exists(Path.Combine(Environment.CurrentDirectory, "UserData/emmVRC/Waypoints/" + currentWorldID + ".json")))
            {
                worldWaypoints = TinyJSON.Decoder.Decode(File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "UserData/emmVRC/Waypoints/" + currentWorldID + ".json"))).Make<List<Waypoint>>();
            } else
            {
                worldWaypoints = new List<Waypoint> { new Waypoint(), new Waypoint(), new Waypoint(), new Waypoint(), new Waypoint(), new Waypoint(), new Waypoint(), new Waypoint() };
                File.WriteAllText(Path.Combine(Environment.CurrentDirectory, "UserData/emmVRC/Waypoints/"+currentWorldID+".json"), TinyJSON.Encoder.Encode(worldWaypoints));
            }
        }

        public static void LoadWaypointList()
        {
            waypoint1Button.setButtonText(worldWaypoints[0].Name == "" ? "Waypoint\n1" : worldWaypoints[0].Name);
            waypoint2Button.setButtonText(worldWaypoints[1].Name == "" ? "Waypoint\n2" : worldWaypoints[1].Name);
            waypoint3Button.setButtonText(worldWaypoints[2].Name == "" ? "Waypoint\n3" : worldWaypoints[2].Name);
            waypoint4Button.setButtonText(worldWaypoints[3].Name == "" ? "Waypoint\n4" : worldWaypoints[3].Name);
            waypoint5Button.setButtonText(worldWaypoints[4].Name == "" ? "Waypoint\n5" : worldWaypoints[4].Name);
            waypoint6Button.setButtonText(worldWaypoints[5].Name == "" ? "Waypoint\n6" : worldWaypoints[5].Name);
            waypoint7Button.setButtonText(worldWaypoints[6].Name == "" ? "Waypoint\n7" : worldWaypoints[6].Name);
            waypoint8Button.setButtonText(worldWaypoints[7].Name == "" ? "Waypoint\n8" : worldWaypoints[7].Name);
            QuickMenuUtils.ShowQuickmenuPage(baseMenu.getMenuName());
        }

        private static void LoadWaypointOptions()
        {
            if (worldWaypoints[selectedWaypoint].x == 0f && worldWaypoints[selectedWaypoint].y == 0f && worldWaypoints[selectedWaypoint].z == 0f)
            {
                teleportButton.getGameObject().GetComponent<UnityEngine.UI.Button>().enabled = false;
                renameButton.getGameObject().GetComponent<UnityEngine.UI.Button>().enabled = false;
            }
            else
            {
                teleportButton.getGameObject().GetComponent<UnityEngine.UI.Button>().enabled = true;
                renameButton.getGameObject().GetComponent<UnityEngine.UI.Button>().enabled = true;
                teleportButton.getGameObject().GetComponent<UnityEngine.UI.Button>().enabled = true;
            }
            QuickMenuUtils.ShowQuickmenuPage(selectedWaypointMenu.getMenuName());
        }

        private static void SaveWaypoints()
        {
            File.WriteAllText(Path.Combine(Environment.CurrentDirectory, "UserData/emmVRC/Waypoints/" + currentWorldID + ".json"), TinyJSON.Encoder.Encode(worldWaypoints));
        }

        
    }
}
