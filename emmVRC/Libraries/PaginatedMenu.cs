using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


namespace emmVRC.Libraries
{
    public enum PageItems
    {
        Button = 1,
        Toggle = 2
    }
    public class PageItem
    {
        public string Name = "";
        public System.Action Action;
        public string Tooltip = "";
        public Sprite buttonSprite = null;
        public bool Active = true;
        public string onName = "";
        public string offName = "";
        public Action OnAction;
        public Action OffAction;
        public bool ToggleState = true;
        public PageItems type = PageItems.Button;
        public PageItem(string name, Action action, string tooltip, bool active = true)
        {
            Name = name;
            Action = action;
            Tooltip = tooltip;
            Active = active;
            type = PageItems.Button;
        }
        public PageItem(string onName, System.Action onAction, string offName, System.Action offAction, string tooltip, bool active = true, bool defaultState = true)
        {
            this.onName = onName;
            this.offName = offName;
            OnAction = onAction;
            OffAction = offAction;
            Tooltip = tooltip;
            Active = active;
            ToggleState = defaultState;
            type = PageItems.Toggle;
        }
        public PageItem(string onName, string offName, System.Action<bool> action, string tooltip, bool active = true, bool defaultState = true) : this(onName, () => action.Invoke(true), offName, () => action.Invoke(false), tooltip, active, defaultState) { }
        public static PageItem Space
        {
            get
            {
                return new PageItem("", null, "", false);
            }
        }
        public void SetToggleState(bool newBool, bool triggerAction = false)
        {
            if (type == PageItems.Button) return;
            ToggleState = newBool;
            if (triggerAction)
            {
                ButtonAction();
            }
        }
        public void ButtonAction()
        {
            SetToggleState(!ToggleState);
            if (ToggleState)
                OnAction.Invoke();
            else if (!ToggleState)
                OffAction.Invoke();
        }
    }
    public class PaginatedMenu
    {
        public string menuName;
        public QMSingleButton menuEntryButton;
        public int currentPage = 0;
        public List<string> pageTitles = new List<string>();
        public List<PageItem> pageItems = new List<PageItem>();
        public QMNestedButton menuBase;
        private QMSingleButton previousPageButton;
        private QMSingleButton pageCount;
        private QMSingleButton nextPageButton;
        private QMSingleButton templateButton;

        private List<QMSingleButton> items = new List<QMSingleButton>();
        private List<QMToggleButton> toggleItems = new List<QMToggleButton>();

        private GameObject menuTitle = null;

