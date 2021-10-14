using emmVRC.Libraries;
using System;
using System.IO;
using System.Collections.Generic;
using emmVRC.Objects;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine;
using emmVRC.Objects.ModuleBases;

namespace emmVRC.Menus
{
    public class ProgramMenuLegacy : MelonLoaderEvents
    {
        public static PaginatedMenu baseMenu;

        public override void OnUiManagerInit()
        {
            baseMenu = new PaginatedMenu(FunctionsMenuLegacy.baseMenu.menuBase, 203945, 102894, "Programs", "", null);
            baseMenu.menuEntryButton.DestroyMe();
            foreach (Program program in Functions.Other.Programs.GetPrograms())
            {
                baseMenu.pageItems.Add(new PageItem(program.name, () =>
                {
                    Functions.Other.Programs.OpenProgram(program);
                }, program.toolTip));
            }
        }
    }
}
