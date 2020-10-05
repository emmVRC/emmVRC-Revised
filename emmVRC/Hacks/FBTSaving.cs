using emmVRC.Libraries;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UnityEngine;

namespace emmVRC.Hacks
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
    public class FBTSaving
    {
        public static List<FBTAvatarCalibrationInfo> calibratedAvatars;
        public static UnityEngine.UI.Button.ButtonClickedEvent originalCalibrateButton;
        public static void Initialize()
        {
            calibratedAvatars = new List<FBTAvatarCalibrationInfo>();
            originalCalibrateButton = QuickMenuUtils.GetQuickMenuInstance().transform.Find("ShortcutMenu/CalibrateButton").GetComponent<UnityEngine.UI.Button>().onClick;
            QuickMenuUtils.GetQuickMenuInstance().transform.Find("ShortcutMenu/CalibrateButton").GetComponent<UnityEngine.UI.Button>().onClick = new UnityEngine.UI.Button.ButtonClickedEvent();
            QuickMenuUtils.GetQuickMenuInstance().transform.Find("ShortcutMenu/CalibrateButton").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(new System.Action(() => { 
                if (Configuration.JSONConfig.TrackingSaving)
                    if (VRCPlayer.field_Internal_Static_VRCPlayer_0.prop_VRCAvatarManager_0 != null && calibratedAvatars.FindIndex(a => a.AvatarID == VRCPlayer.field_Internal_Static_VRCPlayer_0.prop_VRCAvatarManager_0.field_Private_ApiAvatar_0.id) != -1)
                        calibratedAvatars.RemoveAll(a => a.AvatarID == VRCPlayer.field_Internal_Static_VRCPlayer_0.prop_VRCAvatarManager_0.field_Private_ApiAvatar_0.id);
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
                LeftFootTrackerPosition = trackingSteam.leftFoot.localPosition,
                LeftFootTrackerRotation = trackingSteam.leftFoot.localRotation,
                RightFootTrackerPosition = trackingSteam.rightFoot.localPosition,
                RightFootTrackerRotation = trackingSteam.rightFoot.localRotation,
                HipTrackerPosition = trackingSteam.hip.localPosition,
                HipTrackerRotation = trackingSteam.hip.localRotation,
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
                trackingSteam.leftFoot.localPosition = info.LeftFootTrackerPosition;
                trackingSteam.leftFoot.localRotation = info.LeftFootTrackerRotation;
                trackingSteam.rightFoot.localPosition = info.RightFootTrackerPosition;
                trackingSteam.rightFoot.localRotation = info.RightFootTrackerRotation;
                trackingSteam.hip.localPosition = info.HipTrackerPosition;
                trackingSteam.hip.localRotation = info.HipTrackerRotation;
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
