using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnhollowerRuntimeLib.XrefScans;

namespace emmVRC.Libraries
{
    public static class UIManagerUtils
    {
        public static void QueueHUDMessage(this VRCUiManager manager, string message)
        {
            manager.field_Private_List_1_String_0.Add(message);
        }
        public static void ShowScreen(this VRCUiManager manager, VRCUiPage page)
        {
            manager.Method_Public_VRCUiPage_VRCUiPage_0(page);
        }
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
