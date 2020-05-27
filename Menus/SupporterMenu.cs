using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using emmVRC.Libraries;
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
        public static PaginatedMenu baseMenu;
        public static void Initialize()
        {
            baseMenu = new PaginatedMenu(FunctionsMenu.baseMenu.menuBase, 768, 1024, "Supporters", "", null);
            baseMenu.pageItems.Add(PageItem.Space());
            baseMenu.menuEntryButton.DestroyMe();
        }
        public static void LoadMenu()
        {
            try
            {
                baseMenu.OpenMenu();
                MelonLoader.MelonCoroutines.Start(EnterMenu());
            } catch (Exception ex)
            {
                emmVRCLoader.Logger.LogError(ex.ToString());
            }
        }
        public static IEnumerator EnterMenu()
        {
            baseMenu.OpenMenu();
            baseMenu.pageItems.Clear();
            baseMenu.pageTitles.Clear();
            baseMenu.pageItems.Add(PageItem.Space());
            baseMenu.pageTitles.Add("Fetching, please wait...");
            baseMenu.UpdateMenu();
            UnityWebRequest req = UnityWebRequest.Get("http://www.thetrueyoshifan.com/supporters.json");
            req.SendWebRequest();
            while (!req.isDone)
                yield return new WaitForEndOfFrame();

            if (req.responseCode != 200)
            {
                baseMenu.pageTitles[0] = "Network error...";
                baseMenu.UpdateMenu();
                emmVRCLoader.Logger.LogError("Network error occured while grabbing supporters: " + req.error);
            }
            else
            {
                baseMenu.pageItems.Clear();
                baseMenu.pageTitles.Clear();
                string result = Il2CppSystem.Text.Encoding.UTF8.GetString(req.downloadHandler.data);
                List<Supporter> supporters = TinyJSON.Decoder.Decode(result).Make<List<Supporter>>();
                foreach (Supporter supp in supporters)
                {
                    baseMenu.pageItems.Add(new PageItem(supp.Name, null, supp.Tooltip));
                }
                for (int i = 0; i < (int)Math.Ceiling((double)baseMenu.pageItems.Count / 9); i++){
                    baseMenu.pageTitles.Add("Special thanks to these supporters!");
                }
                baseMenu.UpdateMenu();
            }
        }

    }
}
