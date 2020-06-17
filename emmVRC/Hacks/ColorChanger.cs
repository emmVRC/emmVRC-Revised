using emmVRC.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
namespace emmVRC.Hacks
{
    internal static class ColorChanger
    {
        public static void ApplyIfApplicable()
        {
            Color color = Configuration.JSONConfig.UIColorChangingEnabled ? Libraries.ColorConversion.HexToColor(Configuration.JSONConfig.UIColorHex) : Configuration.defaultMenuColor();
            Color darker = new Color(color.r / 2.5f, color.g / 2.5f, color.b / 2.5f, color.a);
            void applyColorOfTypeIfExists(GameObject parent, string contains, Color clr, float opacityToUse, bool useImg = true)
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
            }
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
            if (UnityEngine.Resources.FindObjectsOfTypeAll<HighlightsFXStandalone>().Count != 0)
                UnityEngine.Resources.FindObjectsOfTypeAll<HighlightsFXStandalone>().FirstOrDefault().highlightColor = color;
            if (Libraries.QuickMenuUtils.GetVRCUiMInstance().menuContent != null)
            {
                {
                    GameObject quickMenu = Libraries.QuickMenuUtils.GetVRCUiMInstance().menuContent;
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
                        foreach (Text txt in quickMenu.transform.Find("Popups/InputPopup/Keyboard/Keys").GetComponentsInChildren<Text>(true))
                            txt.color = color;
                        foreach (Text txt in quickMenu.transform.Find("Popups/InputKeypadPopup/Keyboard/Keys").GetComponentsInChildren<Text>(true))
                            txt.color = color;
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
                        quickMenu.transform.Find("Popups/RoomInstancePopup/Popup/Panel").GetComponent<Image>().color = color;
                        quickMenu.transform.Find("Popups/RoomInstancePopup/Popup/BorderImage").GetComponent<Image>().color = darker;
                        quickMenu.transform.Find("Popups/RoomInstancePopup/Popup/BorderImage (1)").GetComponent<Image>().color = darker;
                        quickMenu.transform.Find("Popups/ReportWorldPopup/Popup/Panel").GetComponent<Image>().color = color;
                        quickMenu.transform.Find("Popups/ReportWorldPopup/Popup/BorderImage").GetComponent<Image>().color = darker;
                        quickMenu.transform.Find("Popups/ReportUserPopup/Popup/Panel").GetComponent<Image>().color = color;
                        quickMenu.transform.Find("Popups/ReportUserPopup/Popup/BorderImage").GetComponent<Image>().color = darker;
                        quickMenu.transform.Find("Popups/SearchOptionsPopup/Popup/Panel (1)").GetComponent<Image>().color = color;
                        quickMenu.transform.Find("Popups/SearchOptionsPopup/Popup/BorderImage").GetComponent<Image>().color = darker;

                    }
                    catch (Exception ex)
                    {
                        emmVRCLoader.Logger.LogError(ex.ToString());
                    }
                    try
                    {
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
                    }
                    catch (Exception ex)
                    {
                        emmVRCLoader.Logger.LogError(ex.ToString());
                    }
                    try
                    {
                        applyColorOfTypeIfExists(quickMenu, "Panel_Header", color, color.a);
                        applyColorOfTypeIfExists(quickMenu, "Fill", color, .7f);
                        applyColorOfTypeIfExists(quickMenu, "Handle", color, 1);
                        applyColorOfTypeIfExists(quickMenu, "Background", darker, 0.8f);
                    }
                    catch (Exception ex)
                    {
                        emmVRCLoader.Logger.LogError(ex.ToString());
                    }
                    try
                    {
                        ColorBlock theme = new ColorBlock()
                        {
                            colorMultiplier = 1f,
                            disabledColor = Color.grey,
                            highlightedColor = darker,
                            normalColor = color,
                            pressedColor = Color.gray,
                            fadeDuration = 0.1f
                        };
                        quickMenu.GetComponentsInChildren<Transform>(true).FirstOrDefault(x => x.name == "Row:4 Column:0").GetComponent<Button>().colors = buttonTheme;

                        color.a = .5f;
                        darker.a = 1f;
                        theme.normalColor = darker;
                        foreach (Slider sldr in quickMenu.GetComponentsInChildren<Slider>(true))
                            sldr.colors = theme;
                        darker.a = .5f;
                        theme.normalColor = color;
                        foreach (Button btn in quickMenu.GetComponentsInChildren<Button>(true))
                            btn.colors = buttonTheme;

                        quickMenu = GameObject.Find("QuickMenu");
                        foreach (Button btn in quickMenu.GetComponentsInChildren<Button>(true))
                        {
                            if (btn.gameObject.name != "rColorButton" && btn.gameObject.name != "gColorButton" && btn.gameObject.name != "bColorButton" && btn.gameObject.name != "ColorPickPreviewButton" && btn.transform.parent.name != "SocialNotifications" && btn.gameObject.name != ShortcutMenuButtons.logoButton.getGameObject().name && (btn.gameObject.name != (VRHUD.ToggleHUDButton == null ? "" : VRHUD.ToggleHUDButton.getGameObject().name)) && btn.transform.parent.parent.name != "EmojiMenu")
                                btn.colors = buttonTheme;
                        };
                        foreach (UiToggleButton tglbtn in quickMenu.GetComponentsInChildren<UiToggleButton>(true))
                        {
                            foreach (Image img in tglbtn.GetComponentsInChildren<Image>(true))
                            {
                                img.color = color;
                            }
                        };
                        foreach (Slider sldr in quickMenu.GetComponentsInChildren<Slider>(true))
                        {
                            sldr.colors = theme;
                            foreach (Image img in sldr.GetComponentsInChildren<Image>(true))
                            {
                                img.color = color;
                            }
                        }
                        foreach (Toggle tgle in quickMenu.GetComponentsInChildren<Toggle>(true))
                        {
                            tgle.colors = theme;
                            foreach (Image img in tgle.GetComponentsInChildren<Image>(true))
                            {
                                img.color = color;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ex = new Exception();
                        //emmVRCLoader.Logger.LogError(ex.ToString());
                    }
                    try
                    {
                        Resources.blankGradient = new Texture2D(16, 16);
                        UnityEngine.ImageConversion.LoadImage(Resources.blankGradient, Convert.FromBase64String(B64Textures.Gradient), false);
                        quickMenu = Libraries.QuickMenuUtils.GetVRCUiMInstance().menuContent;
                        quickMenu.transform.Find("Popups/LoadingPopup/3DElements/LoadingBackground_TealGradient/SkyCube_Baked").GetComponent<MeshRenderer>().material.SetTexture("_Tex", ReplaceCubemap.BuildCubemap(Resources.blankGradient)) ;
                        quickMenu.transform.Find("Popups/LoadingPopup/3DElements/LoadingBackground_TealGradient/SkyCube_Baked").GetComponentInChildren<MeshRenderer>().material.SetColor("_Tint", new Color(color.r / 2.5f, color.g / 2.5f, color.b / 2.5f, color.a));
                        GameObject.Find("LoadingBackground_TealGradient_Music/SkyCube_Baked").GetComponent<MeshRenderer>().material.SetTexture("_Tex", ReplaceCubemap.BuildCubemap(Resources.blankGradient));
                        GameObject.Find("LoadingBackground_TealGradient_Music/SkyCube_Baked").GetComponentInChildren<MeshRenderer>().material.SetColor("_Tint", new Color(color.r / 2.5f, color.g / 2.5f, color.b / 2.5f, color.a));
                        quickMenu.transform.Find("Popups/LoadingPopup/ProgressPanel/Parent_Loading_Progress/Panel_Backdrop").GetComponentInChildren<Image>().color = color;
                        quickMenu.transform.Find("Popups/LoadingPopup/ProgressPanel/Parent_Loading_Progress/Decoration_Left").GetComponentInChildren<Image>().color = color;
                        quickMenu.transform.Find("Popups/LoadingPopup/ProgressPanel/Parent_Loading_Progress/Decoration_Right").GetComponentInChildren<Image>().color = color;
                        //quickMenu.transform.Find("Popups/LoadingPopup/3DElements/LoadingBackground_TealGradient/SkyCube_Baked").GetComponentInChildren<MeshRenderer>().material = Resources.gradientMaterial;
                        
                    }
                    catch (Exception ex)
                    {
                        ex = new Exception();
                        //emmVRCLoader.Logger.LogError(ex.ToString());
                    }
                    try
                    {
                        /*VRCUiCursorManager.field_VRCUiCursorManager_0.mouseCursor.InteractiveColor = color * 1.5f;
                        VRCUiCursorManager.field_VRCUiCursorManager_0.mouseCursor.UiColor = color * 1.5f;
                        VRCUiCursorManager.field_VRCUiCursorManager_0.mouseCursor.FloorColor = color * 1.5f;
                        VRCUiCursorManager.field_VRCUiCursorManager_0.gazeCursor.InteractiveColor = color;
                        VRCUiCursorManager.field_VRCUiCursorManager_0.gazeCursor.UiColor = color;
                        VRCUiCursorManager.field_VRCUiCursorManager_0.gazeCursor.FloorColor = color;
                        VRCUiCursorManager.field_VRCUiCursorManager_0.navigationCursor.InteractiveColor = color;
                        VRCUiCursorManager.field_VRCUiCursorManager_0.navigationCursor.UiColor = color;
                        VRCUiCursorManager.field_VRCUiCursorManager_0.navigationCursor.FloorColor = color;*/
                        VRCUiCursorManager.field_Private_Static_VRCUiCursorManager_0.mouseCursor.UiColor = color;
                        VRCUiCursorManager.field_Private_Static_VRCUiCursorManager_0.handRightCursor.UiColor = color;
                        VRCUiCursorManager.field_Private_Static_VRCUiCursorManager_0.handLeftCursor.UiColor = color;
                    }
                    catch (Exception ex)
                    {
                        ex = new Exception();
                        //emmVRCLoader.Logger.LogError(ex.ToString());
                    }
                    /*
                    try
                    {
                        VRLoadingOverlay.field_Private_Static_VRLoadingOverlay_0.field_Private_AbstractVRLoadingOverlay_0.field_Protected_Material_0.SetColor("_Tint", new Color(color.r / 2.5f, color.g / 2.5f, color.b / 2.5f, color.a));
                    } catch (Exception ex)
                    {
                        emmVRCLoader.Logger.LogError(ex.ToString());
                    }*/
                }
            }

        }

    }
}
