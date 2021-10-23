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
using emmVRC.Objects.ModuleBases;
using emmVRC.Libraries;
using emmVRC.Utils;
using VRC.Core;

namespace emmVRC.Network
{
    [Priority(0)]
    public class NetworkClient : MelonLoaderEvents
    {
        //TODO add caching
        //TODO add sockets
        private const string BaseAddress = "https://api.emmvrc.com";
        private static int Port = 443;
        public static string baseURL { get { return BaseAddress + ":" + Port; } }
        public const string configURL = "https://dl.emmvrc.com"; // TODO: Integrate this with the API
        private static string LoginKey;
        private static string LoginToken;
        private static string _webToken;
        private static bool keyFileTried = false;
        private static bool passwordTried = false;
        public static int retries = 0;
        private static string message = "To those interested; this class is very much temporary. The entire network is going to be rewritten at some point soon.";
        internal static event Action onLogin;
        internal static event Action onLogout;
        public static string webToken
        {
            get { return _webToken; }
            set
            {
                _webToken = value;
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _webToken);
            }
        }
        public static HttpClient httpClient { get; set; }
        public override void OnUiManagerInit()
        {
            Configuration.onConfigUpdated.Add(new System.Collections.Generic.KeyValuePair<string, Action>("emmVRCNetworkEnabled", () =>
            {
                if (Configuration.JSONConfig.emmVRCNetworkEnabled)
                    Network.NetworkClient.ClearAndLogin();
                else
                    Logout();
            }));
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("emmVRC/1.0 (Client; emmVRCClient/" + Attributes.Version + ", Headset; "+(UnityEngine.XR.XRDevice.isPresent ? UnityEngine.XR.XRDevice.model : "None")+")");
            if (Configuration.JSONConfig.emmVRCNetworkEnabled)
                ClearAndLogin();
        }
        public static void ClearAndLogin()
        {
            retries = 0;
            fetchConfig().NoAwait(nameof(fetchConfig));
            login().NoAwait(nameof(login));
        }
        public static void Logout()
        {
            if (webToken != null)
                HTTPRequest.get(baseURL + "/api/authentication/logout").NoAwait("Logout");
            webToken = null;
            onLogout?.DelegateSafeInvoke();
        }

        public static T DestroyClient<T>(Func<T> callback = null)
        {
            httpClient = null;
            webToken = null;
            return callback();
        }

        private static async Task fetchConfig()
        {
            while (RoomManager.field_Internal_Static_ApiWorld_0 == null)
                await emmVRC.AwaitUpdate.Yield();
            await getConfigAsync();
        }

        private static async Task login(string password = "")
        {
            while (RoomManager.field_Internal_Static_ApiWorld_0 == null)
                await emmVRC.AwaitUpdate.Yield();
            await sendRequest(password);
        }

        public static void PromptLogin()
        {
            OpenPasswordPrompt();
        }

