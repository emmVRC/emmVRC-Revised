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
            try
            {
                if (Configuration.JSONConfig.LastVersion != Objects.Attributes.Version.ToString(3))
                {
                    Configuration.WriteConfigOption("LastVersion", Objects.Attributes.Version.ToString(3));
                    Managers.emmVRCNotificationsManager.AddNotification(new Objects.Notification("Update Applied", Functions.Core.Resources.alertSprite, "emmVRC has updated to version " + Objects.Attributes.Version.ToString(3) + (Objects.Attributes.Beta ? ("b" + Objects.Attributes.Version.Revision) : "") + "!", true, true, () => { Menus.ChangelogMenu.changelogPage.OpenMenu(); }, "View Changelog", "Opens the changelog for this release of emmVRC", true, null, "Dismiss"));
                }
                /*if (Objects.Attributes.Beta ? (Configuration.JSONConfig.LastVersion != Objects.Attributes.Version.ToString(5)) : (Configuration.JSONConfig.LastVersion.Remove(5) != Objects.Attributes.Version.ToString(3)))
                {
                    Configuration.WriteConfigOption("LastVersion", Objects.Attributes.Version.ToString(4));
                    Managers.emmVRCNotificationsManager.AddNotification(new Objects.Notification("Update Applied", Functions.Core.Resources.alertSprite, "emmVRC has updated to version " + Objects.Attributes.Version.ToString(3) + (Objects.Attributes.Beta ? ("b" + Objects.Attributes.Version.Revision) : "") + "!", true, true, () => { Menus.ChangelogMenu.changelogPage.OpenMenu(); }, "View Changelog", "Opens the changelog for this release of emmVRC", true, null, "Dismiss"));
                    /*Managers.NotificationManager.AddNotification("emmVRC has updated to version " + Objects.Attributes.Version + "!", "View\nChangelog", () =>
                    { Managers.NotificationManager.DismissCurrentNotification(); Menus.ChangelogMenu.baseMenu.OpenMenu(); }, "Dismiss", Managers.NotificationManager.DismissCurrentNotification, Functions.Core.Resources.alertSprite, -1);*/
                //}
            } catch (Exception ex)
            {
                emmVRCLoader.Logger.LogError("Update notice failed.");
                emmVRCLoader.Logger.LogError("System locale: " + UnityEngine.Application.systemLanguage.ToString());
                emmVRCLoader.Logger.LogError("Config semver: " + Configuration.JSONConfig.LastVersion);
                emmVRCLoader.Logger.LogError("Error produced: " + ex.ToString());
            }
        }
    }
}
