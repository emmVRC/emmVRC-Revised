using System;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using emmVRC.Network;
using System.Net.Http;
using System.Net.Http.Headers;
using UnityEngine;
using System.Collections;
using emmVRC.Network.Objects;
using emmVRC.Objects;

namespace emmVRC.Network
{
    public static class NetworkClient
    {
        //TODO add caching
        //TODO add sockets
        private static string BaseAddress = "http://crreamhub.com";
        private static int Port = 3000;
        public static string baseURL { get { return BaseAddress + ":" + Port; } }
        private static string _authToken;
        public static string authToken {
            get { return _authToken; }
            set
            {
                _authToken = value;
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _authToken);
            }
        }
        public static HttpClient httpClient { get; set; }
        //private static void voidFunc() { }
        //public static T InitializeClient<T>(Func<T> callback = voidFunc())
        public static void InitializeClient()
        {
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("emmVRC/1.0 (Client; emmVRCClient/"+ Attributes.Version+")");
            login();
            //return callback();
        }

        public static T DestroyClient<T>(Func<T> callback = null)
        {
            httpClient = null;
            authToken = null;
            return callback();
        }

        public static void login()
        {
            MelonLoader.MelonCoroutines.Start(sendLogin());
        }

        private static IEnumerator sendLogin()
        {
            while (RoomManager.field_Internal_Static_ApiWorld_0 == null)
                yield return new WaitForEndOfFrame();
            NetworkClient.authToken = HTTPResponse.Serialize(HTTPRequest.post_sync(NetworkClient.baseURL + "/api/authentication/login", new Dictionary<string, string>() { ["username"] = VRC.Core.APIUser.CurrentUser.id, ["name"] = VRC.Core.APIUser.CurrentUser.displayName }))["token"];
        }


    }
}
