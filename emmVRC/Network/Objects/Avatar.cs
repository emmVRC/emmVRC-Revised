using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace emmVRC.Network.Objects
{
    public class Avatar: SerializedObject
    {
        public Avatar() { }
        public Avatar(VRC.Core.ApiAvatar vrcAvatar)
        {
            this.avatar_id = vrcAvatar.id;
            this.avatar_name = vrcAvatar.name;
            this.avatar_asset_url = vrcAvatar.assetUrl;
            this.avatar_author_id = vrcAvatar.authorId;
            this.avatar_author_name = vrcAvatar.authorName;
            this.avatar_category = "";
            this.avatar_thumbnail_image_url = vrcAvatar.thumbnailImageUrl;
            this.avatar_supported_platforms = (int)vrcAvatar.supportedPlatforms;
        }
        public string avatar_name = "";
        public string avatar_id = "";
        public string avatar_asset_url = "";
        public string avatar_thumbnail_image_url = "";
        public string avatar_author_id = "";
        public string avatar_category = "";
        public string avatar_author_name = "";
        public int avatar_supported_platforms = (int)VRC.Core.ApiModel.SupportedPlatforms.All;
    }
}
