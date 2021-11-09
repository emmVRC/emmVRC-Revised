using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnhollowerRuntimeLib.XrefScans;
using VRC.UI;
using UnityEngine;
using VRC;
using VRC.Animation;
using VRC.Core;
using TMPro;

namespace emmVRC.Libraries
{
    public static class Reflections
    {
        #region InputStateController ResetLastPosition
        public delegate void ResetLastPositionAction(InputStateController @this);

        private static ResetLastPositionAction ourResetLastPositionAction;

        public static ResetLastPositionAction ResetLastPositionAct
        {
            get
            {
                if (ourResetLastPositionAction != null) return ourResetLastPositionAction;
                var targetMethod = typeof(InputStateController).GetMethods(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | BindingFlags.DeclaredOnly)
                    .Single(it => XrefScanner.XrefScan(it).Any(jt => jt.Type == XrefType.Method && jt.TryResolve() != null && jt.TryResolve().Name == "get_transform"));

                ourResetLastPositionAction = (ResetLastPositionAction)Delegate.CreateDelegate(typeof(ResetLastPositionAction), targetMethod);

                return ourResetLastPositionAction;
            }
        }

        public static void ResetLastPosition(this InputStateController instance)
        {
            ResetLastPositionAct.Invoke(instance);
        }
        #endregion

        #region VRCMotionState Reset
        public delegate void ResetAction(VRCMotionState @this);

        private static ResetAction ourResetAction;

        public static ResetAction ResetAct
        {
            get
            {
                if (ourResetAction != null) return ourResetAction;
                var targetMethod = typeof(VRCMotionState).GetMethods(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | BindingFlags.DeclaredOnly)
                    .Single(it => XrefScanner.XrefScan(it).Count(jt => jt.Type == XrefType.Method && jt.TryResolve() != null && jt.TryResolve().ReflectedType == typeof(Vector3)) == 4);

                ourResetAction = (ResetAction)Delegate.CreateDelegate(typeof(ResetAction), targetMethod);

                return ourResetAction;
            }
        }

        public static void Reset(this VRCMotionState instance, bool something = false)
        {
            ResetAct.Invoke(instance);
        }
        #endregion

        #region VRCPlayer ReloadAvatar
        public delegate void ReloadAvatarAction(VRCPlayer @this, bool something = false);

        private static ReloadAvatarAction ourReloadAvatarAction;

        public static ReloadAvatarAction ReloadAvatarAct
        {
            get
            {
                if (ourReloadAvatarAction != null) return ourReloadAvatarAction;
                var targetMethod = typeof(VRCPlayer).GetMethods(BindingFlags.Public | BindingFlags.Instance)
                    .Single(it => it != null && it.ReturnType == typeof(void) && it.GetParameters().Length == 1 && it.GetParameters()[0].ParameterType == typeof(bool) && XrefScanner.XrefScan(it).Any(jt => jt.Type == XrefType.Global && jt.ReadAsObject()?.ToString() == "Switching {0} to avatar {1}"));
                ourReloadAvatarAction = (ReloadAvatarAction)Delegate.CreateDelegate(typeof(ReloadAvatarAction), targetMethod);

                return ourReloadAvatarAction;
            }
        }

        private static MethodInfo reloadAvatarsMethod;
        public static void ReloadAvatar(this VRCPlayer instance)
        {
            emmVRCLoader.Logger.LogDebug("ReloadAvatar called");
            if (reloadAvatarsMethod == null)
                reloadAvatarsMethod = typeof(VRCPlayer).GetMethods().First(mi => mi.Name.StartsWith("Method_Private_Void_Boolean_") && mi.Name.Length < 31 && mi.GetParameters().Any(pi => pi.IsOptional));
            reloadAvatarsMethod.Invoke(instance, new object[] { true });
            //ReloadAvatarAct.Invoke(instance, something);
        }
        #endregion

        #region VRCPlayer ReloadAllAvatars
        public static void ReloadAllAvatars(this VRCPlayer instance)
        {
            foreach (Player plr in PlayerManager.field_Private_Static_PlayerManager_0.field_Private_List_1_Player_0)
            {
                plr._vrcplayer.ReloadAvatar();
            }
        }
        #endregion

