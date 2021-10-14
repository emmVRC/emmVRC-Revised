using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using emmVRC.Libraries;
using UnityEngine;
using UnityEngine.UI;
using emmVRC.Objects.ModuleBases;

namespace emmVRC.Menus
{
    [Priority(50)]
    public class WorldTweaksMenuLegacy : MelonLoaderEvents
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

        public override void OnUiManagerInit()
        {
            baseMenu = new QMNestedButton(FunctionsMenuLegacy.baseMenu.menuBase, 192384, 129302, "World\nTweaks", "");
            baseMenu.getMainButton().DestroyMe();
            OptimizeMirrorsButton = new QMSingleButton(baseMenu, 1, 0, "Optimize", () => { Hacks.MirrorTweaks.Optimize(); }, "Sets the mirrors around you to only reflect the bare necessities, for optimization");
            OptimizeMirrorsButton.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1f, 2.0175f);
            OptimizeMirrorsButton.getGameObject().GetComponent<RectTransform>().anchoredPosition += new Vector2(0f, 96f);
            BeautifyMirrorsButton = new QMSingleButton(baseMenu, 1, 0, "Beautify", () => { Hacks.MirrorTweaks.Beautify(); }, "Sets the mirrors around you to reflect absolutely everything");
            BeautifyMirrorsButton.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1f, 2.0175f);
            BeautifyMirrorsButton.getGameObject().GetComponent<RectTransform>().anchoredPosition += new Vector2(0f, -96f);
            RevertMirrorsButton = new QMSingleButton(baseMenu, 2, 0, "Revert\nMirrors", () => { Hacks.MirrorTweaks.Revert(); }, "Reverts the mirrors in the world to their default reflections");
            ReloadWorldButton = new QMSingleButton(baseMenu, 3, 0, "Reload\nWorld", () => { VRCFlowManager.prop_VRCFlowManager_0.EnterWorld(RoomManager.field_Internal_Static_ApiWorld_0.id, RoomManager.field_Internal_Static_ApiWorldInstance_0.instanceId); }, "Loads the current instance again");
            PortalBlockToggle = new QMToggleButton(baseMenu, 4, 0, "Portals On", () => { Configuration.WriteConfigOption("PortalBlockingEnable", false); }, "Portals Off", () => { Configuration.WriteConfigOption("PortalBlockingEnable", true); }, "TOGGLE: Enables or disables portals in the current world", null, null, false, true);
            PortalBlockToggle.setToggleState(!Configuration.JSONConfig.PortalBlockingEnable);
            ChairBlockToggle = new QMToggleButton(baseMenu, 4, 1, "Chairs On", () => { Configuration.WriteConfigOption("ChairBlockingEnable", false); }, "Chairs Off", () => { Configuration.WriteConfigOption("ChairBlockingEnable", true); }, "TOGGLE: Enables or disables chairs in the current world", null, null, false, true);
            ChairBlockToggle.setToggleState(!Configuration.JSONConfig.ChairBlockingEnable);

            #region Korty's Addons
            ItemESPToggle = new QMToggleButton(baseMenu, 1, 1, "Item ESP", () =>
            {
                Functions.UI.WorldFunctions.ToggleItemESP(true);
            }, "Disabled", () =>
            {
                Functions.UI.WorldFunctions.ToggleItemESP(false);
            }, "TOGGLE: An Outline around all Pickup-able items in the world");

            TriggerESPToggle = new QMToggleButton(baseMenu, 2, 1, "Trigger ESP", () =>
            {
                Functions.UI.WorldFunctions.ToggleTriggerESP(true);
            }, "Disabled", () =>
            {
                Functions.UI.WorldFunctions.ToggleTriggerESP(false);
            }, "TOGGLE: An Outline around all triggers in the world\n(Some may not show due to area triggers blocking anything being behide larger area triggers)");

            VRCPickupToggle = new QMToggleButton(baseMenu, 1, 2, "Enable\nItem Pickup", () =>
            {
                Hacks.ComponentToggle.pickupable = true;
                Hacks.ComponentToggle.Toggle();

            }, "Disabled", () =>
            {
                Hacks.ComponentToggle.pickupable = false;
                Hacks.ComponentToggle.Toggle();
            }, "TOGGLE: Keep Objects visible, but disable you being able to pick them up");
            VRCPickupToggle.setToggleState(true); // Each Game Start, value will be true - No Config was set

            PickupObjectToggle = new QMToggleButton(baseMenu, 2, 2, "Show\nPickup Objects", () =>
            {
                Hacks.ComponentToggle.pickup_object = true;
                Hacks.ComponentToggle.Toggle();
            }, "Disabled", () =>
            {
                Hacks.ComponentToggle.pickup_object = false;
                Hacks.ComponentToggle.Toggle();
            }, "TOGGLE: Hide all pickup objects in the world");
            PickupObjectToggle.setToggleState(true); // Each Game Start, value will be true - No Config was set

            VideoPlayerToggle = new QMToggleButton(baseMenu, 3, 2, "Show\nVideo Players", () =>
            {
                Hacks.ComponentToggle.videoplayers = true;
                Hacks.ComponentToggle.Toggle();
            }, "Disabled", () =>
            {
                Hacks.ComponentToggle.videoplayers = false;
                Hacks.ComponentToggle.Toggle();
            }, "TOGGLE: Video Players");
            VideoPlayerToggle.setToggleState(true); // Each Game Start, value will be true - No Config was set

            AvatarPedestalToggle = new QMToggleButton(baseMenu, 4, 2, "Avatar\nPedestals", () =>
            {
                Configuration.WriteConfigOption("DisableAvatarPedestals", false);
                Hacks.PedestalTweaks.Revert();
            }, "Disabled", () =>
            {
                Configuration.WriteConfigOption("DisableAvatarPedestals", true);
                Hacks.PedestalTweaks.Disable();
            }, "Toggles all the avatar pedestals in the world.");
            AvatarPedestalToggle.setToggleState(!Configuration.JSONConfig.DisableAvatarPedestals);
            #endregion
            Components.EnableDisableListener listener = OptimizeMirrorsButton.getGameObject().transform.parent.gameObject.AddComponent<Components.EnableDisableListener>();
            listener.OnEnabled += () =>
            {
                SetRiskyFuncsAllowed(Managers.RiskyFunctionsManager.AreRiskyFunctionsAllowed);
            };
        }

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            if (buildIndex != -1) return;
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
