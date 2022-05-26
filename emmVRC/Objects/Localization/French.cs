using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace emmVRC.Objects.Localization
{
    public class French : Language
    {
        public override string NetworkLoginMessage { get; } = "Vous devez vous connecter au réseau emmVRC. Si vous avez un code PIN, saisissez-le. Si vous n'en avez pas, saisissez votre nouveau PIN.\n\n" +
            "Ce dernier est l'équivalent de votre mot de passe pour vous connecter au réseau emmVRC." +
            " Ne vous contentez pas d'entrer un nombre aléatoire ; faites-en quelque chose de mémorable !\n\n" +
            "Si vous avez oublié votre code PIN ou si vous rencontrez des problèmes, veuillez nous contacter sur le Discord emmVRC.";
        public override string PinResetMessage { get; } = "Votre code PIN doit être réinitialisé pour accéder au réseau emmVRC.";
        public override string BannedMessage { get; } = "Vous ne pouvez pas vous connecter au réseau emmVRC car vous êtes banni.\n\n";
        public override string NetworkUnavailableMessage { get; } = "Le réseau emmVRC est actuellement indisponible. Veuillez réessayer plus tard.";
        public override string AvatarPlatformErrorMessage { get; } = "Vous ne pouvez pas utiliser cet avatar car il n'a pas été publié pour cette plateforme.";
        public override string FavouriteFailedPrivateMessage { get; } = "Impossible de mettre en favori cet avatar (il est privé!)";
        public override string AvatarSwitchPrivateUnfavouriteMessage { get; } = "Impossible d'utiliser cet avatar (il est privé).\nVoulez-vous le retirer des favoris?";
        public override string AvatarSwitchPrivateMessage { get; } = "Impossible d'utiliser cet avatar (il est privé).";
        public override string AvatarSwitchDeletedUnfavouriteMessage { get; } = "Impossible d'utiliser cet avatar (il n'est plus disponible).\nVoulez-vous le retirer des favoris?";
        public override string AvatarSwitchDeletedMessage { get; } = "Impossible d'utiliser cet avatar (il n'est plus disponible).";
        public override string FavouriteListLoadErrorMessage { get; } = "Le chargement de la liste des avatars favoris de emmVRC a échoué. Veuillez vérifier votre connexion Internet.";
        public override string FavouriteListUpdateErrorMessage { get; } = "Une erreur s'est produite lors de la mise à jour de la liste des avatars.";
        public override string SearchRateLimitErrorMessage { get; } = "Veuillez attendre la fin de votre recherche\nactuelle avant d'en commencer une nouvelle.";
        public override string SearchFailedErrorMessage { get; } = "Votre recherche n'a pas pu être traitée.";
        public override string VRCPlusMessage { get; } = "VRChat, comme emmVRC, compte sur le soutien de ses utilisateurs pour que la plateforme reste gratuite. Veuillez soutenir VRChat pour débloquer ces fonctionnalités.";
    }
}
