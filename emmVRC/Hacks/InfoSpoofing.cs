using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using emmVRC.Libraries;

namespace emmVRC.Hacks
{
    public class NameSpoofGenerator
    {
        public static string spoofedName
        {
            get
            {
                if (Configuration.JSONConfig.InfoSpoofingName != "")
                    return Configuration.JSONConfig.InfoSpoofingName;
                else
                    return generatedSpoofName;
            }
        }
        private static string generatedSpoofName = "";
        private static List<string> adjectiveList = new List<string>()
        {
            "Adorable",
            "Adorbs",
            "Alluring",
            "Appealing",
            "Aromatic",
            "Beautiful",
            "Beauteous",
            "Bewitching",
            "Bonny",
            "Cute",
            "Charming",
            "Comely",
            "Darling",
            "Delightful",
            "Daring",
            "Dark",
            "Dreary",
            "Enchanting",
            "Engaging",
            "Exquisite",
            "Fair",
            "Fit",
            "Foxy",
            "Glamorous",
            "Gorgeous",
            "Humble",
            "Happy",
            "Heroic",
            "Harmonious",
            "Hot",
            "Hopeful",
            "Homely",
            "Hex",
            "Hypnotic",
            "Hyper",
            "Luscious",
            "Luxurious",
            "Magnificent",
            "Mistress",
            "Nice",
            "Personable",
            "Pleasing",
            "Pretty",
            "Picturesque",
            "Ravishing",
            "Scenic",
            "Seductive",
            "Sexy",
            "Sightly",
            "Smashing",
            "Splendid",
            "Stunning",
            "Sweet",
            "Tasty"
        };
        private static List<string> nounList = new List<string>()
        {
            "Angel",
            "Aurora",
            "Abelia",
            "Acer",
            "Allium",
            "Alpine",
            "Almond",
            "Abode",
            "Abyss",
            "Ace",
            "Aerie",
            "Alum",
            "Bamboo",
            "Bay",
            "Bella",
            "Bunny",
            "Blossom",
            "Blueberry",
            "Crystal",
            "Camellia",
            "Canna",
            "Carnation",
            "Diamond",
            "Daffodil",
            "Daylight",
            "Demon",
            "Demoness",
            "Emilia",
            "Emerald",
            "Elf",
            "Elderberry",
            "Eucalyptus",
            "Flower",
            "Fairy",
            "Feather",
            "Fox",
            "Garnet",
            "Gamer",
            "Grace",
            "Galaxy",
            "Gift",
            "Gianna",
            "Grape",
            "Hazel",
            "Heart",
            "Humility",
            "Hypatia",
            "Lavender",
            "Lush",
            "Lore",
            "Lapis",
            "Mana",
            "Miracle",
            "Moon",
            "Nora",
            "Nebula",
            "Night",
            "Platinum",
            "Port",
            "Princess",
            "Rhodium",
            "Ruby",
            "Succubus",
            "Seductress",
            "Savior",
            "Topaz",
            "Tera",
            "Tulip",
            "Tale",
            "Tail"
        };
        internal static void GenerateNewName()
        {
            System.Random r = new System.Random();
            int randomAdjectiveIndex = r.Next(adjectiveList.Count());
            string protoString = adjectiveList[randomAdjectiveIndex];
            while (protoString == adjectiveList[randomAdjectiveIndex])
            {
                int randomNounIndex = r.Next(nounList.Count());
                string protoString2 = protoString + nounList[randomNounIndex];
                if (protoString2.Length < 15)
                {
                    protoString = protoString2;
                }
            }
            generatedSpoofName = protoString;
        }
    }
    public class InfoSpoofing
    {
        private static bool Enabled = true;
        private static GameObject nameText;
        private static GameObject spoofedNameText;
        private static GameObject worldText;
        private static GameObject spoofedWorldText;
        private static GameObject avatarImage;
        private static GameObject spoofedAvatarImage;
        private static int spoofedWorldID = 294756;

