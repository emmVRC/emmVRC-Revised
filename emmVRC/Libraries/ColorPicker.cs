using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace emmVRC.Libraries
{
    public class ColorPicker
    {
        internal Objects.Slider r;
        internal Objects.Slider g;
        internal Objects.Slider b;
        internal QMSingleButton rButton;
        internal QMSingleButton gButton;
        internal QMSingleButton bButton;
        internal float rValue = 0f;
        internal float gValue = 0f;
        internal float bValue = 0f;

        internal QMNestedButton baseMenu;
        internal QMSingleButton preview;
        internal QMSingleButton acceptButton;
        internal QMSingleButton cancelButton;
        public ColorPicker(string parentMenu, int x, int y, string menuName, string menuTooltip, System.Action<UnityEngine.Color> accept, System.Action cancel, UnityEngine.Color? defaultColor = null)
        {
            baseMenu = new QMNestedButton(parentMenu, x, y, menuName, menuTooltip);
            baseMenu.getBackButton().DestroyMe();

            preview = new QMSingleButton(baseMenu, 4, 0, "Preview", null, "Preview of the color selected");
            preview.getGameObject().name = "ColorPickPreviewButton";

            acceptButton = new QMSingleButton(baseMenu, 4, 1, "Accept", ()=> { accept.Invoke(new UnityEngine.Color(rValue, gValue, bValue)); }, "Accept the color changes");
            cancelButton = new QMSingleButton(baseMenu, 4, 2, "Cancel", () => { cancel.Invoke(); }, "Cancels the color changes");

            rButton = new QMSingleButton(baseMenu, 1, 0, "R\n1f", null, "Float value for Red", UnityEngine.Color.red);
            rButton.getGameObject().name = "rColorButton";
            gButton = new QMSingleButton(baseMenu, 1, 1, "G\n1f", null, "Float value for Green", UnityEngine.Color.green);
            gButton.getGameObject().name = "gColorButton";
            bButton = new QMSingleButton(baseMenu, 1, 2, "B\n1f", null, "Float value for Blue", UnityEngine.Color.blue);
            bButton.getGameObject().name = "bColorButton";

            r = new Objects.Slider(baseMenu.getMenuName(), 2, 0, (float val) => 
            {
                rValue = val;
                preview.setBackgroundColor(new UnityEngine.Color(rValue, gValue, bValue));
                preview.setTextColor(new UnityEngine.Color(rValue, gValue, bValue));
                rButton.setButtonText("R\n" + rValue.ToString("0.00"));
                gButton.setButtonText("G\n" + gValue.ToString("0.00"));
                bButton.setButtonText("B\n" + bValue.ToString("0.00"));
            });
            g = new Objects.Slider(baseMenu.getMenuName(), 2, 1, (float val) =>
            {
                gValue = val;
                preview.setBackgroundColor(new UnityEngine.Color(rValue, gValue, bValue));
                preview.setTextColor(new UnityEngine.Color(rValue, gValue, bValue));
                rButton.setButtonText("R\n" + rValue.ToString("0.00"));
                gButton.setButtonText("G\n" + gValue.ToString("0.00"));
                bButton.setButtonText("B\n" + bValue.ToString("0.00"));
            });
            b = new Objects.Slider(baseMenu.getMenuName(), 2, 2, (float val) =>
            {
                bValue = val;
                preview.setBackgroundColor(new UnityEngine.Color(rValue, gValue, bValue));
                preview.setTextColor(new UnityEngine.Color(rValue, gValue, bValue));
                rButton.setButtonText("R\n" + rValue.ToString("0.00"));
                gButton.setButtonText("G\n" + gValue.ToString("0.00"));
                bButton.setButtonText("B\n" + bValue.ToString("0.00"));
            });

        }
    }
}
