using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using emmVRC.Libraries;
using emmVRC.Objects.ModuleBases;

namespace emmVRC.Functions.View
{
    class Zoom : MelonLoaderEvents, IWithUpdate
    {
        internal static GameObject referenceCamera = null;
        internal static bool zooming;
        internal static float originalZoom = 0f;
        private static bool _initialized;


        public override void OnUiManagerInit()
        {
            referenceCamera = GameObject.Find("Camera (eye)");
            if (referenceCamera == null)
                referenceCamera = GameObject.Find("CenterEyeAnchor");
        }
        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            if (buildIndex != -1 || _initialized) return;
            Components.EnableDisableListener menuListener = Utils.ButtonAPI.GetQuickMenuInstance().gameObject.AddComponent<Components.EnableDisableListener>();
            menuListener.OnEnabled += () =>
            {
                zooming = false;
            };
            _initialized = true;
        }
        public void OnUpdate()
        {
            if (referenceCamera == null || referenceCamera.GetComponent<Camera>() == null) return;
            if (Input.GetMouseButtonDown(2)) {
                zooming = !zooming;
                if (zooming && originalZoom != 0f && referenceCamera.GetComponent<Camera>().fieldOfView != originalZoom)
                    referenceCamera.GetComponent<Camera>().fieldOfView = originalZoom;
                if (zooming)
                    originalZoom = referenceCamera.GetComponent<Camera>().fieldOfView;
            }
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
            {
                zooming = false;
            }
            if (zooming && referenceCamera.GetComponent<Camera>().fieldOfView > originalZoom - 45f)
                referenceCamera.GetComponent<Camera>().fieldOfView = Mathf.Lerp(referenceCamera.GetComponent<Camera>().fieldOfView, originalZoom - 45f, Time.deltaTime * 10f);
            if (!zooming && referenceCamera.GetComponent<Camera>().fieldOfView < originalZoom)
                referenceCamera.GetComponent<Camera>().fieldOfView = Mathf.Lerp(referenceCamera.GetComponent<Camera>().fieldOfView, originalZoom, Time.deltaTime * 10f);
        }
    }
}
