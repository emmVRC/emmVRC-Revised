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
        private static System.Collections.Generic.List<Objects.SerializedAvatar> LoadedSerializedAvatars = new System.Collections.Generic.List<Objects.SerializedAvatar>();

        internal static void RenderElement(this UiVRCList uivrclist, List<ApiAvatar> AvatarList)
        {
            uivrclist.Method_Protected_List_1_T_Int32_Boolean_1<ApiAvatar>(AvatarList, 0, true);
        }
        internal static void Initialize()
        {
            pageAvatar = Libraries.QuickMenuUtils.GetVRCUiMInstance().menuContent.transform.Find("Screens/Avatar").gameObject;
            FavoriteButton = Libraries.QuickMenuUtils.GetVRCUiMInstance().menuContent.transform.Find("Screens/Avatar/Favorite Button").gameObject;
            FavoriteButtonNew = UnityEngine.Object.Instantiate<GameObject>(FavoriteButton, Libraries.QuickMenuUtils.GetVRCUiMInstance().menuContent.transform.Find("Screens/Avatar/"));
            FavoriteButtonNew.GetComponent<Button>().onClick.RemoveAllListeners();
            FavoriteButtonNew.GetComponent<Button>().onClick.AddListener(UnhollowerRuntimeLib.DelegateSupport.ConvertDelegate<UnityAction>((System.Action)(() =>
            {

                ApiAvatar apiAvatar = pageAvatar.GetComponent<PageAvatar>().avatar.field_ApiAvatar_0;
                bool flag = false;
                for (int i=0; i < LoadedAvatars.Count; i++)
                {
                    if (LoadedAvatars.get_Item(i).id == apiAvatar.id)
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
                            VRCUiPopupManager.field_VRCUiPopupManager_0.ShowStandardPopup("emmVRC", "You have reached the maximum emmVRC favorites size.", "Dismiss", UnhollowerRuntimeLib.DelegateSupport.ConvertDelegate<Action>((System.Action)(() => { VRCUiPopupManager.field_VRCUiPopupManager_0.HideCurrentPopup(); })));
                        }
                    }
                    else
                    {
                        VRCUiPopupManager.field_VRCUiPopupManager_0.ShowStandardPopup("emmVRC", "Cannot favorite this avatar (it is private!)", "Dismiss", UnhollowerRuntimeLib.DelegateSupport.ConvertDelegate<Action>((System.Action)(() => { VRCUiPopupManager.field_VRCUiPopupManager_0.HideCurrentPopup(); })));
                    }
                }
                else
                {
                    VRCUiPopupManager.field_VRCUiPopupManager_0.ShowStandardPopup("emmVRC", "Are you sure you want to unfavorite the avatar \"" + apiAvatar.name+"\"?", "Yes", UnhollowerRuntimeLib.DelegateSupport.ConvertDelegate<Action>((System.Action)(() => {
                        UnfavoriteAvatar(apiAvatar);
                        VRCUiPopupManager.field_VRCUiPopupManager_0.HideCurrentPopup();
                    })), "No", UnhollowerRuntimeLib.DelegateSupport.ConvertDelegate<Action>((System.Action)(() => {
                        VRCUiPopupManager.field_VRCUiPopupManager_0.HideCurrentPopup();
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

            
        }
        public static void FavoriteAvatar(ApiAvatar avtr)
        {
            LoadedAvatars.Add(avtr);
            Objects.SerializedAvatar serAvtr = new Objects.SerializedAvatar
            {
                name = avtr.name,
                id = avtr.id,
                assetUrl = avtr.assetUrl,
                thumbnailImageUrl = avtr.thumbnailImageUrl,
                authorId = avtr.authorId,
                supportedPlatforms = avtr.supportedPlatforms
            };
            LoadedSerializedAvatars.Insert(LoadedSerializedAvatars.Count, serAvtr);
            avText.GetComponentInChildren<Text>().text = "(" + LoadedAvatars.Count + ") emmVRC Favorites";
        }
        public static void UnfavoriteAvatar(ApiAvatar avtr)
        {
            if (LoadedAvatars.Contains(avtr))
                LoadedAvatars.Remove(avtr);
            for (int i=0; i < LoadedSerializedAvatars.Count; i++)
            {
                if (LoadedSerializedAvatars[0].id == avtr.id)
                    LoadedSerializedAvatars.RemoveAt(i);
            }
            avText.GetComponentInChildren<Text>().text = "(" + LoadedAvatars.Count + ") emmVRC Favorites";
        }
        private static void PopulateList()
        {
            foreach(Objects.SerializedAvatar avatar in LoadedSerializedAvatars)
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
                        if (LoadedAvatars.get_Item(i).id == currPageAvatar.avatar.field_ApiAvatar_0.id)
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
            if ((error || !Configuration.JSONConfig.AvatarFavoritesEnabled) && PublicAvatarList.activeSelf && FavoriteButtonNew.activeSelf)
            {
                PublicAvatarList.SetActive(false);
                FavoriteButtonNew.SetActive(false);
            }
            else if (!PublicAvatarList.activeSelf && Configuration.JSONConfig.AvatarFavoritesEnabled)
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
