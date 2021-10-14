using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using emmVRC.Utils;
using emmVRC.Libraries;
using emmVRC.Objects;
using emmVRC.Objects.ModuleBases;

namespace emmVRC.Menus
{
    [Priority(55)]
    public class ProgramsMenu : MelonLoaderEvents
    {
        public static MenuPage programsPage;
        private static SingleButton programsButton;

        private static ButtonGroup mainProgramsGroup;

        private static List<SimpleSingleButton> programsButtons;
        private static List<ButtonGroup> programsGroups;

        private static bool _initialized = false;
        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            if (buildIndex != -1 || _initialized) return;

            programsPage = new MenuPage("emmVRC_Programs", "Programs", false, true);
            programsButton = new SingleButton(FunctionsMenu.featuresGroup, "Programs", () =>
            {
                OpenMenu();
            }, "Lets you launch external programs from within VRChat", Functions.Core.Resources.ProgramsIcon);

            mainProgramsGroup = new ButtonGroup(programsPage, "Programs");

            programsButtons = new List<SimpleSingleButton>();
            programsGroups = new List<ButtonGroup>();

            _initialized = true;

        }
        public static void OpenMenu()
        {
            foreach (SimpleSingleButton btn in programsButtons)
                GameObject.Destroy(btn.gameObject);
            foreach (ButtonGroup grp in programsGroups)
                grp.Destroy();
            programsButtons.Clear();
            programsGroups.Clear();
            int indexCount = 0;
            foreach (Program prgm in Functions.Other.Programs.GetPrograms())
            {
                if (indexCount == 4)
                {
                    programsGroups.Add(new ButtonGroup(programsPage, ""));
                    indexCount = 0;
                }
                else
                    indexCount++;
                SimpleSingleButton instanceButton = new SimpleSingleButton(mainProgramsGroup, prgm.name, () =>
                {
                    Functions.Other.Programs.OpenProgram(prgm);
                }, prgm.toolTip);
                programsButtons.Add(instanceButton);
            }
            programsPage.OpenMenu();
        }
    }
}
