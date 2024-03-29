﻿using System;
using Il2CppSystem.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using VRC.Core;
using VRC.UI;
using emmVRC.Utils;
using emmVRC.Libraries;
using System.Linq;
using emmVRC.Objects;
using System.Reflection;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using emmVRC.Network;
using emmVRC.Network.Object;
using emmVRC.Objects.ModuleBases;
using emmVRC.TinyJSON;
using MelonLoader;
using UnhollowerBaseLib;
using UnhollowerRuntimeLib.XrefScans;
using VRC;
using Avatar = emmVRC.Network.Object.Avatar;
using Logger = emmVRCLoader.Logger;

namespace emmVRC.Functions.UI
{
    public class CustomAvatarFavorites : MelonLoaderEvents, IWithUpdate
    {
        public enum SortingMode
        {
            DateAdded = 0,
            Alphabetical = 1,
            Creator = 2
        }
        internal static GameObject PublicAvatarList;
        internal static UiAvatarList NewAvatarList;
        private static GameObject avText;
        private static Text avTextText;
        private static GameObject ChangeButton;
        public static Button.ButtonClickedEvent baseChooseEvent;
        private static GameObject FavoriteButton;
        private static GameObject FavoriteButtonNew;
        private static Button FavoriteButtonNewButton;
        private static Text FavoriteButtonNewText;
        //private static GameObject ShowAuthorButton;
        public static GameObject pageAvatar;
        private static PageAvatar currPageAvatar;
        private static bool error = false;
        private static bool errorWarned;
        private static bool Searching = false;
        public static List<ApiAvatar> LoadedAvatars;
        private static List<ApiAvatar> SearchedAvatars;
        private static UiInputField searchBox;
        private static UnityAction<string> searchBoxAction;
        private static GameObject refreshButton;
        private static GameObject backButton;
        private static GameObject forwardButton;
        private static GameObject pageTicker;
        private static GameObject sortButton;
        private static bool waitingForSearch = false;
        public static int currentPage = 0;
        private static SortingMode currentSortingMode = SortingMode.DateAdded;
        private static bool sortingInverse = false; // False = First-to-Last, True = Last-to-First

        private static int _apiAvatarOffset;
        
        private delegate void SetPickerContentFromApiModelDelegate(IntPtr thisPtr, IntPtr nativeMethodInfo);
        private static SetPickerContentFromApiModelDelegate _ourSetPickerContentFromApiModelDelegate;
        
        private static void SetPickerContentFromApiModelPatch(IntPtr thisPtr, IntPtr nativeMethodInfo)
        {
            DecodeApiAvatar(thisPtr, nativeMethodInfo).NoAwait("ApiAvatar Decode");
        }

        private static async Task DecodeApiAvatar(IntPtr thisPtr, IntPtr nativeMethodInfo)
        {
            ApiAvatar apiAvatar = null;
            
            unsafe
            {
                var apiAvatarPtr = *(IntPtr*)(thisPtr + _apiAvatarOffset);
                if (apiAvatarPtr != IntPtr.Zero)
                    apiAvatar = new ApiAvatar(apiAvatarPtr);
            }

            if (apiAvatar == null)
            {
                await emmVRC.AwaitUpdate.Yield();
                _ourSetPickerContentFromApiModelDelegate(thisPtr, nativeMethodInfo);
                
                return;
            }
            
            if (apiAvatar.tags != null && apiAvatar.tags.Contains("avatar_needs_decrypt"))
            {
                apiAvatar.tags.Remove("avatar_needs_decrypt");
                
                try
                {
                    var (httpStatus, response) =
                        await Request.AttemptRequest(HttpMethod.Get, $"/avatar/info/{apiAvatar.id}");

                    if (httpStatus == HttpStatusCode.OK)
                    {
                        var decodedAvatar = TinyJSON.Decoder.Decode(response).Make<Avatar>();

                        if (decodedAvatar != null)
                        {
                            if (!string.IsNullOrWhiteSpace(decodedAvatar.avatar_id) &&
                                !string.IsNullOrWhiteSpace(decodedAvatar.avatar_asset_url))
                            {
                                apiAvatar.id = decodedAvatar.avatar_id;
                                apiAvatar.assetUrl = decodedAvatar.avatar_asset_url;
                            }
                            else
                            {
                                apiAvatar.id = "avtr_c38a1615-5bf5-42b4-84eb-a8b6c37cbd11";
                                apiAvatar.assetUrl = "";
                            }
                        }
                    }
                    else
                    {
                        apiAvatar.id = "avtr_c38a1615-5bf5-42b4-84eb-a8b6c37cbd11";
                        apiAvatar.assetUrl = "";
                    }
                }
                catch (Exception e)
                {
                    apiAvatar.id = "avtr_c38a1615-5bf5-42b4-84eb-a8b6c37cbd11";
                    apiAvatar.assetUrl = "";
                }
            }
            
            await emmVRC.AwaitUpdate.Yield();
            _ourSetPickerContentFromApiModelDelegate(thisPtr, nativeMethodInfo);
        }

