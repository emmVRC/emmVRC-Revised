﻿using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using emmVRC.Objects.ModuleBases;
using VRC.UI;
using VRC.UI.Core;
using VRC.UI.Elements;
using VRC.UI.Elements.Controls;
using VRC.UI.Elements.Tooltips;

namespace emmVRC.Utils
{
    public  class ButtonAPI : MelonLoaderEvents
    {
        internal static GameObject singleButtonBase;
        internal static GameObject textButtonBase;
        internal static GameObject toggleButtonBase;
        internal static GameObject buttonGroupBase;
        internal static GameObject buttonGroupHeaderBase;
        internal static GameObject menuPageBase;
        internal static GameObject menuTabBase;
        internal static GameObject sliderBase;
        internal static Sprite onIconSprite;
        internal static VRC.UI.Elements.QuickMenu GetQuickMenuInstance()
        {
            return UnityEngine.Resources.FindObjectsOfTypeAll<VRC.UI.Elements.QuickMenu>().FirstOrDefault();
        }
        internal static MenuStateController GetMenuStateControllerInstance()
        {
            return UnityEngine.Resources.FindObjectsOfTypeAll<VRC.UI.Elements.QuickMenu>().FirstOrDefault().gameObject.GetComponent<MenuStateController>();
        }
        public override void OnUiManagerInit()
        {
            MelonLoader.MelonCoroutines.Start(WaitForQMClone());  
        }
        public static IEnumerator WaitForQMClone()
        {
            while (GameObject.Find("UserInterface").transform.Find("Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_Dashboard/ScrollRect/Viewport/VerticalLayoutGroup/Buttons_QuickActions/") == null)
                yield return new WaitForEndOfFrame();
            emmVRCLoader.Logger.LogDebug("Found the stuff!");
            singleButtonBase = GameObject.Find("UserInterface").transform.Find("Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_Dashboard/ScrollRect/Viewport/VerticalLayoutGroup/Buttons_QuickActions/Button_Respawn").gameObject;
            textButtonBase = GameObject.Find("UserInterface").transform.Find("Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_Settings/Panel_QM_ScrollRect/Viewport/VerticalLayoutGroup/Buttons_Debug/Button_Build").gameObject;
            toggleButtonBase = GameObject.Find("UserInterface").transform.Find("Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_Settings/Panel_QM_ScrollRect/Viewport/VerticalLayoutGroup/Buttons_UI_Elements_Row_1/Button_ToggleQMInfo").gameObject;
            buttonGroupBase = GameObject.Find("UserInterface").transform.Find("Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_Dashboard/ScrollRect/Viewport/VerticalLayoutGroup/Buttons_QuickActions").gameObject;
            buttonGroupHeaderBase = GameObject.Find("UserInterface").transform.Find("Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_Dashboard/ScrollRect/Viewport/VerticalLayoutGroup/Header_QuickActions").gameObject;
            menuPageBase = GameObject.Find("UserInterface").transform.Find("Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_Dashboard").gameObject;
            menuTabBase = GameObject.Find("UserInterface").transform.Find("Canvas_QuickMenu(Clone)/Container/Window/Page_Buttons_QM/HorizontalLayoutGroup/Page_Settings").gameObject;
            sliderBase = GameObject.Find("UserInterface").transform.Find("Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_AudioSettings/Content/Audio/VolumeSlider_Master").gameObject;

            onIconSprite = GameObject.Find("UserInterface").transform.Find("Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_Notifications/Panel_NoNotifications_Message/Icon").GetComponent<Image>().sprite;
            emmVRCLoader.Logger.LogDebug("Sprite name: " + onIconSprite.name);
            GameObject.Find("UserInterface").transform.Find("Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_Dashboard/ScrollRect").GetComponent<ScrollRect>().enabled = true;
        }
    }
    public class SingleButton
    {
        private readonly TextMeshProUGUI buttonText;
        private readonly Image buttonImage;
        private readonly Button buttonButton; // lol
        private readonly VRC.UI.Elements.Tooltips.UiTooltip buttonTooltip;
        public readonly GameObject gameObject;
        
