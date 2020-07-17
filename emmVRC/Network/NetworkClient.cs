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
using VRC.Core;
using System.Linq.Expressions;

namespace emmVRC.Network
{
    public static class NetworkClient
    {
        //TODO add caching
        //TODO add sockets
        private static string BaseAddress = "https://thetrueyoshifan.com";
        private static int Port = 3000;
        public static string baseURL { get { return BaseAddress + ":" + Port; } }
        public static string configURL { get { return "https://thetrueyoshifan.com"; } } // TODO: Integrate this with the API
        private static string LoginKey;
        private static string _authToken;
        private static bool prompted = false;
        private static bool userIDTried = false;
        private static bool keyFileTried = false;
        private static bool passwordTried = false;
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
            fetchConfig();
            login();
            //return callback();
        }

        public static T DestroyClient<T>(Func<T> callback = null)
        {
            httpClient = null;
            authToken = null;
            return callback();
        }

        public static void fetchConfig()
        {
            MelonLoader.MelonCoroutines.Start(goFetchConfig());
        }

        private static IEnumerator goFetchConfig()
        {
            while (RoomManager.field_Internal_Static_ApiWorld_0 == null)
                yield return new WaitForEndOfFrame();
            getConfigAsync();
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
        }

        public static void PromptLogin()
        {
            OpenPasswordPrompt();
        }