        public override void OnUiManagerInit()
        {
            try
            {
                unsafe
                {
                    //var setPickerFromContentFromApiModel =
                    //    typeof(UiAvatarList.__c__DisplayClass28_1)
                    //        .GetMethod(nameof(UiAvatarList.__c__DisplayClass28_1._SetPickerContentFromApiModel_b__1));
                    var setPickerContentFromApiModel =
                        typeof(UiAvatarList.ObjectNPrivateSealedApObApUnique)
                            .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                            .First(m => XrefScanner.XrefScan(m)
                                .Any(xi =>
                                    xi.Type == XrefType.Global
                                    && xi.ReadAsObject() != null &&
                                    xi.ReadAsObject().ToString()
                                        .Equals("You cannot use this avatar as it has not been published for this platform.")));

                    var apiAvatarField = typeof(UiAvatarList.ObjectNPrivateSealedApObApUnique).GetProperty(nameof(
                        UiAvatarList.ObjectNPrivateSealedApObApUnique.field_Public_ApiAvatar_1));
                    _apiAvatarOffset = (int)IL2CPP.il2cpp_field_get_offset((IntPtr)UnhollowerUtils
                        .GetIl2CppFieldInfoPointerFieldForGeneratedFieldAccessor(apiAvatarField.GetMethod)
                        .GetValue(null));

                    var originalMethodPointer = *(IntPtr*)(IntPtr)UnhollowerUtils
                        .GetIl2CppMethodInfoPointerFieldForGeneratedMethod(setPickerContentFromApiModel)
                        .GetValue(null);
                    
                    MelonUtils.NativeHookAttach((IntPtr)(&originalMethodPointer), 
                        Marshal.GetFunctionPointerForDelegate<SetPickerContentFromApiModelDelegate>(SetPickerContentFromApiModelPatch));
                    _ourSetPickerContentFromApiModelDelegate =
                        Marshal.GetDelegateForFunctionPointer<SetPickerContentFromApiModelDelegate>(
                            originalMethodPointer);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }
            
            if (Configuration.JSONConfig.SortingMode <= 2)
                currentSortingMode = (SortingMode)Configuration.JSONConfig.SortingMode;
            else
                currentSortingMode = 0;
            sortingInverse = Configuration.JSONConfig.SortingInverse;

            VRC.UI.PageAvatar pageAvatarComp = UnityEngine.Resources.FindObjectsOfTypeAll<VRC.UI.PageAvatar>().FirstOrDefault();

            pageAvatar = pageAvatarComp.gameObject;
            FavoriteButton = pageAvatarComp.transform.Find("Favorite Button").gameObject;
            FavoriteButtonNew = UnityEngine.Object.Instantiate<GameObject>(FavoriteButton, pageAvatarComp.transform);
            FavoriteButtonNewButton = FavoriteButtonNew.GetComponent<Button>();
            FavoriteButtonNewButton.onClick.RemoveAllListeners();
            FavoriteButtonNewButton.onClick.AddListener(new System.Action(() =>
            {

                ApiAvatar apiAvatar = pageAvatar.GetComponent<PageAvatar>().field_Public_SimpleAvatarPedestal_0.field_Internal_ApiAvatar_0;
                bool flag = false;
                foreach (var avatar in LoadedAvatars)
                    if (avatar.id == apiAvatar.id)
                        flag = true;
                if (!flag)
                {

                    if (!Utils.PlayerUtils.DoesUserHaveVRCPlus())
                        VRCUiPopupManager.prop_VRCUiPopupManager_0.ShowAlert("VRChat Plus Required", Core.Localization.currentLanguage.VRCPlusMessage, 0f);
                    else
                    {
                        if (((apiAvatar.releaseStatus == "public" || apiAvatar.authorId == APIUser.CurrentUser.id) && apiAvatar.releaseStatus != null))
                        {
                            FavoriteAvatar(apiAvatar).NoAwait(nameof(FavoriteAvatar));
                        }
                        else
                        {
                            VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowStandardPopup("emmVRC", Core.Localization.currentLanguage.FavouriteFailedPrivateMessage, "Dismiss", new System.Action(() => { VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup(); }));
                        }
                    }
                }
                else
                {
                    VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowStandardPopup("emmVRC", "Are you sure you want to unfavorite the avatar \"" + apiAvatar.name + "\"?", "Yes", new System.Action(() =>
                    {
                        UnfavoriteAvatar(apiAvatar).NoAwait(nameof(UnfavoriteAvatar));
                        VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup();
                    }), "No", () =>
                    {
                        VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup();
                    });
                }
            }));

            FavoriteButtonNew.GetComponentInChildren<RectTransform>().localPosition += new Vector3(0, 165f);
            FavoriteButtonNewText = FavoriteButtonNew.GetComponentInChildren<Text>();
            FavoriteButtonNewText.supportRichText = true;

            FavoriteButtonNew.transform.Find("Horizontal").GetComponentsInChildren<Transform>(true).ToList().ForEach(a =>
            {
                if (a.name != "FavoriteActionText" && a.name != "Horizontal")
                    a.gameObject.SetActive(false);
            });

            var oldPublicAvatarList = pageAvatarComp.transform.Find("Vertical Scroll View/Viewport/Content/Legacy Avatar List").gameObject;
            oldPublicAvatarList = pageAvatarComp.transform.Find("Vertical Scroll View/Viewport/Content/Legacy Avatar List").gameObject;
            PublicAvatarList = GameObject.Instantiate(oldPublicAvatarList, oldPublicAvatarList.transform.parent);
            PublicAvatarList.transform.SetAsFirstSibling();

            ChangeButton = pageAvatarComp.transform.Find("Change Button").gameObject;
            baseChooseEvent = ChangeButton.GetComponent<Button>().onClick;
            ChangeButton.GetComponent<Button>().onClick = new Button.ButtonClickedEvent();
            ChangeButton.GetComponent<Button>().onClick.AddListener(new System.Action(() =>
            {
                ApiAvatar selectedAvatar = pageAvatar.GetComponent<PageAvatar>().field_Public_SimpleAvatarPedestal_0.field_Internal_ApiAvatar_0;
                if (LoadedAvatars.ToArray().Any(a => a.id == selectedAvatar.id) || SearchedAvatars.ToArray().Any(a => a.id == selectedAvatar.id))
                {
                    //emmVRCLoader.Bootstrapper.Instance.StartCoroutine(CheckAvatar());
                    if (selectedAvatar.releaseStatus == "private" && selectedAvatar.authorId != APIUser.CurrentUser.id)
                    {
                        if (LoadedAvatars.ToArray().Any(a => a.id == selectedAvatar.id))
                            VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowStandardPopup("emmVRC", Core.Localization.currentLanguage.AvatarSwitchPrivateUnfavouriteMessage, "Yes", new System.Action(() => { UnfavoriteAvatar(selectedAvatar).NoAwait(nameof(UnfavoriteAvatar)); VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup(); }), "No", new System.Action(() => { VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup(); }));
                        else
                            VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowStandardPopup("emmVRC", Core.Localization.currentLanguage.AvatarSwitchPrivateMessage, "Dismiss", new System.Action(() => { VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup(); }));
                    }
                    else if (selectedAvatar.releaseStatus == "unavailable")
                    {
                        if (LoadedAvatars.ToArray().Any(a => a.id == selectedAvatar.id))
                            VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowStandardPopup("emmVRC", Core.Localization.currentLanguage.AvatarSwitchDeletedUnfavouriteMessage, "Yes", new System.Action(() => { UnfavoriteAvatar(selectedAvatar).NoAwait(nameof(UnfavoriteAvatar)); VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup(); }), "No", new System.Action(() => { VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup(); }));
                        else
                            VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowStandardPopup("emmVRC", Core.Localization.currentLanguage.AvatarSwitchDeletedMessage, "Dismiss", new System.Action(() => { VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup(); }));
                    }
                    else
                        baseChooseEvent.Invoke();
                }
                else
                {
                    baseChooseEvent.Invoke();
                }
            }));

            avText = PublicAvatarList.transform.Find("Button").gameObject;
            avTextText = avText.GetComponentInChildren<Text>();
            avTextText.text = "(0) emmVRC Favorites";


            currPageAvatar = pageAvatar.GetComponent<PageAvatar>();
            NewAvatarList = PublicAvatarList.GetComponent<UiAvatarList>();
            NewAvatarList.clearUnseenListOnCollapse = false;
            NewAvatarList.field_Public_Category_0 = UiAvatarList.Category.SpecificList;

            currPageAvatar.field_Public_SimpleAvatarPedestal_0.field_Public_Single_0 *= 0.85f;


            refreshButton = GameObject.Instantiate(ChangeButton, avText.transform.parent);
            refreshButton.GetComponentInChildren<Text>().text = "↻";
            refreshButton.GetComponent<Button>().onClick.RemoveAllListeners();
            refreshButton.GetComponent<Button>().onClick.AddListener(new System.Action(() =>
            {
                Searching = false;
                MelonLoader.MelonCoroutines.Start(JumpToStart());
                MelonLoader.MelonCoroutines.Start(RefreshMenu(0.5f));
            }));
            refreshButton.GetComponent<RectTransform>().sizeDelta /= new Vector2(4f, 1f);
            refreshButton.transform.SetParent(avText.transform, true);
            refreshButton.GetComponent<RectTransform>().anchoredPosition = avText.transform.Find("ToggleIcon").GetComponent<RectTransform>().anchoredPosition + new Vector2(980f, 0f);

            backButton = GameObject.Instantiate(ChangeButton, avText.transform.parent);
            backButton.GetComponentInChildren<Text>().text = "←";
            backButton.GetComponent<Button>().onClick.RemoveAllListeners();
            backButton.GetComponent<Button>().onClick.AddListener(new System.Action(() =>
            {
                currentPage--;
                MelonLoader.MelonCoroutines.Start(JumpToStart());
                MelonLoader.MelonCoroutines.Start(RefreshMenu(0.5f));
            }));
            backButton.GetComponent<RectTransform>().sizeDelta /= new Vector2(4f, 1f);
            backButton.transform.SetParent(avText.transform, true);
            backButton.GetComponent<RectTransform>().anchoredPosition = avText.transform.Find("ToggleIcon").GetComponent<RectTransform>().anchoredPosition + new Vector2(750f, 0f);

            forwardButton = GameObject.Instantiate(ChangeButton, avText.transform.parent);
            forwardButton.GetComponentInChildren<Text>().text = "→";
            forwardButton.GetComponent<Button>().onClick.RemoveAllListeners();
            forwardButton.GetComponent<Button>().onClick.AddListener(new System.Action(() =>
            {
                currentPage++;
                MelonLoader.MelonCoroutines.Start(JumpToStart());
                MelonLoader.MelonCoroutines.Start(RefreshMenu(0.5f));
            }));
            forwardButton.GetComponent<RectTransform>().sizeDelta /= new Vector2(4f, 1f);
            forwardButton.transform.SetParent(avText.transform, true);
            forwardButton.GetComponent<RectTransform>().anchoredPosition = avText.transform.Find("ToggleIcon").GetComponent<RectTransform>().anchoredPosition + new Vector2(900f, 0f);

            pageTicker = GameObject.Instantiate(ChangeButton, avText.transform.parent);
            pageTicker.GetComponentInChildren<Text>().text = "0 / 0";
            GameObject.Destroy(pageTicker.GetComponent<Button>());
            GameObject.Destroy(pageTicker.GetComponent<Image>());
            pageTicker.GetComponent<RectTransform>().sizeDelta /= new Vector2(4f, 1f);
            pageTicker.transform.SetParent(avText.transform, true);
            pageTicker.GetComponent<RectTransform>().anchoredPosition = avText.transform.Find("ToggleIcon").GetComponent<RectTransform>().anchoredPosition + new Vector2(825f, 0f);

            sortButton = GameObject.Instantiate(ChangeButton, avText.transform.parent);
            switch (currentSortingMode)
            {
                case SortingMode.DateAdded:
                    sortButton.GetComponentInChildren<Text>().text = "Date " + (sortingInverse ? "↑" : "↓");
                    break;
                case SortingMode.Alphabetical:
                    sortButton.GetComponentInChildren<Text>().text = "ABC " + (sortingInverse ? "↑" : "↓");
                    break;
                case SortingMode.Creator:
                    sortButton.GetComponentInChildren<Text>().text = "Creator " + (sortingInverse ? "↑" : "↓");
                    break;
            }
            sortButton.GetComponent<Button>().onClick.RemoveAllListeners();
            sortButton.GetComponent<Button>().onClick.AddListener(new System.Action(() =>
            {
                if (!sortingInverse)
                    sortingInverse = true;
                else
                {
                    if (currentSortingMode != SortingMode.Creator)
                        currentSortingMode++;
                    else
                        currentSortingMode = SortingMode.DateAdded;
                    sortingInverse = false;
                }
                switch (currentSortingMode)
                {
                    case SortingMode.DateAdded:
                        sortButton.GetComponentInChildren<Text>().text = "Date " + (sortingInverse ? "↑" : "↓");
                        break;
                    case SortingMode.Alphabetical:
                        sortButton.GetComponentInChildren<Text>().text = "ABC " + (sortingInverse ? "↑" : "↓");
                        break;
                    case SortingMode.Creator:
                        sortButton.GetComponentInChildren<Text>().text = "Creator " + (sortingInverse ? "↑" : "↓");
                        break;
                }
                currentPage = 0;
                Configuration.WriteConfigOption("SortingMode", (int)currentSortingMode);
                Configuration.WriteConfigOption("SortingInverse", sortingInverse);
                MelonLoader.MelonCoroutines.Start(JumpToStart());
                MelonLoader.MelonCoroutines.Start(RefreshMenu(0.5f));
            }));
            sortButton.GetComponent<RectTransform>().sizeDelta /= new Vector2(2f, 1f);
            sortButton.transform.SetParent(avText.transform, true);
            sortButton.GetComponent<RectTransform>().anchoredPosition = avText.transform.Find("ToggleIcon").GetComponent<RectTransform>().anchoredPosition + new Vector2(635f, 0f);

            pageAvatar.transform.Find("AvatarPreviewBase").transform.localPosition += new Vector3(0f, 60f, 0f);
            pageAvatar.transform.Find("AvatarPreviewBase").transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
            foreach (PropertyInfo inf in typeof(PageAvatar).GetProperties().Where(a => a.PropertyType == typeof(Vector3) && ((Vector3)a.GetValue(currPageAvatar)).x <= -461f && ((int)((Vector3)a.GetValue(currPageAvatar)).y) == -200))
            {
                Vector3 position = ((Vector3)inf.GetValue(currPageAvatar));
                inf.SetValue(currPageAvatar, new Vector3(position.x, position.y + 80f, position.z));
            }
            foreach (PropertyInfo inf in typeof(PageAvatar).GetProperties().Where(a => a.PropertyType == typeof(Vector3) && ((int)((Vector3)a.GetValue(currPageAvatar)).x) == -91))
            {
                Vector3 position = ((Vector3)inf.GetValue(currPageAvatar));
                inf.SetValue(currPageAvatar, new Vector3(position.x, position.y + 80f, position.z));
            }

            LoadedAvatars = new List<ApiAvatar>();
            SearchedAvatars = new List<ApiAvatar>();

            Components.EnableDisableListener pageAvatarListener = pageAvatar.AddComponent<Components.EnableDisableListener>();
            pageAvatarListener.OnEnabled += () =>
            {
                if ((!Configuration.JSONConfig.AvatarFavoritesEnabled || !Configuration.JSONConfig.emmVRCNetworkEnabled || !NetworkClient.HasJwtToken) && (PublicAvatarList.activeSelf || FavoriteButtonNew.activeSelf))
                {
                    PublicAvatarList.SetActive(false);
                    FavoriteButtonNew.SetActive(false);
                }
                else if ((!PublicAvatarList.activeSelf || !FavoriteButtonNew.activeSelf) && Configuration.JSONConfig.AvatarFavoritesEnabled && Configuration.JSONConfig.emmVRCNetworkEnabled && NetworkClient.HasJwtToken)
                {
                    PublicAvatarList.SetActive(true);
                    FavoriteButtonNew.SetActive(true);
                    MelonLoader.MelonCoroutines.Start(RefreshMenu(1f));
                }
                if (error && !errorWarned)
                {
                    Managers.emmVRCNotificationsManager.AddNotification(new Notification("emmVRC Network", Functions.Core.Resources.errorSprite, Core.Localization.currentLanguage.FavouriteListLoadErrorMessage, true, false, null, "", "", true, null, "Dismiss"));
                    errorWarned = true;
                }
                MelonLoader.MelonCoroutines.Start(WaitToEnableSearch());
            };
            VRCUiPageHeader pageheader = UnityEngine.Resources.FindObjectsOfTypeAll<VRCUiPageHeader>().FirstOrDefault();
            if (pageheader != null)
                searchBox = pageheader.field_Public_UiInputField_0;
            searchBoxAction = UnhollowerRuntimeLib.DelegateSupport.ConvertDelegate<UnityAction<string>>((System.Action<string>)((string searchTerm) =>
            {
                if (string.IsNullOrWhiteSpace(searchTerm) || searchTerm.Length < 2)
                    return;
                SearchAvatars(searchTerm).NoAwait(nameof(SearchAvatars));
            }));

            NetworkClient.onLogin += () =>
            {
                 PopulateList().NoAwait(nameof(CustomAvatarFavorites.PopulateList));
            };
            NetworkClient.onLogout += () =>
            {
                LoadedAvatars = new List<ApiAvatar>();
                SearchedAvatars = new List<ApiAvatar>();
            };

        }

        public static IEnumerator WaitToEnableSearch()
        {
            yield return new WaitForSeconds(0.1f);
            emmVRCLoader.Logger.LogDebug("Searchbox is " + (searchBox == null ? "null" : "not null"));
            emmVRCLoader.Logger.LogDebug("Searchbox button is " + (searchBox.field_Public_Button_0 == null ? "null" : "not null"));
            if (searchBox != null && searchBox.field_Public_Button_0 != null && !searchBox.field_Public_Button_0.interactable && NetworkClient.HasJwtToken && Configuration.JSONConfig.AvatarFavoritesEnabled)
            {
                searchBox.field_Public_Button_0.interactable = true;
                searchBox.field_Public_UnityAction_1_String_0 = searchBoxAction;
            }
        }

        public static async Task FavoriteAvatar(ApiAvatar apiAvatar)
        {
            var loadedCopy = LoadedAvatars.ToArray().ToList();

            if (loadedCopy.All(a => a.id != apiAvatar.id))
            {
                var (httpStatus, response) = await Request.AttemptRequest(HttpMethod.Post, "/avatar", new Avatar(apiAvatar));

                switch (httpStatus)
                {
                    case HttpStatusCode.OK:
                    {
                        LoadedAvatars.Insert(0, apiAvatar);
                        
                        if (!Searching)
                        {
                            currentPage = 0;
                            MelonCoroutines.Start(JumpToStart());
                            MelonCoroutines.Start(RefreshMenu(0.5f));
                        }
                        break;
                    }
                    case HttpStatusCode.InternalServerError:
                    {
                        await emmVRC.AwaitUpdate.Yield();
                        VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowStandardPopup("emmVRC", Core.Localization.currentLanguage.FavouriteListUpdateErrorMessage,
                            "Dismiss", () => { VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup(); });
                        break;
                    }
                    default:
                    {
                        try
                        {
                            var errorResponse = Decoder.Decode(response).Make<Error>();
                        
                            await emmVRC.AwaitUpdate.Yield();
                            VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowStandardPopup("emmVRC", errorResponse.error, 
                                "Dismiss", () => { VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup(); });
                        }
                        catch (Exception)
                        {
                            await emmVRC.AwaitUpdate.Yield();
                            VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowStandardPopup("emmVRC", Core.Localization.currentLanguage.FavouriteListUpdateErrorMessage, 
                                "Dismiss", () => { VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup(); });
                        }
                        break;
                    }
                }
            }
        }
        public static async Task UnfavoriteAvatar(ApiAvatar apiAvatar)
        {
            

            var (httpStatus, response) = await Request.AttemptRequest(HttpMethod.Delete, "/avatar", new Avatar(apiAvatar));

            switch (httpStatus)
            {
                case HttpStatusCode.OK:
                {
                    if (LoadedAvatars.Contains(apiAvatar))
                        LoadedAvatars.Remove(apiAvatar);
                    
                    if (!Searching)
                    {
                        currentPage = 0;
                        MelonCoroutines.Start(JumpToStart());
                        MelonCoroutines.Start(RefreshMenu(0.5f));
                    }
                    break;
                }
                case HttpStatusCode.InternalServerError:
                {
                    await emmVRC.AwaitUpdate.Yield();
                    VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowStandardPopup("emmVRC", Core.Localization.currentLanguage.FavouriteListUpdateErrorMessage,
                        "Dismiss", () => { VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup(); });
                    break;
                }
                default:
                {
                    try
                    {
                        var errorResponse = Decoder.Decode(response).Make<Error>();
                    
                        await emmVRC.AwaitUpdate.Yield();
                        VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowStandardPopup("emmVRC", errorResponse.error, 
                            "Dismiss", () => { VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup(); });
                    }
                    catch (Exception)
                    {
                        await emmVRC.AwaitUpdate.Yield();
                        VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowStandardPopup("emmVRC", Core.Localization.currentLanguage.FavouriteListUpdateErrorMessage, 
                            "Dismiss", () => { VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup(); });
                    }
                    break;
                }
            }
        }
        public static async Task PopulateList()
        {
            LoadedAvatars = new List<ApiAvatar>();
            
            var (httpStatus, response) = await Request.AttemptRequest(HttpMethod.Get, "/avatar");

            switch (httpStatus)
            {
                case HttpStatusCode.OK:
                {
                    var responseData = Decoder.Decode(response).Make<Avatar[]>();

                    await emmVRC.AwaitUpdate.Yield();
                    foreach (var avatar in responseData)
                    {
                        LoadedAvatars.Add(avatar.ToApiAvatar());
                    }

                    break;
                }
                default:
                {
                    await emmVRC.AwaitUpdate.Yield();
                    Managers.emmVRCNotificationsManager.AddNotification(new Notification("emmVRC Network", Core.Resources.errorSprite, 
                        Core.Localization.currentLanguage.FavouriteListLoadErrorMessage, true, 
                        false, null, "", "", true, null, "Dismiss"));
                    break;
                }
            }
        }
        public static IEnumerator RefreshMenu(float delay)
        {
            if (NewAvatarList.scrollRect != null)
            {
                yield return new WaitForSeconds(delay);

                if (Searching)
                {
                    if (currentPage > SearchedAvatars.Count / Configuration.JSONConfig.SearchRenderLimit)
                        currentPage = (int)SearchedAvatars.Count / Configuration.JSONConfig.SearchRenderLimit;
                    if (currentPage < 0)
                        currentPage = 0;

                    List<ApiAvatar> sortedSearchedAvatars = new List<ApiAvatar>();

                    switch (currentSortingMode)
                    {

                        case SortingMode.Alphabetical:
                            foreach (ApiAvatar avtr in SearchedAvatars.ToArray().OrderBy(x => x.name))
                                sortedSearchedAvatars.Add(avtr);
                            break;
                        case SortingMode.Creator:

                            foreach (ApiAvatar avtr in SearchedAvatars.ToArray().OrderBy(x => x.authorName))
                                sortedSearchedAvatars.Add(avtr);
                            break;
                        case SortingMode.DateAdded:
                            foreach (ApiAvatar avtr in SearchedAvatars)
                                sortedSearchedAvatars.Add(avtr);
                            break;
                    }

                    if (sortingInverse)
                        sortedSearchedAvatars.Reverse();

                    pageTicker.GetComponentInChildren<Text>().text = (currentPage + 1) + " / " + ((int)sortedSearchedAvatars.Count / Configuration.JSONConfig.SearchRenderLimit + 1);
                    List<ApiAvatar> avatarsToRender = sortedSearchedAvatars.GetRange(currentPage * Configuration.JSONConfig.SearchRenderLimit, System.Math.Abs(currentPage * Configuration.JSONConfig.SearchRenderLimit - sortedSearchedAvatars.Count));
                    if (avatarsToRender.Count > Configuration.JSONConfig.SearchRenderLimit)
                        avatarsToRender.RemoveRange(Configuration.JSONConfig.SearchRenderLimit, avatarsToRender.Count - Configuration.JSONConfig.SearchRenderLimit);
                    NewAvatarList.RenderElement(new List<ApiAvatar>());
                    NewAvatarList.RenderElement(avatarsToRender);
                    avText.GetComponentInChildren<Text>().text = "(" + sortedSearchedAvatars.Count + ") Search Results";
                    if (currentPage == 0)
                        backButton.GetComponent<Button>().interactable = false;
                    else
                        backButton.GetComponent<Button>().interactable = true;
                    if (currentPage >= sortedSearchedAvatars.Count / Configuration.JSONConfig.SearchRenderLimit)
                        forwardButton.GetComponent<Button>().interactable = false;
                    else
                        forwardButton.GetComponent<Button>().interactable = true;
                }
                else
                {
                    if (currentPage > LoadedAvatars.Count / Configuration.JSONConfig.FavoriteRenderLimit)
                        currentPage = (int)LoadedAvatars.Count / Configuration.JSONConfig.FavoriteRenderLimit;
                    if (currentPage < 0)
                        currentPage = 0;

                    List<ApiAvatar> sortedLoadedAvatars = new List<ApiAvatar>();

                    switch (currentSortingMode)
                    {

                        case SortingMode.Alphabetical:
                            foreach (ApiAvatar avtr in LoadedAvatars.ToArray().OrderBy(x => x.name))
                                sortedLoadedAvatars.Add(avtr);
                            break;
                        case SortingMode.Creator:
                            foreach (ApiAvatar avtr in LoadedAvatars.ToArray().OrderBy(x => x.authorName))
                                sortedLoadedAvatars.Add(avtr);
                            break;
                        case SortingMode.DateAdded:
                            foreach (ApiAvatar avtr in LoadedAvatars)
                                sortedLoadedAvatars.Add(avtr);
                            break;
                    }
                    if (sortingInverse)
                        sortedLoadedAvatars.Reverse();

                    pageTicker.GetComponentInChildren<Text>().text = (currentPage + 1) + " / " + ((int)sortedLoadedAvatars.Count / Configuration.JSONConfig.FavoriteRenderLimit + 1);
                    List<ApiAvatar> avatarsToRender = sortedLoadedAvatars.GetRange(currentPage * Configuration.JSONConfig.FavoriteRenderLimit, System.Math.Abs(currentPage * Configuration.JSONConfig.FavoriteRenderLimit - sortedLoadedAvatars.Count));
                    if (avatarsToRender.Count > Configuration.JSONConfig.FavoriteRenderLimit)
                        avatarsToRender.RemoveRange(Configuration.JSONConfig.FavoriteRenderLimit, avatarsToRender.Count - Configuration.JSONConfig.FavoriteRenderLimit);
                    NewAvatarList.RenderElement(new List<ApiAvatar>());
                    NewAvatarList.RenderElement(avatarsToRender);
                    avText.GetComponentInChildren<Text>().text = "(" + sortedLoadedAvatars.Count + ") emmVRC Favorites";
                    if (currentPage == 0)
                        backButton.GetComponent<Button>().interactable = false;
                    else
                        backButton.GetComponent<Button>().interactable = true;
                    if (currentPage >= sortedLoadedAvatars.Count / Configuration.JSONConfig.FavoriteRenderLimit)
                        forwardButton.GetComponent<Button>().interactable = false;
                    else
                        forwardButton.GetComponent<Button>().interactable = true;
                }
            }
        }
        public static IEnumerator JumpToStart()
        {
            if (Configuration.JSONConfig.AvatarFavoritesJumpToStart)
            {
                while (NewAvatarList.scrollRect.normalizedPosition.x > 0)
                {
                    NewAvatarList.scrollRect.normalizedPosition = new Vector2(NewAvatarList.scrollRect.normalizedPosition.x - 0.1f, 0);
                    yield return new WaitForEndOfFrame();
                }
            }
        }
        public static IEnumerator SearchAvatarsAfterDelay(string query)
        {
            yield return new WaitForSecondsRealtime(1f);
            if (Configuration.JSONConfig.AvatarFavoritesJumpToStart)
            {
                while (NewAvatarList.scrollRect.normalizedPosition.x > 0)
                {
                    NewAvatarList.scrollRect.normalizedPosition = new Vector2(NewAvatarList.scrollRect.normalizedPosition.x - 0.1f, 0);
                    yield return new WaitForEndOfFrame();
                }
            }
            SearchAvatars(query).NoAwait(nameof(SearchAvatars));
        }
        public static async Task SearchAvatars(string query)
        {
            if (!Configuration.JSONConfig.emmVRCNetworkEnabled)
                return;

            if (!Configuration.JSONConfig.AvatarFavoritesEnabled)
                return;

            if (!NetworkClient.HasJwtToken)
                return;

            if (waitingForSearch)
            {
                await emmVRC.AwaitUpdate.Yield();
                VRCUiPopupManager.prop_VRCUiPopupManager_0.ShowStandardPopup("emmVRC",
                    Core.Localization.currentLanguage.SearchRateLimitErrorMessage, 
                    "Okay", () => { VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup(); });
            }

            await emmVRC.AwaitUpdate.Yield();
            avText.GetComponentInChildren<Text>().text = "Searching. Please wait...";
            SearchedAvatars = new List<ApiAvatar>();

            waitingForSearch = true;

            var (httpStatus, result) =
                await Request.AttemptRequest(HttpMethod.Get, $"/avatar/search?q={WebUtility.UrlEncode(query)}");

            waitingForSearch = false;

            switch (httpStatus)
            {
                case HttpStatusCode.OK:
                {
                    var searchResponse = Decoder.Decode(result).Make<Avatar[]>();
                    
                    await emmVRC.AwaitUpdate.Yield();
                    foreach (var avatar in searchResponse)
                    {
                        SearchedAvatars.Add(avatar.ToApiAvatar());
                    }

                    currentPage = 0;
                    Searching = true;
                    break;
                }
                case HttpStatusCode.InternalServerError:
                {
                    await emmVRC.AwaitUpdate.Yield();
                    VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowStandardPopup("emmVRC", Core.Localization.currentLanguage.SearchFailedErrorMessage, 
                        "Dismiss", () => { VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup(); });
                    break;
                }
                default:
                {
                    try
                    {
                        var errorResponse = Decoder.Decode(result).Make<Error>();
                        
                        await emmVRC.AwaitUpdate.Yield();
                        VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowStandardPopup("emmVRC", errorResponse.error, 
                            "Dismiss", () => { VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup(); });
                    }
                    catch (Exception)
                    {
                        await emmVRC.AwaitUpdate.Yield();
                        VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowStandardPopup("emmVRC", Core.Localization.currentLanguage.SearchFailedErrorMessage, 
                        "Dismiss", () => { VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup(); });
                    }
                    
                    break;
                }
            }
            
            await emmVRC.AwaitUpdate.Yield();
            MelonCoroutines.Start(RefreshMenu(0.1f));
        }
        
        public void OnUpdate()
        {

            if (PublicAvatarList == null || !PublicAvatarList.activeInHierarchy) return;

            if (Configuration.JSONConfig.AvatarFavoritesEnabled && Configuration.JSONConfig.emmVRCNetworkEnabled && NetworkClient.HasJwtToken)
            {
                NewAvatarList.collapsedCount = Configuration.JSONConfig.FavoriteRenderLimit + Configuration.JSONConfig.SearchRenderLimit;
                NewAvatarList.expandedCount = Configuration.JSONConfig.FavoriteRenderLimit + Configuration.JSONConfig.SearchRenderLimit;

                if (currPageAvatar != null && currPageAvatar.field_Public_SimpleAvatarPedestal_0 != null && currPageAvatar.field_Public_SimpleAvatarPedestal_0.field_Internal_ApiAvatar_0 != null && LoadedAvatars != null && FavoriteButtonNew != null)
                {
                    FavoriteButtonNewText.text = "<color=#FF69B4>emmVRC</color> Favorite";
                    foreach (ApiAvatar avatar in LoadedAvatars)
                    {
                        if (avatar.id == currPageAvatar.field_Public_SimpleAvatarPedestal_0.field_Internal_ApiAvatar_0.id)
                        {
                            FavoriteButtonNewText.text = "<color=#FF69B4>emmVRC</color> Unfavorite";
                            break;
                        }
                    }
                }
            }

        }

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            if (buildIndex != -1) return;
            CheckForAvatarPedestals().NoAwait();
        }

