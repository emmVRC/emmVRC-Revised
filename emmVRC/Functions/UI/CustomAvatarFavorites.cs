using Il2CppSystem.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using VRC.Core;
using VRC.UI;
using emmVRC.Libraries;
using emmVRC.Network;
using System.Linq;
using emmVRC.Utils;
using emmVRC.Objects;
using System.Reflection;
using System.Collections;
using System.Threading.Tasks;
using emmVRC.Objects.ModuleBases;

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
        private static GameObject ShowAuthorButton;
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
        private static GameObject sortButton;
        private static bool waitingForSearch = false;
        public static int currentPage = 0;
        private static SortingMode currentSortingMode = SortingMode.DateAdded;
        private static bool sortingInverse = false; // False = First-to-Last, True = Last-to-First

        public override void OnUiManagerInit()
        {
            if (Configuration.JSONConfig.SortingMode <= 2)
                currentSortingMode = (SortingMode)Configuration.JSONConfig.SortingMode;
            else
                currentSortingMode = 0;
            sortingInverse = Configuration.JSONConfig.SortingInverse;

            pageAvatar = Libraries.QuickMenuUtils.GetVRCUiMInstance().menuContent().transform.Find("Screens/Avatar").gameObject;
            FavoriteButton = Libraries.QuickMenuUtils.GetVRCUiMInstance().menuContent().transform.Find("Screens/Avatar/Favorite Button").gameObject;
            FavoriteButtonNew = UnityEngine.Object.Instantiate<GameObject>(FavoriteButton, Libraries.QuickMenuUtils.GetVRCUiMInstance().menuContent().transform.Find("Screens/Avatar/"));
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
                        VRCUiPopupManager.prop_VRCUiPopupManager_0.ShowAlert("VRChat Plus Required", Attributes.VRCPlusMessage, 0f);
                    else
                    {
                        if (((apiAvatar.releaseStatus == "public" || apiAvatar.authorId == APIUser.CurrentUser.id) && apiAvatar.releaseStatus != null))
                        {
                            FavoriteAvatar(apiAvatar).NoAwait(nameof(FavoriteAvatar));
                        }
                        else
                        {
                            VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowStandardPopup("emmVRC", "Cannot favorite this avatar (it is private!)", "Dismiss", new System.Action(() => { VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup(); }));
                        }
                    }
                }
                else
                {
                    VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowStandardPopup("emmVRC", "Are you sure you want to unfavorite the avatar \"" + apiAvatar.name + "\"?", "Yes", new System.Action(() =>
                    {
                        UnfavoriteAvatar(apiAvatar).NoAwait(nameof(UnfavoriteAvatar));
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

            FavoriteButtonNew.transform.Find("Horizontal").GetComponentsInChildren<Transform>(true).ToList().ForEach(a =>
            {
                if (a.name != "FavoriteActionText" && a.name != "Horizontal")
                    a.gameObject.SetActive(false);
            });
            /*
            FavoriteButtonNew.transform.Find("Horizontal/FavoritesCountSpacingText").gameObject.SetActive(false);
            FavoriteButtonNew.transform.Find("Horizontal/FavoritesCurrentCountText").gameObject.SetActive(false);
            FavoriteButtonNew.transform.Find("Horizontal/FavoritesCountDividerText").gameObject.SetActive(false);
            FavoriteButtonNew.transform.Find("Horizontal/FavoritesMaxAvailableText").gameObject.SetActive(false);*/


            ShowAuthorButton = UnityEngine.Object.Instantiate<GameObject>(Libraries.QuickMenuUtils.GetVRCUiMInstance().menuContent().transform.Find("Screens/Avatar/Fallback Hide Button").gameObject, Libraries.QuickMenuUtils.GetVRCUiMInstance().menuContent().transform.Find("Screens/Avatar/"));
            ShowAuthorButton.GetComponentInChildren<Text>().text = "";
            ShowAuthorButton.GetComponent<RectTransform>().sizeDelta = new Vector2(82f, 82f);
            ShowAuthorButton.GetComponent<RectTransform>().anchoredPosition -= new Vector2(250f, -25f);
            GameObject.Instantiate(Libraries.QuickMenuUtils.GetVRCUiMInstance().menuContent().transform.Find("Screens/Avatar/AvatarDetails Button/PlatformIcon").gameObject, ShowAuthorButton.transform);
            ShowAuthorButton.transform.Find("PlatformIcon(Clone)").gameObject.SetActive(true);
            ShowAuthorButton.transform.Find("PlatformIcon(Clone)").GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            MelonLoader.MelonCoroutines.Start(ShowAuthorIconUpdate());
            Button ShowAuthorButtonButton = ShowAuthorButton.GetComponent<Button>();
            ShowAuthorButtonButton.onClick = new Button.ButtonClickedEvent();
            ShowAuthorButtonButton.onClick.AddListener(new System.Action(() =>
            {
                UnityEngine.Resources.FindObjectsOfTypeAll<MenuController>().First().ViewUserInfo(pageAvatar.GetComponent<PageAvatar>().field_Public_SimpleAvatarPedestal_0.field_Internal_ApiAvatar_0.authorId);
            }));

            GameObject oldPublicAvatarList;
            oldPublicAvatarList = Libraries.QuickMenuUtils.GetVRCUiMInstance().menuContent().transform.Find("Screens/Avatar/Vertical Scroll View/Viewport/Content/Legacy Avatar List").gameObject;
            PublicAvatarList = GameObject.Instantiate(oldPublicAvatarList, oldPublicAvatarList.transform.parent);
            PublicAvatarList.transform.SetAsFirstSibling();

            ChangeButton = Libraries.QuickMenuUtils.GetVRCUiMInstance().menuContent().transform.Find("Screens/Avatar/Change Button").gameObject;
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
                            VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowStandardPopup("emmVRC", "Cannot switch into this avatar (it is private).\nDo you want to unfavorite it?", "Yes", new System.Action(() => { UnfavoriteAvatar(selectedAvatar).NoAwait(nameof(UnfavoriteAvatar)); VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup(); }), "No", new System.Action(() => { VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup(); }));
                        else
                            VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowStandardPopup("emmVRC", "Cannot switch into this avatar (it is private).", "Dismiss", new System.Action(() => { VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup(); }));
                    }
                    else if (selectedAvatar.releaseStatus == "unavailable")
                    {
                        if (LoadedAvatars.ToArray().Any(a => a.id == selectedAvatar.id))
                            VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowStandardPopup("emmVRC", "Cannot switch into this avatar (no longer available).\nDo you want to unfavorite it?", "Yes", new System.Action(() => { UnfavoriteAvatar(selectedAvatar).NoAwait(nameof(UnfavoriteAvatar)); VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup(); }), "No", new System.Action(() => { VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup(); }));
                        else
                            VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowStandardPopup("emmVRC", "Cannot switch into this avatar (no longer available).", "Dismiss", new System.Action(() => { VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup(); }));
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
            /*pageAvatar.transform.Find("XplatHide Button").GetComponent<Button>().onClick.AddListener(new System.Action(() => {
                pageAvatar.transform.Find("Select Button").GetComponent<RectTransform>().anchoredPosition += new Vector2(0f, 80f);
                pageAvatar.transform.Find("AvatarPreviewBase/FallbackRoot").transform.localPosition += new Vector3(0f, 80f, 0f);
            }));
            pageAvatar.transform.Find("Select Button").GetComponent<Button>().onClick.AddListener(new System.Action(() => {
                pageAvatar.transform.Find("Select Button").GetComponent<RectTransform>().anchoredPosition += new Vector2(0f, 80f);
                pageAvatar.transform.Find("AvatarPreviewBase/FallbackRoot").transform.localPosition += new Vector3(0f, 80f, 0f);
            }));*/


            LoadedAvatars = new List<ApiAvatar>();

            SearchedAvatars = new List<ApiAvatar>();

            Components.EnableDisableListener pageAvatarListener = pageAvatar.AddComponent<Components.EnableDisableListener>();
            pageAvatarListener.OnEnabled += () =>
            {
                if ((!Configuration.JSONConfig.AvatarFavoritesEnabled || !Configuration.JSONConfig.emmVRCNetworkEnabled || NetworkClient.webToken == null) && (PublicAvatarList.activeSelf || FavoriteButtonNew.activeSelf))
                {
                    PublicAvatarList.SetActive(false);
                    FavoriteButtonNew.SetActive(false);
                }
                else if ((!PublicAvatarList.activeSelf || !FavoriteButtonNew.activeSelf) && Configuration.JSONConfig.AvatarFavoritesEnabled && Configuration.JSONConfig.emmVRCNetworkEnabled && NetworkClient.webToken != null)
                {
                    PublicAvatarList.SetActive(true);
                    FavoriteButtonNew.SetActive(true);
                    MelonLoader.MelonCoroutines.Start(RefreshMenu(1f));
                }
                if (error && !errorWarned)
                {
                    Managers.emmVRCNotificationsManager.AddNotification(new Notification("emmVRC Network", Functions.Core.Resources.errorSprite, "Your emmVRC avatars could not be loaded. Please ask in the Discord to resolve this.", true, false, null, "", "", true, null, "Dismiss"));
                    errorWarned = true;
                }
                MelonLoader.MelonCoroutines.Start(WaitToEnableSearch());
            };
            VRCUiPageHeader pageheader = QuickMenuUtils.GetVRCUiMInstance().GetComponentInChildren<VRCUiPageHeader>(true);
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
            if (searchBox != null && searchBox.field_Public_Button_0 != null && !searchBox.field_Public_Button_0.interactable)
            {
                searchBox.field_Public_Button_0.interactable = true;
                searchBox.field_Public_UnityAction_1_String_0 = searchBoxAction;
            }
        }

        public static IEnumerator ShowAuthorIconUpdate()
        {
            while (Functions.Core.Resources.authorSprite == null)
                yield return new WaitForEndOfFrame();
            ShowAuthorButton.transform.Find("PlatformIcon(Clone)").GetComponent<Image>().sprite = Functions.Core.Resources.authorSprite;
            ShowAuthorButton.transform.Find("PlatformIcon(Clone)").GetComponent<RectTransform>().sizeDelta /= new Vector2(1.5f, 1.5f);

        }
        public static async Task FavoriteAvatar(ApiAvatar avtr)
        {
            if (LoadedAvatars.ToArray().ToList().FindIndex(a => a.id == avtr.id) == -1)
            {
                LoadedAvatars.Insert(0, avtr);
                Network.Objects.Avatar serAvtr = new Network.Objects.Avatar(avtr);

                try
                {
                    await HTTPRequest.post(NetworkClient.baseURL + "/api/avatar", serAvtr);

                    if (!Searching)
                    {
                        currentPage = 0;
                        MelonLoader.MelonCoroutines.Start(JumpToStart());
                        MelonLoader.MelonCoroutines.Start(RefreshMenu(0.1f));
                    }
                }
                catch
                {
                    await emmVRC.AwaitUpdate.Yield();
                    VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowStandardPopup("emmVRC", "Error occured while updating avatar list.", "Dismiss", new System.Action(() => { VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup(); }));
                    throw;
                }
            }
            else
            {
                emmVRCLoader.Logger.LogDebug("Tried to add an avatar that already exists...");
            }
        }
        public static async Task UnfavoriteAvatar(ApiAvatar avtr)
        {
            if (LoadedAvatars.Contains(avtr))
                LoadedAvatars.Remove(avtr);

            try
            {
                await HTTPRequest.delete(NetworkClient.baseURL + "/api/avatar", new Network.Objects.Avatar(avtr));
                if (!Searching)
                {
                    await emmVRC.AwaitUpdate.Yield();

                    MelonLoader.MelonCoroutines.Start(JumpToStart());
                    MelonLoader.MelonCoroutines.Start(RefreshMenu(0.1f));
                }
            }
            catch
            {
                await emmVRC.AwaitUpdate.Yield();

                VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowStandardPopup("emmVRC", "Error occured while updating avatar list.", "Dismiss", new System.Action(() => { VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup(); }));
                throw;
            }
        }
        public static async Task PopulateList()
        {
            LoadedAvatars = new List<ApiAvatar>();
            Network.Objects.Avatar[] avatarArray = null;

            try
            {
                var result = await HTTPRequest.get(NetworkClient.baseURL + "/api/avatar");
                avatarArray = TinyJSON.Decoder.Decode(result).Make<Network.Objects.Avatar[]>();
                await emmVRC.AwaitUpdate.Yield();

                if (avatarArray != null)
                {
                    foreach (Network.Objects.Avatar avtr in avatarArray)
                    {
                        LoadedAvatars.Add(avtr.apiAvatar());
                    }
                    MelonLoader.MelonCoroutines.Start(RefreshMenu(0.1f));
                }
            }
            catch
            {
                await emmVRC.AwaitUpdate.Yield();

                Managers.emmVRCNotificationsManager.AddNotification(new Notification("emmVRC Network", Functions.Core.Resources.errorSprite, "emmVRC Avatar Favorites list failed to load. Please check your internet connection.", true, false, null, "", "", true, null, "Dismiss"));
                error = true;
                errorWarned = true;

                throw;
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
            SearchAvatars(query).NoAwait(nameof(SearchAvatars));
        }
        public static async Task SearchAvatars(string query)
        {
            if (!Configuration.JSONConfig.AvatarFavoritesEnabled || !Configuration.JSONConfig.emmVRCNetworkEnabled || NetworkClient.webToken == null)
            {
                await emmVRC.AwaitUpdate.Yield();
            }
            if (waitingForSearch)
                VRCUiPopupManager.prop_VRCUiPopupManager_0.ShowStandardPopup("emmVRC", "Please wait for your current search\nto finish before starting a new one.", "Okay", () => { VRCUiPopupManager.prop_VRCUiPopupManager_0.HideCurrentPopup(); });
            else
            {
                avText.GetComponentInChildren<Text>().text = "Searching. Please wait...";
                emmVRCLoader.Logger.LogDebug("Clearing current search avatars...");
                SearchedAvatars.Clear();
                Network.Objects.Avatar[] avatarArray = null;

                waitingForSearch = true;
                try
                {
                    var result = await HTTPRequest.post(NetworkClient.baseURL + "/api/avatar/search",
                        new System.Collections.Generic.Dictionary<string, string> { ["query"] = query });

                    if (result.Contains("Bad Request") || result.Contains("Too Many Requests"))
                    {
                        await emmVRC.AwaitUpdate.Yield();
                        VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowStandardPopup("emmVRC", "Your search could not be processed.", "Dismiss", new System.Action(() => { VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup(); }));
                    }
                    else
                    {
                        avatarArray = TinyJSON.Decoder.Decode(result).Make<Network.Objects.Avatar[]>();
                        await emmVRC.AwaitUpdate.Yield();
                        if (avatarArray != null)
                            foreach (Network.Objects.Avatar avatar in avatarArray)
                                SearchedAvatars.Add(avatar.apiAvatar());
                        currentPage = 0;
                        Searching = true;
                        MelonLoader.MelonCoroutines.Start(RefreshMenu(0.1f));
                    }
                }
                catch
                {
                    await emmVRC.AwaitUpdate.Yield();
                    VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowStandardPopup("emmVRC", "Your search could not be processed.", "Dismiss", new System.Action(() => { VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup(); }));
                    throw;
                }
                finally
                {
                    waitingForSearch = false;

                    await emmVRC.AwaitUpdate.Yield();

                    if (NewAvatarList.expandButton.gameObject.transform.Find("ToggleIcon").GetComponentInChildren<Image>().sprite == NewAvatarList.expandSprite)
                        NewAvatarList.ToggleExtend();
                }


            }
        }
        public void OnUpdate()
        {

            if (PublicAvatarList == null || !PublicAvatarList.activeInHierarchy) return;

            if (Configuration.JSONConfig.AvatarFavoritesEnabled && Configuration.JSONConfig.emmVRCNetworkEnabled && NetworkClient.webToken != null)
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
            if (NetworkClient.webToken == null || APIUser.CurrentUser == null || !Configuration.JSONConfig.SubmitAvatarPedestals || !NetworkConfig.Instance.AvatarPedestalScansAllowed) return;
            while (RoomManager.field_Internal_Static_ApiWorld_0 == null)
            {
                await emmVRC.AwaitUpdate.Yield();
            }
            ApiWorld currentWorld = RoomManager.field_Internal_Static_ApiWorld_0;
            if (currentWorld.IsPublicPublishedWorld)
            {
                foreach (AvatarPedestal pedestal in UnityEngine.Resources.FindObjectsOfTypeAll<AvatarPedestal>())
                {
                    if (pedestal != null && pedestal.field_Private_ApiAvatar_0 != null && pedestal.field_Private_ApiAvatar_0.releaseStatus == "public")
                    {
                        await emmVRC.AwaitUpdate.Yield();
                        Network.Objects.Avatar serAvtr = new Network.Objects.Avatar(pedestal.field_Private_ApiAvatar_0);
                        emmVRCLoader.Logger.LogDebug("Found pedestal " + pedestal.field_Private_ApiAvatar_0.name + ", putting...");
                        try
                        {
                            await HTTPRequest.put(NetworkClient.baseURL + "/api/avatar", serAvtr);
                        }
                        catch
                        {
                            await emmVRC.AwaitUpdate.Yield();
                            emmVRCLoader.Logger.LogDebug("Could not put avatar");
                            throw;
                        }
                        await Task.Delay(500);
                    }
                }
            }
        }
        public static void ExportAvatars()
        {
            System.Collections.Generic.List<ExportedAvatar> exportedAvatars = new System.Collections.Generic.List<ExportedAvatar>();
            foreach (ApiAvatar avtr in LoadedAvatars)
            {
                exportedAvatars.Add(new ExportedAvatar { avatar_id = avtr.id, avatar_name = avtr.name });
            }
            System.IO.File.WriteAllText(System.IO.Path.Combine(System.Environment.CurrentDirectory, "UserData/emmVRC/ExportedList.json"), TinyJSON.Encoder.Encode(exportedAvatars, TinyJSON.EncodeOptions.PrettyPrint | TinyJSON.EncodeOptions.NoTypeHints));
        }
    }
}
