using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using emmVRC.Objects.ModuleBases;

namespace emmVRC.Functions.Other
{
    public class UpdateNotice : MelonLoaderEvents
    {
        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            if (buildIndex != -1) return;
            if (Objects.Attributes.Beta ? (Configuration.JSONConfig.LastVersion != Objects.Attributes.Version.ToString(4)) : (Configuration.JSONConfig.LastVersion.Remove(5) != Objects.Attributes.Version.ToString(3)))
            {
                Configuration.WriteConfigOption("LastVersion", Objects.Attributes.Version.ToString(4));
                emmVRCLoader.Logger.LogDebug(Configuration.JSONConfig.LastVersion.Remove(5));
                Managers.emmVRCNotificationsManager.AddNotification(new Objects.Notification("Update Applied", Functions.Core.Resources.alertSprite, "emmVRC has updated to version " + Objects.Attributes.Version.ToString(3) + (Objects.Attributes.Beta ? ("b"+Objects.Attributes.Version.Revision) : "") + "!", true, true, () => { Menus.ChangelogMenu.changelogPage.OpenMenu(); }, "View Changelog", "Opens the changelog for this release of emmVRC", true, null, "Dismiss"));
                /*Managers.NotificationManager.AddNotification("emmVRC has updated to version " + Objects.Attributes.Version + "!", "View\nChangelog", () =>
                { Managers.NotificationManager.DismissCurrentNotification(); Menus.ChangelogMenu.baseMenu.OpenMenu(); }, "Dismiss", Managers.NotificationManager.DismissCurrentNotification, Functions.Core.Resources.alertSprite, -1);*/
            }
        }
    }
}
