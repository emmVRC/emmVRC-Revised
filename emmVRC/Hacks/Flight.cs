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

                if (RoomManager.field_Internal_Static_ApiWorld_0 != null)
                {
                    if (localPlayer == null && VRCPlayer.field_Internal_Static_VRCPlayer_0 != null && VRCPlayer.field_Internal_Static_VRCPlayer_0.gameObject != null)
                        localPlayer = VRCPlayer.field_Internal_Static_VRCPlayer_0.gameObject;
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
                            if (VRCTrackingManager.Method_Public_Static_Boolean_11()) // VR is enabled
                            {
                                if (Input.GetAxis("Vertical") != 0)
                                    localPlayer.transform.position += localPlayer.transform.forward * (Time.deltaTime) * Input.GetAxis("Vertical") * (Speed.SpeedModified ? Speed.Modifier : 1f) * 2f;
                                if (Input.GetAxis("Horizontal") != 0)
                                    localPlayer.transform.position += localPlayer.transform.right * (Time.deltaTime) * Input.GetAxis("Horizontal") * (Speed.SpeedModified ? Speed.Modifier : 1f) * 2f;
                                if (Input.GetAxis("Oculus_CrossPlatform_SecondaryThumbstickVertical") != 0)
                                    localPlayer.transform.position += new Vector3(0f, Time.deltaTime * Input.GetAxis("Oculus_CrossPlatform_SecondaryThumbstickVertical") * (Speed.SpeedModified ? Speed.Modifier : 1f) * 2f);
                            }
                            else
                            {
                                if (Input.GetAxis("Vertical") != 0)
                                    localPlayer.transform.position += cameraRotation.transform.forward * (Time.deltaTime) * Input.GetAxis("Vertical") * (Speed.SpeedModified ? Speed.Modifier : 1f) * (Input.GetKey(KeyCode.LeftShift) ? 4f : 2f);
                                if (Input.GetAxis("Horizontal") != 0)
                                    localPlayer.transform.position += cameraRotation.transform.right * (Time.deltaTime) * Input.GetAxis("Horizontal") * (Speed.SpeedModified ? Speed.Modifier : 1f) * (Input.GetKey(KeyCode.LeftShift) ? 4f : 2f);
                                if (Input.GetKey(KeyCode.Q))
                                    localPlayer.transform.position -= new Vector3(0f, (Time.deltaTime * (Input.GetKey(KeyCode.LeftShift) ? 4f : 2f)), 0f);
                                if (Input.GetKey(KeyCode.E))
                                    localPlayer.transform.position += new Vector3(0f, (Time.deltaTime * (Input.GetKey(KeyCode.LeftShift) ? 4f : 2f)), 0f);
                            }
                                

                            if (localPlayer.GetComponent<VRCMotionState>().field_Private_CharacterController_0 != null)
                                localPlayer.GetComponent<VRCMotionState>().field_Private_CharacterController_0.enabled = !NoclipEnabled;
                            //if (localPlayer.GetComponent<VRCMotionState>() != null)
                            //    localPlayer.GetComponent<VRCMotionState>().Method_Public_Void_3();
                            
                            

                            //VRCPlayer.field_Internal_Static_VRCPlayer_0.Method_Public_Vector3_Quaternion_0(localPlayer.transform.position, localPlayer.transform.rotation);
                            if (NoclipEnabled)
                            {
                                Vector3 thing = localPlayer.transform.position - VRCTrackingManager.Method_Public_Static_Vector3_1();
                                Quaternion thing2 = localPlayer.transform.rotation * Quaternion.Inverse(VRCTrackingManager.Method_Public_Static_Quaternion_0());
                                VRCTrackingManager.Method_Public_Static_Void_Vector3_Quaternion_0(thing, thing2);
                            }
                            if (localPlayer.GetComponent<InputStateController>() != null)
                                localPlayer.GetComponent<InputStateController>().Method_Public_Void_0();

                        }
                    }
                }
                yield return new WaitForFixedUpdate();

            }
        }
    }
}