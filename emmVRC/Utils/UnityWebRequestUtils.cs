using MelonLoader;
using System;
using System.Collections;
using UnityEngine.Networking;

namespace emmVRC.Utils
{
    public static class UnityWebRequestUtils
    {
        public static void Get(string url, Action<string> onFinish)
        {
            MelonCoroutines.Start(GetCoroutine(url, onFinish));
        }

        private static IEnumerator GetCoroutine(string url, Action<string> onFinish)
        {
            UnityWebRequest request = UnityWebRequest.Get(url);
            request.Send();
            while (!request.isDone)
                yield return null;

            try
            {
                if (request.isNetworkError || request.isHttpError)
                    onFinish?.Invoke(null);
                onFinish?.Invoke(request.downloadHandler.text);
            }
            finally
            {
                request.Dispose();
            }
        }
    }
}