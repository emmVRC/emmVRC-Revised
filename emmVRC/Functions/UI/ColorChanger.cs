﻿using emmVRC.Libraries;
using emmVRC.Utils;
using emmVRC.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using emmVRC.Objects.ModuleBases;

namespace emmVRC.Functions.UI
{
    [Priority(255)]
    internal class ColorChanger : MelonLoaderEvents
    {
        private static List<Image> normalColorImage;
        private static List<Image> dimmerColorImage;
        private static List<Image> darkerColorImage;
        private static List<Text> normalColorText;
        private static bool setupSkybox = false;
        private static GameObject loadingBackground;
        private static GameObject initialLoadingBackground;
        private static Dictionary<UnityEngine.Object, Color> originalColours;
        private static Dictionary<UnityEngine.Object, Texture2D> originalTextures;
        private static Dictionary<UnityEngine.Object, Sprite> originalSprites;
        private static Dictionary<UnityEngine.Object, ColorBlock> originalColourBlocks;
        private static bool collectingColours = false;

        public override void OnUiManagerInit()
        {
            Configuration.onConfigUpdated.Add(new System.Collections.Generic.KeyValuePair<string, Action>("UIColorChangingEnabled", ApplyIfApplicable));
            Configuration.onConfigUpdated.Add(new System.Collections.Generic.KeyValuePair<string, Action>("UIColorHex", ApplyIfApplicable));
            Configuration.onConfigUpdated.Add(new System.Collections.Generic.KeyValuePair<string, Action>("UIActionMenuColorChangingEnabled", ApplyIfApplicable));
            Configuration.onConfigUpdated.Add(new System.Collections.Generic.KeyValuePair<string, Action>("UIMicIconColorChangingEnabled", ApplyIfApplicable));
            Configuration.onConfigUpdated.Add(new System.Collections.Generic.KeyValuePair<string, Action>("UIMicIconPulsingEnabled", ApplyIfApplicable));
            ApplyIfApplicable();
        }
        public static void ApplyIfApplicable()
        {
            if (originalColours == null || originalTextures == null || originalSprites == null)
            {
                originalColours = new Dictionary<UnityEngine.Object, Color>();
                originalTextures = new Dictionary<UnityEngine.Object, Texture2D>();
                originalSprites = new Dictionary<UnityEngine.Object, Sprite>();
                originalColourBlocks = new Dictionary<UnityEngine.Object, ColorBlock>();
                collectingColours = true;
            }
            Color color = Configuration.JSONConfig.UIColorChangingEnabled ? Configuration.menuColor() : Configuration.defaultMenuColor();
            Color colorT = new Color(color.r, color.g, color.b, 0.7f);
            Color dimmer = new Color(color.r / 0.75f, color.g / 0.75f, color.b / 0.75f);
            Color dimmerT = new Color(color.r / 0.75f, color.g / 0.75f, color.b / 0.75f, 0.9f);
            Color darker = new Color(color.r / 2.5f, color.g / 2.5f, color.b / 2.5f);
            Color darkerT = new Color(color.r / 2.5f, color.g / 2.5f, color.b / 2.5f, 0.9f);

            GameObject quickMenu = VRC.UI.UIManagerImpl.prop_UIManagerImpl_0.field_Private_Transform_0.Find("MenuContent").gameObject;

            if (normalColorImage == null || normalColorImage.Count == 0)
            {
                emmVRCLoader.Logger.LogDebug("Gathering elements to color normally...");
                normalColorImage = new List<Image>();
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
                normalColorImage.Add(quickMenu.transform.Find("Popups/AddToFavoriteListPopup/Popup/Panel").GetComponent<Image>());
                normalColorImage.Add(quickMenu.transform.Find("Popups/EditFavoriteListPopup/Popup/Panel").GetComponent<Image>());
                normalColorImage.Add(quickMenu.transform.Find("Popups/PerformanceSettingsPopup/Popup/Panel").GetComponent<Image>());
                normalColorImage.Add(quickMenu.transform.Find("Popups/AlertPopup/Lighter").GetComponent<Image>());
                normalColorImage.Add(quickMenu.transform.Find("Popups/RoomInstancePopup/Popup/Panel").GetComponent<Image>());
                normalColorImage.Add(quickMenu.transform.Find("Popups/ReportWorldPopup/Popup/Panel").GetComponent<Image>());
                normalColorImage.Add(quickMenu.transform.Find("Popups/ReportUserPopup/Popup/Panel").GetComponent<Image>());
                normalColorImage.Add(quickMenu.transform.Find("Popups/SearchOptionsPopup/Popup/Panel (1)").GetComponent<Image>());
                normalColorImage.Add(quickMenu.transform.Find("Popups/SendInvitePopup/SendInviteMenu/Panel").GetComponent<Image>());
                normalColorImage.Add(quickMenu.transform.Find("Popups/RequestInvitePopup/RequestInviteMenu/Panel").GetComponent<Image>());
                normalColorImage.Add(quickMenu.transform.Find("Popups/ControllerBindingsPopup/Popup/Panel").GetComponent<Image>());
                normalColorImage.Add(quickMenu.transform.Find("Popups/ChangeProfilePicPopup/Popup/PanelBackground").GetComponent<Image>());
                normalColorImage.Add(quickMenu.transform.Find("Popups/ChangeProfilePicPopup/Popup/TitlePanel").GetComponent<Image>());
                normalColorImage.Add(quickMenu.transform.Find("Screens/UserInfo/User Panel/PanelHeaderBackground").GetComponent<Image>());
                normalColorImage.Add(quickMenu.transform.Find("Popups/StandardPopup/ArrowLeft").GetComponent<Image>());
                normalColorImage.Add(quickMenu.transform.Find("Popups/StandardPopup/ArrowRight").GetComponent<Image>());
                //normalColorImage.Add(quickMenu.transform.Find("Screens/UserInfo/User Panel/Panel (1)").GetComponent<Image>());
                foreach (Transform obj in quickMenu.GetComponentsInChildren<Transform>(true).Where(x => x.name.Contains("Panel_Header")))
                {
                    foreach (Image img in obj.GetComponentsInChildren<Image>())
                        if (img.gameObject.name != "Checkmark")
                            normalColorImage.Add(img);
                }
                foreach (Transform obj in quickMenu.GetComponentsInChildren<Transform>(true).Where(x => x.name.Contains("Handle")))
                {
                    foreach (Image img in obj.GetComponentsInChildren<Image>())
                        if (img.gameObject.name != "Checkmark")
                            normalColorImage.Add(img);
                }
                try
                {

                    normalColorImage.Add(quickMenu.transform.Find("Popups/LoadingPopup/ProgressPanel/Parent_Loading_Progress/Panel_Backdrop").GetComponent<Image>());
                    normalColorImage.Add(quickMenu.transform.Find("Popups/LoadingPopup/ProgressPanel/Parent_Loading_Progress/Decoration_Left").GetComponent<Image>());
                    normalColorImage.Add(quickMenu.transform.Find("Popups/LoadingPopup/ProgressPanel/Parent_Loading_Progress/Decoration_Right").GetComponent<Image>());
                    normalColorImage.Add(quickMenu.transform.Find("Popups/LoadingPopup/MirroredElements/ProgressPanel (1)/Parent_Loading_Progress/Panel_Backdrop").GetComponent<Image>());
                    normalColorImage.Add(quickMenu.transform.Find("Popups/LoadingPopup/MirroredElements/ProgressPanel (1)/Parent_Loading_Progress/Decoration_Left").GetComponent<Image>());
                    normalColorImage.Add(quickMenu.transform.Find("Popups/LoadingPopup/MirroredElements/ProgressPanel (1)/Parent_Loading_Progress/Decoration_Right").GetComponent<Image>());
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
                dimmerColorImage.Add(quickMenu.transform.Find("Screens/Settings_Safety/_Buttons_SafetyLevel/Button_Custom/ON/TopPanel_SafetyLevel").GetComponent<Image>());
                dimmerColorImage.Add(quickMenu.transform.Find("Screens/Settings_Safety/_Buttons_SafetyLevel/Button_None/ON/TopPanel_SafetyLevel").GetComponent<Image>());
                dimmerColorImage.Add(quickMenu.transform.Find("Screens/Settings_Safety/_Buttons_SafetyLevel/Button_Normal/ON/TopPanel_SafetyLevel").GetComponent<Image>());
                dimmerColorImage.Add(quickMenu.transform.Find("Screens/Settings_Safety/_Buttons_SafetyLevel/Button_Maxiumum/ON/TopPanel_SafetyLevel").GetComponent<Image>());
                dimmerColorImage.Add(quickMenu.transform.Find("Popups/ChangeProfilePicPopup/Popup/BorderImage").GetComponent<Image>());
                foreach (Transform obj in quickMenu.GetComponentsInChildren<Transform>(true).Where(x => x.name.Contains("Fill")))
                {
                    foreach (Image img in obj.GetComponentsInChildren<Image>())
                        if (img.gameObject.name != "Checkmark")
                            dimmerColorImage.Add(img);
                }
            }
            if (darkerColorImage == null || darkerColorImage.Count == 0)
            {
                emmVRCLoader.Logger.LogDebug("Gathering elements to color darker...");
                darkerColorImage = new List<Image>();
                darkerColorImage.Add(quickMenu.transform.Find("Popups/InputKeypadPopup/Rectangle").GetComponent<Image>());
                darkerColorImage.Add(quickMenu.transform.Find("Popups/StandardPopupV2/Popup/BorderImage").GetComponent<Image>());
                darkerColorImage.Add(quickMenu.transform.Find("Popups/StandardPopup/Rectangle").GetComponent<Image>());
                darkerColorImage.Add(quickMenu.transform.Find("Popups/StandardPopup/MidRing").GetComponent<Image>());
                darkerColorImage.Add(quickMenu.transform.Find("Popups/UpdateStatusPopup/Popup/BorderImage").GetComponent<Image>());
                darkerColorImage.Add(quickMenu.transform.Find("Popups/AdvancedSettingsPopup/Popup/BorderImage").GetComponent<Image>());
                darkerColorImage.Add(quickMenu.transform.Find("Popups/AddToFavoriteListPopup/Popup/BorderImage").GetComponent<Image>());
                darkerColorImage.Add(quickMenu.transform.Find("Popups/EditFavoriteListPopup/Popup/BorderImage").GetComponent<Image>());
                darkerColorImage.Add(quickMenu.transform.Find("Popups/PerformanceSettingsPopup/Popup/BorderImage").GetComponent<Image>());
                darkerColorImage.Add(quickMenu.transform.Find("Popups/RoomInstancePopup/Popup/BorderImage").GetComponent<Image>());
                darkerColorImage.Add(quickMenu.transform.Find("Popups/RoomInstancePopup/Popup/BorderImage (1)").GetComponent<Image>());
                darkerColorImage.Add(quickMenu.transform.Find("Popups/ReportWorldPopup/Popup/BorderImage").GetComponent<Image>());
                darkerColorImage.Add(quickMenu.transform.Find("Popups/ReportUserPopup/Popup/BorderImage").GetComponent<Image>());
                darkerColorImage.Add(quickMenu.transform.Find("Popups/SearchOptionsPopup/Popup/BorderImage").GetComponent<Image>());
                darkerColorImage.Add(quickMenu.transform.Find("Popups/SendInvitePopup/SendInviteMenu/BorderImage").GetComponent<Image>());
                darkerColorImage.Add(quickMenu.transform.Find("Popups/RequestInvitePopup/RequestInviteMenu/BorderImage").GetComponent<Image>());
                darkerColorImage.Add(quickMenu.transform.Find("Popups/ControllerBindingsPopup/Popup/BorderImage").GetComponent<Image>());
                darkerColorImage.Add(quickMenu.transform.Find("Screens/UserInfo/ModerateDialog/Panel/BorderImage").GetComponent<Image>());
                foreach (Transform obj in quickMenu.GetComponentsInChildren<Transform>(true).Where(x => (x.name.Contains("Background") || x.name.Contains("TitlePanel")) && x.name != "PanelHeaderBackground" && !x.transform.parent.name.Contains("UserIcon") && x.transform.name != "Button_PerformanceOptions"))
                {
                    foreach (Image img in obj.GetComponentsInChildren<Image>())
                        if (img.gameObject.name != "Checkmark")
                            darkerColorImage.Add(img);
                }
            }
            if (normalColorText == null || normalColorText.Count == 0)
            {
                emmVRCLoader.Logger.LogDebug("Gathering text elements to color...");
                normalColorText = new List<Text>();
                foreach (Text txt in quickMenu.transform.Find("Popups/InputPopup/Keyboard/Keys").GetComponentsInChildren<Text>(true))
                    normalColorText.Add(txt);
                foreach (Text txt in quickMenu.transform.Find("Popups/InputKeypadPopup/Keyboard/Keys").GetComponentsInChildren<Text>(true))
                    normalColorText.Add(txt);
                normalColorText.Add(quickMenu.transform.Find("Screens/Settings/VolumePanel/VolumeGameWorld/Label").GetComponentInChildren<Text>(true));
                normalColorText.Add(quickMenu.transform.Find("Screens/Settings/VolumePanel/VolumeGameVoice/Label").GetComponentInChildren<Text>(true));
                normalColorText.Add(quickMenu.transform.Find("Screens/Settings/VolumePanel/VolumeGameAvatars/Label").GetComponentInChildren<Text>(true));
                normalColorText.AddRange(quickMenu.transform.Find("Screens/Social/UserProfileAndStatusSection").GetComponentsInChildren<Text>(true));
                //normalColorText.Add(quickMenu.transform.Find("Popups/LoadingPopup/ProgressPanel/Parent_Loading_Progress/Loading Elements/txt_Percent").GetComponentInChildren<Text>(true));
                normalColorText.Add(quickMenu.transform.Find("Popups/LoadingPopup/ProgressPanel/Parent_Loading_Progress/Loading Elements/txt_LOADING_Size").GetComponentInChildren<Text>(true));
                //normalColorText.Add(quickMenu.transform.Find("Popups/LoadingPopup/MirroredElements/ProgressPanel (1)/Parent_Loading_Progress/Loading Elements/txt_Percent").GetComponentInChildren<Text>(true));
                normalColorText.Add(quickMenu.transform.Find("Popups/LoadingPopup/MirroredElements/ProgressPanel (1)/Parent_Loading_Progress/Loading Elements/txt_LOADING_Size").GetComponentInChildren<Text>(true));
            }

            if (collectingColours)
            {
                foreach (Image img in normalColorImage)
                {
                    originalColours.Add(img, img.color);
                    if (img.sprite != null && img.sprite.texture != null)
                    {
                        originalSprites.Add(img, img.sprite);
                    }
                }
                foreach (Image img in dimmerColorImage)
                {
                    originalColours.Add(img, img.color);
                    if (img.sprite != null && img.sprite.texture != null)
                    {
                        originalSprites.Add(img, img.sprite);
                    }
                }
                foreach (Image img in darkerColorImage)
                {
                    originalColours.Add(img, img.color);
                    if (img.sprite != null && img.sprite.texture != null)
                    {
                        originalSprites.Add(img, img.sprite);
                    }
                }
                foreach (Text txt in normalColorText)
                    originalColours.Add(txt, txt.color);
            }

            if (Configuration.JSONConfig.UIColorChangingEnabled)
            {
                emmVRCLoader.Logger.LogDebug("Coloring normal elements...");
                foreach (Image img in normalColorImage)
                {
                    if (img.sprite != null && img.sprite.texture != null)
                        img.sprite = originalSprites[img].ReplaceTexture(img.sprite.UnpackTexture().Desaturate());
                    img.color = colorT;
                }
                emmVRCLoader.Logger.LogDebug("Coloring lighter elements...");
                foreach (Image img in dimmerColorImage)
                {
                    if (img.sprite != null && img.sprite.texture != null)
                        img.sprite = originalSprites[img].ReplaceTexture(img.sprite.UnpackTexture().Desaturate());
                    img.color = dimmerT;
                }
                emmVRCLoader.Logger.LogDebug("Coloring darker elements...");
                foreach (Image img in darkerColorImage)
                {
                    if (img.sprite != null && img.sprite.texture != null)
                        img.sprite = originalSprites[img].ReplaceTexture(img.sprite.UnpackTexture().Desaturate());
                    img.color = darkerT;
                }
                emmVRCLoader.Logger.LogDebug("Coloring text elements...");
                foreach (Text txt in normalColorText)
                    txt.color = color;
            }
            else if (!Configuration.JSONConfig.UIColorChangingEnabled && originalColours != null)
            {
                foreach (Image img in normalColorImage)
                    img.color = originalColours.FirstOrDefault(a => a.Key.GetType() == typeof(Image) && (Image)a.Key == img).Value;
                foreach (Image img in dimmerColorImage)
                    img.color = originalColours.FirstOrDefault(a => a.Key.GetType() == typeof(Image) && (Image)a.Key == img).Value;
                foreach (Image img in darkerColorImage)
                    img.color = originalColours.FirstOrDefault(a => a.Key.GetType() == typeof(Image) && (Image)a.Key == img).Value;
                foreach (Text txt in normalColorText)
                    txt.color = originalColours.FirstOrDefault(a => a.Key.GetType() == typeof(Text) && (Text)a.Key == txt).Value;
            }

            if (!Configuration.JSONConfig.UIColorChangingEnabled && !collectingColours)
            {
                foreach (KeyValuePair<UnityEngine.Object, Sprite> kvp in originalSprites)
                {
                    if (kvp.Key != null && kvp.Key.GetType() == typeof(Image))
                        ((Image)kvp.Key).sprite = kvp.Value;
                }
            }
            if (!setupSkybox && !Functions.Core.ModCompatibility.BetterLoadingScreen)
            {
                try
                {
                    emmVRCLoader.Logger.LogDebug("Setting up skybox coloring...");
                    //Resources.blankGradient = new Texture2D(16, 16);
                    //UnityEngine.ImageConversion.LoadImage(Resources.blankGradient, Convert.FromBase64String(B64Textures.Gradient), false);
                    loadingBackground = quickMenu.transform.Find("Popups/LoadingPopup/3DElements/LoadingBackground_TealGradient/SkyCube_Baked").gameObject;
                    loadingBackground.GetComponent<MeshRenderer>().material.SetTexture("_Tex", Functions.Core.Resources.blankGradient);
                    loadingBackground.GetComponent<MeshRenderer>().material.SetColor("_Tint", new Color(color.r / 2f, color.g / 2f, color.b / 2f, color.a));
                    loadingBackground.GetComponent<MeshRenderer>().material.SetTexture("_Tex", Functions.Core.Resources.blankGradient);
                    setupSkybox = true;

                    initialLoadingBackground = GameObject.Find("LoadingBackground_TealGradient_Music/SkyCube_Baked");
                    initialLoadingBackground.GetComponent<MeshRenderer>().material.SetTexture("_Tex", Functions.Core.Resources.blankGradient);
                    initialLoadingBackground.GetComponent<MeshRenderer>().material.SetColor("_Tint", new Color(color.r / 2f, color.g / 2f, color.b / 2f, color.a));
                    initialLoadingBackground.GetComponent<MeshRenderer>().material.SetTexture("_Tex", Functions.Core.Resources.blankGradient);
                }
                catch (Exception ex)
                {
                    emmVRCLoader.Logger.LogError(ex.ToString());
                }
            }
            if (setupSkybox && loadingBackground != null && !Functions.Core.ModCompatibility.BetterLoadingScreen)
            {
                emmVRCLoader.Logger.LogDebug("Coloring skybox...");
                loadingBackground.GetComponent<MeshRenderer>().material.SetColor("_Tint", new Color(color.r / 2f, color.g / 2f, color.b / 2f, color.a));
            }
            ColorBlock buttonTheme = new ColorBlock()
            {
                colorMultiplier = 1f,
                disabledColor = Color.grey,
                highlightedColor = color * 1.5f,
                normalColor = color / 1.5f,
                pressedColor = new Color(1f, 1f, 1f, 1f),
                fadeDuration = 0.1f,
                selectedColor = color / 1.5f
            };
            color.a = 0.9f;
            foreach (HighlightsFXStandalone highlights in UnityEngine.Resources.FindObjectsOfTypeAll<HighlightsFXStandalone>())
            {
                if (highlights != null && collectingColours)
                {
                    originalColours.Add(highlights, highlights.highlightColor);
                }
                if (Configuration.JSONConfig.UIColorChangingEnabled)
                {
                    highlights.highlightColor = color;
                }
                else if (!Configuration.JSONConfig.UIColorChangingEnabled && originalColours != null)
                {
                    highlights.highlightColor = originalColours.FirstOrDefault(a => a.Key.GetType() == typeof(HighlightsFXStandalone)).Value;
                }
            }
            try
            {
                GameObject HudVoiceIndicator = UnityEngine.Resources.FindObjectsOfTypeAll<FadeCycleEffect>().First(a => a.gameObject.name == "VoiceDotDisabled").transform.parent.gameObject;
                if (Configuration.JSONConfig.UIMicIconColorChangingEnabled && Configuration.JSONConfig.UIColorChangingEnabled)
                {
                    emmVRCLoader.Logger.LogDebug("Coloring Push To Talk icons...");
                    foreach (Image img in HudVoiceIndicator.transform.GetComponentsInChildren<Image>())
                        if (img.gameObject.name != "PushToTalkKeybd" && img.gameObject.name != "PushToTalkXbox")
                            img.color = color;
                }
                else
                {
                    emmVRCLoader.Logger.LogDebug("Decoloring Push To Talk icons...");
                    foreach (Image img in HudVoiceIndicator.transform.GetComponentsInChildren<Image>())
                        if (img.gameObject.name != "PushToTalkKeybd" && img.gameObject.name != "PushToTalkXbox")
                            img.color = Color.red;
                }
                HudVoiceIndicator.transform.Find("VoiceDotDisabled").GetComponent<FadeCycleEffect>().enabled = Configuration.JSONConfig.UIMicIconPulsingEnabled;
            }
            catch (Exception ex)
            {
                ex = new Exception();
            }

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
            //SpriteRenderer cursorRenderer = UnityEngine.Resources.FindObjectsOfTypeAll<VRC.UI.CursorIcon>().FirstOrDefault().field_Public_SpriteRenderer_0;
            //if (collectingColours)
            //    originalColours.Add(cursorRenderer, cursorRenderer.color);
            //if (Configuration.JSONConfig.UIColorChangingEnabled)
            //    cursorRenderer.color = color;
            //else if (!Configuration.JSONConfig.UIColorChangingEnabled && !collectingColours)
            //    cursorRenderer.color = originalColours[cursorRenderer];

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

                color.a = .5f;
                darker.a = 1f;
                theme.normalColor = darker;
                foreach (UnityEngine.UI.Slider sldr in quickMenu.GetComponentsInChildren<UnityEngine.UI.Slider>(true))
                {
                    if (collectingColours)
                        originalColourBlocks.Add(sldr, sldr.colors);
                    if (Configuration.JSONConfig.UIColorChangingEnabled)
                        sldr.colors = theme;
                    else if (!Configuration.JSONConfig.UIColorChangingEnabled && !collectingColours)
                        sldr.colors = originalColourBlocks.FirstOrDefault(a => a.Key != null && a.Key == sldr).Value;
                }
                darker.a = .5f;
                theme.normalColor = color;
                foreach (Button btn in quickMenu.GetComponentsInChildren<Button>(true))
                {
                    if (btn.gameObject.GetComponentsInChildren<Transform>(true).Any(a => a.name == "emmVRCDoNotColor") || btn.name != "SubscribeToAddPhotosButton" && btn.name != "SupporterButton" && btn.name != "ModerateButton" && btn.transform.parent.name != "VRC+PageTab" && (btn.name != "ReportButton" || btn.transform.parent.name.Contains("WorldInfo")))
                    {
                        if (collectingColours)
                            originalColourBlocks.Add(btn, btn.colors);
                        if (Configuration.JSONConfig.UIColorChangingEnabled)
                            btn.colors = buttonTheme;
                        else if (!Configuration.JSONConfig.UIColorChangingEnabled && !collectingColours)
                            btn.colors = originalColourBlocks.FirstOrDefault(a => a.Key != null && a.Key == btn).Value;
                    }
                }


            }
            catch (Exception ex)
            {
                ex = new Exception();
            }

            if (Configuration.JSONConfig.UIActionMenuColorChangingEnabled)
                try
                {
                    emmVRCLoader.Logger.LogDebug("Coloring Action Menu...");
                    Color referenceColor = (Configuration.JSONConfig.UIColorChangingEnabled ? Configuration.menuColor() : new Color(Configuration.defaultMenuColor().r * 1.5f, Configuration.defaultMenuColor().g * 1.5f, Configuration.defaultMenuColor().b * 1.5f));
                    Color transparent = new Color(referenceColor.r, referenceColor.g, referenceColor.b, referenceColor.a / 1.25f);
                    foreach (PedalGraphic grph in UnityEngine.Resources.FindObjectsOfTypeAll<PedalGraphic>())
                    {
                        if (grph.gameObject.name != "Center")
                        {
                            //grph.material.SetColor("_Color", Color.white);
                            if (grph._texture != null)
                                grph._texture = (grph._texture.ToTexture2D().Desaturate().ToTexture());
                            grph.color = referenceColor;
                        }
                        //grph.CrossFadeColor(Color.white, 0f, false, false);

                    }
                    foreach (ActionMenu menu in UnityEngine.Resources.FindObjectsOfTypeAll<ActionMenu>())
                    {
                        Image baseImage = menu.transform.Find("Main/Cursor").GetComponentInChildren<Image>();
                        if (baseImage == null) return;
                        baseImage.sprite = baseImage.sprite.ReplaceTexture(baseImage.sprite.UnpackTexture().Desaturate());
                        baseImage.color = transparent;
                        //menu.cursor.GetComponentInChildren<Image>().color = transparent;
                    }
                }
                catch (Exception ex)
                {
                    emmVRCLoader.Logger.LogError(ex.ToString());
                }


            collectingColours = false;
            //try
            //{
            //    foreach (Image img in ButtonAPI.GetQuickMenuInstance().GetComponentsInChildren<Image>(true))
            //    {
            //        if (img == null || img.sprite == null || img.sprite.texture == null) return;
            //        img.sprite = img.sprite.ReplaceTexture(img.sprite.UnpackTexture().Desaturate());
            //        img.color = color;
            //    }
            //} catch (Exception ex)
            //{
            //    emmVRCLoader.Logger.LogError(ex.ToString());
            //}
        }

    }
}
