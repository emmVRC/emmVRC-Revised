using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VRC.Core;
using VRC;
using VRC.UI;
using VRC.UserCamera;

namespace emmVRC.Hacks
{
    public class CameraPlus
    {
        public static void Initialize()
        {
            // Grab Controller
            UserCameraController userCameraController = UnityEngine.Resources.FindObjectsOfTypeAll<UserCameraController>()[0];

            // Create Sprites
            //Sprite zoomin_sprite = CreateSprite(ImageData.zoomin_image);
            //Sprite zoomout_sprite = CreateSprite(ImageData.zoomout_image);
            //Sprite cameraindicator_on_sprite = CreateSprite(ImageData.cameraindicator_on_image);
            //Sprite cameraindicator_off_sprite = CreateSprite(ImageData.cameraindicator_off_image);

            // Zoom-In
            GameObject zoomInButton = GameObject.Instantiate(userCameraController.transform.Find("ViewFinder/PhotoControls/Right_Filters").gameObject, userCameraController.transform.Find("ViewFinder"));
            Objects.VRC_UdonTrigger.Instantiate(zoomInButton, "Zoom-In", () =>
            {
                Camera cam = userCameraController.transform.Find("PhotoCamera").GetComponent<Camera>();
                if ((cam.fieldOfView - 10) > 0) cam.fieldOfView -= 10;
                cam = userCameraController.transform.Find("PhotoCamera/VideoCamera").GetComponent<Camera>();
                if ((cam.fieldOfView - 10) > 0) cam.fieldOfView -= 10;
                userCameraController.field_Public_AudioSource_0.PlayOneShot(userCameraController.field_Public_AudioClip_0);
            });
            SetButtonSprite(zoomInButton, Resources.zoomIn);
            SetButtonIconScale(zoomInButton);
            SetButtonOffset(zoomInButton);

            // Zoom-Out
            GameObject zoomOutButton = GameObject.Instantiate(userCameraController.transform.Find("ViewFinder/PhotoControls/Right_Lock").gameObject, userCameraController.transform.Find("ViewFinder"));
            Objects.VRC_UdonTrigger.Instantiate(zoomOutButton,"Zoom-Out", () =>
            {
                Camera cam = userCameraController.transform.Find("PhotoCamera").GetComponent<Camera>();
                if ((cam.fieldOfView + 10) < 180) cam.fieldOfView += 10;
                cam = userCameraController.transform.Find("PhotoCamera/VideoCamera").GetComponent<Camera>();
                if ((cam.fieldOfView + 10) < 180) cam.fieldOfView += 10;
                userCameraController.field_Public_AudioSource_0.PlayOneShot(userCameraController.field_Public_AudioClip_0);
            });
            SetButtonSprite(zoomOutButton, Resources.zoomOut);
            SetButtonIconScale(zoomOutButton);
            SetButtonOffset(zoomOutButton);

            // Toggle Camera Indicator
            GameObject cameraHelper = userCameraController.transform.Find("PhotoCamera/camera_lens_mesh").gameObject;
            GameObject toggleCameraIndicatorButton = GameObject.Instantiate(userCameraController.transform.Find("ViewFinder/PhotoControls/Right_Timer").gameObject, userCameraController.transform.Find("ViewFinder"));
            Objects.VRC_UdonTrigger.Instantiate(toggleCameraIndicatorButton, "Camera Indicator", () =>
            {
                cameraHelper.SetActive(!cameraHelper.activeSelf);
                if (cameraHelper.activeSelf)
                    SetButtonSprite(toggleCameraIndicatorButton, Resources.lensOn);
                else
                    SetButtonSprite(toggleCameraIndicatorButton, Resources.lensOff);
                userCameraController.field_Public_AudioSource_0.PlayOneShot(userCameraController.field_Public_AudioClip_0);
            });
            SetButtonSprite(toggleCameraIndicatorButton, Resources.lensOn);
            SetButtonIconScale(toggleCameraIndicatorButton);
            SetButtonOffset(toggleCameraIndicatorButton);

            // Resize Camera Body
            Transform camera_body = userCameraController.transform.Find("ViewFinder/camera_mesh/body");
            camera_body.localPosition = camera_body.localPosition + new Vector3(-0.025f, 0, 0);
            camera_body.localScale = camera_body.localScale + new Vector3(0.8f, 0, 0);
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
    }
}
