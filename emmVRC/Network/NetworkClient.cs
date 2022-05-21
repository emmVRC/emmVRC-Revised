using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using emmVRC.Functions.Core;
using emmVRC.Libraries;
using emmVRC.Network.Object;
using emmVRC.Objects;
using emmVRC.Objects.ModuleBases;
using emmVRC.TinyJSON;
using emmVRC.Utils;
using MelonLoader;
using UnityEngine.UI;
using UnityEngine.XR;
using VRC.Core;
using Logger = emmVRCLoader.Logger;

namespace emmVRC.Network
{
    [Priority(0)]
    public class NetworkClient : MelonLoaderEvents
    {
        public static HttpClient httpClient;
        public static NetworkConfiguration networkConfiguration;

        private string _configUrl = "https://dl.emmvrc.com/configuration.php";
        private static string _jwtToken;
        public static bool HasJwtToken => !string.IsNullOrWhiteSpace(_jwtToken);

        private static int _currentAttempt;

        internal static event Action onLogin;
        internal static event Action onLogout;

        public override void OnUiManagerInit()
        {
            var refreshThread = new Thread(() =>
            {
                var r = new Random();
                while (true)
                {
                    var randomOffset = r.Next(0, 240000);
                    Thread.Sleep(600000 + randomOffset);
                    RefreshToken().NoAwait("Refresh Token");
                }
                // ReSharper disable once FunctionNeverReturns
            });

            refreshThread.IsBackground = true;
            refreshThread.Start();
            
            Configuration.onConfigUpdated.Add(new KeyValuePair<string, Action>("emmVRCNetworkEnabled", () =>
            {
                if (Configuration.JSONConfig.emmVRCNetworkEnabled)
                {
                    InitializeConfig().NoAwait("Initialize Configuration");
                    AttemptLogin(null, true).NoAwait("Login");
                }
                else
                {
                    DestroySession();
                }
            }));

            if (!Configuration.JSONConfig.emmVRCNetworkEnabled)
                return;
            
            InitializeConfig().NoAwait("Initialize Configuration");
            MelonCoroutines.Start(WaitForAPIUser());
        }
        
        // Await for APIUser to become usable
        private IEnumerator WaitForAPIUser()
        {
            while (APIUser.CurrentUser == null)
                yield return null;
            
            AttemptLogin(null, true).NoAwait("Login");
        }

        private async Task InitializeConfig()
        {
            if (networkConfiguration != null)
                return;
            
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(
                $"emmVRC/1.0 (Client; emmVRCClient/{Attributes.Version}, Headset; {(XRDevice.isPresent ? XRDevice.model : "None")})");
            
            // Let's grab the configuration so we can actually do everything else
            var (httpStatus, response) = await Request.AttemptRequest(HttpMethod.Get, _configUrl, null, true);

            if (httpStatus != HttpStatusCode.OK)
            {
                Logger.LogError("There was an issue initializing the network config. Using defaults... Server responded with: " + httpStatus);
                networkConfiguration = new NetworkConfiguration();
            }
            else
            {
                try
                {
                    networkConfiguration = Decoder.Decode(response).Make<NetworkConfiguration>();

                    await emmVRC.AwaitUpdate.Yield();

                    if (networkConfiguration.MessageID != -1 
                        && Configuration.JSONConfig.LastSeenStartupMessage != networkConfiguration.MessageID)
                        Managers.emmVRCNotificationsManager.AddNotification(new Notification("emmVRC Network Notice", Resources.messageSprite, networkConfiguration.StartupMessage, true, false, null, "", "", true, null, "Dismiss"));
                }
                catch (Exception ex)
                {
                    Logger.LogError($"There was an issue initializing the network config. Using defaults... {ex}");
                    networkConfiguration = new NetworkConfiguration();
                }
            }

            // This is required to work with CoreCLR.
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(
                $"emmVRC/1.0 (Client; emmVRCClient/{Attributes.Version}, Headset; {(XRDevice.isPresent ? XRDevice.model : "None")})");
            
            //httpClient.BaseAddress = new Uri(networkConfiguration.APIUrl);
            //httpClient.BaseAddress = new Uri("http://127.0.0.1:3002/");
            httpClient.BaseAddress = new Uri("https://prod-api.emmvrc.com/");
            Logger.Log("Initialized network config.");
        }
        
