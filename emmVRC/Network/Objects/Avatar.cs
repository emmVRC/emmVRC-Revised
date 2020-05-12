using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace emmVRC.Network.Objects
{
    public class Avatar
    {
        public string avatar_name = "";
        public string avatar_id = "";
        public string avatar_asset_url = "";
        public string avatar_thumbnail_image_url = "";
        public string avatar_author_id = "";
        public string avatar_category = "";
        public VRC.Core.ApiModel.SupportedPlatforms avatar_supported_platforms = VRC.Core.ApiModel.SupportedPlatforms.All;
    }
}
