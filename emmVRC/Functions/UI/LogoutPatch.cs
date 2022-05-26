using System;
using System.Linq;
using emmVRC.Libraries;
using emmVRC.Network;
using UnityEngine.UI;
using emmVRC.Objects.ModuleBases;

namespace emmVRC.Functions.UI
{
    public class LogoutPatch : MelonLoaderEvents
    {
        public override void OnUiManagerInit()
        {
            UnityEngine.Resources.FindObjectsOfTypeAll<VRCUiPageSettings>().FirstOrDefault().transform.Find("Footer/Logout")
                .GetComponent<Button>().onClick.AddListener(new System.Action(NetworkClient.DestroySession));
        }
    }
}