        #region VRCPlayer TriggerEmote
        public delegate void TriggerEmoteAction(VRCPlayer @this, int emote);

        private static TriggerEmoteAction ourTriggerEmoteAction;

        public static TriggerEmoteAction TriggerEmoteAct
        {
            get
            {
                if (ourTriggerEmoteAction != null) return ourTriggerEmoteAction;
                var targetMethod = typeof(VRCPlayer).GetMethods(BindingFlags.Public | BindingFlags.Instance)
                    .Single(it => it != null && it.ReturnType == typeof(void) && it.GetParameters().Length == 1 && it.GetParameters()[0].ParameterType == typeof(int) && XrefScanner.XrefScan(it).Any(jt => jt.Type == XrefType.Global && jt.ReadAsObject()?.ToString() == "PlayEmoteRPC"));

                ourTriggerEmoteAction = (TriggerEmoteAction)Delegate.CreateDelegate(typeof(TriggerEmoteAction), targetMethod);

                return ourTriggerEmoteAction;
            }
        }

        public static void TriggerEmote(this VRCPlayer instance, int emote)
        {
            TriggerEmoteAct.Invoke(instance, emote);
        }
        #endregion

        #region AvatarPlayableController ApplyParameters
        public delegate void ApplyParametersAction(AvatarPlayableController @this, int value);

        private static ApplyParametersAction ourApplyParametersAction;

        public static ApplyParametersAction ApplyParametersAct
        {
            get
            {
                if (ourApplyParametersAction != null) return ourApplyParametersAction;
                var targetMethod = typeof(AvatarPlayableController).GetMethods(BindingFlags.Public | BindingFlags.Instance)
                    .Single(it => it != null && it.ReturnType == typeof(void) && it.GetParameters().Length == 1 && it.GetParameters()[0].ParameterType == typeof(int) && XrefScanner.XrefScan(it).Any(jt => jt.Type == XrefType.Global && jt.ReadAsObject()?.ToString() == "Tried to clear an unassigned puppet channel!"));

                ourApplyParametersAction = (ApplyParametersAction)Delegate.CreateDelegate(typeof(ApplyParametersAction), targetMethod);

                return ourApplyParametersAction;
            }
        }

        public static void ApplyParameters(this AvatarPlayableController instance, int value)
        {
            ApplyParametersAct.Invoke(instance, value);
        }
        #endregion

        #region VRCTrackingManager GetPlayerHeight
        private static MethodInfo ourGetPlayerHeightMethod;
        public static MethodInfo getPlayerHeightMethod {
            get
            {
                if (ourGetPlayerHeightMethod != null) return ourGetPlayerHeightMethod;
                var targetMethod = typeof(VRCTrackingManager).GetMethods()
                    .Single(it => it != null && it.ReturnType == typeof(Single) && XrefScanner.XrefScan(it).Any(jt => jt.Type == XrefType.Global && jt.ReadAsObject()?.ToString() == "PlayerHeight"));
                ourGetPlayerHeightMethod = targetMethod;
                return ourGetPlayerHeightMethod;
            }
        }

        public static Single GetPlayerHeight(this VRCTrackingManager instance)
        {
            return (Single)getPlayerHeightMethod.Invoke(instance, null);
        }
        #endregion

        #region VRCTrackingManager SetPlayerHeight
        private static MethodInfo ourSetPlayerHeightMethod;
        public static MethodInfo SetPlayerHeightMethod
        {
            get
            {
                if (ourSetPlayerHeightMethod != null) return ourSetPlayerHeightMethod;
                var targetMethod = typeof(VRCTrackingManager).GetMethods()
                    .Single(it => it != null && it.ReturnType == typeof(void) && it.GetParameters().Length == 1 && it.GetParameters().First().ParameterType == typeof(Single) && XrefScanner.XrefScan(it).Any(jt => jt.Type == XrefType.Global && jt.ReadAsObject()?.ToString() == "PlayerHeight"));
                ourSetPlayerHeightMethod = targetMethod;
                return ourSetPlayerHeightMethod;
            }
        }

        public static void SetPlayerHeight(this VRCTrackingManager instance, float height)
        {
            SetPlayerHeightMethod.Invoke(instance, new object[]{ height });
        }
        #endregion

