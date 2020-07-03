﻿using System;
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
        private static string BaseAddress = "https://crreamhub.com";//"https://thetrueyoshifan.com";
        private static int Port = 3000;
        public static string baseURL { get { return BaseAddress + ":" + Port; } }
        public static string configURL { get { return "https://thetrueyoshifan.com"; } } // TODO: Integrate this with the API
        private static string LoginKey;
        private static string _authToken;
        private static bool prompted = false;
        private static bool userIDTried = false;
        private static bool keyFileTried = false;
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

            /*            if (!Authentication.Authentication.Exists(VRC.Core.APIUser.CurrentUser.id) && !prompted)
                        {
                            prompted = true;
                            Managers.NotificationManager.AddNotification("Better protect your emmVRC account by setting a pin\n\nAlready have a pin? Login!", "Set\nPassword", () => { Managers.NotificationManager.DismissCurrentNotification(); PromptLogin(); }, "Login", () => { Managers.NotificationManager.DismissCurrentNotification(); PromptLogin(); }, Resources.alertSprite, -1);
                        }*/
        }

        public static void PromptLogin()
        {
            OpenPasswordPrompt();
        }

        private static void OpenPasswordPrompt()
        {
            InputUtilities.OpenNumberPad("To login, please enter your pin", "Login", (string password) => { 
                login(password);
                VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup();
            });
        }
        private static void RequestNewPin()
        {
            InputUtilities.OpenNumberPad("Please enter your new pin", "Set Pin", (string pin) => {
                InputUtilities.OpenNumberPad("Please confirm your new pin", "Confirm", (string pin2) =>
                {
                    if (pin == pin2)
                    {
                        MelonLoader.MelonCoroutines.Start(sendLogin(pin2));
                        VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup();
                    }
                    else
                    {
                        VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowStandardPopup("emmVRC", "The pins you entered did not match. Please try again.", "Okay", () => { VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup(); });
                    }
                });
            });
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
            if (password == "" && !userIDTried)
            {
                LoginKey = VRC.Core.APIUser.CurrentUser.id;
                userIDTried = true;
            }
            else if (password == "" && Authentication.Authentication.Exists(VRC.Core.APIUser.CurrentUser.id) && userIDTried && !keyFileTried)
            {
                LoginKey = Authentication.Authentication.ReadTokenFile(VRC.Core.APIUser.CurrentUser.id);
                keyFileTried = true;
            }
            else if (password != "" || (!Authentication.Authentication.Exists(VRC.Core.APIUser.CurrentUser.id) && userIDTried))
            {
                LoginKey = password;
                Authentication.Authentication.DeleteTokenFile(VRC.Core.APIUser.CurrentUser.id);
            }
            else
            {
                emmVRCLoader.Logger.LogError(":catShrug:");
                emmVRCLoader.Logger.LogDebug("userIDTried = " + userIDTried);
                emmVRCLoader.Logger.LogDebug("keyFileTried = " +keyFileTried);
            }


            string createFile = "0";
            if (!Authentication.Authentication.Exists(VRC.Core.APIUser.CurrentUser.id))
                createFile = "1";
            try
            {
                TinyJSON.Variant result = HTTPResponse.Serialize(await HTTPRequest.post(NetworkClient.baseURL + "/api/authentication/login", new Dictionary<string, string>() { ["username"] = VRC.Core.APIUser.CurrentUser.id, ["name"] = VRC.Core.APIUser.CurrentUser.displayName, ["password"] = LoginKey, ["loginKey"] = createFile }));
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
            }
            catch (Exception exception)
            {
                if (exception.ToString().Contains("unauthorized") || exception.ToString().Contains("Unauthorized"))
                {
                    if (userIDTried && (!keyFileTried || password != ""))
                    {
                        sendRequest(password);
                    } else
                        Managers.NotificationManager.AddNotification("You need to log in to emmVRC.\nIf you have forgotten, or do not have a pin, please contact us in the emmVRC Discord.", "Login", () => { Managers.NotificationManager.DismissCurrentNotification(); PromptLogin(); }, "Dismiss", Managers.NotificationManager.DismissCurrentNotification, Resources.alertSprite, -1);
                }
                else if (exception.ToString().Contains("forbidden") || exception.ToString().Contains("Forbidden"))
                {
                    Managers.NotificationManager.AddNotification("You have tried to log in too many times. Please try again later.", "Dismiss", Managers.NotificationManager.DismissCurrentNotification, "", null, Resources.errorSprite, -1);
                }
                else
                {
                    Managers.NotificationManager.AddNotification("The emmVRC Network is currently unavailable. Please try again later.", "Reconnect", () => {
                        Network.NetworkClient.InitializeClient();
                        //Network.NetworkClient.PromptLogin();
                        MelonLoader.MelonCoroutines.Start(emmVRC.loadNetworked());
                        Managers.NotificationManager.DismissCurrentNotification();
                    }, "Dismiss", Managers.NotificationManager.DismissCurrentNotification, Resources.errorSprite, -1);
                    emmVRCLoader.Logger.LogError(exception.ToString());
                }
            }   
        }
    }
}
