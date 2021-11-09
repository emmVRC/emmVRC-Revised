using emmVRC.Libraries;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using emmVRC.Objects.ModuleBases;

namespace emmVRC.Functions.UI
{
    public class UserListRefresh : MelonLoaderEvents
    {
        public static int cooldown = 0;
        public static GameObject refreshButton;
        public override void OnUiManagerInit()
        {
            refreshButton = GameObject.Instantiate(QuickMenuUtils.GetVRCUiMInstance().menuContent().transform.Find("Screens/Social/UserProfileAndStatusSection/Status/EditStatusButton").gameObject, QuickMenuUtils.GetVRCUiMInstance().menuContent().transform.Find("Screens/Social"));
            refreshButton.GetComponent<RectTransform>().anchoredPosition += new Vector2(1260f, 370f);
            refreshButton.GetComponentInChildren<Text>().text = "<color=#FFFFFF>Refresh</color>";
            refreshButton.GetComponent<Button>().onClick = new Button.ButtonClickedEvent();
            refreshButton.GetComponent<Button>().onClick.AddListener(new System.Action(() => {
                RefreshMenu();
            }));

        }
        public static void RefreshMenu()
        {
            if (cooldown <= 0)
            {
                // This changed with obfuscation, and since we're not updating the deob table (yet), this is hardcoded. Should be the same on Oculus anyway
                // This is UiUserList, btw
                foreach (UiUserList list in UnityEngine.Resources.FindObjectsOfTypeAll<UiUserList>())
                {
                    try
                    {
                        list.Method_Public_Void_0();
                        list.Method_Public_Void_1();
                        //list.Method_Public_Void_2();
                    }
                    catch (Exception ex)
                    {
                        emmVRCLoader.Logger.LogError(ex.ToString());
                    }
                    cooldown = 10;
                    MelonLoader.MelonCoroutines.Start(CooldownTimer());
                }
            }
            else
            {
                VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowAlert("emmVRC", "You must wait " + cooldown + " seconds before refreshing again.", 10f);
                //VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowStandardPopup("emmVRC", "You must wait " + cooldown + " seconds before refreshing again.");
            }
        }
        public static IEnumerator CooldownTimer()
        {
            yield return new WaitForSecondsRealtime(cooldown);
            cooldown = 0;

        }
    }
}
