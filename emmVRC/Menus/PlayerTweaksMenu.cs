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
    public class PlayerTweaksMenu
    {
        public static QMNestedButton baseMenu;
        public static GameObject SpeedText;
        public static QMSingleButton UnloadDynamicBonesButton;
        public static QMSingleButton SelectCurrentUserButton;
        public static QMSingleButton ReloadAllAvatars;
        public static QMSingleButton EnableJumpButton;
        public static QMSingleButton WaypointMenu;
        public static QMToggleButton AvatarClone;
        public static QMToggleButton FlightToggle;
        public static QMToggleButton NoclipToggle;
        public static QMToggleButton ESPToggle;
        public static QMToggleButton SpeedToggle;
        public static QMSingleButton SpeedReset;
        public static QMSingleButton SpeedMinusButton;
        public static QMSingleButton SpeedPlusButton;
        public static Objects.Slider SpeedSlider;


        public static void Initialize()
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

            ReloadAllAvatars = new QMSingleButton(baseMenu, 2, 0, "Reload\nAll\nAvatars", () =>
            {
                VRCPlayer.field_VRCPlayer_0.Method_Public_Boolean_0(false);
            }, "Reloads all the current avatars in the room");

            SelectCurrentUserButton = new QMSingleButton(baseMenu, 3, 0, "Select\nCurrent\nUser", () =>
            {
                QuickMenuUtils.GetQuickMenuInstance().Method_Public_VRCPlayer_0(VRCPlayer.field_VRCPlayer_0);
            }, "Selects you as the current user");
            EnableJumpButton = new QMSingleButton(baseMenu, 4, 0, "Enable\nJumping", () =>
            {
                if (VRCPlayer.field_VRCPlayer_0.gameObject.GetComponent<PlayerModComponentJump>() == null)
                    VRCPlayer.field_VRCPlayer_0.gameObject.AddComponent<PlayerModComponentJump>();
                EnableJumpButton.getGameObject().GetComponent<Button>().enabled = false;
            }, "Enables jumping for this world. Requires Risky Functions");

            WaypointMenu = new QMSingleButton(baseMenu, 1, 1, "Waypoints", () => {
                WaypointsMenu.LoadWaypointList();
            }, "Allows you to teleport to pre-defined waypoints. Requires Risky Functions");
            
            FlightToggle = new QMToggleButton(baseMenu, 2, 1, "Flight", () => { 
                Hacks.Flight.FlightEnabled = true;
            }, "Disabled", () => { 
                Hacks.Flight.FlightEnabled = false;
                if (Hacks.Flight.NoclipEnabled) { Hacks.Flight.NoclipEnabled = false; 
                    NoclipToggle.setToggleState(false);
                    VRCPlayer.field_VRCPlayer_0.GetComponent<VRCMotionState>().field_CharacterController_0.enabled = true;
                } 
            }, "TOGGLE: Enables flight. Requires Risky Functions");
            
            NoclipToggle = new QMToggleButton(baseMenu, 3, 1, "Noclip", () => { 
                Hacks.Flight.NoclipEnabled = true; 
                if (!Hacks.Flight.FlightEnabled) {
                    Hacks.Flight.FlightEnabled = true; 
                    FlightToggle.setToggleState(true); 
                } 
            }, "Disabled", () => { 
                Hacks.Flight.NoclipEnabled = false;
            }, "TOGGLE: Enables NoClip. Requires Risky Functions");
            
            ESPToggle = new QMToggleButton(baseMenu, 4, 1, "ESP On", () => {
                Hacks.ESP.ESPEnabled = true;
            }, "ESP Off", () => {
                Hacks.ESP.ESPEnabled = false;
            }, "TOGGLE: Enables ESP for all the players in the instance. Requires Risky Functions");

            SpeedToggle = new QMToggleButton(baseMenu, 4, 2, "Speed On", () =>
            {
                Hacks.Speed.SpeedModified = true;
                SpeedSlider.slider.GetComponent<Slider>().enabled = true;
                SpeedReset.getGameObject().GetComponent<Button>().enabled = true;
                SpeedMinusButton.getGameObject().GetComponent<Button>().enabled = true;
                SpeedPlusButton.getGameObject().GetComponent<Button>().enabled = true;
                Hacks.Speed.Modifier = SpeedSlider.slider.GetComponent<Slider>().value;
                SpeedText.GetComponent<Text>().text = "Speed: " + SpeedSlider.slider.GetComponent<UnityEngine.UI.Slider>().value.ToString("N2");
            }, "Speed Off", () =>
            {
                Hacks.Speed.SpeedModified = false;
                SpeedSlider.slider.GetComponent<Slider>().enabled = false;
                SpeedReset.getGameObject().GetComponent<Button>().enabled = false;
                SpeedMinusButton.getGameObject().GetComponent<Button>().enabled = false;
                SpeedPlusButton.getGameObject().GetComponent<Button>().enabled = false;
                SpeedText.GetComponent<Text>().text = "Speed: Disabled";
            }, "TOGGLE: Enables the Speed modifier. Requires Risky Functions");
            SpeedSlider = new Objects.Slider(baseMenu.getMenuName(), 1, 2, (float val) =>
            {
                Hacks.Speed.Modifier = val;
                if (Hacks.Speed.SpeedModified)
                    SpeedText.GetComponent<Text>().text = "Speed: " + val.ToString("N2");
            }, 0.5f);
            SpeedSlider.slider.GetComponent<UnityEngine.UI.Slider>().enabled = false;
            SpeedSlider.slider.GetComponent<RectTransform>().anchoredPosition += new Vector2(256f, -104f);
            SpeedSlider.slider.GetComponent<UnityEngine.UI.Slider>().maxValue = 5f;

            SpeedReset = new QMSingleButton(baseMenu, 3, 2, "Reset", () =>
            {
                SpeedSlider.slider.GetComponent<UnityEngine.UI.Slider>().value = 1f;
                Hacks.Speed.Modifier = SpeedSlider.slider.GetComponent<UnityEngine.UI.Slider>().value;
                SpeedText.GetComponent<Text>().text = "Speed: " + SpeedSlider.slider.GetComponent<UnityEngine.UI.Slider>().value.ToString("N2");
            }, "Resets the speed modifier to the default value");
            SpeedReset.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(1f, 2.0175f);
            SpeedReset.getGameObject().GetComponent<RectTransform>().anchoredPosition += new Vector2(0f, 96f);
            SpeedReset.getGameObject().GetComponent<UnityEngine.UI.Button>().enabled = false;

            SpeedMinusButton = new QMSingleButton(baseMenu, 1, 2, "-", () =>
            {
                SpeedSlider.slider.GetComponent<UnityEngine.UI.Slider>().value -= 0.25f;
                Hacks.Speed.Modifier = SpeedSlider.slider.GetComponent<UnityEngine.UI.Slider>().value;
                SpeedText.GetComponent<Text>().text = "Speed: " + SpeedSlider.slider.GetComponent<UnityEngine.UI.Slider>().value.ToString("N2");
            }, "Decrease the speed modifier by 0.25");
            SpeedMinusButton.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(2.0175f, 2.0175f);
            SpeedMinusButton.getGameObject().GetComponent<RectTransform>().anchoredPosition += new Vector2(-96f, -96f);
            SpeedMinusButton.getGameObject().GetComponent<Button>().enabled = false;

            SpeedPlusButton = new QMSingleButton(baseMenu, 3, 2, "+", () =>
            {
                SpeedSlider.slider.GetComponent<UnityEngine.UI.Slider>().value += 0.25f;
                Hacks.Speed.Modifier = SpeedSlider.slider.GetComponent<UnityEngine.UI.Slider>().value;
                SpeedText.GetComponent<Text>().text = "Speed: " + SpeedSlider.slider.GetComponent<UnityEngine.UI.Slider>().value.ToString("N2");
            }, "Increase the speed modifier by 0.25");
            SpeedPlusButton.getGameObject().GetComponent<RectTransform>().sizeDelta /= new Vector2(2.0175f, 2.0175f);
            SpeedPlusButton.getGameObject().GetComponent<RectTransform>().anchoredPosition += new Vector2(96f, -96f);
            SpeedPlusButton.getGameObject().GetComponent<Button>().enabled = false;


            SpeedText = GameObject.Instantiate(QuickMenuUtils.GetQuickMenuInstance().transform.Find("ShortcutMenu/EarlyAccessText").gameObject, SpeedMinusButton.getGameObject().transform.parent);
            SpeedText.GetComponent<UnityEngine.UI.Text>().fontStyle = FontStyle.Normal;
            SpeedText.GetComponent<UnityEngine.UI.Text>().text = "Speed: Disabled";
            SpeedText.GetComponent<RectTransform>().anchoredPosition = SpeedMinusButton.getGameObject().GetComponent<RectTransform>().anchoredPosition + new Vector2(192f, 192f);
        }
        public static void SetRiskyFunctions(bool state)
        {
            if (state)
            {
                WaypointMenu.getGameObject().GetComponent<Button>().enabled = true;
                FlightToggle.getGameObject().GetComponent<Button>().enabled = true;
                NoclipToggle.getGameObject().GetComponent<Button>().enabled = true;
                SpeedToggle.getGameObject().GetComponent<Button>().enabled = true;
                ESPToggle.getGameObject().GetComponent<Button>().enabled = true;
                SpeedMinusButton.getGameObject().GetComponent<Button>().enabled = true;
                SpeedPlusButton.getGameObject().GetComponent<Button>().enabled = true;
                SpeedReset.getGameObject().GetComponent<Button>().enabled = true;
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
    }
}
