using Il2CppSystem;
using Il2CppSystem.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using VRC.Core;
using VRC.UI;
using emmVRC.Libraries;
using emmVRC.Network;


namespace emmVRC.Hacks
{
    public static class CustomAvatarFavorites
    {

        internal static GameObject PublicAvatarList;
        internal static UiAvatarList NewAvatarList;
        private static GameObject avText;
        private static GameObject ChangeButton;
        private static Button.ButtonClickedEvent baseChooseEvent;
        private static GameObject FavoriteButton;
        private static GameObject FavoriteButtonNew;
        private static Button FavoriteButtonNewButton;
        private static Text FavoriteButtonNewText;
        private static GameObject pageAvatar;
        private static PageAvatar currPageAvatar;
        private static bool error = false;
        private static bool errorWarned;
        private static List<ApiAvatar> LoadedAvatars;
        private static bool menuJustActivated = false;
        //private static System.Collections.Generic.List<Objects.SerializedAvatar> LoadedSerializedAvatars = new System.Collections.Generic.List<Objects.SerializedAvatar>();

        internal static void RenderElement(this UiVRCList uivrclist, List<ApiAvatar> AvatarList)
        {
            uivrclist.Method_Protected_Void_List_1_T_Int32_Boolean_0<ApiAvatar>(AvatarList, 0, true);
            //uivrclist.Method_Protected_List_1_T_Int32_Boolean_1<ApiAvatar>(AvatarList, 0, true);
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
                    if ((apiAvatar.releaseStatus == "public" || apiAvatar.authorId == APIUser.CurrentUser.id) && apiAvatar.releaseStatus != null)
                    {
                        if (LoadedAvatars.Count < 500)
                        {
                            MelonLoader.MelonCoroutines.Start(FavoriteAvatar(apiAvatar));
                        }
                        else
                        {
                            emmVRCLoader.Logger.LogError("[emmVRC] Could not favorite avatar because you have reached the maximum favorites");
                            VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowStandardPopup("emmVRC", "You have reached the maximum emmVRC favorites size.", "Dismiss", new System.Action(() => { VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup(); }));
                        }
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
            GameObject oldPublicAvatarList;
            oldPublicAvatarList = Libraries.QuickMenuUtils.GetVRCUiMInstance().menuContent.transform.Find("Screens/Avatar/Vertical Scroll View/Viewport/Content/Legacy Avatar List").gameObject;
            PublicAvatarList = GameObject.Instantiate(oldPublicAvatarList, oldPublicAvatarList.transform.parent);
            PublicAvatarList.transform.SetAsFirstSibling();
            ChangeButton = Libraries.QuickMenuUtils.GetVRCUiMInstance().menuContent.transform.Find("Screens/Avatar/Change Button").gameObject;
            baseChooseEvent = ChangeButton.GetComponent<Button>().onClick;
            ChangeButton.GetComponent<Button>().onClick = new Button.ButtonClickedEvent();
            ChangeButton.GetComponent<Button>().onClick.AddListener(new System.Action(() =>
            {
                //emmVRCLoader.Bootstrapper.Instance.StartCoroutine(CheckAvatar());
                ApiAvatar selectedAvatar = pageAvatar.GetComponent<PageAvatar>().avatar.field_Internal_ApiAvatar_0;
                if (selectedAvatar.releaseStatus == "private" && selectedAvatar.authorId != APIUser.CurrentUser.id)
                {
                    VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowStandardPopup("emmVRC", "Cannot switch into this avatar (it is private).\nDo you want to unfavorite it?", "Yes", new System.Action(() => { UnfavoriteAvatar(selectedAvatar); VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup(); }), "No", new System.Action(() => { VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup(); }));
                }
                else if (selectedAvatar.releaseStatus == "unavailable")
                {
                    VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowStandardPopup("emmVRC", "Cannot switch into this avatar (no longer available).\nDo you want to unfavorite it?", "Yes", new System.Action(() => { UnfavoriteAvatar(selectedAvatar); VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup(); }), "No", new System.Action(() => { VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup(); }));
                } else
                baseChooseEvent.Invoke();
            }));

            avText = PublicAvatarList.transform.Find("Button").gameObject;
            avText.GetComponentInChildren<Text>().text = "(0) emmVRC Favorites";
            currPageAvatar = pageAvatar.GetComponent<PageAvatar>();
            NewAvatarList = PublicAvatarList.GetComponent<UiAvatarList>();
            NewAvatarList.clearUnseenListOnCollapse = false;
            GameObject refreshButton = GameObject.Instantiate(ChangeButton, avText.transform.parent);
            refreshButton.GetComponentInChildren<Text>().text = "↻";
            refreshButton.GetComponent<Button>().onClick.RemoveAllListeners();
            refreshButton.GetComponent<Button>().onClick.AddListener(new System.Action(() =>
            {
                    MelonLoader.MelonCoroutines.Start(RefreshMenu(0.5f));
            }));
            refreshButton.GetComponent<RectTransform>().sizeDelta /= new Vector2(4f, 1f);
            refreshButton.GetComponent<RectTransform>().anchoredPosition = avText.transform.Find("ToggleIcon").GetComponent<RectTransform>().anchoredPosition + new Vector2(325f, 110f);

            pageAvatar.transform.Find("AvatarModel").transform.localPosition += new Vector3(0f, 60f, 0f);

            LoadedAvatars = new List<ApiAvatar>();
        }
        public static void Refresh()
        {
            MelonLoader.MelonCoroutines.Start(RefreshMenu(0.5f));
        }
        public static System.Collections.IEnumerator FavoriteAvatar(ApiAvatar avtr)
        {
            LoadedAvatars.Insert(0, avtr);
            Network.Objects.Avatar serAvtr = new Network.Objects.Avatar(avtr);

            var request = HTTPRequest.post(NetworkClient.baseURL + "/api/avatar", serAvtr);
            while (!request.IsCompleted && !request.IsFaulted)
                yield return new WaitForEndOfFrame();
            if (!request.IsFaulted)
            {
                avText.GetComponentInChildren<Text>().text = "(" + LoadedAvatars.Count + ") emmVRC Favorites";
                MelonLoader.MelonCoroutines.Start(RefreshMenu(0.1f));
            }
            else
            {
                emmVRCLoader.Logger.LogError("Asynchronous net post failed: " + request.Exception);
                VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowStandardPopup("emmVRC", "Error occured while updating avatar list.", "Dismiss", new System.Action(() => { VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup(); }));
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
                avText.GetComponentInChildren<Text>().text = "(" + LoadedAvatars.Count + ") emmVRC Favorites";
                MelonLoader.MelonCoroutines.Start(RefreshMenu(0.1f));
            } else
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
                avatarArray = TinyJSON.Decoder.Decode(HTTPRequest.get(NetworkClient.baseURL + "/api/avatar").Result).Make<Network.Objects.Avatar[]>();
                if (avatarArray != null)
                {
                    try
                    {
                        foreach (Network.Objects.Avatar avtr in avatarArray)
                        {
                            LoadedAvatars.Add(avtr.apiAvatar());
                        }
                        avText.GetComponentInChildren<Text>().text = "(" + LoadedAvatars.Count + ") emmVRC Favorites";
                    }
                    catch (System.Exception ex)
                    {
                        emmVRCLoader.Logger.LogError(ex.ToString());
                    }
                }
            } else
            {
                emmVRCLoader.Logger.LogError("Asynchronous net get failed: " + request.Exception);
                Managers.NotificationManager.AddNotification("emmVRC Avatar Favorites list failed to load. Please check your internet connection.", "Dismiss", () => { Managers.NotificationManager.DismissCurrentNotification(); }, "", null, Resources.errorSprite, -1);
                error = true;
                errorWarned = true;
            }

        }
        /*internal static void UpdateAvatarList()
        {
            try
            {
                avText.GetComponentInChildren<Text>().text = "(" + LoadedAvatars.Count + ") emmVRC Favorites";
                System.Collections.Generic.List<Objects.SerializedAvatar> avtrs = new System.Collections.Generic.List<Objects.SerializedAvatar>();
                 foreach (ApiAvatar avatar in LoadedAvatars)
                {
                    Objects.SerializedAvatar avtr = new Objects.SerializedAvatar();
                    avtr.name = Convert.ToBase64String(Encoding.UTF8.GetBytes(avatar.name));
                    avtr.id = avatar.id;
                    avtr.assetUrl = avatar.assetUrl;
                    avtr.thumbnailImageUrl = avatar.thumbnailImageUrl;
                    avtr.authorId = avatar.authorId;
                    avtr.supportedPlatforms = avatar.supportedPlatforms;
                    avtrs.Add(avtr);
                }
            }
            catch (System.Exception ex)
            {
                emmVRCLoader.Logger.LogError("[emmVRC] Unable to save favorited avatars: " + ex.ToString());
            }
        }*/
        public static System.Collections.IEnumerator RefreshMenu(float delay)
        {
            PublicAvatarList.GetComponent<ScrollRect>().movementType = ScrollRect.MovementType.Unrestricted;
            yield return new WaitForSeconds(delay);
            NewAvatarList.RenderElement(LoadedAvatars);
            PublicAvatarList.GetComponent<ScrollRect>().movementType = ScrollRect.MovementType.Elastic;
        }
        internal static void OnUpdate()
        {
            if (PublicAvatarList == null || FavoriteButtonNew == null) return;
            if (PublicAvatarList.activeSelf && Configuration.JSONConfig.AvatarFavoritesEnabled && Configuration.JSONConfig.emmVRCNetworkEnabled && NetworkClient.authToken != null)
            {
                NewAvatarList.collapsedCount = 500;
                NewAvatarList.expandedCount = 500;

                /*if (!PublicAvatarList.activeInHierarchy)
                {
                    menuJustActivated = false;
                    menuWasActivated = false;
                }
                if (PublicAvatarList.activeInHierarchy && !menuWasActivated && !menuJustActivated)
                    menuJustActivated = true;*/
                
                if (!menuJustActivated)
                {
                    MelonLoader.MelonCoroutines.Start(RefreshMenu(1f));
                    menuJustActivated = true;
                }
                if (menuJustActivated && (NewAvatarList.pickers.Count < LoadedAvatars.Count || NewAvatarList.isOffScreen))
                    menuJustActivated = false;
                NewAvatarList.clearUnseenListOnCollapse = false;
                NewAvatarList.deferInitialFetch = true;
                NewAvatarList.hideElementsWhenContracted = false;
                NewAvatarList.hideWhenEmpty = false;
                NewAvatarList.usePagination = false;
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
            if ((error || LoadedAvatars.Count == 0 || !Configuration.JSONConfig.AvatarFavoritesEnabled || !Configuration.JSONConfig.emmVRCNetworkEnabled || NetworkClient.authToken == null) && (PublicAvatarList.activeSelf || FavoriteButtonNew.activeSelf))
            {
                PublicAvatarList.SetActive(false);
                if (error || !Configuration.JSONConfig.AvatarFavoritesEnabled || !Configuration.JSONConfig.emmVRCNetworkEnabled || NetworkClient.authToken == null)
                    FavoriteButtonNew.SetActive(false);
            }
            else if ((!PublicAvatarList.activeSelf || !FavoriteButtonNew.activeSelf) && Configuration.JSONConfig.AvatarFavoritesEnabled && Configuration.JSONConfig.emmVRCNetworkEnabled)
            {
                if (LoadedAvatars.Count > 0)
                    PublicAvatarList.SetActive(true);
                FavoriteButtonNew.SetActive(true);
            }
            if (error && !errorWarned)
            {
                Managers.NotificationManager.AddNotification("Your emmVRC avatars could not be loaded. Please contact yoshifan#9550 to resolve this.", "Dismiss", () => { Managers.NotificationManager.DismissCurrentNotification(); }, "", null, Resources.errorSprite, -1);
                errorWarned = true;
            }
        }

        private static System.Collections.IEnumerator SetAvatarListAfterDelay(UiAvatarList avatars, List<ApiAvatar> models)
        {
            if (models.Count == 0) yield break;

            var tempLis = new List<ApiAvatar>();
            tempLis.Add(models[0]);
            avatars.Method_Protected_Void_List_1_T_Int32_Boolean_0(tempLis, 0, true);
            yield return new WaitForSeconds(1f);
            avatars.Method_Protected_Void_List_1_T_Int32_Boolean_0(models, 0, true);
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
