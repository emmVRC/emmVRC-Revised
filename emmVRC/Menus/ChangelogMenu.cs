using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using emmVRC.Utils;
using emmVRC.Objects.ModuleBases;
using TMPro;

namespace emmVRC.Menus
{
    [Priority(55)]
    public class ChangelogMenu : MelonLoaderEvents
    {
        private static bool _initialized = false;
        internal static MenuPage changelogPage;
        private static SingleButton changelogPageButton;
        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            if (buildIndex != -1 || _initialized) return;
            changelogPage = new MenuPage("emmVRC_Changelog", "Changelog", false, true, true, () => {
                Utils.ButtonAPI.GetQuickMenuInstance().AskConfirmOpenURL("https://discord.gg/emmVRC");
            });
            changelogPage.menuContents.GetComponent<UnityEngine.UI.VerticalLayoutGroup>().childControlHeight = true;
            changelogPageButton = new SingleButton(FunctionsMenu.otherGroup, "Changelog", changelogPage.OpenMenu, "View what's new with the current version of emmVRC", Functions.Core.Resources.ChangelogIcon);
            GameObject textBase = new GameObject("ChangelogText");
            textBase.transform.SetParent(changelogPage.menuContents);
            textBase.transform.localPosition = Vector3.zero;
            textBase.transform.localRotation = new Quaternion(0, 0, 0, 0);
            textBase.transform.localScale = Vector3.one;
            TextMeshProUGUI textText = textBase.AddComponent<TextMeshProUGUI>();
            textText.margin = new Vector4(25, 0, 50, 0);
            textText.text = "<size=50><color=#FF69B4>emmVRC</color> version " + Objects.Attributes.Version.ToString(3) + (Objects.Attributes.Beta ? ("b"+Objects.Attributes.Version.Revision) : "") + " (" + Objects.Attributes.DateUpdated + ")</size>\n\n"+Objects.Attributes.Changelog;

            _initialized = true;
        }
    }
}
