using emmVRC.Libraries;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using emmVRC.Objects;
using emmVRC.Managers;
using UnityEngine;
using emmVRC.Objects.ModuleBases;

namespace emmVRC.Menus
{
    public class InstanceHistoryMenuLegacy : MelonLoaderEvents
    {
        public static PaginatedMenu baseMenu;
        private static QMSingleButton clearInstances;

        public override void OnUiManagerInit()
        {
            baseMenu = new PaginatedMenu(FunctionsMenuLegacy.baseMenu.menuBase, 201945, 104894, "Instance\nHistory", "", null);
            baseMenu.menuEntryButton.DestroyMe();
            clearInstances = new QMSingleButton(baseMenu.menuBase, 5, 1, "<color=#FFCCBB>Clear\nInstances</color>", () =>
            {
                VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowStandardPopup("Instance History", "Are you sure you want to clear the instance history? All previously joined instances will be lost!", "Yes", new System.Action(() =>
                {
                    Functions.WorldHacks.InstanceHistory.ClearInstances();
                    LoadMenu();
                    VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup();
                }), "No", new System.Action(() =>
                {
                    VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup();
                }));
            }, "Clears the instance history of all previous instances");

        }
        public static void LoadMenu()
        {
            baseMenu.pageItems.Clear();
            foreach (SerializedWorld pastInstance in Functions.WorldHacks.InstanceHistory.previousInstances)
            {
                baseMenu.pageItems.Insert(0, new PageItem(pastInstance.WorldName + "\n" + InstanceIDUtilities.GetInstanceID(pastInstance.WorldTags), () =>
                {
                    VRCFlowManager.prop_VRCFlowManager_0.EnterWorld(pastInstance.WorldID, pastInstance.WorldTags); //Just kill me
                }, pastInstance.WorldName + (pastInstance.WorldTags.Contains("region(jp)") ? " [JP Region]" : (pastInstance.WorldTags.Contains("region(eu)") ? " [EU Region]" : "")) + " (" + Functions.WorldHacks.InstanceHistory.PrettifyInstanceType(pastInstance.WorldType) + ")" + ", last joined " + UnixTime.ToDateTime(pastInstance.loggedDateTime).ToShortDateString() + " " + UnixTime.ToDateTime(pastInstance.loggedDateTime).ToShortTimeString() + "\nSelect to join"));
            }
        }

    }
}
