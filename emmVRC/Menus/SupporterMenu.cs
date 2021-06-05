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


namespace emmVRC.Menus
{
    public class Supporter
    {
        public string Name = "";
        public string Tooltip = "";
    }
    public class SupporterMenu
    {
        //public static PaginatedMenu baseMenu;
        public static ScrollingTextMenu baseMenu;
        public static void Initialize()
        {
            baseMenu = new ScrollingTextMenu(FunctionsMenu.baseMenu.menuBase, 768, 1024, "Supporters", "", null, "", "");
            //baseMenu = new PaginatedMenu(FunctionsMenu.baseMenu.menuBase, 768, 1024, "Supporters", "", null);
            //baseMenu.pageItems.Add(PageItem.Space);
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
            /*baseMenu.pageItems.Clear();
            baseMenu.pageTitles.Clear();
            baseMenu.pageItems.Add(PageItem.Space);
            baseMenu.pageTitles.Add("Fetching, please wait...");
            baseMenu.UpdateMenu();*/
            baseMenu.menuTitleText.text = "Fetching, please wait...";
            baseMenu.baseTextText.text = "";

            try
            {
                var result = await HTTPRequest.get("http://dl.emmvrc.com/supporters.json");
                List<Supporter> supporters = TinyJSON.Decoder.Decode(result).Make<List<Supporter>>();

                await emmVRC.AwaitUpdate.Yield();

                baseMenu.menuTitleText.text = "Special thanks to these supporters!";
                //baseMenu.pageItems.Clear();
                //baseMenu.pageTitles.Clear();
                foreach (Supporter supp in supporters)
                {
                    baseMenu.baseTextText.text += supp.Name.Replace("\n", " ") + "\n";
                    //baseMenu.pageItems.Add(new PageItem(supp.Name, null, supp.Tooltip));
                }
                /*for (int i = 0; i < (int)Math.Ceiling((double)baseMenu.pageItems.Count / 9); i++)
                {
                    baseMenu.pageTitles.Add("Special thanks to these supporters!");
                }*/
            }
            catch (Exception ex)
            {
                baseMenu.menuTitleText.text = "Network error...";
                baseMenu.baseTextText.text = "<color=#ff0000>" + ex.ToString() + "</color>";
                //baseMenu.pageTitles[0] = "Network error...";
                //baseMenu.UpdateMenu();
                
                await emmVRC.AwaitUpdate.Yield();
                
                throw ex;
            }
        }

    }
}
