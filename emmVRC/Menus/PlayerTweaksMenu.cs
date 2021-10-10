using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using emmVRC.Libraries;
using emmVRC.Managers;
using UnityEngine;
using UnityEngine.UI;
using VRC.Animation;
using emmVRC.Objects.ModuleBases;

namespace emmVRC.Menus
{
    [Priority(50)]
    internal class PlayerTweaksMenu : MelonLoaderEvents
    {
        internal static QMNestedButton baseMenu;
        internal static GameObject SpeedText;
        internal static QMSingleButton UnloadDynamicBonesButton;
        internal static QMSingleButton SelectCurrentUserButton;
        //internal static QMSingleButton SaveAvatarParameters;
        internal static QMSingleButton ReloadAllAvatars;
        internal static QMSingleButton AvatarOptionsMenu;
        internal static QMSingleButton EnableJumpButton;
        internal static QMSingleButton WaypointMenu;
        internal static QMToggleButton FlightToggle;
        internal static QMToggleButton NoclipToggle;
        internal static QMToggleButton ESPToggle;
        internal static QMToggleButton SpeedToggle;
        internal static QMSingleButton SpeedReset;
        internal static QMSingleButton SpeedMinusButton;
        internal static QMSingleButton SpeedPlusButton;
        internal static Objects.Slider SpeedSlider;


        public override void OnUiManagerInit()
        {
            baseMenu = new QMNestedButton(FunctionsMenu.baseMenu.menuBase, 192948, 102394, "Player\nTweaks", "");
            baseMenu.getMainButton().DestroyMe();

            UnloadDynamicBonesButton = new QMSingleButton(baseMenu, 1, 0, "Remove\nLoaded\nDynamic\nBones", () =>
            {
                foreach (DynamicBone bone in GameObject.FindObjectsOfType<DynamicBone>())
                    GameObject.Destroy(bone);
                foreach (DynamicBoneCollider coll in GameObject.FindObjectsOfType<DynamicBoneCollider>())
                    GameObject.Destroy(coll);
            }, "Unload all the current dynamic bones in the instance");

            /*SaveAvatarParameters = new QMSingleButton(baseMenu, 2, 0, "Save\nAvatar\nParameters", () =>
            {
                Hacks.AvatarPropertySaving.SaveAvatarParameters();
            }, "Saves all of the current parameters for your avatar. Only works with Avatars 3.0 content");*/

            SelectCurrentUserButton = new QMSingleButton(baseMenu, 3, 0, "Select\nCurrent\nUser", () =>
            {
                QuickMenuUtils.GetQuickMenuInstance().SelectPlayer(VRCPlayer.field_Internal_Static_VRCPlayer_0._player);
                //GameObject.Find(PlayerTweaksMenu.baseMenu.getMenuName()).SetActive(false);
                //QuickMenuUtils.GetQuickMenuInstance().Method_Public_Void_EnumNPublicSealedvaUnShEmUsEmNoCaMo_nUnique_0(QuickMenu.EnumNPublicSealedvaUnShEmUsEmNoCaMo_nUnique.UserInteractMenu);
                //QuickMenuUtils.GetQuickMenuInstance().Method_Public_Void_VRCPlayer_PDM_0(VRCPlayer.field_Internal_Static_VRCPlayer_0);
            }, "Selects you as the current user");

            AvatarOptionsMenu = new QMSingleButton(baseMenu, 4, 0, "Avatar\nOptions", () => {
                AvatarPermissionManager.OpenMenu(VRCPlayer.field_Internal_Static_VRCPlayer_0.prop_ApiAvatar_0.id, false);
            }, "Allows you to configure permissions for this user's avatar, which includes dynamic bone settings");

            EnableJumpButton = new QMSingleButton(baseMenu, 1, 1, "Jumping", () =>
            {
                VRCPlayer.field_Internal_Static_VRCPlayer_0.prop_VRCPlayerApi_0.SetJumpImpulse(2.8f);
                EnableJumpButton.getGameObject().GetComponent<Button>().enabled = false;
            }, "Enables jumping for this world. Requires Risky Functions");
            EnableJumpButton.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1f, 2.0175f);
            EnableJumpButton.getGameObject().GetComponent<RectTransform>().anchoredPosition += new Vector2(0f, -96f);

            WaypointMenu = new QMSingleButton(baseMenu, 1, 1, "Waypoints", () => {
                WaypointsMenu.LoadWaypointList();
            }, "Allows you to teleport to pre-defined waypoints. Requires Risky Functions");
            WaypointMenu.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1f, 2.0175f);
            WaypointMenu.getGameObject().GetComponent<RectTransform>().anchoredPosition += new Vector2(0f, 96f);

