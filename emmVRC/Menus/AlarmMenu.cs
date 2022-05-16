using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using emmVRC.Objects;
using emmVRC.Objects.ModuleBases;
using emmVRC.Libraries;
using emmVRC.Network;
using emmVRC.Utils;
using UnityEngine;
using VRC.Core;
using VRC.DataModel;

namespace emmVRC.Menus
{
    [Priority(55)]
    public class AlarmMenu : MelonLoaderEvents
    {
        public static MenuPage alarmPage;
        private static SingleButton alarmButton;

        private static ButtonGroup optionsGroup;
        private static ToggleButton alarmsEnabledToggle;

        private static ButtonGroup alarmsGroup;
        private static List<SimpleSingleButton> alarmButtons;
        private static SimpleSingleButton addAlarmButton;

        public static MenuPage editAlarmPage;
        private static ButtonGroup mainButtonsAndToggles;
        private static SimpleSingleButton alarmNameButton;
        private static SimpleSingleButton alarmTimeButton;
        private static ToggleButton alarmEnabledToggle;
        private static ToggleButton instanceAlarmToggle;
        private static ToggleButton repeatToggle;
        private static Slider volumeSlider;

        public static MenuPage deleteAlarmPage;
        private static ButtonGroup deleteAlarmButtonGroup;
        private static List<SimpleSingleButton> deleteAlarmButtons;

        private static Alarm selectedAlarm;


        private static bool _initialized = false;

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            if (buildIndex != -1 || _initialized) return;

            alarmPage = new MenuPage("emmVRC_Alarms", "Alarms", false, true, true, () => { OpenDeleteMenu(); }, "Select an alarm to be deleted", Utils.ButtonAPI.xIconSprite);
            alarmPage.menuContents.GetComponent<UnityEngine.UI.VerticalLayoutGroup>().childControlHeight = true;
            alarmButton = new SingleButton(Menus.FunctionsMenu.featuresGroup, "Alarms", () => { OpenMenu(); }, "View and set alarms to go off at real or instance times", Functions.Core.Resources.AlarmClockIcon);

            optionsGroup = new ButtonGroup(alarmPage, "Options");
            alarmsEnabledToggle = new ToggleButton(optionsGroup, "Enabled", (bool val) =>
            {
                Configuration.WriteConfigOption("AlarmsEnabled", val);
            }, "Enable all alarm clocks", "Disable all alarm clocks");
            
            alarmsGroup = new ButtonGroup(alarmPage, "Alarms");
            alarmButtons = new List<SimpleSingleButton>();

            editAlarmPage = new MenuPage("emmVRC_EditAlarms", "Edit Alarm", false, true, true, () => { Functions.Other.AlarmClock.SaveConfig(); editAlarmPage.CloseMenu(); OpenMenu(); }, "Save the current alarm", Functions.Core.Resources.CheckMarkIcon);
            mainButtonsAndToggles = new ButtonGroup(editAlarmPage, "");

            alarmNameButton = new SimpleSingleButton(mainButtonsAndToggles, "Name", () => {
                VRCUiPopupManager.prop_VRCUiPopupManager_0.ShowInputPopup("Enter the name for the alarm", "", UnityEngine.UI.InputField.InputType.Standard, false, "Accept", new System.Action<string, Il2CppSystem.Collections.Generic.List<UnityEngine.KeyCode>, UnityEngine.UI.Text>((string str, Il2CppSystem.Collections.Generic.List<KeyCode> keycodes, UnityEngine.UI.Text text) =>
                {
                    if (str == "" || str == null)
                        str = new TimeSpan(selectedAlarm.Time).ToString("h':'mm");
                    selectedAlarm.Name = str;
                }), null);
            }, "Select to set the alarm's name");
            alarmTimeButton = new SimpleSingleButton(mainButtonsAndToggles, "Time", () =>
            {
                VRCUiPopupManager.prop_VRCUiPopupManager_0.ShowInputPopup("Enter the time for the alarm", new TimeSpan(selectedAlarm.Time).ToString("h':'mm"+(selectedAlarm.IsSystemTime ? "" : "':'ss")), UnityEngine.UI.InputField.InputType.Standard, false, "Accept", new System.Action<string, Il2CppSystem.Collections.Generic.List<UnityEngine.KeyCode>, UnityEngine.UI.Text>((string str, Il2CppSystem.Collections.Generic.List<UnityEngine.KeyCode> keycodes, UnityEngine.UI.Text txt) => {
                    try
                    {
                        selectedAlarm.Time = TimeSpan.Parse(str).Ticks;
                        OpenMenu();
                        OpenAddEditMenu();
                    } catch (Exception ex)
                    {
                        emmVRCLoader.Logger.LogError("The entered string could not be parsed. Alarm time unchanged. Error: " + ex.ToString());
                        ButtonAPI.GetQuickMenuInstance().ShowAlert("Could not parse the entered time");
                    }
                }), null);
            }, "Select to change the time of the alarm");
            alarmEnabledToggle = new ToggleButton(mainButtonsAndToggles, "Enabled", (bool val) => { selectedAlarm.IsEnabled = val; }, "Alarm is disabled", "Alarm is enabled");
            instanceAlarmToggle = new ToggleButton(mainButtonsAndToggles, "Instance Time", (bool val) => { selectedAlarm.IsSystemTime = !val; }, "Alarm will go off when the real time reaches the alarm time", "Alarm will go off when the instance time reaches the alarm time");
            repeatToggle = new ToggleButton(mainButtonsAndToggles, "Repeat", (bool val) => { selectedAlarm.Repeats = val; }, "Alarm will disable after going off", "Alarm will go off more than once");
            ButtonGroup volGroup = new ButtonGroup(editAlarmPage, "");
            volumeSlider = new Slider(editAlarmPage, "Volume", (float val) => { selectedAlarm.Volume = val / 100; }, "Adjust the volume of this alarm", 100, 100, true, true);

