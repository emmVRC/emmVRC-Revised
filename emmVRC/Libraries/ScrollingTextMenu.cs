using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace emmVRC.Libraries
{
    public class ScrollingTextMenu
    {
        public QMSingleButton menuEntryButton { get; set; }
        public QMNestedButton menuBase;
        public QMSingleButton actionButton;
        private QMSingleButton upButton;
        private QMSingleButton downButton;

        private GameObject console = null;
        private GameObject menuTitle = null;
        public Text menuTitleText = null;
        private GameObject baseText = null;
        public Text baseTextText = null;

        public ScrollingTextMenu(string parentPath, int x, int y, string menuName, string menuTooltip, Color? buttonColor, string titleText, string bodyText, string buttonText = "", Action buttonAction = null, string buttonTooltip = "")
        {
            menuBase = new QMNestedButton(parentPath, x, y, menuName, "");
            menuBase.getMainButton().DestroyMe();
            menuBase.getBackButton().setAction(() => { QuickMenuUtils.ShowQuickmenuPage("ShortcutMenu"); });

            menuEntryButton = new QMSingleButton(parentPath, x, y, menuName, this.OpenMenu, menuTooltip, buttonColor);

            console = GameObject.Instantiate(QuickMenuUtils.GetQuickMenuInstance().transform.Find("AvatarStatsMenu/_Console").gameObject, this.menuBase.getBackButton().getGameObject().transform.parent);

            menuTitle = console.transform.Find("_Header/Text_Overall").gameObject;
            menuTitleText = menuTitle.GetComponent<Text>();
            menuTitleText.color = Color.white;
            menuTitleText.text = titleText;
            menuTitleText.supportRichText = true;
            menuTitleText.rectTransform.anchoredPosition -= new Vector2(100f, 0f);
            menuTitleText.rectTransform.offsetMax = new Vector2(900f, 0f);
            console.transform.Find("_Header/Text_Rating").localScale = Vector3.zero;
            console.transform.Find("_Header/InfoIcon").localScale = Vector3.zero;

            baseText = GameObject.Instantiate(QuickMenuUtils.GetQuickMenuInstance().transform.Find("QuickMenu_NewElements/_InfoBar/EarlyAccessText").gameObject, console.transform.Find("_StatsConsole/Viewport/Content"));
            baseTextText = baseText.GetComponent<Text>();
            baseTextText.rectTransform.anchoredPosition = new Vector2(0, 0);
            baseTextText.fontStyle = FontStyle.Normal;
            baseTextText.alignment = TextAnchor.UpperLeft;
            baseTextText.color = Color.white;
            baseTextText.text = bodyText;
            baseTextText.supportRichText = true;
            baseTextText.horizontalOverflow = HorizontalWrapMode.Wrap;
            baseTextText.fontSize = (int)(baseText.GetComponent<Text>().fontSize / 1.25);
            baseTextText.rectTransform.offsetMax = new Vector2(825f, -10f);
            

            upButton = new QMSingleButton(menuBase, 4, 0, "", () =>
            {
                console.transform.Find("_StatsConsole").GetComponent<ScrollRect>().velocity -= new Vector2(0f, 1000f);
            }, "Scrolls the text view up");
            upButton.getGameObject().GetComponent<Image>().sprite = QuickMenuUtils.GetQuickMenuInstance().transform.Find("EmojiMenu/PageUp").GetComponent<Image>().sprite;
            downButton = new QMSingleButton(menuBase, 4, (buttonAction == null ? 2 : 1), "", () =>
            {
                console.transform.Find("_StatsConsole").GetComponent<ScrollRect>().velocity += new Vector2(0f, 1000f);
            }, "Scrolls the text view down");
            downButton.getGameObject().GetComponent<Image>().sprite = QuickMenuUtils.GetQuickMenuInstance().transform.Find("EmojiMenu/PageDown").GetComponent<Image>().sprite;

            if (buttonAction != null)
            {
                actionButton = new QMSingleButton(menuBase, 2, 2, buttonText, buttonAction, buttonTooltip);
                actionButton.getGameObject().GetComponent<RectTransform>().sizeDelta *= new Vector2(4f, 0.75f);
                actionButton.getGameObject().GetComponent<RectTransform>().anchoredPosition += new Vector2(200f, -50f);
                actionButton.getGameObject().GetComponentInChildren<Text>().fontSize += 2;
            }
            if (buttonAction == null)
            {
                console.GetComponent<RectTransform>().sizeDelta += new Vector2(0f, 300f);
                console.transform.Find("_StatsConsole").GetComponent<RectTransform>().sizeDelta += new Vector2(0f, 250f);
            }
        }

        public ScrollingTextMenu(QMNestedButton menuButton, int x, int y, string menuName, string menuTooltip, Color? buttonColor, string titleText, string bodyText, string buttonText = "", Action buttonAction = null, string buttonTooltip = "") : this(menuButton.getMenuName(), x, y, menuName, menuTooltip, buttonColor, titleText, bodyText, buttonText, buttonAction, buttonTooltip) { }
        public void OpenMenu()
        {
            QuickMenuUtils.ShowQuickmenuPage(menuBase.getMenuName());
            //console.transform.Find("_StatsConsole").GetComponent<ScrollRect>().normalizedPosition = Vector2.zero;
        }
    }
}
