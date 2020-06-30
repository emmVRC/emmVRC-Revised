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
using emmVRC.Libraries;

namespace emmVRC.Network
{
    public static class NetworkClient
    {
        //TODO add caching
        //TODO add sockets
        private static string BaseAddress = "http://thetrueyoshifan.com";
        private static int Port = 3000;
        public static string baseURL { get { return BaseAddress + ":" + Port; } }
        private static string LoginKey;
        private static string _authToken;
        private static bool prompted = false;
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

        public static void login(string password = "")
        {
            MelonLoader.MelonCoroutines.Start(sendLogin(password));
        }

        private static IEnumerator sendLogin(string password)
        {
            while (RoomManager.field_Internal_Static_ApiWorld_0 == null)
                yield return new WaitForEndOfFrame();
            sendRequest(password);

            if (!Authentication.Authentication.Exists(VRC.Core.APIUser.CurrentUser.id) && !prompted)
            {
                prompted = true;
                Managers.NotificationManager.AddNotification("Better protect your emmVRC account by setting a pin\n\nAlready have a pin? Login!", "Set\nPassword", () => { Managers.NotificationManager.DismissCurrentNotification(); PromptLogin(); }, "Login", () => { Managers.NotificationManager.DismissCurrentNotification(); PromptLogin(); }, Resources.alertSprite, -1);
            }
        }

        public static void PromptLogin()
        {
            if (Authentication.Authentication.Exists(VRC.Core.APIUser.CurrentUser.id))
                return;
            OpenPasswordPrompt();
        }

        private static void OpenPasswordPrompt()
        {
            InputUtilities.OpenNumberPad("To login, please enter your password", "Login", (string password) => { 
                login(password);
            });
        }

        private static async void sendRequest(string password = "")
        {
            if (password == "")
                LoginKey = Authentication.Authentication.ReadTokenFile(VRC.Core.APIUser.CurrentUser.id);
            else
                LoginKey = password;

            string createFile = "0";
            if (!Authentication.Authentication.Exists(VRC.Core.APIUser.CurrentUser.id))
                createFile = "1";

            try
            {
                TinyJSON.Variant result = HTTPResponse.Serialize(await HTTPRequest.post(NetworkClient.baseURL + "/api/authentication/login", new Dictionary<string, string>() { ["username"] = VRC.Core.APIUser.CurrentUser.id, ["name"] = VRC.Core.APIUser.CurrentUser.displayName, ["password"] = LoginKey, ["loginKey"] = createFile }));
                NetworkClient.authToken = result["token"];
                if (createFile == "1")
                {
                    Authentication.Authentication.CreateTokenFile(VRC.Core.APIUser.CurrentUser.id, result["loginKey"]);
                    LoginKey = result["loginKey"];
                }
            }
            catch(Exception exception)
            {
                Managers.NotificationManager.AddNotification("Oops! We couldn't log you in!\nTry Again", "Login", () => { Managers.NotificationManager.DismissCurrentNotification(); PromptLogin(); }, "Dismiss", () => { Managers.NotificationManager.DismissCurrentNotification(); }, Resources.alertSprite, -1);
                emmVRCLoader.Logger.LogError(exception.ToString());
            }   
        }
    }
}