            deleteAlarmPage = new MenuPage("emmVRC_DeleteAlarms", "Delete Alarms", false, true);
            deleteAlarmPage.menuContents.GetComponent<UnityEngine.UI.VerticalLayoutGroup>().childControlHeight = true;
            deleteAlarmButtonGroup = new ButtonGroup(deleteAlarmPage, "");
            deleteAlarmButtons = new List<SimpleSingleButton>();

            _initialized = true;
        }

        private static void OpenAddEditMenu()
        {
            if (selectedAlarm == null)
            {
                selectedAlarm = new Alarm {
                    Time = 0,
                    Name = "0:00",
                    IsEnabled = false,
                    IsSystemTime = true,
                    Repeats = false,
                    Volume = 1f,
                };
                Functions.Other.AlarmClock.Alarms.Add(selectedAlarm);
            }
            mainButtonsAndToggles.SetText("<color=white>Time: " + DateTime.Now.TimeOfDay.ToString("h':'mm") + "\nInstance: "+ TimeSpan.FromSeconds(RoomManager.prop_Single_0).ToString("h':'mm':'ss"));
            alarmTimeButton.SetText("<color=white>"+new TimeSpan(selectedAlarm.Time).ToString("h':'mm"+(selectedAlarm.IsSystemTime ? "" : "':'ss"))+"</color>");
            alarmEnabledToggle.SetToggleState(selectedAlarm.IsEnabled);
            instanceAlarmToggle.SetToggleState(!selectedAlarm.IsSystemTime);
            repeatToggle.SetToggleState(selectedAlarm.Repeats);
            volumeSlider.SetValue(selectedAlarm.Volume * 100);

            editAlarmPage.OpenMenu();
        }

        private static void OpenMenu()
        {
            alarmsEnabledToggle.SetToggleState(Configuration.JSONConfig.AlarmsEnabled);
            foreach (SimpleSingleButton btn in alarmButtons)
                GameObject.Destroy(btn.gameObject);
            alarmButtons = new List<SimpleSingleButton>();
            addAlarmButton = new SimpleSingleButton(alarmsGroup, "New\nAlarm", () => {
                selectedAlarm = null;
                OpenAddEditMenu();
            }, "Select to add a new alarm");
            alarmButtons.Add(addAlarmButton);
            foreach (Alarm alarm in Functions.Other.AlarmClock.Alarms)
            {
                SimpleSingleButton alarmButton = new SimpleSingleButton(alarmsGroup, alarm.Name+"\n<color=white>"+new TimeSpan(alarm.Time).ToString("h':'mm"+(alarm.IsSystemTime ? "" : "':'ss"))+ (alarm.IsSystemTime ? "</color>" : "</color>\n<color=yellow>[Instance]</color>")+"\n"+(alarm.IsEnabled ? "<color=green>Enabled</color>" : "<color=grey>Disabled</color>"), () =>
                {
                    selectedAlarm = alarm;
                    OpenAddEditMenu();
                }, "Select to modify this alarm");
                alarmButtons.Add(alarmButton);
            }
            alarmPage.OpenMenu();
        }
        private static void OpenDeleteMenu()
        {
            foreach (SimpleSingleButton btn in deleteAlarmButtons)
                GameObject.Destroy(btn.gameObject);
            deleteAlarmButtons = new List<SimpleSingleButton>();
            foreach (Alarm alarm in Functions.Other.AlarmClock.Alarms)
            {
                SimpleSingleButton alarmButton = new SimpleSingleButton(deleteAlarmButtonGroup, alarm.Name + "\n<color=white>" + new TimeSpan(alarm.Time).ToString("h':'mm" + (alarm.IsSystemTime ? "" : "':'ss")) + (alarm.IsSystemTime ? "</color>" : "</color>\n<color=yellow>[Instance]</color>") + "\n" + (alarm.IsEnabled ? "<color=green>Enabled</color>" : "<color=grey>Disabled</color>"), () =>
                {
                    ButtonAPI.GetQuickMenuInstance().ShowCustomDialog(alarm.Name, "Are you sure you want to delete this alarm?", "", "Yes", "No", null, () => { Functions.Other.AlarmClock.Alarms.Remove(alarm); Functions.Other.AlarmClock.SaveConfig(); OpenMenu(); OpenDeleteMenu(); });
                }, "Select to modify this alarm");
                deleteAlarmButtons.Add(alarmButton);
            }
            deleteAlarmPage.OpenMenu();
        }
    }
}