            FlightToggle = new QMToggleButton(baseMenu, 2, 1, "Flight", () => {
                Functions.PlayerHacks.Flight.SetFlyActive(true);
            }, "Disabled", () => {
                if (Functions.PlayerHacks.Flight.IsNoClipEnabled) {
                    Functions.PlayerHacks.Flight.SetNoClipActive(false);
                    NoclipToggle.setToggleState(false);
                }
                Functions.PlayerHacks.Flight.SetFlyActive(false);
            }, "TOGGLE: Enables flight. Requires Risky Functions");
            
            NoclipToggle = new QMToggleButton(baseMenu, 3, 1, "Noclip", () => {
                if (!Functions.PlayerHacks.Flight.IsFlyEnabled) {
                    Functions.PlayerHacks.Flight.SetFlyActive(true);
                    FlightToggle.setToggleState(true); 
                }
                Functions.PlayerHacks.Flight.SetNoClipActive(true);
            }, "Disabled", () => {
                Functions.PlayerHacks.Flight.SetNoClipActive(false);
            }, "TOGGLE: Enables NoClip. Requires Risky Functions");
            
            ESPToggle = new QMToggleButton(baseMenu, 4, 1, "ESP On", () => {
                Functions.PlayerHacks.ESP.SetActive(true);
            }, "ESP Off", () => {
                Functions.PlayerHacks.ESP.SetActive(false);
            }, "TOGGLE: Enables ESP for all the players in the instance. Requires Risky Functions");

            ReloadAllAvatars = new QMSingleButton(baseMenu, 5, 1, "Reload\nAll\nAvatars", () =>
            {
                VRCPlayer.field_Internal_Static_VRCPlayer_0.ReloadAllAvatars();
            }, "Reloads all the current avatars in the room");

            SpeedToggle = new QMToggleButton(baseMenu, 4, 2, "Speed On", () =>
            {
                Functions.PlayerHacks.Speed.SetActive(true);
                SpeedSlider.slider.GetComponent<Slider>().enabled = true;
                SpeedReset.getGameObject().GetComponent<Button>().enabled = true;
                SpeedMinusButton.getGameObject().GetComponent<Button>().enabled = true;
                SpeedPlusButton.getGameObject().GetComponent<Button>().enabled = true;
            }, "Speed Off", () =>
            {
                Functions.PlayerHacks.Speed.SetActive(false);
                SpeedSlider.slider.GetComponent<Slider>().enabled = false;
                SpeedReset.getGameObject().GetComponent<Button>().enabled = false;
                SpeedMinusButton.getGameObject().GetComponent<Button>().enabled = false;
                SpeedPlusButton.getGameObject().GetComponent<Button>().enabled = false;
                SpeedText.GetComponent<Text>().text = "Speed: Disabled";
            }, "TOGGLE: Enables the Speed modifier. Requires Risky Functions");
            SpeedSlider = new Objects.Slider(baseMenu.getMenuName(), 1, 2, (float val) =>
            {
                Configuration.JSONConfig.SpeedModifier = val;
                Functions.PlayerHacks.Speed.SetActive(Functions.PlayerHacks.Speed.IsEnabled);
                if (Functions.PlayerHacks.Speed.IsEnabled)
                    SpeedText.GetComponent<Text>().text = "Speed: " + Configuration.JSONConfig.SpeedModifier.ToString("N2");
            }, Configuration.JSONConfig.SpeedModifier);
            SpeedSlider.slider.GetComponent<UnityEngine.UI.Slider>().enabled = false;
            SpeedSlider.slider.GetComponent<RectTransform>().anchoredPosition += new Vector2(256f, -104f);
            SpeedSlider.slider.GetComponent<RectTransform>().sizeDelta *= new Vector2(0.85f, 1f);
            SpeedSlider.slider.GetComponent<UnityEngine.UI.Slider>().maxValue = Configuration.JSONConfig.MaxSpeedIncrease;

