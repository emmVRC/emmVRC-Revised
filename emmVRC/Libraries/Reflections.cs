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

                UnhollowerRuntimeLib.XrefScans.XrefScanMethodDb.RegisterType<Transform>();
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

                UnhollowerRuntimeLib.XrefScans.XrefScanMethodDb.RegisterType<Transform>();
                UnhollowerRuntimeLib.XrefScans.XrefScanMethodDb.RegisterType<Vector3>();
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

        public static void ReloadAvatar(this VRCPlayer instance, bool ignoreSelf = false)
        {
            instance.Method_Public_Void_Boolean_0(ignoreSelf);
            //ReloadAvatarAct.Invoke(instance, something);
        }
        #endregion

        #region VRCPlayer ReloadAllAvatars
        public static void ReloadAllAvatars(this VRCPlayer instance)
        {
            foreach (Player plr in PlayerManager.field_Private_Static_PlayerManager_0.field_Private_List_1_Player_0)
            {
                plr.field_Internal_VRCPlayer_0.ReloadAvatar(false);
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
    }
}
