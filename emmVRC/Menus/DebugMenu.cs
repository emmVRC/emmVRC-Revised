using emmVRC.Libraries;
using emmVRC.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.Threading;
using UnhollowerBaseLib;

namespace emmVRC.Menus
{
    public class DebugMenu
    {
        public static QMNestedButton menuBase;
        private static QMSingleButton testPopupV1OneButtonButton;
        private static QMSingleButton testPopupV1TwoButtonsButton;
        private static QMSingleButton testPopupV2OneButtonButton;
        private static QMSingleButton testPopupV2TwoButtonsButton;
        private static QMSingleButton testInputPopupButton;
        private static QMSingleButton testHUDMessageButton;
        private static QMSingleButton testNotificationButton;

        private static QMNestedButton benchmarkMenu;
        private static QMSingleButton colorChangeBenchmark;
        private static QMSingleButton monoListBenchmark;
        private static QMSingleButton il2cppListBenchmark;

        public static PaginatedMenu debugActionsMenu;
        public static void Initialize()
        {
            menuBase = new QMNestedButton(FunctionsMenu.baseMenu.menuBase, 10293, 20934, "Debug", "");
            menuBase.getMainButton().DestroyMe();
            //loadDebugModuleButton = new QMSingleButton(menuBase, 1, 0, "Load\nDebug\nModule", () => { MelonLoader.MelonCoroutines.Start(loadDebugModule()); }, "Loads a test module from .DLL, in order to assist with testing and development of new features");

            testPopupV1OneButtonButton = new QMSingleButton(menuBase, 1, 0, "Test\nSingle\nButton\nPopup", () =>
            {
                try
                {
                    VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowStandardPopup("emmVRC Debug", "This is a test of the popup with one button.", "Okay", new System.Action(() => { VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup(); }));
                } catch (Exception ex)
                {
                    emmVRCLoader.Logger.LogError("Single button popup failed: " + ex.ToString());
                    Managers.NotificationManager.AddNotification("Single button popup failed with an exception. See the console for more details.", "Dismiss", Managers.NotificationManager.DismissCurrentNotification, "", null, Resources.errorSprite);
                }
            }, "Test the single-button variant of the standard popup");
            testPopupV1TwoButtonsButton = new QMSingleButton(menuBase, 2, 0, "Test\nDouble\nButton\nPopup", () =>
            {
                try
                {
                    VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowStandardPopup("emmVRC Debug", "This is a test of the popup with two buttons.", "Okay", new System.Action(() => { VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup(); }), "Okay again", new System.Action(() => { VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup(); }));
                }
                catch (Exception ex)
                {
                    emmVRCLoader.Logger.LogError("Double button popup failed: " + ex.ToString());
                    Managers.NotificationManager.AddNotification("Double button popup failed with an exception. See the console for more details.", "Dismiss", Managers.NotificationManager.DismissCurrentNotification, "", null, Resources.errorSprite);
                }
            }, "Test the double-button variant of the standard popup");

            testPopupV2OneButtonButton = new QMSingleButton(menuBase, 3, 0, "Test\nSingle\nButton\nPopup V2", () =>
            {
                try
                {
                    VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowStandardPopupV2("emmVRC Debug", "This is a test of the popup V2 with one button.", "Okay", new System.Action(() => { VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup(); }));
                }
                catch (Exception ex)
                {
                    emmVRCLoader.Logger.LogError("Single button popup V2 failed: " + ex.ToString());
                    Managers.NotificationManager.AddNotification("Single button popup V2 failed with an exception. See the console for more details.", "Dismiss", Managers.NotificationManager.DismissCurrentNotification, "", null, Resources.errorSprite);
                }
            }, "Test the single-button variant of the standard popup");
            testPopupV2TwoButtonsButton = new QMSingleButton(menuBase, 4, 0, "Test\nDouble\nButton\nPopup V2", () =>
            {
                try
                {
                    VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowStandardPopupV2("emmVRC Debug", "This is a test of the popup V2 with two buttons.", "Okay", new System.Action(() => { VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup(); }), "Okay again", new System.Action(() => { VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup(); }));
                }
                catch (Exception ex)
                {
                    emmVRCLoader.Logger.LogError("Double button popup V2 failed: " + ex.ToString());
                    Managers.NotificationManager.AddNotification("Double button popup V2 failed with an exception. See the console for more details.", "Dismiss", Managers.NotificationManager.DismissCurrentNotification, "", null, Resources.errorSprite);
                }
            }, "Test the double-button variant of the standard popup");
            testInputPopupButton = new QMSingleButton(menuBase, 1, 1, "Test\nInput\nPopup", () =>
            {
                try
                {
                    VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowInputPopup("Enter test text here", "", UnityEngine.UI.InputField.InputType.Standard, false, "Foobar", new System.Action<string, Il2CppSystem.Collections.Generic.List<KeyCode>, UnityEngine.UI.Text>((string str, Il2CppSystem.Collections.Generic.List<KeyCode> keycodes, UnityEngine.UI.Text txt) => {
                        VRCUiManager.prop_VRCUiManager_0.QueueHUDMessage("Text entered: " + str);
                    }), null, "Enter text...", true, null, true, 0);
                } catch (Exception ex)
                {
                    emmVRCLoader.Logger.LogError("Input popup failed: " + ex.ToString());
                    Managers.NotificationManager.AddNotification("Input popup failed with an exception. See the console for more details.", "Dismiss", Managers.NotificationManager.DismissCurrentNotification, "", null, Resources.errorSprite);
                }
            }, "Test the input popup");
            testHUDMessageButton = new QMSingleButton(menuBase, 2, 1, "Test\nHUD\nMessage", () =>
            {
                try
                {
                    VRCUiManager.prop_VRCUiManager_0.QueueHUDMessage("This is a test message");
                }
                catch (Exception ex)
                {
                    emmVRCLoader.Logger.LogError("HUD message failed: " + ex.ToString());
                    Managers.NotificationManager.AddNotification("HUD message failed with an exception. See the console for more details.", "Dismiss", Managers.NotificationManager.DismissCurrentNotification, "", null, Resources.errorSprite);
                }
            }, "Test HUD message queueing");

            testNotificationButton = new QMSingleButton(menuBase, 3, 1, "Test\nNotifications", () => {
                try
                {
                    Managers.NotificationManager.AddNotification("This is a test notification.", "Dismiss", Managers.NotificationManager.DismissCurrentNotification, "Also\nDismiss", Managers.NotificationManager.DismissCurrentNotification, Resources.onlineSprite);
                }
                catch (Exception ex)
                {
                    emmVRCLoader.Logger.LogError("Notification manager error: " + ex.ToString());
                    VRCUiManager.prop_VRCUiManager_0.QueueHUDMessage("A notification manager error occured.");
                }
            }, "Add a test notification to the Notification Manager");
            benchmarkMenu = new QMNestedButton(menuBase, 4, 1, "Benchmarks", "Test the performance of various sections of emmVRC");
            colorChangeBenchmark = new QMSingleButton(benchmarkMenu, 1, 0, "Color\nChanging\nBenchmark", () =>
            {
                System.Diagnostics.Stopwatch[] watches = new System.Diagnostics.Stopwatch[5] { new System.Diagnostics.Stopwatch(), new System.Diagnostics.Stopwatch(), new System.Diagnostics.Stopwatch(), new System.Diagnostics.Stopwatch(), new System.Diagnostics.Stopwatch() };
                var benchWatch = new System.Diagnostics.Stopwatch();
                string oldColor = Configuration.JSONConfig.UIColorHex;
                bool useColorChanging = Configuration.JSONConfig.UIColorChangingEnabled;
                Configuration.JSONConfig.UIColorHex = "#FFFFFF";
                Configuration.JSONConfig.UIColorChangingEnabled = true;
                for (int i=0; i < 5; i++)
                {
                    watches[i].Start();
                    Hacks.ColorChanger.ApplyIfApplicable();
                    watches[i].Stop();
                }
                Configuration.JSONConfig.UIColorHex = oldColor;
                Configuration.JSONConfig.UIColorChangingEnabled = useColorChanging;
                Hacks.ColorChanger.ApplyIfApplicable();
                long watchAvg = 0;
                foreach (System.Diagnostics.Stopwatch watch in watches)
                    watchAvg += watch.ElapsedMilliseconds;
                watchAvg = watchAvg / 5;
                Managers.NotificationManager.AddNotification("Average time taken for color change: " + watchAvg + "ms.", "Dismiss", Managers.NotificationManager.DismissCurrentNotification, "", null, Resources.alertSprite);
            }, "Tests the UI color changing, to see the time it consumes CPU");
            il2cppListBenchmark = new QMSingleButton(benchmarkMenu, 2, 0, "IL2CPP\nList\nBenchmark", () =>
            {
                System.Diagnostics.Stopwatch[] watchesFill = new System.Diagnostics.Stopwatch[5] { new System.Diagnostics.Stopwatch(), new System.Diagnostics.Stopwatch(), new System.Diagnostics.Stopwatch(), new System.Diagnostics.Stopwatch(), new System.Diagnostics.Stopwatch() };
                System.Diagnostics.Stopwatch[] watchesIterate = new System.Diagnostics.Stopwatch[5] { new System.Diagnostics.Stopwatch(), new System.Diagnostics.Stopwatch(), new System.Diagnostics.Stopwatch(), new System.Diagnostics.Stopwatch(), new System.Diagnostics.Stopwatch() };
                Il2CppSystem.Collections.Generic.List<float> benchList = new Il2CppSystem.Collections.Generic.List<float>();
                for (int i=0; i < 5; i++)
                {
                    watchesFill[i].Start();
                    for (int j = 0; j < 63; j++)
                        benchList.Add(j * 0.1f);
                    watchesFill[i].Stop();
                    float outsideValue = 0.0f;
                    watchesIterate[i].Start();
                    foreach (float flt in benchList)
                        outsideValue = flt;
                    watchesIterate[i].Stop();
                }
                long fillAverage = 0;
                foreach (System.Diagnostics.Stopwatch watch in watchesFill)
                    fillAverage += watch.ElapsedMilliseconds;
                fillAverage = fillAverage / 5;

                long iterateAverage = 0;
                foreach (System.Diagnostics.Stopwatch watch in watchesIterate)
                    iterateAverage += watch.ElapsedMilliseconds;
                iterateAverage = iterateAverage / 5;

                Managers.NotificationManager.AddNotification("IL2CPP Average fill time: " + fillAverage + "ms, average iterate time: " + iterateAverage + "ms.", "Dismiss", Managers.NotificationManager.DismissCurrentNotification, "", null, Resources.alertSprite);
            }, "Tests IL2CPP lists for efficiency");
            monoListBenchmark = new QMSingleButton(benchmarkMenu, 3, 0, "Mono\nList\nBenchmark", () =>
            {
                System.Diagnostics.Stopwatch[] watchesFill = new System.Diagnostics.Stopwatch[5] {new System.Diagnostics.Stopwatch(), new System.Diagnostics.Stopwatch(), new System.Diagnostics.Stopwatch(), new System.Diagnostics.Stopwatch(), new System.Diagnostics.Stopwatch() };
                System.Diagnostics.Stopwatch[] watchesIterate = new System.Diagnostics.Stopwatch[5] { new System.Diagnostics.Stopwatch(), new System.Diagnostics.Stopwatch(), new System.Diagnostics.Stopwatch(), new System.Diagnostics.Stopwatch(), new System.Diagnostics.Stopwatch() };
                System.Collections.Generic.List<float> benchList = new System.Collections.Generic.List<float>();
                for (int i = 0; i < 5; i++)
                {
                    watchesFill[i].Start();
                    for (int j = 0; j < 63; j++)
                        benchList.Add(j * 0.1f);
                    watchesFill[i].Stop();
                    float outsideValue = 0.0f;
                    watchesIterate[i].Start();
                    foreach (float flt in benchList)
                        outsideValue = flt;
                    watchesIterate[i].Stop();
                }
                long fillAverage = 0;
                foreach (System.Diagnostics.Stopwatch watch in watchesFill)
                    fillAverage += watch.ElapsedMilliseconds;
                fillAverage = fillAverage / 5;

                long iterateAverage = 0;
                foreach (System.Diagnostics.Stopwatch watch in watchesIterate)
                    iterateAverage += watch.ElapsedMilliseconds;
                iterateAverage = iterateAverage / 5;
                Managers.NotificationManager.AddNotification("Mono Average fill time: " + fillAverage + "ms, average iterate time: " + iterateAverage + "ms.", "Dismiss", Managers.NotificationManager.DismissCurrentNotification, "", null, Resources.alertSprite);
            }, "Tests Mono lists for efficiency");
            debugActionsMenu = new PaginatedMenu(menuBase, 1, 2, "Registered\nDebug\nActions", "Shows all of the registered debug actions registered in this build of emmVRC, if any", null);

        }
        public static void PopulateDebugMenu()
        {
            if (DebugManager.DebugActions.Count > 0)
                foreach (DebugAction action in DebugManager.DebugActions)
                {
                    string actionName = action.Name;
                    Action newAction = action.ActionAction;
                    if (string.IsNullOrWhiteSpace(actionName))
                        actionName = "Action " + (debugActionsMenu.pageItems.Count + 1);
                    debugActionsMenu.pageItems.Add(new PageItem(actionName, newAction, action.Description));
                }
        }
        /*public static IEnumerator loadDebugModule()
        {
            string modulePath = Path.Combine(Environment.CurrentDirectory, "Dependencies/emmVRCTest.dll");
            if (File.Exists(modulePath))
            {
                try
                {
                    byte[] debugModuleBytes = File.ReadAllBytes(modulePath);
                    Assembly debugModule = Assembly.Load(debugModuleBytes);
                    
                    foreach (Type t in GetLoadableTypes(debugModule))
                    {
                        t.GetMethod("Start", BindingFlags.Public | BindingFlags.Static).Invoke(null, null);
                    }
                    
                    //AppDomain newDomain = AppDomain.CreateDomain(AppDomain.CurrentDomain);

                    /*AppDomain newDomain = AppDomain.CreateDomain("emmVRC Debug environment");
                    byte[] buffer = File.ReadAllBytes(modulePath);
                    Assembly debugModule = newDomain.Load(buffer);
                    -- End of comment goes here
                    Managers.NotificationManager.AddNotification("Debug module execution complete.", "Dismiss", Managers.NotificationManager.DismissCurrentNotification, "", null, Resources.alertSprite);
                }
                catch (Exception ex)
                {
                    emmVRCLoader.Logger.LogError("Unable to load debug module: " + ex.ToString());
                    Managers.NotificationManager.AddNotification("Unable to load the debug module. An exception has been logged to the console.", "Dismiss", Managers.NotificationManager.DismissCurrentNotification, "", null, Resources.errorSprite);
                }
                yield return null;
            }
        }
        public static IEnumerable<Type> GetLoadableTypes(Assembly assembly)
        {
            if (assembly == null) throw new ArgumentNullException(nameof(assembly));
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                emmVRCLoader.Logger.LogError("An error occured while getting types from assembly " + assembly.GetName().Name + ". Returning types from error.\n" + e);
                return e.Types.Where(t => t != null);
            }
        }*/

    }
}
