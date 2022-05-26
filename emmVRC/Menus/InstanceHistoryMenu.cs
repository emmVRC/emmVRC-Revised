using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using emmVRC.Utils;
using emmVRC.Libraries;
using emmVRC.Objects;
using emmVRC.Objects.ModuleBases;
using VRC.DataModel;

namespace emmVRC.Menus
{
    [Priority(55)]
    public class InstanceHistoryMenu : MelonLoaderEvents
    {
        public static MenuPage instanceHistoryPage;
        private static SingleButton instanceHistoryButton;

        private static ButtonGroup optionsGroup;
        private static SimpleSingleButton clearInstanceHistoryButton;

        private static ButtonGroup mainHistoryGroup;

        private static List<SimpleSingleButton> instanceHistoryButtons;
        private static List<ButtonGroup> instanceHistoryGroups;

        private static bool _initialized = false;
        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            if (buildIndex != -1 || _initialized) return;

            instanceHistoryPage = new MenuPage("emmVRC_InstanceHistory", "Instance History", false, true, true, () => {
                ButtonAPI.GetQuickMenuInstance().ShowConfirmDialog("Instance History", "Are you sure you want to clear your instance history?", new System.Action(() => {
                    Functions.WorldHacks.InstanceHistory.ClearInstances();
                    instanceHistoryPage.CloseMenu();
                    OpenMenu();
                    ButtonAPI.GetQuickMenuInstance().ShowAlert("Instance History has been cleared");
                }), new System.Action(() => { }));
            }, "Clear your instance history", ButtonAPI.trashIconSprite);
            instanceHistoryButton = new SingleButton(FunctionsMenu.featuresGroup, "Instance\nHistory", () =>
            {
                OpenMenu();
            }, "View all the instances you've been in, and join them if permitted", Functions.Core.Resources.WorldHistoryIcon);
            //optionsGroup = new ButtonGroup(instanceHistoryPage, "Options");
            //clearInstanceHistoryButton = new SimpleSingleButton(optionsGroup, "Clear\nHistory", () => {
            //    ButtonAPI.GetQuickMenuInstance().ShowConfirmDialog("Instance History", "Are you sure you want to clear your instance history?", new System.Action(() => {
            //        Functions.WorldHacks.InstanceHistory.ClearInstances();
            //        ButtonAPI.GetMenuStateControllerInstance().PopPage();
            //        OpenMenu();
            //        ButtonAPI.GetQuickMenuInstance().ShowAlert("Instance History has been cleared");
            //    }), new System.Action(() => { }));
            //}, "Clears your instance history");

            mainHistoryGroup = new ButtonGroup(instanceHistoryPage, "History");

            instanceHistoryButtons = new List<SimpleSingleButton>();
            instanceHistoryGroups = new List<ButtonGroup>();

            _initialized = true;

        }
        public static void OpenMenu()
        {
            foreach (SimpleSingleButton btn in instanceHistoryButtons)
                GameObject.Destroy(btn.gameObject);
            foreach (ButtonGroup grp in instanceHistoryGroups)
                grp.Destroy();
            instanceHistoryButtons.Clear();
            instanceHistoryGroups.Clear();
            int indexCount = 0;
            foreach (SerializedWorld world in Functions.WorldHacks.InstanceHistory.previousInstances)
            {
                if (indexCount == 4)
                {
                    instanceHistoryGroups.Add(new ButtonGroup(instanceHistoryPage, ""));
                    indexCount = 0;
                }
                else
                    indexCount++;
                SimpleSingleButton instanceButton = new SimpleSingleButton(mainHistoryGroup, world.WorldName + "\n" + InstanceIDUtilities.GetInstanceID(world.WorldTags), () =>
                {
                    VRCFlowManager.prop_VRCFlowManager_0.EnterWorld(world.WorldID, world.WorldTags); //Just kill me
                }, world.WorldName + (world.WorldTags.Contains("region(jp)") ? " [JP Region]" : (world.WorldTags.Contains("region(eu)") ? " [EU Region]" : "")) + " (" + Functions.WorldHacks.InstanceHistory.PrettifyInstanceType(world.WorldType) + ")" + ", last joined " + UnixTime.ToDateTime(world.loggedDateTime).ToShortDateString() + " " + UnixTime.ToDateTime(world.loggedDateTime).ToShortTimeString() + "\nSelect to join");
                instanceButton.gameObject.transform.SetAsFirstSibling();
                instanceHistoryButtons.Add(instanceButton);

            }
            instanceHistoryPage.OpenMenu();
        }
    }
}
