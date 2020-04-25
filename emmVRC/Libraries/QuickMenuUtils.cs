using Il2CppSystem;
using System.Linq;
using Il2CppSystem.Collections;
using Il2CppSystem.Collections.Generic;
using Il2CppSystem.Reflection;
using UnityEngine;

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

        // Internal cache of the QuickMenu
        private static QuickMenu quickmenuInstance;

        // Internal cache of the VRCUiManager
        private static VRCUiManager vrcuimInstance;



        // Fetch the background from the Quick Menu
        public static BoxCollider QuickMenuBackground()
        {
            if (QuickMenuBackgroundReference == null)
                QuickMenuBackgroundReference = GetQuickMenuInstance().GetComponent<BoxCollider>();
            return QuickMenuBackgroundReference;
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
            if (quickmenuInstance == null)
            {
                quickmenuInstance = QuickMenu.prop_QuickMenu_0;
            }
            return quickmenuInstance;
        }
        // Fetch the VRCUiManager instance
        public static VRCUiManager GetVRCUiMInstance()
        {
            if (vrcuimInstance == null)
            {
                vrcuimInstance = VRCUiManager.field_VRCUiManager_0;
            }
            return vrcuimInstance;
        }

        // Cache the FieldInfo for getting the current page. Hope to god this works!
        private static FieldInfo currentPageGetter;

        // Show a Quick Menu page via the Page Name. Hope to god this works!
        public static void ShowQuickmenuPage(string pagename)
        {
            QuickMenu quickmenu = GetQuickMenuInstance();
            Transform pageTransform = quickmenu?.transform.Find(pagename);
            if (pageTransform == null)
            {
                Console.WriteLine("[QuickMenuUtils] pageTransform is null !");
            }

            if (currentPageGetter == null)
            {
                GameObject shortcutMenu = quickmenu.transform.Find("ShortcutMenu").gameObject;
                if (!shortcutMenu.activeInHierarchy)
                    shortcutMenu = quickmenu.transform.Find("UserInteractMenu").gameObject;

                
                FieldInfo[] fis = QuickMenu.Il2CppType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance).Where((fi) => fi.FieldType == GameObject.Il2CppType).ToArray();
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
                    Console.WriteLine("[QuickMenuUtils] Unable to find field currentPage in QuickMenu");
                    return;
                }
            }

            currentPageGetter.GetValue(quickmenu)?.Cast<GameObject>().SetActive(false);

            GameObject infoBar = GetQuickMenuInstance().transform.Find("QuickMenu_NewElements/_InfoBar").gameObject;
            infoBar.SetActive(pagename == "ShortcutMenu");

            QuickMenuContextualDisplay quickmenuContextualDisplay = GetQuickMenuInstance().field_QuickMenuContextualDisplay_0;
            quickmenuContextualDisplay.Method_Public_Nested0_0(QuickMenuContextualDisplay.Nested0.NoSelection);

            pageTransform.gameObject.SetActive(true);

            currentPageGetter.SetValue(quickmenu, pageTransform.gameObject);
            if (pagename == "ShortcutMenu")
            {
                SetIndex(0);
            }
            else if (pagename == "UserInteractMenu")
            {
                SetIndex(3);
            }
            else
            {
                SetIndex(-1);
            }
        }

        // Set the current Quick Menu index
        public static void SetIndex(int index)
        {
            /*if (_CurrentIndex == null)
            {
                _CurrentIndex = typeof(QuickMenu).GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
                                                 .First(x => !x.IsStatic && (x.FieldType.Name == typeof(int).Name));
                if (_CurrentIndex == null)
                {
                    Console.WriteLine("[QuickMenuUtils] Index reflection is null");
                    return;
                }
            }

            _CurrentIndex.SetValue(GetQuickMenuInstance(), index);*/
            GetQuickMenuInstance().field_Int32_0 = index;
        }
    }
}
