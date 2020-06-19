using emmVRC.Libraries;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VRC;
using VRC.Core;
using VRC.SDKBase;

namespace emmVRC.Hacks
{
    public class Flight
    {
        public static bool FlightEnabled = false;
        public static bool NoclipEnabled = false;
        private static GameObject localPlayer;
        private static Player player;
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
                    if (player == null && RoomManager.field_Internal_Static_ApiWorld_0 != null && PlayerManager.field_Private_Static_PlayerManager_0 != null && PlayerManager.field_Private_Static_PlayerManager_0.field_Private_List_1_Player_0 != null)
                    {
                        try
                        {
                            foreach (Player plr in PlayerManager.field_Private_Static_PlayerManager_0.field_Private_List_1_Player_0)
                            {
                                if (plr != null && plr.field_Private_APIUser_0 != null)
                                    if (plr.field_Private_APIUser_0.id == APIUser.CurrentUser.id)
                                        player = plr;
                            }
                        } catch (Exception ex)
                        {
                            ex = new Exception();
                        }
                    }
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
                            if (VRCTrackingManager.Method_Public_Static_Boolean_9() && Configuration.JSONConfig.VRFlightControls) // VR is enabled
                            {
                                if (Input.GetAxis("Vertical") != 0)
                                    localPlayer.transform.position += localPlayer.transform.forward * (Time.deltaTime) * Input.GetAxis("Vertical") * (Speed.SpeedModified ? Speed.Modifier : 1f) * 2f;
                                if (Input.GetAxis("Horizontal") != 0)
                                    localPlayer.transform.position += localPlayer.transform.right * (Time.deltaTime) * Input.GetAxis("Horizontal") * (Speed.SpeedModified ? Speed.Modifier : 1f) * 2f;
                                if (Input.GetAxis("Oculus_CrossPlatform_SecondaryThumbstickVertical") != 0)
                                    localPlayer.transform.position += new Vector3(0f, Time.deltaTime * Input.GetAxis("Oculus_CrossPlatform_SecondaryThumbstickVertical") * (Speed.SpeedModified ? Speed.Modifier : 1f) * 2f);
                            }
                            else // VR is not enabled, or our check failed
                            {
                                if (Input.GetAxis("Vertical") != 0)
                                    localPlayer.transform.position += cameraRotation.transform.forward * (Time.deltaTime) * Input.GetAxis("Vertical") * (Speed.SpeedModified ? Speed.Modifier : 1f) * (Input.GetKey(KeyCode.LeftShift) ? 4f : 2f);
                                if (Input.GetAxis("Horizontal") != 0)
                                    localPlayer.transform.position += cameraRotation.transform.right * (Time.deltaTime) * Input.GetAxis("Horizontal") * (Speed.SpeedModified ? Speed.Modifier : 1f) * (Input.GetKey(KeyCode.LeftShift) ? 4f : 2f);
                                if (Input.GetKey(KeyCode.Q))
                                    localPlayer.transform.position -= new Vector3(0f, (Time.deltaTime * (Input.GetKey(KeyCode.LeftShift) ? 4f : 2f)) * (Speed.SpeedModified ? Speed.Modifier : 1f), 0f);
                                if (Input.GetKey(KeyCode.E))
                                    localPlayer.transform.position += new Vector3(0f, (Time.deltaTime * (Input.GetKey(KeyCode.LeftShift) ? 4f : 2f)) * (Speed.SpeedModified ? Speed.Modifier : 1f), 0f);
                            }

                            // Stops momentum from affecting the player during flight
                            if (localPlayer.GetComponent<VRCMotionState>() != null)
                                localPlayer.GetComponent<VRCMotionState>().Method_Public_Void_5();

                            // Disable the character controller during noclip, in order to allow the character to pass through colliders
                            if (localPlayer.GetComponent<VRCMotionState>().field_Private_CharacterController_0 != null)
                                localPlayer.GetComponent<VRCMotionState>().field_Private_CharacterController_0.enabled = !NoclipEnabled;

                            // Locks the player's body into place when flying. Without the noclip check, this also leaves the player's body behind when in VR
                            if (localPlayer.GetComponent<InputStateController>() != null && !NoclipEnabled)
                                localPlayer.GetComponent<InputStateController>().Method_Public_Void_0();
                            
                        }
                    }
                }
                yield return new WaitForEndOfFrame();

            }
        }
    }
}