        #region VRCTrackingManager SetControllerVisibility
        private static MethodInfo ourSetControllerVisibilityMethod;
        public static MethodInfo SetControllerVisibilityMethod
        {
            get
            {
                if (ourSetControllerVisibilityMethod != null) return ourSetControllerVisibilityMethod;
                var targetMethod = typeof(VRCTrackingManager).GetMethods()
                    .Single(it => it != null && it.ReturnType == typeof(void) && it.GetParameters().Length == 1 && it.GetParameters().First().ParameterType == typeof(bool) && XrefScanner.XrefScan(it).Any(jt => jt.Type == XrefType.Method && jt.TryResolve() != null && jt.TryResolve().ReflectedType != null && jt.TryResolve().ReflectedType.Name == "Whiteboard"));
                ourSetControllerVisibilityMethod = targetMethod;
                return ourSetControllerVisibilityMethod;
            }
        }

        public static void SetControllerVisibility(this VRCTrackingManager instance, bool value)
        {
            SetControllerVisibilityMethod.Invoke(instance, new object[] { value });
        }
        #endregion

        #region MenuController ViewUserInfo
        private static MethodInfo ourViewUserInfoMethod;
        public static MethodInfo ViewUserInfoMethod
        {
            get
            {
                if (ourViewUserInfoMethod != null) return ourViewUserInfoMethod;
                var targetMethod = typeof(MenuController).GetMethods()
                    .First(it => it != null && it.GetParameters().Length == 1 && it.GetParameters().First().ParameterType == typeof(string) && XrefScanner.XrefScan(it).Any(jt => jt.Type == XrefType.Method && jt.TryResolve()?.Name == "FetchUser"));
                ourViewUserInfoMethod = targetMethod;
                return ourViewUserInfoMethod;
            }
        }

        public static void ViewUserInfo(this MenuController instance, string userID)
        {
            ViewUserInfoMethod.Invoke(instance, new object[] { userID });
        }
        #endregion

        #region VRCPlayer GetNameplateText
        public static TextMeshProUGUI GetNameplateText(this VRCPlayer player) { return player.field_Public_PlayerNameplate_0.gameObject.transform.Find("Contents/Main/Text Container/Name").GetComponent<TextMeshProUGUI>(); }
        #endregion

        #region VRCPlayer GetNameplateBackground
        public static ImageThreeSlice GetNameplateBackground(this VRCPlayer player) { return player.field_Public_PlayerNameplate_0.gameObject.transform.Find("Contents/Main/Background").GetComponent<ImageThreeSlice>(); }
        #endregion

        #region PlayerNet Framerate
        public static float GetFramerate(this PlayerNet net)
        {
            if (net.field_Private_Byte_0 == null || net.field_Private_Byte_0 == 0f) return -1f;
            return Mathf.Floor(1000f / net.field_Private_Byte_0);
        }
        public static Color LerpFramerateColor(this PlayerNet net)
        {
            if (net.GetFramerate() == -1f) return Color.grey;
            return Color.Lerp(Color.red, Color.green, (net.GetFramerate() / 100f));
        }
        #endregion
        public static GameObject menuContent(this VRCUiManager mngr)
        {
            return mngr.field_Public_GameObject_0;
        }

        private static MethodInfo enterWorldMethod;
        private static Type transitionInfoEnum;
        public static void EnterWorld(this VRCFlowManager mngr, string id, string tags)
        {
            if (enterWorldMethod == null || transitionInfoEnum == null)
            {
                transitionInfoEnum = typeof(WorldTransitionInfo).GetNestedTypes().First();
                enterWorldMethod = typeof(VRCFlowManager).GetMethod("Method_Public_Void_String_String_WorldTransitionInfo_Action_1_String_Boolean_0");
            }
            object currentPortalInfo = Activator.CreateInstance(typeof(WorldTransitionInfo), new object[2] { Enum.Parse(transitionInfoEnum, "Menu"), "WorldInfo_Go" });
            currentPortalInfo.GetType().GetProperty($"field_Public_{transitionInfoEnum.Name}_0").SetValue(currentPortalInfo, transitionInfoEnum.GetEnumValues().GetValue(3));
            enterWorldMethod.Invoke(VRCFlowManager.prop_VRCFlowManager_0, new object[5] { id, tags, currentPortalInfo, null, false });
        }

    }
}
