using emmVRC.Objects;
using emmVRC.Objects.ModuleBases;
using emmVRC.TinyJSON;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

namespace emmVRC.Functions.Other
{
    public class AlarmClock : MelonLoaderEvents, IWithFixedUpdate
    {
        public static readonly string alarmsFilePath = Path.Combine(Environment.CurrentDirectory, "UserData/emmVRC/alarms.json");

        private static AudioSource alarmSource;

        public static List<Alarm> Alarms { get; private set; }

        private static long lastInstanceTime = long.MaxValue;
        private static long lastCurrentTime = long.MaxValue;

        private static bool alarmRinging = false;

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
            if (!Configuration.JSONConfig.AlarmsEnabled) return;
            long currentTime = DateTime.Now.TimeOfDay.Ticks;
            long timeInInstance = TimeSpan.FromSeconds(RoomManager.prop_Single_0).Ticks;

            if (alarmSource == null && GameObject.Find("MenuAudio") == null) return;

            if (alarmSource == null)
            {
                GameObject alarmSourceObj = new GameObject("emmVRC_AlarmSource");
                alarmSourceObj.transform.SetParent(GameObject.Find("MenuAudio").transform.parent);
                AudioSource sourceAudioSource = GameObject.Find("MenuAudio").GetComponent<AudioSource>();
                alarmSource = alarmSourceObj.AddComponent<AudioSource>(sourceAudioSource);
            }
            if (alarmRinging && !alarmSource.isPlaying)
            {
                emmVRCLoader.Logger.LogDebug("Alarm Ringing is " + (alarmRinging ? "true" : "false"));
                emmVRCLoader.Logger.LogDebug("Alarm Source is " + (alarmSource.isPlaying ? "playing" : "not playing"));
                emmVRCLoader.Logger.LogDebug("Playing audio source");
                alarmSource.clip = Functions.Core.Resources.alarmTone;
                alarmSource.loop = false;
                alarmSource.Play();
            }
            if (!alarmRinging && alarmSource.isPlaying)
            {
                emmVRCLoader.Logger.LogDebug("Alarm Ringing is " + (alarmRinging ? "true" : "false"));
                emmVRCLoader.Logger.LogDebug("Alarm Source is " + (alarmSource.isPlaying ? "playing" : "not playing"));
                emmVRCLoader.Logger.LogDebug("Stopping audio source");
                alarmSource.Stop();
            }

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
                    OnAlarmTrigger(Alarms[i]);
                }
            }

            lastCurrentTime = currentTime;
            lastInstanceTime = timeInInstance;
        }

        private static void OnAlarmTrigger(Alarm alarm)
        {
            emmVRCLoader.Logger.LogDebug("OnAlarmTrigger Triggered");
            alarmSource.volume = alarm.Volume;
            alarmRinging = true;
            if (alarm.IsSystemTime)
                Managers.emmVRCNotificationsManager.AddNotification(new Notification(alarm.Name, Functions.Core.Resources.alarmSprite, "Your alarm for " + new TimeSpan(alarm.Time).ToString("h':'mm") + " is ringing.", false, true, () => { alarmRinging = false; }, "Dismiss", "Dismisses the current alarm", false));
            else
                Managers.emmVRCNotificationsManager.AddNotification(new Notification(alarm.Name, Functions.Core.Resources.alarmSprite, "Your instance alarm at " + new TimeSpan(alarm.Time).ToString("h':'mm':'ss") + " is ringing.", false, true, () => { alarmRinging = false; }, "Dismiss", "Dismisses the current alarm", false));
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