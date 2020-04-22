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
        public PageItem()
        { }
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
        public static PageItem Space()
        {
            return new PageItem("", null, "", false);
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
        public QMSingleButton menuEntryButton { get; set; }
        public int currentPage { get; set; } = 0;
        public List<string> pageTitles = new List<string>();
        public List<PageItem> pageItems = new List<PageItem>();
        public QMNestedButton menuBase;
        private QMSingleButton previousPageButton;
        private QMSingleButton pageCount;
        private QMSingleButton nextPageButton;

        private QMSingleButton item1;
        private QMSingleButton item2;
        private QMSingleButton item3;
        private QMSingleButton item4;
        private QMSingleButton item5;
        private QMSingleButton item6;
        private QMSingleButton item7;
        private QMSingleButton item8;
        private QMSingleButton item9;
        private QMToggleButton toggleItem1;
        private QMToggleButton toggleItem2;
        private QMToggleButton toggleItem3;
        private QMToggleButton toggleItem4;
        private QMToggleButton toggleItem5;
        private QMToggleButton toggleItem6;
        private QMToggleButton toggleItem7;
        private QMToggleButton toggleItem8;
        private QMToggleButton toggleItem9;

        private GameObject menuTitle = null;

        public PaginatedMenu(string parentPath, int x, int y, string menuName, string menuTooltip, Color? buttonColor)
        {
            menuBase = new QMNestedButton(parentPath, x, y, menuName, "");
            menuBase.getMainButton().DestroyMe();
            

            menuEntryButton = new QMSingleButton(parentPath, x, y, menuName, () => { this.OpenMenu(); }, menuTooltip, buttonColor);

            previousPageButton = new QMSingleButton(menuBase, 4, 0, "", () => {
                if (currentPage != 0)
                    currentPage--;
                try
                {
                    UpdateMenu();
                }
                catch (Exception ex)
                {
                    //emmVRCLoader.Logger.Log(ex.ToString());
                }
            }, "Move to the previous page", buttonColor);
            menuTitle = GameObject.Instantiate(QuickMenuUtils.GetQuickMenuInstance().transform.Find("ShortcutMenu/EarlyAccessText").gameObject, this.menuBase.getBackButton().getGameObject().transform.parent);
            menuTitle.GetComponent<Text>().fontStyle = FontStyle.Normal;
            menuTitle.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
            menuTitle.GetComponent<Text>().text = "";
            menuTitle.GetComponent<RectTransform>().anchoredPosition += new Vector2(580f, -440f);
            previousPageButton.getGameObject().GetComponent<Image>().sprite = QuickMenuUtils.GetQuickMenuInstance().transform.Find("EmojiMenu/PageUp").GetComponent<Image>().sprite;
            pageCount = new QMSingleButton(menuBase, 4, 1, "Page\n0/0", null, "Indicates the page you are on");
            GameObject.DestroyObject(pageCount.getGameObject().GetComponentInChildren<ButtonReaction>());
            GameObject.DestroyObject(pageCount.getGameObject().GetComponentInChildren<UiTooltip>());
            GameObject.DestroyObject(pageCount.getGameObject().GetComponentInChildren<Image>());
            nextPageButton = new QMSingleButton(menuBase, 4, 2, "", () => 
            {
                if (pageItems.Count > 9)
                    currentPage++;
                try
                {
                    UpdateMenu();
                }
                catch (Exception ex)
                {
                    //emmVRCLoader.Logger.Log(ex.ToString());
                }
            }, "Move to the next page", buttonColor);
            nextPageButton.getGameObject().GetComponent<Image>().sprite = QuickMenuUtils.GetQuickMenuInstance().transform.Find("EmojiMenu/PageDown").GetComponent<Image>().sprite;
            item1 = new QMSingleButton(menuBase, 1, 0, "", null, "", buttonColor);
            item2 = new QMSingleButton(menuBase, 2, 0, "", null, "", buttonColor);
            item3 = new QMSingleButton(menuBase, 3, 0, "", null, "", buttonColor);
            item4 = new QMSingleButton(menuBase, 1, 1, "", null, "", buttonColor);
            item5 = new QMSingleButton(menuBase, 2, 1, "", null, "", buttonColor);
            item6 = new QMSingleButton(menuBase, 3, 1, "", null, "", buttonColor);
            item7 = new QMSingleButton(menuBase, 1, 2, "", null, "", buttonColor);
            item8 = new QMSingleButton(menuBase, 2, 2, "", null, "", buttonColor);
            item9 = new QMSingleButton(menuBase, 3, 2, "", null, "", buttonColor);
            toggleItem1 = new QMToggleButton(menuBase, 1, 0, "", null, "", null, "", buttonColor);
            toggleItem2 = new QMToggleButton(menuBase, 2, 0, "", null, "", null, "", buttonColor);
            toggleItem3 = new QMToggleButton(menuBase, 3, 0, "", null, "", null, "", buttonColor);
            toggleItem4 = new QMToggleButton(menuBase, 1, 1, "", null, "", null, "", buttonColor);
            toggleItem5 = new QMToggleButton(menuBase, 2, 1, "", null, "", null, "", buttonColor);
            toggleItem6 = new QMToggleButton(menuBase, 3, 1, "", null, "", null, "", buttonColor);
            toggleItem7 = new QMToggleButton(menuBase, 1, 2, "", null, "", null, "", buttonColor);
            toggleItem8 = new QMToggleButton(menuBase, 2, 2, "", null, "", null, "", buttonColor);
            toggleItem9 = new QMToggleButton(menuBase, 3, 2, "", null, "", null, "", buttonColor);
        }

        public PaginatedMenu(QMNestedButton menuButton, int x, int y, string menuName, string menuTooltip, Color? buttonColor)
        {
            menuBase = new QMNestedButton(menuButton, x, y, menuName, "");
            menuBase.getMainButton().DestroyMe();

            menuEntryButton = new QMSingleButton(menuButton, x, y, menuName, () => 
            {
                this.OpenMenu();
            }, menuTooltip, buttonColor);

            previousPageButton = new QMSingleButton(menuBase, 4, 0, "", () => 
            {
                currentPage--;
                try
                {
                    UpdateMenu();
                }
                catch (Exception ex)
                {
                    //emmVRCLoader.Logger.Log(ex.ToString());
                }
            }, "Move to the previous page", buttonColor);
            menuTitle = GameObject.Instantiate(QuickMenuUtils.GetQuickMenuInstance().transform.Find("ShortcutMenu/EarlyAccessText").gameObject, this.menuBase.getBackButton().getGameObject().transform.parent);
            menuTitle.GetComponent<Text>().fontStyle = FontStyle.Normal;
            menuTitle.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
            menuTitle.GetComponent<Text>().text = "";
            menuTitle.GetComponent<RectTransform>().anchoredPosition += new Vector2(580f, -440f);
            previousPageButton.getGameObject().GetComponent<Image>().sprite = QuickMenuUtils.GetQuickMenuInstance().transform.Find("EmojiMenu/PageUp").GetComponent<Image>().sprite;
            pageCount = new QMSingleButton(menuBase, 4, 1, "Page\n0/0", null, "Indicates the page you are on");
            GameObject.DestroyObject(pageCount.getGameObject().GetComponentInChildren<ButtonReaction>());
            GameObject.DestroyObject(pageCount.getGameObject().GetComponentInChildren<UiTooltip>());
            GameObject.DestroyObject(pageCount.getGameObject().GetComponentInChildren<Image>());
            nextPageButton = new QMSingleButton(menuBase, 4, 2, "", () => 
            {
                currentPage++;
                try
                {
                    UpdateMenu();
                }
                catch (Exception ex)
                {
                    //emmVRCLoader.Logger.Log(ex.ToString());
                }
            }, "Move to the next page", buttonColor);
            nextPageButton.getGameObject().GetComponent<Image>().sprite = QuickMenuUtils.GetQuickMenuInstance().transform.Find("EmojiMenu/PageDown").GetComponent<Image>().sprite;
            item1 = new QMSingleButton(menuBase, 1, 0, "", null, "", buttonColor);
            item2 = new QMSingleButton(menuBase, 2, 0, "", null, "", buttonColor);
            item3 = new QMSingleButton(menuBase, 3, 0, "", null, "", buttonColor);
            item4 = new QMSingleButton(menuBase, 1, 1, "", null, "", buttonColor);
            item5 = new QMSingleButton(menuBase, 2, 1, "", null, "", buttonColor);
            item6 = new QMSingleButton(menuBase, 3, 1, "", null, "", buttonColor);
            item7 = new QMSingleButton(menuBase, 1, 2, "", null, "", buttonColor);
            item8 = new QMSingleButton(menuBase, 2, 2, "", null, "", buttonColor);
            item9 = new QMSingleButton(menuBase, 3, 2, "", null, "", buttonColor);
            toggleItem1 = new QMToggleButton(menuBase, 1, 0, "", null, "", null, "", buttonColor);
            toggleItem2 = new QMToggleButton(menuBase, 2, 0, "", null, "", null, "", buttonColor);
            toggleItem3 = new QMToggleButton(menuBase, 3, 0, "", null, "", null, "", buttonColor);
            toggleItem4 = new QMToggleButton(menuBase, 1, 1, "", null, "", null, "", buttonColor);
            toggleItem5 = new QMToggleButton(menuBase, 2, 1, "", null, "", null, "", buttonColor);
            toggleItem6 = new QMToggleButton(menuBase, 3, 1, "", null, "", null, "", buttonColor);
            toggleItem7 = new QMToggleButton(menuBase, 1, 2, "", null, "", null, "", buttonColor);
            toggleItem8 = new QMToggleButton(menuBase, 2, 2, "", null, "", null, "", buttonColor);
            toggleItem9 = new QMToggleButton(menuBase, 3, 2, "", null, "", null, "", buttonColor);
        }
        public void OpenMenu()
        {
            currentPage = 0;
            try
            {
                UpdateMenu();
            }
            catch (Exception ex)
            {
                //emmVRCLoader.Logger.Log(ex.ToString());
            }
            QuickMenuUtils.ShowQuickmenuPage(menuBase.getMenuName());
        }
        public void UpdateMenu()
        {
            try
            {

                pageCount.setActive(false);
                QMSingleButton[] buttons = { item1, item2, item3, item4, item5, item6, item7, item8, item9 };
                QMToggleButton[] qMToggles = { toggleItem1, toggleItem2, toggleItem3, toggleItem4, toggleItem5, toggleItem6, toggleItem7, toggleItem8, toggleItem9 };
                foreach (QMSingleButton qmbutton in buttons)
                {
                    qmbutton.setActive(false);
                }

                foreach (QMToggleButton qmtoggle in qMToggles)
                {
                    qmtoggle.setActive(false);
                }
                if (currentPage < 0)
                    currentPage = 0;
                if (currentPage > pageItems.Count / 9)
                    currentPage = (int)pageItems.Count / 9;
                if (pageItems.Count > 9)
                {
                    pageCount.setActive(true);
                    pageCount.setButtonText("Page\n" + (currentPage + 1) + " of " + (((int)pageItems.Count / 9) + 1));
                }

                List<PageItem> itemsToAdd = pageItems.GetRange(currentPage * 9, Math.Abs(currentPage * 9 - pageItems.Count));

                if (itemsToAdd == null)
                    emmVRCLoader.Logger.LogError("The page list of items is null. This is a problem.");
                else if (itemsToAdd.Count > 0)
                {
                    if (itemsToAdd[0].type == PageItems.Button)
                    {
                        item1.setButtonText(itemsToAdd[0].Name);
                        item1.setAction(itemsToAdd[0].Action);
                        item1.setToolTip(itemsToAdd[0].Tooltip);
                        if (itemsToAdd[0].Active)
                            item1.setActive(true);
                    }
                    else if (itemsToAdd[0].type == PageItems.Toggle)
                    {
                        toggleItem1.setOnText(itemsToAdd[0].onName);
                        toggleItem1.setOffText(itemsToAdd[0].offName);
                        toggleItem1.setAction(itemsToAdd[0].ButtonAction, itemsToAdd[0].ButtonAction);
                        toggleItem1.setToggleState(itemsToAdd[0].ToggleState);
                        toggleItem1.setToolTip(itemsToAdd[0].Tooltip);
                        if (itemsToAdd[0].Active)
                            toggleItem1.setActive(true);
                    }
                }
                if (itemsToAdd.Count > 1)
                {
                    if (itemsToAdd[1].type == PageItems.Button)
                    {
                        item2.setButtonText(itemsToAdd[1].Name);
                        item2.setAction(itemsToAdd[1].Action);
                        item2.setToolTip(itemsToAdd[1].Tooltip);
                        if (itemsToAdd[1].Active)
                            item2.setActive(true);
                    }
                    else if (itemsToAdd[1].type == PageItems.Toggle)
                    {
                        toggleItem2.setOnText(itemsToAdd[1].onName);
                        toggleItem2.setOffText(itemsToAdd[1].offName);
                        toggleItem2.setAction(itemsToAdd[1].ButtonAction, itemsToAdd[1].ButtonAction);
                        toggleItem2.setToggleState(itemsToAdd[1].ToggleState);
                        toggleItem2.setToolTip(itemsToAdd[1].Tooltip);
                        if (itemsToAdd[1].Active)
                            toggleItem2.setActive(true);
                    }
                }
                if (itemsToAdd.Count > 2)
                {
                    if (itemsToAdd[2].type == PageItems.Button)
                    {
                        item3.setButtonText(itemsToAdd[2].Name);
                        item3.setAction(itemsToAdd[2].Action);
                        item3.setToolTip(itemsToAdd[2].Tooltip);
                        if (itemsToAdd[2].Active)
                            item3.setActive(true);
                    }
                    else if (itemsToAdd[2].type == PageItems.Toggle)
                    {
                        toggleItem3.setOnText(itemsToAdd[2].onName);
                        toggleItem3.setOffText(itemsToAdd[2].offName);
                        toggleItem3.setAction(itemsToAdd[2].ButtonAction, itemsToAdd[2].ButtonAction);
                        toggleItem3.setToggleState(itemsToAdd[2].ToggleState);
                        toggleItem3.setToolTip(itemsToAdd[2].Tooltip);
                        if (itemsToAdd[2].Active)
                            toggleItem3.setActive(true);
                    }
                }
                if (itemsToAdd.Count > 3)
                {
                    if (itemsToAdd[3].type == PageItems.Button)
                    {
                        item4.setButtonText(itemsToAdd[3].Name);
                        item4.setAction(itemsToAdd[3].Action);
                        item4.setToolTip(itemsToAdd[3].Tooltip);
                        if (itemsToAdd[3].Active)
                            item4.setActive(true);
                    }
                    else if (itemsToAdd[3].type == PageItems.Toggle)
                    {
                        toggleItem4.setOnText(itemsToAdd[3].onName);
                        toggleItem4.setOffText(itemsToAdd[3].offName);
                        toggleItem4.setAction(itemsToAdd[3].ButtonAction, itemsToAdd[3].ButtonAction);
                        toggleItem4.setToggleState(itemsToAdd[3].ToggleState);
                        toggleItem4.setToolTip(itemsToAdd[3].Tooltip);
                        if (itemsToAdd[3].Active)
                            toggleItem4.setActive(true);
                    }
                }
                if (itemsToAdd.Count > 4)
                {
                    if (itemsToAdd[4].type == PageItems.Button)
                    {
                        item5.setButtonText(itemsToAdd[4].Name);
                        item5.setAction(itemsToAdd[4].Action);
                        item5.setToolTip(itemsToAdd[4].Tooltip);
                        if (itemsToAdd[4].Active)
                            item5.setActive(true);
                    }
                    else if (itemsToAdd[4].type == PageItems.Toggle)
                    {
                        toggleItem5.setOnText(itemsToAdd[4].onName);
                        toggleItem5.setOffText(itemsToAdd[4].offName);
                        toggleItem5.setAction(itemsToAdd[4].ButtonAction, itemsToAdd[4].ButtonAction);
                        toggleItem5.setToggleState(itemsToAdd[4].ToggleState);
                        toggleItem5.setToolTip(itemsToAdd[4].Tooltip);
                        if (itemsToAdd[4].Active)
                            toggleItem5.setActive(true);
                    }
                }
                if (itemsToAdd.Count > 5)
                {
                    if (itemsToAdd[5].type == PageItems.Button)
                    {
                        item6.setButtonText(itemsToAdd[5].Name);
                        item6.setAction(itemsToAdd[5].Action);
                        item6.setToolTip(itemsToAdd[5].Tooltip);
                        if (itemsToAdd[5].Active)
                            item6.setActive(true);
                    }
                    else if (itemsToAdd[5].type == PageItems.Toggle)
                    {
                        toggleItem6.setOnText(itemsToAdd[5].onName);
                        toggleItem6.setOffText(itemsToAdd[5].offName);
                        toggleItem6.setAction(itemsToAdd[5].ButtonAction, itemsToAdd[5].ButtonAction);
                        toggleItem6.setToggleState(itemsToAdd[5].ToggleState);
                        toggleItem6.setToolTip(itemsToAdd[5].Tooltip);
                        if (itemsToAdd[5].Active)
                            toggleItem6.setActive(true);
                    }
                }
                if (itemsToAdd.Count > 6)
                {
                    if (itemsToAdd[6].type == PageItems.Button)
                    {
                        item7.setButtonText(itemsToAdd[6].Name);
                        item7.setAction(itemsToAdd[6].Action);
                        item7.setToolTip(itemsToAdd[6].Tooltip);
                        if (itemsToAdd[6].Active)
                            item7.setActive(true);
                    }
                    else if (itemsToAdd[6].type == PageItems.Toggle)
                    {
                        toggleItem7.setOnText(itemsToAdd[6].onName);
                        toggleItem7.setOffText(itemsToAdd[6].offName);
                        toggleItem7.setAction(itemsToAdd[6].ButtonAction, itemsToAdd[6].ButtonAction);
                        toggleItem7.setToggleState(itemsToAdd[6].ToggleState);
                        toggleItem7.setToolTip(itemsToAdd[6].Tooltip);
                        if (itemsToAdd[6].Active)
                            toggleItem7.setActive(true);
                    }
                }
                if (itemsToAdd.Count > 7)
                {
                    if (itemsToAdd[7].type == PageItems.Button)
                    {
                        item8.setButtonText(itemsToAdd[7].Name);
                        item8.setAction(itemsToAdd[7].Action);
                        item8.setToolTip(itemsToAdd[7].Tooltip);
                        if (itemsToAdd[7].Active)
                            item8.setActive(true);
                    }
                    else if (itemsToAdd[7].type == PageItems.Toggle)
                    {
                        toggleItem8.setOnText(itemsToAdd[7].onName);
                        toggleItem8.setOffText(itemsToAdd[7].offName);
                        toggleItem8.setAction(itemsToAdd[7].ButtonAction, itemsToAdd[7].ButtonAction);
                        toggleItem8.setToggleState(itemsToAdd[7].ToggleState);
                        toggleItem8.setToolTip(itemsToAdd[7].Tooltip);
                        if (itemsToAdd[7].Active)
                            toggleItem8.setActive(true);
                    }
                }
                if (itemsToAdd.Count > 8)
                {
                    if (itemsToAdd[8].type == PageItems.Button)
                    {
                        item9.setButtonText(itemsToAdd[8].Name);
                        item9.setAction(itemsToAdd[8].Action);
                        item9.setToolTip(itemsToAdd[8].Tooltip);
                        if (itemsToAdd[8].Active)
                            item9.setActive(true);
                    }
                    else if (itemsToAdd[8].type == PageItems.Toggle)
                    {
                        toggleItem9.setOnText(itemsToAdd[8].onName);
                        toggleItem9.setOffText(itemsToAdd[8].offName);
                        toggleItem9.setAction(itemsToAdd[8].ButtonAction, itemsToAdd[8].ButtonAction);
                        toggleItem9.setToggleState(itemsToAdd[8].ToggleState);
                        toggleItem9.setToolTip(itemsToAdd[8].Tooltip);
                        if (itemsToAdd[8].Active)
                            toggleItem9.setActive(true);
                    }
                }
                /*for (int i = 0; i < itemsToAdd.Count; i++)
                {
                    {
                        if (itemsToAdd[i].type == PageItems.Button)
                        {
                            buttons[i].setButtonText(itemsToAdd[i].Name);
                            buttons[i].setAction(itemsToAdd[i].Action);
                            buttons[i].setToolTip(itemsToAdd[i].Tooltip);
                            if (itemsToAdd[i].Active)
                                buttons[i].setActive(true);
                        }
                        else if (itemsToAdd[i].type == PageItems.Toggle)
                        {
                            qMToggles[i].setOnText(itemsToAdd[i].onName);
                            qMToggles[i].setOffText(itemsToAdd[i].offName);
                            qMToggles[i].setAction(() => { itemsToAdd[i].ButtonAction(); }, () => { itemsToAdd[i].ButtonAction(); });
                            qMToggles[i].setToggleState(itemsToAdd[i].ToggleState);
                            emmVRCLoader.Logger.Log(i.ToString());
                            emmVRCLoader.Logger.Log(itemsToAdd.Count.ToString());
                            qMToggles[i].setToolTip(itemsToAdd[i].Tooltip);
                            if (itemsToAdd[i].Active)
                                qMToggles[i].setActive(true);
                        }
                    }
                }*/

                if (currentPage > pageTitles.Count && menuTitle != null)
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
            catch (Exception ex)
            {
                emmVRCLoader.Logger.LogError("Error with paginated menu: " + ex);
            }
        }
    }
}
