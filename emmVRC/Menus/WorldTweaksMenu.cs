using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using emmVRC.Utils;
using emmVRC.Objects.ModuleBases;

namespace emmVRC.Menus
{
    [Priority(55)]
    public class WorldTweaksMenu : MelonLoaderEvents
    {
        public static MenuPage worldTweaksPage;
        private static SingleButton worldTweaksButton;

        private static ButtonGroup mirrorsGroup;
        private static SimpleSingleButton optimizeMirrorsButton;
        private static SimpleSingleButton beautifyMirrorsButton;
        private static SimpleSingleButton revertMirrorsButton;

        public static ButtonGroup objectsGroup;

        private static ButtonGroup componentTogglesGroup;
        private static ToggleButton portalsToggle;
        private static ToggleButton chairsToggle;
        private static ToggleButton pedestalsToggle;
        private static ToggleButton videoPlayersToggle;
        private static ButtonGroup componentTogglesGroup2;
        private static ToggleButton pickupsToggle;
        private static ToggleButton pickupVisibilityToggle;

        private static ButtonGroup espTogglesGroup;
        private static ToggleButton itemEspToggle;
        private static ToggleButton triggerEspToggle;

        private static bool _initialized = false;
        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            if (buildIndex != -1 || _initialized) return;

            worldTweaksPage = new MenuPage("emmVRC_WorldTweaks", "World Tweaks", false, true);
            worldTweaksButton = new SingleButton(FunctionsMenu.tweaksGroup, "World", () =>
            {
                worldTweaksPage.OpenMenu();
            }, "Functions that affect the current world you're in", Functions.Core.Resources.WorldIcon);

            mirrorsGroup = new ButtonGroup(worldTweaksPage, "Mirrors");
            optimizeMirrorsButton = new SimpleSingleButton(mirrorsGroup, "Optimize", () => { }, "Optimizes all the mirrors in the current world by only rendering players");
            beautifyMirrorsButton = new SimpleSingleButton(mirrorsGroup, "Beautify", () => { }, "Improves all the mirrors in the current world by rendering <i>everything</i>");
            revertMirrorsButton = new SimpleSingleButton(mirrorsGroup, "Revert", () => { }, "Reverts all the mirrors in the current world to their original state");

            objectsGroup = new ButtonGroup(worldTweaksPage, "Objects");

            componentTogglesGroup = new ButtonGroup(worldTweaksPage, "Components");
            portalsToggle = new ToggleButton(componentTogglesGroup, "Portals", (bool val) =>
            {

            }, "Allow entering portals", "Prevent entering portals", Functions.Core.Resources.CheckMarkIcon);
            chairsToggle = new ToggleButton(componentTogglesGroup, "Chairs", (bool val) =>
            {

            }, "Allow interacting with chairs", "Prevent interaction with chairs", Functions.Core.Resources.CheckMarkIcon);
            pedestalsToggle = new ToggleButton(componentTogglesGroup, "Avatar\nPedestals", (bool val) =>
            {

            }, "Show all avatar pedestals", "Hide all avatar pedestals", Functions.Core.Resources.CheckMarkIcon);
            videoPlayersToggle = new ToggleButton(componentTogglesGroup, "Video\nPlayers", (bool val) =>
            {

            }, "Enable Video Players", "Disable Video Players", Functions.Core.Resources.CheckMarkIcon);
            componentTogglesGroup2 = new ButtonGroup(worldTweaksPage, "");
            pickupsToggle = new ToggleButton(componentTogglesGroup, "Pickups", (bool val) =>
            {

            }, "Allow picking up objects", "Prevent picking up objects", Functions.Core.Resources.CheckMarkIcon);
            pickupVisibilityToggle = new ToggleButton(componentTogglesGroup, "Pickups\nVisible", (bool val) =>
            {

            }, "Show pickup objects", "Hide pickup objects", Functions.Core.Resources.CheckMarkIcon);

            espTogglesGroup = new ButtonGroup(worldTweaksPage, "ESP");
            itemEspToggle = new ToggleButton(espTogglesGroup, "Items", (bool val) =>
            {

            }, "Turn on glow for items in the world", "Turn off glow for items in the world", Functions.Core.Resources.CheckMarkIcon);
            triggerEspToggle = new ToggleButton(espTogglesGroup, "Triggers", (bool val) =>
            {

            }, "Turn on glow for triggers in the world", "Turn off glow for triggers in the world (SDK2 only)", Functions.Core.Resources.CheckMarkIcon);

            _initialized = true;

        }
    }
}
