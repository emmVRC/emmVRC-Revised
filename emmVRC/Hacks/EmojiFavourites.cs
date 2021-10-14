using System;
using Il2CppSystem;
using UnhollowerBaseLib;
using UnhollowerRuntimeLib;
using Il2CppSystem.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using emmVRC.Libraries;
using UnityEngine;
using emmVRC.Objects.ModuleBases;

namespace emmVRC.Hacks
{
    public class EmojiFavourites : MelonLoaderEvents
    {
        public static QMNestedButton baseMenu;
        public static PaginatedMenu AvailableEmojiMenu;
        public static PaginatedMenu CurrentEmojiMenu;
        private static QMSingleButton availableEmojiMenuButton;
        private static QMSingleButton currentEmojiMenuButton;
        public static List<GameObject> AvailableEmojis;
        public override void OnUiManagerInit()
        {
            if (Functions.Other.BuildNumber.buildNumber > 1134) return;
            MelonLoader.MelonCoroutines.Start(Initialize());
        }
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
            while (VRCPlayer.field_Internal_Static_VRCPlayer_0 == null)
                yield return new WaitForSeconds(1f);
            AvailableEmojis = emojiObjects();
        }

        private static Il2CppSystem.Reflection.FieldInfo emojiGenField = null;
        private static Il2CppSystem.Reflection.FieldInfo emojisField = null;
        public static List<GameObject> emojiObjects()
        {
            var vrcPlayerFields = Il2CppType.Of<VRCPlayer>().GetFields(Il2CppSystem.Reflection.BindingFlags.Instance | Il2CppSystem.Reflection.BindingFlags.Public | Il2CppSystem.Reflection.BindingFlags.NonPublic);
            if (emojiGenField == null || emojisField == null)
            { 
                foreach (var f in vrcPlayerFields)
                {
                    var il2CppFields = f.FieldType.GetFields(Il2CppSystem.Reflection.BindingFlags.Instance | Il2CppSystem.Reflection.BindingFlags.Public | Il2CppSystem.Reflection.BindingFlags.NonPublic);
                    if (il2CppFields.Length > 1) // EmojiGenerator only has 1 field
                        continue;

                    foreach (var sf in il2CppFields)
                    {
                        if (sf.FieldType.IsArray && sf.FieldType.GetElementType() == Il2CppType.Of<GameObject>())
                        {
                            emojiGenField = f;
                            emojisField = sf;
                            break;
                        }
                    }
                } }
            var emojiGen = emojiGenField.GetValue(VRCPlayer.field_Internal_Static_VRCPlayer_0);
            return emojisField.GetValue(emojiGen).Cast<Il2CppReferenceArray<GameObject>>().ToList();
        }

        public static void OpenAvailableEmojiMenu()
        {
            if (Configuration.JSONConfig.FavouritedEmojis.Count >= 8)
            {
                VRCUiPopupManager.prop_VRCUiPopupManager_0.ShowAlert("emmVRC", "You have reached the maximum amount of Emoji Favorites.", 0f);
            }
            else
            {
                AvailableEmojiMenu.pageItems.Clear();

                for (int i = 0; i < AvailableEmojis.Count; i++)
                {
                    if (!Configuration.JSONConfig.FavouritedEmojis.Contains(i))
                    {
                        int emojiValue = i;
                        PageItem itm = new PageItem("", () =>
                        {
                            List<int> favouritedEmojis = new List<int>();
                            favouritedEmojis.AddRange(Configuration.JSONConfig.FavouritedEmojis.ToArray());
                            favouritedEmojis.Add(emojiValue);
                            Configuration.WriteConfigOption("FavouritedEmojis", favouritedEmojis);
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
                    }
                    else
                    {
                        AvailableEmojiMenu.pageItems.Add(PageItem.Space);
                    }
                }
                AvailableEmojiMenu.OpenMenu();
            }
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
                    List<int> favouritedEmojis = new List<int>();
                    favouritedEmojis.AddRange(Configuration.JSONConfig.FavouritedEmojis.ToArray());
                    favouritedEmojis.Remove(emojiValue);
                    Configuration.WriteConfigOption("FavouritedEmojis", favouritedEmojis);
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
