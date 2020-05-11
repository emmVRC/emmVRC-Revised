﻿using System;
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
        public static QMSingleButton ReloadWorldButton;
        public static QMToggleButton PortalBlockToggle;
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
            ReloadWorldButton = new QMSingleButton(baseMenu, 3, 0, "Reload\nWorld", () => { new PortalInternal().Method_Private_Void_String_String_0(RoomManager.field_Internal_Static_ApiWorld_0.id, RoomManager.field_Internal_Static_ApiWorldInstance_0.idWithTags); }, "Loads the current instance again");
        }
    }
}
