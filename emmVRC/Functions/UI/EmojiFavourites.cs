using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using emmVRC.Utils;
using emmVRC.Objects.ModuleBases;
using TMPro;

namespace emmVRC.Functions.UI
{
    [Priority(55)]
    public class EmojiFavourites : MelonLoaderEvents, IWithLateUpdate
    {
        private static MenuPage emojiFavouritesPage;
        private static List<ButtonGroup> emojiGroups;
        private static List<EmojiFavouriteButton> emojiButtons;
        public static List<Texture2D> emojiTextures;
        private static bool _initialized;

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            if (buildIndex != -1 || _initialized) return;

            emojiFavouritesPage = new MenuPage("emmVRC_FavouriteEmojis", "Favorite Emojis", false, true, true, () => { ButtonAPI.GetQuickMenuInstance().ShowCustomDialog("Emoji Favorites", "Are you sure you want to clear your Emoji Favorites?", "", "Yes", "No", null, () => { Configuration.WriteConfigOption("FavouritedEmojis", new List<int>()); OpenMenu(); }, null); }, "Select to clear your Emoji Favorites", ButtonAPI.xIconSprite);

            emojiFavouritesPage.menuContents.GetComponent<UnityEngine.UI.VerticalLayoutGroup>().childControlHeight = true;
            emojiGroups = new List<ButtonGroup>();
            emojiButtons = new List<EmojiFavouriteButton>();

            _initialized = true;
        }
        public void LateUpdate()
        {
            if (emojiTextures != null || !_initialized) return;
            if (VRC.UI.EmojiManager.prop_EmojiManager_0 != null && VRC.UI.EmojiManager.prop_EmojiManager_0.field_Private_List_1_EmojiData_0 != null && VRC.UI.EmojiManager.prop_EmojiManager_0.field_Private_List_1_EmojiData_0.Count > 0)
            {
                emojiTextures = new List<Texture2D>();
                foreach (VRC.UI.Client.Emoji.EmojiData emojiData in VRC.UI.EmojiManager.prop_EmojiManager_0.field_Private_List_1_EmojiData_0)
                {
                    var baseImage = emojiData.Image;

                    Color[] c = baseImage.texture.CloneReadable().GetPixels((int)baseImage.textureRect.x, (int)baseImage.textureRect.y, (int)baseImage.textureRect.width, (int)baseImage.textureRect.height);
                    var slicedText = new Texture2D((int)baseImage.textureRect.width, (int)baseImage.textureRect.height);
                    slicedText.hideFlags |= HideFlags.DontUnloadUnusedAsset;
                    slicedText.SetPixels(c);
                    slicedText.Apply();
                    emojiTextures.Add(slicedText);
                }

            }
        }
        public static void OpenMenu()
        {
            foreach (EmojiFavouriteButton button in emojiButtons)
                GameObject.Destroy(button.emojiFavouriteButtonBase.gameObject);
            foreach (ButtonGroup group in emojiGroups)
            {
                group.Destroy();
            }
            emojiButtons = new List<EmojiFavouriteButton>();
            emojiGroups = new List<ButtonGroup>();
            VRC.UI.EmojiManager man = VRC.UI.EmojiManager.prop_EmojiManager_0;
            for (int i=0; i < man.field_Public_EmojiCategoryList_0.Categories.Count; i++)
            {
                var currentGroup = new ButtonGroup(emojiFavouritesPage, man.field_Public_EmojiCategoryList_0.Categories[i].Name);
                
                for (int j=0; j < man.field_Public_EmojiCategoryList_0.Categories[i].Data.Count; j++)
                {
                    emojiButtons.Add(new EmojiFavouriteButton(currentGroup, man.field_Public_EmojiCategoryList_0.Categories[i].Data[j].Name, man.field_Private_List_1_EmojiData_0.IndexOf(man.field_Public_EmojiCategoryList_0.Categories[i].Data[j]), man.field_Public_EmojiCategoryList_0.Categories[i].Data[j].Image));
                }
                emojiGroups.Add(currentGroup);
            }
            emojiFavouritesPage.OpenMenu();
        }
        public static void AddRemoveEmojiFavourite(int emojiId)
        {
            List<int> currentFavourites = Configuration.JSONConfig.FavouritedEmojis;
            if (currentFavourites.Contains(emojiId))
                currentFavourites.Remove(emojiId);
            else
            {
                if (currentFavourites.Count < 8)
                    currentFavourites.Add(emojiId);
                else
                    ButtonAPI.GetQuickMenuInstance().ShowAlert("You cannot add more emojis to your favorites.");
            }
            Configuration.WriteConfigOption("FavouritedEmojis", currentFavourites);
        }
        
    }
    internal class EmojiFavouriteButton
    {
        public readonly SingleButton emojiFavouriteButtonBase;
        private readonly int emojiId;
        public EmojiFavouriteButton(ButtonGroup group, string name, int id, Sprite sprite)
        {
            emojiId = id;
            emojiFavouriteButtonBase = new SingleButton(group, name, () => { EmojiFavourites.AddRemoveEmojiFavourite(emojiId); Functions.UI.EmojiFavourites.OpenMenu(); }, name, sprite);
                emojiFavouriteButtonBase.SetBadgeIcon(ButtonAPI.onIconSprite);
            emojiFavouriteButtonBase.SetBadgeEnabled(Configuration.JSONConfig.FavouritedEmojis.Contains(emojiId));
            emojiFavouriteButtonBase.SetIcon(sprite, true);
        }
    }
}
