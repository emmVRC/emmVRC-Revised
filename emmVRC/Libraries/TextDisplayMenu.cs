using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


namespace emmVRC.Libraries
{
    public class TextDisplayMenu
    {
        public QMSingleButton menuEntryButton { get; set; }
        public string menuName;
        public QMNestedButton menuBase;
        public QMSingleButton actionButton;

        private GameObject menuTitle = null;
        private GameObject baseText = null;

        public TextDisplayMenu(string parentPath, int x, int y, string menuName, string menuTooltip, Color? buttonColor, string titleText, string bodyText, string buttonText = "", Action buttonAction = null, string buttonTooltip = "")
        {
            menuBase = new QMNestedButton(parentPath, x, y, menuName, "");
            menuBase.getMainButton().DestroyMe();
            menuBase.getBackButton().setAction(() => { QuickMenuUtils.ShowQuickmenuPage("ShortcutMenu"); });

            menuEntryButton = new QMSingleButton(parentPath, x, y, menuName,  this.OpenMenu, menuTooltip, buttonColor);

            menuTitle = GameObject.Instantiate(QuickMenuUtils.GetQuickMenuInstance().transform.Find("QuickMenu_NewElements/_InfoBar/EarlyAccessText").gameObject, this.menuBase.getBackButton().getGameObject().transform.parent);
            menuTitle.GetComponent<Text>().fontStyle = FontStyle.Normal;
            menuTitle.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
            menuTitle.GetComponent<Text>().color = Color.white;
            menuTitle.GetComponent<Text>().text = titleText;
            menuTitle.GetComponent<RectTransform>().anchoredPosition += new Vector2(580f, -495f);
            

            baseText = GameObject.Instantiate(QuickMenuUtils.GetQuickMenuInstance().transform.Find("QuickMenu_NewElements/_InfoBar/EarlyAccessText").gameObject, this.menuBase.getBackButton().getGameObject().transform.parent);
            baseText.GetComponent<Text>().fontStyle = FontStyle.Normal;
            baseText.GetComponent<Text>().alignment = TextAnchor.UpperLeft;
            baseText.GetComponent<Text>().color = Color.white;
            baseText.GetComponent<Text>().text = bodyText;
            baseText.GetComponent<Text>().fontSize = (int)(baseText.GetComponent<Text>().fontSize / 1.25);
            baseText.GetComponent<RectTransform>().anchoredPosition += new Vector2(25f, -620f);
            
            if (buttonAction != null)
            {
                actionButton = new QMSingleButton(menuBase, 2, 2, buttonText, buttonAction, buttonTooltip);
                actionButton.getGameObject().GetComponent<RectTransform>().sizeDelta *= new Vector2(4f, 1f);
                actionButton.getGameObject().GetComponent<RectTransform>().anchoredPosition += new Vector2(200f, 0f);
                actionButton.getGameObject().GetComponentInChildren<Text>().fontSize += 2;
            }
        }

        public TextDisplayMenu(QMNestedButton menuButton, int x, int y, string menuName, string menuTooltip, Color? buttonColor, string titleText, string bodyText, string buttonText = "", Action buttonAction = null, string buttonTooltip = "") : this(menuButton.getMenuName(), x, y, menuName, menuTooltip, buttonColor, titleText, bodyText, buttonText, buttonAction, buttonTooltip) { }
        public void OpenMenu()
        {
            UpdateMenu();
            QuickMenuUtils.ShowQuickmenuPage(menuBase.getMenuName());
        }
        public void UpdateMenu()
        {
        }
    }
}
