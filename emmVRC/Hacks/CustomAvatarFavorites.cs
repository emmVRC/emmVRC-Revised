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
        private static List<ApiAvatar> LoadedAvatars = new List<ApiAvatar>();
        private static System.Collections.Generic.List<Network.Objects.Avatar> LoadedSerializedAvatars = new System.Collections.Generic.List<Network.Objects.Avatar>();

        internal static void RenderElement(this UiVRCList uivrclist, List<ApiAvatar> AvatarList)
        {
            uivrclist.Method_Protected_Void_List_1_T_Int32_Boolean_0<ApiAvatar>(AvatarList, 0, true);
            //uivrclist.Method_Protected_List_1_T_Int32_Boolean_1<ApiAvatar>(AvatarList, 0, true);
        }
        internal static void Initialize() //TODO: FIX THIS SHIT GOD DAMNIT
        {

            //TODO test and finish
            TinyJSON.ProxyArray responseobj = (TinyJSON.ProxyArray)HTTPResponse.Serialize(HTTPRequest.get_sync(NetworkClient.baseURL + "/api/avatar"));
            //GET all avatars and thow them into a list
            foreach (object obj in responseobj)
            {
                emmVRCLoader.Logger.Log(obj.ToString());
            }
            pageAvatar = Libraries.QuickMenuUtils.GetVRCUiMInstance().menuContent.transform.Find("Screens/Avatar").gameObject;
            FavoriteButton = Libraries.QuickMenuUtils.GetVRCUiMInstance().menuContent.transform.Find("Screens/Avatar/Favorite Button").gameObject;
            FavoriteButtonNew = UnityEngine.Object.Instantiate<GameObject>(FavoriteButton, Libraries.QuickMenuUtils.GetVRCUiMInstance().menuContent.transform.Find("Screens/Avatar/"));
            FavoriteButtonNew.GetComponent<Button>().onClick.RemoveAllListeners();
            FavoriteButtonNew.GetComponent<Button>().onClick.AddListener(UnhollowerRuntimeLib.DelegateSupport.ConvertDelegate<UnityAction>((System.Action)(() =>
            {

                ApiAvatar apiAvatar = pageAvatar.GetComponent<PageAvatar>().avatar.field_Internal_ApiAvatar_0;
                bool flag = false;
                for (int i=0; i < LoadedAvatars.Count; i++)
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
                            emmVRCLoader.Logger.LogError("Could not favorite avatar because you have reached the maximum favorites");
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
                    VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowStandardPopup("emmVRC", "Are you sure you want to unfavorite the avatar \"" + apiAvatar.name+"\"?", "Yes", UnhollowerRuntimeLib.DelegateSupport.ConvertDelegate<Action>((System.Action)(() => {
                        UnfavoriteAvatar(apiAvatar);
                        VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup();
                    })), "No", UnhollowerRuntimeLib.DelegateSupport.ConvertDelegate<Action>((System.Action)(() => {
                        VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.HideCurrentPopup();
                    })));
                }
            })));

            FavoriteButtonNew.GetComponentInChildren<RectTransform>().localPosition += new Vector3(0, 165f);
            GameObject oldPublicAvatarList;
            oldPublicAvatarList = Libraries.QuickMenuUtils.GetVRCUiMInstance().menuContent.transform.Find("Screens/Avatar/Vertical Scroll View/Viewport/Content/Legacy Avatar List").gameObject;
            PublicAvatarList = GameObject.Instantiate(oldPublicAvatarList, oldPublicAvatarList.transform.parent);
            PublicAvatarList.name = "emmVRC Avatar List";
            PublicAvatarList.GetComponent<UiAvatarList>().category = UiAvatarList.EnumNPublicSealedvaInPuMiFaSpClPuLi9vUnique.SpecificList;
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

            
        }
        public static void FavoriteAvatar(ApiAvatar avtr)
        {
            LoadedAvatars.Insert(0, avtr);
            Network.Objects.Avatar serAvtr = new Network.Objects.Avatar
            {
                avatar_name = avtr.name,
                avatar_id = avtr.id,
                avatar_asset_url = avtr.assetUrl,
                avatar_thumbnail_image_url = avtr.thumbnailImageUrl,
                avatar_author_id = avtr.authorId,
                avatar_supported_platforms = avtr.supportedPlatforms
            };
            LoadedSerializedAvatars.Insert(0, serAvtr);
            try
            {
                //TODO test
                HTTPRequest.post_sync(NetworkClient.baseURL + "/api/avatar", serAvtr);
            }catch (System.Exception e)
            {
                emmVRCLoader.Logger.LogError(e.Message);
            }
            avText.GetComponentInChildren<Text>().text = "(" + LoadedAvatars.Count + ") emmVRC Favorites";
        }
        public static void UnfavoriteAvatar(ApiAvatar avtr)
        {
            if (LoadedAvatars.Contains(avtr))
            {
                //TODO: test
                HTTPRequest.delete_sync(NetworkClient.baseURL + "/api/avatar", avtr);
                LoadedAvatars.Remove(avtr);
            }
                
            for (int i=0; i < LoadedSerializedAvatars.Count; i++)
            {
                if (LoadedSerializedAvatars[0].avatar_id == avtr.id)
                    LoadedSerializedAvatars.RemoveAt(i);
            }
            avText.GetComponentInChildren<Text>().text = "(" + LoadedAvatars.Count + ") emmVRC Favorites";
        }
        private static void PopulateList()
        {
            foreach(Network.Objects.Avatar avatar in LoadedSerializedAvatars)
            {
                ApiAvatar item = new ApiAvatar
                {
                    name = avatar.avatar_name,
                    id = avatar.avatar_id,
                    assetUrl = avatar.avatar_asset_url,
                    thumbnailImageUrl = avatar.avatar_thumbnail_image_url,
                    authorId = avatar.avatar_author_id,
                    supportedPlatforms = avatar.avatar_supported_platforms,
                    releaseStatus = "public"
                };
                LoadedAvatars.Insert(0, item);
            }
            avText.GetComponent<Text>().text = "(" + LoadedAvatars.Count + ") emmVRC Favorites";
        }
        internal static void LoadUnblobbedAvatars()
        {
            ApiAvatar item = new ApiAvatar
            {
                name = "Emmy",
                id = "avtr_e34869bc-f63f-4921-8d52-ce1a5f0c8920",
                assetUrl = "https://api.vrchat.cloud/api/1/file/file_f77287b6-09eb-40a1-9a10-05ed75bfde92/19/file",
                thumbnailImageUrl = "https://d348imysud55la.cloudfront.net/thumbnails/file_b0ebcca9-1161-4883-bcd5-b5d38a6ac97c.201495736.1.thumbnail-256.png",
                authorId = "usr_6b650903-d1ca-41cf-9032-81c0001f1c6f",
                supportedPlatforms = ApiModel.SupportedPlatforms.All,
                releaseStatus = "public"
            };
            LoadedAvatars.Insert(0, item);
        }
        internal static void UpdateAvatarList()
        {
            try
            {
                //TODO finish
                TinyJSON.ProxyArray responseobj = (TinyJSON.ProxyArray)HTTPResponse.Serialize(HTTPRequest.get_sync(NetworkClient.baseURL + "/api/avatar"));
                avText.GetComponentInChildren<Text>().text = "(" + LoadedAvatars.Count + ") emmVRC Favorites";
                System.Collections.Generic.List<Network.Objects.Avatar> avtrs = new System.Collections.Generic.List<Network.Objects.Avatar>();
                foreach (ApiAvatar avatar in LoadedAvatars)
                {
                    Network.Objects.Avatar avtr = new Network.Objects.Avatar();
                    avtr.avatar_name = Convert.ToBase64String(Encoding.UTF8.GetBytes(avatar.name));
                    avtr.avatar_id = avatar.id;
                    avtr.avatar_asset_url = avatar.assetUrl;
                    avtr.avatar_thumbnail_image_url = avatar.thumbnailImageUrl;
                    avtr.avatar_author_id = avatar.authorId;
                    avtr.avatar_supported_platforms = avatar.supportedPlatforms;
                    avtrs.Add(avtr);
                }
            }
            catch (System.Exception ex)
            {
                emmVRCLoader.Logger.LogError("Unable to save favorited avatars: " + ex.ToString());
            }
        }
        internal static void OnUpdate()
        {
            if (PublicAvatarList == null || FavoriteButtonNew == null) return;
            if (PublicAvatarList.activeSelf && Configuration.JSONConfig.AvatarFavoritesEnabled)
            {
                try
                {
                    PublicAvatarList.GetComponent<UiAvatarList>().collapsedCount = 500;
                    PublicAvatarList.GetComponent<UiAvatarList>().expandedCount = 500;
                    if (PublicAvatarList.GetComponent<UiAvatarList>().pickers.Count < LoadedAvatars.Count)
                    {
                        PublicAvatarList.GetComponent<ScrollRect>().movementType = ScrollRect.MovementType.Unrestricted;
                    }
                    else
                    {
                        PublicAvatarList.GetComponent<ScrollRect>().movementType = ScrollRect.MovementType.Elastic;
                    }
                    PublicAvatarList.GetComponent<UiAvatarList>().RenderElement(LoadedAvatars);
                }
                catch (System.Exception ex)
                {
                    emmVRCLoader.Logger.LogError("An error occured manipulating the avatar menu: " + ex.ToString());
                }
                if (currPageAvatar != null && LoadedAvatars != null && FavoriteButtonNew != null)
                {
                    bool flag = false;
                    for (int i=0; i < LoadedAvatars.Count; i++)
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
            if ((error || LoadedAvatars.Count == 0 || !Configuration.JSONConfig.AvatarFavoritesEnabled) && PublicAvatarList.activeSelf && FavoriteButtonNew.activeSelf)
            {
                PublicAvatarList.SetActive(false);
                if (error || !Configuration.JSONConfig.AvatarFavoritesEnabled)
                    FavoriteButtonNew.SetActive(false);
            }
            else if (!PublicAvatarList.activeSelf && Configuration.JSONConfig.AvatarFavoritesEnabled)
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
