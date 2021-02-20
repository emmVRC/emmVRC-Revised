using Il2CppSystem;
using System.Linq;
using Il2CppSystem.Collections;
using Il2CppSystem.Collections.Generic;
using Il2CppSystem.Reflection;
using UnityEngine;
using UnhollowerRuntimeLib;
using UnhollowerRuntimeLib.XrefScans;
using VRC.UI;
using System;

namespace emmVRC.Libraries
{
    public class QuickMenuUtils
    {
        // Internal cache of the BoxCollider Background for the Quick Menu
        private static BoxCollider QuickMenuBackgroundReference;

        // Internal cache of the Single Button Template for the Quick Menu
        private static GameObject SingleButtonReference;

        // Internal cache of the Toggle Button Template for the Quick Menu
        private static GameObject ToggleButtonReference;

        // Internal cache of the Nested Menu Template for the Quick Menu
        private static Transform NestedButtonReference;

        private static Vector3 QuickMenuColliderSizeNormal = Vector3.zero;
        private static Vector3 QuickMenuColliderPositionNormal = Vector3.zero;

        // Fetch the background from the Quick Menu
        public static BoxCollider QuickMenuBackground()
        {
            if (QuickMenuBackgroundReference == null)
                QuickMenuBackgroundReference = GetQuickMenuInstance().GetComponent<BoxCollider>();
            return QuickMenuBackgroundReference;
        }

        public static void ResizeQuickMenuCollider()
        {
            if (QuickMenuColliderPositionNormal == Vector3.zero && QuickMenuColliderSizeNormal == Vector3.zero)
            {
                QuickMenuColliderSizeNormal = QuickMenuBackground().size;
                QuickMenuColliderPositionNormal = QuickMenuBackground().center;
                QuickMenuBackground().size = new Vector3(QuickMenuColliderSizeNormal.x, QuickMenuColliderSizeNormal.y + (QuickMenuColliderSizeNormal.y / 4f), QuickMenuColliderSizeNormal.z);
                QuickMenuBackground().center = new Vector3(QuickMenuColliderPositionNormal.x, QuickMenuColliderPositionNormal.y + (QuickMenuColliderPositionNormal.y / 8f), QuickMenuColliderPositionNormal.z);
            }
            else
            {
                QuickMenuBackground().size = QuickMenuColliderSizeNormal;
                QuickMenuBackground().center = QuickMenuColliderPositionNormal;
                QuickMenuColliderSizeNormal = Vector3.zero;
                QuickMenuColliderPositionNormal = Vector3.zero;
            }
        }

        // Fetch the Single Button Template from the Quick Menu
        public static GameObject SingleButtonTemplate()
        {
            if (SingleButtonReference == null)
                SingleButtonReference = GetQuickMenuInstance().transform.Find("ShortcutMenu/WorldsButton").gameObject;
            return SingleButtonReference;
        }

        // Fetch the Toggle Button Template from the Quick Menu
        public static GameObject ToggleButtonTemplate()
        {
            if (ToggleButtonReference == null)
            {
                ToggleButtonReference = GetQuickMenuInstance().transform.Find("UserInteractMenu/BlockButton").gameObject;
            }
            return ToggleButtonReference;
        }

        // Fetch the Nested Menu Template from the Quick Menu
        public static Transform NestedMenuTemplate()
        {
            if (NestedButtonReference == null)
            {
                NestedButtonReference = GetQuickMenuInstance().transform.Find("CameraMenu");
            }
            return NestedButtonReference;
        }

        // Fetch the Quick Menu instance
        public static QuickMenu GetQuickMenuInstance()
        {
            return QuickMenu.prop_QuickMenu_0;
        }
        // Fetch the VRCUiManager instance
        public static VRCUiManager GetVRCUiMInstance()
        {
            return VRCUiManager.prop_VRCUiManager_0;
        }

        // Cache the FieldInfo for getting the current page. Hope to god this works!
        private static FieldInfo currentPageGetter;

        private static GameObject shortcutMenu;
        private static GameObject userInteractMenu;

        // Show a Quick Menu page via the Page Name. Hope to god this works!
        public static void ShowQuickmenuPage(string pagename)
        {
            QuickMenu quickmenu = GetQuickMenuInstance();
            Transform pageTransform = quickmenu?.transform.Find(pagename);
            if (pageTransform == null)
            {
                emmVRCLoader.Logger.LogError("pageTransform is null!");
            }
            if (shortcutMenu == null)
                shortcutMenu = QuickMenu.prop_QuickMenu_0.transform.Find("ShortcutMenu")?.gameObject;

            if (userInteractMenu == null)
                userInteractMenu = QuickMenu.prop_QuickMenu_0.transform.Find("UserInteractMenu")?.gameObject;

            if (currentPageGetter == null)
            {
                GameObject shortcutMenu = quickmenu.transform.Find("ShortcutMenu").gameObject;
                if (!shortcutMenu.activeInHierarchy)
                    shortcutMenu = quickmenu.transform.Find("UserInteractMenu").gameObject;


                FieldInfo[] fis = Il2CppType.Of<QuickMenu>().GetFields(BindingFlags.NonPublic | BindingFlags.Instance).Where((fi) => fi.FieldType == Il2CppType.Of<GameObject>()).ToArray();
                //emmVRCLoader.Logger.Log("[QuickMenuUtils] GameObject Fields in QuickMenu:");
                int count = 0;
                foreach (FieldInfo fi in fis)
                {
                    GameObject value = fi.GetValue(quickmenu)?.TryCast<GameObject>();
                    if (value == shortcutMenu && ++count == 2)
                    {
                        //emmVRCLoader.Logger.Log("[QuickMenuUtils] currentPage field: " + fi.Name);
                        currentPageGetter = fi;
                        break;
                    }
                }
                if (currentPageGetter == null)
                {
                    emmVRCLoader.Logger.LogError("Unable to find field \"currentPage\" in QuickMenu");
                    return;
                }
            }

            currentPageGetter.GetValue(quickmenu)?.Cast<GameObject>().SetActive(false);

            GameObject infoBar = GetQuickMenuInstance().transform.Find("QuickMenu_NewElements/_InfoBar").gameObject;
            infoBar.SetActive(pagename == "ShortcutMenu");

            GameObject tabBar = GetQuickMenuInstance().transform.Find("QuickModeTabs").gameObject;

            QuickMenuContextualDisplay quickmenuContextualDisplay = GetQuickMenuInstance().field_Private_QuickMenuContextualDisplay_0;
            quickmenuContextualDisplay.Method_Public_Void_EnumNPublicSealedvaUnNoToUs7vUsNoUnique_0(QuickMenuContextualDisplay.EnumNPublicSealedvaUnNoToUs7vUsNoUnique.NoSelection);
            //quickmenuContextualDisplay.Method_Public_Nested0_0(QuickMenuContextualDisplay.Nested0.NoSelection);

            pageTransform.gameObject.SetActive(true);

            currentPageGetter.SetValue(quickmenu, pageTransform.gameObject);
            if (pagename == "ShortcutMenu")
            {
                SetIndex(0);
                tabBar.SetActive(true);
            }
            else if (pagename == "UserInteractMenu")
            {
                SetIndex(3);
            }
            else
            {
                SetIndex(-1);
                shortcutMenu?.SetActive(false);
                userInteractMenu?.SetActive(false);
                tabBar.SetActive(false);
            }
        }

        // Set the current Quick Menu index
        public static void SetIndex(int index)
        {
            GetQuickMenuInstance().field_Private_Int32_0 = index;
        }
    }
}
