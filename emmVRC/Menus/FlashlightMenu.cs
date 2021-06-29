﻿using emmVRC.Libraries;
using emmVRC.Objects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace emmVRC.Menus
{
    public class FlashlightMenu
    {
        public static QMNestedButton baseMenu;

        public static QMToggleButton toggleFlashlight;
        public static QMToggleButton toggleHeadlight;
        public static ColorPicker setFlashlightLightColor;
        public static Objects.Slider flashlightStrengthSlider;

        public static Color lightColor = Color.white;
        public static float LightStrength = 10f;

        public static GameObject FlashlightObject;
        public static GameObject HeadlightObject;

        public static GameObject CameraBase;

        public static void Initialize()
        {
            baseMenu = new QMNestedButton(WorldTweaksMenu.baseMenu, 3, 1, "Flashlight", "Configure and summon a flashlight you can carry through your current world");
            toggleFlashlight = new QMToggleButton(baseMenu, 1, 0, "Flashlight On", () =>
            {
                GameObject flashLightBase = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                Renderer ren1 = flashLightBase.GetComponent<Renderer>();
                ren1.material.color = Configuration.menuColor();
                Collider coll = flashLightBase.GetComponent<Collider>();
                coll.isTrigger = true;
                flashLightBase.transform.localScale = new Vector3(0.05f, 0.125f, 0.05f);
                flashLightBase.transform.position = VRCPlayer.field_Internal_Static_VRCPlayer_0.transform.position + new Vector3(0f, 1f, 0f);
                Rigidbody rgbd = flashLightBase.AddComponent<Rigidbody>();
                rgbd.useGravity = false;
                rgbd.isKinematic = true;
                VRCSDK2.VRC_Pickup pickup = flashLightBase.AddComponent<VRCSDK2.VRC_Pickup>();
                pickup.AutoHold = VRC.SDKBase.VRC_Pickup.AutoHoldMode.Yes;
                GameObject flashLightLight = new GameObject("LightBase");
                flashLightLight.transform.SetParent(flashLightBase.transform);
                flashLightLight.transform.localPosition = Vector3.zero;
                Light lght = flashLightLight.AddComponent<Light>();
                lght.color = lightColor;
                flashLightLight.transform.Rotate(90f, 0f, 0f);
                GameObject flashLightFront = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                Renderer ren2 = flashLightFront.GetComponent<Renderer>();
                ren2.material.color = Configuration.menuColor();
                flashLightFront.transform.SetParent(flashLightBase.transform);
                flashLightFront.transform.localPosition = new Vector3(0, -0.75f, 0f);
                flashLightFront.transform.localScale = new Vector3(1.5f, 0.25f, 1.5f);
                flashLightFront.GetComponent<Collider>().isTrigger = true;
                lght.type = LightType.Spot;
                lght.range = LightStrength;
                flashLightBase.transform.Rotate(90f, 0f, 0f);
                FlashlightObject = flashLightBase;
                VRC_UdonTrigger.Instantiate(FlashlightObject, "Toggle On/Off", () => {
                    lght.enabled = !lght.enabled;
                });
            }, "Flashlight Off", () =>
            {
                GameObject.Destroy(FlashlightObject);
            }, "TOGGLE: Turns on and off the flashlight");

            toggleHeadlight = new QMToggleButton(baseMenu, 2, 0, "Headlight On", () =>
            {
                if (CameraBase == null)
                CameraBase = GameObject.Find("Camera (eye)");

                if (CameraBase == null)
                    CameraBase = GameObject.Find("CenterEyeAnchor");
                if (CameraBase != null)
                {
                    GameObject headLightBase = new GameObject("Headlight");
                    headLightBase.transform.SetParent(CameraBase.transform, false);
                    GameObject headLightLight = new GameObject("LightBase");
                    headLightLight.transform.SetParent(headLightBase.transform);
                    headLightLight.transform.localPosition = Vector3.zero;
                    Light lght = headLightLight.AddComponent<Light>();
                    lght.color = lightColor;
                    headLightLight.transform.rotation = VRCPlayer.field_Internal_Static_VRCPlayer_0.transform.rotation;
                    headLightLight.transform.Rotate(20f, 0f, 0f);
                    lght.type = LightType.Spot;
                    lght.range = LightStrength;
                    lght.spotAngle *= 2;
                    HeadlightObject = headLightBase;
                }
            }, "Headlight Off", () =>
            {
                GameObject.Destroy(HeadlightObject);
            }, "TOGGLE: Turns on and off the headlight");

            setFlashlightLightColor = new ColorPicker(baseMenu.getMenuName(), 4, 0, "Light\nColor", "Allows you to set the flashlight color", (Color result) =>
            {
                lightColor = result;
                if (FlashlightObject != null)
                    FlashlightObject.GetComponentInChildren<Light>().color = result;
                if (HeadlightObject != null)
                    HeadlightObject.GetComponentInChildren<Light>().color = result;
                QuickMenuUtils.ShowQuickmenuPage(baseMenu.getMenuName());
            }, () => {
                QuickMenuUtils.ShowQuickmenuPage(baseMenu.getMenuName());
            }, lightColor, Color.white);
            flashlightStrengthSlider = new Objects.Slider(baseMenu.getMenuName(), 1, 2, new Action<float>((float flt) => {
                LightStrength = flt;
                if (FlashlightObject != null)
                    FlashlightObject.GetComponentInChildren<Light>().range = LightStrength;
                if (HeadlightObject != null)
                    HeadlightObject.GetComponentInChildren<Light>().range = LightStrength;
            }), 10);
            flashlightStrengthSlider.slider.GetComponent<UnityEngine.UI.Slider>().maxValue = 100;
            flashlightStrengthSlider.slider.GetComponent<UnityEngine.UI.Slider>().minValue = 10;
            flashlightStrengthSlider.slider.GetComponent<RectTransform>().anchoredPosition += new Vector2(480f, -104f);
            flashlightStrengthSlider.slider.GetComponent<RectTransform>().sizeDelta *= new Vector2(2f, 1f);

            MelonLoader.MelonCoroutines.Start(Loop());
        }
        private static IEnumerator Loop()
        {
            while (true)
            {
                yield return new WaitForEndOfFrame();
                /*if (HeadlightObject != null && VRCPlayer.field_Internal_Static_VRCPlayer_0 != null && RoomManager.field_Internal_Static_ApiWorld_0 != null)
                {
                    try
                    {
                        HeadlightObject.transform.position = VRCPlayer.field_Internal_Static_VRCPlayer_0.prop_VRCAvatarManager_0.field_Private_Animator_0.GetBoneTransform(HumanBodyBones.Head).position;
                        HeadlightObject.transform.rotation = VRCPlayer.field_Internal_Static_VRCPlayer_0.prop_VRCAvatarManager_0.field_Private_Animator_0.GetBoneTransform(HumanBodyBones.Head).rotation;
                    }
                    catch (Exception ex)
                    {
                        ex = new Exception();
                    }
                }*/
            }
        }
    }
}
