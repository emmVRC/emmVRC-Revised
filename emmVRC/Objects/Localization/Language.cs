using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace emmVRC.Objects.Localization
{
    abstract public class Language
    {
        abstract public string NetworkLoginMessage { get; }
        abstract public string PinResetMessage { get; }
        abstract public string BannedMessage { get; }
        abstract public string NetworkUnavailableMessage { get; }
        abstract public string AvatarPlatformErrorMessage { get; }
        abstract public string FavouriteFailedPrivateMessage { get; }
        abstract public string AvatarSwitchPrivateUnfavouriteMessage { get; }
        abstract public string AvatarSwitchPrivateMessage { get; }
        abstract public string AvatarSwitchDeletedUnfavouriteMessage { get; }
        abstract public string AvatarSwitchDeletedMessage { get; }
        abstract public string FavouriteListLoadErrorMessage { get; }
        abstract public string FavouriteListUpdateErrorMessage { get; }
        abstract public string SearchRateLimitErrorMessage { get; }
        abstract public string SearchFailedErrorMessage { get; }
        abstract public string VRCPlusMessage { get; }
    }
}
