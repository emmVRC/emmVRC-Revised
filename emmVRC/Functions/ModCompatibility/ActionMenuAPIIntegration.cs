using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ActionMenuApi.Api;

namespace emmVRC.Functions.ModCompatibility
{
    public class ActionMenuAPIIntegration
    {
        private static bool _initialized;
        public static void Initialize()
        {
            if (_initialized) return;
            if (Configuration.JSONConfig.ActionMenuAPIIntegration)
                AMUtils.AddToModsFolder("emmVRC", () => { Menus.ActionMenuFunctions.functionsMenu.OpenMenu(Menus.ActionMenuFunctions.functionsMenu.previousPage); }, Functions.Core.Resources.onlineSprite.texture);
            _initialized = true;
        }
    }
}
