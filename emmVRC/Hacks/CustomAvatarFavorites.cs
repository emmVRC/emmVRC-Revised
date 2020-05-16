using Il2CppSystem;
using Il2CppSystem.Collections;
using Il2CppSystem.Collections.Generic;
using Il2CppSystem.Linq;
using Il2CppSystem.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Il2CppSystem.Reflection;
using VRC.Core;
using VRC;
using VRC.UI;
using emmVRC.Libraries;
using emmVRC.Network;

namespace emmVRC.Hacks
{
    public static class CustomAvatarFavorites
    {

        internal static GameObject PublicAvatarList;
        private static GameObject avText;
        private static GameObject ChangeButton;
        private static Button.ButtonClickedEvent baseChooseEvent;
        private static GameObject FavoriteButton;
        private static GameObject FavoriteButtonNew;
        private static GameObject pageAvatar;
        private static PageAvatar currPageAvatar;
        private static bool error = false;
        private static bool errorWarned;
        private static List<ApiAvatar> LoadedAvatars;
        private static int frameTimer = 0;
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
            FavoriteButtonNew.GetComponent<Button>().onClick.RemoveAllListeners();
            FavoriteButtonNew.GetComponent<Button>().onClick.AddListener(UnhollowerRuntimeLib.DelegateSupport.ConvertDelegate<UnityAction>((System.Action)(() =>
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
                            FavoriteAvatar(apiAvatar);
                        }
                        else
                        {
                            emmVRCLoader.Logger.LogError("[emmVRC] Could not favorite avatar because you have reached the maximum favorites");
                            VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowStandardPopup("emmVRC", "You have reached the maximum emmVRC favorites size.", "Dismiss", UnhollowerRuntimeLib.DelegateSupport.ConvertDelegate<Action>((System.Action)(() => { VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup(); })));
                        }
                    }
                    else
                    {
                        VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowStandardPopup("emmVRC", "Cannot favorite this avatar (it is private!)", "Dismiss", UnhollowerRuntimeLib.DelegateSupport.ConvertDelegate<Action>((System.Action)(() => { VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup(); })));
                    }
                }
                else
                {
                    VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowStandardPopup("emmVRC", "Are you sure you want to unfavorite the avatar \"" + apiAvatar.name + "\"?", "Yes", UnhollowerRuntimeLib.DelegateSupport.ConvertDelegate<Action>((System.Action)(() =>
                    {
                        UnfavoriteAvatar(apiAvatar);
                        VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup();
                    })), "No", UnhollowerRuntimeLib.DelegateSupport.ConvertDelegate<Action>((System.Action)(() =>
                    {
                        VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup();
                    })));
                }
            })));

            FavoriteButtonNew.GetComponentInChildren<RectTransform>().localPosition += new Vector3(0, 165f);
            GameObject oldPublicAvatarList;
            oldPublicAvatarList = Libraries.QuickMenuUtils.GetVRCUiMInstance().menuContent.transform.Find("Screens/Avatar/Vertical Scroll View/Viewport/Content/Legacy Avatar List").gameObject;
            PublicAvatarList = GameObject.Instantiate(oldPublicAvatarList, oldPublicAvatarList.transform.parent);
            PublicAvatarList.transform.SetAsFirstSibling();
            ChangeButton = Libraries.QuickMenuUtils.GetVRCUiMInstance().menuContent.transform.Find("Screens/Avatar/Change Button").gameObject;
            baseChooseEvent = ChangeButton.GetComponent<Button>().onClick;
            ChangeButton.GetComponent<Button>().onClick = new Button.ButtonClickedEvent();
            ChangeButton.GetComponent<Button>().onClick.AddListener(UnhollowerRuntimeLib.DelegateSupport.ConvertDelegate<UnityAction>((System.Action)(() =>
            {
                //emmVRCLoader.Bootstrapper.Instance.StartCoroutine(CheckAvatar());
                baseChooseEvent.Invoke();
            })));

            avText = PublicAvatarList.transform.Find("Button").gameObject;
            avText.GetComponentInChildren<Text>().text = "(0) emmVRC Favorites";
            currPageAvatar = pageAvatar.GetComponent<PageAvatar>();

            PublicAvatarList.GetComponent<UiAvatarList>().clearUnseenListOnCollapse = false;
            

            LoadedAvatars = new List<ApiAvatar>();

        }
        public static void FavoriteAvatar(ApiAvatar avtr)
        {
            LoadedAvatars.Insert(0, avtr);
            Network.Objects.Avatar serAvtr = new Network.Objects.Avatar(avtr);
            try
            {
                HTTPRequest.post_sync(NetworkClient.baseURL + "/api/avatar", serAvtr);
            }
            catch (System.Exception ex)
            {
                emmVRCLoader.Logger.LogError(ex.ToString());
            }
            avText.GetComponentInChildren<Text>().text = "(" + LoadedAvatars.Count + ") emmVRC Favorites";
        }
        public static void UnfavoriteAvatar(ApiAvatar avtr)
        {
            if (LoadedAvatars.Contains(avtr))
                LoadedAvatars.Remove(avtr);
            try
            {
                HTTPRequest.delete_sync(NetworkClient.baseURL + "/api/avatar", new Network.Objects.Avatar(avtr));
            }
            catch (System.Exception ex)
            {
                emmVRCLoader.Logger.LogError(ex.ToString());
            }
            avText.GetComponentInChildren<Text>().text = "(" + LoadedAvatars.Count + ") emmVRC Favorites";
        }
        public static void PopulateList()
        {
            LoadedAvatars = new List<ApiAvatar>();
            Network.Objects.Avatar[] avatarArray = null;
            try
            {
                avatarArray = TinyJSON.Decoder.Decode(HTTPRequest.get_sync(NetworkClient.baseURL + "/api/avatar")).Make<Network.Objects.Avatar[]>();
            }
            catch (System.Exception ex)
            {
                emmVRCLoader.Logger.LogError(ex.ToString());
            }
            if (avatarArray != null)
            {
                try
                {
                    foreach (Network.Objects.Avatar avtr in avatarArray)
                    {
                        LoadedAvatars.Add(avtr.apiAvatar());
                    }
                }
                catch (System.Exception ex)
                {
                    emmVRCLoader.Logger.LogError(ex.ToString());
                }
            }
            /*foreach (Objects.SerializedAvatar avatar in LoadedSerializedAvatars)
            {
                ApiAvatar item = new ApiAvatar
                {
                    name = avatar.name,
                    id = avatar.id,
                    assetUrl = avatar.assetUrl,
                    thumbnailImageUrl = avatar.thumbnailImageUrl,
                    authorId = avatar.authorId,
                    supportedPlatforms = avatar.supportedPlatforms,
                    releaseStatus = "public"
                };
                LoadedAvatars.Insert(0, item);
            }*/
            try
            {
                avText.GetComponentInChildren<Text>().text = "(" + LoadedAvatars.Count + ") emmVRC Favorites";
            }
            catch (System.Exception ex)
            {
                emmVRCLoader.Logger.LogError("Error occured while populating favorites: " + ex.ToString());
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
        internal static void OnUpdate()
        {
            if (PublicAvatarList == null || FavoriteButtonNew == null) return;
            if (PublicAvatarList.activeSelf && Configuration.JSONConfig.AvatarFavoritesEnabled && Configuration.JSONConfig.emmVRCNetworkEnabled && NetworkClient.authToken != null)
            {
                PublicAvatarList.GetComponent<UiAvatarList>().collapsedCount = 500;
                PublicAvatarList.GetComponent<UiAvatarList>().expandedCount = 500;

                if (PublicAvatarList.GetComponent<UiAvatarList>().pickers.Count < LoadedAvatars.Count || PublicAvatarList.GetComponent<UiAvatarList>().content.childCount <= 0)
                {
                    PublicAvatarList.GetComponent<ScrollRect>().movementType = ScrollRect.MovementType.Unrestricted;

                }
                else
                {
                    PublicAvatarList.GetComponent<ScrollRect>().movementType = ScrollRect.MovementType.Elastic;
                }
                PublicAvatarList.GetComponent<UiAvatarList>().RenderElement(LoadedAvatars);


                
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
                        FavoriteButtonNew.GetComponentInChildren<Text>().text = "<color=#FF69B4>emmVRC</color> Favorite";
                    }
                    else
                    {
                        FavoriteButtonNew.GetComponentInChildren<Text>().text = "<color=#FF69B4>emmVRC</color> Unfavorite";
                    }
                }
            }
            if ((error || LoadedAvatars.Count == 0 || !Configuration.JSONConfig.AvatarFavoritesEnabled || !Configuration.JSONConfig.emmVRCNetworkEnabled || NetworkClient.authToken == null) && PublicAvatarList.activeSelf)
            {
                PublicAvatarList.SetActive(false);
                if (error || !Configuration.JSONConfig.AvatarFavoritesEnabled || !Configuration.JSONConfig.emmVRCNetworkEnabled || NetworkClient.authToken == null)
                    FavoriteButtonNew.SetActive(false);
            }
            else if (!PublicAvatarList.activeSelf && Configuration.JSONConfig.AvatarFavoritesEnabled && Configuration.JSONConfig.emmVRCNetworkEnabled)
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
