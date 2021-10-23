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
    public class CreditsMenu : MelonLoaderEvents
    {
        private static bool _initialized = false;
        private static MenuPage creditsPage;
        private static SingleButton creditsPageButton;

        private static ButtonGroup creditsMembersGroup;
        private static MenuPage creditsNotePage;
        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            if (buildIndex != -1 || _initialized) return;
            creditsPage = new MenuPage("emmVRC_CreditsMenu", "Credits", false, true, true, () => { creditsNotePage.OpenMenu(); }, "...", Functions.Core.Resources.SupporterIcon);
            creditsPage.menuContents.GetComponent<UnityEngine.UI.VerticalLayoutGroup>().childControlHeight = true;
            creditsPageButton = new SingleButton(FunctionsMenu.otherGroup, "emmVRC Team", creditsPage.OpenMenu, "View all of the people that make emmVRC possible", Functions.Core.Resources.TeamIcon);
            creditsMembersGroup = new ButtonGroup(creditsPage, "emmVRC Team");
            new SimpleSingleButton(creditsMembersGroup, "<color=#E91E63>Emilia</color>", null, "Main developer and founder of emmVRC");
            new SimpleSingleButton(creditsMembersGroup, "<color=#E91E63>Hordini</color>", null, "Cofounder of emmVRC, community manager, and major cutie");
            new SimpleSingleButton(creditsMembersGroup, "<color=#E91E63>Ben</color>", null, "Current developer of the emmVRC Network");
            new SimpleSingleButton(creditsMembersGroup, "<color=#E91E63>Brandon</color>", null, "Original developer of the emmVRC Network");
            new SimpleSingleButton(creditsMembersGroup, "<color=#E91E63>Loukylor</color>", null, "Major developer responsible for emmVRC 3.0.0");
            new SimpleSingleButton(creditsMembersGroup, "<color=#E91E63>Janni9009</color>", null, "Developer and supporter");
            new SimpleSingleButton(creditsMembersGroup, "<color=#E91E63>Lily</color>", null, "Developer and supporter");
            new SimpleSingleButton(creditsMembersGroup, "<color=#0091FF>Supah</color>", null, "Major supporter and moderator");
            // new SimpleSingleButton(creditsPage, "<color=#0091FF>Provania</color>", null, "Moderator and helper in the community");
            new SimpleSingleButton(creditsMembersGroup, "<color=#0091FF>Rakosi2</color>", null, "Moderator and helper in the community");
            new SimpleSingleButton(creditsMembersGroup, "<color=#C67228>RiskiVR</color>", null, "Helper in the community");
            new SimpleSingleButton(creditsMembersGroup, "<color=#71368A>Requi</color>", null, "Huge help in figuring out the new VRChat UI system");
            new SimpleSingleButton(creditsMembersGroup, "<color=#71368A>Herp Derpinstine</color>", null, "Major coding help, and developer of MelonLoader");
            new SimpleSingleButton(creditsMembersGroup, "<color=#71368A>knah</color>", null, "Netcode developer, as well as developer of the Unhollower, the most important part of MelonLoader");
            new SimpleSingleButton(creditsMembersGroup, "<color=#71368A>Slaynash</color>", null, "Original developer of VRCModLoader, VRCTools and AvatarFav, the entire reason why emmVRC exists today");
            new SimpleSingleButton(creditsMembersGroup, "<color=#71368A>DubyaDude</color>", null, "Developer of the Ruby Button API, the library that allowed emmVRC to be one of the biggest VR-friendly mods");

            creditsNotePage = new MenuPage("emmVRC_CreditsNotes", "Note to you", false, true);
            creditsNotePage.menuContents.GetComponent<UnityEngine.UI.VerticalLayoutGroup>().childControlHeight = true;
            GameObject textBase = new GameObject("CreditsNoteText");

            textBase.transform.SetParent(creditsNotePage.menuContents);
            textBase.transform.localPosition = Vector3.zero;
            textBase.transform.localRotation = new Quaternion(0, 0, 0, 0);
            textBase.transform.localScale = Vector3.one;
            TextMeshProUGUI textText = textBase.AddComponent<TextMeshProUGUI>();
            textText.margin = new Vector4(25, 0, 50, 0);
            textText.text = "Here we are again, after yet another major rewrite of emmVRC. emmVRC started as a simple proof-of-concept for " +
                "modding VRChat. It was spawned from my constant curiosity to tinker with everything, and it originally only had a " +
                "primitive Global Dynamic Bones module, and that was all. But seeing the potential of mods, and especially wanting " +
                "to change the standard of VRChat modding (most mods at the time were desktop-only and used the console for everything), " +
                "I set off to test the boundaries of what a simple Mono mod could do.\n" +
                "\n" +
                "Fast forward over two years later, and emmVRC is a far bigger project than I ever could have imagined. Every day, we see " +
                "more and more users joining the Discord, and finding enjoyment out of emmVRC. It's amazing to see, and I could not be where " +
                "I am now without the support of all of the amazing people in the emmVRC Team, the VRChat Modding Group, and especially the " +
                "insanely supportive community. That means you guys!\n" +
                "\n" +
                "Thank you all for supporting emmVRC throughout the years. You make it worth it, and are the reason I'll continue to work on " +
                "this project for as long as I possibly can! See you around!\n" +
                "\n" +
                "-Emilia ♥";
            //textText.text = "emmVRC started off as a simple proof-of-concept for modding " +
            //                "VRChat. It originally only had a primitive Global Dynamic Bones " +
            //                "setup, and that was all. But seeing the potential of mods, " +
            //                "I set off to test the boundaries of what a simple Mono mod " +
            //                "could do.\n" +
            //                "\n" +
            //                "Fast forward an entire year later, and emmVRC is a far bigger " +
            //                "project than I ever could have imagined. Every day, we see " +
            //                "more and more users joining the Discord, and using the mod. " +
            //                "It's amazing to see, and I couldn't have done it without " +
            //                "all of you guys.\n" +
            //                "\n" +
            //                "Thank you for supporting emmVRC throughout the year, " +
            //                "and helping me keep doing what I do! -Emilia";

            _initialized = true;
        }
    }
}
