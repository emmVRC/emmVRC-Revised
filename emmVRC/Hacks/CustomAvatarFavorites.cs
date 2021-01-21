﻿using Il2CppSystem.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using VRC.Core;
using VRC.UI;
using emmVRC.Libraries;
using emmVRC.Network;
using System.Linq;
using System.IO;
using emmVRC.Objects;
using System.Reflection;
using System.Collections;

namespace emmVRC.Hacks
{
    public static class CustomAvatarFavorites
    {

        internal static GameObject PublicAvatarList;
        internal static UiAvatarList NewAvatarList;
        private static GameObject avText;
        private static Text avTextText;
        private static GameObject ChangeButton;
        public static Button.ButtonClickedEvent baseChooseEvent;
        private static GameObject FavoriteButton;
        private static GameObject FavoriteButtonNew;
        public static GameObject MigrateButton;
        private static Button FavoriteButtonNewButton;
        private static Text FavoriteButtonNewText;
        public static GameObject pageAvatar;
        private static PageAvatar currPageAvatar;
        private static bool error = false;
        private static bool errorWarned;
        private static bool Searching = false;
        public static List<ApiAvatar> LoadedAvatars;
        private static List<ApiAvatar> SearchedAvatars;
        private static bool menuJustActivated = false;
        private static UiInputField searchBox;
        private static UnityAction<string> searchBoxAction;
        private static GameObject refreshButton;
        private static GameObject backButton;
        private static GameObject forwardButton;
        private static GameObject pageTicker;
        private static bool waitingForSearch = false;
        public static int currentPage = 0;