        private static void OpenPasswordPrompt()
        {
            VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowInputPopup("Please enter your pin", "", UnityEngine.UI.InputField.InputType.Standard, true, "Login", new Action<string, Il2CppSystem.Collections.Generic.List<UnityEngine.KeyCode>, Text>((pinCode, keyk, tx) =>
            {
                AttemptLogin(pinCode, true).NoAwait("Login");
            }), null, "Enter pin....");
        }

        private static void OpenResetPrompt(string oldPin)
        {
            VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowInputPopup("Please enter your pin", "", UnityEngine.UI.InputField.InputType.Standard, true, "Login", new Action<string, Il2CppSystem.Collections.Generic.List<UnityEngine.KeyCode>, Text>((pinCode, keyk, tx) =>
            {
                AttemptPinReset(oldPin, pinCode).NoAwait("Login");
            }), null, "Enter pin....");
        }

        private static async Task RefreshToken()
        {
            if (HasJwtToken)
            {
                var (httpStatus, response) = await Request.AttemptRequest(Request.Patch, "/auth");

                switch (httpStatus)
                {
                    case HttpStatusCode.OK:
                    {
                        var networkResponse = Decoder.Decode(response);
                        _jwtToken = networkResponse["token"];
                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _jwtToken);
                        break;
                    }
                    case HttpStatusCode.Forbidden:
                    {
                        var networkResponse = Decoder.Decode(response);
                        var banId = networkResponse["ban_id"];
                        var banReason = networkResponse["ban_reason"];
                        var banExpires = DateTime.Parse(networkResponse["ban_expires"]);
                        var banDateText = banExpires.Year == 1 ? "Never" : banExpires.ToString("MMMM dd, yyyy");
                        
                        await emmVRC.AwaitUpdate.Yield();
                        Managers.emmVRCNotificationsManager.AddNotification(new Notification("emmVRC", Resources.errorSprite,
                            $"You cannot connect to the emmVRC Network because you are banned.\n\n" +
                            $"Ban ID: {banId}\n" +
                            $"Reason: {banReason}\n" +
                            $"Expires: {banDateText}",
                            false, false, null, "", "", true, null, "Dismiss"));

                        break;
                    }
                    case HttpStatusCode.ServiceUnavailable:
                        break;
                    default:
                        _jwtToken = null;
                        break;
                }
            }
        }

        private static async Task AttemptPinReset(string originalPinCode, string newPinCode)
        {
            var currentUser = APIUser.CurrentUser;
            var loginInfo = new Dictionary<string, object>()
            {
                ["user_id"] = currentUser.id,
                ["display_name"] = currentUser.GetName(),
                ["password"] = originalPinCode,
                ["new_password"] = newPinCode
            };
            
            var (httpStatus, response) = await Request.AttemptRequest(HttpMethod.Post, "/auth/reset", loginInfo);

            switch (httpStatus)
            {
                case HttpStatusCode.OK:
                {
                    await AttemptLogin(newPinCode, true);
                    break;
                }
            }
        }

