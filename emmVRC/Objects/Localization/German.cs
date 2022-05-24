using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace emmVRC.Objects.Localization
{
    public class German : Language
    {
        public override string NetworkLoginMessage { get; } = "Du musst dich beim emmVRC-Netzwerk anmelden. Wenn du einen PIN hast, gebe ihn ein. Wenn du keinen PIN hast, gib deinen neue PIN ein.\n\n" +
            "Deine PIN ist wie ein Passwort, um dich mit dem emmVRC-Netzwerk zu verbinden. Gib nicht einfach irgendeine Zufallszahl ein; Sorg dafür das du deinen Pin nicht vergisst!\n\n" +
            "Falls du das doch tust oder Probleme auftreten, musst du im emmVRC Discord nach einem pin reset fragen.";
        public override string PinResetMessage { get; } = "Deine Pin muss zurückgesetzt werden, um auf das emmVRC-Netzwerk zugreifen zu können.";
        public override string BannedMessage { get; } = "Du kannst dich nicht mit dem emmVRC-Netzwerk verbinden, weil du gebannt bist.\n\n";
        public override string NetworkUnavailableMessage { get; } = "Das emmVRC-Netzwerk ist derzeit nicht verfügbar. Bitte versuche es später erneut.";
        public override string AvatarPlatformErrorMessage { get; } = "Du kannst diesen Avatar nicht verwenden, da er nicht für diese Plattform veröffentlicht wurde.";
        public override string FavouriteFailedPrivateMessage { get; } = "Private Avatare können nicht favorisiert werden!";
        public override string AvatarSwitchPrivateUnfavouriteMessage { get; } = "Kann nicht zu diesem Avatar wechseln (Privater Avatar).\nMöchtest du ihn aus den Favoriten entfernen?";
        public override string AvatarSwitchPrivateMessage { get; } = "Kann nicht zu diesem Avatar wechseln (Privater Avatar).";
        public override string AvatarSwitchDeletedUnfavouriteMessage { get; } = "Kann nicht zu diesem Avatar wechseln (Nicht mehr verfügbar).\nMöchtest du ihn aus den Favoriten entfernen?";
        public override string AvatarSwitchDeletedMessage { get; } = "Kann nicht zu diesem Avatar wechseln (Nicht mehr verfügbar).";
        public override string FavouriteListLoadErrorMessage { get; } = "emmVRC-Avatar-Favoritenliste konnte nicht geladen werden. Bitte überprüfe deine Internetverbindung.";
        public override string FavouriteListUpdateErrorMessage { get; } = "Beim Aktualisieren der Avatar-Liste ist ein Fehler aufgetreten.";
        public override string SearchRateLimitErrorMessage { get; } = "Bitte warte, bis deine aktuelle Suche\nbeendet ist, bevor du eine neue Suche startest.";
        public override string SearchFailedErrorMessage { get; } = "Fehler beim Suchen.";
        public override string VRCPlusMessage { get; } = "VRChat ist wie emmVRC auf die Unterstützung ihrer Benutzer angewiesen, um die Plattform kostenlos zu halten. Kaufe VRChat+, um diese Funktion freizuschalten.";
    }
}
