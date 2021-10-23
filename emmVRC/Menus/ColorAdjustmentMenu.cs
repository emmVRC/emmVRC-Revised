using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using emmVRC.Utils;
using emmVRC.Objects.ModuleBases;
using UnityEngine;
using UnityEngine.UI;

namespace emmVRC.Menus
{
    [Priority(55)]
    public class ColorAdjustmentMenu : MelonLoaderEvents
    {
        private static bool _initialized = false;
        private static MenuPage colorAdjustmentPage;
        private static Utils.Slider rSlider;
        private static Utils.Slider gSlider;
        private static Utils.Slider bSlider;
        private static Utils.Slider aSlider;
        private static Image colorDemo;
        private static Action<Color> onColorAccepted;
        private static Color currentColor = Color.white;
        public override void OnSceneWasInitialized(int buildIndex, string sceneName)
        {
            if (buildIndex != -1 || _initialized) return;

            colorAdjustmentPage = new MenuPage("emmVRC_ColorAdjustment", "Color", false, true, true, () =>
            {
                onColorAccepted?.Invoke(currentColor);
                colorAdjustmentPage.CloseMenu();
            }, "Save the changes to this color", ButtonAPI.onIconSprite);
            rSlider = new Utils.Slider(colorAdjustmentPage, "Red", (float flt) =>
            {
                currentColor.r = (Mathf.Floor(flt) / 255);
                RenderColor();
            }, "Adjust the red value of the color", 255f, 0f, true, false);
            gSlider = new Utils.Slider(colorAdjustmentPage, "Green", (float flt) =>
            {
                currentColor.g = (Mathf.Floor(flt) / 255);
                RenderColor();
            }, "Adjust the green value of the color", 255f, 0f, true, false);

            bSlider = new Utils.Slider(colorAdjustmentPage, "Blue", (float flt) =>
            {
                currentColor.b = (Mathf.Floor(flt) / 255);
                RenderColor();
            }, "Adjust the green value of the color", 255f, 0f, true, false);
            aSlider = new Utils.Slider(colorAdjustmentPage, "Alpha", (float flt) =>
            {
                currentColor.a = (Mathf.Floor(flt) / 100);
                RenderColor();
            }, "Adjust the transparency of the color", 100f, 0f, true, false);
            ButtonGroup previewButtonGroup = new ButtonGroup(colorAdjustmentPage, "");
            SimpleSingleButton btn = new SimpleSingleButton(previewButtonGroup, "", null, "");
            GameObject.DestroyImmediate(btn.gameObject.GetComponent<VRC.UI.Elements.Tooltips.UiTooltip>());
            GameObject.DestroyImmediate(btn.gameObject.GetComponent<Button>());
            btn.gameObject.transform.DestroyChildren(a => !a.name.Contains("Foreground"));
            colorDemo = btn.gameObject.GetComponentInChildren<Image>(true);
            colorDemo.sprite = Functions.Core.Resources.TabIcon;
            btn.gameObject.transform.GetChild(0).gameObject.SetActive(true);

            

            _initialized = true;
        }
        public static void ShowMenu(Color? defaultColor, Action<Color> result)
        {
            currentColor = defaultColor ?? Color.white;
            
            rSlider.SetValue(Mathf.Floor(currentColor.r * 255));
            gSlider.SetValue(Mathf.Floor(currentColor.g * 255));
            bSlider.SetValue(Mathf.Floor(currentColor.b * 255));
            aSlider.SetValue(Mathf.Floor(currentColor.a * 255));
            onColorAccepted = result;
            colorAdjustmentPage.OpenMenu();
            RenderColor();

        }
        private static void RenderColor()
        {
            colorDemo.color = currentColor;
        }
    }
}