            SpeedReset = new QMSingleButton(baseMenu, 3, 2, "Reset", () =>
            {
                SpeedSlider.slider.GetComponent<UnityEngine.UI.Slider>().value = 2f;
                Functions.PlayerHacks.Speed.SetActive(Functions.PlayerHacks.Speed.IsEnabled);
                Configuration.JSONConfig.SpeedModifier = 2f;
                SpeedText.GetComponent<Text>().text = "Speed: " + SpeedSlider.slider.GetComponent<UnityEngine.UI.Slider>().value.ToString("N2");
            }, "Resets the speed modifier to the default value");
            SpeedReset.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1f, 2.0175f);
            SpeedReset.getGameObject().GetComponent<RectTransform>().anchoredPosition += new Vector2(0f, 96f);
            SpeedReset.getGameObject().GetComponent<UnityEngine.UI.Button>().enabled = false;

            SpeedMinusButton = new QMSingleButton(baseMenu, 1, 2, "-", () =>
            {
                SpeedSlider.slider.GetComponent<UnityEngine.UI.Slider>().value -= 0.25f;
                Functions.PlayerHacks.Speed.SetActive(Functions.PlayerHacks.Speed.IsEnabled);
                Configuration.JSONConfig.SpeedModifier = SpeedSlider.slider.GetComponent<UnityEngine.UI.Slider>().value;
                SpeedText.GetComponent<Text>().text = "Speed: " + SpeedSlider.slider.GetComponent<UnityEngine.UI.Slider>().value.ToString("N2");
            }, "Decrease the speed modifier by 0.25");
            SpeedMinusButton.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(2.0175f, 2.0175f);
            SpeedMinusButton.getGameObject().GetComponent<RectTransform>().anchoredPosition += new Vector2(-96f, -96f);
            SpeedMinusButton.getGameObject().GetComponent<Button>().enabled = false;

            SpeedPlusButton = new QMSingleButton(baseMenu, 3, 2, "+", () =>
            {
                SpeedSlider.slider.GetComponent<UnityEngine.UI.Slider>().value += 0.25f;
                Functions.PlayerHacks.Speed.SetActive(Functions.PlayerHacks.Speed.IsEnabled);
                Configuration.JSONConfig.SpeedModifier = SpeedSlider.slider.GetComponent<UnityEngine.UI.Slider>().value;
                SpeedText.GetComponent<Text>().text = "Speed: " + SpeedSlider.slider.GetComponent<UnityEngine.UI.Slider>().value.ToString("N2");
            }, "Increase the speed modifier by 0.25");
            SpeedPlusButton.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(2.0175f, 2.0175f);
            SpeedPlusButton.getGameObject().GetComponent<RectTransform>().anchoredPosition += new Vector2(96f, -96f);
            SpeedPlusButton.getGameObject().GetComponent<Button>().enabled = false;


            SpeedText = GameObject.Instantiate(QuickMenuUtils.GetQuickMenuInstance().transform.Find("QuickMenu_NewElements/_InfoBar/EarlyAccessText").gameObject, SpeedMinusButton.getGameObject().transform.parent);
            SpeedText.GetComponent<UnityEngine.UI.Text>().fontStyle = FontStyle.Normal;
            SpeedText.GetComponent<UnityEngine.UI.Text>().text = "Speed: Disabled";
            SpeedText.GetComponent<RectTransform>().anchoredPosition = SpeedMinusButton.getGameObject().GetComponent<RectTransform>().anchoredPosition + new Vector2(192f, 192f);
            Components.EnableDisableListener listener = FlightToggle.getGameObject().transform.parent.gameObject.AddComponent<Components.EnableDisableListener>();
            listener.OnEnabled += () =>
            {
                SetRiskyFuncsAllowed(RiskyFunctionsManager.AreRiskyFunctionsAllowed);
            };
        }
        internal static void SetRiskyFuncsAllowed(bool state)
        {
            if (state)
            {
                WaypointMenu.getGameObject().GetComponent<Button>().enabled = true;
                FlightToggle.getGameObject().GetComponent<Button>().enabled = true;
                NoclipToggle.getGameObject().GetComponent<Button>().enabled = true;
                SpeedToggle.getGameObject().GetComponent<Button>().enabled = true;
                ESPToggle.getGameObject().GetComponent<Button>().enabled = true;
                SpeedSlider.slider.GetComponent<Slider>().enabled = true;
                SpeedText.GetComponent<Text>().text = "Speed: Disabled";
                EnableJumpButton.getGameObject().GetComponent<Button>().enabled = true;
            }
            else
            {
                WaypointMenu.getGameObject().GetComponent<Button>().enabled = false;
                FlightToggle.getGameObject().GetComponent<Button>().enabled = false;
                NoclipToggle.getGameObject().GetComponent<Button>().enabled = false;
                SpeedToggle.getGameObject().GetComponent<Button>().enabled = false;
                ESPToggle.getGameObject().GetComponent<Button>().enabled = false;
                SpeedMinusButton.getGameObject().GetComponent<Button>().enabled = false;
                SpeedPlusButton.getGameObject().GetComponent<Button>().enabled = false;
                SpeedReset.getGameObject().GetComponent<Button>().enabled = false;
                SpeedSlider.slider.GetComponent<Slider>().enabled = false;
                SpeedText.GetComponent<Text>().text = (Configuration.JSONConfig.RiskyFunctionsEnabled ? "     Risky Functions\n        Not allowed" : "     Risky Functions\n        Not enabled");
                EnableJumpButton.getGameObject().GetComponent<Button>().enabled = false;
            }
        }

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            if (buildIndex == -1)
            {
                Menus.PlayerTweaksMenu.SpeedToggle.setToggleState(false, true);
                Menus.PlayerTweaksMenu.FlightToggle.setToggleState(false, true);
                Menus.PlayerTweaksMenu.NoclipToggle.setToggleState(false, true);
                Menus.PlayerTweaksMenu.ESPToggle.setToggleState(false, true);
            }
        }
    }
}
