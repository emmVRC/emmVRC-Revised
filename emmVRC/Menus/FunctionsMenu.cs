using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using emmVRC.Libraries;
using UnityEngine.Events;
using UnityEngine;
using UnityEngine.UI;
using UnhollowerRuntimeLib;

namespace emmVRC.Menus
{
    public class FunctionsMenu
    {
        public static PaginatedMenu baseMenu;
        private static PageItem worldTweaksButton;
        private static PageItem playerTweaksButton;
        private static PageItem instanceHistoryButton;
        private static PageItem disabledButtonsButton;
        private static PageItem programsButton;
        private static PageItem settingsButton;
        private static PageItem creditsButton;
        private static PageItem supporterButton;
        private static PageItem forceQuitButton;
        private static PageItem instantRestartButton;
        public static void Initialize()
        {
            // Initialize the base menu for the Functions menu
            baseMenu = new PaginatedMenu("ShortcutMenu", Configuration.JSONConfig.FunctionsButtonX, Configuration.JSONConfig.FunctionsButtonY, "<color=#FF69B4>emmVRC</color>\nFunctions", "Extra functions that can enhance the user experience or provide practical features", null);

            // Add the World Tweaks button
            worldTweaksButton = new PageItem("World\nTweaks", () => { QuickMenuUtils.ShowQuickmenuPage(WorldTweaksMenu.baseMenu.getMenuName()); }, "Contains tweaks to affect the world around you");
            baseMenu.pageItems.Add(worldTweaksButton);

            // Add the Player Tweaks button
            playerTweaksButton = new PageItem("Player\nTweaks", () => { QuickMenuUtils.ShowQuickmenuPage(PlayerTweaksMenu.baseMenu.getMenuName()); }, "Contains tweaks to affect your movement, as well as the players around you");
            baseMenu.pageItems.Add(playerTweaksButton);

            // Add the Instance History button
            instanceHistoryButton = new PageItem("Instance\nHistory", () => { InstanceHistoryMenu.baseMenu.OpenMenu(); InstanceHistoryMenu.LoadMenu(); }, "Allows you to join an instance you were previously in, so long as you have not been kicked from it");
            baseMenu.pageItems.Add(instanceHistoryButton);

            // Add the Disabled Buttons button
            disabledButtonsButton = new PageItem("Disabled\nButtons", () => { DisabledButtonMenu.LoadMenu(); }, "Contains buttons from the Quick Menu that were disabled by emmVRC");
            baseMenu.pageItems.Add(disabledButtonsButton);

            // Add the Programs button
            programsButton = new PageItem("Programs", () => { ProgramMenu.baseMenu.OpenMenu(); }, "Lets you launch external programs from within VRChat.");
            baseMenu.pageItems.Add(programsButton);

            // Add the Settings button
            settingsButton = new PageItem("Settings", () => {
                SettingsMenu.LoadMenu();
            }, "Access the Settings for emmVRC, including Risky Functions, color changes, etc.");
            baseMenu.pageItems.Add(settingsButton);
            for (int i=0; i <= 5; i++)
            {
                baseMenu.pageItems.Add(PageItem.Space);
            }
            creditsButton = new PageItem("<color=#ee006c>emmVRC\nTeam</color>", () => { CreditsMenu.baseMenu.OpenMenu(); }, "View all the users that make this project possible! <3");
            baseMenu.pageItems.Add(creditsButton);
            baseMenu.pageItems.Add(PageItem.Space);
            baseMenu.pageItems.Add(PageItem.Space);



            supporterButton = new PageItem("<color=#eac81e>Supporters</color>", () => { SupporterMenu.LoadMenu(); }, "Shows all the current supporters of the emmVRC project! Thank you to everyone who has donated so far! <3");
            baseMenu.pageItems.Add(supporterButton);

            forceQuitButton = new PageItem("Force\nQuit", () => { DestructiveActions.ForceQuit(); }, "Quits the game, instantly.");
            baseMenu.pageItems.Add(forceQuitButton);
            instantRestartButton = new PageItem("Instant\nRestart", () => { DestructiveActions.ForceRestart(); }, "Restarts the game, instantly.");
            baseMenu.pageItems.Add(instantRestartButton);

            AddMediaKeys();
        }
        private static GameObject BaseButton;
        private static Transform parentMenu;
        private static GameObject PrevButton;
        private static Button PrevButtonButton;
        private static GameObject PlayButton;
        private static Button PlayButtonButton;
        private static GameObject StopButton;
        private static Button StopButtonButton;
        private static GameObject NextButton;
        private static Button NextButtonButton;
        //Seperate method to more easily read or disable temporarily. Could also help with a cfg option.
        private static void AddMediaKeys() {
            BaseButton = QuickMenuUtils.GetQuickMenuInstance().transform.Find("ShortcutMenu/WorldsButton").gameObject;
            parentMenu = QuickMenuUtils.GetQuickMenuInstance().transform.Find(baseMenu.menuBase.getMenuName());
            PrevButton = GameObject.Instantiate(BaseButton, parentMenu, true);
            PrevButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(210f, 192f);
            PrevButton.GetComponentInChildren<Text>().text = "";
            PrevButton.GetComponent<Image>().sprite = Resources.Media_Nav;
            PrevButton.GetComponent<UiTooltip>().text = "Go to the next song in your Playlist";
            PrevButton.GetComponent<UiTooltip>().alternateText = "Go to the next song in your Playlist";
            PrevButton.transform.rotation *= Quaternion.Euler(0f, 0f, 180f);
            PrevButtonButton = PrevButton.GetComponent<Button>();
            PrevButtonButton.name = "emmVRC_PreviousSong";
            PrevButtonButton.onClick = new Button.ButtonClickedEvent();
            PrevButtonButton.onClick.AddListener(DelegateSupport.ConvertDelegate<UnityAction>(new Action(MediaControl.PrevTrack)));
            PlayButton = GameObject.Instantiate(BaseButton, parentMenu, true);
            PlayButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(630f, 192f);
            PlayButton.GetComponentInChildren<Text>().text = "";
            PlayButton.GetComponent<Image>().sprite = Resources.Media_PlayPause;
            PlayButton.GetComponent<UiTooltip>().text = "Pause or continue listening to the current song";
            PlayButton.GetComponent<UiTooltip>().alternateText = "Pause or continue listening to the current song";
            PlayButtonButton = PlayButton.GetComponent<Button>();
            PlayButtonButton.name = "emmVRC_PlayPause";
            PlayButtonButton.onClick = new Button.ButtonClickedEvent();
            PlayButtonButton.onClick.AddListener(DelegateSupport.ConvertDelegate<UnityAction>(new Action(MediaControl.PlayPause)));
            StopButton = GameObject.Instantiate(BaseButton, parentMenu, true);
            StopButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(1050f, 192f);
            StopButton.GetComponentInChildren<Text>().text = "";
            StopButton.GetComponent<Image>().sprite = Resources.Media_Stop;
            StopButton.GetComponent<UiTooltip>().text = "Stop the current song completely";
            StopButton.GetComponent<UiTooltip>().alternateText = "Stop the current song completely";
            StopButtonButton = StopButton.GetComponent<Button>();
            StopButtonButton.name = "emmVRC_StopSong";
            StopButtonButton.onClick = new Button.ButtonClickedEvent();
            StopButtonButton.onClick.AddListener(DelegateSupport.ConvertDelegate<UnityAction>(new Action(MediaControl.Stop)));
            NextButton = GameObject.Instantiate(BaseButton, parentMenu, true);
            NextButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(1470f, 192f);
            NextButton.GetComponentInChildren<Text>().text = "";
            NextButton.GetComponent<Image>().sprite = Resources.Media_Nav;
            NextButton.GetComponent<UiTooltip>().text = "Go to the previous song in your Playlist (click twice)\n or restart the current song";
            NextButton.GetComponent<UiTooltip>().alternateText = "Go to the previous song in your Playlist (click twice)\n or restart the current song";
            NextButtonButton = NextButton.GetComponent<Button>();
            NextButtonButton.name = "emmVRC_NextSong";
            NextButtonButton.onClick = new Button.ButtonClickedEvent();
            NextButtonButton.onClick.AddListener(DelegateSupport.ConvertDelegate<UnityAction>(new Action(MediaControl.NextTrack)));
        }
    }
}
