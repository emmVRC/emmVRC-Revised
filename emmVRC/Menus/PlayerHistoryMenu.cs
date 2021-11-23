using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using emmVRC.Objects;
using emmVRC.Objects.ModuleBases;
using emmVRC.Libraries;
using emmVRC.Network;
using emmVRC.Utils;
using UnityEngine;
using VRC.Core;
using VRC.DataModel;

namespace emmVRC.Menus
{
    [Priority(55)]
    public class PlayerHistoryMenu : MelonLoaderEvents
    {
        public static MenuPage playerHistoryPage;
        private static SingleButton playerHistoryButton;

        private static ButtonGroup optionsGroup;
        private static ToggleButton historyToggle;
        private static ToggleButton logPlayerToggle;

        private static ButtonGroup mainHistoryGroup;

        private static int Timeout = 0;
        private static List<SimpleSingleButton> playerHistoryButtons;
        private static List<ButtonGroup> playerHistoryGroups;

        private static bool _initialized = false;

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            if (buildIndex != -1 || _initialized) return;

            playerHistoryPage = new MenuPage("emmVRC_PlayerHistory", "Player History", false, true);
            playerHistoryButton = new SingleButton(Menus.FunctionsMenu.featuresGroup, "Player\nHistory", () => { OpenMenu(); }, "View all the players that have been in your instance since you joined", Functions.Core.Resources.PlayerHistoryIcon);

            optionsGroup = new ButtonGroup(playerHistoryPage, "Options");
            historyToggle = new ToggleButton(optionsGroup, "Player\nHistory", (bool val) =>
            {
                Configuration.WriteConfigOption("PlayerHistoryEnable", val);
                if (!val)
                {
                    foreach (SimpleSingleButton btn in playerHistoryButtons)
                        GameObject.Destroy(btn.gameObject);
                    foreach (ButtonGroup grp in playerHistoryGroups)
                        grp.Destroy();
                }    
            }, "Enable player history for this menu", "Disable player history for this menu");
            logPlayerToggle = new ToggleButton(optionsGroup, "Log Join\nand Leaves", (bool val) =>
            {
                Configuration.WriteConfigOption("LogPlayerJoin", val);
            }, "Enable logging of joined players to the console", "Disable logging of joined players to the console");

            mainHistoryGroup = new ButtonGroup(playerHistoryPage, "History");

            playerHistoryButtons = new List<SimpleSingleButton>();
            playerHistoryGroups = new List<ButtonGroup>();

            _initialized = true;
        }
        private static void OpenMenu()
        {
            foreach (SimpleSingleButton btn in playerHistoryButtons)
                GameObject.Destroy(btn.gameObject);
            foreach (ButtonGroup grp in playerHistoryGroups)
                grp.Destroy();
            int indexCount = 0;
            foreach (InstancePlayer plr in Functions.PlayerHacks.PlayerHistory.currentPlayers)
            {
                if (indexCount == 4)
                {
                    playerHistoryGroups.Add(new ButtonGroup(playerHistoryPage, ""));
                    indexCount = 0;
                }
                else
                    indexCount++;
                SimpleSingleButton playerButton = new SimpleSingleButton(mainHistoryGroup, plr.Name, () =>
                {
                    if (Timeout == 0 && NetworkClient.networkConfiguration.APICallsAllowed)
                    {
                        //else
                        //    user = VRC.DataModel.Core.DataModelManager.field_Private_Static_DataModelManager_0.field_Private_DataModelCache_0.Method_Public_TYPE_String_TYPE2_Boolean_0<IUser, VRC.Core.APIUser>(plr.UserID, false);
                        APIUser.FetchUser(plr.UserID, new System.Action<APIUser>((VRC.Core.APIUser usr) => { ButtonAPI.GetQuickMenuInstance().OpenUser(usr); }), new System.Action<string>((string str) => {
                            emmVRCLoader.Logger.LogError("API returned an error: " + str);
                        }));
                        Timeout = 5;
                        MelonLoader.MelonCoroutines.Start(WaitForTimeout());
                    }
                }, "Joined " + plr.TimeJoinedStamp);
                playerButton.gameObject.transform.SetAsFirstSibling();
                playerHistoryButtons.Add(playerButton);
            }
            playerHistoryPage.OpenMenu();
        }
        private static IEnumerator WaitForTimeout()
        {
            while (Timeout != 0)
            {
                yield return new WaitForSeconds(1f);
                Timeout--;
            }
        }
    }
}
