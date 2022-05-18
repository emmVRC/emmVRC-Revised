using System;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using UnhollowerRuntimeLib;
using UnhollowerRuntimeLib.XrefScans;
using VRC;
using VRC.Core;
using VRC.DataModel;
using VRC.DataModel.Core;

namespace emmVRC.Utils
{
    public static class Extensions
    {
        public static GameObject FindObject(this GameObject parent, string name)
        {
            Transform[] trs = parent.GetComponentsInChildren<Transform>(true);
            foreach (Transform t in trs)
            {
                if (t.name == name)
                {
                    return t.gameObject;
                }
            }
            return null;
        }
        public static string GetPath(this GameObject gameObject)
        {
            string path = "/" + gameObject.name;
            while (gameObject.transform.parent != null)
            {
                gameObject = gameObject.transform.parent.gameObject;
                path = "/" + gameObject.name + path;
            }
            return path;
        }

        public static void DestroyChildren(this Transform transform, Func<Transform, bool> exclude)
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
                if (exclude == null || exclude(transform.GetChild(i)))
                    GameObject.DestroyImmediate(transform.GetChild(i).gameObject);
        }

        public static void DestroyChildren(this Transform transform) => DestroyChildren(transform, null);

        public static Vector3 SetX(this Vector3 vector, float x)
        {
            return new Vector3(x, vector.y, vector.z);
        }
        public static Vector3 SetY(this Vector3 vector, float y)
        {
            return new Vector3(vector.x, y, vector.z);
        }
        public static Vector3 SetZ(this Vector3 vector, float z)
        {
            return new Vector3(vector.x, vector.y, z);
        }

        public static float RoundAmount(this float i, float nearestFactor)
        {
            return (float)Math.Round(i / nearestFactor) * nearestFactor;
        }

        public static Vector3 RoundAmount(this Vector3 i, float nearestFactor)
        {
            return new Vector3(i.x.RoundAmount(nearestFactor), i.y.RoundAmount(nearestFactor), i.z.RoundAmount(nearestFactor));
        }

        public static Vector2 RoundAmount(this Vector2 i, float nearestFactor)
        {
            return new Vector2(i.x.RoundAmount(nearestFactor), i.y.RoundAmount(nearestFactor));
        }

        public static Sprite ToSprite(this Texture2D tex)
        {
            Rect rect = new Rect(0, 0, tex.width, tex.height);
            Vector2 pivot = new Vector2(0.5f, 0.5f);
            Vector4 border = Vector4.zero;
            Sprite sprite = Sprite.CreateSprite_Injected(tex, ref rect, ref pivot, 50, 0, SpriteMeshType.FullRect, ref border, false);
            sprite.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            return sprite;
        }

        // https://stackoverflow.com/questions/8809354/replace-first-occurrence-of-pattern-in-a-string
        public static string ReplaceFirst(this string text, string search, string replace)
        {
            int pos = text.IndexOf(search);
            if (pos < 0)
            {
                return text;
            }
            return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
        }

        public static ColorBlock SetColor(this ColorBlock block, Color color)
        {
            return new ColorBlock()
            {
                colorMultiplier = block.colorMultiplier,
                disabledColor = Color.grey,
                highlightedColor = color,
                normalColor = color / 1.5f,
                pressedColor = Color.white,
                selectedColor = color / 1.5f,
            };
        }

        public static void DelegateSafeInvoke(this Delegate @delegate, params object[] args)
        {
            Delegate[] delegates = @delegate.GetInvocationList();
            for (int i = 0; i < delegates.Length; i++)
            {
                try
                {
                    delegates[i].DynamicInvoke(args);
                }
                catch (Exception ex)
                {
                    emmVRCLoader.Logger.LogError("Error while executing delegate:\n" + ex.ToString());
                }
            }
        }

        public static string ToEasyString(this TimeSpan timeSpan)
        {
            if (Mathf.FloorToInt(timeSpan.Ticks / TimeSpan.TicksPerDay) > 0)
                return $"{timeSpan:%d} days";
            else if (Mathf.FloorToInt(timeSpan.Ticks / TimeSpan.TicksPerHour) > 0)
                return $"{timeSpan:%h} hours";
            else if (Mathf.FloorToInt(timeSpan.Ticks / TimeSpan.TicksPerMinute) > 0)
                return $"{timeSpan:%m} minutes";
            else
                return $"{timeSpan:%s} seconds";
        }
        #region QuickMenu ShowAlert
        private static MethodInfo ourShowAlertMethod;

