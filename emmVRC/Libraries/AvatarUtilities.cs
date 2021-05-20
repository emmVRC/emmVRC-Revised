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
                    emmVRCLoader.Logger.LogDebug("Found avatar " + container.Model.Cast<ApiAvatar>().name);
                    MelonLoader.MelonCoroutines.Start(Delay());
                }), new Action<ApiContainer>((ApiContainer container) =>
                {
                    errored = true;
                    emmVRCLoader.Logger.LogDebug("Current avatar is not available.");
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