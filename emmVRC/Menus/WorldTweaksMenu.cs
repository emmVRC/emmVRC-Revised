using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using emmVRC.Libraries;
using UnityEngine;
using UnityEngine.UI;
namespace emmVRC.Menus
{
    public class WorldTweaksMenu
    {
        public static QMNestedButton baseMenu;
        public static QMSingleButton OptimizeMirrorsButton;
        public static QMSingleButton BeautifyMirrorsButton;
        public static QMSingleButton RevertMirrorsButton;
        public static QMSingleButton OptimizePedestalsButton;
        public static QMSingleButton RevertPedestalsButton;
        public static QMSingleButton ReloadWorldButton;
        public static QMToggleButton PortalBlockToggle;
        public static QMSingleButton DeletePortalsButton;
        public static QMToggleButton ChairBlockToggle;
        public static void Initialize()
        {
            baseMenu = new QMNestedButton(FunctionsMenu.baseMenu.menuBase, 192384, 129302, "World\nTweaks", "");
            baseMenu.getMainButton().DestroyMe();
            OptimizeMirrorsButton = new QMSingleButton(baseMenu, 1, 0, "Optimize", () => { Hacks.MirrorTweaks.Optimize(); }, "Sets the mirrors around you to only reflect the bare necessities, for optimization");
            OptimizeMirrorsButton.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1f, 2.0175f);
            OptimizeMirrorsButton.getGameObject().GetComponent<RectTransform>().anchoredPosition += new Vector2(0f, 96f);
            BeautifyMirrorsButton = new QMSingleButton(baseMenu, 1, 0, "Beautify", () => { Hacks.MirrorTweaks.Beautify(); }, "Sets the mirrors around you to reflect absolutely everything");
            BeautifyMirrorsButton.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1f, 2.0175f);
            BeautifyMirrorsButton.getGameObject().GetComponent<RectTransform>().anchoredPosition += new Vector2(0f, -96f);
            RevertMirrorsButton = new QMSingleButton(baseMenu, 2, 0, "Revert\nMirrors", () => { Hacks.MirrorTweaks.Revert(); }, "Reverts the mirrors in the world to their default reflections");

            OptimizePedestalsButton = new QMSingleButton(baseMenu, 1, 1, "Disable\nPedestals", () => { Hacks.PedestalTweaks.Disable(); }, "Disables all the pedestals in the world, for optimization");
            RevertPedestalsButton = new QMSingleButton(baseMenu, 2, 1, "Enable\nPedestals", () => { Hacks.PedestalTweaks.Revert(); }, "Reverts the pedestals in the world to their default visibility");

            ReloadWorldButton = new QMSingleButton(baseMenu, 3, 0, "Reload\nWorld", () => { new PortalInternal().Method_Private_Void_String_String_PDM_0(RoomManager.field_Internal_Static_ApiWorld_0.id, RoomManager.field_Internal_Static_ApiWorldInstance_0.idWithTags); }, "Loads the current instance again");
            PortalBlockToggle = new QMToggleButton(baseMenu, 4, 0, "Portals On", () => { Configuration.JSONConfig.PortalBlockingEnable = false; Configuration.SaveConfig(); }, "Portals Off", () => { Configuration.JSONConfig.PortalBlockingEnable = true; Configuration.SaveConfig(); }, "TOGGLE: Enables or disables portals in the current world", null, null, false, true);
            PortalBlockToggle.setToggleState(!Configuration.JSONConfig.PortalBlockingEnable);
            ChairBlockToggle = new QMToggleButton(baseMenu, 4, 1, "Chairs On", () => { Configuration.JSONConfig.ChairBlockingEnable = false; Configuration.SaveConfig(); }, "Chairs Off", () => { Configuration.JSONConfig.ChairBlockingEnable = true; Configuration.SaveConfig(); }, "TOGGLE: Enables or disables chairs in the current world", null, null, false, true);
            ChairBlockToggle.setToggleState(!Configuration.JSONConfig.ChairBlockingEnable);
        }
    }
}
