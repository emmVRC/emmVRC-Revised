using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VRC.Social;
using VRC;
using VRC.Core;
using VRC.UI;

namespace emmVRC
{
    internal class socialMenuFunctions
    {
        private static GameObject SocialFunctionsButton = new GameObject();
        private static GameObject UserSendMessage = new GameObject();
        private static GameObject UserNotes = new GameObject();
        private static GameObject TeleportButton = new GameObject();
        private static GameObject WorldNotesButton = new GameObject();
        private static GameObject ToggleBlockButton = new GameObject();

        internal static void Start()
        {
            SocialFunctionsButton = GameObject.Instantiate(GameObject.Find("MenuContent/Screens/UserInfo/User Panel/Playlists/PlaylistsButton"), GameObject.Find("MenuContent/Screens/UserInfo/User Panel/Playlists").transform);

        }
    }
}
