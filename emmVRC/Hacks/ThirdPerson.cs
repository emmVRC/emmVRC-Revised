using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using emmVRC.Libraries;
namespace emmVRC.Hacks
{
    class ThirdPerson
    {
        internal static GameObject TPCameraBack = null;
        internal static GameObject TPCameraFront = null;
        internal static GameObject referenceCamera = null;
        internal static float zoomOffset = 0f;
        internal static float offsetX = 0f;
        internal static float offsetY = 0f;
        public static int CameraSetup = 0;

        // 0 = Vanilla
        // 1 = Back
        // 2 = Front


        internal static void Initialize()
        {
            GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            GameObject.Destroy(gameObject.GetComponent<MeshRenderer>());
            referenceCamera = GameObject.Find("Camera (eye)");
            if (referenceCamera != null)
            {
                gameObject.transform.localScale = referenceCamera.transform.localScale;
                Rigidbody rigidbody = gameObject.AddComponent<Rigidbody>();
                rigidbody.isKinematic = true;
                rigidbody.useGravity = false;
                if (gameObject.GetComponent<Collider>())
                {
                    gameObject.GetComponent<Collider>().enabled = false;
                }
                gameObject.GetComponent<Renderer>().enabled = false;
                gameObject.AddComponent<Camera>();
                GameObject gameObject2 = referenceCamera;
                gameObject.transform.parent = gameObject2.transform;
                gameObject.transform.rotation = gameObject2.transform.rotation;
                gameObject.transform.position = gameObject2.transform.position;
                gameObject.transform.position -= gameObject.transform.forward * 2f;
                gameObject2.GetComponent<Camera>().enabled = false;
                gameObject.GetComponent<Camera>().fieldOfView = 75f;
                TPCameraBack = gameObject;
                GameObject gameObject3 = GameObject.CreatePrimitive(PrimitiveType.Cube);
                GameObject.Destroy(gameObject3.GetComponent<MeshRenderer>());
                gameObject3.transform.localScale = referenceCamera.transform.localScale;
                Rigidbody rigidbody2 = gameObject3.AddComponent<Rigidbody>();
                rigidbody2.isKinematic = true;
                rigidbody2.useGravity = false;
                if (gameObject3.GetComponent<Collider>())
                {
                    gameObject3.GetComponent<Collider>().enabled = false;
                }
                gameObject3.GetComponent<Renderer>().enabled = false;
                gameObject3.AddComponent<Camera>();
                gameObject3.transform.parent = gameObject2.transform;
                gameObject3.transform.rotation = gameObject2.transform.rotation;
                gameObject3.transform.Rotate(0, 180, 0);
                gameObject3.transform.position = gameObject2.transform.position;
                gameObject3.transform.position += -gameObject3.transform.forward * 2f;
                gameObject2.GetComponent<Camera>().enabled = false;
                gameObject3.GetComponent<Camera>().fieldOfView = 75f;
                TPCameraFront = gameObject3;
                TPCameraBack.GetComponent<Camera>().enabled = false;
                TPCameraFront.GetComponent<Camera>().enabled = false;
                GameObject.Find("Camera (eye)").GetComponent<Camera>().enabled = true;
                /*thirdPersonBack = new QMSingleButton("CameraMenu", 4, 0, "Third\nPerson\nBack View", delegate
                {
                    CameraSetup = 1;
                }, "Switches your perspective to third-person, facing your back. Press CTRL+T or move the joystick to revert", configUtils.buttonColor());
                thirdPersonFront = new QMSingleButton("CameraMenu", 4, 1, "Third\nPerson\nFront View", delegate
                {
                    CameraSetup = 2;
                }, "Switches your perspective to third-person, facing your front. Press CTRL+T or move the joystick to revert", configUtils.buttonColor());
                setup = true;*/
                MelonLoader.MelonCoroutines.Start(Loop());
            }
        }

        internal static IEnumerator Loop()
        {
            while (true)
            {
                try
                {
                    if (TPCameraBack != null && TPCameraFront != null)
                    {
                        if (CameraSetup == 0)
                        {
                            GameObject.Find("Camera (eye)").GetComponent<Camera>().enabled = true;
                            TPCameraBack.GetComponent<Camera>().enabled = false;
                            TPCameraFront.GetComponent<Camera>().enabled = false;
                        }
                        else if (CameraSetup == 1)
                        {
                            GameObject.Find("Camera (eye)").GetComponent<Camera>().enabled = false;
                            TPCameraBack.GetComponent<Camera>().enabled = true;
                            TPCameraFront.GetComponent<Camera>().enabled = false;
                        }
                        else if (CameraSetup == 2)
                        {
                            GameObject.Find("Camera (eye)").GetComponent<Camera>().enabled = false;
                            TPCameraBack.GetComponent<Camera>().enabled = false;
                            TPCameraFront.GetComponent<Camera>().enabled = true;
                        }
                        if (CameraSetup != 0)
                        {
                            if (Input.GetKeyDown(KeyCode.Escape))
                                CameraSetup = 0;
                            var d = Input.GetAxis("Mouse ScrollWheel");
                            if (d > 0f)
                            {
                                TPCameraBack.transform.position += TPCameraBack.transform.forward * 0.1f;
                                TPCameraFront.transform.position -= TPCameraBack.transform.forward * 0.1f;
                                zoomOffset += 0.1f;
                            }
                            else if (d < 0f)
                            {
                                TPCameraBack.transform.position -= TPCameraBack.transform.forward * 0.1f;
                                TPCameraFront.transform.position += TPCameraBack.transform.forward * 0.1f;
                                zoomOffset -= 0.1f;
                            }
                        }
                    }
                } catch (Exception Ex)
                {
                    emmVRCLoader.Logger.Log(Ex.ToString()); 
                }
                yield return new WaitForEndOfFrame();
            }
        }
    }
}
