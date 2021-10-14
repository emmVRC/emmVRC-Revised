using emmVRC.Functions.Other;
using emmVRC.Objects;
using emmVRC.Objects.ModuleBases;
using emmVRC.Managers;
using emmVRC.Libraries;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace emmVRC.Menus
{
    public class AlarmClockMenu : MelonLoaderEvents
    {
        public static PaginatedMenu Menu { get; private set; }

        public static QMNestedButton AlarmMenu { get; private set; }

        private static Alarm currentAlarm;

        private static QMToggleButton enabledToggle;
        private static QMToggleButton repeatsToggle;
        private static Objects.Slider volumeSlider;
        private static QMToggleButton systemTimeToggle;
        private static QMSingleButton setTimeButton;

        public override void OnApplicationStart()
        {
            AlarmClock.OnAlarmTrigger += OnAlarmTrigger;
        }

        public override void OnUiManagerInit()
        {
            Menu = new PaginatedMenu(FunctionsMenuLegacy.baseMenu.menuBase.getMenuName(), 10293, 12931, "emmVRCAlarmClockMenu", "", null);
            Menu.menuEntryButton.DestroyMe();

            new QMSingleButton(Menu.menuBase, 5, 1, "Add\nAlarm", new Action(() =>
            {
                // Gets highest current id
                currentAlarm = new Alarm(AlarmClock.Alarms.Count > 0 ? AlarmClock.Alarms.Max(alarm => alarm.Id) + 1 : 0);
                AlarmClock.Alarms.Add(currentAlarm);
                AlarmClock.SaveConfig();
                Refresh();
            }), "Add a new alarm to the list.");

            AlarmMenu = new QMNestedButton(Menu.menuBase, 19283, 10223, "emmVRCAlarmClockConfigMenu", "");
            AlarmMenu.getMainButton().DestroyMe();

            enabledToggle = new QMToggleButton(AlarmMenu, 1, 0, "Enabled", () => { currentAlarm.IsEnabled = true; AlarmClock.SaveConfig(); }, "Disabled", () => { currentAlarm.IsEnabled = false; AlarmClock.SaveConfig(); }, "TOGGLE: Enable the alarm.");
            new QMSingleButton(AlarmMenu, 2, 0, "Set Alarm\nName", new Action(() =>
            {
                VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowInputPopup("Enter the alarm name", "", UnityEngine.UI.InputField.InputType.Standard, false, "Accept", new System.Action<string, Il2CppSystem.Collections.Generic.List<KeyCode>, UnityEngine.UI.Text>((string time, Il2CppSystem.Collections.Generic.List<KeyCode> keycodes, UnityEngine.UI.Text txt) =>
                {
                    currentAlarm.Name = time;
                    AlarmClock.SaveConfig();
                    VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup();
                }), null, "");
            }), "Set the name of the alarm.");
            repeatsToggle = new QMToggleButton(AlarmMenu, 3, 0, "Repeats", () => { currentAlarm.Repeats = true; AlarmClock.SaveConfig(); }, "Disabled", () => { }, "TOGGLE: Have the alarm repeat.");
            new QMSingleButton(AlarmMenu, 4, 0, "Remove", new Action(() => { AlarmClock.Alarms.Remove(currentAlarm); AlarmClock.SaveConfig(); Open(); }), "Deletes the alarm.");

            volumeSlider = new Objects.Slider(AlarmMenu.getMenuName(), 2, 1, new Action<float>((value) => { if (currentAlarm == null) return; currentAlarm.Volume = value; AlarmClock.SaveConfig(); }), 0.5f);
            volumeSlider.slider.GetComponent<RectTransform>().localScale *= 1.5f;
            //new Label(AlarmMenu.rectTransform, new Vector2(1, 1), "Volume", "VolumeSliderLabel");

            systemTimeToggle = new QMToggleButton(AlarmMenu, 1, 2, "System Time", () => { currentAlarm.IsSystemTime = true; AlarmClock.SaveConfig(); OpenAlarmMenu(); }, "Instance Time", () => {currentAlarm.IsSystemTime = false; AlarmClock.SaveConfig(); OpenAlarmMenu(); }, "TOGGLE: Switch between System time and Instance time");

            setTimeButton = new QMSingleButton(AlarmMenu, 3, 2, "Alarm\nTime", new Action(()
                =>
            {
                VRCUiPopupManager.prop_VRCUiPopupManager_0.ShowInputPopup("Set Alarm Time", "", InputField.InputType.Standard, false, "Accept", new System.Action<string, Il2CppSystem.Collections.Generic.List<KeyCode>, Text>((input, keycodes, text) =>
                {
                    if (DateTime.TryParse(input, out DateTime inputTime))
                    {
                        TimeSpan realTime = new TimeSpan(inputTime.Hour, inputTime.Minute, inputTime.Second);
                        currentAlarm.Time = realTime.Ticks;
                        AlarmClock.SaveConfig();
                    }
                    else
                    {
                        VRCUiPopupManager.prop_VRCUiPopupManager_0.ShowAlert("Error", "Input invalid, please try again!", 10f);
                    }
                }), null, "Enter a time...");
            }), "Enter a time for the alarm");
            //setTimeButton.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, UiManager.buttonSize * 3);
            //setTimeButton.TextComponent.rectTransform.anchoredPosition = new Vector2(UiManager.buttonSize * 1.5f, UiManager.buttonSize / 2);
            //setTimeButton.TextComponent.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, UiManager.buttonSize * 3);
        }

        public static void Open()
        {
            Refresh();
            Menu.OpenMenu();
        }

        private static void Refresh()
        {
            Menu.pageItems.Clear();
            for (int i = 0; i < AlarmClock.Alarms.Count; i++)
            {
                int k = i;
                Menu.pageItems.Add(new PageItem(AlarmClock.Alarms[k].Name, new Action(() =>
                {
                    currentAlarm = AlarmClock.Alarms[k];
                    OpenAlarmMenu();
                }),  $"Edit the \"{AlarmClock.Alarms[k].Name}\" alarm."));
            }
            Menu.pageItems.Add(PageItem.Space);
            Menu.OpenMenu();
        }

        private static void OnAlarmTrigger(Alarm alarm)
        {
            Managers.NotificationManager.AddNotification($"Your alarm \"{alarm.Name}\" has gone off.", "Dismiss", Managers.NotificationManager.DismissCurrentNotification, "", null, Functions.Core.Resources.alarmSprite);
            // TODO: Add sound 
        }

        private static void OpenAlarmMenu()
        {
            AlarmMenu.Open();

            enabledToggle.setToggleState(currentAlarm.IsEnabled);
            repeatsToggle.setToggleState(currentAlarm.Repeats);
            volumeSlider.slider.GetComponent<UnityEngine.UI.Slider>().value = currentAlarm.Volume;
            systemTimeToggle.setToggleState(currentAlarm.IsSystemTime);

            bool uses24HourTime = DateTimeFormatInfo.CurrentInfo.AMDesignator == "";
            string timeString;
            if (currentAlarm.IsSystemTime)
                timeString = uses24HourTime ? new DateTime(currentAlarm.Time).ToString("t") : new DateTime(currentAlarm.Time).ToString("h:mm tt");
            else
                timeString = new TimeSpan(currentAlarm.Time).ToString("c");
            setTimeButton.setButtonText("Set Alarm Time:\n" + timeString);
        }
    }
}