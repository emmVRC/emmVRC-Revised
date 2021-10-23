using emmVRC.Objects;
using emmVRC.Objects.ModuleBases;
using emmVRC.TinyJSON;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace emmVRC.Functions.Other
{
    public class AlarmClock : MelonLoaderEvents, IWithFixedUpdate
    {
        public static readonly string alarmsFilePath = Path.Combine(Environment.CurrentDirectory, "UserData/emmVRC/alarms.json");

        public static List<Alarm> Alarms { get; private set; }
        public static event Action<Alarm> OnAlarmTrigger;

        private static long lastInstanceTime = long.MaxValue;
        private static long lastCurrentTime = long.MaxValue;

        public override void OnApplicationStart()
        {
            // Just assume if the file doesn't exist its the old version
            if (!File.Exists(alarmsFilePath))
                PortOldConfig();
            else
                LoadConfig();
        }

        public void OnFixedUpdate()
        {
            long currentTime = DateTime.Now.TimeOfDay.Ticks;
            long timeInInstance = TimeSpan.FromSeconds(RoomManager.prop_Single_0).Ticks;

            for (int i = 0; i < Alarms.Count; i++)
            {
                if (!Alarms[i].IsEnabled)
                    continue;

                if (Alarms[i].IsSystemTime ? currentTime > Alarms[i].Time && lastCurrentTime < Alarms[i].Time : timeInInstance > Alarms[i].Time && lastInstanceTime < Alarms[i].Time)
                {
                    if (!Alarms[i].Repeats)
                    {
                        Alarms[i].IsEnabled = false;
                        SaveConfig();
                    }
                    OnAlarmTrigger?.Invoke(Alarms[i]);
                }
            }

            lastCurrentTime = currentTime;
            lastInstanceTime = timeInInstance;
        }

        private static void PortOldConfig()
        {
            Alarms = new List<Alarm>();
            SaveConfig();
        }

        private static void LoadConfig()
        {
            Alarms = Decoder.Decode(File.ReadAllText(alarmsFilePath)).Make<List<Alarm>>();
        }

        public static void SaveConfig()
        {
            emmVRCLoader.Logger.LogDebug("Saving alarms");
            File.WriteAllText(alarmsFilePath, Encoder.Encode(Alarms, EncodeOptions.PrettyPrint | EncodeOptions.NoTypeHints));
        }
    }
}