        public static void ShowAlert(this VRC.UI.Elements.QuickMenu qm, string message)
        {

            if (ourShowAlertMethod == null)
                foreach (MethodInfo inf in typeof(VRC.UI.Elements.Controls.ModalAlert).GetMethods())
                {
                    emmVRCLoader.Logger.LogDebug(inf.Name);
                    if (inf.Name.Contains("Method_Private_Void_") && !inf.Name.Contains("PDM") && inf.GetParameters().Length == 0)
                    {
                        qm.field_Private_ModalAlert_0.field_Private_String_0 = message;
                        inf.Invoke(qm.field_Private_ModalAlert_0, null);
                        if (qm.transform.Find("Container/Window/QMParent/Modal_Alert/Background_Alert").gameObject.activeSelf)
                        {
                            ourShowAlertMethod = inf;
                            break;
                        }
                    }
                }
            else
            {
                qm.field_Private_ModalAlert_0.field_Private_String_0 = message;
                ourShowAlertMethod.Invoke(qm.field_Private_ModalAlert_0, null);
            }
            //ShowAlertMethod.Invoke(qm, new object[] { message });
        }
        #endregion
        #region QuickMenu ShowOKDialog
        private static MethodInfo ourShowOKDialogMethod;
        public static MethodInfo ShowOKDialogMethod
        {
            get
            {
                if (ourShowOKDialogMethod != null) return ourShowOKDialogMethod;
                var targetMethod = typeof(VRC.UI.Elements.QuickMenu).GetMethods()
                    .First(it => it != null && it.GetParameters().Length == 3 && it.Name.Contains("_PDM") && it.GetParameters().First().ParameterType == typeof(string) && it.GetParameters().Last().ParameterType == typeof(Il2CppSystem.Action) && XrefScanner.XrefScan(it).Any(jt => jt.Type == XrefType.Global && jt.ReadAsObject()?.ToString() == "ConfirmDialog"));
                ourShowOKDialogMethod = targetMethod;
                return ourShowOKDialogMethod;
            }
        }

        public static void ShowOKDialog(this VRC.UI.Elements.QuickMenu qm, string title, string message, Action okButton = null)
        {
            ShowOKDialogMethod.Invoke(qm, new object[] { title, message, DelegateSupport.ConvertDelegate<Il2CppSystem.Action>(okButton) });
        }
        #endregion
        #region QuickMenu ShowConfirmDialog
        private static MethodInfo ourShowConfirmDialogMethod;
        public static MethodInfo ShowConfirmDialogMethod
        {
            get
            {
                if (ourShowConfirmDialogMethod != null) return ourShowConfirmDialogMethod;
                var targetMethod = typeof(VRC.UI.Elements.QuickMenu).GetMethods()
                    .First(it => it != null && it.GetParameters().Length == 4 && it.Name.Contains("_PDM") && it.GetParameters()[0].ParameterType == typeof(string) && it.GetParameters()[1].ParameterType == typeof(string) && it.GetParameters()[2].ParameterType == typeof(Il2CppSystem.Action) && it.GetParameters()[3].ParameterType == typeof(Il2CppSystem.Action) && XrefScanner.UsedBy(it).ToList().Count > 30);
                ourShowConfirmDialogMethod = targetMethod;
                return ourShowConfirmDialogMethod;
            }
        }

        public static void ShowConfirmDialog(this VRC.UI.Elements.QuickMenu qm, string title, string message, Action yesButton = null, Action noButton = null)
        {
            ShowConfirmDialogMethod.Invoke(qm, new object[] { title, message, DelegateSupport.ConvertDelegate<Il2CppSystem.Action>(yesButton), DelegateSupport.ConvertDelegate<Il2CppSystem.Action>(noButton) });
        }

        #endregion
        #region QuickMenu ShowCustomDialog
        private static MethodInfo ourShowCustomDialogMethod;
        public static MethodInfo ShowCustomDialogMethod
        {
            get
            {
                if (ourShowCustomDialogMethod != null) return ourShowCustomDialogMethod;
                var targetMethod = typeof(VRC.UI.Elements.QuickMenu).GetMethods()
                    .First(it => it != null && it.GetParameters().Length == 8 && it.Name.Contains("Method_Public_Void_String_String_String_String_String_Action_Action_Action_PDM_") && XrefScanner.XrefScan(it).Any(jt => jt.Type == XrefType.Global && jt.ReadAsObject()?.ToString() == "ConfirmDialog"));
                ourShowCustomDialogMethod = targetMethod;
                return ourShowCustomDialogMethod;
            }
        }

