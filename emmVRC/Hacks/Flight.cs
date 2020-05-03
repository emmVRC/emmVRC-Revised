using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace emmVRC.Hacks
{
    public class Flight
    {
        public static bool FlightEnabled = false;
        public static bool NoclipEnabled = false;
        private static GameObject localPlayer;
        private static Vector3 originalGravity;
        public static void Initialize()
        {
            MelonLoader.MelonCoroutines.Start(Loop());
        }
        public static IEnumerator Loop()
        {
            while (true)
            {
                
                if (RoomManager.field_ApiWorld_0 != null)
                {
                    if (localPlayer == null && VRCPlayer.field_VRCPlayer_0 != null && VRCPlayer.field_VRCPlayer_0.gameObject != null)
                        localPlayer = VRCPlayer.field_VRCPlayer_0.gameObject;
                    else
                    {
                        if (FlightEnabled && originalGravity == Vector3.zero)
                        {
                            originalGravity = Physics.gravity;
                            Physics.gravity = Vector3.zero;
                        }
                        if (!FlightEnabled && originalGravity != Vector3.zero)
                        {
                            Physics.gravity = originalGravity;
                            originalGravity = Vector3.zero;
                        }
                        if (!FlightEnabled && NoclipEnabled)
                        {
                            NoclipEnabled = false;
                        }
                        if (FlightEnabled)
                        {
                            var cameraRotation = Camera.main.transform;

                            if (Input.GetKey(KeyCode.W))
                                localPlayer.transform.position += cameraRotation.transform.forward * (Time.deltaTime * (Input.GetKey(KeyCode.LeftShift) ? 2f : 1f));
                            if (Input.GetKey(KeyCode.S))
                                localPlayer.transform.position -= cameraRotation.transform.forward * (Time.deltaTime * (Input.GetKey(KeyCode.LeftShift) ? 2f : 1f));
                            if (Input.GetKey(KeyCode.A))
                                localPlayer.transform.position -= cameraRotation.transform.right * (Time.deltaTime * (Input.GetKey(KeyCode.LeftShift) ? 2f : 1f));
                            if (Input.GetKey(KeyCode.D))
                                localPlayer.transform.position += cameraRotation.transform.right * (Time.deltaTime * (Input.GetKey(KeyCode.LeftShift) ? 2f : 1f));
                            if (Input.GetKey(KeyCode.Q))
                                localPlayer.transform.position = new Vector3(localPlayer.transform.position.x, localPlayer.transform.position.y - (Time.deltaTime * (Input.GetKey(KeyCode.LeftShift) ? 4f : 2f)), localPlayer.transform.position.z);
                            if (Input.GetKey(KeyCode.E))
                                localPlayer.transform.position = new Vector3(localPlayer.transform.position.x, localPlayer.transform.position.y + (Time.deltaTime * (Input.GetKey(KeyCode.LeftShift) ? 4f : 2f)), localPlayer.transform.position.z);

                            if (localPlayer.GetComponent<VRCMotionState>().field_CharacterController_0 != null)
                                localPlayer.GetComponent<VRCMotionState>().field_CharacterController_0.enabled = !NoclipEnabled;

                            if (Input.GetAxis("Vertical") != 0)
                                localPlayer.transform.position += cameraRotation.transform.forward * (Time.deltaTime) * Input.GetAxis("Vertical");
                            if (Input.GetAxis("Horizontal") != 0)
                                localPlayer.transform.position += cameraRotation.transform.right * (Time.deltaTime) * Input.GetAxis("Horizontal");

                            //VRCPlayer.field_VRCPlayer_0.Method_Public_Vector3_Quaternion_0(localPlayer.transform.position, localPlayer.transform.rotation);
                            if (NoclipEnabled)
                            {
                                Vector3 thing = localPlayer.transform.position - VRCTrackingManager.Method_Public_21();
                                Quaternion thing2 = localPlayer.transform.rotation * Quaternion.Inverse(VRCTrackingManager.Method_Public_16());
                                VRCTrackingManager.Method_Public_Vector3_Quaternion_0(thing, thing2);
                            }
                            if (localPlayer.GetComponent<InputStateController>() != null)
                                localPlayer.GetComponent<InputStateController>().Method_Public_2();

                        }
                    }
                }
                yield return new WaitForFixedUpdate();

            }
        }
    }
}