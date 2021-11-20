using emmVRC.Libraries;
using emmVRC.Objects.ModuleBases;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace emmVRC.Functions.PlayerHacks
{
    public class FBTAvatarCalibrationInfo
    {
        public string AvatarID;
        public Vector3 LeftFootTrackerPosition;
        public Vector3 RightFootTrackerPosition;
        public Vector3 HipTrackerPosition;
        public Quaternion LeftFootTrackerRotation;
        public Quaternion RightFootTrackerRotation;
        public Quaternion HipTrackerRotation;
        public float PlayerHeight;
    }
    [Priority(55)]
    public class FBTSaving : MelonLoaderEvents
    {
        public static List<FBTAvatarCalibrationInfo> calibratedAvatars;
        public static UnityEngine.UI.Button.ButtonClickedEvent originalCalibrateButton;
        public static System.Reflection.PropertyInfo leftFootInf;
        public static System.Reflection.PropertyInfo rightFootInf;
        public static System.Reflection.PropertyInfo hipInf;

        private static bool _initialized = false;
        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            if (buildIndex != -1 || _initialized) return;
            if (Environment.CurrentDirectory.Contains("vrchat-vrchat")) return; // Really awful and crude Oculus check
            SetupFBTSaving();
            _initialized = true;
        }
        private static void SetupFBTSaving()
        {
            calibratedAvatars = new List<FBTAvatarCalibrationInfo>();
            originalCalibrateButton = Utils.ButtonAPI.menuPageBase.transform.Find("ScrollRect/Viewport/VerticalLayoutGroup/Buttons_QuickActions").Find("SitStandCalibrateButton/Button_CalibrateFBT").GetComponentInChildren<UnityEngine.UI.Button>().onClick;

            VRCTrackingSteam steam = UnityEngine.Resources.FindObjectsOfTypeAll<VRCTrackingSteam>().First();
            leftFootInf = typeof(VRCTrackingSteam).GetProperties().First(a => a.PropertyType == typeof(Transform) && ((Transform)a.GetValue(steam)).parent.name == "Puck1");
            rightFootInf = typeof(VRCTrackingSteam).GetProperties().First(a => a.PropertyType == typeof(Transform) && ((Transform)a.GetValue(steam)).parent.name == "Puck2");
            hipInf = typeof(VRCTrackingSteam).GetProperties().First(a => a.PropertyType == typeof(Transform) && ((Transform)a.GetValue(steam)).parent.name == "Puck3");

            Utils.ButtonAPI.menuPageBase.transform.Find("ScrollRect/Viewport/VerticalLayoutGroup/Buttons_QuickActions").Find("SitStandCalibrateButton/Button_CalibrateFBT").GetComponentInChildren<UnityEngine.UI.Button>().onClick = new UnityEngine.UI.Button.ButtonClickedEvent();
            Utils.ButtonAPI.menuPageBase.transform.Find("ScrollRect/Viewport/VerticalLayoutGroup/Buttons_QuickActions").Find("SitStandCalibrateButton/Button_CalibrateFBT").GetComponentInChildren<UnityEngine.UI.Button>().onClick.AddListener(new System.Action(() =>
            {
                if (Configuration.JSONConfig.TrackingSaving && VRCPlayer.field_Internal_Static_VRCPlayer_0.prop_VRCAvatarManager_0 != null)
                {
                    try
                    {

                        VRC.Core.ApiAvatar targetAvtr;
                        if (VRCPlayer.field_Internal_Static_VRCPlayer_0.prop_VRCAvatarManager_0.field_Private_ApiAvatar_1 != null)
                            targetAvtr = (VRCPlayer.field_Internal_Static_VRCPlayer_0.prop_VRCAvatarManager_0.field_Private_ApiAvatar_1);
                        else
                            targetAvtr = (VRCPlayer.field_Internal_Static_VRCPlayer_0.prop_VRCAvatarManager_0.field_Private_ApiAvatar_0);
                        if (calibratedAvatars.FindIndex(a => a.AvatarID == targetAvtr.id) != -1)
                            calibratedAvatars.RemoveAll(a => a.AvatarID == targetAvtr.id);


                    }
                    catch (Exception ex)
                    {
                        originalCalibrateButton.Invoke();
                        emmVRCLoader.Logger.LogError("An error occured with FBT Saving. Invoking original calibration button. Error: " + ex.ToString());
                    }
                }
                originalCalibrateButton.Invoke();
            }));
        }
        public static IEnumerator SaveCalibrationInfo(VRCTrackingSteam trackingSteam, string avatarID)
        {
            yield return new WaitForSeconds(1f);
            if (calibratedAvatars.FindIndex(a => a.AvatarID == avatarID) != -1)
                calibratedAvatars.RemoveAll(a => a.AvatarID == avatarID);
            FBTAvatarCalibrationInfo info = new FBTAvatarCalibrationInfo
            {
                AvatarID = avatarID,
                LeftFootTrackerPosition = ((Transform)leftFootInf.GetValue(trackingSteam)).localPosition,
                LeftFootTrackerRotation = ((Transform)leftFootInf.GetValue(trackingSteam)).localRotation,
                RightFootTrackerPosition = ((Transform)rightFootInf.GetValue(trackingSteam)).localPosition,
                RightFootTrackerRotation = ((Transform)rightFootInf.GetValue(trackingSteam)).localRotation,
                HipTrackerPosition = ((Transform)hipInf.GetValue(trackingSteam)).localPosition,
                HipTrackerRotation = ((Transform)hipInf.GetValue(trackingSteam)).localRotation,

                //LeftFootTrackerPosition = trackingSteam.leftFoot.localPosition,
                //LeftFootTrackerRotation = trackingSteam.leftFoot.localRotation,
                //RightFootTrackerPosition = trackingSteam.rightFoot.localPosition,
                //RightFootTrackerRotation = trackingSteam.rightFoot.localRotation,
                //HipTrackerPosition = trackingSteam.hip.localPosition,
                //HipTrackerRotation = trackingSteam.hip.localRotation,

                PlayerHeight = VRCTrackingManager.field_Private_Static_VRCTrackingManager_0.GetPlayerHeight()
            };
            calibratedAvatars.Add(info);
            emmVRCLoader.Logger.LogDebug("Saved calibration info");
        }
        public static void LoadCalibrationInfo(VRCTrackingSteam trackingSteam, string avatarID)
        {
            if (calibratedAvatars.FindIndex(a => a.AvatarID == avatarID) != -1)
            {
                FBTAvatarCalibrationInfo info = calibratedAvatars.Find(a => a.AvatarID == avatarID);

                ((Transform)leftFootInf.GetValue(trackingSteam)).localPosition = info.LeftFootTrackerPosition;
                ((Transform)leftFootInf.GetValue(trackingSteam)).localRotation = info.LeftFootTrackerRotation;
                ((Transform)rightFootInf.GetValue(trackingSteam)).localPosition = info.RightFootTrackerPosition;
                ((Transform)rightFootInf.GetValue(trackingSteam)).localRotation = info.RightFootTrackerRotation;
                ((Transform)hipInf.GetValue(trackingSteam)).localPosition = info.HipTrackerPosition;
                ((Transform)hipInf.GetValue(trackingSteam)).localRotation = info.HipTrackerRotation;
                VRCTrackingManager.field_Private_Static_VRCTrackingManager_0.SetPlayerHeight(info.PlayerHeight);
            }
            emmVRCLoader.Logger.LogDebug("Loaded calibration info");
        }
        public static bool IsPreviouslyCalibrated(string avatarID)
        {
            return (calibratedAvatars.FindIndex(a => a.AvatarID == avatarID) != -1);
        }
    }
}
