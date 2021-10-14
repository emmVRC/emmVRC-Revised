using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using emmVRC.Libraries;
using emmVRC.Network;
using UnityEngine;
using UnityEngine.Networking;
using emmVRC.Objects.ModuleBases;


namespace emmVRC.Menus
{
    public class Supporter
    {
        public string Name = "";
        public string Tooltip = "";
    }
    public class SupporterMenu : MelonLoaderEvents
    {
        public static ScrollingTextMenu baseMenu;
        public override void OnUiManagerInit()
        {
            baseMenu = new ScrollingTextMenu(FunctionsMenuLegacy.baseMenu.menuBase, 768, 1024, "Supporters", "", null, "", "");
            baseMenu.menuEntryButton.DestroyMe();
        }
        public static void LoadMenu()
        {
            try
            {
                baseMenu.OpenMenu();
                EnterMenu().NoAwait(nameof(SupporterMenu));
            } catch (Exception ex)
            {
                emmVRCLoader.Logger.LogError(ex.ToString());
            }
        }
        public static async Task EnterMenu()
        {
            baseMenu.OpenMenu();
            baseMenu.menuTitleText.text = "Fetching, please wait...";
            baseMenu.baseTextText.text = "";

            try
            {
                var result = await HTTPRequest.get("http://dl.emmvrc.com/supporters.json");
                List<Supporter> supporters = TinyJSON.Decoder.Decode(result).Make<List<Supporter>>();

                await emmVRC.AwaitUpdate.Yield();

                baseMenu.menuTitleText.text = "Special thanks to these supporters!";
                foreach (Supporter supp in supporters)
                {
                    baseMenu.baseTextText.text += supp.Name.Replace("\n", " ") + "\n";
                }
            }
            catch (Exception ex)
            {
                baseMenu.menuTitleText.text = "Network error...";
                baseMenu.baseTextText.text = "<color=#ff0000>" + ex.ToString() + "</color>";
                
                await emmVRC.AwaitUpdate.Yield();
                
                throw ex;
            }
        }

    }
}
