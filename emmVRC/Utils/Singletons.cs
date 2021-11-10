using UnityEngine;
using VRC;
using VRC.UI;

#pragma warning disable IDE1006 // Naming Styles

namespace emmVRC.Utils
{
    public class Singletons
    {
        public static VRCUiManager vrcUiManager => VRCUiManager.field_Private_Static_VRCUiManager_0;
        public static VRCUiPopupManager vrcUiPopupManger => VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0;
        public static PageWorldInfo pageWorldInfo
        {
            get
            {
                if (_pageWorldInfo == null)
                    _pageWorldInfo = GameObject.Find("UserInterface/MenuContent/Screens/WorldInfo").GetComponent<PageWorldInfo>();
                return _pageWorldInfo;
            }
        }
        private static PageWorldInfo _pageWorldInfo;
        public static PlayerManager playerManager => PlayerManager.field_Private_Static_PlayerManager_0;
    }
}