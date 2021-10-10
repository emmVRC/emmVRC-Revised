using emmVRC.Objects;
using emmVRC.Hacks;
using emmVRC.Objects.ModuleBases;
using emmVRC.TinyJSON;
using emmVRC.Utils;
using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;
using VRC.Core;

namespace emmVRC.Functions.PlayerHacks
{
    public class Waypoints : MelonLoaderEvents
    {
        public static List<Waypoint> CurrentWaypoints { get; private set; }

        private static string currentWorld;

        public override void OnApplicationStart()
        {
            CurrentWaypoints = new List<Waypoint>();
            NetworkEvents.OnInstanceChanged += OnInstanceChanged;
        }

        private static void OnInstanceChanged(ApiWorld world, ApiWorldInstance instance)
        {
            currentWorld = world.id;
            CurrentWaypoints.Clear();

            string filePath = Path.Combine(Environment.CurrentDirectory, "UserData/emmVRC/Waypoints");
            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);

            filePath = Path.Combine(filePath, $"{currentWorld}.json");
            if (File.Exists(filePath))
            {
                string WaypointText = File.ReadAllText(filePath);
                if (WaypointText.Contains("emmVRC.Menus"))
                {
                    WaypointText = WaypointText.Replace("emmVRC.Menus", "emmVRC.Objects");
                    File.WriteAllText(filePath, WaypointText);
                }
                CurrentWaypoints = Decoder.Decode(File.ReadAllText(filePath)).Make<List<Waypoint>>();
            }

            for (int i = 0; i < CurrentWaypoints.Count; i++)
                if (CurrentWaypoints[i] != null && CurrentWaypoints[i].IsEmpty())
                    CurrentWaypoints.RemoveAt(i);
        }

        public static void SaveWaypoints()
        {
            emmVRCLoader.Logger.LogDebug("Saving waypoints...");
            if (CurrentWaypoints.All(waypoint => waypoint == null))
                return;

            string filePath = Path.Combine(Environment.CurrentDirectory, "UserData/emmVRC/Waypoints");
            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);

            string jsonString = Encoder.Encode(CurrentWaypoints, EncodeOptions.PrettyPrint | EncodeOptions.NoTypeHints);
            filePath = Path.Combine(filePath, $"{currentWorld}.json");
            if (!File.Exists(filePath))
                using (StreamWriter writer = File.CreateText(filePath))
                    writer.Write(jsonString);
            else
                File.WriteAllText(filePath, jsonString);
        }
    }
}