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
            worldTweaksButton = new SingleButton(FunctionsMenu.tweaksGroup, "World", OpenMenu, "Functions that affect the current world you're in", Functions.Core.Resources.WorldIcon);

            mirrorsGroup = new ButtonGroup(worldTweaksPage, "Mirrors");
            optimizeMirrorsButton = new SimpleSingleButton(mirrorsGroup, "Optimize", Hacks.MirrorTweaks.Optimize, "Optimizes all the mirrors in the current world by only rendering players");
            beautifyMirrorsButton = new SimpleSingleButton(mirrorsGroup, "Beautify", Hacks.MirrorTweaks.Beautify, "Improves all the mirrors in the current world by rendering <i>everything</i>");
            revertMirrorsButton = new SimpleSingleButton(mirrorsGroup, "Revert", Hacks.MirrorTweaks.Revert, "Reverts all the mirrors in the current world to their original state");

            objectsGroup = new ButtonGroup(worldTweaksPage, "Objects");

            componentTogglesGroup = new ButtonGroup(worldTweaksPage, "Components");
            portalsToggle = new ToggleButton(componentTogglesGroup, "Portals", (bool val) =>
            {
                Configuration.WriteConfigOption("PortalBlockingEnable", !val);
            }, "Allow entering portals", "Prevent entering portals");
            chairsToggle = new ToggleButton(componentTogglesGroup, "Chairs", (bool val) =>
            {
                Configuration.WriteConfigOption("ChairBlockingEnable", !val);
            }, "Allow interacting with chairs", "Prevent interaction with chairs");
            pedestalsToggle = new ToggleButton(componentTogglesGroup, "Avatar\nPedestals", (bool val) =>
            {
                Configuration.WriteConfigOption("DisableAvatarPedestals", !val);
                if (val)
                    Hacks.PedestalTweaks.Disable();
                else
                    Hacks.PedestalTweaks.Revert();
            }, "Show all avatar pedestals", "Hide all avatar pedestals");
            videoPlayersToggle = new ToggleButton(componentTogglesGroup, "Video\nPlayers", (bool val) =>
            {
                Hacks.ComponentToggle.videoplayers = val;
                Hacks.ComponentToggle.Toggle();
            }, "Enable Video Players", "Disable Video Players");
            componentTogglesGroup2 = new ButtonGroup(worldTweaksPage, "");
            pickupsToggle = new ToggleButton(componentTogglesGroup, "Pickups", (bool val) =>
            {
                Hacks.ComponentToggle.pickupable = val;
                Hacks.ComponentToggle.Toggle();
            }, "Allow picking up objects", "Prevent picking up objects");
            pickupVisibilityToggle = new ToggleButton(componentTogglesGroup, "Pickups\nVisible", (bool val) =>
            {
                Hacks.ComponentToggle.pickup_object = val;
                Hacks.ComponentToggle.Toggle();
            }, "Show pickup objects", "Hide pickup objects");

            espTogglesGroup = new ButtonGroup(worldTweaksPage, "ESP");
            itemEspToggle = new ToggleButton(espTogglesGroup, "Items", (bool val) =>
            {
                if (Managers.RiskyFunctionsManager.AreRiskyFunctionsAllowed && Configuration.JSONConfig.RiskyFunctionsEnabled)
                    Functions.WorldHacks.WorldESP.ToggleItemESP(val);
            }, "Turn on glow for items in the world", "Turn off glow for items in the world");
            triggerEspToggle = new ToggleButton(espTogglesGroup, "Triggers", (bool val) =>
            {
                if (Managers.RiskyFunctionsManager.AreRiskyFunctionsAllowed && Configuration.JSONConfig.RiskyFunctionsEnabled)
                Functions.WorldHacks.WorldESP.ToggleTriggerESP(val);
            }, "Turn on glow for triggers in the world", "Turn off glow for triggers in the world (SDK2 only)");

            Managers.RiskyFunctionsManager.OnRiskyFunctionCheckCompleted += (bool val) => {
                itemEspToggle.SetInteractable(val);
                triggerEspToggle.SetInteractable(val);
                pickupsToggle.SetInteractable(val);
                pickupVisibilityToggle.SetInteractable(val);
            };

            _initialized = true;

        }
        private static void OpenMenu()
        {
            worldTweaksPage.OpenMenu();
            chairsToggle.SetToggleState(!Configuration.JSONConfig.ChairBlockingEnable);
            pedestalsToggle.SetToggleState(!Configuration.JSONConfig.DisableAvatarPedestals);
            portalsToggle.SetToggleState(!Configuration.JSONConfig.PortalBlockingEnable);
            pickupsToggle.SetToggleState(Hacks.ComponentToggle.pickupable);
            pickupVisibilityToggle.SetToggleState(Hacks.ComponentToggle.pickup_object);
            videoPlayersToggle.SetToggleState(Hacks.ComponentToggle.videoplayers);
        }
    }
}
