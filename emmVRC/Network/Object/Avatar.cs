using emmVRC.Objects;
using Il2CppSystem.Collections.Generic;
using VRC.Core;
// ReSharper disable MemberCanBePrivate.Global

namespace emmVRC.Network.Object
{
    public class Avatar
    {
        public string avatar_name = "";
        public string avatar_id = "";
        public string avatar_asset_url = "";
        public string avatar_thumbnail_image_url = "";
        public string avatar_author_id = "";
        public string avatar_category = "";
        public string avatar_author_name = "";
        public int avatar_public = 1;
        public int avatar_supported_platforms = (int)ApiModel.SupportedPlatforms.All;

        private List<string> avatar_tags
        {
            get
            {
                List<string> list = new List<string>();
                list.Add("avatar");
                return list;
            }
        }
        
        public Avatar()
        {
            
        }
        
        public Avatar(ApiAvatar vrcAvatar)
        {
            avatar_id = vrcAvatar.id;
            avatar_name = vrcAvatar.name;
            avatar_asset_url = vrcAvatar.assetUrl;
            avatar_author_id = vrcAvatar.authorId;
            avatar_author_name = vrcAvatar.authorName;
            avatar_category = "";
            avatar_thumbnail_image_url = vrcAvatar.thumbnailImageUrl;
            avatar_supported_platforms = (int)vrcAvatar.supportedPlatforms;
            avatar_public = (vrcAvatar.releaseStatus == "private" ? 0 : (vrcAvatar.releaseStatus == "public" ? 1 : 255));
        }
        
        
        public ApiAvatar ToApiAvatar()
        {
            if (avatar_public != 2)
            {
                return new ApiAvatar
                {
                    name = avatar_name,
                    id = avatar_id,
                    assetUrl = avatar_asset_url,
                    thumbnailImageUrl = avatar_thumbnail_image_url,
                    authorId = avatar_author_id,
                    authorName = avatar_author_name,
                    description = avatar_name,
                    releaseStatus = (NetworkClient.networkConfiguration.DisableAvatarChecks ? "public" : (avatar_public == 0 ? "private" : (avatar_public == 1 ? "public" : "unavailable"))),
                    unityVersion = "2018.4.20f1",
                    version = 1,
                    apiVersion = 1,
                    Endpoint = "avatars",
                    Populated = false,
                    assetVersion = new AssetVersion("2018.4.20f1", 0),
                    tags = avatar_tags,
                    supportedPlatforms = (ApiModel.SupportedPlatforms)avatar_supported_platforms
                };
            }

            return new ApiAvatar
            {
                releaseStatus = "unavailable",
                name = avatar_name,
                id = "null",
                assetUrl = "",
                thumbnailImageUrl = "http://img.thetrueyoshifan.com/AvatarUnavailable.png",
                version = 0,
                apiVersion = 0,
                Endpoint = "avatars",
                Populated = false,
                assetVersion = new AssetVersion("2018.4.20f1", 0)
            };
        }
    }

}