        public static async Task CheckForAvatarPedestals()
        {
            if (!NetworkClient.networkConfiguration.AvatarPedestalScansAllowed)
                return;
            
            if (!Configuration.JSONConfig.emmVRCNetworkEnabled)
                return;
            
            if (!NetworkClient.HasJwtToken)
                return;

            if (!Configuration.JSONConfig.SubmitAvatarPedestals)
                return;
            
            while (RoomManager.field_Internal_Static_ApiWorld_0 == null)
                await emmVRC.AwaitUpdate.Yield();
            
            var currentWorld = RoomManager.field_Internal_Static_ApiWorld_0;
            if (!currentWorld.IsPublicPublishedWorld)
                return;

            foreach (var pedestal in Resources.FindObjectsOfTypeAll<AvatarPedestal>())
            {
                if (pedestal == null)
                    continue;
                
                await emmVRC.AwaitUpdate.Yield();
                var apiAvatar = pedestal.field_Private_ApiAvatar_0;
                if (apiAvatar == null)
                    continue;

                if (apiAvatar.releaseStatus != "public")
                    continue;

                if (string.IsNullOrEmpty(apiAvatar.assetUrl))
                    continue;
                
                Logger.LogDebug($"Found pedestal {apiAvatar.name}, putting...");
                
                var (httpStatus, result) = await Request.AttemptRequest(HttpMethod.Put, $"/avatar", new Avatar(apiAvatar));

                switch (httpStatus)
                {
                    case HttpStatusCode.OK:
                        break;
                    default:
                        Logger.LogDebug("Failed to submit avatar.");
                        break;
                }

                await Task.Delay(500);
            }
        }
        
        public static async Task ExportAvatars()
        {
            if (!NetworkClient.HasJwtToken)
                return;
            
            try
            {
                var (httpStatus, response) = await Request.AttemptRequest(HttpMethod.Get, "/avatar/export");

                if (httpStatus == HttpStatusCode.OK)
                {
                    var decodedExport = Decoder.Decode(response).Make<ExportedAvatar[]>();
                    File.WriteAllText(Path.Combine(Environment.CurrentDirectory, "UserData/emmVRC/ExportedList.json"),
                        Encoder.Encode(decodedExport, EncodeOptions.PrettyPrint | EncodeOptions.NoTypeHints));
                }
            }
            catch (Exception ex)
            {
                Logger.LogError($"There was an issue exporting your avatars.\n{ex}");
            }
        }
    }
}
