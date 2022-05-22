using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace emmVRC.Objects.Localization
{
    public class English : Language
    {
        public override string NetworkLoginMessage { get; } = "You need to log in to the emmVRC Network. If you have a pin, enter it. If you do not have a pin, enter your new pin.\n\n" +
                            "Your pin is the equivalent of your password to connect to the emmVRC Network." +
                            " Do not just enter a random number; make it something memorable!\n\n" +
                            "If you have forgotten your pin, or are experiencing issues, please contact us in the emmVRC Discord.";
        public override string PinResetMessage { get; } = "Your pin is required to be reset to access the emmVRC Network.";
        public override string BannedMessage { get; } = "You cannot connect to the emmVRC Network because you are banned.\n\n";
        public override string NetworkUnavailableMessage { get; } = "The emmVRC Network is currently unavailable. Please try again later.";
        public override string AvatarPlatformErrorMessage { get; } = "You cannot use this avatar as it has not been published for this platform.";
        public override string FavouriteFailedPrivateMessage { get; } = "Cannot favorite this avatar (it is private!)";
        public override string AvatarSwitchPrivateUnfavouriteMessage { get; } = "Cannot switch into this avatar (it is private).\nDo you want to unfavorite it?";
        public override string AvatarSwitchPrivateMessage { get; } = "Cannot switch into this avatar (it is private).";
        public override string AvatarSwitchDeletedUnfavouriteMessage { get; } = "Cannot switch into this avatar (no longer available).\nDo you want to unfavorite it?";
        public override string AvatarSwitchDeletedMessage { get; } = "Cannot switch into this avatar (no longer available).";
        public override string FavouriteListLoadErrorMessage { get; } = "emmVRC Avatar Favorites list failed to load. Please check your internet connection.";
        public override string FavouriteListUpdateErrorMessage { get; } = "Error occured while updating avatar list.";
        public override string SearchRateLimitErrorMessage { get; } = "Please wait for your current search\nto finish before starting a new one.";
        public override string SearchFailedErrorMessage { get; } = "Your search could not be processed.";
        public override string VRCPlusMessage { get; } = "VRChat, like emmVRC, relies on the support of their users to keep the platform free. Please support VRChat to unlock these features.";
    }
}
