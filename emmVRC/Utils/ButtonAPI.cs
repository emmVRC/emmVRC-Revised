using System;
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
        internal static GameObject wingPageBase;
        internal static GameObject wingSingleButtonBase;
        internal static GameObject wingPageBaseL;
        internal static GameObject wingPageBaseR;
        internal static Sprite onIconSprite;
        internal static Sprite plusIconSprite;
        internal static Sprite xIconSprite;
        internal static Sprite trashIconSprite;

        internal static VRC.UI.Elements.QuickMenu GetQuickMenuInstance()
        {
            return UnityEngine.Resources.FindObjectsOfTypeAll<VRC.UI.Elements.QuickMenu>().FirstOrDefault();
        }
        internal static MenuStateController GetMenuStateControllerInstance()
        {
            return UnityEngine.Resources.FindObjectsOfTypeAll<VRC.UI.Elements.QuickMenu>().FirstOrDefault().gameObject.GetComponent<MenuStateController>();
        }
        internal static MenuStateController GetLeftWingControllerInstance()
        {
            return UnityEngine.Resources.FindObjectsOfTypeAll<MenuStateController>().FirstOrDefault(a => a.gameObject.name == "Wing_Left");
        }
        internal static MenuStateController GetRightWingControllerInstance()
        {
            return UnityEngine.Resources.FindObjectsOfTypeAll<MenuStateController>().FirstOrDefault(a => a.gameObject.name == "Wing_Right");
        }
        public override void OnUiManagerInit()
        {
            MelonLoader.MelonCoroutines.Start(WaitForQMClone());  
        }
        public static IEnumerator WaitForQMClone()
        {
            while (GameObject.Find("UserInterface").transform.Find("Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_Dashboard/ScrollRect/Viewport/VerticalLayoutGroup/Buttons_QuickActions/") == null)
                yield return new WaitForEndOfFrame();
            singleButtonBase = GameObject.Find("UserInterface").transform.Find("Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_Dashboard/ScrollRect/Viewport/VerticalLayoutGroup/Buttons_QuickActions/Button_Respawn").gameObject;
            for (int i = 0; i < singleButtonBase.GetComponentsInChildren<VRC.UI.Elements.Tooltips.UiTooltip>().Count; i++)
            {
                if (singleButtonBase.GetComponentsInChildren<VRC.UI.Elements.Tooltips.UiTooltip>()[i].field_Public_String_0 == "" && singleButtonBase.GetComponentsInChildren<VRC.UI.Elements.Tooltips.UiTooltip>()[i].field_Public_String_1 == "")
                    GameObject.DestroyImmediate(singleButtonBase.GetComponentsInChildren<VRC.UI.Elements.Tooltips.UiTooltip>()[i]);
            };
            textButtonBase = GameObject.Find("UserInterface").transform.Find("Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_Settings/Panel_QM_ScrollRect/Viewport/VerticalLayoutGroup/Buttons_Debug/Button_Build").gameObject;
            toggleButtonBase = GameObject.Find("UserInterface").transform.Find("Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_Settings/Panel_QM_ScrollRect/Viewport/VerticalLayoutGroup/Buttons_UI_Elements_Row_1/Button_ToggleQMInfo").gameObject;
            buttonGroupBase = GameObject.Find("UserInterface").transform.Find("Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_Dashboard/ScrollRect/Viewport/VerticalLayoutGroup/Buttons_QuickActions").gameObject;
            buttonGroupHeaderBase = GameObject.Find("UserInterface").transform.Find("Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_Dashboard/ScrollRect/Viewport/VerticalLayoutGroup/Header_QuickActions").gameObject;
            menuPageBase = GameObject.Find("UserInterface").transform.Find("Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_Dashboard").gameObject;
            menuTabBase = GameObject.Find("UserInterface").transform.Find("Canvas_QuickMenu(Clone)/Container/Window/Page_Buttons_QM/HorizontalLayoutGroup/Page_Settings").gameObject;
            sliderBase = GameObject.Find("UserInterface").transform.Find("Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_AudioSettings/Content/Audio/VolumeSlider_Master").gameObject;
            wingPageBase = GameObject.Find("UserInterface").transform.Find("Canvas_QuickMenu(Clone)/Container/Window/Wing_Left/Container/InnerContainer/Explore").gameObject;
            wingSingleButtonBase = GameObject.Find("UserInterface").transform.Find("Canvas_QuickMenu(Clone)/Container/Window/Wing_Left/Container/InnerContainer/WingMenu/ScrollRect/Viewport/VerticalLayoutGroup/Button_Profile").gameObject;
            
            wingPageBaseL = GameObject.Find("UserInterface").transform.Find("Canvas_QuickMenu(Clone)/Container/Window/Wing_Left/Container/InnerContainer").gameObject;
            wingPageBaseR = GameObject.Find("UserInterface").transform.Find("Canvas_QuickMenu(Clone)/Container/Window/Wing_Right/Container/InnerContainer").gameObject;

            onIconSprite = GameObject.Find("UserInterface").transform.Find("Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_Notifications/Panel_NoNotifications_Message/Icon").GetComponent<Image>().sprite;
            plusIconSprite = GameObject.Find("UserInterface").transform.Find("Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_Here/ScrollRect/Viewport/VerticalLayoutGroup/Buttons_WorldActions/Button_FavoriteWorld/Icon").GetComponent<Image>().sprite;
            xIconSprite = toggleButtonBase.transform.Find("Icon_Off").GetComponent<Image>().sprite;
            trashIconSprite = GameObject.Find("UserInterface").transform.Find("Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_Here/ScrollRect/Viewport/VerticalLayoutGroup/Buttons_WorldActions/Button_FavoriteWorld/Icon_Secondary").GetComponent<Image>().sprite;
        }
    }
    public class SingleButton
    {
        private readonly TextMeshProUGUI buttonText;
        private readonly Image buttonImage;
        private readonly Button buttonButton; // lol
        private readonly Image buttonBadge;
        private readonly VRC.UI.Elements.Tooltips.UiTooltip buttonTooltip;
        public readonly GameObject gameObject;
        
        public SingleButton(Transform parent, string text, Action click, string tooltip, Sprite icon = null, bool preserveColor = false)
        {
            gameObject = GameObject.Instantiate(ButtonAPI.singleButtonBase, parent);
            buttonText = gameObject.GetComponentInChildren<TextMeshProUGUI>(true);
            buttonText.text = text;
            buttonButton = gameObject.GetComponentInChildren<Button>(true);
            buttonButton.onClick = new Button.ButtonClickedEvent();
            buttonButton.onClick.AddListener(click);
            buttonTooltip = gameObject.GetComponentInChildren<VRC.UI.Elements.Tooltips.UiTooltip>(true);
            buttonTooltip.field_Public_String_0 = tooltip;
            buttonImage = gameObject.transform.Find("Icon").GetComponentInChildren<Image>(true);
            buttonBadge = gameObject.transform.Find("Badge_Close").GetComponentInChildren<Image>(true);
            if (icon != null)
            {
                buttonImage.sprite = icon;
                buttonImage.overrideSprite = icon;
                buttonImage.gameObject.SetActive(true);
                if (preserveColor)
                {
                    buttonImage.color = Color.white;
                    buttonImage.GetComponent<VRC.UI.Core.Styles.StyleElement>().enabled = false;
                }
            }
            else
                buttonImage.gameObject.SetActive(false);
        }
        public SingleButton(MenuPage pge, string text, Action click, string tooltip, Sprite icon = null, bool preserveColor = false) : this(pge.menuContents, text, click, tooltip, icon, preserveColor) { }
        public SingleButton(ButtonGroup grp, string text, Action click, string tooltip, Sprite icon = null, bool preserveColor = false) : this(grp.gameObject.transform, text, click, tooltip, icon, preserveColor) { }
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
            buttonTooltip.field_Public_String_0 = newTooltip;
        }
        public void SetIcon(Sprite newIcon, bool preserveColor = false)
        {
            if (newIcon == null)
                buttonImage.gameObject.SetActive(false);
            else
            {
                buttonImage.sprite = newIcon;
                buttonImage.overrideSprite = newIcon;
                buttonImage.gameObject.SetActive(true);
                if (preserveColor)
                {
                    buttonImage.color = Color.white;
                    buttonImage.GetComponent<VRC.UI.Core.Styles.StyleElement>().enabled = false;
                }
            }
            
        }
        public void SetBadgeIcon(Sprite newBadge)
        {
            buttonBadge.sprite = newBadge;
            buttonBadge.overrideSprite = newBadge;
        }
        public void SetBadgeEnabled(bool enabled)
        {
            buttonBadge.gameObject.SetActive(enabled);
        }
        public void SetIconColor(Color color)
        {
            buttonImage.color = color;
            buttonImage.GetComponent<VRC.UI.Core.Styles.StyleElement>().enabled = false;
        }
        public void SetInteractable(bool val)
        {
            buttonButton.interactable = val;
        }
        public void SetActive(bool state)
        {
            gameObject.SetActive(state);
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
            buttonTooltip.field_Public_String_0 = tooltip;
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
            buttonTooltip.field_Public_String_0 = newTooltip;
        }
        public void SetInteractable(bool val)
        {
            buttonButton.interactable = val;
        }
        public void SetActive(bool state)
        {
            gameObject.SetActive(state);
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
            buttonTooltip.field_Public_String_0 = tooltip;
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
            buttonTooltip.field_Public_String_0 = newTooltip;
        }
        public void SetBigText(string newText)
        {
            buttonTextBig.text = newText;
        }
        public void SetInteractable(bool val)
        {
            buttonButton.interactable = val;
        }
        public void SetActive(bool state)
        {
            gameObject.SetActive(state);
        }
    }

    public class ToggleButton
    {
        private readonly TextMeshProUGUI buttonText;
        private readonly Image buttonImage;
        private readonly Toggle buttonToggle;
        private readonly VRC.UI.Elements.Tooltips.UiToggleTooltip toggleTooltip;
        public readonly GameObject gameObject;
        private bool _stateInvoke = true;

        public ToggleButton(Transform parent, string text, Action<bool> stateChanged, string offTooltip, string onTooltip, Sprite icon = null)
        {
            gameObject = GameObject.Instantiate(ButtonAPI.toggleButtonBase, parent);
            buttonText = gameObject.GetComponentInChildren<TextMeshProUGUI>(true);
            buttonText.text = text;
            buttonToggle = gameObject.GetComponentInChildren<Toggle>(true);
            buttonToggle.onValueChanged = new Toggle.ToggleEvent();
            buttonToggle.isOn = false;
            buttonToggle.onValueChanged.AddListener(new System.Action<bool>((bool val) => {
                if (_stateInvoke)
                stateChanged.Invoke(val);
            }));
            toggleTooltip = gameObject.GetComponentInChildren<VRC.UI.Elements.Tooltips.UiToggleTooltip>(true);
            toggleTooltip.field_Public_String_0 = onTooltip;
            toggleTooltip.field_Public_String_1 = offTooltip;
            toggleTooltip.prop_Boolean_0 = true;
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
            toggleTooltip.field_Public_String_0 = newOnTooltip;
            toggleTooltip.field_Public_String_1 = newOffTooltip;
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
            _stateInvoke = false;
            buttonToggle.isOn = newState;
            buttonToggle.onValueChanged.Invoke(newState);
            _stateInvoke = true;
            if (invoke)
                buttonToggle.onValueChanged.Invoke(newState);
        }

        public void SetInteractable(bool val)
        {
            buttonToggle.interactable = val;
            buttonToggle.m_Interactable = val;
        }
        public void SetActive(bool state)
        {
            gameObject.SetActive(state);
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
            sliderTooltip.field_Public_String_0 = tooltip;
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
            sliderTooltip.field_Public_String_0 = newTooltip;
        }
        public void SetInteractable(bool val)
        {
            sliderSlider.interactable = val;
        }
        public void SetActive(bool state)
        {
            sliderSlider.gameObject.SetActive(state);
            sliderTooltip.gameObject.SetActive(state);
            sliderPercentText.gameObject.SetActive(state);
        }
        public void SetValue(float newValue, bool invoke = false)
        {
            UnityEngine.UI.Slider.SliderEvent evt = sliderSlider.onValueChanged;
            sliderSlider.onValueChanged = new UnityEngine.UI.Slider.SliderEvent();
            sliderSlider.value = newValue;
            sliderSlider.onValueChanged = evt;
            if (invoke)
                sliderSlider.onValueChanged.Invoke(newValue);
        }
    }
    public class ButtonGroup
    {
        private readonly TextMeshProUGUI headerText;
        public readonly GameObject gameObject;
        private readonly GameObject headerGameObject;
        public readonly RectMask2D parentMenuMask;
        public ButtonGroup(Transform parent, string text)
        {
            headerGameObject = GameObject.Instantiate(ButtonAPI.buttonGroupHeaderBase, parent);
            headerText = headerGameObject.GetComponentInChildren<TextMeshProUGUI>(true);
            headerText.text = text;
            gameObject = GameObject.Instantiate(ButtonAPI.buttonGroupBase, parent);
            gameObject.transform.DestroyChildren();
            parentMenuMask = parent.parent.GetComponent<RectMask2D>();
        }
        public ButtonGroup(MenuPage pge, string text) : this(pge.menuContents, text) { }
        public void SetText(string newText)
        {
            headerText.text = newText;
        }
        public void Destroy()
        {
            GameObject.Destroy(headerText.gameObject);
            GameObject.Destroy(headerGameObject);
            GameObject.Destroy(gameObject);
        }
        public void SetActive(bool state)
        {
            if (headerGameObject != null)
                headerGameObject.SetActive(state);
            gameObject.SetActive(state);
        }
    }
    public class SimpleButtonGroup
    {
        public readonly GameObject gameObject;
        public readonly RectMask2D parentMenuMask;
        public SimpleButtonGroup(Transform parent, string text)
        {
            gameObject = GameObject.Instantiate(ButtonAPI.buttonGroupBase, parent);
            gameObject.transform.DestroyChildren();
            parentMenuMask = parent.parent.GetComponent<RectMask2D>();
        }
        public SimpleButtonGroup(MenuPage pge, string text) : this(pge.menuContents, text) { }
        public void Destroy()
        {
            GameObject.Destroy(gameObject);
        }
        public void SetActive(bool state)
        {
            gameObject.SetActive(state);
        }
    }
    public class MenuPage
    {
        private readonly UIPage page;
        private readonly GameObject gameObject;
        public readonly Transform menuContents;
        private readonly TextMeshProUGUI pageTitleText;
        private readonly bool isRoot;
        public readonly GameObject backButtonGameObject;
        private readonly GameObject extButtonGameObject;
        private bool preserveColor;
        public readonly RectMask2D menuMask;
        public MenuPage(string menuName, string pageTitle, bool root = true, bool backButton = true, bool extButton = false, Action extButtonAction = null, string extButtonTooltip = "", Sprite extButtonSprite = null, bool preserveColor = false)
        {
            gameObject = GameObject.Instantiate(ButtonAPI.menuPageBase, ButtonAPI.menuPageBase.transform.parent);
            gameObject.name = "Menu_" + menuName;
            gameObject.transform.SetSiblingIndex(5);
            gameObject.SetActive(false);
            GameObject.DestroyImmediate(gameObject.GetComponent<VRC.UI.Elements.Menus.LaunchPadQMMenu>());
            page = gameObject.AddComponent<UIPage>();
            page.field_Public_String_0 = menuName;
            page.field_Private_Boolean_1 = true;
            page.field_Protected_MenuStateController_0 = ButtonAPI.GetMenuStateControllerInstance();
            page.field_Private_List_1_UIPage_0 = new Il2CppSystem.Collections.Generic.List<UIPage>();
            page.field_Private_List_1_UIPage_0.Add(page);
            ButtonAPI.GetMenuStateControllerInstance().field_Private_Dictionary_2_String_UIPage_0.Add(menuName, page);
            if (root)
            {
                List<UIPage> menuRootPages = ButtonAPI.GetMenuStateControllerInstance().field_Public_ArrayOf_UIPage_0.ToList();
                menuRootPages.Add(page);
                ButtonAPI.GetMenuStateControllerInstance().field_Public_ArrayOf_UIPage_0 = menuRootPages.ToArray();
            }
            gameObject.transform.Find("ScrollRect/Viewport/VerticalLayoutGroup").DestroyChildren();
            menuContents = gameObject.transform.Find("ScrollRect/Viewport/VerticalLayoutGroup");
            menuContents.GetComponent<UnityEngine.UI.VerticalLayoutGroup>().childControlHeight = false; // Overriding this in case other mods change this value, like ReMod
            pageTitleText = gameObject.GetComponentInChildren<TextMeshProUGUI>(true);
            pageTitleText.text = pageTitle;
            isRoot = root;
            backButtonGameObject = gameObject.transform.GetChild(0).Find("LeftItemContainer/Button_Back").gameObject;
            extButtonGameObject = gameObject.transform.GetChild(0).Find("RightItemContainer/Button_QM_Expand").gameObject;
            backButtonGameObject.SetActive(backButton);
            backButtonGameObject.GetComponentInChildren<Button>().onClick = new Button.ButtonClickedEvent();
            backButtonGameObject.GetComponentInChildren<Button>().onClick.AddListener(new System.Action(() => {
                //if (isRoot)
                //    ButtonAPI.GetMenuStateControllerInstance().Method_Public_Void_String_Boolean_0("QuickMenuDashboard");
                //else
                    this.page.Method_Protected_Virtual_New_Void_0();
            }));
            extButtonGameObject.SetActive(extButton);
            extButtonGameObject.GetComponentInChildren<Button>().onClick = new Button.ButtonClickedEvent();
            extButtonGameObject.GetComponentInChildren<Button>().onClick.AddListener(extButtonAction);
            extButtonGameObject.GetComponentInChildren<VRC.UI.Elements.Tooltips.UiTooltip>().field_Public_String_0 = extButtonTooltip;
            if (extButtonSprite != null)
            {
                extButtonGameObject.GetComponentInChildren<Image>().sprite = extButtonSprite;
                extButtonGameObject.GetComponentInChildren<Image>().overrideSprite = extButtonSprite;
                if (preserveColor)
                {
                    extButtonGameObject.GetComponentInChildren<Image>().color = Color.white;
                    extButtonGameObject.GetComponentInChildren<VRC.UI.Core.Styles.StyleElement>().enabled = false;
                }
            }
            this.preserveColor = preserveColor;
            menuMask = menuContents.parent.gameObject.GetComponent<RectMask2D>();
            menuMask.enabled = true;
            gameObject.transform.Find("ScrollRect").GetComponent<ScrollRect>().enabled = true;
            gameObject.transform.Find("ScrollRect").GetComponent<ScrollRect>().verticalScrollbar = gameObject.transform.Find("ScrollRect/Scrollbar").GetComponent<Scrollbar>();
            gameObject.transform.Find("ScrollRect").GetComponent<ScrollRect>().verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHide;
        }
        public void AddExtButton(Action onClick, string tooltip, Sprite icon)
        {
            GameObject newExtButton = GameObject.Instantiate(extButtonGameObject, extButtonGameObject.transform.parent);
            newExtButton.SetActive(true);
            newExtButton.GetComponentInChildren<Button>().onClick = new Button.ButtonClickedEvent();
            newExtButton.GetComponentInChildren<Button>().onClick.AddListener(onClick);
            newExtButton.GetComponentInChildren<Image>().sprite = icon;
            newExtButton.GetComponentInChildren<Image>().overrideSprite = icon;
            newExtButton.GetComponentInChildren<VRC.UI.Elements.Tooltips.UiTooltip>().field_Public_String_0 = tooltip;
        }
        public void OpenMenu()
        {
            ButtonAPI.GetMenuStateControllerInstance().Method_Public_Void_String_UIContext_Boolean_TransitionType_0(page.field_Public_String_0, page.prop_UIContext_0, false, UIPage.TransitionType.Right);
        }
        public void CloseMenu()
        {
            backButtonGameObject.GetComponentInChildren<Button>().onClick.Invoke();
            //typeof(UIPage).GetMethods().First(a => XrefUtils.CheckUsing(a, "Method_Public_Void_UIPage_0")).Invoke(page, null);
            //page.Method_Public_Virtual_New_Void_0();
        }
    }
    public class Tab
    {
        private readonly GameObject gameObject;
        private readonly MenuTab menuTab;
        private readonly Image tabIcon;
        private readonly VRC.UI.Elements.Tooltips.UiTooltip tabTooltip;
        private readonly GameObject badgeGameObject;
        private readonly TextMeshProUGUI badgeText;
        public Tab(Transform parent, string menuName, string tooltip, Sprite icon = null, Action tabAction = null)
        {
            gameObject = GameObject.Instantiate(ButtonAPI.menuTabBase, parent);
            gameObject.name = menuName + "Tab";
            menuTab = gameObject.GetComponent<MenuTab>();
            menuTab.field_Private_MenuStateController_0 = ButtonAPI.GetMenuStateControllerInstance();
            menuTab.field_Public_String_0 = menuName;
            tabIcon = gameObject.transform.Find("Icon").GetComponent<Image>();
            tabIcon.sprite = icon;
            tabIcon.overrideSprite = icon;
            tabTooltip = gameObject.GetComponent<VRC.UI.Elements.Tooltips.UiTooltip>();
            tabTooltip.field_Public_String_0 = tooltip;

            badgeGameObject = gameObject.transform.GetChild(0).gameObject;
            badgeText = badgeGameObject.GetComponentInChildren<TextMeshProUGUI>();

            menuTab.GetComponent<VRC.UI.Core.Styles.StyleElement>().field_Private_Selectable_0 = menuTab.GetComponent<Button>();
            menuTab.GetComponent<Button>().onClick.AddListener(new System.Action(() => {
                tabAction?.DelegateSafeInvoke();
                menuTab.GetComponent<VRC.UI.Core.Styles.StyleElement>().field_Private_Selectable_0 = menuTab.GetComponent<Button>();
            }));
        }
        public void SetBadge(bool showing = true, string text = "")
        {
            if (badgeGameObject == null || badgeText == null) return;
            badgeGameObject.SetActive(showing);
            badgeText.text = text;
        }
        public void SetTooltip(string newText)
        {
            tabTooltip.field_Public_String_0 = newText;
        }
    }

    public class WingButton {
        public readonly GameObject leftGameObject;
        public readonly GameObject rightGameObject;

        private readonly Image leftButtonImage;
        private readonly Image rightButtonImage;

        private readonly TextMeshProUGUI leftButtonText;
        private readonly TextMeshProUGUI rightButtonText;

        private readonly Button leftButtonButton;
        private readonly Button rightButtonButton;

        private readonly Image leftButtonArrow;
        private readonly Image rightButtonArrow;

        private readonly GameObject leftSeparator;
        private readonly GameObject rightSeparator;

        private readonly VRC.UI.Elements.Tooltips.UiTooltip leftButtonTooltip;
        private readonly VRC.UI.Elements.Tooltips.UiTooltip rightButtonTooltip;

        public WingButton(Transform parentLeft, Transform parentRight, string text, Action click, string tooltip, Sprite icon = null, bool preserveColor = false)
        {
            leftGameObject = GameObject.Instantiate(ButtonAPI.wingSingleButtonBase, parentLeft);
            leftButtonText = leftGameObject.GetComponentInChildren<TextMeshProUGUI>(true);
            leftButtonText.text = text;
            leftButtonButton = leftGameObject.GetComponentInChildren<Button>(true);
            leftButtonButton.onClick = new Button.ButtonClickedEvent();
            leftButtonButton.onClick.AddListener(click);
            leftButtonTooltip = leftGameObject.GetComponentInChildren<VRC.UI.Elements.Tooltips.UiTooltip>(true);
            leftButtonTooltip.field_Public_String_0 = tooltip;
            leftButtonImage = leftGameObject.transform.Find("Container/Icon").GetComponentInChildren<Image>(true);
            leftButtonArrow = leftGameObject.transform.Find("Container/Icon_Arrow").GetComponentInChildren<Image>(true);
            leftSeparator = leftGameObject.transform.Find("Separator").gameObject;
            if (icon != null)
            {
                leftButtonImage.sprite = icon;
                leftButtonImage.overrideSprite = icon;
                leftButtonImage.gameObject.SetActive(true);
                if (preserveColor)
                {
                    leftButtonImage.color = Color.white;
                    leftButtonImage.GetComponent<VRC.UI.Core.Styles.StyleElement>().enabled = false;
                }
            }
            else
                leftButtonImage.gameObject.SetActive(false);
            rightGameObject = GameObject.Instantiate(leftGameObject, parentRight);
            rightButtonText = rightGameObject.GetComponentInChildren<TextMeshProUGUI>(true);
            rightButtonButton = rightGameObject.GetComponentInChildren<Button>(true);
            rightButtonTooltip = rightGameObject.GetComponentInChildren<VRC.UI.Elements.Tooltips.UiTooltip>(true);
            rightButtonImage = rightGameObject.transform.Find("Container/Icon").GetComponentInChildren<Image>(true);
            rightButtonArrow = rightGameObject.transform.Find("Container/Icon_Arrow").GetComponentInChildren<Image>(true);
            rightSeparator = rightGameObject.transform.Find("Separator").gameObject;
        }
        public WingButton(WingPage basePage, string text, Action click, string tooltip, Sprite icon = null, bool preserveColor = false)
        {
            leftGameObject = GameObject.Instantiate(ButtonAPI.wingSingleButtonBase, basePage.leftMenuContents);
            leftButtonText = leftGameObject.GetComponentInChildren<TextMeshProUGUI>(true);
            leftButtonText.text = text;
            leftButtonButton = leftGameObject.GetComponentInChildren<Button>(true);
            leftButtonButton.onClick = new Button.ButtonClickedEvent();
            leftButtonButton.onClick.AddListener(click);
            leftButtonTooltip = leftGameObject.GetComponentInChildren<VRC.UI.Elements.Tooltips.UiTooltip>(true);
            leftButtonTooltip.field_Public_String_0 = tooltip;
            leftButtonImage = leftGameObject.transform.Find("Container/Icon").GetComponentInChildren<Image>(true);
            leftButtonArrow = leftGameObject.transform.Find("Container/Icon_Arrow").GetComponentInChildren<Image>(true);
            leftSeparator = leftGameObject.transform.Find("Separator").gameObject;
            if (icon != null)
            {
                leftButtonImage.sprite = icon;
                leftButtonImage.overrideSprite = icon;
                leftButtonImage.gameObject.SetActive(true);
                if (preserveColor)
                {
                    leftButtonImage.color = Color.white;
                    leftButtonImage.GetComponent<VRC.UI.Core.Styles.StyleElement>().enabled = false;
                }
            }
            else
                leftButtonImage.gameObject.SetActive(false);
            rightGameObject = GameObject.Instantiate(leftGameObject, basePage.rightMenuContents);
            rightButtonText = rightGameObject.GetComponentInChildren<TextMeshProUGUI>(true);
            rightButtonButton = rightGameObject.GetComponentInChildren<Button>(true);
            rightButtonTooltip = rightGameObject.GetComponentInChildren<VRC.UI.Elements.Tooltips.UiTooltip>(true);
            rightButtonImage = rightGameObject.transform.Find("Container/Icon").GetComponentInChildren<Image>(true);
            rightButtonArrow = rightGameObject.transform.Find("Container/Icon_Arrow").GetComponentInChildren<Image>(true);
            rightSeparator = rightGameObject.transform.Find("Separator").gameObject;
        }
        public WingButton(string text, Action click, string tooltip, Sprite icon = null, bool preserveColor = false) : this(ButtonAPI.wingPageBaseL.transform.Find("WingMenu/ScrollRect/Viewport/VerticalLayoutGroup"), ButtonAPI.wingPageBaseR.transform.Find("WingMenu/ScrollRect/Viewport/VerticalLayoutGroup"), text, click, tooltip, icon, preserveColor) { }
        //public SingleButton(MenuPage pge, string text, Action click, string tooltip, Sprite icon = null, bool preserveColor = false) : this(pge.menuContents, text, click, tooltip, icon, preserveColor) { }
        //public SingleButton(ButtonGroup grp, string text, Action click, string tooltip, Sprite icon = null, bool preserveColor = false) : this(grp.gameObject.transform, text, click, tooltip, icon, preserveColor) { }
        public void SetAction(Action newAction)
        {
            leftButtonButton.onClick = new Button.ButtonClickedEvent();
            leftButtonButton.onClick.AddListener(newAction);
            rightButtonButton.onClick = new Button.ButtonClickedEvent();
            rightButtonButton.onClick.AddListener(newAction);
        }
        public void SetAction(Action newAction, bool right)
        {
            if (!right)
            {
                leftButtonButton.onClick = new Button.ButtonClickedEvent();
                leftButtonButton.onClick.AddListener(newAction);
            }
            else
            {
                rightButtonButton.onClick = new Button.ButtonClickedEvent();
                rightButtonButton.onClick.AddListener(newAction);
            }
        }
        public void SetText(string newText)
        {
            leftButtonText.text = newText;
            rightButtonText.text = newText;
        }
        public void SetTooltip(string newTooltip)
        {
            leftButtonTooltip.field_Public_String_0 = newTooltip;
            rightButtonTooltip.field_Public_String_0 = newTooltip;
        }
        public void SetIcon(Sprite newIcon, bool preserveColor = false)
        {
            if (newIcon == null)
            {
                leftButtonImage.gameObject.SetActive(false);
                rightButtonImage.gameObject.SetActive(false);
            }
            else
            {
                leftButtonImage.sprite = newIcon;
                leftButtonImage.overrideSprite = newIcon;
                leftButtonImage.gameObject.SetActive(true);
                if (preserveColor)
                {
                    leftButtonImage.color = Color.white;
                    leftButtonImage.GetComponent<VRC.UI.Core.Styles.StyleElement>().enabled = false;
                }
                rightButtonImage.sprite = newIcon;
                rightButtonImage.overrideSprite = newIcon;
                rightButtonImage.gameObject.SetActive(true);
                if (preserveColor)
                {
                    rightButtonImage.color = Color.white;
                    rightButtonImage.GetComponent<VRC.UI.Core.Styles.StyleElement>().enabled = false;
                }
            }

        }
        public void SetIconColor(Color color)
        {
            leftButtonImage.color = color;
            leftButtonImage.GetComponent<VRC.UI.Core.Styles.StyleElement>().enabled = false;
            rightButtonImage.color = color;
            rightButtonImage.GetComponent<VRC.UI.Core.Styles.StyleElement>().enabled = false;
        }
        public void SetInteractable(bool val)
        {
            leftButtonButton.interactable = val;
            rightButtonButton.interactable = val;
        }
        public void SetActive(bool state)
        {
            leftGameObject.SetActive(state);
            rightGameObject.SetActive(state);
        }
        public void SetArrowEnabled(bool state)
        {
            leftButtonArrow.gameObject.SetActive(state);
            rightButtonArrow.gameObject.SetActive(state);
        }
        public void SetSeparatorEnabled(bool state)
        {
            leftSeparator.SetActive(state);
            rightSeparator.SetActive(state);
        }
        public Image[] GetBackground()
        {
            Image backgroundL = leftGameObject.transform.Find("Container/Background").GetComponentInChildren<Image>(true);
            Image backgroundR = rightGameObject.transform.Find("Container/Background").GetComponentInChildren<Image>(true);
            return new Image[] { backgroundL, backgroundR };
        }

    }
    public class WingToggle
    {
        private readonly WingButton baseButton;
        private bool value;
        private readonly Sprite offSprite;
        private readonly Sprite onSprite;
        private Action<bool> onClick;
        private string offTooltip;
        private string onTooltip;

        public WingToggle(Transform leftTransform, Transform rightTransform, string text, Action<bool> click,string offTooltip, string onTooltip, Sprite icon = null, bool preserveColor = false, bool defaultValue = false)
        {
            if (icon == null) icon = ButtonAPI.onIconSprite;
            baseButton = new WingButton(leftTransform, rightTransform, text, ToggleClicked, defaultValue ? onTooltip : offTooltip, icon, preserveColor);
            baseButton.SetAction(ToggleClicked, true);
            onClick = click;
            value = defaultValue;
            offSprite = ButtonAPI.xIconSprite;
            onSprite = icon;
            this.offTooltip = offTooltip;
            this.onTooltip = onTooltip;
        }
        public WingToggle(WingPage basePage, string text, Action<bool> click, string offTooltip, string onTooltip, Sprite icon = null, bool preserveColor = false, bool defaultValue = false)
        {
            if (icon == null) icon = ButtonAPI.onIconSprite;
            baseButton = new WingButton(basePage, text, () => { ToggleClicked(); }, defaultValue ? onTooltip : offTooltip, icon, preserveColor);
            baseButton.SetAction(ToggleClicked, true);
            onClick = click;
            value = defaultValue;
            offSprite = ButtonAPI.xIconSprite;
            onSprite = icon;
            this.offTooltip = offTooltip;
            this.onTooltip = onTooltip;
        }
        private void ToggleClicked()
        {
            value = !value;
            baseButton.SetIcon(value ? onSprite : offSprite);
            baseButton.SetTooltip(value ? onTooltip : offTooltip);
            onClick?.Invoke(value);
        }
        public void SetToggleState(bool newValue, bool invoke = false)
        {
            value = newValue;
            baseButton.SetIcon(value ? onSprite : offSprite);
            if (invoke)
                onClick?.Invoke(value);
        }
        public void SetInteractable(bool value) => baseButton.SetInteractable(value);
        public void SetArrowEnabled(bool value) => baseButton.SetArrowEnabled(value);
        public void SetSeparatorEnabled(bool value) => baseButton.SetSeparatorEnabled(value);

    }
    public class WingPage
    {
        private readonly UIPage leftPage;
        private readonly UIPage rightPage;
        private readonly GameObject leftGameObject;
        private readonly GameObject rightGameObject;
        public readonly Transform leftMenuContents;
        public readonly Transform rightMenuContents;
        private readonly TextMeshProUGUI leftPageTitleText;
        private readonly TextMeshProUGUI rightPageTitleText;
        private readonly bool isRoot;
        private readonly GameObject leftBackButtonGameObject;
        private readonly GameObject rightBackButtonGameObject;
        private bool preserveColor;
        public readonly RectMask2D leftMenuMask;
        public readonly RectMask2D rightMenuMask;
        public WingPage(string menuName, string pageTitle, bool root = true, bool backButton = true, bool preserveColor = false)
        {
            leftGameObject = GameObject.Instantiate(ButtonAPI.wingPageBase, ButtonAPI.wingPageBaseL.transform);
            leftGameObject.name = "WingMenu_" + menuName+"_L";
            leftGameObject.SetActive(false);
            GameObject.DestroyImmediate(leftGameObject.GetComponent<UIPage>());
            GameObject.DestroyImmediate(leftGameObject.GetComponent<MonoBehaviourPublicOb_aGa_aUnique>());
            leftPage = leftGameObject.AddComponent<UIPage>();
            leftPage.field_Public_String_0 = menuName+"_L";
            leftPage.field_Private_Boolean_1 = true;
            leftPage.field_Protected_MenuStateController_0 = ButtonAPI.GetLeftWingControllerInstance();
            leftPage.field_Private_List_1_UIPage_0 = new Il2CppSystem.Collections.Generic.List<UIPage>();
            leftPage.field_Private_List_1_UIPage_0.Add(leftPage);
            ButtonAPI.GetLeftWingControllerInstance().field_Private_Dictionary_2_String_UIPage_0.Add(menuName+"_L", leftPage);
            if (root)
            {
                List<UIPage> menuRootPages = ButtonAPI.GetLeftWingControllerInstance().field_Public_ArrayOf_UIPage_0.ToList();
                menuRootPages.Add(leftPage);
                ButtonAPI.GetLeftWingControllerInstance().field_Public_ArrayOf_UIPage_0 = menuRootPages.ToArray();
            }
            leftGameObject.transform.Find("ScrollRect/Viewport/VerticalLayoutGroup").DestroyChildren();
            leftGameObject.transform.Find("ScrollRect/Viewport/VerticalLayoutGroup").GetComponent<VerticalLayoutGroup>().spacing = 0;
            leftGameObject.transform.Find("ScrollRect/Viewport/VerticalLayoutGroup").GetComponent<VerticalLayoutGroup>().padding = new RectOffset(0, 0, 0, 0);
            leftMenuContents = leftGameObject.transform.Find("ScrollRect/Viewport/VerticalLayoutGroup");
            leftMenuContents.transform.DestroyChildren();
            leftPageTitleText = leftGameObject.GetComponentInChildren<TextMeshProUGUI>(true);
            leftPageTitleText.text = pageTitle;
            isRoot = root;
            leftBackButtonGameObject = leftGameObject.transform.Find("WngHeader_H1/LeftItemContainer/Button_Back").gameObject;
            leftBackButtonGameObject.SetActive(backButton);
            leftBackButtonGameObject.GetComponentInChildren<Button>().onClick = new Button.ButtonClickedEvent();
            leftBackButtonGameObject.GetComponentInChildren<Button>().onClick.AddListener(new System.Action(() =>
            {
                    ButtonAPI.GetLeftWingControllerInstance().Method_Public_Void_String_UIContext_Boolean_TransitionType_0(leftPage.field_Public_String_0, leftPage.prop_UIContext_0, false, UIPage.TransitionType.Left);
            }));
            this.preserveColor = preserveColor;

            rightGameObject = GameObject.Instantiate(leftGameObject, ButtonAPI.wingPageBaseR.transform);
            rightGameObject.name = "WingMenu_" + menuName+"_R";
            rightGameObject.SetActive(false);
            rightPage = rightGameObject.GetComponentInChildren<UIPage>(true);
            rightPage.field_Public_String_0 = menuName + "_R";
            rightPage.field_Protected_MenuStateController_0 = ButtonAPI.GetRightWingControllerInstance();
            rightPage.field_Private_List_1_UIPage_0 = new Il2CppSystem.Collections.Generic.List<UIPage>();
            rightPage.field_Private_List_1_UIPage_0.Add(rightPage);
            ButtonAPI.GetRightWingControllerInstance().field_Private_Dictionary_2_String_UIPage_0.Add(menuName + "_R", rightPage);
            if (root)
            {
                List<UIPage> menuRootPages = ButtonAPI.GetRightWingControllerInstance().field_Public_ArrayOf_UIPage_0.ToList();
                menuRootPages.Add(rightPage);
                ButtonAPI.GetRightWingControllerInstance().field_Public_ArrayOf_UIPage_0 = menuRootPages.ToArray();
            }
            rightMenuContents = rightGameObject.transform.Find("ScrollRect/Viewport/VerticalLayoutGroup");
            rightPageTitleText = rightGameObject.GetComponentInChildren<TextMeshProUGUI>(true);
            rightBackButtonGameObject = rightGameObject.transform.Find("WngHeader_H1/LeftItemContainer/Button_Back").gameObject;
            rightBackButtonGameObject.GetComponentInChildren<Button>().onClick.AddListener(new System.Action(() =>
            {
                    ButtonAPI.GetRightWingControllerInstance().Method_Public_Void_String_UIContext_Boolean_TransitionType_0(rightPage.field_Public_String_0, rightPage.prop_UIContext_0, false, UIPage.TransitionType.Left);
                //this.rightPage.Method_Protected_Virtual_New_Void_0();
            }));
        }
        public void OpenLeftMenu()
        {
            ButtonAPI.GetLeftWingControllerInstance().Method_Public_Void_String_UIContext_Boolean_TransitionType_0(leftPage.field_Public_String_0, leftPage.prop_UIContext_0, false, UIPage.TransitionType.Right);
            ButtonAPI.GetLeftWingControllerInstance().field_Private_ArrayOf_Wing_0.First().prop_String_0 = leftPage.field_Public_String_0;
        }
        public void OpenRightMenu()
        {
            ButtonAPI.GetRightWingControllerInstance().Method_Public_Void_String_UIContext_Boolean_TransitionType_0(rightPage.field_Public_String_0, rightPage.prop_UIContext_0, false, UIPage.TransitionType.Right);
            ButtonAPI.GetRightWingControllerInstance().field_Private_ArrayOf_Wing_0.First().prop_String_0 = rightPage.field_Public_String_0;
        }
        public void CloseLeftMenu()
        {
            typeof(UIPage).GetMethods().First(a => XrefUtils.CheckUsing(a, "Method_Public_Void_UIPage_0")).Invoke(leftPage, null);
        }
        public void CloseRightMenu()
        {
            typeof(UIPage).GetMethods().First(a => XrefUtils.CheckUsing(a, "Method_Public_Void_UIPage_0")).Invoke(rightPage, null);
        }
    }
}
