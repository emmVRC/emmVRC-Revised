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
    public class PlayerTweaksMenu : MelonLoaderEvents
    {
        public static MenuPage playerTweaksPage;
        private static SingleButton playerTweaksButton;

        private static ButtonGroup avatarsGroup;
        private static SimpleSingleButton removeLoadedDynamicBonesButton;
        private static SimpleSingleButton reloadAllAvatarsButton;
        private static SimpleSingleButton avatarPermissions;
        private static SimpleSingleButton dynamicBoneOptions;

        private static ButtonGroup riskyFunctionsGroup;
        private static SimpleSingleButton jumpingToggleButton;
        private static SimpleSingleButton waypointsButton;
        private static ButtonGroup riskyFunctionsGroup2;
        private static ToggleButton flightToggle;
        private static ToggleButton noclipToggle;
        private static ToggleButton speedToggle;
        private static ToggleButton espToggle;
        private static Slider speedSlider;

        private static bool _initialized = false;
        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            if (buildIndex != -1 || _initialized) return;

            playerTweaksPage = new MenuPage("emmVRC_PlayerTweaks", "Player Tweaks", false, true);
            playerTweaksButton = new SingleButton(FunctionsMenu.tweaksGroup, "Player", OpenMenu, "Functions that affect the current world you're in", Functions.Core.Resources.PlayerIcon);

            avatarsGroup = new ButtonGroup(playerTweaksPage, "Avatars");
            removeLoadedDynamicBonesButton = new SimpleSingleButton(avatarsGroup, "Remove\nLoaded\nDynanic\nBones", () =>
            {
                foreach (DynamicBone bone in GameObject.FindObjectsOfType<DynamicBone>())
                    GameObject.Destroy(bone);
                foreach (DynamicBoneCollider coll in GameObject.FindObjectsOfType<DynamicBoneCollider>())
                    GameObject.Destroy(coll);
            }, "Unload all dynamic bones in the current instance");
            reloadAllAvatarsButton = new SimpleSingleButton(avatarsGroup, "Reload All\nAvatars", () =>
            {
                Utils.PlayerUtils.ReloadAllAvatars();
            }, "Reloads every avatar in the current instance, including the current one");
            avatarPermissions = new SimpleSingleButton(avatarsGroup, "Avatar\nPermissions", () =>
            {
                ButtonAPI.GetQuickMenuInstance().ShowAlert("Not yet implemented"); // TODO: Add thing
            }, "Turn off components on a per-avatar basis");
            dynamicBoneOptions = new SimpleSingleButton(avatarsGroup, "Global\nDynamic\nBones", () =>
            {
                ButtonAPI.GetQuickMenuInstance().ShowAlert("Not yet implemented"); // TODO: Add thing
            }, "Configure the Global Dynamic Bones system of emmVRC");

            riskyFunctionsGroup = new ButtonGroup(playerTweaksPage, "Risky Functions");
            jumpingToggleButton = new SimpleSingleButton(riskyFunctionsGroup, "Enable\nJumping", () => {
                ButtonAPI.GetQuickMenuInstance().ShowAlert("Not yet implemented"); // TODO: Add thing
            }, "Enable jumping in the current world, if it isn't available already");
            waypointsButton = new SimpleSingleButton(riskyFunctionsGroup, "Waypoints", () =>
            {
                WaypointsMenu.OpenMenu();
            }, "Configure an unlimited amount of points in the world to teleport to");
            riskyFunctionsGroup2 = new ButtonGroup(playerTweaksPage.menuContents, "");
            flightToggle = new ToggleButton(riskyFunctionsGroup, "Flight", (bool val) =>
            {
                if (Managers.RiskyFunctionsManager.AreRiskyFunctionsAllowed && Configuration.JSONConfig.RiskyFunctionsEnabled)
                    Functions.PlayerHacks.Flight.SetFlyActive(val);
            }, "Fly around the world using your controllers or mouse + keyboard", "Stop using flight");
            noclipToggle = new ToggleButton(riskyFunctionsGroup, "Noclip", (bool val) =>
            {
                if (Managers.RiskyFunctionsManager.AreRiskyFunctionsAllowed && Configuration.JSONConfig.RiskyFunctionsEnabled)
                    Functions.PlayerHacks.Flight.SetNoClipActive(val);
            }, "Clip through walls to access areas quicker, or find secrets. Requires flight", "Stop using noclip");
            speedToggle = new ToggleButton(riskyFunctionsGroup, "Speed", (bool val) =>
            {
                if (Managers.RiskyFunctionsManager.AreRiskyFunctionsAllowed && Configuration.JSONConfig.RiskyFunctionsEnabled)
                    Functions.PlayerHacks.Speed.SetActive(val);
            }, "Increase or decrease your movement speed", "Go back to the world's default speed");
            espToggle = new ToggleButton(riskyFunctionsGroup, "ESP", (bool val) =>
            {
                if (Managers.RiskyFunctionsManager.AreRiskyFunctionsAllowed && Configuration.JSONConfig.RiskyFunctionsEnabled)
                    Functions.PlayerHacks.ESP.SetActive(val);
            }, "Emit a glow around players in the current instance", "Remove glow around surrounding players");

            speedSlider = new Slider(playerTweaksPage, "Current speed", (float val) => {
                Configuration.WriteConfigOption("SpeedModifier", val/10);
                if (Functions.PlayerHacks.Speed.IsEnabled)
                {
                    Functions.PlayerHacks.Speed.SetActive(false);
                    Functions.PlayerHacks.Speed.SetActive(true);
                }
            }, "Adjust the speed that you move at", Configuration.JSONConfig.MaxSpeedIncrease*10, Configuration.JSONConfig.SpeedModifier*10, true, false);

            Managers.RiskyFunctionsManager.RiskyFuncsProcessed += (bool val) => {
                if (!val)
                {
                    flightToggle.SetToggleState(false, true);
                    noclipToggle.SetToggleState(false, true);
                    espToggle.SetToggleState(false, true);
                }
                jumpingToggleButton.SetInteractable(val);
                waypointsButton.SetInteractable(val);
                flightToggle.SetInteractable(val);
                noclipToggle.SetInteractable(val);
                speedToggle.SetInteractable(val);
                speedSlider.SetInteractable(val);
                espToggle.SetInteractable(val);
            };

            _initialized = true;
        }
        private static void OpenMenu()
        {
            playerTweaksPage.OpenMenu();
            flightToggle.SetToggleState(Functions.PlayerHacks.Flight.IsFlyEnabled);
            noclipToggle.SetToggleState(Functions.PlayerHacks.Flight.IsNoClipEnabled);
            speedToggle.SetToggleState(Functions.PlayerHacks.Speed.IsEnabled);
            espToggle.SetToggleState(Functions.PlayerHacks.ESP.IsEnabled);
        }
    }
}
