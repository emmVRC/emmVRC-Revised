using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using emmVRC.Objects.ModuleBases;
using emmVRC.Utils;

namespace emmVRC.Menus
{
    [Priority(50)]
    public class FunctionsMenu : MelonLoaderEvents
    {
        public static MenuPage basePage;
        private static Tab mainTab;
        internal static ButtonGroup tweaksGroup;
        internal static ButtonGroup featuresGroup;
        internal static ButtonGroup otherGroup;

        private static bool _initialized = false;

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            if (buildIndex != -1 || _initialized) return;
            basePage = new Utils.MenuPage("emmVRC_MainMenu", "emmVRC", true, true, false, null);
            mainTab = new Utils.Tab(Utils.ButtonAPI.menuTabBase.transform.parent, "emmVRC_MainMenu", "", Functions.Core.Resources.TabIcon);
            tweaksGroup = new Utils.ButtonGroup(basePage.menuContents, "Tweaks");
            featuresGroup = new Utils.ButtonGroup(basePage.menuContents, "Features");
            Utils.SingleButton testBtn6 = new Utils.SingleButton(featuresGroup.gameObject.transform, "Alarm\nClocks", null, "Nothing here", Functions.Core.Resources.AlarmClockIcon);
            otherGroup = new Utils.ButtonGroup(basePage.menuContents, "Other");
            Utils.SingleButton testBtn8 = new Utils.SingleButton(otherGroup.gameObject.transform, "<color=#A98A98>emmVRC Team</color>", null, "Nothing here", Functions.Core.Resources.TeamIcon);
            Utils.SingleButton testBtn9 = new Utils.SingleButton(otherGroup.gameObject.transform, "<color=#555500>Supporters</color>", null, "Nothing here", Functions.Core.Resources.SupporterIcon);
            Utils.SingleButton testBtn10 = new Utils.SingleButton(otherGroup.gameObject.transform, "Changelog", null, "Nothing here", Functions.Core.Resources.ChangelogIcon);
            Utils.ButtonGroup grp2 = new ButtonGroup(basePage.menuContents, "");
            Utils.SingleButton testBtn11 = new Utils.SingleButton(grp2.gameObject.transform, "Instant\nRestart", () =>
            {
                DestructiveActions.ForceRestart();
            }, "Restarts, instantly.", null);
            _initialized = true;
        }
    }
}