        private static void OpenPasswordPrompt()
        {
            VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowInputPopup("To login, please enter your pin", "", UnityEngine.UI.InputField.InputType.Standard, true, "Login", new System.Action<string, Il2CppSystem.Collections.Generic.List<UnityEngine.KeyCode>, UnityEngine.UI.Text>((string password, Il2CppSystem.Collections.Generic.List<UnityEngine.KeyCode> keyk, UnityEngine.UI.Text tx) =>
            {
                passwordTried = false;
                login(password);
            }), null, "Enter pin....");
        }
        private static void RequestNewPin()
        {
            VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowInputPopup("Please enter your new pin", "", UnityEngine.UI.InputField.InputType.Standard, true, "Set Pin", new System.Action<string, Il2CppSystem.Collections.Generic.List<UnityEngine.KeyCode>, UnityEngine.UI.Text>((string pin, Il2CppSystem.Collections.Generic.List<UnityEngine.KeyCode> keyk, UnityEngine.UI.Text tx) => {
                VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowInputPopup("Please confirm your new pin", "", UnityEngine.UI.InputField.InputType.Standard, true, "Set Pin", new System.Action<string, Il2CppSystem.Collections.Generic.List<UnityEngine.KeyCode>, UnityEngine.UI.Text>((string pin2, Il2CppSystem.Collections.Generic.List<UnityEngine.KeyCode> keyk2, UnityEngine.UI.Text tx2) => {
                    if (pin == pin2)
                    {
                        MelonLoader.MelonCoroutines.Start(sendLogin(pin2));
                    }
                    else
                    {
                        VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowStandardPopup("emmVRC", "The pins you entered did not match. Please try again.", "Okay", () => { VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup(); });
                    }
                }), null, "Enter pin....");
            }), null, "Enter pin....");
        }
        private static async void getConfigAsync()
        {
            try
            {
                string result = await HTTPRequest.get(NetworkClient.configURL + "/configuration.php");
                NetworkConfig.Instance = TinyJSON.Decoder.Decode(result).Make<NetworkConfig>();
            }
            catch (Exception exception)
            {
                emmVRCLoader.Logger.LogError("Client configuration could not be fetched from emmVRC. Assuming default values. Error: "+exception.ToString());
                NetworkConfig.Instance = new NetworkConfig();
            }
        }
        private static async void sendRequest(string password = "")
        {
            if (NetworkConfig.Instance.DeleteAndDisableAuthFile)
            {
                Authentication.Authentication.DeleteTokenFile(APIUser.CurrentUser.id);
            }
            if (password == "" && !userIDTried)
            {
                LoginKey = VRC.Core.APIUser.CurrentUser.id;
                userIDTried = true;
            }
            else if (password == "" && userIDTried && Authentication.Authentication.Exists(VRC.Core.APIUser.CurrentUser.id))
            {
                LoginKey = Authentication.Authentication.ReadTokenFile(APIUser.CurrentUser.id);
            }
            else
            {
                LoginKey = password;
            }

            string createFile = "0";
            if (!Authentication.Authentication.Exists(VRC.Core.APIUser.CurrentUser.id))
                createFile = "1";
            string response = "undefined";
            try
            {
                //emmVRCLoader.Logger.LogDebug("User ID Tried: " + userIDTried);
                //emmVRCLoader.Logger.LogDebug("Keyfile Tried: " + keyFileTried);
                //emmVRCLoader.Logger.LogDebug("Password Tried: " + passwordTried);
                response = await HTTPRequest.post(NetworkClient.baseURL + "/api/authentication/login", new Dictionary<string, string>() { ["username"] = VRC.Core.APIUser.CurrentUser.id, ["name"] = VRC.Core.APIUser.CurrentUser.displayName, ["password"] = LoginKey, ["loginKey"] = createFile });
                TinyJSON.Variant result = HTTPResponse.Serialize(response);
                NetworkClient.authToken = result["token"];
                if (result["reset"])
                {
                    Managers.NotificationManager.AddNotification("You need to set a pin to protect your emmVRC account.", "Set\nPin", () => {
                        Managers.NotificationManager.DismissCurrentNotification();
                        RequestNewPin();
                    }, "Dismiss", Managers.NotificationManager.DismissCurrentNotification, Resources.alertSprite);
                }
                if (createFile == "1" && LoginKey != APIUser.CurrentUser.id)
                {
                    try
                    {
                        Authentication.Authentication.CreateTokenFile(VRC.Core.APIUser.CurrentUser.id, result["loginKey"]);
                    } catch (Exception ex)
                    {
                        emmVRCLoader.Logger.LogError(ex.ToString());
                    }
                    LoginKey = result["loginKey"];
                }
                userIDTried = false;
                keyFileTried = false;
                passwordTried = false;
            }
            catch (Exception exception)
            {
                if (response.ToLower().Contains("unauthorized"))
                {
                    //emmVRCLoader.Logger.LogDebug("Password: " + password);
                    if (keyFileTried && Authentication.Authentication.Exists(APIUser.CurrentUser.id))
                        Authentication.Authentication.DeleteTokenFile(APIUser.CurrentUser.id);
                    if (userIDTried && !keyFileTried)
                    {
                        sendRequest();
                        keyFileTried = true;
                    }
                    else if (userIDTried && keyFileTried && password != "" && !passwordTried && !NetworkConfig.Instance.DisableAuthFile && !NetworkConfig.Instance.DeleteAndDisableAuthFile)
                    {
                        sendRequest(password);
                        passwordTried = true;
                    }
                    else
                    {
                        Managers.NotificationManager.AddNotification("You need to log in to emmVRC.\nIf you have forgotten, or do not have a pin, please contact us in the emmVRC Discord.", "Login", () => { Managers.NotificationManager.DismissCurrentNotification(); PromptLogin(); }, "Dismiss", Managers.NotificationManager.DismissCurrentNotification, Resources.alertSprite, -1);
                    }
                }
                else if (response.ToLower().Contains("forbidden"))
                {
                    Managers.NotificationManager.AddNotification("You have tried to log in too many times. Please try again later.", "Dismiss", Managers.NotificationManager.DismissCurrentNotification, "", null, Resources.errorSprite, -1);
                    exception = new Exception();
                }
                else
                {
                    Managers.NotificationManager.AddNotification("The emmVRC Network is currently unavailable. Please try again later.", "Reconnect", () => {
                        Network.NetworkClient.InitializeClient();
                        MelonLoader.MelonCoroutines.Start(emmVRC.loadNetworked());
                        Managers.NotificationManager.DismissCurrentNotification();
                    }, "Dismiss", Managers.NotificationManager.DismissCurrentNotification, Resources.errorSprite, -1);
                    //emmVRCLoader.Logger.LogError(response);
                }
            }   
        }
    }
}
