using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnhollowerRuntimeLib;
using UnhollowerRuntimeLib.XrefScans;
using UnityEngine;

namespace emmVRC.Libraries
{
    public static class CustomActionMenu
    {
        private static List<Page> customPages = new List<Page>();
        private static List<Button> mainMenuButtons = new List<Button>();
        private static bool Initialized = false;
        public static ActionMenu activeActionMenu;

        public static Texture2D ToggleOffTexture;
        public static Texture2D ToggleOnTexture;

        public static bool isOpen(this ActionMenuOpener actionMenuOpener)
        {
            return actionMenuOpener.field_Private_Boolean_0;
        }
        private static ActionMenuOpener GetActionMenuOpener()
        {
            if (!ActionMenuDriver.field_Public_Static_ActionMenuDriver_0.field_Public_ActionMenuOpener_0.isOpen() && ActionMenuDriver.field_Public_Static_ActionMenuDriver_0.field_Public_ActionMenuOpener_1.isOpen())
            {
                return ActionMenuDriver.field_Public_Static_ActionMenuDriver_0.field_Public_ActionMenuOpener_1;
            }
            else if (ActionMenuDriver.field_Public_Static_ActionMenuDriver_0.field_Public_ActionMenuOpener_0.isOpen() && !ActionMenuDriver.field_Public_Static_ActionMenuDriver_0.field_Public_ActionMenuOpener_1.isOpen())
            {
                return ActionMenuDriver.field_Public_Static_ActionMenuDriver_0.field_Public_ActionMenuOpener_0;
            }
            else return null;
            /*
            else if (ActionMenuDriver._instance.openerL.isOpen() && ActionMenuDriver._instance.openerR.isOpen())
            {
                return null; //Which one to return ¯\_(ツ)_/¯ Mystery till I figure something smart out
            }
            */


        }

        private static void Initialize()
        {
            Harmony.HarmonyInstance inst = Harmony.HarmonyInstance.Create("customActionMenuHarmony");
            inst.Patch(typeof(ActionMenu).GetMethods().FirstOrDefault(it => UnhollowerRuntimeLib.XrefScans.XrefScanner.XrefScan(it).Any(jt => jt.Type == UnhollowerRuntimeLib.XrefScans.XrefType.Global && jt.ReadAsObject()?.ToString() == "Emojis")), null, new Harmony.HarmonyMethod(typeof(CustomActionMenu).GetMethod("OpenMainPage", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)));
            ToggleOffTexture = UnityEngine.Resources.Load<Texture2D>("GUI_Toggle_OFF");
            ToggleOnTexture = UnityEngine.Resources.Load<Texture2D>("GUI_Toggle_ON");
            Initialized = true;
        }
        private static void OpenMainPage(ActionMenu __instance)
        {
            activeActionMenu = __instance;
            if (Configuration.JSONConfig.ActionMenuIntegration)
            {
                foreach (Button btn in mainMenuButtons)
                {
                    var newButton = activeActionMenu.Method_Private_PedalOption_0();
                    newButton.prop_String_0 = btn.ButtonText;
                    newButton.field_Public_MulticastDelegateNPublicSealedBoUnique_0 = DelegateSupport.ConvertDelegate<PedalOption.MulticastDelegateNPublicSealedBoUnique>(btn.ButtonAction);
                    if (btn.ButtonIcon != null)
                        newButton.prop_Texture2D_0 = btn.ButtonIcon;
                    btn.currentPedalOption = newButton;
                }
            }
        }

        public enum BaseMenu
        {
            MainMenu = 1
        }
        public class Page
        {
            public List<Button> buttons = new List<Button>();
            public Page previousPage;
            public Button menuEntryButton;
            public Page(BaseMenu baseMenu, string buttonText, Texture2D buttonIcon = null)
            {
                if (!Initialized)
                    Initialize();
                if (baseMenu == BaseMenu.MainMenu)
                {
                    menuEntryButton = new Button(BaseMenu.MainMenu, buttonText, delegate { OpenMenu(null); }, buttonIcon);
                }
            }
            public Page(Page basePage, string buttonText, Texture2D buttonIcon = null)
            {
                if (!Initialized)
                    Initialize();
                previousPage = basePage;
                menuEntryButton = new Button(previousPage, buttonText, delegate { OpenMenu(basePage); }, buttonIcon);
            }
            
            public void OpenMenu(Page currentPage)
            {
                if (!Initialized)
                    Initialize();
                GetActionMenuOpener().field_Public_ActionMenu_0.Method_Public_ObjectNPublicAcTeAcStGaUnique_Action_Action_Texture2D_String_0((new Action(delegate
                {
                    foreach(Button btn in buttons)
                    {
                        var newButton = GetActionMenuOpener().field_Public_ActionMenu_0.Method_Private_PedalOption_0();
                        newButton.prop_String_0 = btn.ButtonText; // Button Text
                        newButton.field_Public_MulticastDelegateNPublicSealedBoUnique_0 = DelegateSupport.ConvertDelegate<PedalOption.MulticastDelegateNPublicSealedBoUnique>(btn.ButtonAction);
                        newButton.prop_Boolean_0 = btn.IsEnabled;
                        if (btn.ButtonIcon != null)
                            newButton.prop_Texture2D_0 = btn.ButtonIcon; // Button Texture
                        btn.currentPedalOption = newButton;
                    }
                })), null);
            }
        }
        public class Button
        {
            public string ButtonText;
            public bool IsEnabled;
            public Action ButtonAction;
            public Texture2D ButtonIcon;
            public PedalOption currentPedalOption;
            public Button(BaseMenu baseMenu, string buttonText, System.Action buttonAction, Texture2D buttonIcon = null)
            {
                if (!Initialized)
                    Initialize();
                this.ButtonText = buttonText;
                this.ButtonAction = buttonAction;
                this.ButtonIcon = buttonIcon;
                if (baseMenu == BaseMenu.MainMenu)
                {
                    mainMenuButtons.Add(this);
                }
            }
            public Button(Page basePage, string buttonText, System.Action buttonAction, Texture2D buttonIcon = null)
            {
                if (!Initialized)
                    Initialize();
                this.ButtonText = buttonText;
                this.ButtonAction = buttonAction;
                this.ButtonIcon = buttonIcon;
                basePage.buttons.Add(this);
            }
            public void SetButtonText(string newText)
            {
                this.ButtonText = newText;
                if (currentPedalOption != null)
                    currentPedalOption.prop_String_0 = newText;
            }
            public void SetIcon(Texture2D newTexture)
            {
                this.ButtonIcon = newTexture;
                if (currentPedalOption != null)
                    currentPedalOption.prop_Texture2D_0 = newTexture;
            }
            public void SetEnabled(bool enabled)
            {
                IsEnabled = enabled;
                if (currentPedalOption != null)
                    currentPedalOption.prop_Boolean_0 = !enabled;
            }
        }
    }
}