        public PaginatedMenu(string parentPath, int x, int y, string menuName, string menuTooltip, Color? buttonColor)
        {
            emmVRCLoader.Logger.LogDebug("Initializing paginated menu...");
            menuBase = new QMNestedButton(parentPath, x, y, menuName, "");
            menuBase.getMainButton().DestroyMe();

            menuEntryButton = new QMSingleButton(parentPath, x, y, menuName, this.OpenMenu, menuTooltip, buttonColor);

            previousPageButton = new QMSingleButton(menuBase, 4, 0, "", () =>
            {
                if (currentPage != 0)
                    currentPage--;
                UpdateMenu();
            }, "Move to the previous page", buttonColor);
            menuTitle = GameObject.Instantiate(QuickMenuUtils.GetQuickMenuInstance().transform.Find("QuickMenu_NewElements/_InfoBar/EarlyAccessText").gameObject, this.menuBase.getBackButton().getGameObject().transform.parent);
            menuTitle.GetComponent<Text>().fontStyle = FontStyle.Normal;
            menuTitle.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
            menuTitle.GetComponent<Text>().text = "";
            menuTitle.GetComponent<RectTransform>().anchoredPosition += new Vector2(580f, -1700f /*-440f*/);
            previousPageButton.getGameObject().GetComponent<Image>().sprite = QuickMenuUtils.GetQuickMenuInstance().transform.Find("EmojiMenu/PageUp").GetComponent<Image>().sprite;
            pageCount = new QMSingleButton(menuBase, 4, 1, "Page\n0/0", null, "Indicates the page you are on");
            GameObject.DestroyObject(pageCount.getGameObject().GetComponentInChildren<ButtonReaction>());
            GameObject.DestroyObject(pageCount.getGameObject().GetComponentInChildren<UiTooltip>());
            GameObject.DestroyObject(pageCount.getGameObject().GetComponentInChildren<Image>());
            nextPageButton = new QMSingleButton(menuBase, 4, 2, "", () =>
            {
                if (pageItems.Count > 9)
                    currentPage++;
                UpdateMenu();
            }, "Move to the next page", buttonColor);
            nextPageButton.getGameObject().GetComponent<Image>().sprite = QuickMenuUtils.GetQuickMenuInstance().transform.Find("EmojiMenu/PageDown").GetComponent<Image>().sprite;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    items.Add(new QMSingleButton(menuBase, j + 1, i, "", null, "", buttonColor));
                    toggleItems.Add(new QMToggleButton(menuBase, j + 1, i, "", null, "", null, "", buttonColor));
                }
            }
            templateButton = new QMSingleButton(menuBase, 1923, 1023, "", null, "");
            templateButton.setActive(false);
        }

        public PaginatedMenu(QMNestedButton menuButton, int x, int y, string menuName, string menuTooltip, Color? buttonColor) : this(menuButton.getMenuName(), x, y, menuName, menuTooltip, buttonColor) { }
        public void OpenMenu()
        {
            currentPage = 0;
            UpdateMenu();
            QuickMenuUtils.ShowQuickmenuPage(menuBase.getMenuName());
        }
        public void UpdateMenu()
        {

            pageCount.setActive(false);
            foreach (QMSingleButton qmbutton in items)
            {
                qmbutton.setActive(false);
            }

            foreach (QMToggleButton qmtoggle in toggleItems)
            {
                qmtoggle.setActive(false);
            }
            int maxPages = (int)Math.Ceiling((double)pageItems.Count / 9);
            maxPages--;
            if (currentPage < 0)
                currentPage = 0;
            if (currentPage > maxPages)
                currentPage = maxPages;
            if (pageItems.Count > 9)
            {
                pageCount.setActive(true);
                pageCount.setButtonText("Page\n" + (currentPage + 1) + " of " + ((int)Math.Ceiling((double)pageItems.Count / 9)));
            }

            List<PageItem> itemsToAdd = pageItems.GetRange(currentPage * 9, Math.Abs(currentPage * 9 - pageItems.Count));

            if (itemsToAdd == null)
                emmVRCLoader.Logger.LogError("The page list of items is null. This is a problem.");
            else if (itemsToAdd.Count > 0)
            {
                for (int i = 0; i < (itemsToAdd.Count > 9 ? 9 : itemsToAdd.Count); i++)
                {
                    if (itemsToAdd[i].type == PageItems.Button)
                    {
                        items[i].setButtonText(itemsToAdd[i].Name);
                        if (itemsToAdd[i].buttonSprite != null)
                        {
                            items[i].getGameObject().GetComponent<Image>().sprite = itemsToAdd[i].buttonSprite;
                            items[i].getGameObject().GetComponent<Button>().colors = new ColorBlock
                            {
                                normalColor = new Color(1f, 1f, 1f, 1f),
                                disabledColor = new Color(0.75f, 0.75f, 0.75f, 1f),
                                highlightedColor = new Color(0.975f, 0.975f, 0.975f, 1f),
                                pressedColor = new Color(0.75f, 0.75f, 0.75f, 1f),
                                colorMultiplier = 1f,
                                fadeDuration = 0.1f
                            };
                        }
                        else
                        {
                            items[i].getGameObject().GetComponent<Image>().sprite = templateButton.getGameObject().GetComponent<Image>().sprite;
                            //items[i].getGameObject().GetComponent<Button>().colors = templateButton.getGameObject().GetComponent<Button>().colors;
                        }
                        items[i].setAction(itemsToAdd[i].Action);
                        items[i].setToolTip(itemsToAdd[i].Tooltip);
                        items[i].setActive(itemsToAdd[i].Active);
                    }
                    else
                    {
                        toggleItems[i].setOnText(itemsToAdd[i].onName);
                        toggleItems[i].setOffText(itemsToAdd[i].offName);
                        toggleItems[i].setAction(itemsToAdd[i].ButtonAction, itemsToAdd[i].ButtonAction);
                        toggleItems[i].setToggleState(itemsToAdd[i].ToggleState);
                        toggleItems[i].setToolTip(itemsToAdd[i].Tooltip);
                        toggleItems[i].setActive(itemsToAdd[i].Active);
                    }
                        if ((currentPage + 1 > pageTitles.Count || pageTitles.Count <= 0) && menuTitle != null)
                        {
                            menuTitle.GetComponent<Text>().text = "";
                            //menuTitle.GetComponent<Text>().color = configUtils.menuColor();
                        }
                        else if (menuTitle != null)
                        {
                            menuTitle.GetComponent<Text>().text = pageTitles[currentPage];
                            //menuTitle.GetComponent<Text>().color = configUtils.menuColor();
                        }
                }
            }
        }
    }
}
