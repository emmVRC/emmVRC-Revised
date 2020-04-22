using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
namespace emmVRC.Hacks
{
    internal static class colorChanger
    {
        public static void SetColorTheme(Color color)
        {
            /*void applyColorOfTypeIfExists(GameObject parent, string contains, Color clr, float opacityToUse, bool useImg = true)
            {
                clr.a = opacityToUse;
                parent.GetComponentsInChildren<Transform>(true).Where(x => x.name.Contains(contains)).ToList().ForEach(x =>
                {
                    if (useImg)
                    {
                        Image img = x?.GetComponent<Image>();
                        if (img != null)
                            img.color = clr;
                    }
                    else
                    {
                        Text txt = x?.GetComponent<Text>();
                        if (txt != null)
                            txt.color = clr;
                    }
                });
                clr.a = .7f;
            }*/

            ColorBlock buttonTheme = new ColorBlock()
            {
                colorMultiplier = 1f,
                disabledColor = Color.grey,
                highlightedColor = color * 1.5f,
                normalColor = color / 1.5f,
                pressedColor = Color.grey * 1.5f,
                fadeDuration = 0.1f
            };
            color.a = 0.9f;
            Color darker = new Color(color.r / 2.5f, color.g / 2.5f, color.b / 2.5f, color.a);

            GameObject quickMenu = Libraries.QuickMenuUtils.GetVRCUiMInstance().menuContent;
            try
            {
                /*foreach (Material mat in quickMenu.transform.Find("Popups/LoadingPopup/3DElements/LoadingBackground_TealGradient/SkyCube_Baked").GetComponent<MeshRenderer>().materials)
                {
                    mat.SetTexture("_Tex", resources.blankGradient);
                    mat.SetColor("_Tint", new Color(color.r / 1.5f, color.g / 1.5f, color.b / 1.5f, color.a));
                }*/
                //for (int i=0; i <= quickMenu.transform.Find("Popups/LoadingPopup/3DElements/LoadingBackground_TealGradient/SkyCube_Baked").GetComponent<MeshRenderer>().materials.Count(); i++)
                //quickMenu.transform.Find("Popups/LoadingPopup/3DElements/LoadingBackground_TealGradient/SkyCube_Baked").GetComponent<MeshRenderer>().material.SetTexture("_Tex", ReplaceCubemap.BuildCubemap(resources.blankGradient));
                //quickMenu.transform.Find("Popups/LoadingPopup/3DElements/LoadingBackground_TealGradient/SkyCube_Baked").GetComponent<MeshRenderer>().material = resources.gradientMaterial;
                quickMenu.transform.Find("Popups/LoadingPopup/3DElements/LoadingBackground_TealGradient/SkyCube_Baked").GetComponent<MeshRenderer>().material.SetColor("_Tint", new Color(color.r / 2.5f, color.g / 2.5f, color.b / 2.5f, color.a));
                quickMenu.transform.Find("Popups/LoadingPopup/ProgressPanel/Parent_Loading_Progress/Panel_Backdrop").GetComponent<Image>().color = color;
                quickMenu.transform.Find("Popups/LoadingPopup/ProgressPanel/Parent_Loading_Progress/Decoration_Left").GetComponent<Image>().color = color;
                quickMenu.transform.Find("Popups/LoadingPopup/ProgressPanel/Parent_Loading_Progress/Decoration_Right").GetComponent<Image>().color = color;
                /*foreach (Material mat in quickMenu.transform.Find("Popups/LoadingPopup/3DElements/LoadingInfoPanel/InfoPanel_Template_ANIM/ICON/iconFrame").GetComponent<MeshRenderer>().materials)
                {
                    mat.color = darker;
                }
                foreach (Material mat in quickMenu.transform.Find("Popups/LoadingPopup/3DElements/LoadingInfoPanel/InfoPanel_Template_ANIM/SCREEN/mainFrame").GetComponent<MeshRenderer>().materials)
                {
                    mat.color = darker;
                }
                foreach (Material mat in quickMenu.transform.Find("Popups/LoadingPopup/3DElements/LoadingInfoPanel/InfoPanel_Template_ANIM/TITLE/titleFrame").GetComponent<MeshRenderer>().materials)
                {
                    mat.color = darker;
                }*/
            }
            catch (Exception ex)
            {
                emmVRCLoader.Logger.LogError("Could not set skybox color: " + ex.ToString());
            }
            try
            {
                quickMenu.transform.Find("Screens/UserInfo/User Panel/Panel (1)").GetComponent<Image>().color = color;
                quickMenu.transform.Find("Screens/Settings_Safety/_Description_SafetyLevel").GetComponent<Image>().color = color;
                quickMenu.transform.Find("Screens/Settings_Safety/_Buttons_SafetyLevel/Button_Custom/ON").GetComponent<Image>().color = color;
                quickMenu.transform.Find("Screens/Settings_Safety/_Buttons_SafetyLevel/Button_Custom/ON/TopPanel_SafetyLevel").GetComponent<Image>().color = new Color(color.r / 0.75f, color.g / 0.75f, color.b / 0.75f);
                quickMenu.transform.Find("Screens/Settings_Safety/_Buttons_SafetyLevel/Button_None/ON").GetComponent<Image>().color = color;
                quickMenu.transform.Find("Screens/Settings_Safety/_Buttons_SafetyLevel/Button_None/ON/TopPanel_SafetyLevel").GetComponent<Image>().color = new Color(color.r / 0.75f, color.g / 0.75f, color.b / 0.75f);
                quickMenu.transform.Find("Screens/Settings_Safety/_Buttons_SafetyLevel/Button_Normal/ON").GetComponent<Image>().color = color;
                quickMenu.transform.Find("Screens/Settings_Safety/_Buttons_SafetyLevel/Button_Normal/ON/TopPanel_SafetyLevel").GetComponent<Image>().color = new Color(color.r / 0.75f, color.g / 0.75f, color.b / 0.75f);
                quickMenu.transform.Find("Screens/Settings_Safety/_Buttons_SafetyLevel/Button_Maxiumum/ON").GetComponent<Image>().color = color;
                quickMenu.transform.Find("Screens/Settings_Safety/_Buttons_SafetyLevel/Button_Maxiumum/ON/TopPanel_SafetyLevel").GetComponent<Image>().color = new Color(color.r / 0.75f, color.g / 0.75f, color.b / 0.75f);
                quickMenu.transform.Find("Popups/InputPopup/Keyboard/Keys").GetComponentsInChildren<Text>(true).ToList().ForEach(x => x.color = color);
                quickMenu.transform.Find("Popups/InputKeypadPopup/Keyboard/Keys").GetComponentsInChildren<Text>(true).ToList().ForEach(x => x.color = color);
                quickMenu.transform.Find("Popups/InputKeypadPopup/Rectangle").GetComponent<Image>().color = new Color(color.r / 2.5f, color.g / 2.5f, color.b / 2.5f);
                quickMenu.transform.Find("Popups/InputKeypadPopup/Rectangle/Panel").GetComponent<Image>().color = color;
                quickMenu.transform.Find("Popups/InputKeypadPopup/InputField").GetComponent<Image>().color = color;
                quickMenu.transform.Find("Popups/StandardPopupV2/Popup/Panel").GetComponent<Image>().color = color;
                quickMenu.transform.Find("Popups/StandardPopupV2/Popup/BorderImage").GetComponent<Image>().color = new Color(color.r / 2.5f, color.g / 2.5f, color.b / 2.5f);
                quickMenu.transform.Find("Popups/InputPopup/InputField").GetComponent<Image>().color = color;
                quickMenu.transform.Find("Popups/StandardPopup/Rectangle").GetComponent<Image>().color = new Color(color.r / 2.5f, color.g / 2.5f, color.b / 2.5f);
                quickMenu.transform.Find("Popups/StandardPopup/MidRing").GetComponent<Image>().color = new Color(color.r / 2.5f, color.g / 2.5f, color.b / 2.5f);
                quickMenu.transform.Find("Popups/StandardPopup/InnerDashRing").GetComponent<Image>().color = color;
                quickMenu.transform.Find("Popups/StandardPopup/RingGlow").GetComponent<Image>().color = color;
                quickMenu.transform.Find("Popups/UpdateStatusPopup/Popup/Panel").GetComponent<Image>().color = color;
                quickMenu.transform.Find("Popups/UpdateStatusPopup/Popup/BorderImage").GetComponent<Image>().color = new Color(color.r / 2.5f, color.g / 2.5f, color.b / 2.5f);
                quickMenu.transform.Find("Popups/UpdateStatusPopup/Popup/InputFieldStatus").GetComponent<Image>().color = color;
                quickMenu.transform.Find("Popups/AdvancedSettingsPopup/Popup/Panel").GetComponent<Image>().color = color;
                quickMenu.transform.Find("Popups/AdvancedSettingsPopup/Popup/BorderImage").GetComponent<Image>().color = darker;
                quickMenu.transform.Find("Popups/AddToPlaylistPopup/Popup/Panel").GetComponent<Image>().color = color;
                quickMenu.transform.Find("Popups/AddToPlaylistPopup/Popup/BorderImage").GetComponent<Image>().color = darker;
                quickMenu.transform.Find("Popups/BookmarkFriendPopup/Popup/Panel (2)").GetComponent<Image>().color = color;
                quickMenu.transform.Find("Popups/BookmarkFriendPopup/Popup/BorderImage").GetComponent<Image>().color = darker;
                quickMenu.transform.Find("Popups/EditPlaylistPopup/Popup/Panel").GetComponent<Image>().color = color;
                quickMenu.transform.Find("Popups/EditPlaylistPopup/Popup/BorderImage").GetComponent<Image>().color = darker;
                quickMenu.transform.Find("Popups/PerformanceSettingsPopup/Popup/Panel").GetComponent<Image>().color = color;
                quickMenu.transform.Find("Popups/PerformanceSettingsPopup/Popup/BorderImage").GetComponent<Image>().color = darker;
                quickMenu.transform.Find("Popups/AlertPopup/Lighter").GetComponent<Image>().color = color;
            }
            catch (Exception ex)
            {
                emmVRCLoader.Logger.LogError("Error loading popup colors: " + ex.ToString());
            }


            var inputHolder = quickMenu.transform.Find("Popups/InputPopup");
            darker.a = 0.8f;
            inputHolder.Find("Rectangle").GetComponent<Image>().color = darker;
            darker.a = .5f;
            color.a = 0.8f;
            inputHolder.Find("Rectangle/Panel").GetComponent<Image>().color = color;
            color.a = .5f;
            var holder = quickMenu.transform.Find("Backdrop/Header/Tabs/ViewPort/Content/Search");
            holder.Find("SearchTitle").GetComponent<Text>().color = color;
            holder.Find("InputField").GetComponent<Image>().color = color;

            /*applyColorOfTypeIfExists(quickMenu, "Panel_Header", color, color.a);
            applyColorOfTypeIfExists(quickMenu, "Fill", color, .7f);
            applyColorOfTypeIfExists(quickMenu, "Handle", color, 1);
            applyColorOfTypeIfExists(quickMenu, "Background", darker, 0.8f);*/

            ColorBlock theme = new ColorBlock()
            {
                colorMultiplier = 1f,
                disabledColor = Color.grey,
                highlightedColor = darker,
                normalColor = color,
                pressedColor = Color.gray,
                fadeDuration = 0.1f
            };

            quickMenu.GetComponentsInChildren<Transform>(true).First(x => x.name == "Row:4 Column:0").GetComponent<Button>().colors = buttonTheme;

            color.a = .5f;
            darker.a = 1f;
            theme.normalColor = darker;
            quickMenu.GetComponentsInChildren<Slider>(true).ToList().ForEach(x => x.colors = theme);
            darker.a = .5f;
            theme.normalColor = color;
            quickMenu.GetComponentsInChildren<Button>(true).ToList().ForEach(x => x.colors = buttonTheme);

            quickMenu = GameObject.Find("QuickMenu");
            quickMenu.GetComponentsInChildren<Button>(true).ToList().ForEach(x =>
            {
                if (x.gameObject.name != "ColorPickPreviewButton" && x.transform.parent.name != "SocialNotifications" && x.gameObject.name != ShortcutMenuButtons.logoButton.getGameObject().name && x.transform.parent.parent.name != "EmojiMenu")
                    x.colors = buttonTheme;
            });
            quickMenu.GetComponentsInChildren<UiToggleButton>(true).ToList().ForEach(x =>
                x.GetComponentsInChildren<Image>(true).ToList().ForEach(f => f.color = color));
            //quickMenu.GetComponentsInChildren<Slider>(true).ToList().ForEach(x => x.colors = buttonTheme);
            //quickMenu.GetComponentsInChildren<Slider>(true).ToList().ForEach(x => { x.gameObject.GetComponentInChildren<Image>().color = color; });

            //"Applied new custom coloring settings"._sout(ConsoleColor.Green);
        }
    }
}
