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
    [Priority(60)]
    public class FlashlightMenu : MelonLoaderEvents
    {
        public static MenuPage flashlightPage;
        private static SimpleSingleButton flashlightButton;

        private static ButtonGroup lightTogglesGroup;
        private static ToggleButton flashlightToggle;
        private static ToggleButton headlightToggle;

        private static ButtonGroup optionsGroup;
        private static SingleButton setColor;

        private static Utils.Slider rangeSlider;
        private static Utils.Slider powerSlider;
        private static Utils.Slider angleSlider;


        private static bool _initialized = false;
        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            if (buildIndex != -1 || _initialized) return;

            flashlightPage = new MenuPage("emmVRC_FlashlightMenu", "Flashlight", false, true);
            flashlightButton = new SimpleSingleButton(WorldTweaksMenu.objectsGroup, "Flashlight", OpenMenu, "Spawn a flashlight you can carry around, or use the headlight to light dark areas");

            lightTogglesGroup = new ButtonGroup(flashlightPage, "Toggles");
            flashlightToggle = new ToggleButton(lightTogglesGroup, "Flashlight", (bool val) => Functions.WorldHacks.Flashlight.SetFlashlightActive(val), "Spawn a flashlight in the world", "Remove the flashlight in the world");
            headlightToggle = new ToggleButton(lightTogglesGroup, "Headlight", (bool val) => Functions.WorldHacks.Flashlight.SetHeadlightActive(val), "Turn on a flashlight attached to your head", "Turn off the flashlight attacked to your head");

            rangeSlider = new Slider(flashlightPage, "Range", (float val) => Configuration.WriteConfigOption("FlashlightRange", val), "", 20f, Configuration.JSONConfig.FlashlightRange);
            powerSlider = new Slider(flashlightPage, "Power", (float val) => Configuration.WriteConfigOption("FlashlightPower", val), "", 2f, Configuration.JSONConfig.FlashlightPower);
            angleSlider = new Slider(flashlightPage, "Angle", (float val) => Configuration.WriteConfigOption("FlashlightAngle", val), "", 180f, Configuration.JSONConfig.FlashlightAngle);

            optionsGroup = new ButtonGroup(flashlightPage, "");
            setColor = new SingleButton(optionsGroup, "Light\nColor", () =>
            {
                ColorAdjustmentMenu.ShowMenu(Libraries.ColorConversion.HexToColor(Configuration.JSONConfig.FlashlightColorHex), (Color col) => {
                    Configuration.WriteConfigOption("FlashlightColorHex", Libraries.ColorConversion.ColorToHex(col, true));
                    setColor.SetIconColor(Libraries.ColorConversion.HexToColor(Configuration.JSONConfig.FlashlightColorHex));
                });
            }, "Adjust the color of the light from the flashlight/headlight", Functions.Core.Resources.TabIcon, true);
            setColor.SetIconColor(Libraries.ColorConversion.HexToColor(Configuration.JSONConfig.FlashlightColorHex));
            _initialized = true;
        }
        private static void OpenMenu()
        {
            flashlightPage.OpenMenu();
        }
    }
}
