using emmVRC.Components;
using emmVRC.Objects.ModuleBases;
using System;
using UnityEngine;
using VRC.SDK3.Components;
using VRC.SDKBase;

namespace emmVRC.Functions.WorldHacks
{
    public class Flashlight : MelonLoaderEvents
    {
        public static bool IsHeadlightEnabled { get; private set; }
        public static bool IsFlashlightEnabled { get; private set; }

        private static GameObject flashlightParent;
        private static Material flashlightParentMaterial;
        private static Light flashlight;
        private static Light headlight;

        public override void OnApplicationStart()
        {
            Configuration.onConfigUpdated.Add(new System.Collections.Generic.KeyValuePair<string, Action>("FlashlightColorHex", () => { if (flashlight == null) return; flashlight.color = Libraries.ColorConversion.HexToColor(Configuration.JSONConfig.FlashlightColorHex); headlight.color = Libraries.ColorConversion.HexToColor(Configuration.JSONConfig.FlashlightColorHex); }));
            Configuration.onConfigUpdated.Add(new System.Collections.Generic.KeyValuePair<string, Action>("FlashlightRange", () => { if (flashlight == null) return; flashlight.range = Configuration.JSONConfig.FlashlightRange; headlight.range = Configuration.JSONConfig.FlashlightRange; }));
            Configuration.onConfigUpdated.Add(new System.Collections.Generic.KeyValuePair<string, Action>("FlashlightPower", () => { if (flashlight == null) return; flashlight.intensity = Configuration.JSONConfig.FlashlightPower; headlight.intensity = Configuration.JSONConfig.FlashlightPower; }));
            Configuration.onConfigUpdated.Add(new System.Collections.Generic.KeyValuePair<string, Action>("FlashlightAngle", () => { if (flashlight == null) return; flashlight.spotAngle = Configuration.JSONConfig.FlashlightAngle; headlight.spotAngle = Configuration.JSONConfig.FlashlightAngle; }));
            //Menus.FlashlightMenuLegacy.LightColorChanged += new Action<Color>((newValue) => { if (flashlight == null) return; flashlight.color = newValue; headlight.color = newValue; });
            //Menus.FlashlightMenuLegacy.LightStrengthChanged += new Action<float>((newValue) => { if (flashlight == null) return; flashlight.range = newValue; headlight.range = newValue; });
        }

        public override void OnUiManagerInit()
        {
            GameObject headlightParent = new GameObject("emmVRCHeadlight");
            // This is on the main camera game object
            headlightParent.transform.SetParent(HighlightsFX.field_Private_Static_HighlightsFX_0.transform, false);
            headlight = headlightParent.AddComponent<Light>();
            headlight.type = LightType.Spot;
            headlight.enabled = false;

            flashlightParent = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            GameObject.DontDestroyOnLoad(flashlightParent);
            flashlightParent.name = "emmVRCFlashlightParent";
            flashlightParent.transform.localScale = new Vector3(0.05f, 0.125f, 0.05f);

            Renderer flashlightRenderer = flashlightParent.GetComponent<Renderer>();
            flashlightRenderer.material.color = Libraries.ColorConversion.HexToColor(Configuration.JSONConfig.UIColorHex);
            flashlightParentMaterial = flashlightRenderer.material;

            flashlightParent.GetComponent<Collider>().isTrigger = true;
            flashlightParent.AddComponent<Rigidbody>().isKinematic = true;

            VRC_UdonPickupTrigger flashlightTrigger = flashlightParent.AddComponent<VRC_UdonPickupTrigger>();
            flashlightTrigger.OnPickupUseUp = new Action(() => flashlight.enabled = !flashlight.enabled);

            VRCPickup pickup = flashlightParent.AddComponent<VRCPickup>();
            pickup.AutoHold = VRC_Pickup.AutoHoldMode.Yes;
            pickup.UseText = "Toggle Light";

            GameObject flashlightLight = new GameObject("Light");
            flashlightLight.transform.SetParent(flashlightParent.transform, false);
            flashlightLight.transform.Rotate(90f, 0f, 0f);
            flashlight = flashlightLight.AddComponent<Light>();
            flashlight.type = LightType.Spot;

            GameObject flashLightFront = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            flashLightFront.transform.SetParent(flashlightParent.transform, false);
            flashLightFront.transform.localPosition = new Vector3(0, -0.75f, 0f);
            flashLightFront.transform.localScale = new Vector3(1.5f, 0.25f, 1.5f);
            flashlightParent.transform.Rotate(90f, 0f, 0f);

            flashLightFront.GetComponent<Renderer>().material = flashlightParentMaterial;
            flashLightFront.GetComponent<Collider>().isTrigger = true;

            flashlightParent.SetActive(false);
        }

        public static void SetFlashlightActive(bool active)
        {
            flashlightParent.transform.position = HighlightsFX.field_Private_Static_HighlightsFX_0.transform.position + HighlightsFX.field_Private_Static_HighlightsFX_0.transform.forward;
            flashlightParent.transform.rotation = Quaternion.LookRotation(HighlightsFX.field_Private_Static_HighlightsFX_0.transform.up);
            IsFlashlightEnabled = active;
            flashlightParent.SetActive(active);
            flashlight.enabled = active;
            flashlight.color = Libraries.ColorConversion.HexToColor(Configuration.JSONConfig.FlashlightColorHex);
            flashlight.range = Configuration.JSONConfig.FlashlightRange;
            flashlight.intensity = Configuration.JSONConfig.FlashlightPower;
            flashlight.spotAngle = Configuration.JSONConfig.FlashlightAngle;
        }

        public static void SetHeadlightActive(bool active)
        {
            IsHeadlightEnabled = active;
            headlight.enabled = active;
            headlight.color = Libraries.ColorConversion.HexToColor(Configuration.JSONConfig.FlashlightColorHex);
            headlight.range = Configuration.JSONConfig.FlashlightRange;
            headlight.intensity = Configuration.JSONConfig.FlashlightPower;
            headlight.spotAngle = Configuration.JSONConfig.FlashlightAngle;
        }
    }
}