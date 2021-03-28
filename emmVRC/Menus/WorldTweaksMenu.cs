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

        public static QMToggleButton ItemESPToggle;
        public static QMToggleButton TriggerESPToggle;
        public static QMToggleButton VideoPlayerToggle;
        public static QMToggleButton VRCPickupToggle;
        public static QMToggleButton PickupObjectToggle;
        public static QMToggleButton AvatarPedestalToggle;

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

            //OptimizePedestalsButton = new QMSingleButton(baseMenu, 1, 1, "Disable\nPedestals", () => { Hacks.PedestalTweaks.Disable(); }, "Disables all the pedestals in the world, for optimization");
            //OptimizePedestalsButton.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1f, 2.0175f);
            //OptimizePedestalsButton.getGameObject().GetComponent<RectTransform>().anchoredPosition += new Vector2(0f, 96f);
            //RevertPedestalsButton = new QMSingleButton(baseMenu, 2, 1, "Enable\nPedestals", () => { Hacks.PedestalTweaks.Revert(); }, "Reverts the pedestals in the world to their default visibility");
            //RevertPedestalsButton = new QMSingleButton(baseMenu, 1, 1, "Enable\nPedestals", () => { Hacks.PedestalTweaks.Revert(); }, "Reverts the pedestals in the world to their default visibility");
            //RevertPedestalsButton.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1f, 2.0175f);
            //RevertPedestalsButton.getGameObject().GetComponent<RectTransform>().anchoredPosition += new Vector2(0f, -96f);


            ReloadWorldButton = new QMSingleButton(baseMenu, 3, 0, "Reload\nWorld", () => { new PortalInternal().Method_Private_Void_String_String_PDM_0(RoomManager.field_Internal_Static_ApiWorld_0.id, RoomManager.field_Internal_Static_ApiWorldInstance_0.idWithTags); }, "Loads the current instance again");
            PortalBlockToggle = new QMToggleButton(baseMenu, 4, 0, "Portals On", () => { Configuration.JSONConfig.PortalBlockingEnable = false; Configuration.SaveConfig(); }, "Portals Off", () => { Configuration.JSONConfig.PortalBlockingEnable = true; Configuration.SaveConfig(); }, "TOGGLE: Enables or disables portals in the current world", null, null, false, true);
            PortalBlockToggle.setToggleState(!Configuration.JSONConfig.PortalBlockingEnable);
            ChairBlockToggle = new QMToggleButton(baseMenu, 4, 1, "Chairs On", () => { Configuration.JSONConfig.ChairBlockingEnable = false; Configuration.SaveConfig(); }, "Chairs Off", () => { Configuration.JSONConfig.ChairBlockingEnable = true; Configuration.SaveConfig(); }, "TOGGLE: Enables or disables chairs in the current world", null, null, false, true);
            ChairBlockToggle.setToggleState(!Configuration.JSONConfig.ChairBlockingEnable);

            #region Korty's Addons
            ItemESPToggle = new QMToggleButton(baseMenu, 1, 1, "Item ESP", () =>
            {
                Hacks.WorldFunctions.ToggleItemESP(true);
            }, "Disabled", () =>
            {
                Hacks.WorldFunctions.ToggleItemESP(false);
            }, "TOGGLE: An Outline around all Pickup-able items in the world");

            TriggerESPToggle = new QMToggleButton(baseMenu, 2, 1, "Trigger ESP", () =>
            {
                Hacks.WorldFunctions.ToggleTriggerESP(true);
            }, "Disabled", () =>
            {
                Hacks.WorldFunctions.ToggleTriggerESP(false);
            }, "TOGGLE: An Outline around all triggers in the world\n(Some may not show due to area triggers blocking anything being behide larger area triggers)");

            VRCPickupToggle = new QMToggleButton(baseMenu, 1, 2, "Enable\nItem Pickup", () =>
            {
                Hacks.ComponentToggle.pickupable = true;
                Hacks.ComponentToggle.Toggle();

            }, "Disabled", () =>
            {
                Hacks.ComponentToggle.pickupable = false;
                Hacks.ComponentToggle.Toggle();
            }, "TOGGLE: Keep Objects visible, but disable you being able to pick them up.");
            VRCPickupToggle.setToggleState(true); // Each Game Start, value will be true - No Config was set

            PickupObjectToggle = new QMToggleButton(baseMenu, 2, 2, "Show\nPickup Objects", () =>
            {
                Hacks.ComponentToggle.pickup_object = true;
                Hacks.ComponentToggle.Toggle();
            }, "Disabled", () =>
            {
                Hacks.ComponentToggle.pickup_object = false;
                Hacks.ComponentToggle.Toggle();
            }, "TOGGLE: Keep Objects visible, but disable you being able to pick them up.");
            PickupObjectToggle.setToggleState(true); // Each Game Start, value will be true - No Config was set

            VideoPlayerToggle = new QMToggleButton(baseMenu, 3, 2, "Show\nVideo Players", () =>
            {
                Hacks.ComponentToggle.videoplayers = true;
                Hacks.ComponentToggle.Toggle();
            }, "Disabled", () =>
            {
                Hacks.ComponentToggle.videoplayers = false;
                Hacks.ComponentToggle.Toggle();
            }, "TOGGLE: Video Players\n<color=red>Does not work in Udon Worlds</color>");
            VideoPlayerToggle.setToggleState(true); // Each Game Start, value will be true - No Config was set

            AvatarPedestalToggle = new QMToggleButton(baseMenu, 4, 2, "Avatar\nPedestals", () =>
            {
                Configuration.JSONConfig.DisableAvatarPedestals = false;
                Hacks.PedestalTweaks.Revert();
                Configuration.SaveConfig();
            }, "Disabled", () =>
            {
                Configuration.JSONConfig.DisableAvatarPedestals = true;
                Hacks.PedestalTweaks.Disable();
                Configuration.SaveConfig();
            }, "Toggles all the avatar pedestals in the world.");
            AvatarPedestalToggle.setToggleState(!Configuration.JSONConfig.DisableAvatarPedestals);
            #endregion
        }

        public static void DisableOnSceneLoad()
        {
            ItemESPToggle.setToggleState(false, true);
            TriggerESPToggle.setToggleState(false, true);
        }

        internal static void SetRiskyFuncsAllowed(bool state)
        {
            switch (state)
            {
                case true:
                    if (!Hacks.ComponentToggle.pickupable)
                    {
                        Hacks.ComponentToggle.pickupable = true;
                        VRCPickupToggle.setToggleState(true);
                    }
                    if (!Hacks.ComponentToggle.pickup_object)
                    {
                        Hacks.ComponentToggle.pickup_object = true;
                        PickupObjectToggle.setToggleState(true);
                    }
                    ItemESPToggle.getGameObject().GetComponent<Button>().enabled = true;
                    TriggerESPToggle.getGameObject().GetComponent<Button>().enabled = true;
                    VRCPickupToggle.getGameObject().GetComponent<Button>().enabled = true;
                    PickupObjectToggle.getGameObject().GetComponent<Button>().enabled = true;
                    break;
                default:
                    ItemESPToggle.getGameObject().GetComponent<Button>().enabled = false;
                    TriggerESPToggle.getGameObject().GetComponent<Button>().enabled = false;
                    VRCPickupToggle.getGameObject().GetComponent<Button>().enabled = false;
                    PickupObjectToggle.getGameObject().GetComponent<Button>().enabled = false;
                    break;
            }
        }
    }
}
