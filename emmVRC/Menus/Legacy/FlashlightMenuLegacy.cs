using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using emmVRC.Utils;
using emmVRC.Libraries;
using emmVRC.Objects.ModuleBases;

namespace emmVRC.Menus
{
    [Priority(60)]
    public class FlashlightMenuLegacy : MelonLoaderEvents
    {
        public static QMNestedButton baseMenu;

        public static QMToggleButton toggleFlashlight;
        public static QMToggleButton toggleHeadlight;
        public static ColorPicker setFlashlightLightColor;
        public static Objects.Slider flashlightStrengthSlider;

        public static event Action<Color> LightColorChanged;
        public static event Action<float> LightStrengthChanged;

        public override void OnUiManagerInit()
        {
            baseMenu = new QMNestedButton(Menus.WorldTweaksMenuLegacy.baseMenu, 3, 1, "Flashlight", "Configure and summon a flashlight you can carry through your current world");
            toggleFlashlight = new QMToggleButton(baseMenu, 1, 0, "Flashlight On", () =>
            {
                Functions.WorldHacks.Flashlight.SetFlashlightActive(true);
            }, "Flashlight Off", () =>
            {
                Functions.WorldHacks.Flashlight.SetFlashlightActive(false);
            }, "TOGGLE: Turns on and off the flashlight");
            toggleHeadlight = new QMToggleButton(baseMenu, 2, 0, "Headlight On", () =>
            {
                Functions.WorldHacks.Flashlight.SetHeadlightActive(true);
            }, "Headlight Off", () =>
            {
                Functions.WorldHacks.Flashlight.SetHeadlightActive(false);
            }, "TOGGLE: Turns on and off the headlight");
            setFlashlightLightColor = new ColorPicker(baseMenu.getMenuName(), 4, 0, "Light\nColor", "Allows you to set the flashlight color", (Color result) =>
            {
                LightColorChanged.DelegateSafeInvoke(result);
                QuickMenuUtils.ShowQuickmenuPage(baseMenu.getMenuName());
            }, () => {
                QuickMenuUtils.ShowQuickmenuPage(baseMenu.getMenuName());
            }, Color.white, Color.white);
            flashlightStrengthSlider = new Objects.Slider(baseMenu.getMenuName(), 1, 2, new Action<float>((float flt) => {
                LightStrengthChanged.DelegateSafeInvoke(flt);
            }), 50);
            flashlightStrengthSlider.slider.GetComponent<UnityEngine.UI.Slider>().maxValue = 100;
            flashlightStrengthSlider.slider.GetComponent<UnityEngine.UI.Slider>().minValue = 10;
            flashlightStrengthSlider.slider.GetComponent<RectTransform>().anchoredPosition += new Vector2(480f, -104f);
            flashlightStrengthSlider.slider.GetComponent<RectTransform>().sizeDelta *= new Vector2(2f, 1f);

        }

    }
}