        private static void OpenPasswordPrompt()
        {
            VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowInputPopup("Please enter your pin", "", UnityEngine.UI.InputField.InputType.Standard, true, "Login", new System.Action<string, Il2CppSystem.Collections.Generic.List<UnityEngine.KeyCode>, UnityEngine.UI.Text>((string password, Il2CppSystem.Collections.Generic.List<UnityEngine.KeyCode> keyk, UnityEngine.UI.Text tx) =>
            {
                passwordTried = false;
                login(password).NoAwait(nameof(login));
            }), null, "Enter pin....");
        }
        private static async Task getConfigAsync()
        {
            try
            {
                string result = await HTTPRequest.get(NetworkClient.configURL + "/configuration.php");
                NetworkConfig.Instance = TinyJSON.Decoder.Decode(result).Make<NetworkConfig>();
                await emmVRC.AwaitUpdate.Yield();
                if (NetworkConfig.Instance.MessageID != -1 && Configuration.JSONConfig.LastSeenStartupMessage != NetworkConfig.Instance.MessageID){
                    Managers.emmVRCNotificationsManager.AddNotification(new Notification("emmVRC Network Notice", null, NetworkConfig.Instance.StartupMessage, true, false, null, "", "", true, null, "Dismiss"));
                }
            }
            catch (Exception exception)
            {
                emmVRCLoader.Logger.LogError("Client configuration could not be fetched from emmVRC. Assuming default values. Error: " + exception);
                NetworkConfig.Instance = new NetworkConfig();
            }
        }
        private static async Task sendRequest(string password = "")
        {
            var currentUser = APIUser.CurrentUser;
            var emaExists = Authentication.Exists(currentUser.id);

            if (NetworkConfig.Instance.DeleteAndDisableAuthFile)
            {
                Authentication.DeleteTokenFile(currentUser.id);
            }
            else if (string.IsNullOrWhiteSpace(password) && emaExists)
            {
                LoginToken = Authentication.ReadTokenFile(currentUser.id);

                if (password == "")
                    keyFileTried = true;
            }

            if (LoginToken == null)
                LoginToken = "";

            LoginKey = password;

            var createFile = emaExists ? "0" : "1";
            var response = "undefined";

            try
            {
                response = await HTTPRequest.post($"{NetworkClient.baseURL}/api/authentication/login", new Dictionary<string, string>()
                {
                    ["username"] = currentUser.id,
                    ["name"] = currentUser.GetName(),
                    ["password"] = LoginKey,
                    ["loginToken"] = string.IsNullOrWhiteSpace(password) ? LoginToken : "",
                    ["loginKey"] = createFile
                });

                var result = HTTPResponse.Serialize(response);
                NetworkClient.webToken = result["token"];

                await emmVRC.AwaitUpdate.Yield();

                if (createFile == "1")
                {
                    try
                    {
                        Authentication.CreateTokenFile(currentUser.id, result["loginKey"]);
                    }
                    catch (Exception ex)
                    {
                        emmVRCLoader.Logger.Log($"There was an issue saving your emmVRC Network token.\n{ex}");
                    }
                }

                keyFileTried = false;
                passwordTried = false;

                onLogin?.DelegateSafeInvoke();
            }
            catch (Exception)
            {
                await emmVRC.AwaitUpdate.Yield();
                response = response.ToLower();

                if (response.Contains("unauthorized"))
                {
                    if (response.Contains("banned"))
                    {
                        Managers.emmVRCNotificationsManager.AddNotification(new Notification("emmVRC", Functions.Core.Resources.errorSprite, "You cannot connect to the emmVRC Network because you are banned.", false, false, null, "", "", true, null, "Dismiss"));
                    }
                    else
                    {
                        if (keyFileTried && emaExists)
                            Authentication.DeleteTokenFile(currentUser.id);

                        if (keyFileTried && password != "" && !passwordTried && !NetworkConfig.Instance.DisableAuthFile && !NetworkConfig.Instance.DeleteAndDisableAuthFile)
                        {
                            passwordTried = true;
                            await sendRequest(password);
                        }
                        else
                        {
                            await emmVRC.AwaitUpdate.Yield();
                            Managers.emmVRCNotificationsManager.AddNotification(new Notification("emmVRC", Functions.Core.Resources.alertSprite, "You need to log in to the emmVRC Network. Please log in or enter a pin to create one. If you have forgotten your pin, or are experiencing issues, please contact us in the emmVRC Discord.", false, true, PromptLogin, "Login", "", true, null, "Dismiss"));
                        }
                    }
                }
                else if (response.ToLower().Contains("forbidden"))
                {
                    Managers.emmVRCNotificationsManager.AddNotification(new Notification("emmVRC", Functions.Core.Resources.errorSprite, "You have tried to log in too many times. Please try again later.", false, false, null, "", "", true, null, "Dismiss"));
                }
                else
                {
                    if (retries++ <= NetworkConfig.Instance.NetworkAutoRetries)
                    {
                        await sendRequest(password);
                    }
                    else
                    {
                        Managers.emmVRCNotificationsManager.AddNotification(new Notification("emmVRC", Functions.Core.Resources.errorSprite, "The emmVRC Network is currently unavailable. Please try again later.", false, true, ClearAndLogin, "Reconnect", "", true, null, "Dismiss"));
                    }
                }
            }
        }
    }
}