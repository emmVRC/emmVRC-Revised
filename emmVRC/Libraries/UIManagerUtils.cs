using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnhollowerRuntimeLib.XrefScans;
using VRC.UI;

namespace emmVRC.Libraries
{
    public static class UIManagerUtils
    {
        public static void QueueHUDMessage(this VRCUiManager manager, string message)
        {
            manager.field_Private_List_1_String_0.Add(message);
        }

        #region Show Screen
        public delegate VRCUiPage ShowScreenAction(VRCUiPage page);

        private static ShowScreenAction ourShowScreenAction;

        public static ShowScreenAction ShowScreenActionAction
        {
            get
            {
                if (ourShowScreenAction != null) return ourShowScreenAction;

                var targetMethod = typeof(VRCUiManager).GetMethods(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public)
                    .Single(it => it.ReturnType == typeof(VRCUiPage) && it.GetParameters().Length == 1 && it.GetParameters()[0].ParameterType == typeof(VRCUiPage) && XrefScanner.XrefScan(it).Any(jt => jt.Type == XrefType.Global && jt.ReadAsObject()?.ToString() == "Screen Not Found - "));

                ourShowScreenAction = (ShowScreenAction)Delegate.CreateDelegate(typeof(ShowScreenAction), VRCUiManager.prop_VRCUiManager_0, targetMethod);

                return ourShowScreenAction;
            }
        }
        public static VRCUiPage ShowScreen(this VRCUiManager manager, VRCUiPage page)
        {
            return ShowScreenActionAction.Invoke(page);
        }
        #endregion

        /*public static void ShowScreen(this VRCUiManager manager, VRCUiPage page)
        {
            manager.Method_Public_VRCUiPage_VRCUiPage_1(page);
        }*/
        public static void ShowScreen(this VRCUiManager manager, string pageName, bool otherThing)
        {
            VRCUiPage page = GetPage.Invoke(pageName);
            if (page != null && otherThing)
            {
                manager.ShowScreen(page);
            }
        }
        #region Get Page
        public delegate VRCUiPage GetPageAction(string page);

        private static GetPageAction ourGetPageAction;

        public static GetPageAction GetPage
        {
            get
            {
                if (ourGetPageAction != null) return ourGetPageAction;

                var targetMethod = typeof(VRCUiPopupManager).GetMethods(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public)
                    .Single(it => it.GetParameters().Length == 1 && XrefScanner.XrefScan(it).Any(jt => jt.Type == XrefType.Global && jt.ReadAsObject()?.ToString() == "Screen Not Found - "));

                ourGetPageAction = (GetPageAction)Delegate.CreateDelegate(typeof(GetPageAction), VRCUiPopupManager.prop_VRCUiPopupManager_0, targetMethod);

                return ourGetPageAction;
            }
        }
        #endregion
    }
}