        public SingleButton(Transform parent, string text, Action click, string tooltip, Sprite icon = null)
        {
            gameObject = GameObject.Instantiate(ButtonAPI.singleButtonBase, parent);
            buttonText = gameObject.GetComponentInChildren<TextMeshProUGUI>(true);
            buttonText.text = text;
            buttonButton = gameObject.GetComponentInChildren<Button>(true);
            buttonButton.onClick = new Button.ButtonClickedEvent();
            buttonButton.onClick.AddListener(click);
            buttonTooltip = gameObject.GetComponentInChildren<VRC.UI.Elements.Tooltips.UiTooltip>(true);
            buttonTooltip.text = tooltip;
            buttonImage = gameObject.transform.Find("Icon").GetComponentInChildren<Image>(true);
            if (icon != null)
            {
                buttonImage.sprite = icon;
                buttonImage.overrideSprite = icon;
                buttonImage.gameObject.SetActive(true);
            }
            else
                buttonImage.gameObject.SetActive(false);
        }
        public SingleButton(MenuPage pge, string text, Action click, string tooltip, Sprite icon = null) : this(pge.menuContents, text, click, tooltip, icon) { }
        public SingleButton(ButtonGroup grp, string text, Action click, string tooltip, Sprite icon = null) : this(grp.gameObject.transform, text, click, tooltip, icon) { }
        public void SetAction(Action newAction)
        {
            buttonButton.onClick = new Button.ButtonClickedEvent();
            buttonButton.onClick.AddListener(newAction);
        }
        public void SetText(string newText)
        {
            buttonText.text = newText;
        }
        public void SetTooltip(string newTooltip)
        {
            buttonTooltip.text = newTooltip;
        }
        public void SetIcon(Sprite newIcon)
        {
            if (newIcon == null)
                buttonImage.gameObject.SetActive(false);
            else
            {
                buttonImage.sprite = newIcon;
                buttonImage.overrideSprite = newIcon;
                buttonImage.gameObject.SetActive(true);
            }
        }
        public void SetInteractable(bool val)
        {
            buttonButton.interactable = val;
        }
    }
    public class SimpleSingleButton
    {
        private readonly TextMeshProUGUI buttonText;
        private readonly Button buttonButton; // lol
        private readonly VRC.UI.Elements.Tooltips.UiTooltip buttonTooltip;
        public readonly GameObject gameObject;

        public SimpleSingleButton(Transform parent, string text, Action click, string tooltip)
        {
            gameObject = GameObject.Instantiate(ButtonAPI.singleButtonBase, parent);
            buttonText = gameObject.GetComponentInChildren<TextMeshProUGUI>(true);
            buttonText.text = text;
            buttonText.fontSize = 28;
            buttonText.gameObject.GetComponent<RectTransform>().localPosition = new Vector3(0f, -25f, 0f);
            buttonButton = gameObject.GetComponentInChildren<Button>(true);
            buttonButton.onClick = new Button.ButtonClickedEvent();
            buttonButton.onClick.AddListener(click);
            buttonTooltip = gameObject.GetComponentInChildren<VRC.UI.Elements.Tooltips.UiTooltip>(true);
            buttonTooltip.text = tooltip;
            GameObject.Destroy(gameObject.transform.Find("Icon").gameObject);
            GameObject.Destroy(gameObject.transform.Find("Icon_Secondary").gameObject);
            buttonText.color = new Color(.9f, .9f, .9f);
        }
        public SimpleSingleButton(MenuPage pge, string text, Action click, string tooltip) : this(pge.menuContents, text, click, tooltip) { }
        public SimpleSingleButton(ButtonGroup grp, string text, Action click, string tooltip) : this(grp.gameObject.transform, text, click, tooltip) { }
        public void SetAction(Action newAction)
        {
            buttonButton.onClick = new Button.ButtonClickedEvent();
            buttonButton.onClick.AddListener(newAction);
        }
        public void SetText(string newText)
        {
            buttonText.text = newText;
        }
        public void SetTooltip(string newTooltip)
        {
            buttonTooltip.text = newTooltip;
        }
        public void SetInteractable(bool val)
        {
            buttonButton.interactable = val;
        }
    }
    public class TextButton
    {
        private readonly TextMeshProUGUI buttonText;
        private readonly TextMeshProUGUI buttonTextBig;
        private readonly Button buttonButton; // lol
        private readonly VRC.UI.Elements.Tooltips.UiTooltip buttonTooltip;
        public readonly GameObject gameObject;

