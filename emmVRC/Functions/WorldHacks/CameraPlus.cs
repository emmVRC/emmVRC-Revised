using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VRC.Core;
using VRC;
using VRC.UI;
using VRC.UserCamera;
using emmVRC.Objects.ModuleBases;

namespace emmVRC.Functions.WorldHacks
{
    public class CameraPlus : MelonLoaderEvents
    {
        public static GameObject zoomInButton;
        public static GameObject zoomOutButton;
        public static GameObject toggleCameraIndicatorButton;
        private static Transform camera_body;
        public override void OnUiManagerInit()
        {
            if (Configuration.JSONConfig.StealthMode || Functions.Core.ModCompatibility.CameraPlus || Functions.Other.BuildNumber.buildNumber > 1134) return;
            Configuration.onConfigUpdated.Add(new System.Collections.Generic.KeyValuePair<string, Action>("CameraPlus", SetCameraPlus));
            MelonLoader.MelonCoroutines.Start(Initialize());
        }
        public static IEnumerator Initialize()
        {
            while (Functions.Core.Resources.zoomIn == null || Functions.Core.Resources.zoomOut == null || Functions.Core.Resources.lensOff == null || Functions.Core.Resources.lensOn == null)
                yield return new WaitForEndOfFrame();
            // Grab Controller
            UserCameraController userCameraController = UnityEngine.Resources.FindObjectsOfTypeAll<UserCameraController>()[0];

            // Create Sprites
            //Sprite zoomin_sprite = CreateSprite(ImageData.zoomin_image);
            //Sprite zoomout_sprite = CreateSprite(ImageData.zoomout_image);
            //Sprite cameraindicator_on_sprite = CreateSprite(ImageData.cameraindicator_on_image);
            //Sprite cameraindicator_off_sprite = CreateSprite(ImageData.cameraindicator_off_image);

            // Zoom-In
            zoomInButton = GameObject.Instantiate(userCameraController.transform.Find("ViewFinder/PhotoControls/Right_Filters").gameObject, userCameraController.transform.Find("ViewFinder"));
            Components.VRC_UdonTrigger zoomInTrig = zoomInButton.AddComponent<Components.VRC_UdonTrigger>();
            zoomInTrig.InteractText = "Zoom-In";
            zoomInTrig.OnInteract = () => 
            {
                Camera cam = userCameraController.transform.Find("PhotoCamera").GetComponent<Camera>();
                if ((cam.fieldOfView - 10) > 0) cam.fieldOfView -= 10;
                cam = userCameraController.transform.Find("PhotoCamera/VideoCamera").GetComponent<Camera>();
                if ((cam.fieldOfView - 10) > 0) cam.fieldOfView -= 10;
                userCameraController.field_Public_AudioSource_0.PlayOneShot(userCameraController.field_Public_AudioClip_0);
            };
            SetButtonSprite(zoomInButton, Functions.Core.Resources.zoomIn);
            SetButtonIconScale(zoomInButton);
            SetButtonOffset(zoomInButton);

            // Zoom-Out
            zoomOutButton = GameObject.Instantiate(userCameraController.transform.Find("ViewFinder/PhotoControls/Right_Lock").gameObject, userCameraController.transform.Find("ViewFinder"));
            Components.VRC_UdonTrigger zoomOutTrig = zoomOutButton.AddComponent<Components.VRC_UdonTrigger>();
            zoomOutTrig.InteractText = "Zoom-Out";
            zoomOutTrig.OnInteract = () => 
            {
                Camera cam = userCameraController.transform.Find("PhotoCamera").GetComponent<Camera>();
                if ((cam.fieldOfView + 10) < 120) cam.fieldOfView += 10;
                cam = userCameraController.transform.Find("PhotoCamera/VideoCamera").GetComponent<Camera>();
                if ((cam.fieldOfView + 10) < 120) cam.fieldOfView += 10;
                userCameraController.field_Public_AudioSource_0.PlayOneShot(userCameraController.field_Public_AudioClip_0);
            };
            SetButtonSprite(zoomOutButton, Functions.Core.Resources.zoomOut);
            SetButtonIconScale(zoomOutButton);
            SetButtonOffset(zoomOutButton);

            // Toggle Camera Indicator
            GameObject cameraHelper = userCameraController.transform.Find("PhotoCamera/camera_lens_mesh").gameObject;
            toggleCameraIndicatorButton = GameObject.Instantiate(userCameraController.transform.Find("ViewFinder/PhotoControls/Right_Timer").gameObject, userCameraController.transform.Find("ViewFinder"));
            Components.VRC_UdonTrigger toggleCamTrig = toggleCameraIndicatorButton.AddComponent<Components.VRC_UdonTrigger>();
            toggleCamTrig.InteractText = "Camera Indicator";
            toggleCamTrig.OnInteract = () => 
            {
                cameraHelper.SetActive(!cameraHelper.activeSelf);
                if (cameraHelper.activeSelf)
                    SetButtonSprite(toggleCameraIndicatorButton, Functions.Core.Resources.lensOn);
                else
                    SetButtonSprite(toggleCameraIndicatorButton, Functions.Core.Resources.lensOff);
                userCameraController.field_Public_AudioSource_0.PlayOneShot(userCameraController.field_Public_AudioClip_0);
            };

            SetButtonSprite(toggleCameraIndicatorButton, Functions.Core.Resources.lensOn);
            SetButtonIconScale(toggleCameraIndicatorButton);
            SetButtonOffset(toggleCameraIndicatorButton);

            // Resize Camera Body
            camera_body = userCameraController.transform.Find("ViewFinder/camera_mesh/body");
            camera_body.localPosition = camera_body.localPosition + new Vector3(-0.025f, 0, 0);
            camera_body.localScale = camera_body.localScale + new Vector3(0.8f, 0, 0);
            SetCameraPlus();
        }
        private static void SetButtonOffset(GameObject button) { button.transform.localPosition = button.transform.localPosition + new Vector3(-0.05f, 0, 0); }
        private static void SetButtonIconScale(GameObject button)
        {
            foreach (Transform trans in button.GetComponentsInChildren<Transform>(true))
            {
                if ((trans != null) && trans.gameObject.name.StartsWith("Icon"))
                    trans.localScale *= 1.25f;
            }
        }
        private static void SetButtonSprite(GameObject button, Sprite sp)
        {
            foreach (Transform trans in button.GetComponentsInChildren<Transform>(true))
            {
                if ((trans != null) && trans.gameObject.name.StartsWith("Icon"))
                    trans.GetComponent<SpriteRenderer>().sprite = sp;
            }
        }
        public static void SetCameraPlus()
        {
            if (Configuration.JSONConfig.CameraPlus)
            {
                camera_body.localPosition = new Vector3(-0.025f, 0f, 0f);
                camera_body.localScale = new Vector3(6.3317f, camera_body.localScale.y, camera_body.localScale.z);
                zoomInButton.SetActive(true);
                zoomOutButton.SetActive(true);
                toggleCameraIndicatorButton.SetActive(true);
            }
            else
            {
                camera_body.localPosition = Vector3.zero;
                camera_body.localScale = new Vector3(5.5317f, camera_body.localScale.y, camera_body.localScale.z);
                zoomInButton.SetActive(false);
                zoomOutButton.SetActive(false);
                toggleCameraIndicatorButton.SetActive(false);
            }
        }
    }
}