        private static async Task AttemptLogin(string pinCode = null, bool resetAttempts = false)
        {
            if (resetAttempts)
                _currentAttempt = 0;

            var currentUser = APIUser.CurrentUser;
            var emaExists = Authentication.Exists(currentUser.id);

            if (networkConfiguration.DeleteAndDisableAuthFile)
            {
                Authentication.DeleteTokenFile(currentUser.id);
                emaExists = false;
            }
            
            var persistentToken = "";

            if (string.IsNullOrWhiteSpace(pinCode) && emaExists)
                persistentToken = Authentication.ReadTokenFile(currentUser.id);

            if (string.IsNullOrWhiteSpace(pinCode) && string.IsNullOrWhiteSpace(persistentToken))
            {
                if (emaExists)
                    Authentication.DeleteTokenFile(currentUser.id);
                
                await emmVRC.AwaitUpdate.Yield();
                Managers.emmVRCNotificationsManager.AddNotification(new Notification("emmVRC", Resources.alertSprite,
                    "You need to log in to the emmVRC Network. If you have a pin, enter it. If you do not have a pin, enter your new pin.\n\n" +
                    "Your pin is the equivalent of your password to connect to the emmVRC Network." +
                    " Do not just enter a random number; make it something memorable!\n\n" +
                    "If you have forgotten your pin, or are experiencing issues, please contact us in the emmVRC Discord.",
                    false, true, OpenPasswordPrompt, "Login", "", true, null, "Dismiss"));
                return;
            }
            
            var loginInfo = new Dictionary<string, object>()
            {
                ["user_id"] = currentUser.id,
                ["display_name"] = currentUser.GetName(),
                ["password"] = pinCode ?? "",
                ["persistent_token"] = persistentToken,
                ["need_persistent_token"] = !emaExists
            };
            
            var (httpStatus, response) = await Request.AttemptRequest(HttpMethod.Post,
                "/auth", loginInfo);

            switch (httpStatus)
            {
                case HttpStatusCode.OK:
                {
                    var networkResponse = Decoder.Decode(response);
                    _jwtToken = networkResponse["token"];
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _jwtToken);

                    if (!emaExists)
                    {
                        try
                        {
                            Authentication.CreateTokenFile(currentUser.id, networkResponse["persistent_token"]);
                        }
                        catch (Exception ex)
                        {
                            Logger.Log($"There was an issue creating the persistent token file. {ex}");
                        }
                    }

                    await emmVRC.AwaitUpdate.Yield();
                    onLogin?.DelegateSafeInvoke();

                    break;
                }

                case HttpStatusCode.UpgradeRequired:
                {
                    if (emaExists)
                        Authentication.DeleteTokenFile(currentUser.id);
                    
                    await emmVRC.AwaitUpdate.Yield();
                    Managers.emmVRCNotificationsManager.AddNotification(new Notification("emmVRC", Resources.alertSprite,
                        "Your pin is required to be reset to access the emmVRC Network.",
                        false, true, () => OpenResetPrompt(pinCode), "Reset", "", true, null, "Dismiss"));
                    
                    break;
                }
                
                case HttpStatusCode.BadRequest:
                case HttpStatusCode.Unauthorized:
                {
                    if (emaExists)
                        Authentication.DeleteTokenFile(currentUser.id);
                    
                    await emmVRC.AwaitUpdate.Yield();
                    Managers.emmVRCNotificationsManager.AddNotification(new Notification("emmVRC", Resources.alertSprite,
                            "You need to log in to the emmVRC Network. If you have a pin, enter it. If you do not have a pin, enter your new pin.\n\n" +
                            "Your pin is the equivalent of your password to connect to the emmVRC Network." +
                            " Do not just enter a random number; make it something memorable!\n\n" +
                            "If you have forgotten your pin, or are experiencing issues, please contact us in the emmVRC Discord.",
                            false, true, OpenPasswordPrompt, "Login", "", true, null, "Dismiss"));
                    
                    break;
                }

                case HttpStatusCode.Forbidden:
                {
                    var networkResponse = Decoder.Decode(response);
                    var banId = networkResponse["ban_id"];
                    var banReason = networkResponse["ban_reason"];
                    var banExpires = DateTime.Parse(networkResponse["ban_expires"]);
                    var banDateText = banExpires.Year == 1 ? "Never" : banExpires.ToString("MMMM dd, yyyy");
                    
                    await emmVRC.AwaitUpdate.Yield();
                    Managers.emmVRCNotificationsManager.AddNotification(new Notification("emmVRC", Resources.errorSprite,
                        $"You cannot connect to the emmVRC Network because you are banned.\n\n" +
                        $"Ban ID: {banId}\n" +
                        $"Reason: {banReason}\n" +
                        $"Expires: {banDateText}",
                        false, false, null, "", "", true, null, "Dismiss"));
                    break;
                }

                default:
                {
                    await emmVRC.AwaitUpdate.Yield();
                    Managers.emmVRCNotificationsManager.AddNotification(new Notification("emmVRC", Functions.Core.Resources.errorSprite, "The emmVRC Network is currently unavailable. Please try again later.", false, true,
                        () => AttemptLogin(pinCode, true).NoAwait("Login"), "Reconnect", "", true, null, "Dismiss"));
                    break;
                }
            }
        }

        public static void DestroySession()
        {
            if (httpClient == null) return;
            if (networkConfiguration == null) return;
            if (string.IsNullOrEmpty(_jwtToken)) return;
            
            Request.AttemptRequest(HttpMethod.Delete, "/auth").NoAwait("Destroy Session");
            _jwtToken = null;

            onLogout?.DelegateSafeInvoke();
        }
    }
}
