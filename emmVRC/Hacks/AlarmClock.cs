using System;
using System.Globalization;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using emmVRC.Libraries;
using emmVRC.Menus;

namespace emmVRC.Hacks
{
    public class AlarmClock
    {
        public static bool AlarmEnabled = false;
        public static string AlarmTimeString = "00:00";
        public static TimeSpan alarmTimeActual;

        public static bool InstanceAlarmEnabled = false;
        public static string InstanceAlarmTimeString = "00:00";
        public static TimeSpan instanceAlarmTimeActual;

        public static bool AlarmTriggered = false;
        public static bool InstanceAlarmTriggered = false;
        private static List<AudioClip> alarmClips = new List<AudioClip>();
        private static AudioSource referenceSource;
        private static AudioSource alarmSource;

        public static bool Hour24 = false;

        public static QMNestedButton baseMenu;

        private static QMNestedButton alarmMenu;
        private static QMToggleButton alarmEnabled;
        private static QMToggleButton alarmPersistentEnabled;
        private static QMSingleButton alarmTime;

        private static QMNestedButton instanceAlarmMenu;
        private static QMToggleButton instanceAlarmEnabled;
        private static QMToggleButton instanceAlarmPersistentEnabled;
        private static QMSingleButton instanceAlarmTime;


        public static IEnumerator Initialize()
        {
            while (Resources.onlineSprite == null) yield return new WaitForSeconds(1f);
            if (!Directory.Exists(Path.Combine(Environment.CurrentDirectory, "UserData/emmVRC/AlarmSounds")))
                Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, "UserData/emmVRC/AlarmSounds"));
            if (Directory.GetFiles(Path.Combine(Environment.CurrentDirectory, "UserData/emmVRC/AlarmSounds")).Length > 0)
            {
                foreach (string audioFile in Directory.GetFiles(Path.Combine(Environment.CurrentDirectory, "UserData/emmVRC/AlarmSounds")))
                {
                    if (audioFile.Contains(".ogg") || audioFile.Contains(".wav"))
                    {
                        emmVRCLoader.Logger.LogDebug("Processing alarm clip " + audioFile);
                        WWW AlarmClipWWW = new WWW(string.Format("file://{0}", audioFile).Replace(@"\", "/"), null, new Il2CppSystem.Collections.Generic.Dictionary<string, string>());
                        AudioClip alarmClip = AlarmClipWWW.GetAudioClip();
                        while (!AlarmClipWWW.isDone || alarmClip.loadState == AudioDataLoadState.Loading) yield return new WaitForEndOfFrame();
                        alarmClip.hideFlags |= HideFlags.DontUnloadUnusedAsset;
                        alarmClips.Add(alarmClip);
                    }
                }
            } else
            {
                alarmClips.Add(Resources.alarmTone);
            }
            while (GameObject.Find("UserInterface/MenuContent/Popups/LoadingPopup") == null) yield return new WaitForEndOfFrame();
            referenceSource = GameObject.Find("UserInterface/MenuContent/Popups/LoadingPopup").GetComponentInChildren<AudioSource>(true);
            GameObject alarmSourceObj = GameObject.Instantiate(referenceSource.gameObject);
            alarmSourceObj.transform.parent = null;
            alarmSourceObj.name = "AlarmSound";
            GameObject.DontDestroyOnLoad(alarmSourceObj);
            alarmSource = alarmSourceObj.GetComponent<AudioSource>();
            alarmSource.clip = null;

            Hour24 = !System.Globalization.DateTimeFormatInfo.CurrentInfo.ShortTimePattern.ToLower().Contains("h");
            alarmTimeActual = TimeSpan.FromSeconds(Configuration.JSONConfig.AlarmTime);
            AlarmTimeString = alarmTimeActual.ToString();

            AlarmEnabled = Configuration.JSONConfig.PersistentAlarm;
            InstanceAlarmEnabled = Configuration.JSONConfig.PersistentInstanceAlarm;

            instanceAlarmTimeActual = TimeSpan.FromSeconds(Configuration.JSONConfig.InstanceAlarmTime);
            InstanceAlarmTimeString = instanceAlarmTimeActual.ToString();

            baseMenu = new QMNestedButton(FunctionsMenu.baseMenu.menuBase, 19283, 10293, "", "");
            baseMenu.getMainButton().DestroyMe();
            alarmMenu = new QMNestedButton(baseMenu, 1, 0, "Basic\nAlarm", "Configure the basic alarm clock, which uses system time");
            alarmEnabled = new QMToggleButton(alarmMenu, 1, 0, "Alarm\nEnabled", () =>
            {
                AlarmEnabled = true;
                AlarmTriggered = false;
            }, "Disabled", () =>
            {
                AlarmEnabled = false;
                AlarmTriggered = false;
            }, "TOGGLE: Enables the alarm clock for the time provided", null, null, false, AlarmEnabled);
            alarmPersistentEnabled = new QMToggleButton(alarmMenu, 2, 0, "Keep Alarm", () =>
            {
                Configuration.JSONConfig.PersistentAlarm = true;
                Configuration.JSONConfig.AlarmTime = (uint)alarmTimeActual.TotalSeconds;
                Configuration.SaveConfig();
            }, "Disabled", () =>
            {
                Configuration.JSONConfig.PersistentAlarm = false;
                Configuration.SaveConfig();
            }, "TOGGLE: Allow this alarm to stay on, even across restarts", null, null, false, Configuration.JSONConfig.PersistentAlarm);
            alarmTime = new QMSingleButton(alarmMenu, 3, 0, "Time:\n" + AlarmTimeString, () =>
            {
                VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowInputPopup("Enter the alarm time", AlarmTimeString, UnityEngine.UI.InputField.InputType.Standard, false, "Accept", new System.Action<string, Il2CppSystem.Collections.Generic.List<KeyCode>, UnityEngine.UI.Text>((string time, Il2CppSystem.Collections.Generic.List<KeyCode> keycodes, UnityEngine.UI.Text txt) => {
                    alarmTimeActual = new TimeSpan(DateTime.Parse(time).Hour, DateTime.Parse(time).Minute, DateTime.Parse(time).Second);
                    AlarmTimeString = (Hour24 ? (new DateTime() + alarmTimeActual).ToString("hh:mm") : (new DateTime() + alarmTimeActual).ToString("HH:mm tt"));
                    if (Configuration.JSONConfig.PersistentAlarm)
                    {
                        Configuration.JSONConfig.AlarmTime = (uint)alarmTimeActual.TotalSeconds;
                        Configuration.SaveConfig();
                        alarmTime.setButtonText("Time:\n" + AlarmTimeString);
                    }    
                }), null, Hour24 ? "00:00" : "00:00 PM");
            }, "The time the alarm will go off. Select to change.");

            instanceAlarmMenu = new QMNestedButton(baseMenu, 2, 0, "Instance\nAlarm", "Configure the instance alarm clock, which uses the time you've been in the current instance");
            instanceAlarmEnabled = new QMToggleButton(instanceAlarmMenu, 1, 0, "Instance\nAlarm Enabled", () =>
            {
                InstanceAlarmEnabled = true;
                InstanceAlarmTriggered = false;
            }, "Disabled", () =>
            {
                InstanceAlarmEnabled = false;
                InstanceAlarmTriggered = false;
            }, "TOGGLE: Enables the instance alarm clock for the time provided");
            instanceAlarmPersistentEnabled = new QMToggleButton(instanceAlarmMenu, 2, 0, "Keep Instance\nAlarm", () =>
            {
                Configuration.JSONConfig.PersistentInstanceAlarm = true;
                Configuration.JSONConfig.InstanceAlarmTime = (uint)instanceAlarmTimeActual.TotalSeconds;
                Configuration.SaveConfig();
            }, "Disabled", () =>
            {
                Configuration.JSONConfig.PersistentInstanceAlarm = false;
                Configuration.SaveConfig();
            }, "TOGGLE: Allow this alarm to stay on, even across restarts");
            instanceAlarmTime = new QMSingleButton(instanceAlarmMenu, 3, 0, "Instance\nTime:\n" + InstanceAlarmTimeString, () =>
            {
                VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowInputPopup("Enter the instance alarm time", InstanceAlarmTimeString, UnityEngine.UI.InputField.InputType.Standard, false, "Accept", new System.Action<string, Il2CppSystem.Collections.Generic.List<KeyCode>, UnityEngine.UI.Text>((string time, Il2CppSystem.Collections.Generic.List<KeyCode> keycodes, UnityEngine.UI.Text txt) => {
                    instanceAlarmTimeActual = new TimeSpan(DateTime.Parse(time).Hour, DateTime.Parse(time).Minute, DateTime.Parse(time).Second);
                    InstanceAlarmTimeString = time;
                    if (Configuration.JSONConfig.PersistentInstanceAlarm)
                    {
                        Configuration.JSONConfig.InstanceAlarmTime = (uint)instanceAlarmTimeActual.TotalSeconds;
                        Configuration.SaveConfig();
                        instanceAlarmTime.setButtonText("Time:\n" + InstanceAlarmTimeString);
                    }
                }), null, "00:00");
            }, "The time the instance alarm will go off. Select to change.");
            MelonLoader.MelonCoroutines.Start(Loop());
        }
        public static IEnumerator Loop()
        {
            while (true)
            {
                yield return new WaitForFixedUpdate();
                if (alarmSource != null && referenceSource != null)
                {
                    alarmSource.volume = referenceSource.volume;
                    alarmSource.outputAudioMixerGroup = referenceSource.outputAudioMixerGroup;
                }
                if (AlarmEnabled && !AlarmTriggered)
                {
                    if (DateTime.Now.TimeOfDay.Hours == alarmTimeActual.Hours && DateTime.Now.TimeOfDay.Minutes == alarmTimeActual.Minutes && DateTime.Now.TimeOfDay.Seconds == alarmTimeActual.Seconds && !AlarmTriggered)
                    {
                        AlarmTriggered = true;
                        emmVRCLoader.Logger.LogDebug("Alarm triggered");
                        alarmSource.clip = (alarmClips.Count > 1 ? alarmClips[new System.Random().Next(alarmClips.Count)] : alarmClips.First());
                        if (alarmSource.clip == null)
                            alarmSource.clip = Resources.alarmTone;
                        alarmSource.Play();
                        Managers.NotificationManager.AddNotification("Your alarm for " + (Hour24 ? (new DateTime() + alarmTimeActual).ToString("hh:mm") : (new DateTime() + alarmTimeActual).ToString("HH:mm tt")) + " is ringing.", "Snooze\n(15 min)", () => {
                            alarmTimeActual.Add(new TimeSpan(0, 15, 0));
                            AlarmTriggered = false;
                            Managers.NotificationManager.DismissCurrentNotification();
                        }, "Dismiss", () => {
                            AlarmTriggered = false;
                            if (!Configuration.JSONConfig.PersistentAlarm)
                            {
                                AlarmEnabled = false;
                            }
                            Managers.NotificationManager.DismissCurrentNotification();
                        }, Resources.alarmSprite);
                    }
                }
                if (InstanceAlarmEnabled && !InstanceAlarmTriggered)
                {
                    if (TimeSpan.FromSeconds(InfoBarClock.instanceTime) == instanceAlarmTimeActual)
                    {
                        InstanceAlarmTriggered = true;
                        emmVRCLoader.Logger.LogDebug("Instance Alarm triggered");
                        alarmSource.clip = (alarmClips.Count > 1 ? alarmClips[new System.Random().Next(alarmClips.Count)] : alarmClips.First());
                        if (alarmSource.clip == null)
                            alarmSource.clip = Resources.alarmTone;
                        alarmSource.Play();
                        Managers.NotificationManager.AddNotification("Your instance alarm for " + ((new DateTime() + instanceAlarmTimeActual).ToString("HH:mm:ss")) + " is ringing.", "Snooze\n(15 min)", () => {
                            instanceAlarmTimeActual.Add(new TimeSpan(0, 15, 0));
                            InstanceAlarmTriggered = false;
                            Managers.NotificationManager.DismissCurrentNotification();
                        }, "Dismiss", () => {
                            InstanceAlarmTriggered = false;
                            if (!Configuration.JSONConfig.PersistentInstanceAlarm)
                            {
                                InstanceAlarmEnabled = false;
                            }
                            Managers.NotificationManager.DismissCurrentNotification();
                        }, Resources.alarmSprite);
                    }
                }
                if (alarmSource.isPlaying && ((!AlarmEnabled && !InstanceAlarmEnabled) || (!AlarmTriggered && !InstanceAlarmTriggered)))
                {
                    emmVRCLoader.Logger.LogDebug("Alarm stopped");
                    alarmSource.Stop();
                }
            }
        }
    }
}