using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using emmVRC.Libraries;
using UnityEngine;

namespace emmVRC.Hacks
{
    public class EmojiFavourites
    {
        public static QMNestedButton baseMenu;
        public static PaginatedMenu AvailableEmojiMenu;
        public static PaginatedMenu CurrentEmojiMenu;
        private static QMSingleButton availableEmojiMenuButton;
        private static QMSingleButton currentEmojiMenuButton;
        public static GameObject[] AvailableEmojis;
        public static IEnumerator Initialize()
        {
            baseMenu = new QMNestedButton("ShortcutMenu", 13213, 29481, "", "");
            baseMenu.getMainButton().DestroyMe();
            AvailableEmojiMenu = new PaginatedMenu(baseMenu, 1, 0, "", "", null);
            AvailableEmojiMenu.menuEntryButton.DestroyMe();
            CurrentEmojiMenu = new PaginatedMenu(baseMenu, 2, 0, "", "", null);
            CurrentEmojiMenu.menuEntryButton.DestroyMe();
            availableEmojiMenuButton = new QMSingleButton(baseMenu, 1, 0, "Add\nEmoji", OpenAvailableEmojiMenu, "Add an Emoji to your favorites");
            currentEmojiMenuButton = new QMSingleButton(baseMenu, 2, 0, "Remove\nEmoji", OpenDeleteFavouriteEmojiMenu, "Delete an Emoji from your favorites");
            while (VRCPlayer.field_Internal_Static_VRCPlayer_0 == null || VRCPlayer.field_Internal_Static_VRCPlayer_0.field_Private_MonoBehaviourPublicGaVoInStInVoStInVoStUnique_0 == null || VRCPlayer.field_Internal_Static_VRCPlayer_0.field_Private_MonoBehaviourPublicGaVoInStInVoStInVoStUnique_0.field_Public_ArrayOf_GameObject_0 == null)
                yield return new WaitForSeconds(1f);
            AvailableEmojis = VRCPlayer.field_Internal_Static_VRCPlayer_0.field_Private_MonoBehaviourPublicGaVoInStInVoStInVoStUnique_0.field_Public_ArrayOf_GameObject_0;

         }
        public static void OpenAvailableEmojiMenu()
        {
            AvailableEmojiMenu.pageItems.Clear();

            for (int i=0; i < AvailableEmojis.Length; i++)
            {
                if (!Configuration.JSONConfig.FavouritedEmojis.Contains(i))
                {
                    int emojiValue = i;
                    PageItem itm = new PageItem("", () => {
                        Configuration.JSONConfig.FavouritedEmojis.Add(emojiValue);
                        Configuration.SaveConfig();
                        QuickMenuUtils.ShowQuickmenuPage(baseMenu.getMenuName());
                    }, AvailableEmojis[i].name.Replace("Emoji", "").Replace("_", " "));
                    Texture2D text = AvailableEmojis[i].GetComponent<ParticleSystemRenderer>().material.mainTexture.Cast<Texture2D>();
                    itm.buttonSprite = Sprite.CreateSprite(text, new Rect(0.0f, 0.0f, text.width, text.height), new Vector2(0.5f, 0.5f), 25.0f, 0, 0, new Vector4(), false);
                    AvailableEmojiMenu.pageItems.Add(itm);
                    if (i == 30)
                        for (int j = 0; j < 5; j++)
                            AvailableEmojiMenu.pageItems.Add(PageItem.Space);
                    if (i == 38)
                        AvailableEmojiMenu.pageItems.Add(PageItem.Space);
                } else
                {
                    AvailableEmojiMenu.pageItems.Add(PageItem.Space);
                }
            }
            AvailableEmojiMenu.OpenMenu();
        }
        public static void OpenDeleteFavouriteEmojiMenu()
        {
            if (Configuration.JSONConfig.FavouritedEmojis.Count == 0) return;
            CurrentEmojiMenu.pageItems.Clear();
            foreach (int emojiValue in Configuration.JSONConfig.FavouritedEmojis)
            {
                int delEmojiValue = emojiValue;
                PageItem itm = new PageItem("", () =>
                {
                    Configuration.JSONConfig.FavouritedEmojis.Remove(delEmojiValue);
                    Configuration.SaveConfig();
                    QuickMenuUtils.ShowQuickmenuPage(baseMenu.getMenuName());
                }, AvailableEmojis[emojiValue].name.Replace("Emoji", "").Replace("_", " "));
                Texture2D text = AvailableEmojis[emojiValue].GetComponent<ParticleSystemRenderer>().material.mainTexture.Cast<Texture2D>();
                itm.buttonSprite = Sprite.CreateSprite(text, new Rect(0.0f, 0.0f, text.width, text.height), new Vector2(0.5f, 0.5f), 25.0f, 0, 0, new Vector4(), false);
                CurrentEmojiMenu.pageItems.Add(itm);
            }
            CurrentEmojiMenu.OpenMenu();
        }
    }
    
}