        public static void ShowCustomDialog(this VRC.UI.Elements.QuickMenu qm, string title, string message, string button1Text, string button2Text, string button3Text,  Action button1Action = null, Action button2Action = null, Action button3Action = null)
        {
            ShowCustomDialogMethod.Invoke(qm, new object[] { title, message, button1Text, button2Text, button3Text, DelegateSupport.ConvertDelegate<Il2CppSystem.Action>(button1Action), DelegateSupport.ConvertDelegate<Il2CppSystem.Action>(button2Action), DelegateSupport.ConvertDelegate<Il2CppSystem.Action>(button3Action) });
        }

        #endregion
        //public static void ShowCustomDialog(this VRC.UI.Elements.QuickMenu qm, string title, string message, string button1Text, string button2Text, string button3Text, Action button1Action = null, Action button2Action = null, Action button3Action = null) => qm.Method_Public_Void_String_String_String_String_String_Action_Action_Action_PDM_1(title, message, button1Text, button2Text, button3Text, button1Action, button2Action, button3Action);
        #region QuickMenu AskConfirmOpenURL
        private static MethodInfo ourAskConfirmOpenURLMethod;
        public static MethodInfo AskConfirmOpenURLMethod
        {
            get
            {
                if (ourAskConfirmOpenURLMethod != null) return ourAskConfirmOpenURLMethod;
                var targetMethod = typeof(VRC.UI.Elements.QuickMenu).GetMethods()
                    .First(it => it != null && it.GetParameters().Length == 1 && it.Name.Contains("_Virtual_Final_New") && it.GetParameters().First().ParameterType == typeof(string) && XrefScanner.XrefScan(it).Any(jt => jt.Type == XrefType.Global && jt.ReadAsObject() != null && jt.ReadAsObject().ToString().Contains("This link will open in your web browser.")));
                ourAskConfirmOpenURLMethod = targetMethod;
                return ourAskConfirmOpenURLMethod;
            }
        }

        public static void AskConfirmOpenURL(this VRC.UI.Elements.QuickMenu qm, string url)
        {
            AskConfirmOpenURLMethod.Invoke(qm, new object[] { url });
        }
        #endregion
        #region APIUser ToIUser
        // Thank you Loukylor for this!


        private static MethodInfo _apiUserToIUser;
        public static MethodInfo ToIUserMethod
        {
            get
            {
                if (_apiUserToIUser == null)
                {
                    Type iUserParent = typeof(VRCPlayer).Assembly.GetTypes()
                .First(type => type.GetMethods()
                    .FirstOrDefault(method => method.Name.StartsWith("Method_Private_Void_Action_1_ApiWorldInstance_Action_1_String_")) != null && type.GetMethods()
                    .FirstOrDefault(method => method.Name.StartsWith("Method_Public_Virtual_Final_New_Observable_1_List_1_String_")) == null);
                    _apiUserToIUser = typeof(DataModelCache).GetMethod("Method_Public_TYPE_String_TYPE2_Boolean_0");
                    _apiUserToIUser = _apiUserToIUser.MakeGenericMethod(iUserParent, typeof(APIUser));
                }
                return _apiUserToIUser;
            }
        }

        public static IUser ToIUser(this APIUser value)
        {
            return ((Il2CppSystem.Object)_apiUserToIUser.Invoke(DataModelManager.field_Private_Static_DataModelManager_0.field_Private_DataModelCache_0, new object[3] { value.id, value, false })).Cast<IUser>();
        }
        #endregion
        #region VRCPlayer SpawnEmoji
        private static MethodInfo ourSpawnEmojiMethod;
        public static MethodInfo SpawnEmojiMethod
        {
            get
            {
                if (ourSpawnEmojiMethod != null) return ourSpawnEmojiMethod;
                var targetMethod = typeof(VRCPlayer).GetMethods()
                    .FirstOrDefault(it => it != null && it.GetParameters().Length == 1 && it.GetParameters().First().ParameterType == typeof(int) && XrefScanner.XrefScan(it).Any(jt => jt.Type == XrefType.Global && jt.ReadAsObject() != null && jt.ReadAsObject().ToString().Contains("SpawnEmojiRPC")));
                ourSpawnEmojiMethod = targetMethod;
                return ourSpawnEmojiMethod;
            }
        }

        public static void SpawnEmoji(this VRCPlayer player, int emojiId)
        {
            SpawnEmojiMethod.Invoke(player, new object[] { emojiId });
        }
        #endregion
    }
}