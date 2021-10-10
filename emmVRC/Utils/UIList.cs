using System;
using Il2CppSystem.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using VRC.Core;

namespace emmVRC.Utils
{
    public static class UIList
    {
        private static MethodInfo renderElementMethod;
        internal static void RenderElement(this UiVRCList uivrclist, List<ApiAvatar> AvatarList)
        {
            if (!uivrclist.gameObject.activeInHierarchy || !uivrclist.isActiveAndEnabled || uivrclist.isOffScreen || !uivrclist.enabled) return;
            if (renderElementMethod == null)
            {
                renderElementMethod = typeof(UiVRCList).GetMethods().FirstOrDefault(a => a.Name.Contains("Method_Protected_Void_List_1_T_Int32_Boolean")).MakeGenericMethod(typeof(ApiAvatar));
            }
            renderElementMethod.Invoke(uivrclist, new object[] { AvatarList, 0, true, null });
        }
        internal static void RenderElement(this UiVRCList uivrclist, List<string> idList)
        {
            if (!uivrclist.gameObject.activeInHierarchy || !uivrclist.isActiveAndEnabled || uivrclist.isOffScreen || !uivrclist.enabled) return;
            if (renderElementMethod == null)
            {
                renderElementMethod = typeof(UiVRCList).GetMethods().FirstOrDefault(a => a.Name.Contains("Method_Protected_Void_List_1_T_Int32_Boolean")).MakeGenericMethod(typeof(ApiAvatar));
            }
            renderElementMethod.Invoke(uivrclist, new object[] { idList, 0, true, null });
        }
    }
}