        public TextButton(Transform parent, string text, Action click, string tooltip, string bigText)
        {
            gameObject = GameObject.Instantiate(ButtonAPI.singleButtonBase, parent);
            buttonText = gameObject.GetComponentsInChildren<TextMeshProUGUI>(true).First();
            buttonText.text = text;
            buttonTextBig = gameObject.GetComponentsInChildren<TextMeshProUGUI>(true).Last();
            buttonTextBig.text = bigText;
            buttonButton = gameObject.GetComponentInChildren<Button>(true);
            buttonButton.onClick = new Button.ButtonClickedEvent();
            buttonButton.onClick.AddListener(click);
            buttonTooltip = gameObject.GetComponentInChildren<VRC.UI.Elements.Tooltips.UiTooltip>(true);
            buttonTooltip.text = tooltip;
        }
        public TextButton(MenuPage pge, string text, Action click, string tooltip, string bigText) : this(pge.menuContents, text, click, tooltip, bigText) { }
        public TextButton(ButtonGroup grp, string text, Action click, string tooltip, string bigText) : this(grp.gameObject.transform, text, click, tooltip, bigText) { }
        public void SetAction(Action newAction)
        {
            buttonButton.onClick = new Button.ButtonClickedEvent();
            buttonButton.onClick.AddListener(newAction);
        }
        public void SetText(string newText)
        {
            buttonText.text = newText;
        }
        public void SetTooltip(string newTooltip)
        {
            buttonTooltip.text = newTooltip;
        }
        public void SetBigText(string newText)
        {
            buttonTextBig.text = newText;
        }
        public void SetInteractable(bool val)
        {
            buttonButton.interactable = val;
        }
    }

    public class ToggleButton
    {
        private readonly TextMeshProUGUI buttonText;
        private readonly Image buttonImage;
        private readonly Toggle buttonToggle;
        private readonly VRC.UI.Elements.Tooltips.UiToggleTooltip toggleTooltip;
        public readonly GameObject gameObject;

        public ToggleButton(Transform parent, string text, Action<bool> stateChanged, string offTooltip, string onTooltip, Sprite icon = null)
        {
            gameObject = GameObject.Instantiate(ButtonAPI.toggleButtonBase, parent);
            buttonText = gameObject.GetComponentInChildren<TextMeshProUGUI>(true);
            buttonText.text = text;
            buttonToggle = gameObject.GetComponentInChildren<Toggle>(true);
            buttonToggle.onValueChanged = new Toggle.ToggleEvent();
            buttonToggle.isOn = false;
            buttonToggle.onValueChanged.AddListener(new System.Action<bool>((bool val) => {
                stateChanged.Invoke(val);
            }));
            toggleTooltip = gameObject.GetComponentInChildren<VRC.UI.Elements.Tooltips.UiToggleTooltip>(true);
            toggleTooltip.text = onTooltip;
            toggleTooltip.alternateText = offTooltip;
            toggleTooltip.displayAlternateTooltip = true;
            buttonImage = gameObject.transform.Find("Icon_On").GetComponentInChildren<Image>(true);
            if (icon != null)
            {
                buttonImage.sprite = icon;
                buttonImage.overrideSprite = icon;
            }
            else
            {
                buttonImage.sprite = ButtonAPI.onIconSprite;
                buttonImage.overrideSprite = ButtonAPI.onIconSprite;
            }
        }
        public ToggleButton(MenuPage pge, string text, Action<bool> stateChanged, string offTooltip, string onTooltip, Sprite icon = null) : this(pge.menuContents, text, stateChanged, offTooltip, onTooltip, icon) { }
        public ToggleButton(ButtonGroup grp, string text, Action<bool> stateChanged, string offTooltip, string onTooltip, Sprite icon = null) : this(grp.gameObject.transform, text, stateChanged, offTooltip, onTooltip, icon) { }
        public void SetAction(Action<bool> newAction)
        {
            buttonToggle.onValueChanged = new Toggle.ToggleEvent();
            buttonToggle.onValueChanged.AddListener(new System.Action<bool>((bool val) => {
                newAction.Invoke(val);
            }));
        }
        public void SetText(string newText)
        {
            buttonText.text = newText;
        }
        public void SetTooltip(string newOffTooltip, string newOnTooltip)
        {
            toggleTooltip.text = newOnTooltip;
            toggleTooltip.alternateText = newOffTooltip;
        }
        public void SetIcon(Sprite newIcon)
        {
            if (newIcon == null)
                buttonImage.gameObject.SetActive(false);
            else
            {
                buttonImage.sprite = newIcon;
                buttonImage.overrideSprite = newIcon;
                buttonImage.gameObject.SetActive(true);
            }
        }
        public void SetToggleState(bool newState, bool invoke = false)
        {
            Toggle.ToggleEvent evt = buttonToggle.onValueChanged;
            buttonToggle.onValueChanged = new Toggle.ToggleEvent();
            buttonToggle.isOn = newState;
            buttonToggle.onValueChanged = evt;
            buttonToggle.GetComponent<ToggleIcon>().OnValueChanged(newState);
            toggleTooltip.displayAlternateTooltip = !newState;
            if (invoke)
                buttonToggle.onValueChanged.Invoke(newState);
        }
        public void SetInteractable(bool val)
        {
            buttonToggle.interactable = val;
        }
    }

