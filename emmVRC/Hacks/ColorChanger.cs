﻿using emmVRC.Libraries;
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
        private static List<Image> normalColorImage;
        private static List<Image> dimmerColorImage;
        private static List<Image> darkerColorImage;
        private static List<Text> normalColorText;
        private static bool setupSkybox = false;
        private static GameObject loadingBackground;
        private static GameObject initialLoadingBackground;
        public static void ApplyIfApplicable()
        {
            Color color = Configuration.JSONConfig.UIColorChangingEnabled ? Configuration.menuColor() : Configuration.defaultMenuColor();
            Color colorT = new Color(color.r, color.g, color.b, 0.7f);
            Color dimmer = new Color(color.r / 0.75f, color.g / 0.75f, color.b / 0.75f);
            Color dimmerT = new Color(color.r / 0.75f, color.g / 0.75f, color.b / 0.75f, 0.9f);
            Color darker = new Color(color.r / 2.5f, color.g / 2.5f, color.b / 2.5f);
            Color darkerT = new Color(color.r / 2.5f, color.g / 2.5f, color.b / 2.5f, 0.9f);

            if (normalColorImage == null || normalColorImage.Count == 0)
            {
                emmVRCLoader.Logger.LogDebug("Gathering elements to color normally...");
                normalColorImage = new List<Image>();
                GameObject quickMenu = Libraries.QuickMenuUtils.GetVRCUiMInstance().menuContent();
                normalColorImage.Add(quickMenu.transform.Find("Screens/Settings_Safety/_Description_SafetyLevel").GetComponent<Image>());
                normalColorImage.Add(quickMenu.transform.Find("Screens/Settings_Safety/_Buttons_SafetyLevel/Button_Custom/ON").GetComponent<Image>());
                normalColorImage.Add(quickMenu.transform.Find("Screens/Settings_Safety/_Buttons_SafetyLevel/Button_None/ON").GetComponent<Image>());
                normalColorImage.Add(quickMenu.transform.Find("Screens/Settings_Safety/_Buttons_SafetyLevel/Button_Normal/ON").GetComponent<Image>());
                normalColorImage.Add(quickMenu.transform.Find("Screens/Settings_Safety/_Buttons_SafetyLevel/Button_Maxiumum/ON").GetComponent<Image>());
                normalColorImage.Add(quickMenu.transform.Find("Popups/InputKeypadPopup/Rectangle/Panel").GetComponent<Image>());
                normalColorImage.Add(quickMenu.transform.Find("Popups/InputKeypadPopup/InputField").GetComponent<Image>());
                normalColorImage.Add(quickMenu.transform.Find("Popups/StandardPopupV2/Popup/Panel").GetComponent<Image>());
                normalColorImage.Add(quickMenu.transform.Find("Popups/StandardPopup/InnerDashRing").GetComponent<Image>());
                normalColorImage.Add(quickMenu.transform.Find("Popups/StandardPopup/RingGlow").GetComponent<Image>());
                normalColorImage.Add(quickMenu.transform.Find("Popups/UpdateStatusPopup/Popup/Panel").GetComponent<Image>());
                normalColorImage.Add(quickMenu.transform.Find("Popups/InputPopup/InputField").GetComponent<Image>());
                normalColorImage.Add(quickMenu.transform.Find("Popups/UpdateStatusPopup/Popup/InputFieldStatus").GetComponent<Image>());
                normalColorImage.Add(quickMenu.transform.Find("Popups/AdvancedSettingsPopup/Popup/Panel").GetComponent<Image>());
                normalColorImage.Add(quickMenu.transform.Find("Popups/AddToPlaylistPopup/Popup/Panel").GetComponent<Image>());
                normalColorImage.Add(quickMenu.transform.Find("Popups/BookmarkFriendPopup/Popup/Panel (2)").GetComponent<Image>());
                normalColorImage.Add(quickMenu.transform.Find("Popups/EditPlaylistPopup/Popup/Panel").GetComponent<Image>());
                normalColorImage.Add(quickMenu.transform.Find("Popups/PerformanceSettingsPopup/Popup/Panel").GetComponent<Image>());
                normalColorImage.Add(quickMenu.transform.Find("Popups/AlertPopup/Lighter").GetComponent<Image>());
                normalColorImage.Add(quickMenu.transform.Find("Popups/RoomInstancePopup/Popup/Panel").GetComponent<Image>());
                normalColorImage.Add(quickMenu.transform.Find("Popups/ReportWorldPopup/Popup/Panel").GetComponent<Image>());
                normalColorImage.Add(quickMenu.transform.Find("Popups/ReportUserPopup/Popup/Panel").GetComponent<Image>());
                normalColorImage.Add(quickMenu.transform.Find("Popups/SearchOptionsPopup/Popup/Panel (1)").GetComponent<Image>());
                normalColorImage.Add(quickMenu.transform.Find("Popups/SendInvitePopup/SendInviteMenu/Panel").GetComponent<Image>());
                normalColorImage.Add(quickMenu.transform.Find("Popups/RequestInvitePopup/RequestInviteMenu/Panel").GetComponent<Image>());
                normalColorImage.Add(quickMenu.transform.Find("Popups/ControllerBindingsPopup/Popup/Panel").GetComponent<Image>());
                normalColorImage.Add(quickMenu.transform.Find("Screens/UserInfo/User Panel/Panel (1)").GetComponent<Image>());
                foreach (Transform obj in quickMenu.GetComponentsInChildren<Transform>(true).Where(x => x.name.Contains("Panel_Header")))
                {
                    foreach (Image img in obj.GetComponentsInChildren<Image>())
                        normalColorImage.Add(img);
                }
                foreach (Transform obj in quickMenu.GetComponentsInChildren<Transform>(true).Where(x => x.name.Contains("Handle")))
                {
                    foreach (Image img in obj.GetComponentsInChildren<Image>())
                        normalColorImage.Add(img);
                }
                try
                {
                    normalColorImage.Add(quickMenu.transform.Find("Popups/LoadingPopup/ProgressPanel/Parent_Loading_Progress/Panel_Backdrop").GetComponent<Image>());
                    normalColorImage.Add(quickMenu.transform.Find("Popups/LoadingPopup/ProgressPanel/Parent_Loading_Progress/Decoration_Left").GetComponent<Image>());
                    normalColorImage.Add(quickMenu.transform.Find("Popups/LoadingPopup/ProgressPanel/Parent_Loading_Progress/Decoration_Right").GetComponent<Image>());
                }
                catch (Exception ex)
                {
                    ex = new Exception();
                }
            }
            if (dimmerColorImage == null || dimmerColorImage.Count == 0)
            {
                emmVRCLoader.Logger.LogDebug("Gathering elements to color lighter...");
                dimmerColorImage = new List<Image>();
                GameObject quickMenu = Libraries.QuickMenuUtils.GetVRCUiMInstance().menuContent();
                dimmerColorImage.Add(quickMenu.transform.Find("Screens/Settings_Safety/_Buttons_SafetyLevel/Button_Custom/ON/TopPanel_SafetyLevel").GetComponent<Image>());
                dimmerColorImage.Add(quickMenu.transform.Find("Screens/Settings_Safety/_Buttons_SafetyLevel/Button_None/ON/TopPanel_SafetyLevel").GetComponent<Image>());
                dimmerColorImage.Add(quickMenu.transform.Find("Screens/Settings_Safety/_Buttons_SafetyLevel/Button_Normal/ON/TopPanel_SafetyLevel").GetComponent<Image>());
                dimmerColorImage.Add(quickMenu.transform.Find("Screens/Settings_Safety/_Buttons_SafetyLevel/Button_Maxiumum/ON/TopPanel_SafetyLevel").GetComponent<Image>());
                foreach (Transform obj in quickMenu.GetComponentsInChildren<Transform>(true).Where(x => x.name.Contains("Fill")))
                {
                    foreach (Image img in obj.GetComponentsInChildren<Image>())
                        dimmerColorImage.Add(img);
                }
            }
            if (darkerColorImage == null || darkerColorImage.Count == 0)
            {
                emmVRCLoader.Logger.LogDebug("Gathering elements to color darker...");
                darkerColorImage = new List<Image>();
                GameObject quickMenu = Libraries.QuickMenuUtils.GetVRCUiMInstance().menuContent();
                darkerColorImage.Add(quickMenu.transform.Find("Popups/InputKeypadPopup/Rectangle").GetComponent<Image>());
                darkerColorImage.Add(quickMenu.transform.Find("Popups/StandardPopupV2/Popup/BorderImage").GetComponent<Image>());
                darkerColorImage.Add(quickMenu.transform.Find("Popups/StandardPopup/Rectangle").GetComponent<Image>());
                darkerColorImage.Add(quickMenu.transform.Find("Popups/StandardPopup/MidRing").GetComponent<Image>());
                darkerColorImage.Add(quickMenu.transform.Find("Popups/UpdateStatusPopup/Popup/BorderImage").GetComponent<Image>());
                darkerColorImage.Add(quickMenu.transform.Find("Popups/AdvancedSettingsPopup/Popup/BorderImage").GetComponent<Image>());
                darkerColorImage.Add(quickMenu.transform.Find("Popups/AddToPlaylistPopup/Popup/BorderImage").GetComponent<Image>());
                darkerColorImage.Add(quickMenu.transform.Find("Popups/BookmarkFriendPopup/Popup/BorderImage").GetComponent<Image>());
                darkerColorImage.Add(quickMenu.transform.Find("Popups/EditPlaylistPopup/Popup/BorderImage").GetComponent<Image>());
                darkerColorImage.Add(quickMenu.transform.Find("Popups/PerformanceSettingsPopup/Popup/BorderImage").GetComponent<Image>());
                darkerColorImage.Add(quickMenu.transform.Find("Popups/RoomInstancePopup/Popup/BorderImage").GetComponent<Image>());
                darkerColorImage.Add(quickMenu.transform.Find("Popups/RoomInstancePopup/Popup/BorderImage (1)").GetComponent<Image>());
                darkerColorImage.Add(quickMenu.transform.Find("Popups/ReportWorldPopup/Popup/BorderImage").GetComponent<Image>());
                darkerColorImage.Add(quickMenu.transform.Find("Popups/ReportUserPopup/Popup/BorderImage").GetComponent<Image>());
                darkerColorImage.Add(quickMenu.transform.Find("Popups/SearchOptionsPopup/Popup/BorderImage").GetComponent<Image>());
                darkerColorImage.Add(quickMenu.transform.Find("Popups/SendInvitePopup/SendInviteMenu/BorderImage").GetComponent<Image>());
                darkerColorImage.Add(quickMenu.transform.Find("Popups/RequestInvitePopup/RequestInviteMenu/BorderImage").GetComponent<Image>());
                darkerColorImage.Add(quickMenu.transform.Find("Popups/ControllerBindingsPopup/Popup/BorderImage").GetComponent<Image>());
                foreach (Transform obj in quickMenu.GetComponentsInChildren<Transform>(true).Where(x => x.name.Contains("Background")))
                {
                    foreach (Image img in obj.GetComponentsInChildren<Image>())
                        darkerColorImage.Add(img);
                }
            }
            if (normalColorText == null || normalColorText.Count == 0)
            {
                emmVRCLoader.Logger.LogDebug("Gathering text elements to color...");
                normalColorText = new List<Text>();
                GameObject quickMenu = Libraries.QuickMenuUtils.GetVRCUiMInstance().menuContent();
                foreach (Text txt in quickMenu.transform.Find("Popups/InputPopup/Keyboard/Keys").GetComponentsInChildren<Text>(true))
                    normalColorText.Add(txt);
                foreach (Text txt in quickMenu.transform.Find("Popups/InputKeypadPopup/Keyboard/Keys").GetComponentsInChildren<Text>(true))
                    normalColorText.Add(txt);
            }

            emmVRCLoader.Logger.LogDebug("Coloring normal elements...");
            foreach (Image img in normalColorImage)
                img.color = colorT;
            emmVRCLoader.Logger.LogDebug("Coloring lighter elements...");
            foreach (Image img in dimmerColorImage)
                img.color = dimmerT;
            emmVRCLoader.Logger.LogDebug("Coloring darker elements...");
            foreach (Image img in darkerColorImage)
                img.color = darkerT;
            emmVRCLoader.Logger.LogDebug("Coloring text elements...");
            foreach (Text txt in normalColorText)
                txt.color = color;

            if (!setupSkybox && !ModCompatibility.BetterLoadingScreen)
            {
                try
                {
                    emmVRCLoader.Logger.LogDebug("Setting up skybox coloring...");                  
                    Resources.blankGradient = new Texture2D(16, 16);
                    UnityEngine.ImageConversion.LoadImage(Resources.blankGradient, Convert.FromBase64String(B64Textures.Gradient), false);
                    GameObject quickMenu = Libraries.QuickMenuUtils.GetVRCUiMInstance().menuContent();
                    loadingBackground = quickMenu.transform.Find("Popups/LoadingPopup/3DElements/LoadingBackground_TealGradient/SkyCube_Baked").gameObject;
                    loadingBackground.GetComponent<MeshRenderer>().material.SetTexture("_Tex", ReplaceCubemap.BuildCubemap(Resources.blankGradient));
                    loadingBackground.GetComponent<MeshRenderer>().material.SetColor("_Tint", new Color(color.r / 2.5f, color.g / 2.5f, color.b / 2.5f, color.a));
                    loadingBackground.GetComponent<MeshRenderer>().material.SetTexture("_Tex", ReplaceCubemap.BuildCubemap(Resources.blankGradient));
                    setupSkybox = true;

                    initialLoadingBackground = GameObject.Find("LoadingBackground_TealGradient_Music/SkyCube_Baked");
                    initialLoadingBackground.GetComponent<MeshRenderer>().material.SetTexture("_Tex", ReplaceCubemap.BuildCubemap(Resources.blankGradient));
                    initialLoadingBackground.GetComponent<MeshRenderer>().material.SetColor("_Tint", new Color(color.r / 2.5f, color.g / 2.5f, color.b / 2.5f, color.a));
                    initialLoadingBackground.GetComponent<MeshRenderer>().material.SetTexture("_Tex", ReplaceCubemap.BuildCubemap(Resources.blankGradient));
                }
                catch (Exception ex)
                {
                    emmVRCLoader.Logger.LogError(ex.ToString());
                }
            }
            if (setupSkybox && loadingBackground != null && !ModCompatibility.BetterLoadingScreen)
            {
                emmVRCLoader.Logger.LogDebug("Coloring skybox...");
                loadingBackground.GetComponent<MeshRenderer>().material.SetColor("_Tint", new Color(color.r / 2.5f, color.g / 2.5f, color.b / 2.5f, color.a));
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
            try
            {
                if (Configuration.JSONConfig.UIMicIconColorChangingEnabled && Configuration.JSONConfig.UIColorChangingEnabled)
                {
                    emmVRCLoader.Logger.LogDebug("Coloring Push To Talk icons...");
                    foreach (Image img in GameObject.FindObjectOfType<HudVoiceIndicator>().transform.GetComponentsInChildren<Image>())
                        if (img.gameObject.name != "PushToTalkKeybd" && img.gameObject.name != "PushToTalkXbox")
                            img.color = color;
                }
                else
                {
                    emmVRCLoader.Logger.LogDebug("Decoloring Push To Talk icons...");
                    foreach (Image img in GameObject.FindObjectOfType<HudVoiceIndicator>().transform.GetComponentsInChildren<Image>())
                        if (img.gameObject.name != "PushToTalkKeybd" && img.gameObject.name != "PushToTalkXbox")
                            img.color = Color.red;
                }
                foreach (HudVoiceIndicator indicator in GameObject.FindObjectsOfType<HudVoiceIndicator>())
                {
                    indicator.transform.Find("VoiceDotDisabled").GetComponent<FadeCycleEffect>().enabled = Configuration.JSONConfig.UIMicIconPulsingEnabled;
                }
            }
            catch (Exception ex)
            {
                ex = new Exception();
            }
            
            if (Libraries.QuickMenuUtils.GetVRCUiMInstance().menuContent() != null)
            {
                {
                    GameObject quickMenu = Libraries.QuickMenuUtils.GetVRCUiMInstance().menuContent();
                    emmVRCLoader.Logger.LogDebug("Coloring input popup...");
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
                    emmVRCLoader.Logger.LogDebug("Coloring QM buttons...");
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
                            if (btn.gameObject.name != "SubscribeToAddPhotosButton")
                                btn.colors = buttonTheme;

                        quickMenu = GameObject.Find("QuickMenu");
                        foreach (Button btn in quickMenu.GetComponentsInChildren<Button>(true))
                        {
                            if (btn.gameObject.name != "rColorButton" && btn.gameObject.name != "gColorButton" && btn.gameObject.name != "bColorButton" && btn.gameObject.name != "ColorPickPreviewButton" && btn.transform.parent.name != "SocialNotifications" && btn.gameObject.name != ShortcutMenuButtons.logoButton.getGameObject().name && (btn.gameObject.name != (VRHUD.ToggleHUDButton == null ? "" : VRHUD.ToggleHUDButton.getGameObject().name)) && btn.transform.parent.parent.name != "EmojiMenu" && !btn.transform.parent.name.Contains("NotificationUiPrefab"))
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
                        GameObject NotificationRoot = GameObject.Find("UserInterface/QuickMenu/QuickModeMenus/QuickModeNotificationsMenu/ScrollRect");
                        foreach (Image img in NotificationRoot.GetComponentsInChildren<Image>(true))
                        {
                            if (img.transform.name == "Background")
                                img.color = color;
                        };
                        foreach (MonoBehaviourPublicObCoGaCoObCoObCoUnique tab in GameObject.Find("UserInterface/QuickMenu/QuickModeTabs").GetComponentsInChildren<MonoBehaviourPublicObCoGaCoObCoObCoUnique>())
                        {
                            Color lightlydarker = new Color(color.r / 2.25f, color.g / 2.25f, color.b / 2.25f);
                            tab.field_Public_Color32_0 = lightlydarker;
                        }
                    }
                    catch (Exception ex)
                    {
                        ex = new Exception();
                        //emmVRCLoader.Logger.LogError(ex.ToString());
                    }
                    /*emmVRCLoader.Logger.LogDebug("Coloring cursor...");
                    VRCUiCursorManager.field_Private_Static_VRCUiCursorManager_0.curso mouseCursor.UiColor = color;
                    VRCUiCursorManager.field_Private_Static_VRCUiCursorManager_0.handRightCursor.UiColor = color;
                    VRCUiCursorManager.field_Private_Static_VRCUiCursorManager_0.handLeftCursor.UiColor = color;*/


                    emmVRCLoader.Logger.LogDebug("Coloring Quick Menu header text...");
                    foreach (Text text in QuickMenuUtils.GetQuickMenuInstance().transform.Find("QuickMenu_NewElements/_CONTEXT").GetComponentsInChildren<Text>(true))
                    {
                        if (text.transform.name == "Text" && text.transform.parent.name != "AvatarImage")
                            text.color = new Color(color.r * 1.25f, color.g * 1.25f, color.b * 1.25f);
                    }
                    if (Configuration.JSONConfig.UIActionMenuColorChangingEnabled)
                        try
                        {
                            emmVRCLoader.Logger.LogDebug("Coloring Action Menu...");
                            Color referenceColor = (Configuration.JSONConfig.UIColorChangingEnabled ? Configuration.menuColor() : new Color(Configuration.defaultMenuColor().r * 1.5f, Configuration.defaultMenuColor().g * 1.5f, Configuration.defaultMenuColor().b * 1.5f));
                            Color transparent = new Color(referenceColor.r, referenceColor.g, referenceColor.b, referenceColor.a / 1.25f);
                            foreach (PedalGraphic grph in UnityEngine.Resources.FindObjectsOfTypeAll<PedalGraphic>())
                            {
                                //grph.material.SetColor("_Color", Color.white);
                                grph.color = referenceColor;
                                //grph.CrossFadeColor(Color.white, 0f, false, false);

                            }
                            foreach (ActionMenu menu in UnityEngine.Resources.FindObjectsOfTypeAll<ActionMenu>())
                            {
                                //menu.cursor.GetComponentInChildren<Image>().color = transparent;
                            }
                        }
                        catch (Exception ex)
                        {
                            emmVRCLoader.Logger.LogError(ex.ToString());
                        }

                }
            }

        }

    }
}