        private static MethodInfo renderElementMethod;
        internal static void RenderElement(this UiVRCList uivrclist, List<ApiAvatar> AvatarList)
        {
            if (!uivrclist.gameObject.activeInHierarchy || !uivrclist.isActiveAndEnabled || uivrclist.isOffScreen || !uivrclist.enabled) return;
            if (renderElementMethod == null)
            {
                renderElementMethod = typeof(UiVRCList).GetMethods().FirstOrDefault(a => a.Name.Contains("Method_Protected_Void_List_1_T_Int32_Boolean")).MakeGenericMethod(typeof(ApiAvatar));
            }
            renderElementMethod.Invoke(uivrclist, new object[] { AvatarList, 0, true, null });
        }
        internal static void Initialize()
        {
            pageAvatar = Libraries.QuickMenuUtils.GetVRCUiMInstance().menuContent.transform.Find("Screens/Avatar").gameObject;
            FavoriteButton = Libraries.QuickMenuUtils.GetVRCUiMInstance().menuContent.transform.Find("Screens/Avatar/Favorite Button").gameObject;
            FavoriteButtonNew = UnityEngine.Object.Instantiate<GameObject>(FavoriteButton, Libraries.QuickMenuUtils.GetVRCUiMInstance().menuContent.transform.Find("Screens/Avatar/"));
            FavoriteButtonNewButton = FavoriteButtonNew.GetComponent<Button>();
            FavoriteButtonNewButton.onClick.RemoveAllListeners();
            FavoriteButtonNewButton.onClick.AddListener(new System.Action(() =>
            {

                ApiAvatar apiAvatar = pageAvatar.GetComponent<PageAvatar>().avatar.field_Internal_ApiAvatar_0;
                bool flag = false;
                for (int i = 0; i < LoadedAvatars.Count; i++)
                {
                    if (LoadedAvatars[i].id == apiAvatar.id)
                        flag = true;
                }
                if (!flag)
                {
                    if (((apiAvatar.releaseStatus == "public" || apiAvatar.authorId == APIUser.CurrentUser.id) && apiAvatar.releaseStatus != null))
                    {
                        MelonLoader.MelonCoroutines.Start(FavoriteAvatar(apiAvatar));
                    }
                    else
                    {
                        VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowStandardPopup("emmVRC", "Cannot favorite this avatar (it is private!)", "Dismiss", new System.Action(() => { VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup(); }));
                    }
                }
                else
                {
                    VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowStandardPopup("emmVRC", "Are you sure you want to unfavorite the avatar \"" + apiAvatar.name + "\"?", "Yes", new System.Action(() =>
                    {
                        MelonLoader.MelonCoroutines.Start(UnfavoriteAvatar(apiAvatar));
                        VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup();
                    }), "No", new System.Action(() =>
                    {
                        VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup();
                    }));
                }
            }));

            FavoriteButtonNew.GetComponentInChildren<RectTransform>().localPosition += new Vector3(0, 165f);
            FavoriteButtonNewText = FavoriteButtonNew.GetComponentInChildren<Text>();
            FavoriteButtonNewText.supportRichText = true;
            try
                {
                    FavoriteButtonNew.transform.Find("Horizontal/FavoritesCountSpacingText").gameObject.SetActive(false);
                    FavoriteButtonNew.transform.Find("Horizontal/FavoritesCurrentCountText").gameObject.SetActive(false);
                    FavoriteButtonNew.transform.Find("Horizontal/FavoritesCountDividerText").gameObject.SetActive(false);
                    FavoriteButtonNew.transform.Find("Horizontal/FavoritesMaxAvailableText").gameObject.SetActive(false);
                } catch (System.Exception ex)
                {
                    emmVRCLoader.Logger.LogError("GameObject toggling failed. VRChat must have moved something in an update. Sorry!");
                }

            MigrateButton = UnityEngine.Object.Instantiate<GameObject>(FavoriteButton, Libraries.QuickMenuUtils.GetVRCUiMInstance().menuContent.transform.Find("Screens/Avatar/"));
            MigrateButton.GetComponentInChildren<RectTransform>().localPosition += new Vector3(0f, 765f);
            MigrateButton.GetComponentInChildren<Text>().text = "Migrate";
            MigrateButton.GetComponentInChildren<Button>().onClick = new Button.ButtonClickedEvent();
            MigrateButton.GetComponentInChildren<Button>().onClick.AddListener(new System.Action(() => {
                VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowStandardPopup("emmVRC", "Do you want to migrate your AviFav+ avatars to emmVRC?", "Yes", () => {
                    System.Collections.Generic.List<AviFavAvatar> aviFavAvatars = TinyJSON.Decoder.Decode(File.ReadAllText(Path.Combine(System.Environment.CurrentDirectory, "404Mods/AviFavorites/avatars.json"))).Make<System.Collections.Generic.List<AviFavAvatar>>();
                    System.Collections.Generic.List<string> ids = new System.Collections.Generic.List<string>();
                    foreach (AviFavAvatar avtr in aviFavAvatars)
                    {
                        ids.Add(avtr.AvatarID);
                    }
                    if (ids.Count > 0)
                    {
                        VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowStandardPopup("emmVRC", "Your avatars are being migrated in the background. This may take a few minutes. Please do not close VRChat.", "Dismiss", () => { VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup(); });
                        MigrateButton.GetComponentInChildren<Button>().enabled = false;
                        MigrateButton.GetComponentInChildren<Text>().text = "Migrating...";

                        MelonLoader.MelonCoroutines.Start(AvatarUtilities.fetchAvatars(ids, (System.Collections.Generic.List<ApiAvatar> avatars, bool errored) =>
                        {
                            MelonLoader.MelonCoroutines.Start(AvatarUtilities.FavoriteAvatars(avatars, errored));
                        }));
                    }
                }, "No", () => {
                    VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup();
                });
            }));
            if (File.Exists(Path.Combine(System.Environment.CurrentDirectory, "404Mods/AviFavorites/avatars.json")))
            {
                MigrateButton.SetActive(true);
                MigrateButton.GetComponentInChildren<Button>().enabled = true;
            }
            else
            {
                MigrateButton.SetActive(false);
            }

            GameObject oldPublicAvatarList;
            oldPublicAvatarList = Libraries.QuickMenuUtils.GetVRCUiMInstance().menuContent.transform.Find("Screens/Avatar/Vertical Scroll View/Viewport/Content/Legacy Avatar List").gameObject;
            PublicAvatarList = GameObject.Instantiate(oldPublicAvatarList, oldPublicAvatarList.transform.parent);
            PublicAvatarList.transform.SetAsFirstSibling();

            ChangeButton = Libraries.QuickMenuUtils.GetVRCUiMInstance().menuContent.transform.Find("Screens/Avatar/Change Button").gameObject;
            baseChooseEvent = ChangeButton.GetComponent<Button>().onClick;
            ChangeButton.GetComponent<Button>().onClick = new Button.ButtonClickedEvent();
            ChangeButton.GetComponent<Button>().onClick.AddListener(new System.Action(() =>
            {
                ApiAvatar selectedAvatar = pageAvatar.GetComponent<PageAvatar>().avatar.field_Internal_ApiAvatar_0;
                if (NetworkConfig.Instance.APICallsAllowed && !selectedAvatar.id.Contains("local"))
                {
                    API.Fetch<ApiAvatar>(selectedAvatar.id, new System.Action<ApiContainer>((ApiContainer cont) => {
                        ApiAvatar fetchedAvatar = cont.Model.Cast<ApiAvatar>();
                        if (fetchedAvatar.releaseStatus == "private" && fetchedAvatar.authorId != APIUser.CurrentUser.id && fetchedAvatar.authorName != "tafi_licensed")
                            VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowStandardPopup("emmVRC", "Cannot switch into this avatar (it is private).\nDo you want to unfavorite it?", "Yes", new System.Action(() => { MelonLoader.MelonCoroutines.Start(UnfavoriteAvatar(selectedAvatar)); VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup(); }), "No", new System.Action(() => { VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup(); }));
                        else
                            baseChooseEvent.Invoke();
                    }), new System.Action<ApiContainer>((ApiContainer cont) => {
                        VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowStandardPopup("emmVRC", "Cannot switch into this avatar (no longer available).\nDo you want to unfavorite it?", "Yes", new System.Action(() => { MelonLoader.MelonCoroutines.Start(UnfavoriteAvatar(selectedAvatar)); VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup(); }), "No", new System.Action(() => { VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup(); }));
                    }));
                }
                else
                {
                    //emmVRCLoader.Bootstrapper.Instance.StartCoroutine(CheckAvatar());
                    if (selectedAvatar.releaseStatus == "private" && selectedAvatar.authorId != APIUser.CurrentUser.id && selectedAvatar.authorName != "tafi_licensed")
                    {
                        VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowStandardPopup("emmVRC", "Cannot switch into this avatar (it is private).\nDo you want to unfavorite it?", "Yes", new System.Action(() => { MelonLoader.MelonCoroutines.Start(UnfavoriteAvatar(selectedAvatar)); VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup(); }), "No", new System.Action(() => { VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup(); }));
                    }
                    else if (selectedAvatar.releaseStatus == "unavailable")
                    {
                        VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowStandardPopup("emmVRC", "Cannot switch into this avatar (no longer available).\nDo you want to unfavorite it?", "Yes", new System.Action(() => { MelonLoader.MelonCoroutines.Start(UnfavoriteAvatar(selectedAvatar)); VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup(); }), "No", new System.Action(() => { VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup(); }));
                    }
                    else
                        baseChooseEvent.Invoke();
                }
            }));

            avText = PublicAvatarList.transform.Find("Button").gameObject;
            avTextText = avText.GetComponentInChildren<Text>();
            avTextText.text = "(0) emmVRC Favorites";


            currPageAvatar = pageAvatar.GetComponent<PageAvatar>();
            NewAvatarList = PublicAvatarList.GetComponent<UiAvatarList>();
            NewAvatarList.clearUnseenListOnCollapse = false;
            NewAvatarList.category = UiAvatarList.EnumNPublicSealedvaInPuMiFaSpClPuLi9vUnique.SpecificList;

            currPageAvatar.avatar.avatarScale *= 0.85f;


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
            refreshButton.GetComponent<RectTransform>().anchoredPosition = avText.transform.Find("ToggleIcon").GetComponent<RectTransform>().anchoredPosition + new Vector2(975f, 0f);

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
            backButton.GetComponent<RectTransform>().anchoredPosition = avText.transform.Find("ToggleIcon").GetComponent<RectTransform>().anchoredPosition + new Vector2(725f, 0f);

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
            forwardButton.GetComponent<RectTransform>().anchoredPosition = avText.transform.Find("ToggleIcon").GetComponent<RectTransform>().anchoredPosition + new Vector2(875f, 0f);

            pageTicker = GameObject.Instantiate(ChangeButton, avText.transform.parent);
            pageTicker.GetComponentInChildren<Text>().text = "0 / 0";
            GameObject.Destroy(pageTicker.GetComponent<Button>());
            GameObject.Destroy(pageTicker.GetComponent<Image>());
            pageTicker.GetComponent<RectTransform>().sizeDelta /= new Vector2(4f, 1f);
            pageTicker.transform.SetParent(avText.transform, true);
            pageTicker.GetComponent<RectTransform>().anchoredPosition = avText.transform.Find("ToggleIcon").GetComponent<RectTransform>().anchoredPosition + new Vector2(800f, 0f);

            pageAvatar.transform.Find("AvatarModel").transform.localPosition += new Vector3(0f, 60f, 0f);

            LoadedAvatars = new List<ApiAvatar>();

            SearchedAvatars = new List<ApiAvatar>();

        }
        public static System.Collections.IEnumerator FavoriteAvatar(ApiAvatar avtr)
        {
            if (LoadedAvatars.ToArray().ToList().FindIndex(a => a.id == avtr.id) == -1)
            {
                LoadedAvatars.Insert(0, avtr);
                Network.Objects.Avatar serAvtr = new Network.Objects.Avatar(avtr);

                var request = HTTPRequest.post(NetworkClient.baseURL + "/api/avatar", serAvtr);
                while (!request.IsCompleted && !request.IsFaulted)
                    yield return new WaitForEndOfFrame();
                if (!request.IsFaulted)
                {
                    if (!Searching)
                    {
                        currentPage = 0;
                        MelonLoader.MelonCoroutines.Start(JumpToStart());
                        MelonLoader.MelonCoroutines.Start(RefreshMenu(0.1f));
                    }
                }
                else
                {
                    emmVRCLoader.Logger.LogError("Asynchronous net post failed: " + request.Exception);
                    VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowStandardPopup("emmVRC", "Error occured while updating avatar list.", "Dismiss", new System.Action(() => { VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup(); }));
                }
            }
            else
            {
                emmVRCLoader.Logger.LogDebug("Tried to add an avatar that already exists...");
            }
        }
        public static System.Collections.IEnumerator UnfavoriteAvatar(ApiAvatar avtr)
        {
            if (LoadedAvatars.Contains(avtr))
                LoadedAvatars.Remove(avtr);

            var request = HTTPRequest.delete(NetworkClient.baseURL + "/api/avatar", new Network.Objects.Avatar(avtr));
            while (!request.IsCompleted && !request.IsFaulted)
                yield return new WaitForEndOfFrame();
            if (!request.IsFaulted)
            {
                if (!Searching)
                {
                    MelonLoader.MelonCoroutines.Start(JumpToStart());
                    MelonLoader.MelonCoroutines.Start(RefreshMenu(0.1f));
                }
            }
            else
            {
                emmVRCLoader.Logger.LogError("Asynchronous net delete failed: " + request.Exception);
                VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowStandardPopup("emmVRC", "Error occured while updating avatar list.", "Dismiss", new System.Action(() => { VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup(); }));
            }
        }
        public static void AddEmptyFavorite()
        {
            ApiAvatar avtr = new ApiAvatar
            {
                releaseStatus = "unavailable",
                name = "Avatar not available",
                id = "null",
                assetUrl = "",
                thumbnailImageUrl = "http://img.thetrueyoshifan.com/AvatarUnavailable.png",
            };
            LoadedAvatars.Insert(0, avtr);
            MelonLoader.MelonCoroutines.Start(RefreshMenu(0.125f));
        }
        public static System.Collections.IEnumerator PopulateList()
        {
            LoadedAvatars = new List<ApiAvatar>();
            Network.Objects.Avatar[] avatarArray = null;
            var request = HTTPRequest.get(NetworkClient.baseURL + "/api/avatar");
            while (!request.IsCompleted && !request.IsFaulted)
                yield return new WaitForEndOfFrame();
            if (!request.IsFaulted)
            {
                avatarArray = TinyJSON.Decoder.Decode(request.Result).Make<Network.Objects.Avatar[]>();
                if (avatarArray != null)
                {
                    foreach (Network.Objects.Avatar avtr in avatarArray)
                    {
                        LoadedAvatars.Add(avtr.apiAvatar());
                    }
                    MelonLoader.MelonCoroutines.Start(RefreshMenu(0.1f));
                }
            }
            else
            {
                emmVRCLoader.Logger.LogError("Asynchronous net get failed: " + request.Exception);
                Managers.NotificationManager.AddNotification("emmVRC Avatar Favorites list failed to load. Please check your internet connection.", "Dismiss", Managers.NotificationManager.DismissCurrentNotification, "", null, Resources.errorSprite, -1);
                error = true;
                errorWarned = true;
            }

        }
        public static System.Collections.IEnumerator RefreshMenu(float delay)
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
                    pageTicker.GetComponentInChildren<Text>().text = (currentPage + 1) + " / " + ((int)SearchedAvatars.Count / Configuration.JSONConfig.SearchRenderLimit + 1);
                    List<ApiAvatar> avatarsToRender = SearchedAvatars.GetRange(currentPage * Configuration.JSONConfig.SearchRenderLimit, System.Math.Abs(currentPage * Configuration.JSONConfig.SearchRenderLimit - SearchedAvatars.Count));
                    if (avatarsToRender.Count > Configuration.JSONConfig.SearchRenderLimit) 
                        avatarsToRender.RemoveRange(Configuration.JSONConfig.SearchRenderLimit,  avatarsToRender.Count - Configuration.JSONConfig.SearchRenderLimit);
                    NewAvatarList.RenderElement(new List<ApiAvatar>());
                    NewAvatarList.RenderElement(avatarsToRender);
                    avText.GetComponentInChildren<Text>().text = "(" + SearchedAvatars.Count + ") Search Results";
                    if (currentPage == 0)
                        backButton.GetComponent<Button>().interactable = false;
                    else
                        backButton.GetComponent<Button>().interactable = true;
                    if (currentPage >= SearchedAvatars.Count / Configuration.JSONConfig.SearchRenderLimit)
                        forwardButton.GetComponent<Button>().interactable = false;
                    else
                        forwardButton.GetComponent<Button>().interactable = true;
                } else
                {
                    if (currentPage > LoadedAvatars.Count / Configuration.JSONConfig.FavoriteRenderLimit)
                        currentPage = (int)LoadedAvatars.Count / Configuration.JSONConfig.FavoriteRenderLimit;
                    if (currentPage < 0)
                        currentPage = 0;
                    pageTicker.GetComponentInChildren<Text>().text = (currentPage + 1) + " / " + ((int)LoadedAvatars.Count / Configuration.JSONConfig.FavoriteRenderLimit + 1);
                    List<ApiAvatar> avatarsToRender = LoadedAvatars.GetRange(currentPage * Configuration.JSONConfig.FavoriteRenderLimit, System.Math.Abs(currentPage * Configuration.JSONConfig.FavoriteRenderLimit - LoadedAvatars.Count));
                    if (avatarsToRender.Count > Configuration.JSONConfig.FavoriteRenderLimit)
                        avatarsToRender.RemoveRange(Configuration.JSONConfig.FavoriteRenderLimit, avatarsToRender.Count -  Configuration.JSONConfig.FavoriteRenderLimit);
                    NewAvatarList.RenderElement(new List<ApiAvatar>());
                    NewAvatarList.RenderElement(avatarsToRender);
                    avText.GetComponentInChildren<Text>().text = "(" + LoadedAvatars.Count + ") emmVRC Favorites";
                    if (currentPage == 0)
                        backButton.GetComponent<Button>().interactable = false;
                    else
                        backButton.GetComponent<Button>().interactable = true;
                    if (currentPage >= LoadedAvatars.Count / Configuration.JSONConfig.FavoriteRenderLimit)
                        forwardButton.GetComponent<Button>().interactable = false;
                    else
                        forwardButton.GetComponent<Button>().interactable = true;
                }
            }
        }
        public static System.Collections.IEnumerator JumpToStart()
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
        public static System.Collections.IEnumerator SearchAvatarsAfterDelay(string query)
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
            MelonLoader.MelonCoroutines.Start(SearchAvatars(query));
        }
        public static System.Collections.IEnumerator SearchAvatars(string query)
        {
            if (!Configuration.JSONConfig.AvatarFavoritesEnabled || !Configuration.JSONConfig.emmVRCNetworkEnabled || NetworkClient.webToken == null)
            {
                yield return new WaitForEndOfFrame();
            }
            if (waitingForSearch)
                VRCUiPopupManager.prop_VRCUiPopupManager_0.ShowStandardPopup("emmVRC", "Please wait for your current search\nto finish before starting a new one.", "Okay", () => { VRCUiPopupManager.prop_VRCUiPopupManager_0.HideCurrentPopup(); });
            else
            {
                avText.GetComponentInChildren<Text>().text = "Searching. Please wait...";
                SearchedAvatars.Clear();
                Network.Objects.Avatar[] avatarArray = null;

                waitingForSearch = true;
                var request = HTTPRequest.post(NetworkClient.baseURL + "/api/avatar/search", new System.Collections.Generic.Dictionary<string, string> { ["query"] = query });
                while (!request.IsCompleted && !request.IsFaulted)
                    yield return new WaitForEndOfFrame();
                waitingForSearch = false;
                if (!request.IsFaulted && !request.Result.Contains("Bad Request"))
                {
                    avatarArray = TinyJSON.Decoder.Decode(request.Result).Make<Network.Objects.Avatar[]>();
                    if (avatarArray != null)
                    {
                        foreach (Network.Objects.Avatar avatar in avatarArray)
                        {
                            SearchedAvatars.Add(avatar.apiAvatar());
                        }
                    }
                    currentPage = 0;
                    Searching = true;
                    MelonLoader.MelonCoroutines.Start(RefreshMenu(0.1f));
                }
                else
                {
                    emmVRCLoader.Logger.LogError("Asynchronous net post failed: " + request.Exception);
                    VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowStandardPopup("emmVRC", "Your search could not be processed.", "Dismiss", new System.Action(() => { VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup(); }));
                }
                if (NewAvatarList.expandButton.gameObject.transform.Find("ToggleIcon").GetComponentInChildren<Image>().sprite == NewAvatarList.expandSprite)
                    NewAvatarList.ToggleExtend();
            }
        }
        internal static void OnUpdate()
        {
            if (PublicAvatarList == null || FavoriteButtonNew == null || RoomManager.field_Internal_Static_ApiWorld_0 == null) return;
            if (searchBox == null && NewAvatarList.gameObject.activeInHierarchy)
            {
                VRCUiPageHeader pageheader = QuickMenuUtils.GetVRCUiMInstance().GetComponentInChildren<VRCUiPageHeader>(true);
                if (pageheader != null)
                {
                    searchBox = pageheader.searchBar;
                }
            }
            if (searchBoxAction == null)
            {
                searchBoxAction = UnhollowerRuntimeLib.DelegateSupport.ConvertDelegate<UnityAction<string>>((System.Action<string>)((string searchTerm) =>
                {
                    if (string.IsNullOrWhiteSpace(searchTerm) || searchTerm.Length < 2)
                        return;
                    MelonLoader.MelonCoroutines.Start(SearchAvatars(searchTerm));
                }));
            }
            if (searchBox != null && searchBox.editButton != null && !searchBox.editButton.interactable && PublicAvatarList.activeInHierarchy && Configuration.JSONConfig.AvatarFavoritesEnabled && Configuration.JSONConfig.emmVRCNetworkEnabled && NetworkClient.webToken != null && RoomManager.field_Internal_Static_ApiWorld_0 != null)
            {
                searchBox.editButton.interactable = true;
                searchBox.onDoneInputting = searchBoxAction;
            }


            if (PublicAvatarList.activeSelf && Configuration.JSONConfig.AvatarFavoritesEnabled && Configuration.JSONConfig.emmVRCNetworkEnabled && NetworkClient.webToken != null)
            {
                NewAvatarList.collapsedCount = Configuration.JSONConfig.FavoriteRenderLimit + Configuration.JSONConfig.SearchRenderLimit;
                NewAvatarList.expandedCount = Configuration.JSONConfig.FavoriteRenderLimit + Configuration.JSONConfig.SearchRenderLimit;

                if (!menuJustActivated)
                {
                    MelonLoader.MelonCoroutines.Start(RefreshMenu(1f));
                    menuJustActivated = true;
                }
                if (menuJustActivated && (NewAvatarList.pickers.Count < LoadedAvatars.Count || NewAvatarList.isOffScreen))
                    menuJustActivated = false;
                if (currPageAvatar != null && currPageAvatar.avatar != null && currPageAvatar.avatar.field_Internal_ApiAvatar_0 != null && LoadedAvatars != null && FavoriteButtonNew != null)
                {
                    bool flag = false;
                    for (int i = 0; i < LoadedAvatars.Count; i++)
                    {
                        if (LoadedAvatars[i].id == currPageAvatar.avatar.field_Internal_ApiAvatar_0.id)
                        {
                            flag = true;
                        }
                    }

                    if (!flag)
                    {
                        FavoriteButtonNewText.text = "<color=#FF69B4>emmVRC</color> Favorite";
                    }
                    else
                    {
                        FavoriteButtonNewText.text = "<color=#FF69B4>emmVRC</color> Unfavorite";
                    }
                }
            }
            if ((!Configuration.JSONConfig.AvatarFavoritesEnabled || !Configuration.JSONConfig.emmVRCNetworkEnabled || NetworkClient.webToken == null) && (PublicAvatarList.activeSelf || FavoriteButtonNew.activeSelf))
            {
                PublicAvatarList.SetActive(false);
                FavoriteButtonNew.SetActive(false);
            }
            else if ((!PublicAvatarList.activeSelf || !FavoriteButtonNew.activeSelf) && Configuration.JSONConfig.AvatarFavoritesEnabled && Configuration.JSONConfig.emmVRCNetworkEnabled && NetworkClient.webToken != null)
            {
                PublicAvatarList.SetActive(true);
                FavoriteButtonNew.SetActive(true);
            }
            if (error && !errorWarned)
            {
                Managers.NotificationManager.AddNotification("Your emmVRC avatars could not be loaded. Please contact yoshifan#9550 to resolve this.", "Dismiss", () => { Managers.NotificationManager.DismissCurrentNotification(); }, "", null, Resources.errorSprite, -1);
                errorWarned = true;
            }
        }

        internal static void Hide()
        {
            PublicAvatarList.SetActive(false);
            FavoriteButtonNew.SetActive(false);
        }
        internal static void Show()
        {
            if (!error && Configuration.JSONConfig.AvatarFavoritesEnabled)
            {
                PublicAvatarList.SetActive(true);
                FavoriteButtonNew.SetActive(true);
            }
        }
        internal static void Destroy()
        {
            GameObject.Destroy(PublicAvatarList);
            GameObject.Destroy(FavoriteButtonNew);
        }
    }
}