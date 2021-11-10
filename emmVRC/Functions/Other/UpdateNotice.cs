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
            if (Configuration.JSONConfig.LastVersion != Objects.Attributes.Version)
            {
                Configuration.WriteConfigOption("LastVersion", Objects.Attributes.Version);
                Managers.emmVRCNotificationsManager.AddNotification(new Objects.Notification("Update Applied", Functions.Core.Resources.alertSprite, "emmVRC has updated to version " + Objects.Attributes.Version + "!", true, false, null, "", "", true, null, "Dismiss"));
                /*Managers.NotificationManager.AddNotification("emmVRC has updated to version " + Objects.Attributes.Version + "!", "View\nChangelog", () =>
                { Managers.NotificationManager.DismissCurrentNotification(); Menus.ChangelogMenu.baseMenu.OpenMenu(); }, "Dismiss", Managers.NotificationManager.DismissCurrentNotification, Functions.Core.Resources.alertSprite, -1);*/
            }
        }
    }
}
