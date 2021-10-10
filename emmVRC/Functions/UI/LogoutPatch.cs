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
            QuickMenuUtils.GetVRCUiMInstance().menuContent().transform.Find("Screens/Settings/Footer/Logout").GetComponent<Button>().onClick.AddListener(new System.Action(() =>
            {
                NetworkClient.Logout();
            }));
        }
    }
}
