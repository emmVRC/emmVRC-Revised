using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace emmVRC.Libraries
{
    public class ButtonConfigurationMenu
    {
        public Action<Vector2> acceptAction;
        public Action<ButtonConfigurationMenu> openAction;

        public string menuTitle;

        private QMNestedButton baseMenu;
        private QMSingleButton Button_0_N1;
        private QMSingleButton Button_5_N1;
        private QMSingleButton Button_0_0;
        private QMSingleButton Button_1_0;
        private QMSingleButton Button_2_0;
        private QMSingleButton Button_3_0;
        private QMSingleButton Button_4_0;
        private QMSingleButton Button_5_0;
        private QMSingleButton Button_0_1;
        private QMSingleButton Button_1_1;
        private QMSingleButton Button_2_1;
        private QMSingleButton Button_3_1;
        private QMSingleButton Button_4_1;
        private QMSingleButton Button_5_1;
        private QMSingleButton Button_1_2;
        private QMSingleButton Button_2_2;
        private QMSingleButton Button_3_2;
        private QMSingleButton Button_4_2;
        private QMSingleButton Button_5_2;

        public ButtonConfigurationMenu(string baseMenuName, int x, int y, string menuName, Action<ButtonConfigurationMenu> openAction , Action<Vector2> acceptAction, string tooltip, string menuTitle, List<KeyValuePair<string, Vector2>> disabledButtons = null)
        {
            baseMenu = new QMNestedButton(baseMenuName, x, y, menuName, tooltip);


            Button_0_N1 = new QMSingleButton(baseMenu, 0, -1, "", () => { acceptAction.Invoke(new Vector2(0, -1)); }, "Button Position: 0, -1");
            Button_5_N1 = new QMSingleButton(baseMenu, 5, -1, "", () => { acceptAction.Invoke(new Vector2(5, -1)); }, "Button Position: 5, -1");
            Button_0_0 = new QMSingleButton(baseMenu, 0, 0, "", () => { acceptAction.Invoke(new Vector2(0, 0)); }, "Button Position: 0, 0");
            Button_1_0 = new QMSingleButton(baseMenu, 1, 0, "", () => { acceptAction.Invoke(new Vector2(1, 0)); }, "Button Position: 1, 0");
            Button_2_0 = new QMSingleButton(baseMenu, 2, 0, "", () => { acceptAction.Invoke(new Vector2(2, 0)); }, "Button Position: 2, 0");
            Button_3_0 = new QMSingleButton(baseMenu, 3, 0, "", () => { acceptAction.Invoke(new Vector2(3, 0)); }, "Button Position: 3, 0");
            Button_4_0 = new QMSingleButton(baseMenu, 4, 0, "", () => { acceptAction.Invoke(new Vector2(4, 0)); }, "Button Position: 4, 0");
            Button_5_0 = new QMSingleButton(baseMenu, 5, 0, "", () => { acceptAction.Invoke(new Vector2(5, 0)); }, "Button Position: 5, 0");
            Button_0_1 = new QMSingleButton(baseMenu, 0, 1, "", () => { acceptAction.Invoke(new Vector2(0, 1)); }, "Button Position: 0, 1");
            Button_1_1 = new QMSingleButton(baseMenu, 1, 1, "", () => { acceptAction.Invoke(new Vector2(1, 1)); }, "Button Position: 1, 1");
            Button_2_1 = new QMSingleButton(baseMenu, 2, 1, "", () => { acceptAction.Invoke(new Vector2(2, 1)); }, "Button Position: 2, 1");
            Button_3_1 = new QMSingleButton(baseMenu, 3, 1, "", () => { acceptAction.Invoke(new Vector2(3, 1)); }, "Button Position: 3, 1");
            Button_4_1 = new QMSingleButton(baseMenu, 4, 1, "", () => { acceptAction.Invoke(new Vector2(4, 1)); }, "Button Position: 4, 1");
            Button_5_1 = new QMSingleButton(baseMenu, 5, 1, "", () => { acceptAction.Invoke(new Vector2(5, 1)); }, "Button Position: 5, 1");
            Button_1_2 = new QMSingleButton(baseMenu, 1, 2, "", () => { acceptAction.Invoke(new Vector2(1, 2)); }, "Button Position: 1, 2");
            Button_2_2 = new QMSingleButton(baseMenu, 2, 2, "", () => { acceptAction.Invoke(new Vector2(2, 2)); }, "Button Position: 2, 2");
            Button_3_2 = new QMSingleButton(baseMenu, 3, 2, "", () => { acceptAction.Invoke(new Vector2(3, 2)); }, "Button Position: 3, 2");
            Button_4_2 = new QMSingleButton(baseMenu, 4, 2, "", () => { acceptAction.Invoke(new Vector2(4, 2)); }, "Button Position: 4, 2");
            Button_5_2 = new QMSingleButton(baseMenu, 5, 2, "", () => { acceptAction.Invoke(new Vector2(5, 2)); }, "Button Position: 5, 2");

            baseMenu.getBackButton().DestroyMe();
            this.openAction = openAction;
            this.acceptAction = acceptAction;

            this.menuTitle = menuTitle;

            if (disabledButtons != null)
            {
                ChangeDisabledButtons(disabledButtons);
            }
            baseMenu.getMainButton().setAction(OpenMenu);
        }
        public void OpenMenu()
        {
            openAction.Invoke(this);
            QuickMenuUtils.ShowQuickmenuPage(baseMenu.getMenuName());
            QuickMenuUtils.GetQuickMenuInstance().SetQuickMenuContext(QuickMenuContextualDisplay.EnumNPublicSealedvaUnNoToUs7vUsNoUnique.Notification, null, menuTitle);
        }
        public void ChangeDisabledButtons(List<KeyValuePair<string, Vector2>> newDisabledButtons)
        {
            Button_0_N1.getGameObject().GetComponent<Button>().enabled = true;
            Button_5_N1.getGameObject().GetComponent<Button>().enabled = true;
            Button_0_0.getGameObject().GetComponent<Button>().enabled = true;
            Button_1_0.getGameObject().GetComponent<Button>().enabled = true;
            Button_2_0.getGameObject().GetComponent<Button>().enabled = true;
            Button_3_0.getGameObject().GetComponent<Button>().enabled = true;
            Button_4_0.getGameObject().GetComponent<Button>().enabled = true;
            Button_5_0.getGameObject().GetComponent<Button>().enabled = true;
            Button_0_1.getGameObject().GetComponent<Button>().enabled = true;
            Button_1_1.getGameObject().GetComponent<Button>().enabled = true;
            Button_2_1.getGameObject().GetComponent<Button>().enabled = true;
            Button_3_1.getGameObject().GetComponent<Button>().enabled = true;
            Button_4_1.getGameObject().GetComponent<Button>().enabled = true;
            Button_5_1.getGameObject().GetComponent<Button>().enabled = true;
            Button_1_2.getGameObject().GetComponent<Button>().enabled = true;
            Button_2_2.getGameObject().GetComponent<Button>().enabled = true;
            Button_3_2.getGameObject().GetComponent<Button>().enabled = true;
            Button_4_2.getGameObject().GetComponent<Button>().enabled = true;
            Button_5_2.getGameObject().GetComponent<Button>().enabled = true;
            Button_0_N1.setButtonText("");
            Button_5_N1.setButtonText("");
            Button_0_0.setButtonText("");
            Button_1_0.setButtonText("");
            Button_2_0.setButtonText("");
            Button_3_0.setButtonText("");
            Button_4_0.setButtonText("");
            Button_5_0.setButtonText("");
            Button_0_1.setButtonText("");
            Button_1_1.setButtonText("");
            Button_2_1.setButtonText("");
            Button_3_1.setButtonText("");
            Button_4_1.setButtonText("");
            Button_5_1.setButtonText("");
            Button_1_2.setButtonText("");
            Button_2_2.setButtonText("");
            Button_3_2.setButtonText("");
            Button_4_2.setButtonText("");
            Button_5_2.setButtonText("");
            foreach (KeyValuePair<string, Vector2> disablButt in newDisabledButtons)
            {
                if (disablButt.Value == new Vector2(0, -1))
                {
                    Button_0_N1.setButtonText(disablButt.Key);
                    Button_0_N1.getGameObject().GetComponent<Button>().enabled = false;
                }
                if (disablButt.Value == new Vector2(5, -1))
                {
                    Button_5_N1.setButtonText(disablButt.Key);
                    Button_5_N1.getGameObject().GetComponent<Button>().enabled = false;
                }
                if (disablButt.Value == new Vector2(0, 0))
                {
                    Button_0_0.setButtonText(disablButt.Key);
                    Button_0_0.getGameObject().GetComponent<Button>().enabled = false;
                }
                if (disablButt.Value == new Vector2(1, 0))
                {
                    Button_1_0.setButtonText(disablButt.Key);
                    Button_1_0.getGameObject().GetComponent<Button>().enabled = false;
                }
                if (disablButt.Value == new Vector2(2, 0))
                {
                    Button_2_0.setButtonText(disablButt.Key);
                    Button_2_0.getGameObject().GetComponent<Button>().enabled = false;
                }
                if (disablButt.Value == new Vector2(3, 0))
                {
                    Button_3_0.setButtonText(disablButt.Key);
                    Button_3_0.getGameObject().GetComponent<Button>().enabled = false;
                }
                if (disablButt.Value == new Vector2(4, 0))
                {
                    Button_4_0.setButtonText(disablButt.Key);
                    Button_4_0.getGameObject().GetComponent<Button>().enabled = false;
                }
                if (disablButt.Value == new Vector2(5, 0))
                {
                    Button_5_0.setButtonText(disablButt.Key);
                    Button_5_0.getGameObject().GetComponent<Button>().enabled = false;
                }
                if (disablButt.Value == new Vector2(0, 1))
                {
                    Button_0_1.setButtonText(disablButt.Key);
                    Button_0_1.getGameObject().GetComponent<Button>().enabled = false;
                }
                if (disablButt.Value == new Vector2(1, 1))
                {
                    Button_1_1.setButtonText(disablButt.Key);
                    Button_1_1.getGameObject().GetComponent<Button>().enabled = false;
                }
                if (disablButt.Value == new Vector2(2, 1))
                {
                    Button_2_1.setButtonText(disablButt.Key);
                    Button_2_1.getGameObject().GetComponent<Button>().enabled = false;
                }
                if (disablButt.Value == new Vector2(3, 1))
                {
                    Button_3_1.setButtonText(disablButt.Key);
                    Button_3_1.getGameObject().GetComponent<Button>().enabled = false;
                }
                if (disablButt.Value == new Vector2(4, 1))
                {
                    Button_4_1.setButtonText(disablButt.Key);
                    Button_4_1.getGameObject().GetComponent<Button>().enabled = false;
                }
                if (disablButt.Value == new Vector2(5, 1))
                {
                    Button_5_1.setButtonText(disablButt.Key);
                    Button_5_1.getGameObject().GetComponent<Button>().enabled = false;
                }
                if (disablButt.Value == new Vector2(1, 2))
                {
                    Button_1_2.setButtonText(disablButt.Key);
                    Button_1_2.getGameObject().GetComponent<Button>().enabled = false;
                }
                if (disablButt.Value == new Vector2(2, 2))
                {
                    Button_2_2.setButtonText(disablButt.Key);
                    Button_2_2.getGameObject().GetComponent<Button>().enabled = false;
                }
                if (disablButt.Value == new Vector2(3, 2))
                {
                    Button_3_2.setButtonText(disablButt.Key);
                    Button_3_2.getGameObject().GetComponent<Button>().enabled = false;
                }
                if (disablButt.Value == new Vector2(4, 2))
                {
                    Button_4_2.setButtonText(disablButt.Key);
                    Button_4_2.getGameObject().GetComponent<Button>().enabled = false;
                }
                if (disablButt.Value == new Vector2(5, 2))
                {
                    Button_5_2.setButtonText(disablButt.Key);
                    Button_5_2.getGameObject().GetComponent<Button>().enabled = false;
                }
            }
        }
    }
}
