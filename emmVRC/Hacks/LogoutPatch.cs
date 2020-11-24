using emmVRC.Libraries;
using emmVRC.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;

namespace emmVRC.Hacks
{
    public class LogoutPatch
    {
        public static void Initialize()
        {
            QuickMenuUtils.GetVRCUiMInstance().menuContent.transform.Find("Screens/Settings/Footer/Logout").GetComponent<Button>().onClick.AddListener(new System.Action(() => {
                HTTPRequest.get(NetworkClient.baseURL + "/api/authentication/logout");
                NetworkClient.authToken = null;
                
                CustomAvatarFavorites.LoadedAvatars = null;
            }));
        }
    }
}
