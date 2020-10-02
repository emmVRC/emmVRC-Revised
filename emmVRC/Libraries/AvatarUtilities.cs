using emmVRC.Hacks;
using emmVRC.Network;
using Il2CppSystem.Diagnostics;
using Il2CppSystem.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VRC;
using VRC.Core;

namespace emmVRC.Libraries
{
    public class AviFavAvatar
    {
        public string ThumbnailImageUrl = "";
        public string AvatarID = "";
        public string Name = "";
    }
    public class AvatarUtilities
    {
        public static List<ApiAvatar> processingList;
        public static bool requestFinished = true;
        public static bool errored = false;
        public static IEnumerator FavoriteAvatars(List<ApiAvatar> avatars, bool errored)
        {
            foreach (ApiAvatar avtr in avatars)
            {
                if (avtr.releaseStatus == "public" || avtr.authorId == APIUser.CurrentUser.id)
                    yield return CustomAvatarFavorites.FavoriteAvatar(avtr);
                else
                    errored = true;
                yield return new WaitForSeconds(1f);
            }
            CustomAvatarFavorites.MigrateButton.SetActive(false);
            File.Move(Path.Combine(System.Environment.CurrentDirectory, "404Mods/AviFavorites/avatars.json"), Path.Combine(System.Environment.CurrentDirectory, "404Mods/AviFavorites/avatars.old.json"));
            Managers.NotificationManager.AddNotification("Your avatars have been migrated" + (errored ? ", but one or more of your avatars are no longer available. They may have been privated or deleted." : " successfully."), "Dismiss", Managers.NotificationManager.DismissCurrentNotification, "", null, Resources.alertSprite);
        }
        public static IEnumerator fetchAvatars(List<string> avatars, Action<List<ApiAvatar>, bool> callBack)
        {
            processingList = new List<ApiAvatar>();
            requestFinished = true;
            errored = false;
            foreach (string avatarId in avatars)
            {
                while (!requestFinished)
                    yield return new WaitForEndOfFrame();
                requestFinished = false;
                API.Fetch<ApiAvatar>(avatarId, new Action<ApiContainer>((ApiContainer container) =>
                {
                    processingList.Add(container.Model.Cast<ApiAvatar>());
                    MelonLoader.MelonCoroutines.Start(Delay());
                }), new Action<ApiContainer>((ApiContainer container) =>
                {
                    errored = true;
                    MelonLoader.MelonCoroutines.Start(Delay());
                }));
            }
            while (!requestFinished)
                yield return new WaitForEndOfFrame();
            callBack.Invoke(processingList, errored);
        }
        public static IEnumerator Delay()
        {
            yield return new WaitForSeconds(2.5f);
            requestFinished = true;
        }
    }
}