        public static void Initialize()
        {
            nameText = QuickMenuUtils.GetQuickMenuInstance().transform.Find("ShortcutMenu/NameText").gameObject;
            worldText = QuickMenuUtils.GetQuickMenuInstance().transform.Find("ShortcutMenu/WorldText").gameObject;
            avatarImage = QuickMenuUtils.GetQuickMenuInstance().transform.Find("QuickMenu_NewElements/_CONTEXT/QM_Context_User_Hover/AvatarImage/Text").gameObject;
            if (nameText == null || worldText == null || avatarImage == null)
                emmVRCLoader.Logger.LogError("Info spoofing is going to fail, because one or more of the required elements was null!");
            spoofedNameText = GameObject.Instantiate(nameText, nameText.transform.parent);
            GameObject.Destroy(spoofedNameText.GetComponent<VRC.UI.UsernameDisplay>());
            spoofedWorldText = GameObject.Instantiate(worldText, worldText.transform.parent);
            GameObject.Destroy(spoofedWorldText.GetComponent<VRC.UI.DebugDisplayText>());
            spoofedAvatarImage = GameObject.Instantiate(avatarImage, avatarImage.transform.parent);

            NameSpoofGenerator.GenerateNewName();
            MelonLoader.MelonCoroutines.Start(Loop());
        }
        private static IEnumerator Loop()
        {
            while (Enabled)
            {
                yield return new WaitForSeconds(0.1f);
                try
                {
                    if (Configuration.JSONConfig.InfoHidingEnabled /*|| Configuration.JSONConfig.InfoSpoofingEnabled*/)
                    {
                        if (RoomManager.field_ApiWorld_0 != null)
                        {
                            nameText.SetActive(false);
                            worldText.SetActive(false);
                            avatarImage.SetActive(false);
                        }
                    }
                    if (Configuration.JSONConfig.InfoSpoofingEnabled)
                    {
                        if (RoomManager.field_ApiWorld_0 != null)
                        {
                            spoofedNameText.SetActive(true);
                            spoofedWorldText.SetActive(true);
                            spoofedAvatarImage.SetActive(true);
                            spoofedNameText.GetComponent<Text>().text = "Hi, " + NameSpoofGenerator.spoofedName;
                            spoofedAvatarImage.GetComponentInChildren<Text>().text = NameSpoofGenerator.spoofedName;

                            spoofedWorldText.GetComponent<Text>().text = "Text";// RoomManager.field_ApiWorldInstance_0.instanceWorld.name + ":" + spoofedWorldID.ToString() + "invite";

                            if (QuickMenuUtils.GetVRCUiMInstance().menuContent.activeInHierarchy)
                            {
                                foreach (Text text in QuickMenuUtils.GetVRCUiMInstance().menuContent.GetComponentsInChildren<Text>())
                                {
                                    if (text.text.Contains((VRC.Core.APIUser.CurrentUser.displayName == "" ? VRC.Core.APIUser.CurrentUser.username : VRC.Core.APIUser.CurrentUser.displayName)))
                                        text.text = text.text.Replace((VRC.Core.APIUser.CurrentUser.displayName == "" ? VRC.Core.APIUser.CurrentUser.username : VRC.Core.APIUser.CurrentUser.displayName), NameSpoofGenerator.spoofedName);
                                }
                            }
                        }
                    }
                    if (!Configuration.JSONConfig.InfoSpoofingEnabled)
                    {
                        if (QuickMenuUtils.GetVRCUiMInstance().menuContent.activeInHierarchy && RoomManager.field_ApiWorld_0 != null)
                        {
                            foreach (Text text in QuickMenuUtils.GetVRCUiMInstance().menuContent.GetComponentsInChildren<Text>())
                            {
                                text.text = text.text.Replace(NameSpoofGenerator.spoofedName, (VRC.Core.APIUser.CurrentUser.displayName == "" ? VRC.Core.APIUser.CurrentUser.username : VRC.Core.APIUser.CurrentUser.displayName));
                            }
                        }
                    }
                    if (!Configuration.JSONConfig.InfoSpoofingEnabled && !Configuration.JSONConfig.InfoHidingEnabled)
                    {
                        if (RoomManager.field_ApiWorld_0 != null)
                        {
                            spoofedNameText.SetActive(false);
                            spoofedWorldText.SetActive(false);
                            spoofedAvatarImage.SetActive(false);
                            nameText.SetActive(true);
                            worldText.SetActive(true);
                            avatarImage.SetActive(true);
                        }
                    }
                }
                catch (Exception ex)
                {
                    emmVRCLoader.Logger.LogError("Spoofer error: " + ex.ToString());
                }
            }
        }
    }
}