    public class Slider
    {
        private readonly TextMeshProUGUI sliderText;
        private readonly TextMeshProUGUI sliderPercentText;
        private readonly UnityEngine.UI.Slider sliderSlider; // lmao
        private readonly VRC.UI.Elements.Tooltips.UiTooltip sliderTooltip;
        private bool _floor = false;
        private bool _percent = false;
        public readonly GameObject gameObject;

        public Slider(Transform parent, string text, Action<float> onSliderAdjust, string tooltip, float maxValue = 100f, float defaultValue = 50f, bool floor = false, bool percent = true)
        {
            gameObject = GameObject.Instantiate(ButtonAPI.sliderBase, parent);
            sliderText = gameObject.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>(true);
            sliderText.text = text;
            sliderPercentText = gameObject.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>(true);
            sliderPercentText.text = "0"+(percent?"%":"");
            sliderSlider = gameObject.GetComponentInChildren<UnityEngine.UI.Slider>();
            sliderSlider.onValueChanged = new UnityEngine.UI.Slider.SliderEvent();
            sliderSlider.maxValue = maxValue;
            sliderSlider.value = defaultValue;
            sliderSlider.onValueChanged.AddListener(new System.Action<float>((float val) => {
                sliderPercentText.text = (floor ? Mathf.Floor(val) : val) + (percent ? "%" : "");
                onSliderAdjust.Invoke(val);
            }));
            sliderTooltip = gameObject.GetComponentInChildren<VRC.UI.Elements.Tooltips.UiTooltip>(true);
            sliderTooltip.text = tooltip;
            _floor = floor;
            _percent = percent;
        }
        public Slider(MenuPage pge, string text, Action<float> onSliderAdjust, string tooltip, float maxValue = 100f, float defaultValue = 50f, bool floor = false, bool percent = true) : this(pge.menuContents, text, onSliderAdjust, tooltip, maxValue, defaultValue, floor, percent) { }
        public Slider(ButtonGroup grp, string text, Action<float> onSliderAdjust, string tooltip, float maxValue = 100f, float defaultValue = 50f, bool floor = false, bool percent = true) : this(grp.gameObject.transform, text, onSliderAdjust, tooltip, maxValue, defaultValue, floor, percent) { }
        public void SetAction(Action<float> newAction)
        {
            sliderSlider.onValueChanged = new UnityEngine.UI.Slider.SliderEvent();
            sliderSlider.onValueChanged.AddListener(new System.Action<float>((float val) => {
                sliderPercentText.text = (_floor ? Mathf.Floor(val) : val) + (_percent ? "%" : "");
                newAction.Invoke(val);
            }));
        }
        public void SetText(string newText)
        {
            sliderText.text = newText;
        }
        public void SetTooltip(string newTooltip)
        {
            sliderTooltip.text = newTooltip;
        }
        public void SetInteractable(bool val)
        {
            sliderSlider.interactable = val;
        }
    }
    public class ButtonGroup
    {
        private readonly TextMeshProUGUI headerText;
        public readonly GameObject gameObject;
        private readonly GameObject headerGameObject;
        public ButtonGroup(Transform parent, string text)
        {
            headerGameObject = GameObject.Instantiate(ButtonAPI.buttonGroupHeaderBase, parent);
            headerText = headerGameObject.GetComponentInChildren<TextMeshProUGUI>(true);
            headerText.text = text;
            gameObject = GameObject.Instantiate(ButtonAPI.buttonGroupBase, parent);
            gameObject.transform.DestroyChildren();
        }
        public ButtonGroup(MenuPage pge, string text) : this(pge.menuContents, text) { }
        public void SetText(string newText)
        {
            headerText.text = newText;
        }
        public void Destroy()
        {
            GameObject.Destroy(headerText.gameObject);
            GameObject.Destroy(gameObject);
        }
    }
    public class MenuPage
    {
        private readonly UIPage page;
        private readonly GameObject gameObject;
        public readonly Transform menuContents;
        private readonly TextMeshProUGUI pageTitleText;
        private readonly bool isRoot;
        private readonly GameObject backButtonGameObject;
        private readonly GameObject extButtonGameObject;
        public MenuPage(string menuName, string pageTitle, bool root = true, bool backButton = true, bool extButton = false, Action extButtonAction = null)
        {
            gameObject = GameObject.Instantiate(ButtonAPI.menuPageBase, ButtonAPI.menuPageBase.transform.parent);
            gameObject.transform.SetSiblingIndex(5);
            gameObject.SetActive(false);
            GameObject.DestroyImmediate(gameObject.GetComponent<VRC.UI.Elements.Menus.LaunchPadQMMenu>());
            page = gameObject.AddComponent<UIPage>();
            page.Name = menuName;
            page._inited = true;
            page._menuStateController = ButtonAPI.GetMenuStateControllerInstance();
            page._pageStack = new Il2CppSystem.Collections.Generic.List<UIPage>();
            page._pageStack.Add(page);
            ButtonAPI.GetMenuStateControllerInstance()._uiPages.Add(menuName, page);
            if (root)
            {
                List<UIPage> menuRootPages = ButtonAPI.GetMenuStateControllerInstance().menuRootPages.ToList();
                menuRootPages.Add(page);
                ButtonAPI.GetMenuStateControllerInstance().menuRootPages = menuRootPages.ToArray();
            }
            gameObject.transform.Find("ScrollRect/Viewport/VerticalLayoutGroup").DestroyChildren();
            menuContents = gameObject.transform.Find("ScrollRect/Viewport/VerticalLayoutGroup");
            pageTitleText = gameObject.GetComponentInChildren<TextMeshProUGUI>(true);
            pageTitleText.text = pageTitle;
            isRoot = root;
            backButtonGameObject = gameObject.transform.GetChild(0).Find("LeftItemContainer/Button_Back").gameObject;
            extButtonGameObject = gameObject.transform.GetChild(0).Find("RightItemContainer/Button_QM_Expand").gameObject;
            backButtonGameObject.SetActive(backButton);
            backButtonGameObject.GetComponentInChildren<Button>().onClick = new Button.ButtonClickedEvent();
            backButtonGameObject.GetComponentInChildren<Button>().onClick.AddListener(new System.Action(() => {
                if (isRoot)
                    ButtonAPI.GetMenuStateControllerInstance().SwitchToRootPage("QuickMenuDashboard");
                else
                    ButtonAPI.GetMenuStateControllerInstance().PopPage();
            }));
            extButtonGameObject.SetActive(extButton);
            emmVRCLoader.Logger.LogDebug("GameObject is " + gameObject == null ? "null" : "not null"); emmVRCLoader.Logger.LogDebug("Transform is " + menuContents == null ? "null" : "not null");

        }
        public void OpenMenu()
        {
            if (isRoot)
                ButtonAPI.GetMenuStateControllerInstance().SwitchToRootPage(page.Name);
            else
                ButtonAPI.GetMenuStateControllerInstance().PushPage(page.Name);
            //UnityEngine.Resources.FindObjectsOfTypeAll<MenuStateController>().First().PushPage(pageTitleText.text);
            //UnityEngine.Resources.FindObjectsOfTypeAll<MenuStateController>().First()._currentRootPage.PushPage(page);
        }
    }
    public class Tab
    {
        private readonly GameObject gameObject;
        private readonly MenuTab menuTab;
        private readonly Image tabIcon;
        public Tab(Transform parent, string menuName, string tooltip, Sprite icon = null)
        {
            gameObject = GameObject.Instantiate(ButtonAPI.menuTabBase, parent);
            menuTab = gameObject.GetComponent<MenuTab>();
            menuTab._menuStateController = ButtonAPI.GetMenuStateControllerInstance();
            menuTab.pageName = menuName;
            tabIcon = gameObject.transform.Find("Icon").GetComponent<Image>();
            emmVRCLoader.Logger.LogDebug(tabIcon.gameObject.GetPath());
            emmVRCLoader.Logger.LogDebug("Icon is " + (icon == null ? "null" : "not null"));
            tabIcon.sprite = icon;
            tabIcon.overrideSprite = icon;

            menuTab.GetComponent<VRC.UI.Core.Styles.StyleElement>().selectable = menuTab.GetComponent<Button>();
            menuTab.GetComponent<Button>().onClick.AddListener(new System.Action(() => {
                menuTab.GetComponent<VRC.UI.Core.Styles.StyleElement>().selectable = menuTab.GetComponent<Button>();
            }));
        }
    }
}
